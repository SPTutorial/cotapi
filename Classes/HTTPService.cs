using ElementTracker.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ElementTracker.Services
{
    public class HTTPService
    {
        private readonly JsonSerializerSettings serializerSettings;
        private readonly JsonSerializerSettings requestSerializerSettings;
        private readonly string CommonErrorMessage = "We having some trouble completing your request at the moment. Please try again shortly, and if it persists let us know.";
        public HTTPService()
        {
            serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };
            //requestSerializerSettings = new JsonSerializerSettings
            //{
            //    ContractResolver = new LowercaseContractResolver(),
            //    Formatting = Formatting.Indented
            //};
            serializerSettings.Converters.Add(new StringEnumConverter());
        }
        public async Task<TResult> GetAsync<TResult>(string uri, bool forcedRefresh = false)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return default(TResult);
            }
            try
            {
                Console.WriteLine($"Get API URI: {uri}");
                var httpClient = HttpClientCreator(uri);
                var response = await httpClient.GetAsync(uri);
                await HandleResponse(response);
                var serialized = await response.Content.ReadAsStringAsync();
                var result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized));
                return result;
            }
            catch (Exception ex)
            {
                GlobalFunctions.ShowToast(CommonErrorMessage);
                GlobalFunctions.SendExceptionReport(ex);
                return default(TResult);
            }
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data)
        {
            var httpClient = HttpClientCreator(uri);
            var serialized = await Task.Run(() => JsonConvert.SerializeObject(data));

            try
            {
                var content = new StringContent(serialized, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, content);

                await HandleResponse(response);
                var responseData = await response.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, serializerSettings));
            }
            catch (Exception ex)
            {
                GlobalFunctions.ShowToast(CommonErrorMessage);
                GlobalFunctions.SendExceptionReport(ex);
                return default(TResult);
            }
        }
        public async Task<TResult> PostAsync<TResult>(string uri)
        {
            var httpClient = HttpClientCreator(uri);
            try
            {
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, content);

                await HandleResponse(response);
                var responseData = await response.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, serializerSettings));
            }
            catch (Exception ex)
            {
                GlobalFunctions.ShowToast(CommonErrorMessage);
                GlobalFunctions.SendExceptionReport(ex);
                return default(TResult);
            }
        }
        public async Task<TResult> DeleteAsync<TResult>(string uri)
        {
            var httpClient = HttpClientCreator(uri);
            try
            {
                var request = new HttpRequestMessage(new HttpMethod("DELETE"), uri);
                var response = await httpClient.SendAsync(request);
                await HandleResponse(response);
                var responseData = await response.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, serializerSettings));
            }
            catch (Exception ex)
            {
                GlobalFunctions.ShowToast(CommonErrorMessage);
                GlobalFunctions.SendExceptionReport(ex);
                return default(TResult);
            }
        }
        public async Task<TResult> DeleteAsync<TRequest, TResult>(string uri, TRequest data)
        {
            var httpClient = HttpClientCreator(uri);
            var serialized = await Task.Run(() => JsonConvert.SerializeObject(data));
            try
            {
                var request = new HttpRequestMessage(new HttpMethod("DELETE"), uri);
                request.Content = new StringContent(serialized, Encoding.UTF8, "application/json");
                var response = await httpClient.SendAsync(request);
                await HandleResponse(response);
                var responseData = await response.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, serializerSettings));
            }
            catch (Exception ex)
            {
                GlobalFunctions.ShowToast(CommonErrorMessage);
                GlobalFunctions.SendExceptionReport(ex);
                return default(TResult);
            }
        }

        public async Task<UploadImageResponse> UploadImages(string uri,byte[] imagebyte)
        {
            UploadImageResponse response = new UploadImageResponse();
            using (var httpClient = HttpClientCreator(uri))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), uri))
                {
                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(new ByteArrayContent(imagebyte), "image[]", "image.jpg");
                    //multipartContent.Add(new ByteArrayContent(null), "cropimg[]", Path.GetFileName(""));
                   // multipartContent.Add(new StringContent("test"), "path[]");
                   // multipartContent.Add(new StringContent(""), "imagename[]");


                    //multipartContent.Add(new ByteArrayContent(File.ReadAllBytes("/B:/xampp/htdocs/userveup/assect/images/product/normal/2021/03/20/60565b9c919d3.jpg")), "image[]", Path.GetFileName("/B:/xampp/htdocs/userveup/assect/images/product/normal/2021/03/20/60565b9c919d3.jpg"));
                    //multipartContent.Add(new ByteArrayContent(File.ReadAllBytes("/path/to/file")), "cropimg[]", Path.GetFileName("/path/to/file"));
                    //multipartContent.Add(new StringContent("\"test\""), "path[]");
                    //multipartContent.Add(new StringContent("\"2021/06/03/60b8a6f8a94b6.jpg\""), "image[]");
                    request.Content = multipartContent;

                    var resp = await httpClient.SendAsync(request);
                    await HandleResponse(resp);
                    var responseData = await resp.Content.ReadAsStringAsync();
                    response =  await Task.Run(() => JsonConvert.DeserializeObject<UploadImageResponse>(responseData, serializerSettings));
                }
            }
            return response;
        }
        async Task HandleResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    GlobalFunctions.SendExceptionReport(new Exception(content));
                    GlobalFunctions.ShowToast(content);
                }

                GlobalFunctions.SendExceptionReport(new Exception(content));
                GlobalFunctions.ShowToast(content);
            }
            else
            {
                //convert response to base response object
                var responseObj = await Task.Run(() => JsonConvert.DeserializeObject(content, serializerSettings));
                if (responseObj != null)
                {
                    await Device.InvokeOnMainThreadAsync(() =>
                    {
                        //await _dialogService.ShowAlertAsync(TextsTranslateManager.Translate("SessionExpired"), TextsTranslateManager.Translate("SystemWarning"), "OK");
                        //await _navigationService.NavigateAsync(nameof(LoginPage));
                    });
                }
            }
        }
        private HttpClient HttpClientCreator(string url)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(45);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                httpClient.BaseAddress = new Uri(Constants.BaseUrl);

                //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                //{
                //    NoCache = true
                //};
                return httpClient;
            }
            catch (Exception ex)
            {
                GlobalFunctions.SendExceptionReport(ex);
                return null;
            }
        }              
    }
}
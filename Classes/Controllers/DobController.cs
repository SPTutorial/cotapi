using COT.API.BLL;
using COT.API.Models;
using COT.API.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COT.API.Logger;
using System.Xml;
using COT.DAL;
using System.Data;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace COT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DobController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _config;
        public DobController(ILogger<DobController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
       
        [HttpPost("MatchDOBJSON")]
        public MatchDobResponse MatchDOBJSON(List<DobMatchRequest> requestModel)
        {
            MatchDobResponse dobResponse = new MatchDobResponse();
            try
            {
                if (requestModel != null)
                {
                    foreach (var item in requestModel)
                    {
                        Tuple<bool, string> validatePinresp = CommonUtils.ValidatePin(item.pin.ToString()); // validate pin
                        DobResponse response = new DobResponse();

                        response.pin = item.pin;
                        response.dob = item.dob;

                        if (validatePinresp.Item1)
                        {
                            // query from db based on pin     
                            List<PersonDobEntity> personDOBs = GetDOBListByPin(item.pin);
                            foreach (var personDOB in personDOBs)
                            {
                                response = new DobResponse();
                                response.pin = item.pin;
                                response.dob = item.dob;

                                response.tabledob = personDOB.dob;
                                Tuple<int, string, bool> matchResponse = new DobSvc().MatchDOB(item.dob, personDOB.dob);
                                if (matchResponse.Item3)
                                {
                                    response.matchlevel = matchResponse.Item1.ToString();
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(matchResponse.Item2))
                                    {
                                        response.matchlevel = matchResponse.Item1.ToString();
                                    }
                                    else
                                    {
                                        response.matchlevel = matchResponse.Item2;
                                    }
                                }

                                dobResponse.MatchResults.Add(response);
                            }

                            if (personDOBs.Count > 0)
                                continue;
                        }
                        else
                        {
                            response.matchlevel = validatePinresp.Item2;
                        }

                        dobResponse.MatchResults.Add(response);
                    }
                }
                else
                {
                    _logger?.LogInformation("Invalid input!");
                    dobResponse.Message = "Invalid input!";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogInformation(ex.Message);
                dobResponse.Message = ex.Message;
            }
            return dobResponse;
        }

        [HttpPost("MatchDOBXML")]
        public MatchDobResponse MatchDOBXML(XmlDocument requestXml)
        {
            DobMatchRequestList requestModel = Utils.XmlConvert.Deserialize<DobMatchRequestList>(requestXml.OuterXml);
            MatchDobResponse dobResponse = new MatchDobResponse();
            try
            {
                if (requestModel != null)
                {
                    foreach (var item in requestModel.Inputdata)
                    {
                        Tuple<bool, string> validatePinresp = CommonUtils.ValidatePin(item.pin.ToString()); // validate pin
                        DobResponse response = new DobResponse();

                        response.pin = item.pin;
                        response.dob = item.dob;

                        if (validatePinresp.Item1)
                        {
                            // query from db based on pin     
                            List<PersonDobEntity> personDOBs = GetDOBListByPin(item.pin);
                            foreach (var personDOB in personDOBs)
                            {
                                response = new DobResponse();
                                response.pin = item.pin;
                                response.dob = item.dob;

                                response.tabledob = personDOB.dob;
                                Tuple<int, string, bool> matchResponse = new DobSvc().MatchDOB(item.dob, personDOB.dob);
                                if (matchResponse.Item3)
                                {
                                    response.matchlevel = matchResponse.Item1.ToString();
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(matchResponse.Item2))
                                    {
                                        response.matchlevel = matchResponse.Item1.ToString();
                                    }
                                    else
                                    {
                                        response.matchlevel = matchResponse.Item2;
                                    }
                                }

                                dobResponse.MatchResults.Add(response);
                            }

                            if (personDOBs.Count > 0)
                                continue;
                        }
                        else
                        {
                            response.matchlevel = validatePinresp.Item2;
                        }

                        dobResponse.MatchResults.Add(response);
                    }
                }
                else
                {
                    _logger?.LogInformation("Invalid input!");
                    dobResponse.Message = "Invalid input!";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogInformation(ex.Message);
                dobResponse.Message = ex.Message;
            }
            return dobResponse;
        }

        [NonAction]
        public List<PersonDobEntity> GetDOBListByPin(int pin)
        {
            List<PersonDobEntity> personDobs = new List<PersonDobEntity>();
            try
            {
                MySQLHelper sqlHelper = new MySQLHelper(_config?.GetConnectionString("mySqlConnection"));

                MySqlParameter[] sqlpr = new MySqlParameter[1];
                sqlpr[0] = new MySqlParameter("pin_no", pin);
                DataSet ds = sqlHelper.ExecuteDataset(System.Data.CommandType.StoredProcedure, "sp_GetPersonDOBListByPin", sqlpr);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        PersonDobEntity pde = new PersonDobEntity();
                        pde.id = CommonUtils.ToInt(dr["id"]);
                        pde.pin = CommonUtils.ToInt(dr["pin"]);
                        pde.dob = Convert.ToString(dr["dob"]);

                        personDobs.Add(pde);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            return personDobs;
        }
    }
}
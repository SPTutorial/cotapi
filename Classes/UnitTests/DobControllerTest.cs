using COT.API.Controllers;
using COT.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace COT.UnitTest
{
    [TestFixture]
    class DOBControllerTest
    {
        private IConfiguration _config;

        public DOBControllerTest()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(Configuration);            
        }
        public IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }


        [Test]
        public void DOBController_MatchDOB_Result9()
        {
            DobController dobController = new DobController(null, _config);
            List<DobMatchRequest> requestModel = new List<DobMatchRequest>();
            requestModel.Add(new DobMatchRequest { pin = 1, dob = "19930707" });
            
            MatchDobResponse dobMatchResult = dobController.MatchDOBJSON(requestModel);
            Assert.AreEqual("9", dobMatchResult.MatchResults[0].matchlevel);
        }

        [Test]
        public void DOBController_MatchDOB_ResultFailed()
        {
            DobController dobController = new DobController(null, _config);
            List<DobMatchRequest> requestModel = new List<DobMatchRequest>();
            requestModel.Add(new DobMatchRequest { pin = 1, dob = "19930707s" });

            MatchDobResponse dobMatchResult = dobController.MatchDOBJSON(requestModel);
            Assert.AreNotEqual("9", dobMatchResult.MatchResults[0].matchlevel);
        }
        [Test]
        public void DOBController_MatchDOB_ResultInvalidInput()
        {
            DobController dobController = new DobController(null, _config);
            List<DobMatchRequest> requestModel = null;

            MatchDobResponse dobMatchResult = dobController.MatchDOBJSON(requestModel);
            Assert.AreEqual("Invalid input!", dobMatchResult.Message);
        }
        [Test]
        public void DOBController_MatchDOB_ResultException()
        {
            DobController dobController = new DobController(null, _config);
            List<DobMatchRequest> requestModel = new List<DobMatchRequest>();
            requestModel.Add(new DobMatchRequest { pin = 1, dob = "sdcdjhsdjcds" });

            MatchDobResponse dobMatchResult = dobController.MatchDOBJSON(requestModel);
            Assert.AreNotEqual("0", dobMatchResult.Message);
        }
    }   
}
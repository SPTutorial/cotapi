using COT.API.BLL;
using COT.API.Models;
using COT.API.Utils;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace COT.UnitTest
{
    [TestFixture]
    class UtilityTest
    {
        [SetUp]
        public void Setup()
        {

        }        
        
        #region DOB - Models
       
        [Test]
        public void Models_Validate_DOBInput()
        {
            DobInput oDOBInput = new DobInput();

            //Assign Values
            oDOBInput.Year = 1985;
            oDOBInput.Month = 9;
            oDOBInput.Date = 28;
            oDOBInput.Century = 19;

            oDOBInput.YearStr = "1985";
            oDOBInput.MonthStr = "9";
            oDOBInput.DateStr = "28";
            oDOBInput.CenturyStr = "19";

            //Assert Equals
            Assert.AreEqual(1985, oDOBInput.Year);
            Assert.AreEqual(9, oDOBInput.Month);
            Assert.AreEqual(28, oDOBInput.Date);
            Assert.AreEqual(19, oDOBInput.Century);

            Assert.AreEqual("1985", oDOBInput.YearStr);
            Assert.AreEqual("9", oDOBInput.MonthStr);
            Assert.AreEqual("28", oDOBInput.DateStr);
            Assert.AreEqual("19", oDOBInput.CenturyStr);
        }
        
        [Test]
        public void CommonUtility_ValidateByMonth()
        {
            Tuple<bool, string> dobSvc = CommonUtils.ValidateDateByMonth(1957, 8, 20);
            Assert.AreEqual(true, dobSvc.Item1);
        }
                
        [Test]
        public void Models_Validate_DOBMatchRequest()
        {
            DobMatchRequest oDOBInput = new DobMatchRequest();

            //Assign Values
            oDOBInput.pin = 3;
            oDOBInput.dob = "29-01-1998";
            
            //Assert Equals
            Assert.AreEqual(3, oDOBInput.pin);
            Assert.AreEqual("29-01-1998", oDOBInput.dob);
        }

        #endregion            
    }
}

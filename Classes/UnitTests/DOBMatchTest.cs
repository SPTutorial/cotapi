using COT.API.BLL;
using COT.API.Models;
using NUnit.Framework;
using System;

namespace COT.UnitTest
{
    [TestFixture]
    public class DOBMatchTest
    {
        [SetUp]
        public void Setup()
        {

        }

        #region DOB Rules

        [Test]
        public void MatchDOB_DOB19570101_Target19570101_Result7()
        {
            Tuple<int, string, bool> dobSvc = new DobSvc().MatchDOB("19570101", "1957-01-01");
            Assert.AreEqual(7, dobSvc.Item1);
        }

        [Test]
        public void MatchDOB_DOB19930707_Target19930707_Result9()
        {
            Tuple<int,string,bool> dobSvc = new DobSvc().MatchDOB("19930707", "1993-07-07");
            Assert.AreEqual(9, dobSvc.Item1);
        }
        
        [Test]
        public void MatchDOB_DOB19640607_Target19640611_Result8()
        {
            Tuple<int, string, bool> dobSvc = new DobSvc().MatchDOB("19640607", "1964-06-11");
            Assert.AreEqual(8, dobSvc.Item1);
        }        

        [Test]
        public void MatchDOB_DOB19590919_Target19690919_Result0()
        {
            Tuple<int, string, bool> dobSvc = new DobSvc().MatchDOB("19590919", "1969-09-19");
            Assert.AreEqual(0, dobSvc.Item1);
        }

        [Test]
        public void MatchDOB_DOB19590919_Target19520819_Result8()
        {
            Tuple<int, string, bool> dobSvc = new DobSvc().MatchDOB("19590919", "1952-08-19");
            Assert.AreEqual(8, dobSvc.Item1);
        }

        [Test]
        public void MatchDOB_DOB19640607_Target19680808_Result0()
        {
            Tuple<int, string, bool> dobSvc = new DobSvc().MatchDOB("19640607", "1968-08-08");
            Assert.AreEqual(0, dobSvc.Item1);
        }

        [Test]
        public void MatchDOB_DOB19640607_Target19641010_Result0()
        {
            Tuple<int, string, bool> dobSvc = new DobSvc().MatchDOB("19640607", "1964-10-10");
            Assert.AreEqual(0, dobSvc.Item1);
        }

        [Test]
        public void MatchDOB_DOB19600603_Target19680730_Result6()
        {
            Tuple<int, string, bool> dobSvc = new DobSvc().MatchDOB("19600603", "1967-07-30");
            Assert.AreEqual(6, dobSvc.Item1);
        }

        [Test]
        public void MatchDOB_DOB19500703_Target19600603_Result8()
        {
            Tuple<int, string, bool> dobSvc = new DobSvc().MatchDOB("19500703", "1960-06-03");
            Assert.AreEqual(8, dobSvc.Item1);
        }

        [Test]
        public void MatchDOB_DOB19500703_Target19581114_Result0()
        {
            Tuple<int, string, bool> dobSvc = new DobSvc().MatchDOB("19500703", "1958-11-14");
            Assert.AreEqual(0, dobSvc.Item1);
        }

        #endregion

        #region DOB Services

        [Test]
        public void ValidateInputDOB_19500703_ResultTrue()
        {
            Tuple<bool, string> dobSvc = new DobSvc().ValidateInputDOB("19500703");
            Assert.AreEqual(true, dobSvc.Item1);
        }

        [Test]
        public void Valid_Input1957_03_03_ResultTrue()
        {
            Tuple<bool, string> dobSvc = new DobSvc().ValidateTargetDOB("1950-07-03");
            Assert.AreEqual(true, dobSvc.Item1);
        }

        #endregion
    }
}
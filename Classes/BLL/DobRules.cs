using COT.API.Models;
using COT.API.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace COT.API.BLL
{
    [ExcludeFromCodeCoverage]
    public class DobRules
    {
        readonly string inputCentury;
        readonly string inputYearExceptCC;
        readonly string inputYear;
        readonly string inputMonth;
        readonly string inputDate;

        readonly string targetCentury;
        readonly string targetYearExceptCC;
        readonly string targetYear;
        readonly string targetMonth;
        readonly string targetDate;

        private readonly string exMessage = "";

        public DobRules(string inputDOB,string targetDOB)
        {
            try
            {
                // input dob
                Tuple<DobInput, DateTime, bool, string> inputDOBData = CommonUtils.SeparateDOBInput(inputDOB);

                // target dob
                DateTime? isValidTargetDOB = CommonUtils.ToDate(targetDOB);

                DateTime targetDOBDate = isValidTargetDOB.Value;

                // input dob details
                inputCentury = inputDOBData.Item1.CenturyStr;
                inputYearExceptCC = inputDOBData.Item1.YearStr.Substring(2, 2);
                inputYear = inputDOBData.Item1.YearStr;
                inputMonth = inputDOBData.Item1.MonthStr;
                inputDate = inputDOBData.Item1.DateStr;

                // target dob details
                DobInput targetDOBData = CommonUtils.SeparateDOBInput(targetDOBDate);
                targetCentury = targetDOBData.CenturyStr;
                targetYearExceptCC = targetDOBData.YearStr.Substring(2, 2);
                targetYear = targetDOBData.YearStr;
                targetMonth = targetDOBData.MonthStr;
                targetDate = targetDOBData.DateStr;
            }
            catch(Exception ex)
            {
                exMessage = ex.Message;
            }
        }

        #region DOB - Rules

        public Tuple<string,string,bool> Rule0()
        {
            string score = "0";
            string message = "";
            if (inputYear.Equals(targetYear) && inputMonth.Equals(targetMonth) && inputDate.Equals(targetDate)
                && inputMonth != "01" && inputDate != "01")
            {
                score = "9";
                message = "Rule 0: if both dob exact match then score will be 9 and exit";
                return Tuple.Create(score, message, true);
            }

            return Tuple.Create(score, message, false);
        }

        public Tuple<string, string, bool> Rule1()
        {
            string score = "0";
            string message = "";

            if (!inputCentury.Equals(targetCentury))
            {
                score = "0";
                message = "Rule 1: if century not match then score will be 0 and exit";
                return Tuple.Create(score, message, true);
            }

            return Tuple.Create(score, message, false);
        }

        public Tuple<string, string, bool> Rule2()
        {
            string score = "0";
            string message = "";

            if (inputYear.Equals(targetYear) && inputMonth.Equals(targetMonth) && !inputDate.Equals(targetDate))
            {
                score = "8";
                message = "Rule 2: if CCYYMM matches and DD not match then score will be 8 and exit";
                return Tuple.Create(score, message, true);
            }

            return Tuple.Create(score, message, false);
        }

        public Tuple<string, string, bool> Rule3()
        {
            string score = "0";
            string message = "";

            if (inputYear.Equals(targetYear) && inputDate.Equals(targetDate) && !inputMonth.Equals(targetMonth))
            {
                score = "8";
                message = "Rule 3: if CCYY & DD matches & MM not match then score will be 8 and exit";
                return Tuple.Create(score, message, true);
            }

            return Tuple.Create(score, message, false);
        }

        public Tuple<string, string, bool> Rule4()
        {
            string score = "0";
            string message = "";

            if (inputCentury.Equals(targetCentury) && inputDate.Equals(targetDate) && !inputMonth.Equals(targetMonth))
            {
                string inputDOBFirstY = CommonUtils.GetFirstDigit(CommonUtils.ToInt(inputYearExceptCC));
                string inputDOBSecondY = CommonUtils.GetSecondDigit(CommonUtils.ToInt(inputYearExceptCC));

                string targetDOBFirstY = CommonUtils.GetFirstDigit(CommonUtils.ToInt(targetYearExceptCC));
                string targetDOBSecondY = CommonUtils.GetSecondDigit(CommonUtils.ToInt(targetYearExceptCC));

                if (inputDOBFirstY.Equals(targetDOBFirstY) || inputDOBSecondY.Equals(targetDOBSecondY))
                {
                    score = "8";
                    message = "Rule 4: if CC & DD matches & MM not match & 1 digit of CC matches then score will be 8 and exit";
                    return Tuple.Create(score, message, true);
                }
            }

            return Tuple.Create(score, message, false);
        }

        public Tuple<string, string, bool> Rule5()
        {
            string score = "0";
            string message = "";

            if (inputYear.Equals(targetYear) && (inputMonth.Equals(targetDate) || inputDate.Equals(targetMonth)))
            {
                score = "7";
                message = "Rule 5";
                return Tuple.Create(score, message, true);
            }

            return Tuple.Create(score, message, false);
        }

        public Tuple<string, string, bool> Rule6()
        {
            string score = "0";
            string message = "";

            if (inputYear.Equals(targetYear) && (inputDate.Equals(01) && inputMonth.Equals(01) || (targetDate.Equals(01) && targetMonth.Equals(01))))
            {
                score = "7";
                message = "Rule 6: if CCYY are equal & inputDOB or targetDOB's DDMM are 0101";
                return Tuple.Create(score, message, true);
            }

            return Tuple.Create(score, message, false);
        }

        public Tuple<string, string, bool> Rule7()
        {
            string score = "0";
            string message = "";

            if (inputCentury.Equals(targetCentury) && !inputYearExceptCC.Equals(targetYearExceptCC) && inputDate.Equals(targetDate))
            {
                if ((CommonUtils.ToInt(targetMonth) - CommonUtils.ToInt(inputMonth)) == 1 || (CommonUtils.ToInt(targetMonth) - CommonUtils.ToInt(inputMonth)) == -1)
                {
                    score = "7";
                    message = "Rule 7";
                    return Tuple.Create(score, message, true);
                }
                else if ((CommonUtils.ToInt(targetMonth) - CommonUtils.ToInt(inputMonth) >= 2 || (CommonUtils.ToInt(targetMonth) - CommonUtils.ToInt(inputMonth)) <= -2))
                {
                    score = "0";
                    message = "Rule 7";
                    return Tuple.Create(score, message, true);
                }
            }

            return Tuple.Create(score, message, false);
        }

        public Tuple<string, string, bool> Rule8()
        {
            string score = "0";
            string message = "";

            if (inputCentury.Equals(targetCentury) && !inputYearExceptCC.Equals(targetYearExceptCC) && ((inputMonth.Equals(targetMonth)) || ((CommonUtils.ToInt(inputMonth) - CommonUtils.ToInt(targetMonth)) == 1) || ((CommonUtils.ToInt(inputMonth) - CommonUtils.ToInt(targetMonth)) == -1)))
            {
                string inputDOBFirstD = CommonUtils.GetFirstDigit(CommonUtils.ToInt(inputDate));
                string inputDOBSecondD = CommonUtils.GetSecondDigit(CommonUtils.ToInt(inputDate));

                string targetDOBFirstD = CommonUtils.GetFirstDigit(CommonUtils.ToInt(targetDate));
                string targetDOBSecondD = CommonUtils.GetSecondDigit(CommonUtils.ToInt(targetDate));

                // check if dd is reversed Ex: 02 to 20 or 20 to 02
                if (inputDOBFirstD.Equals(targetDOBSecondD) && inputDOBSecondD.Equals(targetDOBFirstD))
                {
                    score = "6";
                    message = "Rule 6:";
                    return Tuple.Create(score, message, true);
                }
            }

            return Tuple.Create(score, message, false);
        }
              

        #endregion        
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using COT.API.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using System.Linq;
using COT.API.Utils;
using System.Diagnostics.CodeAnalysis;

namespace COT.API.BLL
{
    public class DobSvc
    {
        
        public DobSvc()
        {
           
        }
       
        public Tuple<int, string, bool> MatchDOB(string inputDOB, string targetDOB)
        {            
            bool isSuccess = false;
            string message = "";
            int score = 0;
            DobRules oRules = null;

            try
            {
                // validate input dob
                Tuple<bool, string> validateInputDOBResponse = ValidateInputDOB(inputDOB);
                if (validateInputDOBResponse.Item1)
                {
                    oRules = new DobRules(inputDOB, targetDOB);
                    // validate target dob
                    Tuple<bool, string> validateTargetDOBResponse = ValidateTargetDOB(targetDOB);
                    if (validateTargetDOBResponse.Item1)
                    {
                        DateTime? isValidTargetDOB = CommonUtils.ToDate(targetDOB);
                        if (isValidTargetDOB.HasValue)
                        {
                            Tuple<string, string, bool> rule = oRules.Rule0();

                            if (rule.Item3)
                                return Tuple.Create(CommonUtils.ToInt(rule.Item1), rule.Item2, rule.Item3);

                            rule = oRules.Rule1();
                            if (rule.Item3)
                                return Tuple.Create(CommonUtils.ToInt(rule.Item1), rule.Item2, rule.Item3);

                            rule = oRules.Rule2();
                            if (rule.Item3)
                                return Tuple.Create(CommonUtils.ToInt(rule.Item1), rule.Item2, rule.Item3);

                            rule = oRules.Rule3();
                            if (rule.Item3)
                                return Tuple.Create(CommonUtils.ToInt(rule.Item1), rule.Item2, rule.Item3);

                            rule = oRules.Rule4();
                            if (rule.Item3)
                                return Tuple.Create(CommonUtils.ToInt(rule.Item1), rule.Item2, rule.Item3);

                            rule = oRules.Rule5();
                            if (rule.Item3)
                                return Tuple.Create(CommonUtils.ToInt(rule.Item1), rule.Item2, rule.Item3);

                            rule = oRules.Rule6();
                            if (rule.Item3)
                                return Tuple.Create(CommonUtils.ToInt(rule.Item1), rule.Item2, rule.Item3);

                            rule = oRules.Rule7();
                            if (rule.Item3)
                                return Tuple.Create(CommonUtils.ToInt(rule.Item1), rule.Item2, rule.Item3);

                            rule = oRules.Rule8();
                            if (rule.Item3)
                                return Tuple.Create(CommonUtils.ToInt(rule.Item1), rule.Item2, rule.Item3);
                        }
                        else
                        {
                            message = "Invalid target DOB!";
                        }
                    }
                    else
                    {
                        message = validateTargetDOBResponse.Item2;
                    }
                }
                else
                {
                    message = validateInputDOBResponse.Item2;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Tuple.Create(score, message, isSuccess);
        }        

        public Tuple<bool, string> ValidateInputDOB(string inputDOB)
        {
            string message = "";
            bool isValid = false;
            try
            {
                if (!string.IsNullOrEmpty(inputDOB))
                {
                    // validate the inputDOB
                    // check the inputDOB is a proper no or not
                    if (CommonUtils.ValidateNumber(inputDOB))
                    {
                        // check the length
                        if (inputDOB.Length == 8)
                        {
                            // a valid number
                            DobInput inputDOBData = CommonUtils.SeparateDOBInputFromNumber(inputDOB);
                            // validate month
                            if (CommonUtils.ToInt(inputDOBData.MonthStr) <= 12 && CommonUtils.ToInt(inputDOBData.MonthStr) > 0)
                            {
                                // validate date
                                Tuple<bool, string> validateResp = CommonUtils.ValidateDateByMonth(CommonUtils.ToInt(inputDOBData.YearStr), CommonUtils.ToInt(inputDOBData.MonthStr), CommonUtils.ToInt(inputDOBData.DateStr));
                                if (validateResp.Item1)
                                {
                                    isValid = true;
                                }
                                else
                                {
                                    message = validateResp.Item2;
                                }
                            }
                            else
                            {
                                message = $"Input DOB:{inputDOB} invalid month!";
                            }
                        }
                        else
                        {
                            message = $"Input DOB:{inputDOB} is invalid! input text length should be 8 and format should be CCYYMMDD";
                        }
                    }
                    else
                    {
                        message = $"Input DOB:{inputDOB} is invalid! input text should be a valid number and format should be CCYYMMDD";
                    }
                }
                else
                {
                    message = $"Input DOB:{inputDOB} is invalid! format should be CCYYMMDD";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Tuple.Create(isValid, message);
        }

        public Tuple<bool, string> ValidateTargetDOB(string targetDOB)
        {
            string message = "";
            bool isValid = false;
            try
            {
                if (!string.IsNullOrEmpty(targetDOB))
                {
                    // validate the inputDOB
                    // check the inputDOB is a proper no or not
                    if (CommonUtils.ToDate(targetDOB).HasValue)
                    {
                        isValid = true;
                    }
                    else
                    {
                        message = $"Target DOB:{targetDOB} is invalid! format should be DD-MM-YYYY";
                    }
                }
                else
                {
                    message = $"Target DOB:{targetDOB} is invalid! format should be DD-MM-YYYY";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return Tuple.Create(isValid, message);
        }       
    }
}
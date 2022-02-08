using COT.API.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace COT.API.Utils
{
    [ExcludeFromCodeCoverage]
    public class CommonUtils
    {

        
        public CommonUtils()
        {
            //
            // TODO: Add constructor logic here
            //

        }

        //public static bool ToBool(object value)
        //{
        //    return ToBool(value);
        //}

        public static int ToInt(object value)
        {
            int result = (int)ToFloat(value);

            return result;
        }

        public static long ToInt64(object value)
        {
            Int64 result = 0;

            if (Int64.TryParse((value == null ? 0 : value).ToString(), out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
            //long result = (long)ToFloat(value);

            //return result;
        }

        public static float ToFloat(object value)
        {
            float result = 0;

            if (float.TryParse((value == null ? 0 : value).ToString(), out result))
            {
                return result;
            }
            else
            {
                return 0;
            }

        }
        public static decimal ToDecimal(object value)
        {
            decimal result = 0;

            if (decimal.TryParse((value == null ? 0 : value).ToString(), out result))
            {
                return result;
            }
            else
            {
                return 0;
            }

        }
        public static DateTime? ToDate(object value)
        {
            DateTime? result = (DateTime?)null;
            if (value == null)
                return null;
            try
            {
                result = DateTime.Parse(value.ToString());
            }
            catch (Exception e)
            {
                string exMessage = e.Message;
            }
            return result;
        }

        public static DateTime? ToDate(object value, string format)
        {
            DateTime? result = (DateTime?)null;
            if (value == null)
                return null;
            try
            {
                result = DateTime.ParseExact(value.ToString(), format, null);
            }
            catch (Exception e)
            {
                string exMessage = e.Message;
            }
            return result;
        }

        public static bool ValidateNumber(string number)
        {
            long n;
            return long.TryParse(number, out n);
        }

        public static Tuple<DobInput, DateTime, bool, string> SeparateDOBInput(string dobInput)
        {
            bool isValid = false;
            string message = "";
            // function take the dob paramater and separate the year month day and century
            DobInput input = new DobInput();
            DateTime dt = new DateTime();
            try
            {
                // first 4 digit is year
                input.YearStr = dobInput.Substring(0, 4);
                input.Year = Convert.ToInt32(input.YearStr);

                input.MonthStr = dobInput.Substring(4, 2);
                input.Month = Convert.ToInt32(input.MonthStr);

                input.DateStr = dobInput.Substring(6, 2);
                input.Date = Convert.ToInt32(input.DateStr);

                input.CenturyStr = dobInput.Substring(0, 2);
                input.Century = Convert.ToInt32(input.CenturyStr);

                dt = new DateTime(input.Year, input.Month, input.Date);
                isValid = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Tuple.Create(input, dt, isValid, message);
        }
        public static DobInput SeparateDOBInput(DateTime inputDate)
        {
            // function take the dob paramater and separate the year month day and century
            DobInput input = new DobInput();
            try
            {
                string yyyymmdd = inputDate.ToString("yyyyMMdd");
                // first 4 digit is year
                input = SeparateDOBInputFromNumber(yyyymmdd);
            }
            catch (Exception e)
            {
                string exMessage = e.Message;
            }

            return input;
        }
        public static DobInput SeparateDOBInputFromNumber(string dobInput)
        {
            // function take the dob paramater and separate the year month day and century
            DobInput input = new DobInput();
            // first 4 digit is year
            input.YearStr = dobInput.Substring(0, 4);
            input.MonthStr = dobInput.Substring(4, 2);
            input.DateStr = dobInput.Substring(6, 2);
            input.CenturyStr = dobInput.Substring(0, 2);

            input.Year = CommonUtils.ToInt(input.YearStr);
            input.Month = CommonUtils.ToInt(input.MonthStr);
            input.Date = CommonUtils.ToInt(input.DateStr);
            input.Century = CommonUtils.ToInt(input.CenturyStr);

            return input;
        }

        public static string GetFirstDigit(int number)
        {
            return SingleNoToDouble(number)[0].ToString();
        }
        public static string GetSecondDigit(int number)
        {
            return SingleNoToDouble(number)[1].ToString();
        }
        private static string SingleNoToDouble(int number)
        {
            if (number > 9)
            {
                return number.ToString();
            }
            else
            {
                return $"0{number.ToString()}";
            }
        }

        public static Tuple<bool, string> ValidateDateByMonth(int year, int month, int date)
        {
            bool isValid = false;
            string message = "Invalid date!";
            switch (month)
            {
                case 1:
                    // january 31 days
                    if (date > 0 && date <= 31)
                        isValid = true;
                    break;
                case 2:
                    // feb need check
                    if (IsLeapYear(year))
                    {
                        if (date > 0 && date <= 29)
                            isValid = true;
                    }
                    else
                    {
                        if (date > 0 && date <= 28)
                            isValid = true;
                    }
                    break;
                case 3:
                    // march 31 days
                    if (date > 0 && date <= 31)
                        isValid = true;
                    break;
                case 4:
                    // april 30 days
                    if (date > 0 && date <= 30)
                        isValid = true;
                    break;
                case 5:
                    // may 31 days
                    if (date > 0 && date <= 31)
                        isValid = true;
                    break;
                case 6:
                    // june 30 days
                    if (date > 0 && date <= 30)
                        isValid = true;
                    break;
                case 7:
                    // july 31 days
                    if (date > 0 && date <= 31)
                        isValid = true;
                    break;
                case 8:
                    // august 31 days
                    if (date > 0 && date <= 31)
                        isValid = true;
                    break;
                case 9:
                    // sep 30 days
                    if (date > 0 && date <= 30)
                        isValid = true;
                    break;
                case 10:
                    // oct 31 days
                    if (date > 0 && date <= 31)
                        isValid = true;
                    break;
                case 11:
                    // nov 30 days
                    if (date > 0 && date <= 30)
                        isValid = true;
                    break;
                case 12:
                    // dec 31 days
                    if (date > 0 && date <= 31)
                        isValid = true;
                    break;
            }


            return Tuple.Create(isValid, message);
        }
        public static bool IsLeapYear(int year)
        {
            return DateTime.IsLeapYear(year);
        }

        public static Tuple<bool, string> ValidatePin(string pin)
        {
            string message = "";
            bool isValid = false;
            try
            {
                if (ValidateNumber(pin))
                {
                    isValid = true;
                }
                else
                {
                    message = "Invalid pin number!";
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
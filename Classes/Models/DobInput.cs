using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COT.API.Models
{
    [ExcludeFromCodeCoverage]
    public class DobInput
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Date { get; set; }
        public int Century { get; set; }
        public string YearStr { get; set; }
        public string MonthStr { get; set; }
        public string DateStr { get; set; }
        public string CenturyStr { get; set; }
    }

    #region for xml input
    [ExcludeFromCodeCoverage]
    [XmlRoot(ElementName = "inputdata")]
    public class Inputdata
    {

        [XmlElement(ElementName = "pin")]
        public int pin { get; set; }

        [XmlElement(ElementName = "dob")]
        public string dob { get; set; }

        [XmlElement(ElementName = "target")]
        public string target { get; set; }
    }

    [ExcludeFromCodeCoverage]
    [XmlRoot(ElementName = "root")]
    public class DobMatchRequestList
    {

        [XmlElement(ElementName = "inputdata")]
        public List<Inputdata> Inputdata { get; set; }
    }
    #endregion
}

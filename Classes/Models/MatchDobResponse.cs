using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace COT.API.Models
{
    [ExcludeFromCodeCoverage]
    public class MatchDobResponse : Response
    {
        public MatchDobResponse()
        {
            Message = "";
            MatchResults = new List<DobResponse>();
        }
        public List<DobResponse> MatchResults { get; set; }
        public int Status { get; set; }
    }
    [ExcludeFromCodeCoverage]
    public class DobResponse
    {
        public DobResponse()
        {
            dob = "";
            tabledob = "";
            matchlevel = "0";
        }
        public int pin { get; set; }
        public string dob { get; set; }
        public string tabledob { get; set; }
        public string matchlevel { get; set; }
    }
}
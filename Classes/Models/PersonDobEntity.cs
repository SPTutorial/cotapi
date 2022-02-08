using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace COT.API.Models
{
    [ExcludeFromCodeCoverage]
    public class PersonDobEntity
    {
        public int id { get; set; }
        public int pin { get; set; }
        public string dob { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace COT.API.Models
{
    [ExcludeFromCodeCoverage]
    public class DobMatchRequest
    {
        public int pin { get; set; }
        public string dob { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string target { get; set; }
    }
}

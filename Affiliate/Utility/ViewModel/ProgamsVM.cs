using Affiliate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.Utility.ViewModel
{
    public class ProgamsVM
    {
        public List<ProgramsOpt> ProgramsOpt { get; set; }
    }
    public class ProgramsOpt
    {
        public string programCategory { get; set; }
        public List<Programs> Programs { get; set; }
    }
}

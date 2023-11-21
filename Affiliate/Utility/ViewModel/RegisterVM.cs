using Affiliate.Models;
using Affiliate.Utility.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.Utility.ViewModel
{
    public class RegisterVM
    {
        public RegisterDTO registerz { get; set; }
        //public IEnumerable<Programs> programListz { get; set; }
        public IEnumerable<Countries> countryListz { get; set; }
        public IEnumerable<Banks> bankListz { get; set; }
        public LoginVM dto { get; set; }
       // public IEnumerable<States> nigeriaStatesListz { get; set; }
    }
}

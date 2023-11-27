using Affiliate.Utility.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.Utility.ViewModel
{
    public class LoginVM
    {
        public LoginDTO login { get; set; }
        public string code { get; set; }
        public string email { get; set; }
        public string role { get; set; }
    }
}

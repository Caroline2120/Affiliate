using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.Utility.DTO
{
    public class RegisterDTO
    {
        [Required]
        public string role { get; set; }
        [Required]
        public string Username { get; set; }
      
        public string StaffId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }       
        //public int Designation { get; set; }
        //[Required]
        //public DateTime DateOfBirth { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public int BankId { get; set; }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        //[Required]
        //public List<int> StateIds { get; set; }
        //[Required]
        //public List<int> ProgramIds { get; set; }
        [Required]
        public string Password { get; set; }
        //public string Gender { get; set; }
    }
}

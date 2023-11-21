using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.Models
{
    public class UserReferred
    {
        [Key]
        public int Id { get; set; }
        public string ReferralId { get; set; }
        public Users Referral { get; set; }
        public string PaymentRef { get; set; }
        public string ReferredUserId { get; set; }
        public Users ReferredUser { get; set; }
        public float Earnings { get; set; }
    }
}

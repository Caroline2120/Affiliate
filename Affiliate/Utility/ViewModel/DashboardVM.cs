using Affiliate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.Utility.ViewModel
{
    public class DashboardVM
    {
        public Users user { get; set; }
        public AffiliateUserAccount account { get; set; }
        public List<ProgramRecord> Programs { get; set; }
        public List<StatesRecord> StateRecord { get; set; }
        public string rCode { get; set; }
        public string referralLink { get; set; }
        public string Role { get; set; }
        public int totalReferralUsage { get; set; }
    }
    public class StatesRecord
    {
        public string Country { get; set; }
        public string State { get; set; }
        public int StateId { get; set; }


    }
    public class ProgramRecord
    {
        public string ProgramCategory { get; set; }
        public string Program { get; set; }
        public int userProgramId { get; set; }
    }
    public class ReferralUsage
    {
        public string rCode { get; set; }
        public string referralLink { get; set; }
        public List<ReferralUsage2> ReferralUsage2 { get; set; }

        public List<UserDiscount> Discount { get; set; }
    }
    public class ReferralUsage2
    {
        public string Fullname { get; set; }
        public string Program { get; set; }
        public string AmountPaid { get; set; }
        public string Earnings { get; set; }
        public string DateRegistered { get; set; }
    }
}

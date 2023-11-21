using Affiliate.Models;
using Affiliate.Utility.DTO;
using Affiliate.Utility.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.Utility
{
   public interface IProjectService
    {
        Task<List<Countries>> GetCountry();
        Task<List<States>> GetStatesByCountryId(int CountryId);
        Task<List<Cities>> GetCitiesByStateId(int StateId);
        Task<List<ProgramCategory>> GetPrograms();
        //Task<ProgamsVM> GetPrograms();
        Task<List<Banks>> GetBanks();
        Task<MarketerCode> GetMarketerCodeByCode(string Code, string Email);
        Task<Tuple<Users, string>> RegisterUser(RegisterDTO dto);
        Task<Tuple<Users, string>> Login(LoginDTO dto);
        Task<RegisterVM> GetRegister();
        Task<DashboardVM> DashboardRe(string email);
        Task<ReferralUsage> UserReferralCodeUsage(string email);

    }
}

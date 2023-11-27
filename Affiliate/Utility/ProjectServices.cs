using Affiliate.Models;
using Affiliate.Utility.Data;
using Affiliate.Utility.DTO;
using Affiliate.Utility.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.Utility
{
    public class ProjectServices : IProjectService
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProjectServices(RoleManager<IdentityRole> roleManager, SignInManager<Users> signInManager, ApplicationDBContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<List<ProgramCategory>> GetPrograms()
        {
            //Load countries from DB
            //----------------------
            var countries = await _context.ProgramCategory.ToListAsync();
            return countries;
        }
        //public async Task<ProgamsVM> GetPrograms()
        //{
        //    var programsvm = new ProgamsVM();
        //    var pCat = await _context.ProgramCategories.ToListAsync();
        //    if(pCat != null && pCat.Count()>0)
        //    {
        //        var programsC = new List<ProgramsOpt>();
        //        foreach (var item in pCat)
        //        {
        //            var eachPC = new ProgramsOpt();
        //            //Load program from DB
        //            //--------------------
        //            var programs = await _context.Programs.Include(x => x.ProgramCategory).Where(x=>x.ProgramCategoryId == item.Id).Select(x => new Programs
        //            {
        //               Id = x.Id,
        //               Name = x.Name,
                       
        //            }).ToListAsync();

        //            eachPC.programCategory = item.Name;
        //            eachPC.Programs = programs;

        //            programsC.Add(eachPC);
        //        }

        //        programsvm.ProgramsOpt = programsC;
        //    }
           
        //    return programsvm;
        //}
        public async Task<List<Banks>> GetBanks()
        {
            //Load banks from DB
            //--------------------
            var banks = await _context.Banks.ToListAsync();
            return banks;
        }
        public async Task<List<Countries>> GetCountry()
        {
            //Load countries from DB
            //----------------------
            var countries = await _context.Countries.ToListAsync();
            return countries;
        }
        public async Task<List<States>> GetStatesByCountryId(int CountryId)
        {
            //Load states by country id from DB
            //---------------------------------
            var states = await _context.States.Where(x=>x.CountryId == CountryId).ToListAsync();
            return states;
        }
        public async Task<List<Cities>> GetCitiesByStateId(int StateId)
        {
            //Load cities by state id from DB
            //---------------------------------
            var cities = await _context.Cities.Where(x => x.StateId == StateId).ToListAsync();
            return cities;
        }
        public async Task<MarketerCode> GetMarketerCodeByCode(string Code, string Email)
        {
            //Load code details by code from DB
            //---------------------------------
            var marketerExist = await _context.Marketers.Where(x => x.Email == Email).FirstOrDefaultAsync();
           
            if(marketerExist != null)
            {
                var cod = await _context.MarketerCode.Where(x => x.Code == Code).FirstOrDefaultAsync();
                if(cod != null)
                {
                    return cod;
                }
                return null;

            }
            return null;
        }
        private string GenerateReferalCode()
        {
            StringBuilder builder = new StringBuilder();

            Random rstToken = new Random();

            char ch;
            for (int i = 0; i < 6; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rstToken.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
        public async Task<Tuple<Users, string>> RegisterUser(RegisterDTO dto)
        {

            var existingUser = await _userManager.FindByEmailAsync(dto.Email);// _context.users.Where(x=>x.Email == dto.Email).FirstOrDefaultAsync();


            if (existingUser != null)
            {
                return new Tuple<Users, string>(existingUser, "Email already exist.") ;
            }
            else
            {
                var currentuser = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == dto.Phone);
                if (currentuser != null)
                {
                    return new Tuple<Users, string>(currentuser, "Phone number already exist.") ;

                }

                //var lastUser = await _context.users.CountAsync();
                //int count = 0;
                //if (lastUser >0)
                //{
                //    count = Convert.ToInt32(lastUser);
                //}

                //count = count + 1;
                var rCode = GenerateReferalCode();
                string Id = dto.StaffId;
                if (dto.role == "Freelance")
                {
                    Id = rCode;
                }
                var defaR = await _roleManager.FindByNameAsync(dto.role);

                var newUser = new Users
                {
                    UserName = dto.Username,
                    Email = dto.Email.ToLower(),
                    PhoneNumber = dto.Phone,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                   // Gender = dto.Gender,
                    EmailConfirmed = true,
                    //StateId = dto.StateId,
                    //StateIds =string.Join(",", dto.StateIds),
                    //DateOfBirth = dto.DateOfBirth,
                    MiddleName = dto.MiddleName,
                    ReferralCode = rCode,
                    RegisteredDate = DateTime.Now.Date,
                    //UserId = $"{count:0000}",
                    //Role = dto.Role,
                    Status = UserStatusEnums.Active,
                    DefaultRole = defaR.Id,
                    StudentNumber = Id,
                    StaffDep = StaffDepEnums.None,
                    NYSC=false
                };

                var createdUser = await _userManager.CreateAsync(newUser, dto.Password);

                if (createdUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, dto.role);

                    await _context.SaveChangesAsync();
                    //Add user program into DB
                    //-------------------------
                    var userAcct = new AffiliateUserAccount()
                    {
                        BankId = dto.BankId,
                        AccountName = dto.AccountName,
                        AccountNumber = dto.AccountNumber,
                        UserId = newUser.Id
                    };
                    await _context.AffiliateUserAccount.AddAsync(userAcct);
                    await _context.SaveChangesAsync();

                    var result = await _signInManager.PasswordSignInAsync(newUser, dto.Password, true, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return new Tuple<Users, string>(newUser, "Signed In");
                    }
                    else
                    {
                        return new Tuple<Users, string>(newUser, "Successful");
                    }

                    ////Add user subjects into DB
                    ////-------------------------                    
                    //if (dto.ProgramIds != null && dto.ProgramIds.Count() > 0)
                    //{
                    //    var userPro = new AffiliateUserProgram()
                    //    {
                    //        ProgramIds = string.Join(",", dto.ProgramIds),
                    //        UserId = newUser.Id
                    //    };
                    //    await _context.AffiliateUserProgram.AddAsync(userPro);
                    //}



                }
                return new Tuple<Users, string> (null, "Error creating user.");
            }
        }
        public async Task<Tuple<Users, string>> Login(LoginDTO dto)
        {

            var existingUser = await _userManager.FindByEmailAsync(dto.email);

            if (existingUser == null)
            {
                return new Tuple<Users, string>(null, "Not a user.");
            }
            else
            {
                var userDefaultRole = await _roleManager.FindByIdAsync(existingUser.DefaultRole);
                if(userDefaultRole.Name != "Staff" && userDefaultRole.Name != "Freelance")
                {
                    return new Tuple<Users, string>(null, "Your default role is not an affiliate.");
                }

                var result = await _signInManager.PasswordSignInAsync(existingUser, dto.password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return new Tuple<Users, string>(existingUser, "Successful.");
                }
                return new Tuple<Users, string>(existingUser, "Not Login.");

            }
        }
        public async Task<RegisterVM> GetRegister()
        {
            //Load Program from DB
            //--------------------
            //IEnumerable<Programs> programList = await _context.Programs.ToListAsync();

            //Load banks from DB
            //------------------
            IEnumerable<Banks> bankList = await _context.Banks.ToListAsync();

            //Load countries from DB
            //-----------------------
            IEnumerable<Countries> countryList = await _context.Countries.ToListAsync();
            //Load Nigeria states from DB
            //---------------------------
            //IEnumerable<States> stateList = await _context.States.Where(x=>x.CountryId == 163).ToListAsync();

            var viewModel = new RegisterVM
            {
                bankListz = bankList,
                countryListz = countryList,
                //nigeriaStatesListz = stateList,
                //programListz =programList
            };

            return viewModel;
        }
        public async Task<DashboardVM> DashboardRe(string email)
        {

            var existingUser = await _userManager.FindByEmailAsync(email);//.Users.Include(x=>x.Role).Where(x=>x.Email == email).FirstOrDefaultAsync();


            if (existingUser != null)
            {
                // State History
                //----------------
                var stateList = new List<StatesRecord>();
                //var selectedStates = existingUser.StateIds.Split(',').ToList();
                //if(selectedStates != null && selectedStates.Count() > 0)
                //{
                    
                //    foreach(var item in selectedStates)
                //    {
                //        var state = await _context.States.Include(x => x.Country).Where(x => x.Id == Convert.ToInt32(item)).Select(x=> new StatesRecord { 
                //            Country =x.Country.Name,
                //            State =x.Name,
                //            StateId =x.Id
                //        }).FirstOrDefaultAsync();

                //        stateList.Add(state);
                //    }
                //}

                //Program history
                //---------------
                var programList = new List<ProgramRecord>();
                var userPrograms = await _context.AffiliateUserProgram.Where(x => x.UserId == existingUser.Id).FirstOrDefaultAsync();
                 if(userPrograms != null)
                {
                    var progra = userPrograms.ProgramIds.Split(',').ToList();
                    if(progra != null && progra.Count() >0)
                    {
                        foreach(var p in progra)
                        {
                            var userP = await _context.ProgramCategory.Include(x => x.Institution).Where(x => x.Id == Convert.ToInt32(p)).Select(x=>new ProgramRecord { 
                                Program =x.Name,
                                ProgramCategory = x.Name,
                                userProgramId = userPrograms.Id

                            }).FirstOrDefaultAsync();

                            programList.Add(userP);
                        }
                      
                    }
                }

                var userA = await _context.AffiliateUserAccount.Include(x => x.Bank).Where(x => x.UserId == existingUser.Id).FirstOrDefaultAsync();
                
                var totalRCU = await _context.UserReferred.Where(x => x.ReferralId == existingUser.Id).CountAsync();

                var UserDefa = await _roleManager.FindByIdAsync(existingUser.DefaultRole);
                //var role = 

                var result = new DashboardVM();
                //result.fullName = existingUser.FirstName;
                result.account = userA;
                result.user = existingUser;
                result.Programs = programList;
                result.StateRecord = stateList;
                result.rCode = existingUser.ReferralCode;
                result.totalReferralUsage = totalRCU;
                result.Role = UserDefa.Name;

                GeneralClass.FullName = existingUser.FirstName;
                return result;
            }
            else
            {
                return null;
            }
        }
        public async Task<ReferralUsage> UserReferralCodeUsage(string email)
        {
            //Load user programs
            //------------------
            var result = await _context.UserReferralPaymentHistory.Include(x => x.UserRefer).ThenInclude(x => x.Referral).Where(x => x.UserRefer.Referral.Email == email).OrderByDescending(x => x.Id).Select(x => new ReferralUsage2
            {
                AmountPaid = "₦ " + x.Amount.ToString("N"),
                DateRegistered = x.UserRefer.ReferredUser.RegisteredDate.ToString("dd/MM/yyyy"),
                Earnings = "₦ " + x.Earning.ToString("N"),
                Fullname = x.UserRefer.ReferredUser.FirstName + " " + x.UserRefer.ReferredUser.LastName,
               // Program = x.ReferredUserCourses.CoursePriceOption.Course.Name//x.ReferredUserProgramOption.ProgramOption.Program.Name + "/" + x.ReferredUserProgramOption.ProgramOption.Name
                //UserId = x.ReferredUserProgramOption.UserId,
                //Fullname = x.ReferredUserProgramOption.User.FirstName + " " + x.ReferredUserProgramOption.User.LastName,
                //userProOptionId = x.ReferredUserProgramOptionId,
                //earnings = x.Earnings,
                //rCode = x.Referral.ReferralCode

            }).ToListAsync();

            var resultList = new ReferralUsage();

            var user = await _userManager.FindByEmailAsync(email);
            resultList.rCode = user.ReferralCode;
            resultList.referralLink = "https://my.edurex.academy?code=" + user.ReferralCode;


            if (result.Count() > 0)
            {

                // var resultList2 = new List<ReferralUsage2>();
                //foreach (var item in result)
                //{
                //    //User Payment and Program
                //    //-------------------------
                //    var uProgPayment = await _context.UserPaymentHistory.Include(x => x.UserProgramOption).ThenInclude(x=>x.ProgramOption).ThenInclude(x=>x.Program).Where(x => x.UserProgramOptionId == item.userProOptionId && x.StatusId == PaymentStatusEnums.Paid).OrderByDescending(x=>x.Id).ToListAsync();

                //    if(uProgPayment.Count() >0)
                //    {
                //        foreach(var re in uProgPayment)
                //        {
                //            var refee = new ReferralUsage2
                //            {
                //                AmountPaid = re.Amount.ToString("N"),
                //                DateRegistered = re.UserProgramOption.RegDate.ToShortDateString(),
                //                Earnings = item.earnings.ToString("N"),
                //                Fullname =item.Fullname,
                //                Program = re.UserProgramOption.ProgramOption.Program.Name + "/" + re.UserProgramOption.ProgramOption.Name

                //            };
                //            resultList2.Add(refee);
                //        }

                //    }
                //    resultList.rCode = item.rCode;
                //}

                resultList.ReferralUsage2 = result;

            }

            //Load user programs
            //------------------
            var resultDis = await _context.UserDiscount.Include(x => x.Referral).Where(x => x.Referral.Email == email).OrderByDescending(x => x.Id).ToListAsync();
            if (resultDis.Count() > 0)
            {
                resultList.Discount = resultDis;
            }
            return resultList;
        }
    }
}

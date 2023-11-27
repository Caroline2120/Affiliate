using Affiliate.Models;
using Affiliate.Utility;
using Affiliate.Utility.DTO;
using Affiliate.Utility.ViewModel;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Affiliate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProjectService _projectServices;
        private readonly INotyfService _notyfService;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        private const string SessionUsername = "";

        public HomeController(SignInManager<Users> signInManager, UserManager<Users> userManager, INotyfService notyfService, ILogger<HomeController> logger, IProjectService projectServices)
        {
            _logger = logger;
            _projectServices = projectServices;
            _notyfService = notyfService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Profile()
        {
            var dashboardRecord = new DashboardVM();

            var email = GeneralClass.Email;

            var record = await _projectServices.DashboardRe(email);
            if (record != null)
            {
                dashboardRecord.account = record.account;
                dashboardRecord.user = record.user;
                dashboardRecord.StateRecord = record.StateRecord;
                dashboardRecord.Programs = record.Programs;
                dashboardRecord.rCode = record.rCode;
                return View(dashboardRecord);
            }
            GeneralClass.Email = "";
            GeneralClass.FullName = "";
            GeneralClass.Role = "";
            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //GeneralClass.Role = (int)UserRolesEnums.Freelance;
            //if (GeneralClass.Role == (int)UserRolesEnums.Freelance)
            //{
            //    GeneralClass.Email = "";
            //    GeneralClass.FullName = "";
            //}
            //else if(GeneralClass.Email == "" && GeneralClass.Email == null)
            //{
            //    GeneralClass.Email = "";
            //    GeneralClass.FullName = "";
            //    GeneralClass.Role = 0;
            //    return RedirectToAction(nameof(Login));
            //}
            var response = await _projectServices.GetRegister();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Index(RegisterVM dto)
        {
            //GeneralClass.Email = "";
            //if(GeneralClass.Role != null && GeneralClass.Role > 0)
            //{
            //    dto.registerz.Role = (UserRolesEnums)GeneralClass.Role;


            //}
            
            if (!ModelState.IsValid)
            {
                var response1 = await _projectServices.GetRegister();
                return View("Index", response1);
            }
            var paymentResponse = await _projectServices.RegisterUser(dto.registerz);

            if (paymentResponse.Item1 != null && paymentResponse.Item2 == "Signed In")
            {
               // _notyfService.Success("Login Successful", 10);
                GeneralClass.Role = "";
                HttpContext.Session.SetString(SessionUsername, paymentResponse.Item1.UserName.ToString());

                return RedirectToAction("Dashboard", "Home", new { area = "" });

                //GeneralClass.Email = dto.registerz.Email;
                //GeneralClass.FullName = $"{dto.registerz.FirstName} {dto.registerz.LastName}";

                //return RedirectToAction(nameof(Dashboard));
            }
            else if (paymentResponse.Item1 != null && paymentResponse.Item2 == "Successful")
            {
                _notyfService.Success("Account Created, Login", 10);
                return RedirectToAction("Login", "Home", new { area = "" });

                //GeneralClass.Email = dto.registerz.Email;
                //GeneralClass.FullName = $"{dto.registerz.FirstName} {dto.registerz.LastName}";

                //return RedirectToAction(nameof(Dashboard));
            }
            else
            {
                var response = await _projectServices.GetRegister();
                _notyfService.Error(paymentResponse.Item2, 10);
                return View("Index", response);
            }

           
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Home", new { area = "" });
        }
        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString(SessionUsername) == null)
            {
                _notyfService.Error("Session time out", 5);
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            var sessionUsername = HttpContext.Session.GetString(SessionUsername);
            var user = await _userManager.FindByNameAsync(sessionUsername);
            if (user == null)
            {
                _notyfService.Error("Invalid user", 5);
                return RedirectToAction("Login", "Home", new { area = "" });

            }
            GeneralClass.Email = user.Email;

            var dashboardRecord = new DashboardVM();

            //var email = GeneralClass.Email;

            var record = await _projectServices.DashboardRe(user.Email);
            if (record != null)
            {
                dashboardRecord.account = record.account;
                dashboardRecord.user = record.user;
                dashboardRecord.StateRecord = record.StateRecord;
                dashboardRecord.Programs = record.Programs;
                dashboardRecord.rCode = record.rCode;
                dashboardRecord.totalReferralUsage = record.totalReferralUsage;
                dashboardRecord.Role  = record.Role;
                dashboardRecord.referralLink = "https://my.edurex.academy/?code=" + record.rCode;

                _notyfService.Success("Welcome back, " + user.Email, 10);


                return View(dashboardRecord);
            }

            _notyfService.Error("Error Occured", 10);
            return RedirectToAction("Login", "Home", new { area = "" });
        }
        public IActionResult Login()
        {
            //GeneralClass.Email = "";
            //GeneralClass.FullName = "";
            //GeneralClass.Role = 0;
            HttpContext.Session.Clear();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM dto)
        {
           // GeneralClass.Email = "";
            var userLogin = await _projectServices.Login(dto.login);
            if (userLogin.Item2 == "Successful.")
            {
                HttpContext.Session.SetString(SessionUsername, userLogin.Item1.UserName.ToString());

               // _notyfService.Success("Welcome back, " + userLogin.Item1.Email, 10);
                return RedirectToAction("Dashboard", "Home", new { area = "" });

            }
            else
            {
                _notyfService.Error(userLogin.Item2, 10);
                return View();
            }
           
        }
        public async Task<IActionResult> GetPrograms()
        {
            var programList = await _projectServices.GetPrograms();
            return Json(new SelectList(programList, "Id", "Name"));
            //var programList = await _projectServices.GetPrograms();
            //return Json(programList);//(new SelectList(programList, "Id", "Name"));
        }
        public async Task<IActionResult> GetStates(int CountryId)
        {
            var programList = await _projectServices.GetStatesByCountryId(CountryId);

            return Json(new SelectList(programList, "Id", "Name"));
        }
        //public async Task<IActionResult> ValidateCode(string Code, string Email)
        //{
        //    var programList = await _projectServices.GetMarketerCodeByCode(Code, Email);
        //    //var validCode = "";
        //    if (programList != null && (DateTime.Now.Date <= programList.ExpiryDate.Date))
        //    {
        //        // validCode = "Valid";
        //        GeneralClass.Email = Email;

        //        return RedirectToAction(nameof(Index));
        //    }
        //    return Json("Invalid");

        //}
        public async Task<IActionResult> ValidateCode(LoginVM dto)
        {
            GeneralClass.Role = dto.role;
            if (dto.role != "Marketing")
            {
                //return RedirectToAction(nameof(Index));
                //TempData["Role"] = dto.role;
                //var dtoVM = new RegisterVM();
                //dtoVM.dto = dto;
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else 
            {
                var programList = await _projectServices.GetMarketerCodeByCode(dto.code, dto.email);
                //var validCode = "";
                if (programList != null && (DateTime.Now.Date <= programList.ExpiryDate.Date))
                {
                    // validCode = "Valid";
                    GeneralClass.Email = dto.email;

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                return View("Login");
            }

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> ReferralUsage()
        {
            if (HttpContext.Session.GetString(SessionUsername) == null)
            {
                _notyfService.Error("Session time out", 5);
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            var sessionUsername = HttpContext.Session.GetString(SessionUsername);
            var user = await _userManager.FindByNameAsync(sessionUsername);
            if (user == null)
            {
                _notyfService.Error("Invalid user", 5);
                return RedirectToAction("Login", "Home", new { area = "" });

            }

            var response = await _projectServices.UserReferralCodeUsage(user.Email);
            return View(response);
        }
    }
}

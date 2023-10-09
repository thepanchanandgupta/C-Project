using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project1.Data;
using Project1.Models;
using Project1.Models.Domain;
using System.Runtime.CompilerServices;

namespace Project1.Controllers
{
    public class UsersController : Controller
    {
        private readonly IEmailSender _emailSender;

        private readonly MVCDemoDbContext mVCDemoDbContext;

        public UsersController(MVCDemoDbContext mVCDemoDbContext, IEmailSender emailSender)
        {
            this.mVCDemoDbContext = mVCDemoDbContext;
            this._emailSender = emailSender;

        }

        public JsonResult GetStates(int countryId)
        {
            IQueryable<State> query = mVCDemoDbContext.State;
            query = query.Where(x => x.Country_Id == countryId);

            //var states = mVCDemoDbContext.State.OrderBy(s => s.Country_Id == id).OrderBy(s => s.StateName).ToList();
            return new JsonResult(query.ToList());
        }

        public JsonResult GetCities(int stateId)
        {

            IQueryable<City> query = mVCDemoDbContext.City;
            query = query.Where(x => x.State_Id == stateId);

            //var cities = mVCDemoDbContext.City.OrderBy(s => s.State_Id == id).OrderBy(s => s.CityName).ToList();
            return new JsonResult(query.ToList());
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["RoleName"] = new SelectList(mVCDemoDbContext.Role, "Id", "RoleName");
            ViewData["CountryName"] = new SelectList(mVCDemoDbContext.Countries, "Id", "CountryName");
            ViewData["StateName"] = new SelectList(mVCDemoDbContext.State, "Id", "StateName");
            ViewData["CityName"] = new SelectList(mVCDemoDbContext.City, "Id", "CityName");
            return View();

        }

        [HttpGet]
        public IActionResult Thanks()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Add(UserViewModel addUserRequest)


        {
            ViewData["RoleName"] = new SelectList(mVCDemoDbContext.Role, "Id", "RoleName");
            ViewData["CountryName"] = new SelectList(mVCDemoDbContext.Countries, "Id", "CountryName");
            ViewData["StateName"] = new SelectList(mVCDemoDbContext.State, "Id", "StateName");
            ViewData["CityName"] = new SelectList(mVCDemoDbContext.City, "Id", "CityName");

            //Checking for if email already exists

            IQueryable<User> row_useremail = mVCDemoDbContext.Users;
            IQueryable<User> row_userphone = mVCDemoDbContext.Users;

            row_useremail = row_useremail.Where(u => u.Email.Equals(addUserRequest.Email));
            row_userphone = row_userphone.Where(u => u.PhoneNumber.Equals(addUserRequest.PhoneNumber));

            var flag_exists = false;

            if (row_useremail.Any())
            {
                ModelState.AddModelError("CustomErrorEmail", "Email already exists");
                flag_exists = true;

            }

            if (row_userphone.Any())
            {
                //backend validation for duplicate Ph. 
                ModelState.AddModelError("CustomErrorPhone", "Phone Number already in use with another account");
                flag_exists = true;

            }

            if (flag_exists)
            {
                return View(addUserRequest);

            }

            var password = RandomString(8);


            var user = new User()
            {
                FirstName = addUserRequest.FirstName,
                MiddleName = addUserRequest.MiddleName,
                LastName = addUserRequest.LastName,
                Gender = addUserRequest.Gender,
                Email = addUserRequest.Email,
                PhoneNumber = addUserRequest.PhoneNumber,
                Nationality = addUserRequest.Nationality,
                Role_Id = addUserRequest.Role_Id,
                Country_Id = addUserRequest.Country_Id,
                State_Id = addUserRequest.State_Id,
                City_Id = addUserRequest.City_Id,

                //We Have to Change Function to Create Random Password
                Password = password,

            };

            var receiver = addUserRequest.Email;
            var subject = "Account Password ";
            var message = " Your Password is " + password;

            await mVCDemoDbContext.Users.AddAsync(user);
            await mVCDemoDbContext.SaveChangesAsync();

            // await _emailSender.SendEmailAsync(receiver, subject, message);
            return RedirectToAction("Thanks");

        }

        //Password Generator
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

        }

    }
}

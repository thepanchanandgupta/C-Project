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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Project1.Controllers
{
    public class LoginController : Controller
    {
        private readonly MVCDemoDbContext mVCDemoDbContext;

        public LoginController(MVCDemoDbContext mVCDemoDbContext)
        {
            this.mVCDemoDbContext = mVCDemoDbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LoggedIN()
        {
            return RedirectToAction("UserList");
        }
        [HttpGet]
        public IActionResult LoggedInUser()
        {
            return RedirectToAction("UserListUser");
        }

        [HttpPost]
        public IActionResult Login(LoginUserViewModel loginUserRequest)
        {
            // Table context to search 
            IQueryable<User> row_user = mVCDemoDbContext.Users;

            //Searching in the database by email & phone number
            row_user = row_user.Where(u => u.Email.Equals(loginUserRequest.Email));
            Console.WriteLine(row_user);
            Console.WriteLine(row_user.First());
            if (row_user.IsNullOrEmpty())
            {
                ModelState.AddModelError("CustomError", "user not found");
                return View(loginUserRequest);
            }
            if (row_user.First().Password != loginUserRequest.Password)
            {
                ModelState.AddModelError("CustomError", "Password Not Matched");
                return View(loginUserRequest);
            }
            if (row_user.First().Password == loginUserRequest.Password)
            {
                if (row_user.First().Role_Id == 2)
                {
                    return RedirectToAction("LoggedIN");
                }
                return RedirectToAction("LoggedInUser");
            }
            
            
            ModelState.AddModelError("CustomError", "Something Went Wrong");
            return View(loginUserRequest);
        }

        [HttpGet]
        public async Task<IActionResult> UserList(string searchString)
        {
            var userList = await mVCDemoDbContext.Users.ToListAsync();

            if (mVCDemoDbContext == null)
            {
                return Problem("User List is Empty");
            }
            var users = mVCDemoDbContext.Users.Include(u=> u.Role).Include(c=>c.Country).Include(s=>s.State).Include(ct=>ct.City);
            if (!String.IsNullOrEmpty(searchString))
            {
                IQueryable<User> query = mVCDemoDbContext.Users;
                query = query.Where(
                    s => s.Email.Contains(searchString)
                    || s.PhoneNumber.Contains(searchString)
                    || s.Nationality.Contains(searchString)
                    || s.FirstName.Contains(searchString)
                    ).Include(u => u.Role).Include(c => c.Country).Include(s => s.State).Include(ct => ct.City);
                return View(query.ToList());
            }
            return View(users.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> UserListUser(string searchString)
        {
            var userListUser = await mVCDemoDbContext.Users.ToListAsync();


            if (mVCDemoDbContext == null)
            {
                return Problem("User List is Empty");
            }

            var users = mVCDemoDbContext.Users.Include(u => u.Role).Include(c => c.Country).Include(s => s.State).Include(ct => ct.City);
            if (!String.IsNullOrEmpty(searchString))
            {
                IQueryable<User> query = mVCDemoDbContext.Users;
                query = query.Where(
                    s => s.Email.Contains(searchString)
                    || s.PhoneNumber.Contains(searchString)
                    || s.Nationality.Contains(searchString)
                    || s.FirstName.Contains(searchString)
                    ).Include(u => u.Role).Include(c => c.Country).Include(s => s.State).Include(ct => ct.City);
                return View(query.ToList());
            }

            return View(users.ToList());

        }


        [HttpGet]
        public async Task<IActionResult> ViewUser(int Id)
        {
            ViewData["RoleName"] = new SelectList(mVCDemoDbContext.Role, "Id", "RoleName");
            ViewData["CountryName"] = new SelectList(mVCDemoDbContext.Countries, "Id", "CountryName");
            ViewData["StateName"] = new SelectList(mVCDemoDbContext.State, "Id", "StateName");
            ViewData["CityName"] = new SelectList(mVCDemoDbContext.City, "Id", "CityName");

            var user = await mVCDemoDbContext.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (user != null)
            {
                var viewModel = new UpdateUserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Nationality = user.Nationality,
                    Role_Id = user.Role_Id,
                    Country_Id = (int)user.Country_Id,
                    State_Id = (int)user.State_Id,
                    City_Id = (int)user.City_Id

                };

                return await Task.Run(() => View("ViewUser", viewModel));
            }
            else if (user.Role_Id == 2)
            {
                return RedirectToAction("UserList");
            }
            else if (user.Role_Id == 1)
            {

                return RedirectToAction("UserListUser");
            }
            return RedirectToAction("ViewUser");
        }

        [HttpPost]
        public async Task<IActionResult> ViewUser(UpdateUserViewModel updatemodel)
        {
            var user = await mVCDemoDbContext.Users.FindAsync(updatemodel.Id);

            if (user != null)
            {
                user.FirstName = updatemodel.FirstName;
                user.MiddleName = updatemodel.MiddleName;
                user.LastName = updatemodel.LastName;
                user.Gender = updatemodel.Gender;
                user.Email = updatemodel.Email;
                user.Gender = updatemodel.Gender;
                user.PhoneNumber = updatemodel.PhoneNumber;
                user.Role_Id = updatemodel.Role_Id;
                user.Country_Id = updatemodel.Country_Id;
                user.State_Id = updatemodel.State_Id;
                user.City_Id = updatemodel.City_Id;


                if (mVCDemoDbContext.Users.First().Role_Id == 2)
                {
                    return RedirectToAction("UserList");
                }
                else if (mVCDemoDbContext.Users.First().Role_Id == 1)
                {
                    return RedirectToAction("UserListUser");
                }

            }

            return RedirectToAction("ViewUser");

        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int Id)
        {
            ViewData["RoleName"] = new SelectList(mVCDemoDbContext.Role, "Id", "RoleName");
            ViewData["CountryName"] = new SelectList(mVCDemoDbContext.Countries, "Id", "CountryName");
            ViewData["StateName"] = new SelectList(mVCDemoDbContext.State, "Id", "StateName");
            ViewData["CityName"] = new SelectList(mVCDemoDbContext.City, "Id", "CityName");


            var user = await mVCDemoDbContext.Users.FirstOrDefaultAsync(u => u.Id == Id);

            if (user != null)
            {
                var editModel = new UpdateUserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Nationality = user.Nationality,
                    Role_Id = user.Role_Id,
                    Country_Id = (int)user.Country_Id,
                    State_Id = (int)user.State_Id,
                    City_Id = (int)user.City_Id
                };

                return await Task.Run(() => View("EditUser", editModel));

            }

            return RedirectToAction("EditUser");


        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UpdateUserViewModel updatemodel)
        {
            var user = await mVCDemoDbContext.Users.FindAsync(updatemodel.Id);

            if (user != null)
            {
                user.FirstName = updatemodel.FirstName;
                user.MiddleName = updatemodel.MiddleName;
                user.LastName = updatemodel.LastName;
                user.Gender = updatemodel.Gender;
                user.Email = updatemodel.Email;
                user.Gender = updatemodel.Gender;
                user.PhoneNumber = updatemodel.PhoneNumber;
                user.Role_Id = updatemodel.Role_Id;
                user.Country_Id = updatemodel.Country_Id;
                user.State_Id = updatemodel.State_Id;
                user.City_Id = updatemodel.City_Id;

                await mVCDemoDbContext.SaveChangesAsync();
                return RedirectToAction("UserList");

            }

            return RedirectToAction("EditUser");

        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            ViewData["RoleName"] = new SelectList(mVCDemoDbContext.Role, "Id", "RoleName");
            ViewData["CountryName"] = new SelectList(mVCDemoDbContext.Countries, "Id", "CountryName");
            ViewData["StateName"] = new SelectList(mVCDemoDbContext.State, "Id", "StateName");
            ViewData["CityName"] = new SelectList(mVCDemoDbContext.City, "Id", "CityName");

            var user = await mVCDemoDbContext.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (user != null)
            {
                var deleteModel = new UpdateUserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    MiddleName = user.MiddleName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Nationality = user.Nationality,
                    Role_Id = user.Role_Id,
                    Country_Id = (int)user.Country_Id,
                    State_Id = (int)user.State_Id,
                    City_Id = (int)user.City_Id


                };
                return await Task.Run(() => View("DeleteUser", deleteModel));

            }

            return RedirectToAction("UserList");

        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(UpdateUserViewModel updatemodel)
        {
            var user = await mVCDemoDbContext.Users.FindAsync(updatemodel.Id);

            if (user != null)
            {

                mVCDemoDbContext.Users.Remove(user);
                await mVCDemoDbContext.SaveChangesAsync();
                return RedirectToAction("UserList");

            }

            return RedirectToAction("DeleteUser");

        }


    }
}

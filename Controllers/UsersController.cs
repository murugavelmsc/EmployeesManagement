using EmployeesManagement.Data;
using EmployeesManagement.Models;
using EmployeesManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;                
        }
        public async Task<ActionResult> Index()                   
        {
            var users = await _context.Users.Include(x=> x.Role).ToListAsync();
            return View(users);
        }
        [HttpGet]
        public async Task<ActionResult> Create()
        {            
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(UserViewModel userViewModel)
        {
            ApplicationUser user = new ApplicationUser();
            user.UserName = userViewModel.UserName;
            user.FirstName = userViewModel.FirstName;
            user.MiddleName = userViewModel.MiddleName;
            user.LastName = userViewModel.LastName;
            user.NationalId = userViewModel.NationalId;
            user.NormalizedUserName = userViewModel.UserName.Trim();
            user.Email = userViewModel.Email;
            user.EmailConfirmed = true;
            user.PhoneNumber = userViewModel.PhoneNumber;
            user.PhoneNumberConfirmed = true;
            user.CreatedById = "Admin";
            user.CreatedOn = DateTime.Now;
            user.RoleId = userViewModel.RoleId;

            var result = await _userManager.CreateAsync(user, userViewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(userViewModel);
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", userViewModel.RoleId);

        }
    }
}

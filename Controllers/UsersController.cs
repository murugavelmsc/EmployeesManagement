using EmployeesManagement.Data;
using EmployeesManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;                
        }
        public async Task<ActionResult> Index()                   
        {
            var users = await _context.Users.ToListAsync();

            return View(users);
        }
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            //var users = await _context.Users.ToListAsync();

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(UserViewModel userViewModel)
        {
            IdentityUser user = new IdentityUser();
            user.UserName = userViewModel.UserName;
            user.NormalizedUserName = userViewModel.UserName.Trim();
            user.Email = userViewModel.Email;
            user.EmailConfirmed = true;
            user.PhoneNumber = userViewModel.PhoneNumber;
            user.PhoneNumberConfirmed = true;

            var result = await _userManager.CreateAsync(user, userViewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(userViewModel);
            }

            
        }
    }
}

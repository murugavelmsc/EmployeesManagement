using EmployeesManagement.Data;
using EmployeesManagement.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManagement.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<ActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            //var users = await _context.Users.ToListAsync();

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(RolesViewModel rolesViewModel)
        {
            IdentityRole role = new IdentityRole();
            role.Name = rolesViewModel.RoleName;


            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(rolesViewModel);
            }
        }
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var role = new RolesViewModel();
            var result = await _roleManager.FindByIdAsync(id);
            role.RoleName = result.Name;
            role.Id = result.Id;

            return View(role);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, RolesViewModel rolesViewModel)
        {
            var checkifexists = await _roleManager.RoleExistsAsync(rolesViewModel.Id);

            if (checkifexists)
            {
                var result = await _roleManager.FindByIdAsync(id);
                result.Name = rolesViewModel.RoleName;

                var finalresult = await _roleManager.UpdateAsync(result);

                if (finalresult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(rolesViewModel);
                }
            }
        }
    }
}

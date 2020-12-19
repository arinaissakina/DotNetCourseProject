using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetFinalProject.Data;
using DotNetFinalProject.Models;
using DotNetFinalProject.Services;
using DotNetFinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace DotNetFinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ProjectService _projectService;
        private readonly CourseProjectContext _context;
 
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            CourseProjectContext context, ProjectService projectService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _projectService = projectService;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Name= model.Name};
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Main");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = 
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Main");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Main");
        }
        
        public async Task<IActionResult> MyProjects()
        {
            var userEmail = User.Identity.Name;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            IEnumerable<MyProjectsViewModel> q2 = (from b in _context.Projects 
                join ub in _context.ProjectMembers on b.Id equals ub.ProjectId
                join a in _context.Users on b.OwnerId equals a.Id
                where ub.MemberId == user.Id
                select new MyProjectsViewModel{ ProjectId = b.Id, ProjectName = b.Name, ProjectOwner = a.Name}).ToList();
            return View("MyProjects", q2);
        }
        
        public async Task<IActionResult> Profile()
        {
            var userEmail = User.Identity.Name;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            IEnumerable<ProfileViewModel> q2 = (from b in _context.Specialties
                join ub in _context.SpecialityUsers on b.Id equals ub.SpecialtyId
                where ub.UserId == user.Id
                select new ProfileViewModel {SpecialtyName = b.Name}).ToList();
                                                                                            
            return View("Profile", q2);
        }
        
        //[HttpGet]
        //[Route("/Account/EditSpecialties/{id}")]
        public async Task<IActionResult> EditSpecialties()
        {
            var userEmail = User.Identity.Name;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            var specialties = await _context.Specialties.ToListAsync();
            var specialtyUsers = await _context.SpecialityUsers.Where(b => b.UserId == user.Id).ToListAsync();
            EditSpecialtyViewModel model = new EditSpecialtyViewModel() { User = user, Specialties = specialties, SpecialtyUsers = specialtyUsers};
            return View(model);
        }
        
        
        [HttpGet]
        [Route("/Account/AddSpecialty/{specId}")]
        public async Task<IActionResult> AddSpecialty(long specId, long id)
        {
            var userEmail = User.Identity.Name;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            
            var specialty = await _context.Specialties.FirstOrDefaultAsync(c => c.Id == specId);
            var specialtyUser = await _context.SpecialityUsers.FirstOrDefaultAsync(b => b.UserId == user.Id && b.SpecialtyId == specId);
            
            if(specialtyUser == null)
            {
                _context.SpecialityUsers.Add(new SpecialtyUser {UserId = user.Id, SpecialtyId = specId });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("EditSpecialties", new RouteValueDictionary(
                new { controller = "Account", action = "EditSpecialties", Id = user.Id }));
        }
        
        [HttpGet]
        [Route("/Account/RemoveSpecialty/{specId}")]
        public async Task<IActionResult> RemoveSpecialty(long specId, long id)
        {
            var userEmail = User.Identity.Name;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            var specialtyUser = await _context.SpecialityUsers.FirstOrDefaultAsync(b => b.UserId == user.Id && b.SpecialtyId == specId);
            
            if (specialtyUser != null)
            {
                _context.SpecialityUsers.Remove(specialtyUser);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("EditSpecialties", new RouteValueDictionary(
                new { controller = "Account", action = "EditCategory", Id = specId }));
        }
        
    }
}
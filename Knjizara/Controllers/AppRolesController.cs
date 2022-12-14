using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Knjizara.Data;
using Knjizara.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Knjizara.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Knjizara.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;


        public AppRolesController(ApplicationDbContext context, RoleManager<AppRole> roleManager,UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager; 
        }

        // GET: AppRoles
        public async Task<IActionResult> Index()
        {
              return _context.Roles != null ? 
                          View(await _context.Roles.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Roles'  is null.");
        }

        // GET: AppRoles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var appRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appRole == null)
            {
                return NotFound();
            }

            return View(appRole);
        }

        // GET: AppRoles/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: AppRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Name")] AppRole appRole)
        {
            if (ModelState.IsValid)
            {
                appRole.Id = Guid.NewGuid();
                appRole.NormalizedName = appRole.Name.Trim().ToUpper();
                await _roleManager.CreateAsync(appRole);
                return RedirectToAction(nameof(Index));

            }
            return View(appRole);
        }

        // GET: AppRoles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var appRole = await _context.Roles.FindAsync(id);
            if (appRole == null)
            {
                return NotFound();
            }
            return View(appRole);
        }

        // POST: AppRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Description,Id,Name,NormalizedName,ConcurrencyStamp")] AppRole appRole)
        {
            if (id != appRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppRoleExists(appRole.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(appRole);
        }

        // GET: AppRoles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var appRole = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appRole == null)
            {
                return NotFound();
            }

            return View(appRole);
        }

        // POST: AppRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Roles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Roles'  is null.");
            }
            var appRole = await _context.Roles.FindAsync(id);
            if (appRole != null)
            {
                _context.Roles.Remove(appRole);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppRoleExists(Guid id)
        {
          return (_context.Roles?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> AddAdmin(Guid? id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            await _userManager.AddToRoleAsync(user, "Admin");

            return RedirectToAction(nameof(UsersRolesEdit));


        }
        public async Task<IActionResult> RemoveAdmin(Guid? id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            await _userManager.RemoveFromRoleAsync(user, "Admin");

            return RedirectToAction(nameof(UsersRolesEdit));


        }
        public async Task<IActionResult> UsersRolesEdit(string? searchString)
        {
            List<AppUser> users = new List<AppUser>();

            if (String.IsNullOrWhiteSpace(searchString))
            {
                users = await _userManager.Users.ToListAsync();
            }
            else
            {
                //users = _context.Users.Where(u => u.FirstName.Contains(searchString) || u.LastName.Contains(searchString)
                users = await _userManager.Users.Where(u=>u.Email.Contains(searchString)).ToListAsync();
            }

            var userRolesViewModel = new List<RolesUsersViewModel>();
            foreach (AppUser user in users)
            {
                var thisViewModel = new RolesUsersViewModel();
                thisViewModel.Email = user.Email;
                thisViewModel.UserId = user.Id;

                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }
            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(AppUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
    }
}

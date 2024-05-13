using AdminPanal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanal.Controllers
{
    public class RoleController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var role = await _roleManager.Roles.ToListAsync();
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExsit = await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExsit)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
                    return RedirectToAction("Index", await _roleManager.Roles.ToListAsync());
                }
                else
                {
                    //ViewBag.Name = "Role is Exists";
                    ModelState.AddModelError("Name", "Role is Exists");
                    return View(nameof(Index));
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var mapRole = new RoleViewModel
            {
                Name = role.Name
            };
            return View(mapRole);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExists)
                {

                    role.Name = model.Name;
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                else
                {

                    ModelState.AddModelError("Name", "Role is Exists");
                    return View(nameof(Index), await _roleManager.Roles.ToListAsync());

                }


            }
            return RedirectToAction(nameof(Index));

        }

    }
}

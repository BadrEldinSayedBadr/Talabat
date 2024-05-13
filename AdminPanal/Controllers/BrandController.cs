using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;

namespace AdminPanal.Controllers
{
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> Index()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return View(brands);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductBrand productBrand)
        {
            try
            {
                await _unitOfWork.Repository<ProductBrand>().Add(productBrand);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("Name", "Please enter a new brad");
                return View("Index", await _unitOfWork.Repository<ProductBrand>().GetAllAsync());
            }
        }


        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);
            _unitOfWork.Repository<ProductBrand>().Delete(brand);
            await _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

    }
}

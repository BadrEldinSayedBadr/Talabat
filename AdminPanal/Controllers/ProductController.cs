using AdminPanal.Helpers;
using AdminPanal.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Interfaces;
using Talabat.Core.Specifications;

namespace AdminPanal.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index([FromQuery] ProductSpecParams productSpecParams)
        {
            var spec = new ProductSpecification(productSpecParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            var mappedProduct = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);
            return View(mappedProduct);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    model.PictureUrl = PictuerSettings.UploadImage(model.Image, "products");
                }
                else
                    model.PictureUrl = "images/products/hat-react2.png";


                var mapedProduct = _mapper.Map<ProductViewModel, Product>(model);
                await _unitOfWork.Repository<Product>().Add(mapedProduct);
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            var mappedProducts = _mapper.Map<Product, ProductViewModel>(product);
            return View(mappedProducts);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, IFormFile Image, ProductViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    if (model.PictureUrl != null)
                    {
                        PictuerSettings.DeleteFile(model.PictureUrl, "products");
                        model.PictureUrl = PictuerSettings.UploadImage(model.Image, "products");
                    }
                    else
                    {
                        model.PictureUrl = PictuerSettings.UploadImage(model.Image, "products");
                    }
                }
                else
                {
                    model.Image = Image;
                }
                var mappedProduct = _mapper.Map<ProductViewModel, Product>(model);
                _unitOfWork.Repository<Product>().Update(mappedProduct);
                var result = await _unitOfWork.Complete();
                if (result > 0)
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            var mappedProducts = _mapper.Map<Product, ProductViewModel>(product);
            return View(mappedProducts);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, ProductViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                var prodct = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
                if (prodct.PictureUrl != null)
                {
                    PictuerSettings.DeleteFile(prodct.PictureUrl, "products");
                }
                _unitOfWork.Repository<Product>().Delete(prodct);
                await _unitOfWork.Complete();
                return RedirectToAction("Index");

            }
            catch (System.Exception)
            {
                return View(model);
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ProductsCategories.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductsCategories.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _context;

        public HomeController(MyContext context) 
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.AllProducts = _context.Products
            .ToList();
            return View();
        }
    //----------------------------------------------
        [HttpGet("products")]
        public IActionResult Products()
        {
            ViewBag.AllProducts = _context.Products
            .OrderBy(p => p.ProductId)
            .ToList();
            return View("Products");
        }
//------------------------------------------------------------
//--------------------------------------------------------------
        [HttpPost("create-product-process")]
        public IActionResult CreateProductProcess(Product newProduct)
        {
            _context.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("Index");
            // return RedirectToAction("SingleCategory", addProd);
        }
        
//------------------------------------------------------------
//------------------------------------------------------------
//------------------------------------------------------------
        [HttpGet("products/{prodId}")]
        public IActionResult SingleProduct(int prodId)
        {

            ViewBag.CategoryInProd = _context.Products
                .Include(p => p.Categories)
                    .ThenInclude(a => a.Category)
                .FirstOrDefault(p => p.ProductId == prodId);

            ViewBag.AllCategories = _context.Categorys
                .OrderBy(p => p.CategoryId)
                .Where(a => a.Products.All(b => b.ProductId != prodId))
                .ToList();
            return View();
        }

        //--------------------------------------------------
        [HttpPost("create-category-process")]
        public IActionResult CreateCategoryProcess(Category addCat)
        {
            _context.Add(addCat);

            _context.SaveChanges();

            
            return Redirect($"/categories/{addCat.CategoryId}");

        }
        //-------------------------------------------------------

        [HttpGet("categories")]
        public IActionResult Categories()
        {
            ViewBag.AllCategories = _context.Categorys
            .OrderBy(p => p.CategoryId)
            .ToList();

            return View();
        }
//-------------------------------------------------------------------
        [HttpPost("add-product")]
        public IActionResult AddProduct(Association addedProd)
        {
            _context.Associations.Add(addedProd);
            _context.SaveChanges();
            Console.WriteLine(addedProd);
            return Redirect($"/categories/{addedProd.CategoryId}");
            // return RedirectToAction("CategoryDetail", addedProd)
        }

//-----------------------------------------------------------------------
        [HttpPost("add-category")]
        public IActionResult AddCat(Association addedCat)
        {
            _context.Associations.Add(addedCat);
            _context.SaveChanges();
            
            return Redirect($"/products/{addedCat.ProductId}");
        }
//-----------------------------------------------------------------------
        [HttpGet("categories/{categoryId}")]
        public IActionResult SingleCategory(int categoryId)
        {
            ViewBag.ProductWithCategory = _context.Categorys
                .Include(prod => prod.Products)
                    .ThenInclude(a => a.Product)
                .FirstOrDefault(prod => prod.CategoryId == categoryId);
            
            ViewBag.AllProducts = _context.Products
                .Include(c => c.Categories)
                    .ThenInclude(p => p.Product)
                .OrderBy(p => p.ProductId)
                .Where(a => a.Categories.All(b => b.CategoryId != categoryId))
                .ToList();

            return View();
        }
//------------------------------------------------------------------

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}
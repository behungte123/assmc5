using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;
using System.Security.Claims;

namespace Lab4.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Tiêm DbContext vào để làm việc với database thật [1]
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.SortOrder)
                .ToListAsync();

            return View(products);
        }
        public async Task<IActionResult> Menu()
        {
            var products = await _context.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.SortOrder)
                .ToListAsync();

            return View(products);
        }
    }
    
    }
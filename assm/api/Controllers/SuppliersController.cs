using lab4.Models;
using lab4.Models.ViewModels;
using Lab4.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab4.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuppliersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Suppliers.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (!ModelState.IsValid) return View(supplier);

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // ================= SET PRODUCT =================

        public async Task<IActionResult> SetProducts(int id)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.SupplierProducts)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null) return NotFound();

            // 1️⃣ Lấy danh sách ProductId đã gán (in-memory list)
            var assignedProductIds = supplier.SupplierProducts
                .Select(sp => sp.ProductId)
                .ToList();

            // 2️⃣ Query Products (EF dịch được)
            var products = await _context.Products
                .Select(p => new ProductItem
                {
                    ProductId = p.Id,
                    Name = p.Name,
                    Selected = assignedProductIds.Contains(p.Id)
                })
                .ToListAsync();

            var vm = new AssignProductsVM
            {
                SupplierId = supplier.Id,
                SupplierName = supplier.Name,
                Products = products
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SetProducts(AssignProductsVM vm)
        {
            var supplier = await _context.Suppliers
                .Include(s => s.SupplierProducts)
                .FirstAsync(s => s.Id == vm.SupplierId);

            supplier.SupplierProducts.Clear();

            foreach (var p in vm.Products.Where(x => x.Selected))
            {
                supplier.SupplierProducts.Add(new SupplierProduct
                {
                    SupplierId = supplier.Id,
                    ProductId = p.ProductId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}

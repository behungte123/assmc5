using Lab4.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab4.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // ===== KPI =====
            var completedOrders = _context.Orders
                .Where(o => o.Status == "Completed");

            var pendingOrders = _context.Orders
                .Where(o => o.Status != "Completed" && o.Status != "Cancelled");

            var totalRevenue = completedOrders.Sum(o => (decimal?)o.Total) ?? 0;
            var expectedRevenue = pendingOrders.Sum(o => (decimal?)o.Total) ?? 0;

            var totalOrders = _context.Orders.Count();
            var completedCount = completedOrders.Count();

            var totalInventory = _context.Inventories.Sum(i => i.Quantity);

            // ===== Revenue by day (CHỈ ĐƠN HOÀN TẤT) =====
            var revenueByDay = completedOrders
     .GroupBy(o => o.CreatedAt.Date)
     .Select(g => new
     {
         Date = g.Key,
         Total = g.Sum(x => x.Total)
     })
     .OrderBy(x => x.Date)
     .ToList()   // ⬅️ CHỐT QUERY TẠI ĐÂY
     .Select(x => new
     {
         date = x.Date.ToString("dd/MM"), // ⬅️ FORMAT SAU KHI RA RAM
         total = x.Total
     })
     .ToList();

            // ===== Top products (CHỈ ĐƠN HOÀN TẤT) =====
            var topProducts = _context.OrderItems
                .Where(i => i.Order.Status == "Completed")
                .GroupBy(i => i.ProductName)
                .Select(g => new
                {
                    product = g.Key,
                    quantity = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.quantity)
                .Take(5)
                .ToList();

            // ===== Inventory =====
            var inventories = _context.Inventories
                .Select(i => new
                {
                    product = i.Product.Name,
                    quantity = i.Quantity
                })
                .ToList();

            // ===== ViewBag =====
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.ExpectedRevenue = expectedRevenue;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.CompletedOrders = completedCount;
            ViewBag.TotalInventory = totalInventory;

            ViewBag.RevenueByDay = revenueByDay;
            ViewBag.TopProducts = topProducts;
            ViewBag.Inventories = inventories;

            return View();
        }
    }
}
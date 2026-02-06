using Lab4.Data;
using Microsoft.AspNetCore.Mvc;

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
            // KPI
            var totalRevenue = _context.Orders.Sum(o => o.Total);
            var totalOrders = _context.Orders.Count();
            var avgOrder = totalOrders == 0 ? 0 : totalRevenue / totalOrders;
            var totalInventory = _context.Inventories.Sum(i => i.Quantity);


            // Revenue by day
            var revenueByDay = _context.Orders
            .GroupBy(o => o.CreatedAt.Date)
            .Select(g => new
            {
                Date = g.Key,
                Total = g.Sum(x => x.Total)
            })
            .OrderBy(x => x.Date)
            .ToList();


            // Top products
            var topProducts = _context.OrderItems
            .GroupBy(i => i.ProductName)
            .Select(g => new
            {
                Product = g.Key,
                Quantity = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.Quantity)
            .Take(5)
            .ToList();


            // Inventory
            var inventories = _context.Inventories
            .Select(i => new
            {
                Product = i.Product.Name,
                Quantity = i.Quantity
            }).ToList();


            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.AvgOrder = avgOrder;
            ViewBag.TotalInventory = totalInventory;
            ViewBag.RevenueByDay = revenueByDay;
            ViewBag.TopProducts = topProducts;
            ViewBag.Inventories = inventories;


            return View();
        }
    }
}

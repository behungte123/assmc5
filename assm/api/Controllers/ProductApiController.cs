using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;

namespace Lab4.Controllers.Api
{
    [ApiController]
    [Route("api/products")]
    public class ProductApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var products = await _context.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.SortOrder)
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/products/menu
        [HttpGet("menu")]
        public IActionResult GetMenu()
        {
            var data = _context.Products
                .Where(p => p.IsActive)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    PriceVnd = p.PriceVnd,
                    PriceText = p.PriceText,
                    ImageUrl = p.ImageUrl,
                    Quantity = p.Inventory.Quantity
                })
                .OrderBy(p => p.Id)
                .ToList();

            return Ok(data);
        }

        // GET: api/products/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> Search(
            string? query,
            int? minPrice,
            int? maxPrice,
            string? sortBy = "default")
        {
            var productsQuery = _context.Products.Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(query))
            {
                var lower = query.ToLower();
                productsQuery = productsQuery.Where(p =>
                    p.Name.ToLower().Contains(lower) ||
                    (p.Description != null && p.Description.ToLower().Contains(lower)));
            }

            if (minPrice.HasValue && minPrice > 0)
                productsQuery = productsQuery.Where(p => p.PriceVnd >= minPrice);

            if (maxPrice.HasValue && maxPrice > 0)
                productsQuery = productsQuery.Where(p => p.PriceVnd <= maxPrice);

            productsQuery = sortBy switch
            {
                "price_asc" => productsQuery.OrderBy(p => p.PriceVnd),
                "price_desc" => productsQuery.OrderByDescending(p => p.PriceVnd),
                "name_asc" => productsQuery.OrderBy(p => p.Name),
                "name_desc" => productsQuery.OrderByDescending(p => p.Name),
                _ => productsQuery.OrderBy(p => p.SortOrder)
            };

            return Ok(await productsQuery.ToListAsync());
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _context.Products
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/products/{id} (soft delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/products/{id}/toggle-active
        [HttpPost("{id}/toggle-active")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.IsActive = !product.IsActive;
            await _context.SaveChangesAsync();

            return Ok(product);
        }
    }
}
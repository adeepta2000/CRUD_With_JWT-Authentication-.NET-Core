using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _context;
        public ProductController(ProductContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet]
       
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            return await _context.Products.ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
       
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return product;
        }
        [Authorize]
        [HttpPost]
        
        public async Task<ActionResult<Product>> AddProduct(Product p)
        {
            _context.Products.Add(p);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = p.Id }, p);
        }
        [Authorize]
        [HttpPut("{id}")]
       
        public async Task<ActionResult> EditProduct(int id, Product p)
        {
            if(id != p.Id)
            {
                return BadRequest();
            }
            _context.Entry(p).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) 
            {
                throw;
            }

            return Ok();

        }
        [Authorize]
        [HttpDelete("{id}")]
       
        public async Task<ActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using productservice.Data;
using productservice.Model;

namespace productservice.Controllers
{
    [Route("api/products/'childrenproducts/")]
    public class ChildrenProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChildrenProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{parentId}")]
        [ResponseCache(CacheProfileName = "productscache")]
        public async Task<IActionResult> Get(int parentId)
        {
            var parentThing = await _context.Products
            .Where(x => x.productId == parentId)
            .FirstOrDefaultAsync();

            if (parentThing != null)
            {
                var things = await _context.Products
                .Where(x => parentThing.childrenProductsIds.Contains(x.productId))
                .ToListAsync();

                return Ok(things);
            }
            return NotFound();
        }

        [HttpPost("{parentId}")]
        public async Task<IActionResult> Post(int parentId, [FromBody]Product product)
        {
            var parentProduct = await _context.Products
            .Where(x => x.productId == parentId)
            .FirstOrDefaultAsync();

            var childrenProduct = await _context.Products
            .Where(x => x.productId == product.productId)
            .FirstOrDefaultAsync();

            if (parentProduct != null && childrenProduct != null)
            {
                if (parentProduct.childrenProductsIds == null)
                    parentProduct.childrenProductsIds = new int[0];

                if (parentProduct.childrenProductsIds.Contains(product.productId))
                    ModelState.AddModelError("ChildrenError", "This thing is already in this parent");

                if (ModelState.IsValid)
                {
                    parentProduct.childrenProductsIds = parentProduct.childrenProductsIds.Append(product.productId).ToArray();

                    if (childrenProduct.parentProducId != null)
                    {
                        var oldParentProduct = await _context.Products
                        .Where(x => x.productId == parentId)
                        .FirstOrDefaultAsync();

                        if (oldParentProduct != null)
                            oldParentProduct.childrenProductsIds = oldParentProduct.childrenProductsIds.Where(val => val != product.productId).ToArray();
                    }
                    childrenProduct.parentProducId = parentId;
                    await _context.SaveChangesAsync();
                    return Created($"api/products/{parentProduct.productId}", parentProduct);
                }
                return BadRequest(ModelState);
            }
            return NotFound();
        }

        [HttpDelete("{parentId}")]
        public async Task<IActionResult> Delete(int parentId, [FromBody]Product product)
        {
            var parentProduct = await _context.Products.Where(x => x.productId == parentId).FirstOrDefaultAsync();
            var childrenProduct = await _context.Products.Where(x => x.productId == product.productId).FirstOrDefaultAsync();

            if (parentProduct != null && childrenProduct != null)
            {
                if (!parentProduct.childrenProductsIds.Contains(product.productId) || childrenProduct.parentProducId != parentId)
                    ModelState.AddModelError("ChildrenError", "This thing is not in this parent");

                if (ModelState.IsValid)
                {
                    parentProduct.childrenProductsIds = parentProduct.childrenProductsIds.Where(val => val != product.productId).ToArray();
                    childrenProduct.parentProducId = null;
                    await _context.SaveChangesAsync();
                    return Created($"api/products/{parentProduct.productId}", parentProduct);
                }
                return BadRequest(ModelState);

            }
            return NotFound();
        }
    }

}
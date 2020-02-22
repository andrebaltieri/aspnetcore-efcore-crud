using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Store.Data;

namespace Shop.Controllers
{
    [ApiController]
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> Get(
            [FromServices]StoreDataContext context
        )
        {
            var products = await context.Products.AsNoTracking().ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> Post(
            [FromServices]StoreDataContext context,
            [FromBody]Product product
        )
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            return Created("", product);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult> Put(
            [FromServices]StoreDataContext context,
            [FromBody]Product product,
            int id
        )
        {
            var item = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
                return NotFound();

            context.Entry<Product>(product).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return Ok(item);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult> Delete(
            [FromServices]StoreDataContext context,
            int id
        )
        {
            var item = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
                return NotFound();

            context.Products.Remove(item);
            await context.SaveChangesAsync();

            return Ok(item);
        }
    }
}

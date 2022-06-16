using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniShopApp.Business.Abstract;
using MiniShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShopApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products=await _productService.GetAll();
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetById(id);
            if (product==null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {
            var product= await _productService.CreateAsync(entity);
            return CreatedAtAction("GetProduct",new { id=entity.ProductId },entity);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id,Product entity)
        {
            if (id!=entity.ProductId)
            {
                return BadRequest();
            }
            var product = await _productService.GetById(id);
            if (product==null)
            {
                return NotFound();
            }
            await _productService.UpdateProductAsync(product, entity);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            
            var product = await _productService.GetById(id);
            if (product==null)
            {
                return NotFound();
            }
            await _productService.DeleteProductAsync(product);
            return NoContent();
        }
    }
}

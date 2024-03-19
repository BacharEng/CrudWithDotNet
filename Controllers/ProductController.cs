using Microsoft.AspNetCore.Mvc;
using CrudWithDotnet.Models;
using CrudWithDotnet.Services;

namespace CrudWithDotnet.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAllProducts")]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            return Ok(_productService.GetAll());
        }

        [HttpGet("GetProductById/{id}")]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = _productService.GetById(id);
            if(product == null){
                return NotFound(new { Message = $"Product ID {id} not found" });
            }
            return product;
        }

        [HttpPost("addNewProduct")]
        public ActionResult addNewProduct([FromBody] Product product)
        {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            _productService.AddProduct(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [HttpDelete("deleteProductById/{id}")]
        public ActionResult deleteProductById(int id)
        {
            if(_productService.GetById(id) == null){
                return NotFound(new { Message = $"Product ID {id} not found" });
            }

            _productService.deleteProduct(id);
            return NoContent();
        }

        [HttpPut("updateProductById")]
        public ActionResult updateProductById([FromBody] Product productUpdated)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(_productService.GetById(productUpdated.Id) == null)
            {
                return BadRequest(new { Message = $"Product with ID {productUpdated.Id} not found"});
            }

            _productService.updateProduct(productUpdated);
            return Ok(new {UpdatedProduct = productUpdated});
        }
    }

}
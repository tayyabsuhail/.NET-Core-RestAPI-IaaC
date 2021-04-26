using AutoMapper;
using Catalog.API.DTO;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public CatalogController(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/GetDeployment")]
        public string GetDeployment()
        {
            return "Continuous Delivery";
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var products = await _repository.GetAllAsync();
            if (products == null)
                return NoContent();
            return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
        }

        [HttpGet]
        [Route("{action}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpGet]
        [Route("{action}/{category}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _repository.getProductByCategory(category);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto product)
        {
            Product productItem = _mapper.Map<Product>(product);
            var createdProduct = await _repository.AddAsync(productItem);
            return Ok(_mapper.Map<ProductDto>(createdProduct));
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]

        public async Task<ActionResult> UpdateProduct([FromBody] ProductDto product)
        {
            var productInfo = await _repository.GetByIdAsync(product.Id);
            if (productInfo == null)
                return NotFound();
            Product productItem = _mapper.Map<Product>(product);
            await _repository.UpdateAsync(productItem);
            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            var productInfo = await _repository.GetByIdAsync(id);
            if (productInfo == null)
                return NotFound();
            await _repository.DeleteAsync(productInfo);
            return NoContent();
        }
    }
}

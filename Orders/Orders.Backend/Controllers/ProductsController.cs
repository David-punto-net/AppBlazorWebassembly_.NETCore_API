using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ProductsController : GenericController<Product>
    {
        private readonly IProductsUnitOfWork _productsUnitOfWork;

        public ProductsController(IGenericUnitsOfWork<Product> unitsOfWork, IProductsUnitOfWork productsUnitOfWork) : base(unitsOfWork)
        {
            _productsUnitOfWork = productsUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _productsUnitOfWork.GetAsync(pagination);
            if (response.WassSuccees)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalRecord")]
        public override async Task<IActionResult> GetTotalRecordAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _productsUnitOfWork.GetTotalRecordAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var action = await _productsUnitOfWork.GetAsync(id);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return NotFound(action.Message);
        }

        [HttpPost("full")]
        public async Task<IActionResult> PostFullAsync(ProductDTO productDTO)
        {
            var action = await _productsUnitOfWork.AddFullAsync(productDTO);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return NotFound(action.Message);
        }

        [HttpPut("full")]
        public async Task<IActionResult> PutFullAsync(ProductDTO productDTO)
        {
            var action = await _productsUnitOfWork.UpdateFullAsync(productDTO);

            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return NotFound(action.Message);
        }
    }
}
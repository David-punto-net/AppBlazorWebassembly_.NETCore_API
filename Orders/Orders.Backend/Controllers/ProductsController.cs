using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Implementations;
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet("pagination")]
        public override async Task<IActionResult> GetPaginationAsync(PaginationDTO pagination)
        {
            var action = await _productsUnitOfWork.GetPaginationAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _productsUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [AllowAnonymous]
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

        [HttpPost("addImages")]
        public async Task<IActionResult> PostAddImagesAsync(ImageDTO imageDTO)
        {
            var action = await _productsUnitOfWork.AddImageAsync(imageDTO);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
        }
        [HttpPost("removeLastImage")]
        public async Task<IActionResult> PostRemoveLastImageAsync(ImageDTO imageDTO)
        {
            var action = await _productsUnitOfWork.RemoveLastImageAsync(imageDTO);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
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

        [HttpDelete("{id}")]
        public override async Task<IActionResult> DeleteAsync(int id)
        {
            var action = await _productsUnitOfWork.DeleteAsync(id);

            if (!action.WassSuccees)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
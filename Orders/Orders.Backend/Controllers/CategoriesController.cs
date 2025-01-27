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
    public class CategoriesController : GenericController<Category>
    {
        private readonly ICategoriesUnitsOfWork _categoriesUnitsOfWork;

        public CategoriesController(IGenericUnitsOfWork<Category> unitsOfWork, ICategoriesUnitsOfWork categoriesUnitsOfWork) : base(unitsOfWork)
        {
            _categoriesUnitsOfWork = categoriesUnitsOfWork;
        }

        //[AllowAnonymous]
        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _categoriesUnitsOfWork.GetComboAsync());
        }

        [HttpGet("pagination")]
        public override async Task<IActionResult> GetPaginationAsync(PaginationDTO pagination)
        {
            var action = await _categoriesUnitsOfWork.GetPaginationAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }


        [HttpGet("totalRecord")]
        public override async Task<IActionResult> GetTotalRecordAsync(PaginationDTO pagination)
        {
            var action = await _categoriesUnitsOfWork.GetTotalRecordAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

    }
}
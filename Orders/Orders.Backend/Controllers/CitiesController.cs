using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Implementations;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using System;

namespace Orders.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : GenericController<City>
    {
        private readonly ICitiesUnitsOfWork _citiesUnitsOfWork;

        public CitiesController(IGenericUnitsOfWork<City> unitsOfWork, ICitiesUnitsOfWork citiesUnitsOfWork) : base(unitsOfWork)
        {
            _citiesUnitsOfWork = citiesUnitsOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _citiesUnitsOfWork.GetAsync(pagination);
            if (response.WassSuccees)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
         {
            var action = await _citiesUnitsOfWork.GetTotalPagesAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            } 
            return BadRequest();
         }

    }
}

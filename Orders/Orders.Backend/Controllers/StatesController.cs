using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Implementations;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatesController : GenericController<State>
    {
        private readonly IStatesUnitsOfWork _statesUnitsOfWork;

        public StatesController(IGenericUnitsOfWork<State> unitsOfWork, IStatesUnitsOfWork statesUnitsOfWork)  : base(unitsOfWork)
        {
            _statesUnitsOfWork = statesUnitsOfWork;
        }

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsync()
        {
            var action = await _statesUnitsOfWork.GetAsync();
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var action = await _statesUnitsOfWork.GetAsync(id);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return NotFound();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _statesUnitsOfWork.GetAsync(pagination);
            if (response.WassSuccees)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }
        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _statesUnitsOfWork.GetTotalPagesAsync(pagination);
            
            if (action.WassSuccees)
                {
                    return Ok(action.Result);
                }
                return BadRequest();
        }

        [HttpGet("pagination")]
        public override async Task<IActionResult> GetPaginationAsync(PaginationDTO pagination)
        {
            var action = await _statesUnitsOfWork.GetPaginationAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }


        [HttpGet("totalRecord")]
        public override async Task<IActionResult> GetTotalRecordAsync(PaginationDTO pagination)
        {
            var action = await _statesUnitsOfWork.GetTotalRecordAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

    }
}

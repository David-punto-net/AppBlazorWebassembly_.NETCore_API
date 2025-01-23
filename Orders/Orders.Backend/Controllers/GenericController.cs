using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;

namespace Orders.Backend.Controllers
{
    public class GenericController<T> : ControllerBase where T : class
    {
        private readonly IGenericUnitsOfWork<T> _unitsOfWork;

        public GenericController(IGenericUnitsOfWork<T> unitsOfWork)
        {
            _unitsOfWork = unitsOfWork;
        }

        [HttpGet("full")]
        public virtual async Task<IActionResult> GetAsync()
        {
            var action = await _unitsOfWork.GetAsync();
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("pagination/{skip}/{take}")]
        public virtual async Task<IActionResult> GetPaginationAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _unitsOfWork.GetPaginationAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalRecord")]
        public virtual async Task<IActionResult> GetTotalRecordAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _unitsOfWork.GetTotalRecordAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _unitsOfWork.GetAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public virtual async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _unitsOfWork.GetTotalPagesAsync(pagination);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetAsync(int id)
        {
            var action = await _unitsOfWork.GetAsync(id);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            //return NotFound();
            return BadRequest(action.Message);
        }

        [HttpPost]
        public virtual async Task<IActionResult> PostAsync(T model)
        {
            var action = await _unitsOfWork.AddAsync(model);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
        }

        [HttpPut]
        public virtual async Task<IActionResult> PutAsync(T model)
        {
            var action = await _unitsOfWork.UpdateAsync(model);
            if (action.WassSuccees)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteAsync(int id)
        {
            var action = await _unitsOfWork.DeleteAsync(id);
            if (action.WassSuccees)
            {
                return NoContent();
            }
            return BadRequest(action.Message);
        }
    }
}
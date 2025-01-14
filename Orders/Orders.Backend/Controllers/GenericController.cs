using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;

namespace Orders.Backend.Controllers
{
    public class GenericController<T> : ControllerBase where T : class
    {
        private readonly IGenericUnitsOfWork<T> _unitsOfWork;

        public GenericController(IGenericUnitsOfWork<T> unitsOfWork)
        {
            _unitsOfWork = unitsOfWork;
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAsync()
        {
            var action = await _unitsOfWork.GetAsync();
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
            return NotFound();
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
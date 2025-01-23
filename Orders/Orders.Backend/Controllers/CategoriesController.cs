﻿using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : GenericController<Category>
    {
        private readonly ICategoriesUnitsOfWork _categoriesUnitsOfWork;

        public CategoriesController(IGenericUnitsOfWork<Category> unitsOfWork, ICategoriesUnitsOfWork categoriesUnitsOfWork) : base(unitsOfWork)
        {
            _categoriesUnitsOfWork = categoriesUnitsOfWork;
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
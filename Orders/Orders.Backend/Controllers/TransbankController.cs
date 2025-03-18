using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Backend.Helpers;
using Orders.Shared.DTOs;

namespace Orders.Backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransbankController : ControllerBase
    {
        private readonly ITransbankService _transbankService;

        public TransbankController(ITransbankService transbankService)
        {
            _transbankService = transbankService;
        }

        [HttpPost]
        public IActionResult CrearTransaccion(TransbankRequestDTO request)
        {
            try
            {
                var response = _transbankService.CreateTransaccion(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Hubo un error, codigo del error: " + ex.Message);
            }
        }

        [HttpGet("{token}")]
        public IActionResult ConfirmarTransaccion(string token)
        {
            try
            {
                var response = _transbankService.ConfirmarTransaccion(token);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Hubo un error, codigo del error: " + ex.Message);
            }
        }

        [HttpGet("{token}")]
        public IActionResult ObtenerEstadoTransaccion(string token)
        {
            try
            {
                var response = _transbankService.ConfirmarTransaccion(token);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Hubo un error, codigo del error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult ReversarAnularTransaccion(TransbankReversarAnular request)
        {
            try
            {
                var response = _transbankService.ReversarAnularTransaccion(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Hubo un error, codigo del error: " + ex.Message);
            }
        }

    }
}

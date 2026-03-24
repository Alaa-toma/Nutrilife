using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.DataAccessLayer.DTO.Response;
using Nutrilife.DataAccessLayer.Models;
using Nutrilife.LogicLayer.Service;

namespace NutriLife.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService )
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("AppointmentRequest")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreatrApointmentRequest(AppointmentRequest request)
        {
            var result = await _appointmentService.CreateAppointmentAsync(request);
            if (result == null)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("ClientAppointment")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> GetClientAppointments(string ClientId)
        {
            var result = await _appointmentService.GetClientAppointmentsAsync(ClientId);
            if (result == null) { return BadRequest(result); }
            return Ok(result);
        }


        [HttpGet("NutritionistAppointment")]
        [Authorize(Roles = "Nutritionist")]
        public async Task<IActionResult> GetNutritionistAppointments(string NutriId)
        {
            var result = await _appointmentService.GetNutritionistAppointmentsAsync(NutriId);
            if (result == null) { return BadRequest(result); }
            return Ok(result);
        }


    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Nutrilife.DataAccessLayer.DTO.Request;
using Nutrilife.LogicLayer.Service;
using NutriLife.PresentationLayer.Resources;

namespace NutriLife.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        //  private readonly IStringLocalizer<SharedResources> _localizer;  , IStringLocalizer<SharedResources> localizer
        private readonly IClientService _IclientService;

        public ClientController(IClientService IclientService)
        {
            _IclientService = IclientService;
           // _localizer = localizer;
        }


        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateClient(ClientRequest request)
        {
            var response = await _IclientService.CreateClient(request);

            return Ok( response );
        }


    }

}

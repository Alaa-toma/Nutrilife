using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NutriLife.PresentationLayer.Resources;

namespace NutriLife.PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
    }
}

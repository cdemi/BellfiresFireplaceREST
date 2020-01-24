using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fireplace.Controllers
{
    [ApiController]
    [Route("/")]
    public class FireplaceController : ControllerBase
    {

        private readonly ILogger<FireplaceController> _logger;

        public FireplaceController(ILogger<FireplaceController> logger, FireplaceService fireplaceService)
        {
            _logger = logger;
            FireplaceService = fireplaceService;
        }

        public FireplaceService FireplaceService { get; }

        [HttpGet("On")]
        public void On()
        {
            FireplaceService.TurnOn();
        }

        [HttpGet("Off")]
        public void Off()
        {
            FireplaceService.TurnOff();
        }
    }
}

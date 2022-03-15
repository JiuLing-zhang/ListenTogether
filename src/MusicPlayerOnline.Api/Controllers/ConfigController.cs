using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicPlayerOnline.Api.Enums;

namespace MusicPlayerOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        public ConfigController()
        {

        }
        [HttpGet()]
        public ActionResult Get(ConfigTypeEnum configType)
        {
            return null;
        }

    }
}

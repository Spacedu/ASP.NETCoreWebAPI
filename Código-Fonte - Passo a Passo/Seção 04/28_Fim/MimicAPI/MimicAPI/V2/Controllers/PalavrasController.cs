using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.V2.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiVersion("2.0")]
    public class PalavrasController : ControllerBase
    {
        [HttpGet("", Name = "ObterTodas")]
        public string ObterTodas()
        {
            return "Versão 2.0";
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using LambdaApi.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace LambdaApi.Controllers
{
    [Route("api/[controller]")]
    public class SecretsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        [HttpPost]
        [Route("cadastrar")]
        public async Task<IActionResult> CadastrarSecrets([FromBody] CadastrarSecretRequest request)
        {
            if (request == null)
                return BadRequest("Solicitação inválida");

            return Ok(await Task.Run(() =>
            {
                return new { 
                    Flow = request.Flow,
                    Secrets = request.SecretsManager
                };
            }));
        }
    }
}
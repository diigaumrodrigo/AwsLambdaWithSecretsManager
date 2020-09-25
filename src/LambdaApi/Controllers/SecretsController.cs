using System;
using System.Threading.Tasks;
using LambdaApi.Models.Request;
using LambdaApi.Services;
using LambdaApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LambdaApi.Controllers
{
    [Route("api/[controller]")]
    public class SecretsController : ControllerBase
    {
        private readonly ICadastroSecretsManagerService _cadastroSecrestManagerService;

        public SecretsController()
        {
            _cadastroSecrestManagerService = new CadastroSecretsManagerService();
        }

        [HttpPost]
        [Route("cadastrar")]
        public async Task<IActionResult> CadastrarSecrets([FromBody] CadastrarSecretRequest request)
        {
            if (request == null)
                return BadRequest("Solicitação inválida");

            try
            {
                await _cadastroSecrestManagerService.Execute(request);

                return Ok("Operação realizada com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest($"Problema ao realizar operação | {ex.Message}");
            }
        }
    }
}
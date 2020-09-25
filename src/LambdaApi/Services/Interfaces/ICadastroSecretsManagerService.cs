using System.Threading.Tasks;
using LambdaApi.Models.Request;

namespace LambdaApi.Services.Interfaces
{
    public interface ICadastroSecretsManagerService
    {
         Task Execute(CadastrarSecretRequest request);
    }
}
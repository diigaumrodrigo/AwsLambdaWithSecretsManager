using System.Collections.Generic;

namespace LambdaApi.Models.Request
{
    public class CadastrarSecretRequest
    {
        public string Flow { get; set; }
        public ICollection<SecretManagerModel> SecretsManager { get; set; }
    }

    public class SecretManagerModel
    {
        public string Chave { get; set; }
        public string Valor { get; set; }
    }
}
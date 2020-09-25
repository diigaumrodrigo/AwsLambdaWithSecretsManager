using System.Threading.Tasks;
using LambdaApi.Models.Request;
using LambdaApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Amazon;
using System.IO;

namespace LambdaApi.Services
{
    public class CadastroSecretsManagerService : ICadastroSecretsManagerService
    {
        private readonly AmazonSecretsManagerClient _client;
        private readonly AmazonSecretsManagerConfig _config;

        public CadastroSecretsManagerService()
        {
            _config = new AmazonSecretsManagerConfig() { RegionEndpoint = RegionEndpoint.SAEast1 };
            _client = new AmazonSecretsManagerClient(_config);
        }

        public async Task Execute(CadastrarSecretRequest request)
        {
            switch (request.Flow.ToUpper())
            {
                case "CREATE":
                    await CreateSecret(request.SecretsManager);
                    return;
                case "UPDATE":
                    await UpdateSecret(request.SecretsManager);
                    return;
                default:
                    throw new ArgumentException($"Opção inválida: {request.Flow}");
            }
        }

        private async Task<string> GetSecret(string secretName)
        {
            string secret = "";

            MemoryStream memoryStream = new MemoryStream();

            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretName;
            request.VersionStage = "AWSCURRENT";

            GetSecretValueResponse response = null;

            try
            {
                response = await _client.GetSecretValueAsync(request);
            }
            catch (ResourceNotFoundException)
            {
                return null;
            }

            if (response.SecretString != null)
            {
                secret = response.SecretString;
            }
            else
            {
                memoryStream = response.SecretBinary;
                StreamReader reader = new StreamReader(memoryStream);
                secret = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
            }

            return secret;
        }

        private async Task CreateSecret(ICollection<SecretManagerModel> secretsManager)
        {
            foreach (var secret in secretsManager)
            {
                var secretExisting = await GetSecret(secret.Chave);

                if (!string.IsNullOrEmpty(secretExisting))
                    throw new Exception($"Secret já cadastrada: {secret.Chave}");

                await _client.CreateSecretAsync(new CreateSecretRequest
                {
                    ClientRequestToken = Guid.NewGuid().ToString(),
                    Description = "",
                    Name = secret.Chave,
                    SecretString = secret.Valor
                });
            }
        }

        private async Task UpdateSecret(ICollection<SecretManagerModel> secretsManager)
        {
            foreach (var secret in secretsManager)
            {
                var secretExisting = await GetSecret(secret.Chave);

                if (string.IsNullOrEmpty(secretExisting))
                    throw new Exception($"Secret não cadastrada: {secret.Chave}");

                await _client.UpdateSecretAsync(new UpdateSecretRequest
                {
                    SecretId = secret.Chave,
                    SecretString = secret.Valor
                });
            }
        }
    }
}
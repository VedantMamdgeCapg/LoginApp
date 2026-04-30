using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace LoginApp.Services
{
    public class AzureKeyVaultService
    {
        private readonly SecretClient _secretClient;
        private readonly ILogger<AzureKeyVaultService> _logger;

        public AzureKeyVaultService(IConfiguration configuration, ILogger<AzureKeyVaultService> logger)
        {
            _logger = logger;
            var keyVaultUri = configuration["KeyVault:Uri"];
            if (string.IsNullOrWhiteSpace(keyVaultUri))
            {
                throw new InvalidOperationException("KeyVault:Uri is not configured.");
            }
            try
            {
                _secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
                _logger.LogInformation("Successfully connected to Azure Key Vault at {Uri}", keyVaultUri);
            }
            catch (Exception ex)
            {
                // Log the error and rethrow or handle appropriately
                _logger.LogError(ex, "Failed to connect to Key Vault.");
                throw new InvalidOperationException($"Failed to connect to Key Vault: {ex.Message}", ex);
            }
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            try
            {
                var secret = await _secretClient.GetSecretAsync(secretName);
                return secret.Value.Value;
            }
            catch (Exception ex)
            {
                // Handle specific Key Vault exceptions
                throw new InvalidOperationException($"Failed to retrieve secret '{secretName}': {ex.Message}", ex);
            }
        }
    }
}
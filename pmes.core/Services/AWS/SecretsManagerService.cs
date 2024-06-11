using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pmes.core.Services.AWS.Dtos;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace pmes.core.Services.AWS
{

    public static class SecretsManagerService
    {
        public static string Region { get; set; } = "Region";
        public static string InfraSecret { get; set; } = "InfraSecret";
        public static string DatabaseSecret { get; set; } = "DatabaseSecret";

        public static string GetConnectionString(string secretName, string region)// string accessKey, string secretKey, string databaseName)
        {
            string secret = "";

            //var AWS_ACCESS_KEY_ID = "ASIAVM7S5OLRFG372263";
            //var AWS_SECRET_ACCESS_KEY = "Vvd2+sxvbBaArztdz8PAGdwULk6VlLbUYoR8R8tw";
            //var AWS_SESSION_TOKEN = "IQoJb3JpZ2luX2VjENj//////////wEaCXVzLWVhc3QtMSJHMEUCIQD11FmzxuvkrMCSYKLp4pvChKM1O3/mMCcUM7T9nDljTAIgY1cE4CyHvHYh6OQgIgjhX54R5LJlLBy999zPRa1uhhUqmgMIwf//////////ARAAGgwzNzE0ODcyNDA5MzAiDDLGhX9pRqaWt723QyruAoimqb7+N1KYpMlVbI5HW8W5bTRIQVpyDH9Kc1D/OEI3MkH71Hua5LZLx5e4GolHRFdBbpEb1QoppsUaxwH6wOlTJhWPCwxHcnaRGl6MasTMl9fXx6IxEI6sSWQF0KmVVtCfsimqJD74iKDAuRpqWsDPm2jTw9n4vrpcprmnNXKHex2INGLp6LmniSkInusvJ1uGfM3J+yqcqDBiZeCEKY1/nXBWrJJuJ8op+/ng7pEvsFuGtX2PCzx4d2zGTurKsPiIAF73aFWR+dJJqrPYIShRg5ablk3gSQvLegv60tkgw3vB8XI9ExC9dpH7iH0WwlDryFo9Jr4+OiiM/XxQ7U3Ru3I45NwDgcRfs7lPTWZTJyIdsRORJivjDVGTxEZgz70WQWOEcdb7i9QaIhE/wFMs0BvRZIqItNTnPWtPYykLDwBjn2rKf5+Yv+hF3bZ/W9+APPd8ji11SfhJuGYVpFSAXYehNVSReDBte0PpVjDnoNuhBjqmAWak1BW4lofxq6KaW+1J3T5gwNexwSqmGmAdbSjGqnHPSAyFrFNWvoF2h8ah+7ffP+0vezaaXQx+4KGp1Zyt2vAWLlDdqR/SIss0HH5uk3qvLZeBDjKT7dCkRBm696Tcve058XRZ/fzLAQ9H8BCETcs2sCymMLdHCX6YPGQOmMGi9eGnQh0iyXVrwbYraezgtWolUZqNEQnWUdNcm2bsN7uuL8QbPAU=";
            //IAmazonSecretsManager client = new AmazonSecretsManagerClient(AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY, AWS_SESSION_TOKEN, RegionEndpoint.GetBySystemName(region));
            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new()
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT"
            };

            GetSecretValueResponse response = client.GetSecretValueAsync(request).Result;

            // Decrypts secret using the associated KMS CMK.
            // Depending on whether the secret is a string or binary, one of these fields will be populated.
            if (response.SecretString != null)
            {
                var node = JsonConvert.DeserializeObject<JsonNode>(response.SecretString);
                secret = string.Format("server={0}; port={1}; database={2}; user={3}; password={4}; Allow User Variables=True; SslMode=Required;", node["host"], node["port"], node["database"], node["username"], node["password"]);
            }
            return secret;
        }

        public static AWSSecretModel GetSecret(string secretName, string region)
        {
            AWSSecretModel secret = new();

            var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new()
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT"
            };

            GetSecretValueResponse response = client.GetSecretValueAsync(request).Result;

            if (response.SecretString != null)
            {
                var node = JsonConvert.DeserializeObject<JsonNode>(response.SecretString);

                Log.Information("GetSecret Secret String: {0}", response.SecretString);

                secret = new AWSSecretModel
                {
                    CognitoUserPoolClientId = $"{node["UserPoolClientId"]!}",
                    CognitoUserPoolId = $"{node["UserPoolId"]!}",
                };
            }

            return secret;
        }
    }
}

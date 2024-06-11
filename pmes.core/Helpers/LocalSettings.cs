
using Microsoft.Extensions.Configuration;
using pmes.core.Services.AWS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Helpers
{
    public static class LocalSettings
    {
        public static AWSSecretModel GetLocalSettings(IConfigurationRoot configuration)
        {
            return new AWSSecretModel
            {
                CognitoUserPoolClientId = configuration["local:CognitoUserPoolClientId"]!,
                CognitoUserPoolId = configuration["local:CognitoUserPoolId"]!,
            };
        }
    }
}

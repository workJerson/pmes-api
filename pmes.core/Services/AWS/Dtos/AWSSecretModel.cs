using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Services.AWS.Dtos
{
    public class AWSSecretModel
    {
        public string? CognitoUserPoolClientId { get; set; }
        public string? CognitoUserPoolId { get; set; }
    }
}

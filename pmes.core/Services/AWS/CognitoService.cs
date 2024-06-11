using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Services.AWS
{
    public interface ICognitoService
    {
        public Task<AdminCreateUserResponse> AdminCreateUser(string email, string username);
        public Task<AdminSetUserPasswordResponse> AdminSetUserPassword(string username, string password);
        public Task<AdminUpdateUserAttributesResponse> AdminVerifyEmail(string username);
        public Task<ChangePasswordResponse> ChangePassword(string accessToken, string previousPassword, string proposedPassword);
        public Task<AdminInitiateAuthResponse> InitiateRefreshToken(string token);
        public Task<GlobalSignOutResponse> GlobalSignOut(string accessToken);
        public Task<AuthFlowResponse> UserSignIn(string username, string password);
        Task<AdminUpdateUserAttributesResponse> UpdateUserAttributes(string username, Dictionary<string, string> attributes);
    }

    public class CognitoService(IConfiguration Configuration) : ICognitoService
    {
        private readonly string CognitoUserPoolId = Configuration["AWS:Cognito:UserPoolId"]!;
        private readonly string AwsRegion = Configuration["AWS:Cognito:Region"]!;
        private readonly string AppClientId = Configuration["AWS:Cognito:AppClientId"]!;

        public async Task<AdminCreateUserResponse> AdminCreateUser(string email, string username)
        {
            var provider = new AmazonCognitoIdentityProviderClient(Amazon.RegionEndpoint.GetBySystemName(AwsRegion));

            AdminCreateUserRequest adminCreateUserRequest = new AdminCreateUserRequest()
            {
                Username = username,
                UserPoolId = CognitoUserPoolId,
                MessageAction = "SUPPRESS"
            };
            // Custom Attributes here
            List<AttributeType> attributes = new List<AttributeType>()
            {
                new AttributeType(){Name = "email", Value = email},
            };

            adminCreateUserRequest.UserAttributes = attributes;

            AdminCreateUserResponse result = await provider.AdminCreateUserAsync(adminCreateUserRequest);

            return result;
        }
        public async Task<AdminSetUserPasswordResponse> AdminSetUserPassword(string username, string password)
        {
            var provider = new AmazonCognitoIdentityProviderClient(Amazon.RegionEndpoint.GetBySystemName(AwsRegion));

            AdminSetUserPasswordRequest adminSetPassword = new AdminSetUserPasswordRequest()
            {
                Username = username,
                UserPoolId = CognitoUserPoolId,
                Permanent = true,
                Password = password
            };

            AdminSetUserPasswordResponse result =
                await provider.AdminSetUserPasswordAsync(adminSetPassword);

            return result;
        }

        public async Task<AdminUpdateUserAttributesResponse> AdminVerifyEmail(string username)
        {
            var provider = new AmazonCognitoIdentityProviderClient(Amazon.RegionEndpoint.GetBySystemName(AwsRegion));

            AdminUpdateUserAttributesRequest adminUpdateUserAttributesRequest = new AdminUpdateUserAttributesRequest()
            {
                Username = username,
                UserPoolId = CognitoUserPoolId
            };

            List<AttributeType> attributes = new List<AttributeType>()
            {
                new AttributeType(){Name = "email_verified", Value = "True"}
            };

            adminUpdateUserAttributesRequest.UserAttributes = attributes;

            AdminUpdateUserAttributesResponse result =
                await provider.AdminUpdateUserAttributesAsync(adminUpdateUserAttributesRequest);

            return result;
        }

        public async Task<ChangePasswordResponse> ChangePassword(string accessToken, string previousPassword, string proposedPassword)
        {
            var provider = new AmazonCognitoIdentityProviderClient(Amazon.RegionEndpoint.GetBySystemName(AwsRegion));

            ChangePasswordRequest changePasswordRequest = new ChangePasswordRequest()
            {
                AccessToken = accessToken,
                PreviousPassword = previousPassword,
                ProposedPassword = proposedPassword
            };

            ChangePasswordResponse result = await provider.ChangePasswordAsync(changePasswordRequest);

            return result;
        }

        public async Task<GlobalSignOutResponse> GlobalSignOut(string accessToken)
        {
            var provider = new AmazonCognitoIdentityProviderClient(Amazon.RegionEndpoint.GetBySystemName(AwsRegion));

            GlobalSignOutRequest authRequest = new GlobalSignOutRequest()
            {
                AccessToken = accessToken
            };

            GlobalSignOutResponse signOutResponse = await provider.GlobalSignOutAsync(authRequest);
            return signOutResponse;
        }

        public async Task<AdminInitiateAuthResponse> InitiateRefreshToken(string token)
        {
            var provider = new AmazonCognitoIdentityProviderClient(Amazon.RegionEndpoint.GetBySystemName(AwsRegion));

            CognitoUserPool userPool = new CognitoUserPool(CognitoUserPoolId, AppClientId, provider);

            AdminInitiateAuthRequest authRequest = new AdminInitiateAuthRequest()
            {
                AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                ClientId = AppClientId,
                UserPoolId = CognitoUserPoolId
            };

            authRequest.AuthParameters.Add("REFRESH_TOKEN", token);

            AdminInitiateAuthResponse authFlowResponse = await provider.AdminInitiateAuthAsync(authRequest).ConfigureAwait(false);
            return authFlowResponse;
        }

        public async Task<AdminUpdateUserAttributesResponse> UpdateUserAttributes(string username, Dictionary<string, string> attributes)
        {
            var provider = new AmazonCognitoIdentityProviderClient(Amazon.RegionEndpoint.GetBySystemName(AwsRegion));

            AdminUpdateUserAttributesRequest adminUpdateUserAttributesRequest = new AdminUpdateUserAttributesRequest()
            {
                Username = username,
                UserPoolId = CognitoUserPoolId
            };

            List<AttributeType> userAttributes = new List<AttributeType>() { };

            foreach (var attribute in attributes)
            {
                userAttributes.Add(new AttributeType() { Name = attribute.Key, Value = attribute.Value });
            }

            adminUpdateUserAttributesRequest.UserAttributes = userAttributes;

            AdminUpdateUserAttributesResponse result =
                await provider.AdminUpdateUserAttributesAsync(adminUpdateUserAttributesRequest);

            return result;
        }

        public async Task<AuthFlowResponse> UserSignIn(string username, string password)
        {
            var provider = new AmazonCognitoIdentityProviderClient(Amazon.RegionEndpoint.GetBySystemName(AwsRegion));

            CognitoUserPool userPool = new CognitoUserPool(CognitoUserPoolId, AppClientId, provider);

            CognitoUser user = new CognitoUser(username, AppClientId, userPool, provider);

            InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
            {
                Password = password
            };

            AuthFlowResponse authFlowResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);
            return authFlowResponse;
        }
    }
}

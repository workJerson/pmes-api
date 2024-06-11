using Microsoft.Extensions.Configuration;
using pmes.core.Helpers;
using pmes.core.Services.AWS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Services.Email
{
    public interface IEmailService
    {
        Task<HttpResponseMessage> SendAccountConfirmationEmail(string name, string emailAddress, string accountCode, string accountName, string token);
        Task<HttpResponseMessage> SendSuccessAccountConfirmationEmail(string name, string emailAddress, string accountName);
        Task<HttpResponseMessage> SendIndividualUserAccountConfirmationEmail(string name, string emailAddress, string accountName, string token);
        Task<HttpResponseMessage> SendPasswordResetCodeEmail(string employeeName, string employeeEmailAddress, string authenticationCode);
        Task<HttpResponseMessage> SendForgotPasswordEmail(string employeeEmailAddress, string employeeName, string employeeToken);
        Task<HttpResponseMessage> SendMobileForgotPasswordEmail(string employeeEmailAddress, string employeeName, string oneTimePassword);
    }
    public class EmailService(IHttpClientHelper httpHelper, IConfiguration configuration) : IEmailService
    {
        private readonly IHttpClientHelper httpHelper = httpHelper;
        private readonly IConfiguration configuration = configuration;
        private SendEmailModel emailPayload = new();

        public async Task<HttpResponseMessage> SendAccountConfirmationEmail(string name, string emailAddress, string accountCode, string accountName, string token)
        {
            string accountConfirmationUrl = $"{configuration["AppUrl:BaseUrl"]}{configuration["AppUrl:AccountConfirmationUrl"].Replace("{token}", token)}";

            emailPayload = new SendEmailModel
            {
                To = emailAddress,
                Subject = "Account Verification",
                Message = configuration["Email:BaseTemplate"]
                                        .Replace("{content}", configuration["Email:RegistrationContent"]
                                            .Replace("{name}", name)
                                            .Replace("{companyCode}", accountCode)
                                            .Replace("{companyName}", accountName)
                                            .Replace("{confirmation_link}", accountConfirmationUrl))
            };

            return await SendEmail();
        }

        public async Task<HttpResponseMessage> SendForgotPasswordEmail(string employeeEmailAddress, string employeeName, string employeeToken)
        {
            string forgotPasswordUrl = $"{configuration["AppUrl:BaseUrl"]}{configuration["AppUrl:ResetPasswordUrl"].Replace("{token}", employeeToken)}";

            emailPayload = new SendEmailModel
            {
                Subject = "Reset Password",
                //To = new List<string> { { employee.EmailAddress } },
                To = employeeEmailAddress,
                Message = configuration["Email:BaseTemplate"]
                                            .Replace("{content}", configuration["Email:ForgotPasswordContent"]
                                                .Replace("{name}", employeeName)
                                                .Replace("{forgot_password_link}", forgotPasswordUrl))
            };

            return await SendEmail();
        }

        public async Task<HttpResponseMessage> SendIndividualUserAccountConfirmationEmail(string name, string emailAddress, string accountName, string token)
        {
            string accountConfirmationUrl = $"{configuration["AppUrl:BaseUrl"]}{configuration["AppUrl:AccountConfirmationUrl"].Replace("{token}", token)}";

            emailPayload = new SendEmailModel
            {
                To = emailAddress,
                Subject = "Account Verification",
                Message = configuration["Email:BaseTemplate"]
                                        .Replace("{content}", configuration["Email:AdditionalContactPersonContent"]
                                            .Replace("{name}", name)
                                            .Replace("{companyName}", accountName)
                                            .Replace("{confirmation_link}", accountConfirmationUrl))
            };

            return await SendEmail();
        }

        public async Task<HttpResponseMessage> SendMobileForgotPasswordEmail(string employeeEmailAddress, string employeeName, string oneTimePassword)
        {
            emailPayload = new SendEmailModel
            {
                Subject = "Reset Password OTP",
                //To = new List<string> { { employee.EmailAddress } },
                To = employeeEmailAddress,
                Message = configuration["Email:BaseTemplate"]
                                            .Replace("{content}", configuration["Email:MobileForgotPasswordContent"]
                                                .Replace("{name}", employeeName)
                                                .Replace("{oneTimePassword}", oneTimePassword))
            };

            return await SendEmail();
        }

        public async Task<HttpResponseMessage> SendPasswordResetCodeEmail(string employeeName, string employeeEmailAddress, string authenticationCode)
        {
            emailPayload = new SendEmailModel
            {
                Subject = "Reset Password Pin Code",
                //To = new List<string> { { updatedToken?.Employee?.EmailAddress } },
                To = employeeEmailAddress,
                Message = configuration["Email:BaseTemplate"]
                                                .Replace("{content}", configuration["Email:ForgotPasswordPinContent"]
                                                    .Replace("{authentication_code}", authenticationCode)
                                                    .Replace("{name}", employeeName))
            };

            return await SendEmail();
        }

        public async Task<HttpResponseMessage> SendSuccessAccountConfirmationEmail(string name, string emailAddress, string accountName)
        {
            string webAppUrl = configuration["AppUrl:BaseUrl"];

            emailPayload = new SendEmailModel
            {
                To = emailAddress,
                Subject = "Account Verification: Congratulations! Your account is now verified",
                Message = configuration["Email:BaseTemplate"]
                                        .Replace("{content}", configuration["Email:SuccessAccountVerificationContent"]
                                            .Replace("{name}", name)
                                            .Replace("{companyName}", accountName)
                                            .Replace("{url}", webAppUrl))
            };

            return await SendEmail();
        }

        private async Task<HttpResponseMessage> SendEmail()
        {

            return await httpHelper.PostAsync(configuration["Email:API:BaseUrl"]!,
                emailPayload,
                configuration["Email:API:ApiKey"]!, true);
        }
    }
}

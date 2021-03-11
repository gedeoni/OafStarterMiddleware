using Application.Common.Interfaces;
using Application.Common.DTOs;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Http;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Http
{
    public class HttpWorldClient : IHttpWorldClient
    {
        private readonly SmtpClient _smtpClient;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HttpWorldClient(IConfiguration config)
        {
            _config = config;
            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = int.Parse(_config["DefaultEmailAccount:Port"]),
                Credentials = new NetworkCredential(_config["DefaultEmailAccount:Email"], _config["DefaultEmailAccount:Password"]),
                EnableSsl = bool.Parse(_config["DefaultEmailAccount:EnableSsl"]),
            };

            _httpClient = new HttpClient();
        }

        public Task SendEmail(EmailDto emailDto)
        {
            _smtpClient.Send(emailDto.SenderEmail, emailDto.RecipientEmail, emailDto.Subject, emailDto.Body);
            return Task.CompletedTask;
        }
    }
}
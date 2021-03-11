using System.Threading.Tasks;
using Application.Common.DTOs;

namespace Application.Common.Interfaces
{
    public interface IHttpWorldClient
    {
        public Task SendEmail(EmailDto emailDto);
    }
}
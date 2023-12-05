using Capstone.Application.DTOs.Email;
using System.Threading.Tasks;

namespace Capstone.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
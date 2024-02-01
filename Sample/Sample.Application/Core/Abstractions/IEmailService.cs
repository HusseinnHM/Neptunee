
using Sample.SharedKernel.Contracts.Email;

namespace Sample.Application.Core.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(SendEmailRequest sendEmailRequest);
}
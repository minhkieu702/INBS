namespace INBS.Application.Interfaces
{
    public interface IEmailSender
    {
        Task Send(string? from, string to, string subject, string messageText);
    }
}

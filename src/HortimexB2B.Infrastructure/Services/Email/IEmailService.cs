namespace HortimexB2B.Infrastructure.Services.Email
{
    public interface IEmailService
    {
        void Send(string receiverEmail, string subject, string message);
    }
}

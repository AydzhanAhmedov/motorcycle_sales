using System.Threading.Tasks;

namespace motorcycle_sales.Core.Interfaces;

public interface IEmailSender
{
  Task SendEmailAsync(string to, string from, string subject, string body);
}

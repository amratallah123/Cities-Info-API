using Microsoft.Extensions.Configuration;

namespace CityInfo.API.Services
{
    public class LocalMailService : IMailService
    {
        private string _emailTo;
        private string _emailFrom;
        public LocalMailService(IConfiguration config)
        {
             _emailTo = config["mailSetting:mailToAddress"];
             _emailFrom = config["mailSetting:mailFromAddress"];
        }
        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {_emailFrom} to {_emailTo}" + $" With {nameof(LocalMailService)}");
            Console.WriteLine($"subject: {subject}");
            Console.WriteLine($"subject: {message}");


        }

    }
}

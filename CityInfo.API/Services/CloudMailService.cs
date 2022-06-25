namespace CityInfo.API.Services
{
    public class CloudMailService : IMailService
    {
        private string _emailTo;
        private string _emailFrom;
        public CloudMailService(IConfiguration config)
        {
            _emailTo = config["mailSetting:mailToAddress"];
            _emailFrom = config["mailSetting:mailFromAddress"];
        }

        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {_emailFrom} to {_emailTo}" + $" With {nameof(CloudMailService)}");
            Console.WriteLine($"subject: {subject}");
            Console.WriteLine($"subject: {message}");


        }
    }
}
                                
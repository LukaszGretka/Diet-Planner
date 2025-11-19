using DietPlanner.EmailService.MessageBroker;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
    .Build();

try
{
    var messageBrockerManager = new MessageBrokerManager(configuration);
    messageBrockerManager.RegisterEmailServiceConsumer();
}
catch(Exception ex)
{
    Console.WriteLine($"An errour occured, please any key to exit. Error message: {ex.Message}");
    Console.ReadLine();
}
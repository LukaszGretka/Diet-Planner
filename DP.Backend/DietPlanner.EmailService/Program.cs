using DietPlanner.EmailService.MessageBroker;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

try
{
    var messageBrockerManager = new MessageBrokerManager(configuration);
    messageBrockerManager.RegisterAsConsumer();
}
catch(Exception ex)
{
    Console.WriteLine($"An errour occured, please any key to exit. Error message: {ex.Message}");
    Console.ReadLine();
    return;
}

Console.WriteLine("Email message brocker is running. Type 'exit' to close.");

var input = Console.ReadLine();
while(input != "exit")
{
    input = Console.ReadLine();
    Console.Write("Type 'exit' to close.");
}

# LiteMessageHandler

`LiteMessageHandler` is a simple messaging system.

## How it works

```csharp
internal class Program
{
    static void Main()
    {
        var messageDispatcher = new MessageHandlerDispatcher();
        
        // Executes the handler that have the "PersonRequest" generic parameter.
        // In this case: PersonRequestHandler.
        messageDispatcher.Dispatch(new PersonRequest
        {
            FirstName = "John",
            LastName = "Doe"
        });
    }
}

public class PersonRequestHandler : MessageHandler<PersonRequest>
{
    public override void Execute(PersonRequest message)
    {
        Console.WriteLine($"PersonRequest: {message.FirstName} {message.LastName}");
    }
}

public class PersonRequest
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
```

This example will display the following output:

```
PersonRequest: John Doe
```

## Advanced usages

### Dependency injection

You can use the `MessageHandlerDispatcher` within a [.NET Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host) and take advantage of the built-in dependency injection system. You will be able to inject any kind of service in your handlers.

```csharp
internal class Program
{
    static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IConsoleService, ConsoleService>();
                services.AddSingleton<IMessageHandlerDispatcher, MessageHandlerDispatcher>();
            })
            .Build();

        var dispatcher = host.Services.GetRequiredService<IMessageHandlerDispatcher>();

        dispatcher.Dispatch(new PersonRequest
        {
            FirstName = "John",
            LastName = "Doe"
        });
    }
}

public interface IConsoleService
{
    void WriteLine(string message);
}

public class ConsoleService : IConsoleService
{
    public void WriteLine(string message) => Console.WriteLine(message);
}

public class PersonRequestHandler : IMessageHandler<PersonRequest>
{
    private readonly IConsoleService _consoleService;

    public PersonRequestHandler(IConsoleService consoleService)
    {
        _consoleService = consoleService;
    }

    public void Execute(PersonRequest message)
    {
        _consoleService.WriteLine($"PersonRequest: {message.FirstName} {message.LastName}");
    }
}

public class PersonRequest
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}
```

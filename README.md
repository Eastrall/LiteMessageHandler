# LiteMessageHandler

`LiteMessageHandler` is a simple messaging system.

## How it works

```csharp
internal class Program
{
    static void Main()
    {
        MessageHandlerDispatcher.Load();
        // Executes the handler that have the "PersonRequest" generic parameter.
        // In this case: PersonRequestHandler.
        MessageHandlerDispatcher.Dispatch(new PersonRequest
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

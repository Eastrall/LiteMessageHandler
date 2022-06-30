using System;

namespace LiteMessageHandler.Sample;

internal class Program
{
    static void Main()
    {
        MessageHandlerDispatcher.Load();
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
        Console.WriteLine($"AuthRequest: {message.FirstName} {message.LastName}");
    }
}

public class PersonRequest
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}

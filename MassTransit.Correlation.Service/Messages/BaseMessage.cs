namespace MassTransit.Correlation.Business.Messages;

public class BaseMessage : CorrelatedBy<Guid>
{
    public BaseMessage()
    {
        CorrelationId = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    public Guid CorrelationId { get; init; }
    public DateTime CreationDate { get; }
    public Guid? ConversationId { get; set; }
}
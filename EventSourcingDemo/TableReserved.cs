namespace EventSourcingDemo;

public record TableReserved(
        Guid ReservationId,
        int TableId,
        string Name,
        DateTime DateTime,
        int NrOfGuests
    ) : TableEvent
{
    public void Accept(EventVisitor visitor)
    {
        visitor.Visit(this);
    }
}


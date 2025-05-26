namespace EventSourcingDemo;
public record DrinksServed(Guid Order) : TableEvent
{
    public void Accept(EventVisitor visitor)
    {
        visitor.Visit(this);
    }
}

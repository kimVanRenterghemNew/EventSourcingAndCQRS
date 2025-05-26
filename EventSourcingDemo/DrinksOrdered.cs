namespace EventSourcingDemo;
public record DrinksOrdered(Order Order) : TableEvent
{
    public void Acept(EventVisitor visitor)
    {
        visitor.Visit(this);
    }
}

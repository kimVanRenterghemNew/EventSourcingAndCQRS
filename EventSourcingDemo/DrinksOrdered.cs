﻿namespace EventSourcingDemo;
public record DrinksOrdered(Order Order) : TableEvent
{
    public void Accept(EventVisitor visitor)
    {
        visitor.Visit(this);
    }
}

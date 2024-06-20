namespace EventSourcingDemo;

public record TableReserved(
        int TableId, 
        string Name,
        DateTime DateTime, 
        int NrOfGuests
    ) : TableEvents;


namespace EventSourcingDemo.Application.Query;

public record Table(int Id, IEnumerable<Reservation> Reservations)
{
    public Table Add(Reservation reservation)
    {
        return this with { Reservations = Reservations.Append(reservation) };
    }

    public Table Update(Reservation reservation)
    {
        var newReservations = Reservations.Where(r => r.ReservationId != reservation.ReservationId)
            .Append(reservation);
        return this with { Reservations = newReservations };
    }
};

public record Reservation(Guid ReservationId, string Name, DateTime DateTime, int NrOfGuests, double TotalCost);

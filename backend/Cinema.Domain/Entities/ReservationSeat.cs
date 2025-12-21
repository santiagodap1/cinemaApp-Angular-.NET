using System;
using Cinema.Domain.Common;

namespace Cinema.Domain.Entities;

public sealed class ReservationSeat : Entity
{
    public Guid ReservationId { get; private set; }
    public Guid SeatId { get; private set; }

    public Reservation Reservation { get; private set; } = null!;
    public Seat Seat { get; private set; } = null!;

    private ReservationSeat()
    {
    }

    public ReservationSeat(Guid reservationId, Guid seatId)
    {
        ReservationId = reservationId;
        SeatId = seatId;
    }
}

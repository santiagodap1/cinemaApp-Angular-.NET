using System;
using System.Collections.Generic;
using Cinema.Domain.Common;

namespace Cinema.Domain.Entities;

public sealed class Reservation : Entity
{
    private readonly List<ReservationSeat> _seats = new();

    public Guid ScreeningId { get; private set; }
    public Guid CustomerId { get; private set; }
    public ReservationStatus Status { get; private set; }
    public decimal TotalAmount { get; private set; }
    public DateTimeOffset ReservedAt { get; private set; }

    public Screening Screening { get; private set; } = null!;
    public Customer Customer { get; private set; } = null!;
    public IReadOnlyCollection<ReservationSeat> Seats => _seats.AsReadOnly();

    private Reservation()
    {
    }

    public Reservation(Guid screeningId, Guid customerId, decimal totalAmount, DateTimeOffset reservedAt)
    {
        ScreeningId = screeningId;
        CustomerId = customerId;
        TotalAmount = totalAmount >= 0 ? totalAmount : throw new ArgumentOutOfRangeException(nameof(totalAmount));
        ReservedAt = reservedAt;
        Status = ReservationStatus.Pending;
    }

    public void Confirm()
    {
        Status = ReservationStatus.Confirmed;
    }

    public void Cancel()
    {
        Status = ReservationStatus.Cancelled;
    }

    public void Expire()
    {
        Status = ReservationStatus.Expired;
    }

    public void AddSeat(ReservationSeat seat)
    {
        if (seat is null)
        {
            throw new ArgumentNullException(nameof(seat));
        }

        _seats.Add(seat);
    }

    public void UpdateTotalAmount(decimal totalAmount)
    {
        TotalAmount = totalAmount >= 0 ? totalAmount : throw new ArgumentOutOfRangeException(nameof(totalAmount));
    }
}

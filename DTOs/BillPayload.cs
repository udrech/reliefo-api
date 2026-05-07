using reliefo_api.Models;

namespace reliefo_api.DTOs;

public class BillPayload
{
    public int CustomerId { get; set; }

    public List<Appointment> Appointments { get; set; } = new List<Appointment>();
}

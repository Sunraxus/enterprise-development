using EstateAgency.Domain.Enums;

namespace EstateAgency.Domain.Entities;

public class Application
{
    public required int Id { get; set; }
    public required int CounterpartyId { get; set; }
    public required Counterparty Counterparty { get; set; }
    public required int RealEstateId { get; set; }
    public required RealEstate RealEstate { get; set; } 
    public required ApplicationType Type { get; set; }          
    public required decimal Amount { get; set; }                  
    public required DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}

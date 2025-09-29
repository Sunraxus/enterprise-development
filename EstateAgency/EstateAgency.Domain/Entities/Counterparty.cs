namespace EstateAgency.Domain.Entities;

public class Counterparty
{
    public required int Id { get; set; }
    public required string FullName { get; set; }
    public required string PassportNumber { get; set; }
    public required string Phone { get; set; }
}

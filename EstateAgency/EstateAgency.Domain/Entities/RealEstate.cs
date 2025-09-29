using EstateAgency.Domain.Enums;

namespace EstateAgency.Domain.Entities;

public sealed class RealEstate
{
    public required int Id { get; set; }
    public required TypeRealEstate Type { get; set; }
    public required PurposeRealEstate Purpose { get; set; }
    public required string CadastralNumber { get; set; } 
    public required string Address { get; set; } 
    public required int FloorsTotal { get; set; }                 
    public required double AreaTotal { get; set; }               
    public int? Rooms { get; set; }                       
    public double? CeilingHeight { get; set; }           
    public int? FloorNumber { get; set; }                
    public required bool HasEncumbrances { get; set; }
}

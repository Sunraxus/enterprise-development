using EstateAgency.Domain.Entities;
using EstateAgency.Domain.Enums;

namespace EstateAgency.Test;

public class TestDataFixture
{
    public List<Counterparty> Clients =>
    [
        new() 
        { 
            Id = 1,  
            FullName = "Иванов Иван Иванович",    
            PassportNumber = "4010 123456", 
            Phone = "89030000001" 
        },
        new() 
        { 
            Id = 2,  
            FullName = "Петров Пётр Петрович",     
            PassportNumber = "4011 223344", 
            Phone = "89030000002" 
        },
        new() 
        { 
            Id = 3,  
            FullName = "Сидорова Анна Сергеевна",  
            PassportNumber = "4012 334455", 
            Phone = "89030000003" 
        },
        new() 
        { 
            Id = 4,  
            FullName = "Кузнецов Алексей Иванов",  
            PassportNumber = "4013 445566", 
            Phone = "89030000004" 
        },
        new() 
        { 
            Id = 5,  
            FullName = "Попова Елена Викторовна",  
            PassportNumber = "4014 556677", 
            Phone = "89030000005" 
        },
        new() 
        { 
            Id = 6,  
            FullName = "Соколов Дмитрий Андреевич",
            PassportNumber = "4015 667788", 
            Phone = "89030000006" 
        },
        new() 
        { 
            Id = 7,  
            FullName = "Лебедева Мария Алексеевна",
            PassportNumber = "4016 778899", 
            Phone = "89030000007" 
        },
        new() 
        { 
            Id = 8,  
            FullName = "Морозов Николай Петрович", 
            PassportNumber = "4017 889900", 
            Phone = "89030000008" 
        },
        new() 
        { 
            Id = 9,  
            FullName = "Орлов Артём Александров",  
            PassportNumber = "4018 990011", 
            Phone = "89030000009" 
        },
        new() 
        { 
            Id = 10, 
            FullName = "Волкова Екатерина Мих.",   
            PassportNumber = "4019 110022", 
            Phone = "89030000010" 
        }
    ];

    public List<RealEstate> EstateObjects =>
    [
        new() 
        { 
            Id = 1,  
            Type = TypeRealEstate.Apartment, 
            Purpose = PurposeRealEstate.Residential, 
            CadastralNumber = "63:01:0000001:1",  
            Address = "ул. Ленина, 1, кв. 10", 
            FloorNumber = 3,  
            FloorsTotal = 9,  
            AreaTotal = 42.5, 
            Rooms = 2, 
            CeilingHeight = 270, 
            HasEncumbrances = false 
        },
        new() 
        { 
            Id = 2,  
            Type = TypeRealEstate.Apartment, 
            Purpose = PurposeRealEstate.Residential, 
            CadastralNumber = "63:01:0000001:2",  
            Address = "ул. Ленина, 3, кв. 120",
            FloorNumber = 12, 
            FloorsTotal = 16, 
            AreaTotal = 55.0, 
            Rooms = 3, 
            CeilingHeight = 270, 
            HasEncumbrances = true 
        },
        new() 
        { 
            Id = 3,  
            Type = TypeRealEstate.House,     
            Purpose = PurposeRealEstate.Residential, 
            CadastralNumber = "63:01:0000002:3",  
            Address = "ул. Садовая, 10",     
            FloorNumber = null, 
            FloorsTotal = 2,  
            AreaTotal = 120.0,
            Rooms = 4, 
            CeilingHeight = 300, 
            HasEncumbrances = false 
        },
        new() 
        { 
            Id = 4,  
            Type = TypeRealEstate.Commercial,
            Purpose = PurposeRealEstate.Commercial,  
            CadastralNumber = "63:01:0000003:4",  
            Address = "пр. Масленникова, 5", 
            FloorNumber = 1,  
            FloorsTotal = 5,  
            AreaTotal = 200.0,
            Rooms = null, 
            CeilingHeight = 350, 
            HasEncumbrances = false 
        },
        new() 
        { 
            Id = 5,  
            Type = TypeRealEstate.Office,    
            Purpose = PurposeRealEstate.Office,      
            CadastralNumber = "63:01:0000003:5",  
            Address = "ул. Ново‑Садовая, 150",
            FloorNumber = 14, 
            FloorsTotal = 20, 
            AreaTotal = 95.0, 
            Rooms = 5, 
            CeilingHeight = 300, 
            HasEncumbrances = false 
        },
        new() 
        { 
            Id = 6,
            Type = TypeRealEstate.Warehouse, 
            Purpose = PurposeRealEstate.Storage,     
            CadastralNumber = "63:01:0000004:6",  
            Address = "Промзона, 1",         
            FloorNumber = null, 
            FloorsTotal = 1,  
            AreaTotal = 600.0,
            Rooms = null, 
            CeilingHeight = 600, 
            HasEncumbrances = true  
        },
        new() 
        { 
            Id = 7,  
            Type = TypeRealEstate.Apartment, 
            Purpose = PurposeRealEstate.Residential, 
            CadastralNumber = "63:01:0000005:7",  
            Address = "ул. Осипенко, 22, 78",
            FloorNumber = 7,  
            FloorsTotal = 10, 
            AreaTotal = 36.0, 
            Rooms = 1, 
            CeilingHeight = 260, 
            HasEncumbrances = false 
        },
        new() 
        { 
            Id = 8,  
            Type = TypeRealEstate.House,     
            Purpose = PurposeRealEstate.Residential, 
            CadastralNumber = "63:01:0000006:8",  
            Address = "ул. Загородная, 3",   
            FloorNumber = null, 
            FloorsTotal = 3,  
            AreaTotal = 210.0,
            Rooms = 6, 
            CeilingHeight = 320, 
            HasEncumbrances = false 
        },
        new() 
        { 
            Id = 9,  
            Type = TypeRealEstate.Office,    
            Purpose = PurposeRealEstate.Office,      
            CadastralNumber = "63:01:0000007:9",  
            Address = "БЦ «Волга Плаза»",    
            FloorNumber = 18, 
            FloorsTotal = 24, 
            AreaTotal = 75.0, 
            Rooms = 4, 
            CeilingHeight = 300, 
            HasEncumbrances = false 
        },
        new() 
        { 
            Id = 10, 
            Type = TypeRealEstate.Commercial,
            Purpose = PurposeRealEstate.Commercial,  
            CadastralNumber = "63:01:0000008:10", 
            Address = "ТЦ «Космопорт», пав.12",
            FloorNumber = 2, 
            FloorsTotal = 2,  
            AreaTotal = 48.0, 
            Rooms = 2, 
            CeilingHeight = 350, 
            HasEncumbrances = true 
        }
    ];

    public List<Application> Requests =>
    [
        new() 
        {
            Id = 1,  
            CounterpartyId = 1,  
            Counterparty = Clients[0],  
            RealEstateId = 1,  
            RealEstate = EstateObjects[0],  
            Type = ApplicationType.Sale,     
            Amount = 4_500_000m,  
            Date = new DateOnly(2025, 1, 15) 
        },
        new() 
        {
            Id = 2,  
            CounterpartyId = 2,  
            Counterparty = Clients[1],  
            RealEstateId = 2, 
            RealEstate = EstateObjects[1],  
            Type = ApplicationType.Sale,     
            Amount = 6_800_000m,  
            Date = new DateOnly(2025, 2, 10) 
        },
        new() 
        {
            Id = 3,  
            CounterpartyId = 3,  
            Counterparty = Clients[2],  
            RealEstateId = 3,  
            RealEstate = EstateObjects[2],  
            Type = ApplicationType.Purchase, 
            Amount = 9_500_000m,  
            Date = new DateOnly(2025, 2, 25) 
        },
        new() 
        {
            Id = 4,  
            CounterpartyId = 4,  
            Counterparty = Clients[3],  
            RealEstateId = 4, 
            RealEstate = EstateObjects[3],  
            Type = ApplicationType.Purchase, 
            Amount = 15_000_000m, 
            Date = new DateOnly(2025, 3, 5)  
        },
        new() 
        { 
            Id = 5,  
            CounterpartyId = 5,  
            Counterparty = Clients[4],  
            RealEstateId = 5,  
            RealEstate = EstateObjects[4],  
            Type = ApplicationType.Sale,     
            Amount = 12_300_000m, 
            Date = new DateOnly(2025, 3, 28) 
        },
        new() 
        {
            Id = 6,  
            CounterpartyId = 6,  
            Counterparty = Clients[5],  
            RealEstateId = 6,  
            RealEstate = EstateObjects[5],  
            Type = ApplicationType.Sale,     
            Amount = 25_000_000m, 
            Date = new DateOnly(2025, 4, 12) 
        },
        new() 
        { 
            Id = 7,  
            CounterpartyId = 7,
            Counterparty = Clients[6],  
            RealEstateId = 7,  
            RealEstate = EstateObjects[6],  
            Type = ApplicationType.Purchase, 
            Amount = 3_800_000m,  
            Date = new DateOnly(2025, 4, 20) 
        },
        new() 
        { 
            Id = 8,  
            CounterpartyId = 8,  
            Counterparty = Clients[7],  
            RealEstateId = 8,  
            RealEstate = EstateObjects[7],  
            Type = ApplicationType.Purchase, 
            Amount = 18_700_000m, 
            Date = new DateOnly(2025, 5, 3)  
        },
        new() 
        { 
            Id = 9,  
            CounterpartyId = 9,  
            Counterparty = Clients[8],  
            RealEstateId = 9,  
            RealEstate = EstateObjects[8],  
            Type = ApplicationType.Sale,     
            Amount = 10_900_000m, 
            Date = new DateOnly(2025, 5, 18) 
        },
        new() 
        { 
            Id = 10, 
            CounterpartyId = 10, 
            Counterparty = Clients[9],  
            RealEstateId = 10, 
            RealEstate = EstateObjects[9],  
            Type = ApplicationType.Purchase, 
            Amount = 7_200_000m,  
            Date = new DateOnly(2025, 6, 1)  
        },
        new() 
        { 
            Id = 11, 
            CounterpartyId = 1,  
            Counterparty = Clients[0],  
            RealEstateId = 7,  
            RealEstate = EstateObjects[6],  
            Type = ApplicationType.Purchase, 
            Amount = 4_000_000m,  
            Date = new DateOnly(2025, 6, 15) 
        },
        new() 
        { 
            Id = 12, 
            CounterpartyId = 2,  
            Counterparty = Clients[1],  
            RealEstateId = 1,  
            RealEstate = EstateObjects[0],  
            Type = ApplicationType.Purchase, 
            Amount = 4_600_000m,  
            Date = new DateOnly(2025, 6, 20) 
        },
        new() 
        { 
            Id = 13, 
            CounterpartyId = 3,  
            Counterparty = Clients[2],  
            RealEstateId = 5,  
            RealEstate = EstateObjects[4],  
            Type = ApplicationType.Sale,     
            Amount = 12_100_000m, 
            Date = new DateOnly(2025, 7, 7)  
        },
        new() 
        { 
            Id = 14, 
            CounterpartyId = 4,  
            Counterparty = Clients[3],  
            RealEstateId = 2,  
            RealEstate = EstateObjects[1],  
            Type = ApplicationType.Sale,     
            Amount = 6_600_000m,  
            Date = new DateOnly(2025, 7, 22) 
        },
        new() 
        { 
            Id = 15, 
            CounterpartyId = 5,  
            Counterparty = Clients[4],  
            RealEstateId = 9,  
            RealEstate = EstateObjects[8],  
            Type = ApplicationType.Purchase, 
            Amount = 10_700_000m, 
            Date = new DateOnly(2025, 8, 9)  
        }
    ];
}

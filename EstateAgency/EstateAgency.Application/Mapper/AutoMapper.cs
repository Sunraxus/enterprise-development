using AutoMapper;
using EstateAgency.Application.DTO;
using EstateAgency.Domain.Entities;

namespace EstateAgency.Application.Mapper;

/// <summary>
/// Профиль сопоставления для AutoMapper в проекте агентства недвижимости.
/// Описывает правила преобразования между сущностями домена и DTO,
/// обеспечивая быстрый и безопасный маппинг объектов в приложении.
/// </summary>
public class AutoMapper : Profile
{
    /// <summary>
    /// Конструктор класса AutoMapper.
    /// Инициализирует все сопоставления между DTO и доменными сущностями 
    /// для контрагентов, недвижимости и заявок, включая двусторонний маппинг.
    /// </summary>
    public AutoMapper()
    {
        CreateMap<Counterparty, CounterpartyReadDto>().ReverseMap();
        CreateMap<CounterpartyCreateDto, Counterparty>().ReverseMap();

        CreateMap<RealEstate, RealEstateReadDto>().ReverseMap();
        CreateMap<RealEstateCreateDto, RealEstate>().ReverseMap();

        CreateMap<EstateAgency.Domain.Entities.Application, ApplicationReadDto>().ReverseMap();
        CreateMap<ApplicationCreateDto, EstateAgency.Domain.Entities.Application>().ReverseMap();
    }
}

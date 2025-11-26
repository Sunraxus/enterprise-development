using AutoMapper;
using EstateAgency.Application.Dto;
using EstateAgency.Domain.Entities;

namespace EstateAgency.Application.Mapper;

/// <summary>
/// Профиль сопоставления для AutoMapper в проекте агентства недвижимости.
/// Описывает правила преобразования между сущностями домена и Dto,
/// обеспечивая быстрый и безопасный маппинг объектов в приложении.
/// </summary>
public class AppMapper : Profile
{
    /// <summary>
    /// Конструктор класса AppMapper.
    /// Инициализирует все сопоставления между Dto и доменными сущностями 
    /// для контрагентов, недвижимости и заявок, включая двусторонний маппинг.
    /// </summary>
    public AppMapper()
    {
        CreateMap<Counterparty, CounterpartyReadDto>().ReverseMap();
        CreateMap<CounterpartyCreateDto, Counterparty>().ReverseMap();

        CreateMap<RealEstate, RealEstateReadDto>().ReverseMap();
        CreateMap<RealEstateCreateDto, RealEstate>().ReverseMap();

        CreateMap<Domain.Entities.Application, ApplicationReadDto>().ReverseMap();
        CreateMap<ApplicationCreateDto, Domain.Entities.Application>().ReverseMap();
    }
}
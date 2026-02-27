using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace Shared.MappingProfiles;

/// <summary>
/// AutoMapper profile định nghĩa mapping rules giữa Entities và DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // NaviItem mappings
        CreateMap<NaviItem, NaviItemDto>();
        CreateMap<NaviItemForCreationDto, NaviItem>();
        CreateMap<NaviItemForUpdateDto, NaviItem>();

        // NaviProduct mappings
        CreateMap<NaviProduct, NaviProductDto>();
        CreateMap<NaviProductForCreationDto, NaviProduct>();
        CreateMap<NaviProductForUpdateDto, NaviProduct>();

        // NaviProductItem mappings
        CreateMap<NaviProductItem, NaviProductItemDto>()
            .ForMember(dest => dest.ProductName, 
                       opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : null))
            .ForMember(dest => dest.ItemDescription, 
                       opt => opt.MapFrom(src => src.Item != null ? src.Item.Description : null));
        
        CreateMap<NaviProductItemForCreationDto, NaviProductItem>();

        // NaviProductWithItems composite mappings
        CreateMap<NaviProduct, NaviProductWithItemsDto>()
            .ForMember(dest => dest.Items,
                       opt => opt.MapFrom(src => src.ProductItems != null 
                           ? src.ProductItems.Where(pi => pi.Item != null).Select(pi => pi.Item!).ToList()
                           : new List<NaviItem>()));
        
        CreateMap<NaviProductWithItemsForCreationDto, NaviProduct>();
        CreateMap<NaviProductWithItemsForUpdateDto, NaviProduct>();

        // NaviHistory mappings
        CreateMap<NaviHistory, NaviHistoryDto>();
        CreateMap<NaviHistoryForCreationDto, NaviHistory>();
        CreateMap<NaviHistoryForUpdateDto, NaviHistory>();
    }
}

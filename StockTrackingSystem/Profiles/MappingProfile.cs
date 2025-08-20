using AutoMapper;
using StockTrackingSystem.Entities;
using StockTrackingSystem.Models.StockCard;

namespace StockTrackingSystem.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // Warehouse
            CreateMap<Entities.Warehouse, Models.Warehouse.WarehouseDto>();
            CreateMap<Models.Warehouse.CreateWarehouseDto, Entities.Warehouse>();
            CreateMap<Models.Warehouse.UpdateWarehouseDto, Entities.Warehouse>();
            CreateMap<Entities.Warehouse, Models.Warehouse.UpdateWarehouseDto>();

            //StockCard
            CreateMap<StockCard, StockCardViewDto>()
                .ForMember(d => d.ItemName, o => o.MapFrom(s => s.Item.Name))
                .ForMember(d => d.WarehouseName, o => o.MapFrom(s => s.Warehouse.Name));
            CreateMap<CreateStockCardDto, StockCard>();
            CreateMap<UpdateStockCardDto, StockCard>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}

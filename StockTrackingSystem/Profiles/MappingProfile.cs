using AutoMapper;
using StockTrackingSystem.Entities;
using StockTrackingSystem.Models.Request;
using StockTrackingSystem.Models.StockCard;
using StockTrackingSystem.Models.Transfer;

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

            //Transfer
            CreateMap<CreateTransferDto, Transfer>();
            CreateMap<Transfer, TransferViewDto>()
                .ForMember(d => d.FromWarehouseId, o => o.MapFrom(s => s.FromWarehouseId))
                .ForMember(d => d.ToWarehouseId, o => o.MapFrom(s => s.ToWarehouseId))
                .ForMember(d => d.ItemId, o => o.MapFrom(s => s.ItemId))
                .ForMember(d => d.FromWarehouseName, o => o.MapFrom(s => s.FromWarehouse!.Name))
                .ForMember(d => d.ToWarehouseName, o => o.MapFrom(s => s.ToWarehouse!.Name))
                .ForMember(d => d.ItemName, o => o.MapFrom(s => s.Item!.Name));

            //Request
            CreateMap<CreateRequestDto, Request>();
            CreateMap<Request, RequestViewDto>()
                .ForMember(d => d.FromWarehouseName, o => o.MapFrom(s => s.FromWarehouse.Name))
                .ForMember(d => d.ToWarehouseName, o => o.MapFrom(s => s.ToWarehouse.Name))
                .ForMember(d => d.ItemName, o => o.MapFrom(s => s.Item.Name));
        }
    }
}

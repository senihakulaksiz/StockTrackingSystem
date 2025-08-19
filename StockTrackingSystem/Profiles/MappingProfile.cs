using AutoMapper;

namespace StockTrackingSystem.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Entities.Warehouse, Models.Warehouse.WarehouseDto>();
            CreateMap<Models.Warehouse.CreateWarehouseDto, Entities.Warehouse>();
            CreateMap<Models.Warehouse.UpdateWarehouseDto, Entities.Warehouse>();
            CreateMap<Entities.Warehouse, Models.Warehouse.UpdateWarehouseDto>();
        }
    }
}

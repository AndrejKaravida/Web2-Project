using AutoMapper;
using WEB2Project.Dtos;
using WEB2Project.Models;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Helpers
{
    public class AutoMappingProfiles : Profile
    {
        public AutoMappingProfiles()
        {
            CreateMap<RentACarCompany, CompanyToReturn>();
            CreateMap<Vehicle, VehicleToReturn>();
            CreateMap<Vehicle, DiscountedVehicleToReturn>();
            CreateMap<Reservation, ReservationToReturn>();
            CreateMap<AirCompany, AirCompanyToReturn>();

        }
    }
}

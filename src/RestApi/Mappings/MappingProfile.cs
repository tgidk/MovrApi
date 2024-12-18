using AutoMapper;
using Controllers.Resources;
using Core.Models;

namespace Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		MapResourceToModel();
		MapModelToResource();
	}

	internal void MapResourceToModel()
	{
		CreateMap<AddVehicleDto, Vehicle>();
		CreateMap<LocationDto, LocationHistory>();
		CreateMap<RideDto, Ride>();
		CreateMap<UserDto, User>();
		CreateMap<VehicleDto, Vehicle>();
	}

	internal void MapModelToResource()
	{
		CreateMap<LocationHistory, LocationDto>();
		CreateMap<Ride, RideDto>();
		CreateMap<User, UserDto>();
		CreateMap<Vehicle, VehicleDto>();
	}
}

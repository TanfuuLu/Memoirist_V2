using AutoMapper;
using Memoirist_V2.StoryService.Models.DTO;
using Memoirist_V2.StoryService.Models;
using Memoirist_V2.YarpGateway.Models;
using Memoirist_V2.WriterService.Models.DTO;
using Memoirist_V2.WriterService.Models;

namespace Memoirist_V2.YarpGateway.Mapping;

public class MappingProfile : Profile {
	public MappingProfile() {
		CreateMap<User, RegisterUser>().ReverseMap();
		//CreateMap<Story, AddStoryDTO>().ReverseMap();//Mapping property StoryService
	}
}

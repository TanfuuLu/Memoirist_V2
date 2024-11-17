using AutoMapper;
using Memoirist_V2.StoryService.Models;
using Memoirist_V2.StoryService.Models.DTO;

namespace Memoirist_V2.StoryService.Mapping;

public class StoryMappingProfile : Profile {
	public StoryMappingProfile() {
		CreateMap<Story, AddStoryDTO>().ReverseMap();
		CreateMap<Story, ShowStoryDTO>().ReverseMap();

	}
}

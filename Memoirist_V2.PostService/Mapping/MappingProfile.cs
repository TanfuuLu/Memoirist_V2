using AutoMapper;
using Memoirist_V2.PostService.Models;
using Memoirist_V2.PostService.Models.DTO;

namespace Memoirist_V2.PostService.Mapping;

public class MappingProfile : Profile {
	public MappingProfile() {
		CreateMap<Post, AddPostDTO>().ReverseMap();
	}
}

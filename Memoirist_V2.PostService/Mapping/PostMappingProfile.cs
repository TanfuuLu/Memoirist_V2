using AutoMapper;
using Memoirist_V2.PostService.Models;
using Memoirist_V2.PostService.Models.DTO;

namespace Memoirist_V2.PostService.Mapping;

public class PostMappingProfile : Profile {
	public PostMappingProfile() {
		CreateMap<Post, AddPostDTO>().ReverseMap();
		CreateMap<CommentPost, AddCommentPost>().ReverseMap();
	}
}

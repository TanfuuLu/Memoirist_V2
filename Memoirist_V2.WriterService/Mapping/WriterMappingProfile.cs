using AutoMapper;
using Memoirist_V2.WriterService.Models;
using Memoirist_V2.WriterService.Models.DTO;

namespace Memoirist_V2.WriterService.Mapping;

public class WriterMappingProfile : Profile {
	public WriterMappingProfile() {
		CreateMap<Writer, UpdateWriterDTO>().ReverseMap();
		CreateMap<Writer, ReadWriterDTO>().ReverseMap();
		CreateMap<Writer, ReadFollowWriterDTO>().ReverseMap();
	}
}

using AutoMapper;
using Smart_Librarian.Models.Dtos;
using Smart_Librarian.Models.Entity;

namespace Smart_Librarian.MappingProfile
{
    public class MemberMappingProfile : Profile
    {
        public MemberMappingProfile()
        {
            CreateMap<Member,MemberDto>();
            CreateMap<AddMemberDto, Member>();
            CreateMap<EditMemberDto, Member>();
        }
    }
}

using AutoMapper;
using Smart_Librarian.Models.Dtos;
using Smart_Librarian.Models.Entity;

namespace Smart_Librarian.MappingProfile
{
    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<AddBookDto, Book>();
            CreateMap<EditBookDto, Book>();
        }
    }
}

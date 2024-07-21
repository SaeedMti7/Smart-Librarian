using AutoMapper;
using Smart_Librarian.Models.Dtos;
using Smart_Librarian.Models.Entity;

namespace Smart_Librarian.MappingProfile
{
    public class LoanMappingProfile : Profile
    {
        public LoanMappingProfile()
        {
            CreateMap<Loan, LoanDto>();
            CreateMap<AddLoanDto, Loan>();
            CreateMap<EditLoanDto, Loan>();
        }
    }
}

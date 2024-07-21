using AutoMapper;
using Common.Extensions;
using Common.Models;
using Smart_Librarian.Interfaces;
using Smart_Librarian.Models.Dtos;
using Smart_Librarian.Models.Entity;
using Smart_Librarian.Repositories;

namespace Smart_Librarian.Services
{
    public interface IMemberService
    {
        ResultHub<List<MemberDto>> GetAll();
        ResultHub<MemberDto> Get(int id);
        ResultHub<MemberDto> Add(AddMemberDto addMemberDto);
        ResultHub<MemberDto> Edit(int id, EditMemberDto editMemberDto);
        ResultHub<MemberDto> Remove(int id);
        ResultHub<List<LoanDto>> GetLoansByMemberId(int memberId);
        ResultHub<LoanDto> AddLoanForMember(int memberId, AddLoanDto addLoanDto);
    }

    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public ResultHub<MemberDto> Add(AddMemberDto addMemberDto)
        {
            var result = new ResultHub<MemberDto>();

            try
            {
                var memberRepository = unitOfWork.GetRepository<Member>();

                if (addMemberDto == null)
                    return result.IsNullBadRequest();

                if (memberRepository.Exists(x => x.Name == addMemberDto.Name && !x.IsDeleted))
                    return result.IsConflict();

                var data = mapper.Map<Member>(addMemberDto);

                data.CreatedDate = DateTime.UtcNow;
                data.IsDeleted = false;

                memberRepository.Add(data);
                unitOfWork.Complete();

                return result.SetContent(mapper.Map<MemberDto>(data)).IsCreated();

            }
            catch (Exception ex)
            {

                return result.IsServerError(ex.Message);
            }
        }


        public ResultHub<MemberDto> Edit(int id, EditMemberDto editMemberDto)
        {
            var result = new ResultHub<MemberDto>();

            try
            {
                var memberRepository = unitOfWork.GetRepository<Member>();

                if (editMemberDto == null)
                    return result.IsNullBadRequest();

                if (memberRepository.Exists(x => x.Id != id && x.Name == editMemberDto.Name && !x.IsDeleted))
                    return result.IsConflict();

                var data = memberRepository.GetId(id);

                data.UpdatedDate = DateTime.UtcNow;

                mapper.Map(editMemberDto, data);

                memberRepository.Update(data);
                unitOfWork.Complete();

                return result.Success(mapper.Map<MemberDto>(data)).IsEdited();

            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

        public ResultHub<MemberDto> Get(int id)
        {
            var result = new ResultHub<MemberDto>();

            try
            {
                var memberRepository = unitOfWork.GetRepository<Member>();

                var data = memberRepository.GetId(id);

                if (data == null)
                    return result.Success(null);

                return result.Success(mapper.Map<MemberDto>(data));
            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

        public ResultHub<List<MemberDto>> GetAll()
        {
            var result = new ResultHub<List<MemberDto>>();

            try
            {
                var memberRepository = unitOfWork.GetRepository<Member>();

                var data = memberRepository.GetAll();

                if (data == null)
                    return result.Success(null);

                return result.Success(mapper.Map<List<MemberDto>>(data));
            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

        public ResultHub<MemberDto> Remove(int id)
        {
            var result = new ResultHub<MemberDto>();

            try
            {
                var memberRepository = unitOfWork.GetRepository<Member>();

                var data = memberRepository.GetId(id);

                if (data == null)
                    return result.IsNotFound();

                data.IsDeleted = true;
                data.DeletedDate = DateTime.UtcNow;

                memberRepository.Update(data);

                unitOfWork.Complete();
                return result.IsDeleted();

            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

        public  ResultHub<List<LoanDto>> GetLoansByMemberId(int memberId)
        {
            var result = new ResultHub<List<LoanDto>>();

            try
            {
                var loanRepository = unitOfWork.GetRepository<Loan>();

                var data =  loanRepository.GetListWithIncludes(
                    l => l.MemberId == memberId,
                    l => l.Book,
                    l => l.Member
                );

                if (data == null || !data.Any())
                    return result.Success(null);

                var loanDtoList = mapper.Map<List<LoanDto>>(data);
                return result.Success(loanDtoList).IsEdited();
            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

        public  ResultHub<LoanDto> AddLoanForMember(int memberId, AddLoanDto addLoanDto)
        {
            var result = new ResultHub<LoanDto>();

            try
            {
                var memberRepository = unitOfWork.GetRepository<Member>();
                var member =  memberRepository.GetId(memberId);
                if (member == null)
                {
                    return result.IsNotFound();
                }

                var loanRepository = unitOfWork.GetRepository<Loan>();
                var newLoan = mapper.Map<Loan>(addLoanDto);
                newLoan.MemberId = memberId;

                 loanRepository.AddAsync(newLoan);
                 unitOfWork.Complete();

                var createdLoanDto = mapper.Map<LoanDto>(newLoan);
                return result.Success(createdLoanDto).IsEdited();
            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

    }
}

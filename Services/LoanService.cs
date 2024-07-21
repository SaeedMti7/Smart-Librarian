using AutoMapper;
using Common.Extensions;
using Common.Models;
using Smart_Librarian.Interfaces;
using Smart_Librarian.Models.Dtos;
using Smart_Librarian.Models.Entity;
using Smart_Librarian.Repositories;

namespace Smart_Librarian.Services
{
    public interface ILoanService
    {
        ResultHub<List<LoanDto>> GetAll();
        ResultHub<LoanDto> Get(int id);
        ResultHub<LoanDto> Add(AddLoanDto addLoanDto);
        ResultHub<LoanDto> Edit(int id, EditLoanDto editLoanDto);
        ResultHub<LoanDto> Remove(int id);
        ResultHub<List<LoanDto>> GetLoansByBookId(int bookId);
        ResultHub<LoanDto> AddLoanWithBook(int bookId, AddLoanDto addLoanDto);

    }

    public class LoanService : ILoanService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LoanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public ResultHub<LoanDto> Add(AddLoanDto addLoanDto)
        {
            var result = new ResultHub<LoanDto>();

            try
            {
                var loanRepository = unitOfWork.GetRepository<Loan>();

                if (addLoanDto == null)
                    return result.IsNullBadRequest();

                if (loanRepository.Exists(x => x.BookId == addLoanDto.BookId && x.MemberId == addLoanDto.MemberId && !x.IsDeleted))
                    return result.IsConflict();

                var data = mapper.Map<Loan>(addLoanDto);

                data.CreatedDate = DateTime.UtcNow;
                data.IsDeleted = false;

                loanRepository.Add(data);
                unitOfWork.Complete();

                return result.SetContent(mapper.Map<LoanDto>(data)).IsCreated();

            }
            catch (Exception ex)
            {

                return result.IsServerError(ex.Message);
            }
        }


        public ResultHub<LoanDto> Edit(int id, EditLoanDto editLoanDto)
        {
            var result = new ResultHub<LoanDto>();

            try
            {
                var loanRepository = unitOfWork.GetRepository<Loan>();

                if (editLoanDto == null)
                    return result.IsNullBadRequest();

                if (loanRepository.Exists(x => x.Id != id && x.BookId == editLoanDto.BookId && x.MemberId == id && !x.IsDeleted))
                    return result.IsConflict();

                var data = loanRepository.GetId(id);

                data.UpdatedDate = DateTime.UtcNow;

                mapper.Map(editLoanDto, data);

                loanRepository.Update(data);
                unitOfWork.Complete();

                return result.Success(mapper.Map<LoanDto>(data)).IsEdited();

            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

        public ResultHub<LoanDto> Get(int id)
        {
            var result = new ResultHub<LoanDto>();

            try
            {
                var loanRepository = unitOfWork.GetRepository<Loan>();

                var data = loanRepository.GetId(id);

                if (data == null)
                    return result.Success(null);

                return result.Success(mapper.Map<LoanDto>(data));
            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

        public ResultHub<List<LoanDto>> GetAll()
        {
            var result = new ResultHub<List<LoanDto>>();

            try
            {
                var loanRepository = unitOfWork.GetRepository<Loan>();

                var data = loanRepository.GetAll();

                if (data == null)
                    return result.Success(null);

                return result.Success(mapper.Map<List<LoanDto>>(data));
            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

        public ResultHub<LoanDto> Remove(int id)
        {
            var result = new ResultHub<LoanDto>();

            try
            {
                var loanRepository = unitOfWork.GetRepository<Loan>();

                var data = loanRepository.GetId(id);

                if (data == null)
                    return result.IsNotFound();

                data.IsDeleted = true;
                data.DeletedDate = DateTime.UtcNow;

                loanRepository.Update(data);

                unitOfWork.Complete();
                return result.IsDeleted();

            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

        public ResultHub<List<LoanDto>> GetLoansByBookId(int bookId)
        {
            var result = new ResultHub<List<LoanDto>>();
            try
            {
                var loanRepository = unitOfWork.GetRepository<Loan>();

                var data = loanRepository.GetListWithIncludes(
                              x => x.BookId == bookId,
                              x => x.Book,
                              x => x.Member
                               );

                if (data == null)
                    return result.Success(null);


                return result.Success(mapper.Map<List<LoanDto>>(data));

            }
            catch (Exception ex)
            {

                return result.IsServerError(ex.Message);
            }
        }

        public ResultHub<LoanDto> AddLoanWithBook(int bookId, AddLoanDto addLoanDto)
        {
            var result = new ResultHub<LoanDto>();

            try
            {
                var bookRepository = unitOfWork.GetRepository<Book>();
                var book = bookRepository.GetId(bookId);
                if (book == null)
                {
                    return result.IsNotFound();
                }

                var loanRepository = unitOfWork.GetRepository<Loan>();
                var newLoan = mapper.Map<Loan>(addLoanDto);
                newLoan.BookId = bookId;

                 loanRepository.Add(newLoan);
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

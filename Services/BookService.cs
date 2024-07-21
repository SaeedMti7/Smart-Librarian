using AutoMapper;
using Common.Extensions;
using Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Smart_Librarian.Interfaces;
using Smart_Librarian.Models.Dtos;
using Smart_Librarian.Models.Entity;
using static StackExchange.Redis.Role;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Smart_Librarian.Services
{
    public interface IBookService
    {
        ResultHub<List<BookDto>> GetAll();
        ResultHub<BookDto> Get(int id);
        ResultHub<BookDto> Add(AddBookDto addBookDto);
        ResultHub<BookDto> Edit(int id, EditBookDto editBookDto);
        ResultHub<BookDto> Remove(int id);
    }

    public class BookService : IBookService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public ResultHub<BookDto> Add(AddBookDto addBookDto)
        {
            var result = new ResultHub<BookDto>();

            try
            {
                var bookRepository = unitOfWork.GetRepository<Book>();

                if (addBookDto == null)
                    return result.IsNullBadRequest();

                if (bookRepository.Exists(x => x.Title == addBookDto.Title && !x.IsDeleted))
                    return result.IsConflict();

                var data = mapper.Map<Book>(addBookDto);

                data.CreatedDate = DateTime.UtcNow;
                data.IsDeleted = false;

                bookRepository.Add(data);
                unitOfWork.Complete();

                return result.SetContent(mapper.Map<BookDto>(data)).IsCreated();

            }
            catch (Exception ex)
            {

                return result.IsServerError(ex.Message);
            }
        }


        public ResultHub<BookDto> Edit(int id, EditBookDto editBookDto)
        {
            var result = new ResultHub<BookDto>();

            try
            {
                var bookRepository = unitOfWork.GetRepository<Book>();

                if (editBookDto == null)
                    return result.IsNullBadRequest();

                if (bookRepository.Exists(x => x.Id != id && x.Title == editBookDto.Title && !x.IsDeleted))
                    return result.IsConflict();

                var data = bookRepository.GetId(id);

                data.UpdatedDate = DateTime.UtcNow;

                mapper.Map(editBookDto, data);

                bookRepository.Update(data);
                unitOfWork.Complete();

                return result.Success(mapper.Map<BookDto>(data)).IsEdited();

            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

        public ResultHub<BookDto> Get(int id)
        {
            var result = new ResultHub<BookDto>();

            try
            {
                var bookRepository = unitOfWork.GetRepository<Book>();

                var data = bookRepository.GetId(id);

                if (data == null)
                    return result.Success(null);

                return result.Success(mapper.Map<BookDto>(data));
            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

        public ResultHub<List<BookDto>> GetAll()
        {
            var result = new ResultHub<List<BookDto>>();

            try
            {
                var bookRepository = unitOfWork.GetRepository<Book>();

                var data = bookRepository.GetAll();

                if (data == null)
                    return result.Success(null);

                return result.Success(mapper.Map<List<BookDto>>(data));
            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

        public ResultHub<BookDto> Remove(int id)
        {
            var result = new ResultHub<BookDto>();

            try
            {
                var bookRepository = unitOfWork.GetRepository<Book>();

                var data = bookRepository.GetId(id);

                if (data == null)
                    return result.IsNotFound();

                data.IsDeleted = true;
                data.DeletedDate = DateTime.UtcNow;

                bookRepository.Update(data);

                unitOfWork.Complete();
                return result.IsDeleted();

            }
            catch (Exception ex)
            {
                return result.IsServerError(ex.Message);
            }
        }

    }
}

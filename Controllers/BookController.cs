using Common.Extensions;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Smart_Librarian.Config.Extensions;
using Smart_Librarian.Models.Dtos;
using Smart_Librarian.Services;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Smart_Librarian.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookService bookService;
        private readonly ILoanService loanService;
        private readonly IDistributedCache distributedCache;

        public BookController(IBookService bookService,ILoanService loanService,IDistributedCache distributedCache)
        {
            this.bookService = bookService;
            this.loanService = loanService;
            this.distributedCache = distributedCache;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var result = new ResultHub<List<BookDto>>();


          //  ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("localhost:6379");
          //  IDatabase db = connection.GetDatabase(); 
         //   db.StringSet("key001", "value001");
           // string myValue = db.StringGet("key001");

            var booksJson = distributedCache.Get("Books");
            if(booksJson != null)
            {
                result = JsonSerializer.Deserialize<ResultHub<List<BookDto>>>(booksJson);
            }
            else
            {
                result = bookService.GetAll();

                string booksJsonData = JsonSerializer.Serialize(result);
                byte[] encodedJson = Encoding.UTF8.GetBytes(booksJsonData);

                var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(10));
                distributedCache.Set("Books", encodedJson,options);

            }


            return result.ToObjectResult();
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var result = bookService.Get(id);
            return result.ToObjectResult();
        }

        [HttpPost]
        public ActionResult Add(AddBookDto addBookDto)
        {
            var result = bookService.Add(addBookDto);

            return result.ToObjectResult(
                IsSuccessAction: r =>
                {
                    ControllerContext.HttpContext.Response.Headers.Location =
                        Url.Action(action: nameof(Get), controller: "Book", values: new { id = r.Content.Id }, protocol: Request.Scheme);
                });
        }


        [HttpPut("{id}")]
        public ActionResult Edit(int id, EditBookDto editBookDto)
        {
            var result = bookService.Edit(id, editBookDto);
            return result.ToObjectResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Remove(int id)
        {
            var result = bookService.Remove(id);
            return result.ToObjectResult();
        }

        [HttpGet("{id}/loans")]
        public  ActionResult<ResultHub<List<LoanDto>>> GetLoansByBookIdAsync(int id)
        {
            var result =  loanService.GetLoansByBookId(id);
            if (result == null)
            {
                return result.IsNoContent();
            }
            return Ok(result);
        }

        [HttpPost("{id}/loans")]
        public ActionResult<ResultHub<LoanDto>> CreateLoanAsync(int id, AddLoanDto addLoanDto)
        {
            var result =  loanService.AddLoanWithBook(id, addLoanDto);
            if (result == null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(GetLoansByBookIdAsync), new { id = result.Content.Id }, result);
        }
    }
}

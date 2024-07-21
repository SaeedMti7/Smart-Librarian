using Microsoft.AspNetCore.Mvc;
using Smart_Librarian.Config.Extensions;
using Smart_Librarian.Models.Dtos;
using Smart_Librarian.Services;

namespace Smart_Librarian.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : Controller
    {
        private readonly ILoanService loanService;

        public LoanController(ILoanService loanService)
        {
            this.loanService = loanService;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var result = loanService.GetAll();
            return result.ToObjectResult();
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var result = loanService.Get(id);
            return result.ToObjectResult();
        }

        [HttpPost]
        public ActionResult Add(AddLoanDto addLoanDto)
        {
            var result = loanService.Add(addLoanDto);

            return result.ToObjectResult(
                IsSuccessAction: r =>
                {
                    ControllerContext.HttpContext.Response.Headers.Location =
                        Url.Action(action: nameof(Get), controller: "Loan", values: new { id = r.Content.Id }, protocol: Request.Scheme);
                });
        }


        [HttpPut("{id}")]
        public ActionResult Edit(int id, EditLoanDto editLoanDto)
        {
            var result = loanService.Edit(id, editLoanDto);
            return result.ToObjectResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = loanService.Remove(id);
            return result.ToObjectResult();
        }
    }
}

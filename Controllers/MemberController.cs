using Common.Extensions;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Smart_Librarian.Config.Extensions;
using Smart_Librarian.Models.Dtos;
using Smart_Librarian.Services;

namespace Smart_Librarian.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : Controller
    {
        private readonly IMemberService memberService;
        private readonly ITestService testService;

        public MemberController(IMemberService memberService,ITestService testService)
        {
            this.memberService = memberService;
            this.testService = testService;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var result = memberService.GetAll();
            return result.ToObjectResult();
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var result = memberService.Get(id);
            return result.ToObjectResult();
        }

        [HttpPost]
        public ActionResult Add(AddMemberDto addMemberDto)
        {
            var result = memberService.Add(addMemberDto);

            return result.ToObjectResult(
                IsSuccessAction: r =>
                {
                    ControllerContext.HttpContext.Response.Headers.Location =
                        Url.Action(action: nameof(Get), controller: "Member", values: new { id = r.Content.Id }, protocol: Request.Scheme);
                });
        }


        [HttpPut("{id}")]
        public ActionResult Edit(int id, EditMemberDto editMemberDto)
        {
            var result = memberService.Edit(id, editMemberDto);
            return result.ToObjectResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var result = memberService.Remove(id);
            return result.ToObjectResult();
        }

        [HttpGet("{id}/loans")]
        public async Task<ActionResult<ResultHub<List<LoanDto>>>> GetLoansByMemberIdAsync(int id)
        {
            var result =  memberService.GetLoansByMemberId(id);
            if (result == null)
            {
                return result.IsNoContent();
            }
            return result.ToObjectResult();
        }

        [HttpPost("{id}/loans")]
        public async Task<ActionResult<ResultHub<LoanDto>>> AddLoanForMember(int id, AddLoanDto addLoanDto)
        {
            var result =  memberService.AddLoanForMember(id, addLoanDto);
            if (result == null)
            {
                return result.IsNoContent();
            }
            return CreatedAtAction(nameof(GetLoansByMemberIdAsync), new { id = result.Content.Id }, result);
        }


        [HttpGet("test")]
        public IActionResult test(int id)
        {
             testService.Invoke();
          
            return Ok();
        }
    }
}

using GameStore.BLL.DTO.Comments;
using GameStore.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers
{
    public class CommentController : Controller
    {
        private CommentService service { get; set; }
        public CommentController(CommentService service)
        {
            this.service = service;

        }

        [HttpPost("games/{key}/comments")]
        public async Task<IActionResult> AddComment(string key, [FromBody] AddCommentRequest commentRequest)
        {

            await service.FormatCommentAndAdd(key, commentRequest);
            return Ok();

        }
        [HttpGet("games/{key}/comments")]
        public async Task<ActionResult<IEnumerable<GetCommentRequest>>> GetAllComments(string key)
        {

            var allcomments = await service.GetAllComentsWithIndefiniteChildren(key);
            return Ok(allcomments);


        }
        [HttpDelete("games/{key}/comments/{id}")]
        public async Task<IActionResult> DeleteComment(string key, Guid id)
        {
            //don't know why is game key needed

            await service.DeleteCommentByGameKeyAndCommentId(key, id);
            return Ok();


        }
        [HttpGet("comments/ban/durations")]
        public async Task<ActionResult<IEnumerable<string>>> GetBanDurations()
        {
            return Ok(new List<string>() { "1 hour", "1 day", "1 week", "1 month", "pernament" });
        }
        // [HttpPost("comments/ban")]
        // public async Task<ActionResult> BanUser([FromBody] POST_Ban banRequest)
        // {
        //     service.BanUser(banRequest.user, banRequest.duration);
        // }
    }
}

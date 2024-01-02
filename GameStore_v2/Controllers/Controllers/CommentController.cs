using GameStore.BLL.DTO;
using GameStore.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers.Controllers
{
    public class CommentController : Controller
    {
        private CommentService service { get; set; }
        public CommentController(CommentService service)
        {
            this.service = service;

        }

        [HttpPost("games/{key}/comments")]
        public async Task<IActionResult> AddComment(string key, [FromBody] POST_Comment commentRequest)
        {
            try
            {
                await service.FormatCommentAndAdd(key, commentRequest);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(); }
        }
        [HttpGet("games/{key}/comments")]
        public async Task<ActionResult<IEnumerable<GET_Comment>>> GetAllComments(string key)
        {
            try
            {
                var allcomments = await service.GetAllComentsWithIndefiniteChildren(key);
                return Ok(allcomments);

            }
            catch (Exception ex) { return BadRequest(); }
        }
        [HttpDelete("games/{key}/comments/{id}")]
        public async Task<IActionResult> DeleteComment(string key, Guid id)
        {
            //don't know why is game key needed
            try
            {
                await service.DeleteCommentByGameKeyAndCommentId(key, id);
                return Ok();
            }
            catch (Exception ex) { return BadRequest(); }
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

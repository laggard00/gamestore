using Microsoft.AspNetCore.Mvc;

namespace GameStore.WEB.Controllers
{
    public class FiltrationSortPageinationOptionsController : Controller
    {
        [HttpGet("games/pagination-options")]
        public async Task<ActionResult<IEnumerable<string>>> GetPaiginationOptions()
        {
            try 
            {
                return Ok(new List<string>() { "10", "20", "50", "100", "all" });
            }
            catch (Exception ex) { return  BadRequest(ex.Message); }
        }
        [HttpGet("games/sorting-options")]
        public async Task<ActionResult<IEnumerable<string>>> GetSortingOptions()
        {
            try 
            {
                return Ok(new List<string>() { "Most popular", "Most commented", "Price ASC", "Price DESC", "New" });
            }
            catch (Exception ex) { return BadRequest(); }
        }
        [HttpGet("games/publish-date-options")]
        public async Task<ActionResult<IEnumerable<string>>> GetPublishDateFilterOptions()
        {
            try 
            {
                return Ok(new List<string>() { "last week", "last month", "last year", "2 years", "3 years" });
            }
            catch(Exception ex) { return BadRequest(); }
        }
    }
}

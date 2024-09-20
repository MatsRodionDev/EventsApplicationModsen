using EventsApplication.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EventsApplication.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetEventCategories()
        {
            var categories = Enum.GetNames(typeof(EventCategory));

            return Ok(categories);
        }
    }
}

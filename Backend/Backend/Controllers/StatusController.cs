namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController(MyService service) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<StatusDto>> GetAllStatus()
        {
            return Ok(service.GetAllStatus());
        }
        [HttpPost]
        public ActionResult AddStatus(StatusDto status)
        {
            var response = service.AddStatus(status);
            return response.Status ? Ok() : BadRequest(response.Message);
        }
    }
}

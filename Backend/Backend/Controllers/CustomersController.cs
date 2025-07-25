namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController(MyService service) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<CustomerDto>> GetAllCustomers()
        {
            return Ok(service.GetAllCustomers());
        }
        [HttpPost]
        public ActionResult PostCustomer(CustomerDto newCustomer)
        {
            var response = service.AddCustomer(newCustomer);
            return response.Status ? Ok() : BadRequest(response.Message);
        }
        [HttpPut]
        public ActionResult PutCustomer(CustomerDto newCustomer)
        {
            var response = service.UpdateCustomer(newCustomer);
            return response.Status ? Ok() : BadRequest(response.Message);
        }
        [HttpDelete]
        public ActionResult DeleteCustomer(int customerId)
        {
            var response = service.DeleteCustomer(customerId);
            return response.Status ? Ok() : BadRequest(response.Message);
        }
    }
}

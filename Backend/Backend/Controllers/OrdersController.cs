using Backkend.Dtos;
using System.Text.RegularExpressions;

namespace Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController(MyService service) : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<OrderDto>> GetAllOrders()
        {
            return Ok(service.GetAllOrders());
        }
        [HttpGet("page/{pageNr}")]
        public ActionResult<List<OrderDto>> GetOrdersOfPage(int pageNr)
        {
            return Ok(service.GetOrdersOfPage(pageNr));
        }
        [HttpGet("filter/{filterTxt}")]
        public ActionResult<List<OrderDto>> GetOrdersFiltered(string filterTxt)
        {
            return Ok(service.GetOrdersFiltered(filterTxt));
        }
        [HttpGet("YearInfo/{year}")]
        public ActionResult<YearInfoDto> GetYearInfo(string year)
        {
            if (!Regex.Match(year, @"\d{4}").Success) return BadRequest("Invalid year input!");
            var response = service.GetYearInfo(year);
            return Ok(response);
        }
        [HttpGet("MonthInfos/{year}")]
        public ActionResult<List<MonthInfo>> GetMonthInfos(string year)
        {
            if (!Regex.Match(year, @"\d{4}").Success) return BadRequest("Invalid year input!");
            var response = service.GetMonthsInfos(year);
            return Ok(response);
        }
        [HttpPost]
        public ActionResult PostOrder(OrderDto newOrder)
        {
            var response = service.AddOrder(newOrder);
            return response.Status ? Ok() : BadRequest(new { message = response.Message });
        }
        [HttpPut("{oldOrderId}")]
        public ActionResult UpdateOrder(int oldOrderId, OrderDto newOrder)
        {
            var response = service.UpdateOrder(oldOrderId, newOrder);
            return response.Status ? Ok() : BadRequest(new { message = response.Message });
        }
        [HttpPut("Status")]
        public ActionResult UpdateStatusOfOrders(List<OrderDto> orders)
        {
            var response = service.UpdateStatusOfOrders(orders);
            return response.Status ? Ok() : BadRequest(new { message = response.Message });
        }
        [HttpPost("createPdf")]
        public ActionResult CreatePdfForOrders(List<string> orders)
        {
            var response = service.CreatePdfForOrders(orders);
            return response.Status ? Ok() : BadRequest(new { message = response.Message });
        }
        [HttpDelete]
        public ActionResult DeleteOrder(List<int> orderIds)
        {
            var response = service.DeleteOrder(orderIds);
            return response.Status ? Ok() : BadRequest(new { message = response.Message });
        }
        [HttpPost("import/csv")]
        public ActionResult ImportOrders(List<string> newOrderLines)
        {
            var response = service.ImportOrdersFromCsv(newOrderLines);
            return response.Status ? Ok() : BadRequest(new { message = response.Message });
        }
        [HttpPost("OpenPdf")]
        public ActionResult OpenPdf(string? po, string? bill, string? billCreatedDate)
        {
            var response = service.OpenPdf(po, bill, billCreatedDate);
            return response.Status ? Ok() : BadRequest(new { message = response.Message });
        }
    }
}

using InterviewPrep.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewPrep.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        _orderService = new OrderService(config.GetConnectionString("DefaultConnection"));
    }

    [HttpPost("{orderId}/process")]
    public IActionResult ProcessOrder(int orderId)
    {
        try
        {
            _orderService.ProcessOrder(orderId);
            return Ok(new { message = "Order processed successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("user/{userId}")]
    public IActionResult GetUserOrders(int userId)
    {
        var orders = _orderService.GetUserOrders(userId);
        return Ok(orders);
    }
}

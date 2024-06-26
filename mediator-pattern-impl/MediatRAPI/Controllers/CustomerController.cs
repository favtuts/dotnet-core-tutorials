using MediatR;
using MediatRHandler.DTOs;
using MediatRHandler.Entities;
using MediatRHandler.Requests;
using Microsoft.AspNetCore.Mvc;

namespace MediatRAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController: ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator) 
    {
        _mediator = mediator;
    }

    [HttpGet("customerId")]
    public async Task<ActionResult<Customer?>> GetCustomerAsync(int customerId)
    {
        var customerDetails = await _mediator.Send(new GetCustomerRequest() { CustomerId = customerId});
        if (customerDetails==null)
        {
            return NotFound();
        }
        return Ok(customerDetails);
    }

    [HttpPost]
    public async Task<ActionResult<int>> CreateCustomerAsync(AddCustomerDTO addCustomerDTO) {
        var customerId = await _mediator.Send(new CreateCustomerRequest() { CustomerDTO = addCustomerDTO});
        if (customerId>0)
        {
            return Ok(customerId);
        }
        else
        {   
            return BadRequest(customerId);
        }        
    }
}

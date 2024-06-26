using MediatR;
using MediatRHandler.Entities;

namespace MediatRHandler.Requests
{
    public class GetCustomerRequest: IRequest<Customer?>
    {
        public int CustomerId { get; set; }
    }
}
using MediatR;
using MediatRHandler.DTOs;

namespace MediatRHandler.Requests
{
    public class CreateCustomerRequest: IRequest<int>
    {
        public AddCustomerDTO? CustomerDTO { get; set; }
    }
}
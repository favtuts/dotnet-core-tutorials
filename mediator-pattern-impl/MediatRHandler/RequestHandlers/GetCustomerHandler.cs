using MediatR;
using MediatRHandler.Entities;
using MediatRHandler.Data;
using MediatRHandler.Requests;

namespace MediatRHandler.RequestHandlers;

public class GetCustomerHandler : IRequestHandler<GetCustomerRequest, Customer?>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer?> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        return await _customerRepository.GetById(request.CustomerId);
    }
}
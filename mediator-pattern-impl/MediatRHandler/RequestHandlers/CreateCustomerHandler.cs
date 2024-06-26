using MediatR;
using MediatRHandler.Data;
using MediatRHandler.Entities;
using MediatRHandler.Requests;

namespace MediatRHandler.RequestHandlers;
public class CreateCustomerHandler: IRequestHandler<CreateCustomerRequest, int>
{
    //Inject Validators
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<int> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        // First validate the request
        if (request.CustomerDTO == null) 
        {
            return -1;
        }

        // Mapping DTO to Entity
        Customer customerEntity = new Customer { 
            FirstName = request.CustomerDTO.FirstName,
            LastName = request.CustomerDTO.LastName,
            EmailAddress= request.CustomerDTO.EmailAddress,
            Address = request.CustomerDTO.Address,
        };

        // Save Customer entity to DB
        return await _customerRepository.Create(customerEntity);
    }
}
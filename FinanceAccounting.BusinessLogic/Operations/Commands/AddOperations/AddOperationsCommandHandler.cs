using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Operations.Commands.AddOperations
{
    public class AddOperationsCommandHandler : IRequestHandler<AddOperationsCommand, IEnumerable<OperationDto>>
    {
        private readonly IOperationRepo _repo;
        private readonly IMapper _mapper;

        public AddOperationsCommandHandler(IOperationRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OperationDto>> Handle(AddOperationsCommand request, CancellationToken cancellationToken)
        {
            var addedOperations = new List<Operation>();
            foreach (CreateOperationDto operation in request.Operations)
            {
                var newOperation = new Operation
                {
                    UserId = request.UserId,
                    Type = await _repo.GetOperationTypeByCategoryIdAsync(operation.CategoryId),
                    Date = operation.Date,
                    CategoryId = operation.CategoryId,
                    Sum = operation.Sum,
                    Details = operation.Details
                };

                await _repo.AddAsync(newOperation, true, cancellationToken);
                addedOperations.Add(newOperation);
            }

            return _mapper.Map<IEnumerable<OperationDto>>(addedOperations);
        }
    }
}

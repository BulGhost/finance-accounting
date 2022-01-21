using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Operations.Commands.DeleteOperations
{
    public class DeleteOperationsCommandHandler : IRequestHandler<DeleteOperationsCommand, IEnumerable<int>>
    {
        private readonly IOperationRepo _repo;

        public DeleteOperationsCommandHandler(IOperationRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<int>> Handle(DeleteOperationsCommand request, CancellationToken cancellationToken)
        {
            var deletedOperationIds = new List<int>();
            foreach (int operationId in request.OperationIds)
            {
                Operation operation = await _repo.FindAsync(operationId, cancellationToken);

                if (operation == null || operation.UserId != request.UserId)
                {
                    continue;
                }

                await _repo.DeleteAsync(operation, true, cancellationToken);
                deletedOperationIds.Add(operationId);
            }

            return deletedOperationIds;
        }
    }
}

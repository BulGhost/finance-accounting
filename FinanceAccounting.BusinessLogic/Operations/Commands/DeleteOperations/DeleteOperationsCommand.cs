using System.Collections.Generic;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Operations.Commands.DeleteOperations
{
    public record DeleteOperationsCommand(int UserId, int[] OperationIds) : IRequest<IEnumerable<int>>;
}

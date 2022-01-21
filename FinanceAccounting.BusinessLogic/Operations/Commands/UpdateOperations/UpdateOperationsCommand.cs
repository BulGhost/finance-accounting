using System.Collections.Generic;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Operations.Commands.UpdateOperations
{
    public record UpdateOperationsCommand(int UserId, UpdateOperationDto[] Operations)
        : IRequest<IEnumerable<OperationDto>>;
}

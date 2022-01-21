using System.Collections.Generic;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Operations.Commands.AddOperations
{
    public record AddOperationsCommand(int UserId, CreateOperationDto[] Operations)
        : IRequest<IEnumerable<OperationDto>>;
}

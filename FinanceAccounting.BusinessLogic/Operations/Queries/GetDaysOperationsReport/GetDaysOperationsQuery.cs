using System;
using System.Collections.Generic;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Operations.Queries.GetDaysOperationsReport
{
    public record GetDaysOperationsQuery(int UserId, DateTime Date) : IRequest<IEnumerable<OperationDto>>;
}

using System;
using System.Collections.Generic;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Operations.Queries.GetPeriodsOperationsReport
{
    public record GetPeriodsOperationsQuery(int UserId, DateTime StartDate, DateTime FinalDate)
        : IRequest<IEnumerable<OperationDto>>;
}

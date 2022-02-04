using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Operations.Queries.GetOperationById
{
    public record GetOperationByIdQuery(int UserId, int OperationId) : IRequest<OperationDto>;
}

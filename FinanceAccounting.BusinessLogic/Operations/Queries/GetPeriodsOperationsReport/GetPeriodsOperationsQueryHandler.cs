using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.Domain.Repository;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Operations.Queries.GetPeriodsOperationsReport
{
    public class GetPeriodsOperationsQueryHandler : IRequestHandler<GetPeriodsOperationsQuery, IEnumerable<OperationDto>>
    {
        private readonly IOperationRepo _repo;
        private readonly IMapper _mapper;

        public GetPeriodsOperationsQueryHandler(IOperationRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OperationDto>> Handle(GetPeriodsOperationsQuery request, CancellationToken cancellationToken)
        {
            var operations = await _repo.GetUserOperationsOnDateRangeAsync(request.UserId, request.StartDate, request.FinalDate);

            return _mapper.Map<IEnumerable<OperationDto>>(operations);
        }
    }
}

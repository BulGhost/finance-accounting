using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.Domain.Repository;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Operations.Queries.GetDaysOperationsReport
{
    public class GetDaysOperationsQueryHandler : IRequestHandler<GetDaysOperationsQuery, IEnumerable<OperationDto>>
    {
        private readonly IOperationRepo _repo;
        private readonly IMapper _mapper;

        public GetDaysOperationsQueryHandler(IOperationRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OperationDto>> Handle(GetDaysOperationsQuery request, CancellationToken cancellationToken)
        {
            var operations = await _repo.GetUserOperationsOnDateAsync(request.UserId, request.Date);

            return _mapper.Map<IEnumerable<OperationDto>>(operations);
        }
    }
}

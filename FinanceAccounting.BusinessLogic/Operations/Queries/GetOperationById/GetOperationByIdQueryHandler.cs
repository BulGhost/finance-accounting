using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Operations.Queries.GetOperationById
{
    public class GetOperationByIdQueryHandler : IRequestHandler<GetOperationByIdQuery, OperationDto>
    {
        private readonly IOperationRepo _repo;
        private readonly IMapper _mapper;

        public GetOperationByIdQueryHandler(IOperationRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<OperationDto> Handle(GetOperationByIdQuery request, CancellationToken cancellationToken)
        {
            Operation operation = await _repo.GetUserOperationByIdAsync(request.UserId, request.OperationId, cancellationToken);

            return _mapper.Map<OperationDto>(operation);
        }
    }
}

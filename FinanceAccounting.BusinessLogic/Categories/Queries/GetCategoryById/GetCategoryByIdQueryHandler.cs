using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly ICategoryRepo _repo;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(ICategoryRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            Category category = await _repo.GetUserCategoryByIdAsync(request.UserId, request.CategoryId, cancellationToken);

            return _mapper.Map<CategoryDto>(category);
        }
    }
}

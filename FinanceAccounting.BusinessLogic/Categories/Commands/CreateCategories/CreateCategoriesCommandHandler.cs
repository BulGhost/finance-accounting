using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Categories.Commands.CreateCategories
{
    public class CreateCategoriesCommandHandler : IRequestHandler<CreateCategoriesCommand, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryRepo _repo;
        private readonly IMapper _mapper;

        public CreateCategoriesCommandHandler(ICategoryRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(CreateCategoriesCommand request, CancellationToken cancellationToken)
        {
            var addedCategories = new List<Category>();
            foreach (CreateCategoryDto category in request.Categories)
            {
                if (await _repo.IsCategoryExistsAsync(request.UserId, category.Type, category.Name, cancellationToken))
                {
                    continue;
                }

                var newCategory = new Category
                {
                    Type = category.Type,
                    CategoryName = category.Name,
                    UserId = request.UserId
                };
                await _repo.AddAsync(newCategory, true, cancellationToken);
                addedCategories.Add(newCategory);
            }

            return _mapper.Map<IEnumerable<CategoryDto>>(addedCategories);
        }
    }
}
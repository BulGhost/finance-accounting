using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Categories.Commands.UpdateCategories
{
    public class UpdateCategoriesCommandHandler : IRequestHandler<UpdateCategoriesCommand, IEnumerable<CategoryDto>>
    {
        private readonly ICategoryRepo _repo;
        private readonly IMapper _mapper;

        public UpdateCategoriesCommandHandler(ICategoryRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> Handle(UpdateCategoriesCommand request, CancellationToken cancellationToken)
        {
            var updatedCategories = new List<Category>();
            foreach (UpdateCategoryDto category in request.Categories)
            {
                Category categoryToUpdate = await _repo.FindAsync(category.Id, cancellationToken);
                categoryToUpdate.CategoryName = category.Name;
                await _repo.UpdateAsync(categoryToUpdate, true, cancellationToken);
                updatedCategories.Add(categoryToUpdate);
            }

            return _mapper.Map<IEnumerable<CategoryDto>>(updatedCategories);
        }
    }
}

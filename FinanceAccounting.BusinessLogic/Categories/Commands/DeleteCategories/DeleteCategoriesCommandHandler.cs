using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinanceAccounting.Domain.Entities;
using FinanceAccounting.Domain.Repository;
using MediatR;

namespace FinanceAccounting.BusinessLogic.Categories.Commands.DeleteCategories
{
    public class DeleteCategoriesCommandHandler : IRequestHandler<DeleteCategoriesCommand, IEnumerable<int>>
    {
        private readonly ICategoryRepo _repo;

        public DeleteCategoriesCommandHandler(ICategoryRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<int>> Handle(DeleteCategoriesCommand request, CancellationToken cancellationToken)
        {
            var deletedCategoryIds = new List<int>();
            foreach (int categoryId in request.CategoryIds)
            {
                Category categoryToDelete = await _repo.FindAsync(categoryId, cancellationToken);
                await _repo.DeleteAsync(categoryToDelete, true, cancellationToken);
                deletedCategoryIds.Add(categoryToDelete.Id);
            }

            return deletedCategoryIds;
        }
    }
}

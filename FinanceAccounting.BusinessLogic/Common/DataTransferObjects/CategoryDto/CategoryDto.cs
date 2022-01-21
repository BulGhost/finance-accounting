using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.Mappings;
using FinanceAccounting.Domain.Entities;

namespace FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto
{
    public class CategoryDto : IMappable
    {
        public int Id { get; set; }
        public OperationType Type { get; set; }
        public string CategoryName { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<Category, CategoryDto>();
    }
}
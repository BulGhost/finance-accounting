using System.Linq;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.CategoryDto;
using FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto;
using FinanceAccounting.Domain.Entities;

namespace FinanceAccounting.BusinessLogic.Tests.Stubs
{
    public class MappingProfileStub : Profile
    {
        public MappingProfileStub()
        {
            var categoriesRepo = new CategoryRepoStub().GetAllAsync().Result;
            CreateMap<Category, CategoryDto>();
            CreateMap<Operation, OperationDto>()
                .ForMember(c => c.CategoryName, opt => opt.MapFrom(o =>
                    categoriesRepo.FirstOrDefault(c => c.Id == o.CategoryId).CategoryName));
        }
    }
}

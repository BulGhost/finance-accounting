using System;
using AutoMapper;
using FinanceAccounting.BusinessLogic.Common.Mappings;
using FinanceAccounting.Domain.Entities;

namespace FinanceAccounting.BusinessLogic.Common.DataTransferObjects.OperationDto
{
    public class OperationDto : IMappable
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public OperationType Type { get; set; }
        public string CategoryName { get; set; }
        public decimal Sum { get; set; }
        public string Details { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Operation, OperationDto>()
                .ForMember(dto => dto.CategoryName,
                    opt => opt.MapFrom<CategoryNameValueResolver>());
        }
    }
}

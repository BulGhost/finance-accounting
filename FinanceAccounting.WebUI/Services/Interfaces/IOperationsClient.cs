using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.DTO;
using FinanceAccounting.WebUI.Entities.Models.ReportInterval;
using FinanceAccounting.WebUI.Entities.Models.Requests;

namespace FinanceAccounting.WebUI.Services.Interfaces
{
    public interface IOperationsClient
    {
        Task<OperationsReportDto> GetOperationsReport(IReportInterval reportInterval);
        Task<OperationDto> GetOperationById(int operationId);
        Task<CommandResponseDto> AddOperation(AddOperationRequest newOperation);
        Task<CommandResponseDto> UpdateOperation(UpdateOperationRequest updatedOperation);
        Task<CommandResponseDto> DeleteOperation(int operationId);
    }
}

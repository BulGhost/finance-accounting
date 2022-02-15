using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.Enums;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Shared
{
    public partial class OperationTypeToggle
    {
        public OperationType OperationType { get; set; }

        [Parameter]
        public EventCallback OperationTypeChanged { get; set; }

        private async Task OnOperationTypeChanged()
        {
            OperationType = OperationType == OperationType.Expense
                ? OperationType.Income
                : OperationType.Expense;

            if (OperationTypeChanged.HasDelegate)
            {
                await OperationTypeChanged.InvokeAsync();
            }
        }
    }
}

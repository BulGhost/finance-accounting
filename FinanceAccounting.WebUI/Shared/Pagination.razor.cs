using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.WebUI.Entities.Models;
using Microsoft.AspNetCore.Components;

namespace FinanceAccounting.WebUI.Shared
{
    public partial class Pagination
    {
        private List<PagingLink> _links;

        [Parameter]
        public PaginationData PaginationData { get; set; }

        [Parameter]
        public int Spread { get; set; }

        [Parameter]
        public EventCallback<int> PageSelected { get; set; }

        protected override void OnParametersSet()
        {
            CreatePaginationLinks();
        }

        private void CreatePaginationLinks()
        {
            _links = new List<PagingLink> { new(PaginationData.CurrentPage - 1, PaginationData.HasPrevious, "Previous") };

            for (int i = 1; i <= PaginationData.TotalPages; i++)
            {
                if (i >= PaginationData.CurrentPage - Spread && i <= PaginationData.CurrentPage + Spread)
                {
                    _links.Add(new PagingLink(i, true, i.ToString()) { Active = PaginationData.CurrentPage == i });
                }
            }

            _links.Add(new PagingLink(PaginationData.CurrentPage + 1, PaginationData.HasNext, "Next"));
        }

        private async Task OnPageSelected(PagingLink link)
        {
            if (link.Page == PaginationData.CurrentPage || !link.Enabled)
            {
                return;
            }

            PaginationData.CurrentPage = link.Page;
            await PageSelected.InvokeAsync(link.Page);
        }
    }
}

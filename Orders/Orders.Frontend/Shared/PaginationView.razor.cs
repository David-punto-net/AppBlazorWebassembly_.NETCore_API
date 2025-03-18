using Microsoft.AspNetCore.Components;

namespace Orders.Frontend.Shared;

public partial class PaginationView
{
    private int totalPages;
    private int startPage;
    private int endPage;

    [Parameter] public int CurrentPage { get; set; } = 1;
    [Parameter] public EventCallback<int> CurrentPageChanged { get; set; }
    [Parameter] public int TotalItems { get; set; }
    [Parameter] public int ItemsPerPage { get; set; } = 2;
    [Parameter] public int MaxVisiblePages { get; set; } = 5;

    protected override void OnParametersSet()
    {
        CalculateTotalPages();
        CalculateVisiblePages();
        ValidateCurrentPage();
       
    }

    private void CalculateTotalPages()
    {
        totalPages = (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
        totalPages = Math.Max(1, totalPages);
    }

    private void ValidateCurrentPage()
    {
        if (CurrentPage < 1) CurrentPage = 1;
        if (CurrentPage > totalPages) CurrentPage = totalPages;
    }

    private void CalculateVisiblePages()
    {
        startPage = Math.Max(1, CurrentPage - (MaxVisiblePages / 2));
        endPage = Math.Min(totalPages, startPage + MaxVisiblePages - 1);

        if (endPage - startPage + 1 < MaxVisiblePages)
        {
            startPage = Math.Max(1, endPage - MaxVisiblePages + 1);
        }
    }

    private IEnumerable<int> GetVisiblePages()
    {
        for (var i = startPage; i <= endPage; i++)
        {
            yield return i;
        }
    }

    private async Task NavigateToPage(int page)
    {
        if (page < 1 || page > totalPages) return;

        CurrentPage = page;
        await CurrentPageChanged.InvokeAsync(CurrentPage);
    }

}
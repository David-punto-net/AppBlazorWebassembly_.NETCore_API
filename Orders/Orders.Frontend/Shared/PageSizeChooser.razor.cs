using Microsoft.AspNetCore.Components;

namespace Orders.Frontend.Shared
{
    public partial class PageSizeChooser
    {
        [Parameter] public int ItemsPerPage { get; set; }
        [Parameter] public List<int> Options { get; set; } = new();
        [Parameter] public EventCallback<int> SelectedValueChanged { get; set; }

        private async Task HandleChange(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int value))
            {
                ItemsPerPage = value;
                await SelectedValueChanged.InvokeAsync(value);
            }
        }


    }
}
using Microsoft.AspNetCore.Components;

namespace Orders.Frontend.Shared
{
    public partial class FilterGrid
    {
        [Parameter, SupplyParameterFromQuery] public string TextToFilter { get; set; } = string.Empty; 
        [Parameter] public string PlaceHolder { get; set; } = string.Empty;
        [Parameter] public Func<string, Task> CallBack { get; set; }= async (text) => await Task.CompletedTask;

        private async Task CleanFilterAsync()
        {
            await CallBack(string.Empty);  
        }

        private async Task ApplyFilterAsync()
        {
            await CallBack(TextToFilter);
        }

    }
}
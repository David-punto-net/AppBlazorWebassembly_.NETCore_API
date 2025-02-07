using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Orders.Frontend.Shared
{
    public partial class InputImg
    {
        private string? imageBase64;

        [Parameter] public string? Label { get; set; } = "Imagen";
        [Parameter] public string? ImageURL { get; set; }
        [Parameter] public EventCallback<string> ImageSelected { get; set; }

        IList<IBrowserFile> _files = new List<IBrowserFile>();


        private async Task OnChange(InputFileChangeEventArgs e)
        {
            var imagenes = e.GetMultipleFiles();

            foreach (var imagen in imagenes)
            {
                var arrBytes = new byte[imagen.Size];
                await imagen.OpenReadStream().ReadAsync(arrBytes);
                imageBase64 = Convert.ToBase64String(arrBytes);
                ImageURL = null;

                await ImageSelected.InvokeAsync(imageBase64);
                StateHasChanged();
            }
        }

        private async Task UploadFiles(IReadOnlyList<IBrowserFile> files)
        {
            foreach (var file in files)
            {
                if (!_files.Any(f => f.Name == file.Name))
                {
                    _files.Add(file);

                    var arrBytes = new byte[file.Size];
                    await file.OpenReadStream().ReadAsync(arrBytes);
                    imageBase64 = Convert.ToBase64String(arrBytes);
                    ImageURL = null;

                    await ImageSelected.InvokeAsync(imageBase64);
                    StateHasChanged();
                }
            }
        }
    }
}
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Orders.Frontend.Helpers;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Products
{
    public partial class ProductForm
    {
        private EditContext editContext = null!;
        private string? imageUrl;
        private List<MultipleSelectorModel> selected { get; set; } = new();
        private List<MultipleSelectorModel> nonSelected { get; set; } = new();
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Parameter, EditorRequired] public ProductDTO ProductDTO { get; set; } = null!;
        [Parameter, EditorRequired] public EventCallback OnValidSubmit { get; set; }
        [Parameter, EditorRequired] public EventCallback ReturnAction { get; set; }
        [Parameter, EditorRequired] public List<Category> NonSelectedCategories { get; set; } = new();
        [Parameter] public bool IsEdit { get; set; } = false;
        [Parameter] public EventCallback AddImageAction { get; set; }
        [Parameter] public EventCallback RemoveImageAction { get; set; }
        [Parameter] public List<Category> SelectedCategories { get; set; } = new();
        public bool FormPressCreate { get; set; } = false;
        public bool FormPressReturn { get; set; } = false;

        protected override void OnInitialized()
        {
            editContext = new(ProductDTO);
            selected = SelectedCategories.Select(x => new MultipleSelectorModel(x.Id.ToString(), x.Name)).ToList();
            nonSelected = NonSelectedCategories.Select(x => new MultipleSelectorModel(x.Id.ToString(), x.Name)).ToList();
        }

        private void ImageSelected(string imagenBase64)

        {
            if (ProductDTO.ProductImages is null)
            {
                ProductDTO.ProductImages = new List<string>();
            }
            ProductDTO.ProductImages!.Add(imagenBase64);
            imageUrl = null;
        }

        private async Task OnDataAnnotationsValidatedAsync()
        {
            ProductDTO.ProductCategoryIds = selected.Select(x => int.Parse(x.Key)).ToList();
            await OnValidSubmit.InvokeAsync();
        }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {

            if (!FormPressCreate)
            {
                if (ProductDTO.Name != null)
                {
                    var result = await SweetAlertService.FireAsync(new SweetAlertOptions
                    {
                        Title = "Confirmación",
                        Text = "¿Deseas abandonar la página sin guardar los cambios?",
                        Icon = SweetAlertIcon.Warning,
                        ShowCancelButton = true,
                        ConfirmButtonText = "Sí, salir",
                        CancelButtonText = "No, permanecer aquí",
                    });
                    var confirm = !string.IsNullOrEmpty(result.Value);
                    if (!confirm)
                    {
                        FormPressReturn = false;
                        context.PreventNavigation();
                    }
                }
            }
        }
    }
}
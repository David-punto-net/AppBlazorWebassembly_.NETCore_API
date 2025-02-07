using Blazored.Modal.Services;
using Blazored.Modal;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using System.Diagnostics.Metrics;

namespace Orders.Frontend.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public partial class ProductCreate
    {
        private ProductDTO productDTO = new()
        {
            ProductCategoryIds = new List<int>(),
            ProductImages = new List<string>()
        };

        private ProductForm? productForm;
        private List<Category> selectedCategories = new();
        private List<Category> nonSelectedCategories = new();
        private bool loading = true;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            var httpActionResponse = await Repository.GetAsync<List<Category>>("/api/categories/combo");
            loading = false;

            if (httpActionResponse.Error)
            {
                var message = await httpActionResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            nonSelectedCategories = httpActionResponse.Response!;
        }

        private async Task CreateAsync()
        {
            if (!productForm!.FormPressReturn)
            {
                productForm!.FormPressCreate = true;

                var httpActionResponse = await Repository.PostAsync("/api/products/full", productDTO);
                if (httpActionResponse.Error)
                {
                    var message = await httpActionResponse.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }

                //await BlazoredModal.CloseAsync(ModalResult.Ok());

                NavigationManager.NavigateTo("/products");

                var toast = SweetAlertService.Mixin(new SweetAlertOptions
                {
                    Toast = true,
                    Position = SweetAlertPosition.BottomEnd,
                    ShowConfirmButton = true,
                    Timer = 3000
                });

                await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
            }

        }

        private void Return()
        {

            productForm!.FormPressReturn = true;
            productForm!.FormPressCreate = false;
            NavigationManager.NavigateTo("/products");
        }
    }
}
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Categories
{
    public partial class CategoryCreate
    {
        private Category category = new();

        private FormWithName<Category>? categoryForm;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        private async Task CreateAsync()
        {
            if (!categoryForm!.FormPressReturn)
            {
                categoryForm!.FormPressCreate = true;

                var responseHttp = await Repository.PostAsync("/api/categories", category);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }

                NavigationManager.NavigateTo("/categories");

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

        private async Task Return()
        {
            categoryForm!.FormPressReturn = true;
            categoryForm!.FormPressCreate = false;
            NavigationManager.NavigateTo("/categories");
        }
    }
}
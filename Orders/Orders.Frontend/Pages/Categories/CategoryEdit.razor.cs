using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Categories
{
    public partial class CategoryEdit
    {
        private Category? category;

        private FormWithName<Category>? categoryForm;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [EditorRequired, Parameter] public int Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (Id != 0)
            {
                var responseHttp = await Repository.GetAsync<Category>($"/api/categories/{Id}");
                if (responseHttp.Error)
                {
                    if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        NavigationManager.NavigateTo("/categories");
                    }
                    else
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    }
                }
                else
                {
                    category = responseHttp.Response;
                }
            }
        }

        private async Task UpdateAsync()
        {
            if (!categoryForm!.FormPressReturn)
            {
                categoryForm!.FormPressCreate = true;

                var responseHttp = await Repository.PutAsync("/api/categories", category);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message);
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

                await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro modificado con éxito.");
            }
        }

        private void Return()
        {
            categoryForm!.FormPressReturn = true;
            categoryForm!.FormPressCreate = false;
            NavigationManager.NavigateTo("/categories");
        }
    }
}
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Cities
{
    public partial class CityEdit
    {
        private City? city;

        private FormWithName<City>? cityForm;

        private bool regreso = false;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [EditorRequired, Parameter] public int Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (Id != 0)
            {
                var responseHttp = await Repository.GetAsync<City>($"/api/cities/{Id}");
                if (responseHttp.Error)
                {
                    if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        NavigationManager.NavigateTo($"/states/details/{city!.StateId}");
                    }
                    else
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    }
                }
                else
                {
                    city = responseHttp.Response;
                }
            }
        }

        private async Task UpdateAsync()
        {
            if (!regreso)
            {
                cityForm!.FormPressCreate = true;

                var responseHttp = await Repository.PutAsync("/api/cities", city);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message);
                    return;
                }

                NavigationManager.NavigateTo($"/states/details/{city!.StateId}");

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
            regreso = true;
            cityForm!.FormPressCreate = false;
            NavigationManager.NavigateTo($"/states/details/{city!.StateId}");
        }
    }
}

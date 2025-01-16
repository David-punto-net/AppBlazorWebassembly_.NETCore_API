using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Countries
{
    public partial class CountryEdit
    {
        private Country? country;

        private FormWithName<Country>? countryForm;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [EditorRequired, Parameter] public int Id { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (Id != 0)
            {
                var responseHttp = await Repository.GetAsync<Country>($"/api/countries/{Id}");
                if (responseHttp.Error)
                {
                    if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        NavigationManager.NavigateTo("/countries");
                    }
                    else
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    }
                }
                else
                {
                    country = responseHttp.Response;
                }
            }
        }

        private async Task UpdateAsync()
        {
            var responseHttp = await Repository.PutAsync("/api/countries", country);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message);
                return;
            }

            NavigationManager.NavigateTo("/countries");

            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });

            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro modificado con éxito.");
        }

        private void Return()
        {
            NavigationManager.NavigateTo("/countries");
        }
    }
}
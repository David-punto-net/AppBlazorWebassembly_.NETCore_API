using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Countries
{
    public partial class CountryCreate
    {
        private Country country = new();

        private FormWithName<Country>? countryForm;

        private bool regreso = false;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        private async Task CreateAsync()
        {
            if (!regreso) 
            {
                var responseHttp = await Repository.PostAsync("/api/countries", country);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
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

                await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
            }
        }

        private async Task Return()
        {
            regreso = true;

            if (country.Name == null)
            {
                NavigationManager.NavigateTo("/countries");
                return;
            }

            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = "¿Deseas abandonar la página y perder los cambios?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            NavigationManager.NavigateTo("/countries");

        }


    }
}
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
                countryForm!.FormPressCreate = true;

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
            countryForm!.FormPressCreate = false;
            NavigationManager.NavigateTo("/countries");
        }
    }
}
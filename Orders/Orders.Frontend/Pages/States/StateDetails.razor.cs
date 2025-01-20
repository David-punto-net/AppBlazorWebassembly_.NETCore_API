using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.Frontend.Pages.States
{
    public partial class StateDetails
    {
        private State? state;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Parameter] public int StateId { get; set; }
        public IQueryable<City>? Cities { get; set; }
        public IQueryable<City>? CitiessMaster { get; set; }

        private PaginationState PaginationGrid = new PaginationState { ItemsPerPage = 10 };

        private string nameFilter = "";

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var responseHttp = await Repository.GetAsync<State>($"api/states/{StateId}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo($"/countries/details/{state!.CountryId}");
                }

                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            state = responseHttp.Response;
            Cities = state!.Cities!.AsQueryable();
            CitiessMaster = Cities;
        }

        private async Task DeleteAsync(City city)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"¿Realmente deseas eliminar la Ciudad? {city.Name}",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true,
                CancelButtonText = "No",
                ConfirmButtonText = "Si"
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }
            var responseHttp = await Repository.DeleteAsync<State>($"/api/cities/{city.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode != HttpStatusCode.NotFound)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }
            }

            await LoadAsync();

            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });

            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro eliminado con éxito.");

        }

        private async Task Filtrar()
        {
            if (nameFilter != "")
            {
                Cities = CitiessMaster!.Where(c => c.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                Cities = CitiessMaster;
            }

            //await grid!.RefreshDataAsync();
        }

        private async Task Refrescar()
        {
            nameFilter = "";

            Cities = CitiessMaster!.Where(c => c.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase));

        }
    }
}

using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Orders.Frontend.Pages.Categories;
using Orders.Frontend.Pages.Cities;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.Frontend.Pages.States
{
    [Authorize(Roles = "Admin")]
    public partial class StateDetails
    {
        private State? state;

        private int totalRegistros;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Parameter] public int StateId { get; set; }

        private PaginationState PaginationGrid = new PaginationState { ItemsPerPage = 10 };

        private List<int> pageSizeOptions = new List<int> { 5, 10, 20, 50 };
        [CascadingParameter] IModalService Modal { get; set; } = default!;

        private GridItemsProvider<City>? CitiesProvider;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
        public IQueryable<City>? Cities { get; set; }

        private QuickGrid<City>? myGrid;

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task ShowModalAsync(int id = 0, bool isEdit = false)
        {
            IModalReference modalReference;

            if (isEdit)
            {
                modalReference = Modal.Show<CityEdit>(string.Empty, new ModalParameters().Add("CityId", id));
            }
            else
            {
                modalReference = Modal.Show<CityCreate>(string.Empty, new ModalParameters().Add("StateId", StateId));
            }
            var result = await modalReference.Result;
            if (result.Confirmed)
            {
                await LoadAsync();
            }
        }
        private void OnItemsPerPageChanged(int itemsPerPage)
        {
            PaginationGrid.ItemsPerPage = itemsPerPage;
        }
        private async Task FilterCallback(string filter)
        {
            Filter = filter;
            await Filtrar();
            StateHasChanged();
        }

        private async Task LoadAsync()
        {
            var responseHttp = await Repository.GetAsync<State>($"api/states/{StateId}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo($"/countries");
                }

                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            state = responseHttp.Response;

            await LoadCitiesAsync();
        }

        private async Task LoadCitiesAsync()
        {
            var url = $"api/cities/totalRecord?id={StateId}";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<int>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            totalRegistros = responseHttp.Response;

            CitiesProvider = async req =>
            {
                var url = $"api/cities/pagination?id={StateId}&page={req.StartIndex}&recordsnumber={req.Count}";
                if (!string.IsNullOrEmpty(Filter))
                {
                    url += $"&filter={Filter}";
                }

                var responseHttp = await Repository.GetAsync<List<City>>(url);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                }

                return GridItemsProviderResult.From(items: responseHttp!.Response!, totalItemCount: totalRegistros);
            };
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
            if (!string.IsNullOrEmpty(Filter))
            {
                await LoadCitiesAsync();
                await myGrid!.RefreshDataAsync();
            }
            else
            {
                Filter = string.Empty;

                await LoadCitiesAsync();
            }
        }
    }
}
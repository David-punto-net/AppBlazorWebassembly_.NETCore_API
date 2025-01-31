using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Orders.Frontend.Pages.Categories;
using Orders.Frontend.Pages.States;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;
using System.Net;

namespace Orders.Frontend.Pages.Countries
{
    [Authorize(Roles = "Admin")]
    public partial class CountryDetails
    {
        private Country? country;

        private int totalRegistros;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Parameter] public int CountryId { get; set; }

        private PaginationState PaginationGrid = new PaginationState { ItemsPerPage = 10 };

        private List<int> pageSizeOptions = new List<int> { 5, 10, 20, 50 };

        [CascadingParameter] private IModalService Modal { get; set; } = default!;

        private GridItemsProvider<State>? StatesProvider;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
        public IQueryable<State>? States { get; set; }

        private QuickGrid<State>? myGrid;

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task ShowModalAsync(int id = 0, bool isEdit =false)
        {
            IModalReference modalReference;
            if (isEdit)
            {
                modalReference = Modal.Show<StateEdit>(string.Empty, new ModalParameters().Add("StateId", id));
            }
            else
            {
                modalReference = Modal.Show<StateCreate>(string.Empty, new ModalParameters().Add("CountryId", CountryId));
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
            var responseHttp = await Repository.GetAsync<Country>($"api/countries/{CountryId}");
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

            country = responseHttp.Response;

            await LoadSateteAsync();
        }

        private async Task LoadSateteAsync()
        {
            var url = $"api/states/totalRecord?id={CountryId}";
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

            StatesProvider = async req =>
            {
                var url = $"api/states/pagination?id={CountryId}&page={req.StartIndex}&recordsnumber={req.Count}";
                if (!string.IsNullOrEmpty(Filter))
                {
                    url += $"&filter={Filter}";
                }

                var responseHttp = await Repository.GetAsync<List<State>>(url);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                }

                return GridItemsProviderResult.From(items: responseHttp!.Response!, totalItemCount: totalRegistros);
            };
        }

        private async Task DeleteAsync(State state)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"¿Realmente deseas eliminar el departamento/estado? {state.Name}",
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
            var responseHttp = await Repository.DeleteAsync<State>($"/api/states/{state.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode != HttpStatusCode.NotFound)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }
            }

            await LoadSateteAsync();

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
                await LoadSateteAsync();
                await myGrid!.RefreshDataAsync();
            }
            else
            {
                Filter = string.Empty;

                await LoadSateteAsync();
            }
        }
    }
}
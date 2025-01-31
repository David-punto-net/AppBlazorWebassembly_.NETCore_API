using Blazored.Modal.Services;
using Blazored.Modal;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Pages.Categories;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Products
{
    public partial class ProductsIndex
    {

        private int totalRegistros;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        private PaginationState PaginationGrid = new PaginationState { ItemsPerPage = 10 };

        private List<int> pageSizeOptions = new List<int> { 5, 10, 20, 50 };
        [CascadingParameter] IModalService Modal { get; set; } = default!;

        private GridItemsProvider<Product>? ProductsProvider;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
        public IQueryable<Product>? Products { get; set; }

        private QuickGrid<Product>? myGrid;

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task ShowModalAsync(int id = 0, bool isEdit = false)
        {
            IModalReference modalReference;

            if (isEdit)
            {
                //modalReference = Modal.Show<ProductEdit>(string.Empty, new ModalParameters().Add("Id", id));
                modalReference = Modal.Show<ProductCreate>(string.Empty, new ModalParameters().Add("Id", id));
            }
            else
            {
                modalReference = Modal.Show<ProductCreate>();
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
            var url = "api/products/totalRecord";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"?filter={Filter}";
            }

            var responseHttp = await Repository.GetAsync<int>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            totalRegistros = responseHttp.Response;

            ProductsProvider = async req =>
            {
                var url = $"api/products?page={req.StartIndex}&recordsnumber={req.Count}";
                if (!string.IsNullOrEmpty(Filter))
                {
                    url += $"&filter={Filter}";
                }

                var responseHttp = await Repository.GetAsync<List<Product>>(url);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                }

                return GridItemsProviderResult.From(items: responseHttp!.Response!, totalItemCount: totalRegistros);
            };
        }

        private async Task DeleteAsync(Product category)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"¿Estas seguro de borrar el producto: {category.Name}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var responseHttp = await Repository.DeleteAsync<Product>($"/api/products/{category.Id}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/products");
                }
                else
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                }

                return;
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
                await LoadAsync();
                await myGrid!.RefreshDataAsync();
            }
            else
            {
                Filter = string.Empty;

                await LoadAsync();
            }
        }
    }
}
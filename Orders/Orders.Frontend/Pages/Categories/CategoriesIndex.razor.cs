using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Categories
{
    public partial class CategoriesIndex
    {
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        public IQueryable<Category>? Categories { get; set; }
        public IQueryable<Category>? CategoriesMaster { get; set; }

        private PaginationState PaginationGrid = new PaginationState { ItemsPerPage = 10 };


        private string nameFilter = "";

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task LoadAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Category>>("api/categories/full");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Categories = responseHttp.Response!.AsQueryable();
            CategoriesMaster = Categories;
        }

        private async Task DeleteAsync(Category category)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"¿Estas seguro de borrar la categoía {category.Name}?",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true
            });
            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var responseHttp = await Repository.DeleteAsync<Category>($"/api/categories/{category.Id}");
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
            if (nameFilter != "")
            {
                Categories = CategoriesMaster!.Where(c => c.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                Categories = CategoriesMaster;
            }

        }

        private async Task Refrescar()
        {
            nameFilter = "";
     
           Categories = CategoriesMaster!.Where(c => c.Name.Contains(nameFilter, StringComparison.CurrentCultureIgnoreCase));
           
        }

    }
}
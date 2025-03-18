using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Orders.Frontend.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Products;

public partial class ProductSearch
{
    private int currentPage = 0;
    private int totalItems = 0;
    private List<CardDTO> cards = new();
    private List<int> pageSizeOptions = new List<int> { 2, 12, 24, 32 };

    public List<Product>? Products { get; set; }
    [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 12;

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }
    protected override async Task OnParametersSetAsync()
    {
        await LoadAsync();
    }

   private async Task OnPageSizeChanged(int itemsPerPage)
    {
        RecordsNumber = itemsPerPage;

        await LoadAsync();
    }
    private async Task SelectedRecordsNumberAsync(int recordsnumber)
    {
        RecordsNumber = recordsnumber;
        int page = 1;
        await LoadAsync(page);

        await SelectedPageAsync(page);
    }
    private async Task SelectedPageAsync(int page)
    {
        currentPage = page;
        await LoadAsync(page);
    }
    private async Task LoadAsync(int page = 1)
    {
        bool isListLoaded = await LoadListAsync(page);
        if (isListLoaded)
        {
            await LoadTotalItemsAsync();
        }
        StateHasChanged();
    }
    private void ValidateRecordsNumber(int recordsnumber)
    {
        if (recordsnumber == 0)
        {
            RecordsNumber = 12;
        }
    }
    private async Task LoadTotalItemsAsync()
    {
        ValidateRecordsNumber(RecordsNumber);

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

        totalItems = responseHttp.Response;

    }
    private async Task<bool> LoadListAsync(int page)
    {
        ValidateRecordsNumber(RecordsNumber);

        var url = $"api/products/pagination?page={page}&RecordsNumber={RecordsNumber}";
        if (!string.IsNullOrEmpty(Filter))
        {
            url += $"&filter={Filter}";
        }
        var response = await Repository.GetAsync<List<Product>>(url);
        if (response.Error)
        {
            var message = await response.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return false;
        }

        Products = response.Response;

        CardsProduct();

        return true;
    }
    private void CardsProduct()
    {
        cards.Clear();

        foreach (var product in Products!)
        {
            var card = new CardDTO
            {
                Id = product.Id,
                Title = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.MainImage
            };

            cards.Add(card);
        }
    }



    private void AddToCartAsync(int productId)
    {
    }
}

/*

  private int currentPage = 0;
    private int totalPages;

    private int totalRegistros;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private List<CardDTO> cards = new();
    private List<int> pageSizeOptions = new List<int> { 5, 10, 20, 50, 100 };
    [Parameter] public int ItemsPerPage { get; set; } = 5;
    [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
    [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 8;
    [Parameter] public string Filter { get; set; } = string.Empty;
    public List<Product>? Products { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task SelectedPageAsync(int page)
    {
        currentPage = page;
        await LoadAsync();
    }
    private async Task SelectedRecordsNumberAsync(int recordsnumber)
    {
        ItemsPerPage = recordsnumber;
        int page = 1;
        await LoadAsync();
        await SelectedPageAsync(page);
    }

    protected override async Task OnParametersSetAsync()
    {
        await LoadAsync();
    }
    private async Task OnPageSizeChanged(int itemsPerPage)
    {
        ItemsPerPage = itemsPerPage;

        await LoadAsync();
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

        var urlProduct = $"api/products?page={currentPage}&recordsnumber={ItemsPerPage}";
        if (!string.IsNullOrEmpty(Filter))
        {
            urlProduct += $"&filter={Filter}";
        }

        var responseHttpProduct = await Repository.GetAsync<List<Product>>(urlProduct);
        if (responseHttpProduct.Error)
        {
            var message = await responseHttpProduct.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
        }

        Products = responseHttpProduct.Response;

        CardsProduct();

        StateHasChanged();
    }

    private void CardsProduct()
    {
        cards.Clear();

        foreach (var product in Products!)
        {
            var card = new CardDTO
            {
                Id = product.Id,
                Title = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.MainImage
            };

            cards.Add(card);
        }
    }

    private void AddToCartAsync(int productId)
    {
    }

*/
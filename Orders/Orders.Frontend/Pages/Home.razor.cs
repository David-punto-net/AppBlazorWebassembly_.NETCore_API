using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages;

public partial class Home
{
    private int totalRegistros;
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    public List<Product>? Products { get; set; }
    public List<Category>? Categories { get; set; }

    private List<CardDTO> cards = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task<bool> LoadAsync()
    {
        //var url = $"api/products?page=0&recordsnumber=10";
        var url = $"api/products?page=0";
        var response = await Repository.GetAsync<List<Product>>(url);
        if (response.Error)
        {
            var message = await response.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return false;
        }

        Products = response.Response;

        Categories = Products!.SelectMany(x => x.ProductCategories!.Select(a => new Category
        {
            Id = a.Category!.Id,
            Name = a.Category!.Name
        }))
        .DistinctBy(c => c.Id)
        .ToList();

        foreach (var product in Products!)
        {
            cards.Add(new CardDTO
            {
                Title = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.MainImage,
                Id = product.Id.ToString(),
                Discount = 38
            });
        }

        return true;
    }

    private void AddToCartAsync(int productId)
    {
    }
}
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

    private List<CardDTO> cards = new();

    private List<CategoriaDTO> categoriaDTO = new();
    public List<Product>? Products { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task<bool> LoadAsync()
    {
        var url = $"api/products?page=0";
        var response = await Repository.GetAsync<List<Product>>(url);
        if (response.Error)
        {
            var message = await response.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return false;
        }

        Products = response.Response;

        CardsCarrousel();

        return true;
    }

    private void CardsCarrousel()
    {
        var categoriasUnicas = new Dictionary<int, CategoriaDTO>();

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

            foreach (var category in product.ProductCategories!)
            {
                if (!categoriasUnicas.ContainsKey(category.Category!.Id))
                {
                    categoriasUnicas.Add(category.Category!.Id, new CategoriaDTO
                    {
                        Id = category.Category!.Id,
                        Name = category.Category!.Name,
                        Productos = new List<CardDTO>()
                    });
                }

                categoriasUnicas[category.Category!.Id].Productos.Add(card);
            }
        }

        categoriaDTO.AddRange(categoriasUnicas.Values);
    }
    private void AddToCartAsync(int productId)
    {
    }
}
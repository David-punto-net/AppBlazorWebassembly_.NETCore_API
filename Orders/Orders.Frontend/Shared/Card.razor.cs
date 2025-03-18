using Microsoft.AspNetCore.Components;

namespace Orders.Frontend.Shared;

public partial class Card 
{
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Description { get; set; }
    [Parameter] public decimal Price { get; set; }
    [Parameter] public string? ImageUrl { get; set; }
    [Parameter] public string? Id { get; set; }
}
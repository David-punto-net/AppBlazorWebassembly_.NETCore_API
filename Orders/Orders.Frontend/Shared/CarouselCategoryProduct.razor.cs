using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Orders.Shared.DTOs;


namespace Orders.Frontend.Shared;

public partial class CarouselCategoryProduct
{
    private int currentIndex = 0;
    private int CardsPerPage = 5;

    [Parameter] public List<CardDTO> Cards { get; set; } = new();


    private List<CardDTO> DisplayedCards => Cards
      .Skip(currentIndex)
      .Take(CardsPerPage)
      .ToList();

    private void ShowNext()
    {


        if (currentIndex + CardsPerPage < Cards.Count)
        {
            currentIndex++;
        }
    }

    private void ShowPrevious()
    {

        if (currentIndex > 0)
        {
            currentIndex--;
        }
    }


}
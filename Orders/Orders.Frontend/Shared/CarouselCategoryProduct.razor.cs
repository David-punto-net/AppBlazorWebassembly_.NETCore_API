using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Orders.Shared.DTOs;

namespace Orders.Frontend.Shared;

public partial class CarouselCategoryProduct
{
    private string carousel = Guid.NewGuid().ToString("N");
    private bool IsMouseDown;
    private double PrevPageX;
    private double CurrentScrollPosition;
    private double PromMaxScrollPosition;
    [Inject] private IJSRuntime JS { get; set; } = null!;
    private IJSObjectReference? _module;
    [Parameter] public List<CardDTO> Cards { get; set; } = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import", "./js/scrollHelper.js");
        }
    }

    private async Task OnMouseDown(MouseEventArgs e)
    {

        IsMouseDown = true;
        PrevPageX = e.PageX;
        CurrentScrollPosition = await _module!.InvokeAsync<double>("GetScrollLeft", carousel);
    }

    private async Task OnTouchStart(TouchEventArgs e)
    {
        IsMouseDown = true;
        PrevPageX = e.Touches[0].PageX;
        CurrentScrollPosition = await _module!.InvokeAsync<double>("GetScrollLeft", carousel);
    }

    private async Task OnMouseMove(MouseEventArgs e)
    {
        if (IsMouseDown)
        {
            var MouseMovementRange = e.PageX - PrevPageX;
            await _module!.InvokeVoidAsync("SetScrollLeft", carousel, CurrentScrollPosition - MouseMovementRange);
        }
    }

    private async Task OnTouchMove(TouchEventArgs e)
    {
        if (IsMouseDown)
        {
            var MouseMovementRange = e.Touches[0].PageX - PrevPageX;
            await _module!.InvokeVoidAsync("SetScrollLeft", carousel, CurrentScrollPosition - MouseMovementRange);
        }
    }

    private async Task OnMouseUp()
    {
        await OnMouseLeaveUp();
    }

    private async Task OnMouseLeave()
    {
        
        await OnMouseLeaveUp();
    }

    private async Task OnMouseLeaveUp()
    {
        IsMouseDown = false;
        await HideOrShowArrowIcons();
    }
    private async Task ShowNext()
    {
        CurrentScrollPosition = await _module!.InvokeAsync<double>("GetScrollLeft", carousel);
        await _module!.InvokeVoidAsync("SetScrollLeft", carousel, CurrentScrollPosition + 198);
        await HideOrShowArrowIcons();
    }

    private async Task ShowPrevious()
    {
        CurrentScrollPosition = await _module!.InvokeAsync<double>("GetScrollLeft", carousel);
        await _module!.InvokeVoidAsync("SetScrollLeft", carousel, CurrentScrollPosition - 198);
        await HideOrShowArrowIcons();
    }

    private async Task HideOrShowArrowIcons()
    {
        CurrentScrollPosition = await _module!.InvokeAsync<double>("GetScrollLeft", carousel);

        int CarouselClientWidth = await _module!.InvokeAsync<int>("ElementClientWidth", carousel);
        int CarouselScrollWidth = await _module!.InvokeAsync<int>("ElementScrollWidth", carousel);
        int MaxScrollPosition = (CarouselClientWidth - CarouselScrollWidth) * -1;

        PromMaxScrollPosition = Math.Abs(CurrentScrollPosition - MaxScrollPosition);
    }

    private async Task OnMouseEnter()
    {
        await HideOrShowArrowIcons();
    }

    public async ValueTask DisposeAsync()
    {
        if (_module != null)
        {
            await _module.DisposeAsync();
        }
    }
}
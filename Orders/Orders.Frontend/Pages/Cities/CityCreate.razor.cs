﻿using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.Cities
{
    [Authorize(Roles = "Admin")]
    public partial class CityCreate
    {
        private City city = new();

        private FormWithName<City>? cityForm;
        [Parameter] public int StateId { get; set; }
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;


        private async Task CreateAsync()
        {
            if (!cityForm!.FormPressReturn)
            {
                cityForm!.FormPressCreate = true;

                city.StateId = StateId;
                var responseHttp = await Repository.PostAsync("/api/cities", city);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    return;
                }

                await BlazoredModal.CloseAsync(ModalResult.Ok());
                NavigationManager.NavigateTo($"/states/details/{StateId}");

                var toast = SweetAlertService.Mixin(new SweetAlertOptions
                {
                    Toast = true,
                    Position = SweetAlertPosition.BottomEnd,
                    ShowConfirmButton = true,
                    Timer = 3000
                });

                await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro creado con éxito.");
            }
        }

        private async Task Return()
        {
            cityForm!.FormPressReturn = true;
            cityForm!.FormPressCreate = false;
            NavigationManager.NavigateTo($"/states/details/{StateId}");
        }
    }
}

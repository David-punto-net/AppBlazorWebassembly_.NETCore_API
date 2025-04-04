﻿using Blazored.Modal;
using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Shared;
using Orders.Shared.Entities;

namespace Orders.Frontend.Pages.States
{
    [Authorize(Roles = "Admin")]
    public partial class StateEdit
    {
        private State? state;

        private FormWithName<State>? stateForm;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [EditorRequired, Parameter] public int StateId { get; set; }
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; } = default!;
        protected override async Task OnParametersSetAsync()
        {
            if (StateId != 0)
            {
                var responseHttp = await Repository.GetAsync<State>($"/api/states/{StateId}");
                if (responseHttp.Error)
                {
                    if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        NavigationManager.NavigateTo($"/countries/details/{state!.CountryId}");
                    }
                    else
                    {
                        var message = await responseHttp.GetErrorMessageAsync();
                        await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                    }
                }
                else
                {
                    state = responseHttp.Response;
                }
            }
        }

        private async Task UpdateAsync()
        {
            if (!stateForm!.FormPressReturn)
            {
                stateForm!.FormPressCreate = true;

                var responseHttp = await Repository.PutAsync("/api/states", state);
                if (responseHttp.Error)
                {
                    var message = await responseHttp.GetErrorMessageAsync();
                    await SweetAlertService.FireAsync("Error", message);
                    return;
                }

                await BlazoredModal.CloseAsync(ModalResult.Ok());
                NavigationManager.NavigateTo($"/countries/details/{state!.CountryId}");

                var toast = SweetAlertService.Mixin(new SweetAlertOptions
                {
                    Toast = true,
                    Position = SweetAlertPosition.BottomEnd,
                    ShowConfirmButton = true,
                    Timer = 3000
                });

                await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Registro modificado con éxito.");
            }
        }

        private void Return()
        {
            stateForm!.FormPressReturn = true;
            stateForm!.FormPressCreate = false;
            NavigationManager.NavigateTo($"/countries/details/{state!.CountryId}");
        }
    }
}

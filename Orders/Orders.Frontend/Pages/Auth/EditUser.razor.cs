using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.Frontend.Repositories;
using Orders.Frontend.Services;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Enums;
using System.Net;

namespace Orders.Frontend.Pages.Auth
{
    [Authorize]
    public partial class EditUser
    {
        private User? user;
        private List<Country>? countries;
        private List<State>? states;
        private List<City>? cities;
        private string? imageUrl;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadUserAsync();
            await LoadCountriesAsync();
            await LoadStatesAsyn(user!.City!.State!.Country!.Id);
            await LoadCitiesAsyn(user!.City!.State!.Id);

            if(!string.IsNullOrEmpty(user!.Photo))
            {
                imageUrl = user.Photo;
                user.Photo = null;
            }
        }

        private void ImageSelected(string imagenBase64)
        {
            user!.Photo = imagenBase64;
            imageUrl = null;
        }

        private async Task LoadUserAsync()
        {
            var responseHttp = await Repository.GetAsync<User>("/api/accounts");
            if (responseHttp.Error)
            {
                if(responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/");
                    return;
                }
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            user = responseHttp.Response;
        }

        private async Task LoadCountriesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Country>>("/api/countries/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            countries = responseHttp.Response;
        }

        private async Task CountryChangedAsync(ChangeEventArgs e)
        {
            var countryId = Convert.ToInt32(e.Value!);
            states = null;
            cities = null;
            user!.CityId = 0;
            await LoadStatesAsyn(countryId);
        }

        private async Task StateChangedAsync(ChangeEventArgs e)
        {
            var stateId = Convert.ToInt32(e.Value!);
            cities = null;
            user!.CityId = 0;
            await LoadCitiesAsyn(stateId);
        }

        private async Task LoadStatesAsyn(int countryId)
        {
            var responseHttp = await Repository.GetAsync<List<State>>($"/api/states/combo/{countryId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            states = responseHttp.Response;
        }

        private async Task LoadCitiesAsyn(int stateId)
        {
            var responseHttp = await Repository.GetAsync<List<City>>($"/api/cities/combo/{stateId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            cities = responseHttp.Response;
        }

        private async Task SaveUserAsync()
        {
            var responseHttp = await Repository.PutAsync<User>("/api/accounts", user!);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            NavigationManager.NavigateTo("/");

            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.Top,
                ShowConfirmButton = true,
                Timer = 3000
            });

            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Usuario modificado con éxito.");

        }

    }
}
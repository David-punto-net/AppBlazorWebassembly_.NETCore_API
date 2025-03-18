using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Orders.Frontend.Shared
{
    public partial class AuthLinks
    {
        private string? photoUser;

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;

        protected override async Task OnParametersSetAsync()
        {
            var autenticationState = await AuthenticationStateTask;
            var claims = autenticationState.User.Claims.ToList();
            var photoClaim = claims.FirstOrDefault(x => x.Type == "Photo");
            if (photoClaim is not null)
            {
                photoUser = photoClaim.Value;
            }
        }

        private async Task FilterCallback(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                NavigationManager.NavigateTo($"/products/search/{filter}");
            }
            else
            {
                NavigationManager.NavigateTo("/products/search");
            }
        }
    }
}
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Orders.Shared.Interfaces;

namespace Orders.Frontend.Shared
{
    public partial class FormWithName<TModel> where TModel : IEntityWithName
    {
        private EditContext editContext = null!;

        [EditorRequired, Parameter] public TModel Model { get; set; } = default!;

        [EditorRequired, Parameter] public string Label { get; set; } = null!;

        [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }

        [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        public bool FormPressCreate { get; set; } = false;
        public bool FormPressReturn { get; set; } = false;

        protected override void OnInitialized()
        {
            editContext = new(Model);
        }

        private async Task OnBeforeInternalNavigation(LocationChangingContext context)
        {
            if (!FormPressCreate)
            {
                if (Model.Name != null)
                {
                    var result = await SweetAlertService.FireAsync(new SweetAlertOptions
                    {
                        Title = "Confirmación",
                        Text = "¿Deseas abandonar la página sin guardar los cambios?",
                        Icon = SweetAlertIcon.Warning,
                        ShowCancelButton = true,
                        ConfirmButtonText = "Sí, salir",
                        CancelButtonText = "No, permanecer aquí",
                    });
                    var confirm = !string.IsNullOrEmpty(result.Value);
                    if (!confirm)
                    {
                        FormPressReturn=false;
                        context.PreventNavigation();
                    }
                }
            }
        }
    }
}
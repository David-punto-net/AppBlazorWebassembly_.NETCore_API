using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace Orders.Frontend.Shared
{
    public partial class GenericList<Titem>
    {
        [Parameter] public RenderFragment? Loading { get; set; }

        [Parameter] public RenderFragment? NoRecords { get; set; }

        [EditorRequired]
        [Parameter] public RenderFragment Body { get; set; } = null!;

        [EditorRequired, Parameter] public IQueryable<Titem> MyList { get; set; } = null!;

    }
}
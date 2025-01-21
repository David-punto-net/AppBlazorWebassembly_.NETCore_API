using Microsoft.AspNetCore.Components;

namespace Orders.Frontend.Shared
{
    public partial class Pagination
    {
        private List<PageModel> Links = null!;

        [Parameter] public int CurrentPage { get; set; }
        [Parameter] public int TotalPage { get; set; }
        [Parameter] public int Radio { get; set; } = 10;
        [Parameter] public EventCallback<int> SelectPage { get; set; }

        protected override void OnParametersSet()
        {
            Links = new List<PageModel>();

            Links.Add(new PageModel
            {
                Text = "Anterior",
                Page = CurrentPage - 1,
                Enable = CurrentPage != 1,
            });

            for (int i = 1; i <= TotalPage; i++)
            {

                if(TotalPage <= Radio)
                {
                    Links.Add(new PageModel
                    {
                        Text = i.ToString(),
                        Page = i,
                        Enable = i == CurrentPage,
                    });
                }

                if (TotalPage > Radio && i <= Radio && CurrentPage <= Radio)
                {
                    Links.Add(new PageModel
                    {
                        Text = i.ToString(),
                        Page = i,
                        Enable = CurrentPage == i,
                    });
                }

                if (CurrentPage > Radio && i > CurrentPage - Radio && i <= CurrentPage)
                {
                    Links.Add(new PageModel
                    {
                        Text = i.ToString(),
                        Page = i,
                        Enable = CurrentPage == i,
                    });
                }
            }

            Links.Add(new PageModel
            {
                Text = "Siguiente",
                Page = CurrentPage != TotalPage ? CurrentPage + 1 : CurrentPage,
                Enable = CurrentPage != TotalPage,
            });
        }


        private async Task InternalSelectedPage(PageModel pageModel)
        {
            if (pageModel.Page == CurrentPage || pageModel.Page == 0 ) 
            {
                return;
            }

            await SelectPage.InvokeAsync(pageModel.Page);
        }

        private class PageModel
        {
            public string Text { get; set; } = null!;
            public int Page { get; set; }
            public bool Enable { get; set; }
            public bool Active { get; set; }
        }
    }
}
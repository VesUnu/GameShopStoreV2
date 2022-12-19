using GameShopStoreV2.Core.Common;
using Microsoft.AspNetCore.Mvc;

namespace GameShopStoreV2.Admin.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(BasePagedResult result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}

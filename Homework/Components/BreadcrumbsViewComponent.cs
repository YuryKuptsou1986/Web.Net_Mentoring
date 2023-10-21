using Homework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Homework.ViewComponents
{
    public class BreadcrumbsViewComponent : ViewComponent
    {
        private string Title;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public BreadcrumbsViewComponent(IStringLocalizer<SharedResource> localizer) {
            _localizer = localizer;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["Breadcrumb"] = ConfigureBreadcrumb();
            return View("Index");
        }

        private IEnumerable<Breadcrumb> ConfigureBreadcrumb()
        {
            var breadcrumbs = new List<Breadcrumb>();

            var controllerName = Request.RouteValues["controller"].ToString();
            var action = Request.RouteValues["action"].ToString();

            if (controllerName == "Home") {
                // Don't need to show anything on Home page.
                return breadcrumbs;
            }

            breadcrumbs.Add(new Breadcrumb {
                Action = "Index",
                Active = true,
                Controller = "Home",
                Text = "Home"
            });

            // For controller
            var breadcrumbController = new Breadcrumb {
                Action = "Index",
                Active = action != "Index",
                Controller = controllerName,
                Text = controllerName
            };

            breadcrumbs.Add(breadcrumbController);

            if (action == "Index") {
                // Don't show action with name 'Index'
                return breadcrumbs;
            }

            // For action
            var breadcrumbAction = new Breadcrumb {
                Action = action,
                Active = false,
                Controller = controllerName,
                Text = _localizer[action]
            };

            breadcrumbs.Add(breadcrumbAction);

            return breadcrumbs;
        }
    }
}

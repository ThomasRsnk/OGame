using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Total.AutoCare.Web.Helpers.Razor
{
    public class RazorViewToStringRenderer
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;

        public RazorViewToStringRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderViewToStringAsync<TModel>(string name, TModel model)
        {
            var actionContext = GetActionContext();

            var viewEngineResult = _viewEngine.FindView(actionContext, name, false);

            if (!viewEngineResult.Success)
                throw new InvalidOperationException($"Couldn't find view '{name}'");

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }
        public async Task<string> RenderViewToStringAsync(string name)
        {
            var actionContext = GetActionContext();

            var viewEngineResult = _viewEngine.FindView(actionContext, name, false);

            if (!viewEngineResult.Success)
                throw new InvalidOperationException($"Couldn't find view '{name}'");

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary()),
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider
            };
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}

using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Djm.OGame.Web.Api.Services
{
    public class PageModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.BinderModelName;
            if (string.IsNullOrEmpty(modelName))
            {
                modelName = "page";
            }

            var currentValue = bindingContext.ValueProvider.GetValue(modelName);
            var sizeValue = bindingContext.ValueProvider.GetValue(modelName + "Length");

            if (!int.TryParse(currentValue.FirstValue, out var current))
                current = 1;

            if (!int.TryParse(sizeValue.FirstValue, out var size))
                size = 50;

            var page = new Page
            {
                Current = current,
                Size = size
            };

            bindingContext.Result = ModelBindingResult.Success(page);

            return Task.CompletedTask;
        }
    }
}
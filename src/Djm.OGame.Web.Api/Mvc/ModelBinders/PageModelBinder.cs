using System.Threading.Tasks;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Djm.OGame.Web.Api.Mvc.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace Djm.OGame.Web.Api.Mvc.ModelBinders
{
    public class PageModelBinder : IModelBinder
    {
        public PaginationOptions Opt { get; }

        public PageModelBinder(IOptionsSnapshot<PaginationOptions> opt)
        {
            Opt = opt.Value;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelName = bindingContext.BinderModelName;
            if (string.IsNullOrEmpty(modelName))
            {
                modelName = bindingContext.FieldName;
            }

            var currentValue = bindingContext.ValueProvider.GetValue(modelName);
            var sizeValue = bindingContext.ValueProvider.GetValue(modelName + "Length");

            if (!int.TryParse(currentValue.FirstValue, out var current))
                if (!int.TryParse(Opt.DefaultPageIndex, out current))
                    current = 1;

            if (!int.TryParse(sizeValue.FirstValue, out var size))
                if (!int.TryParse(Opt.DefaultPageSize, out size))
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
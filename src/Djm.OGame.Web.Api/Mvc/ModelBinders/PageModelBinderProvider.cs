using System;
using Djm.OGame.Web.Api.BindingModels.Pagination;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;


namespace Djm.OGame.Web.Api.Mvc.ModelBinders
{
    public class PageModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.Metadata.ModelType == typeof(Page) ? new BinderTypeModelBinder(typeof(PageModelBinder)) : null;
        }
    }
}
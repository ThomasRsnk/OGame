using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OGame.Client;

namespace Djm.OGame.Web.Api.Services
{
    public class PlayerModelBinder : IModelBinder
    {
        public IOgClient OgClient { get; }

        public PlayerModelBinder(IOgClient ogClient)
        {
            OgClient = ogClient;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var currentValue = bindingContext.ValueProvider.GetValue("ownerId");
            var sizeValue = bindingContext.ValueProvider.GetValue("targetId");

            if (!int.TryParse(currentValue.FirstValue, out var current))
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    current.ToString());

//            if (!int.TryParse(sizeValue.FirstValue, out var size))
//                bindingContext.ModelState.TryAddModelError(
//                    bindingContext.ModelName,
//                    sizeValue);

           

            //bindingContext.Result = ModelBindingResult.Success(page);

            return Task.CompletedTask;
        }
    }
}
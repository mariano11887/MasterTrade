using System;
using System.Globalization;
using System.Web.Mvc;

namespace MasterTrade.Common
{
    public class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == null) return base.BindModel(controllerContext, bindingContext);
            try
            {
                string value = valueProviderResult.AttemptedValue.Replace(',', '.');
                return Convert.ToDecimal(value, CultureInfo.GetCultureInfo("en-US"));
            }
            catch (FormatException)
            {
                try
                {
                    // If format error then fallback to InvariantCulture instead of current UI culture
                    return Convert.ToDecimal(valueProviderResult.AttemptedValue, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    return default;
                }
            }
        }
    }
}
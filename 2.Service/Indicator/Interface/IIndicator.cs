using System.Collections.Generic;

namespace _2.Service.Indicator.Interface
{
    public interface IIndicator
    {
        string ConfigurationTitle { get; }

        List<IndicatorMeta> Meta { get; }
    }
}

using _3.Repository;
using _4.DTO;
using System.Collections.Generic;

namespace _2.Service.Indicator.Interface
{
    public interface IIndicator
    {
        string ConfigurationTitle { get; }
        List<IndicatorConfiguration> Configuration { get; }
        List<IndicatorMeta> Meta { get; }

        List<IndicatorMeta> GetCurrentValues(List<Candle> candles, int currentCandleIndex, List<DTOIndicatorConfiguration> configurations);
    }
}

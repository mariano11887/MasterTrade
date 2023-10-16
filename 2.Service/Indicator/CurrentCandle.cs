using _2.Service.Indicator.Interface;
using _4.DTO;
using _4.DTO.Enums;
using System.Collections.Generic;

namespace _2.Service.Indicator
{
    public class CurrentCandle : IIndicator
    {
        public string ConfigurationTitle
        {
            get
            {
                return "";
            }
        }

        public List<IndicatorMeta> Meta
        {
            get
            {
                return new List<IndicatorMeta>
                {
                    new IndicatorMeta
                    {
                        Name = "Apertura",
                        HtmlName = "Open",
                        Type = IndicatorMetaDataType.Double
                    },
                    new IndicatorMeta
                    {
                        Name = "Cierre",
                        HtmlName = "Close",
                        Type = IndicatorMetaDataType.Double
                    },
                    new IndicatorMeta
                    {
                        Name = "Máximo",
                        HtmlName = "High",
                        Type = IndicatorMetaDataType.Double
                    },
                    new IndicatorMeta
                    {
                        Name = "Mínimo",
                        HtmlName = "Low",
                        Type = IndicatorMetaDataType.Double
                    }
                };
            }
        }

        public List<IndicatorConfiguration> Configuration
        {
            get
            {
                return new List<IndicatorConfiguration>();
            }
        }

        public List<IndicatorMeta> GetCurrentValues(List<_3.Repository.Candle> candles, int currentCandleIndex, List<DTOIndicatorConfiguration> configurations)
        {
            return new List<IndicatorMeta>
            {
                new IndicatorMeta
                {
                    Name = "Apertura",
                    Value = candles[currentCandleIndex].Open.ToString(),
                    Type = IndicatorMetaDataType.Double
                },
                new IndicatorMeta
                {
                    Name = "Cierre",
                    Value = candles[currentCandleIndex].Close.ToString(),
                    Type = IndicatorMetaDataType.Double
                },
                new IndicatorMeta
                {
                    Name = "Máximo",
                    Value = candles[currentCandleIndex].High.ToString(),
                    Type = IndicatorMetaDataType.Double
                },
                new IndicatorMeta
                {
                    Name = "Mínimo",
                    Value = candles[currentCandleIndex].Low.ToString(),
                    Type = IndicatorMetaDataType.Double
                }
            };
        }
    }
}

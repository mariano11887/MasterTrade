using _2.Service.Indicator.Interface;
using _4.DTO;
using _4.DTO.Enums;
using System.Collections.Generic;
using System.Linq;

namespace _2.Service.Indicator
{
    public class MovingAverage : IIndicator
    {
        public string ConfigurationTitle { 
            get
            {
                return "Configuración de la media móvil";
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
                        Name = "Valor",
                        HtmlName = "Value",
                        Type = IndicatorMetaDataType.Double
                    }
                };
            }
        }

        public List<IndicatorConfiguration> Configuration
        {
            get
            {
                return new List<IndicatorConfiguration>
                {
                    new IndicatorConfiguration
                    {
                        Name = "Longitud",
                        HtmlName = "Length",
                        Type = IndicatorMetaDataType.Integer
                    }
                };
            }
        }

        public List<IndicatorMeta> GetCurrentValues(List<_3.Repository.Candle> candles, int currentCandleIndex, List<DTOIndicatorConfiguration> configurations)
        {
            int length = int.Parse(configurations.FirstOrDefault(c => c.Name == "Longitud").Value);
            if (currentCandleIndex + 1 < length)
            {
                return null;
            }

            decimal closeSum = 0;
            for (int i = currentCandleIndex + 1 - length; i <= currentCandleIndex; i++)
            {
                closeSum += candles[i].Close;
            }

            decimal currentValue = closeSum / length;

            return new List<IndicatorMeta>
            {
                new IndicatorMeta
                {
                    Name = "Valor",
                    Value = currentValue.ToString(),
                    Type = IndicatorMetaDataType.Double
                }
            };
        }
    }
}

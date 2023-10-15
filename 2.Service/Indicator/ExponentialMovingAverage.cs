using _2.Service.Indicator.Interface;
using _4.DTO;
using _4.DTO.Enums;
using System.Collections.Generic;

namespace _2.Service.Indicator
{
    public class ExponentialMovingAverage : IIndicator
    {
        public string ConfigurationTitle
        {
            get
            {
                return "Configuración de la media móvil exponencial";
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
            throw new System.NotImplementedException();
        }
    }
}

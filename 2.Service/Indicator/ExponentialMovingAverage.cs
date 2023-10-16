using _2.Service.Indicator.Interface;
using _4.DTO;
using _4.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

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
            int length = int.Parse(configurations.FirstOrDefault(c => c.Name == "Longitud").Value);
            if (currentCandleIndex + 1 < length)
            {
                return null;
            }

            decimal alpha = (decimal)2 / (length + 1);

            // Empiezo calculando las EMA desde una cantidad de perídos atrás igual a la longitud, para poder ir pasando las EMAs anteriores a la vela siguiente.
            int startIndex = currentCandleIndex + 1 - length;
            if (startIndex < length)
            {
                startIndex = length - 1;
            }

            decimal ema = 0;
            for (int i = startIndex; i <= currentCandleIndex; i++)
            {
                if (i == startIndex)
                {
                    ema = new MovingAverage().GetCurrentValues(candles, i, configurations).Select(im => decimal.Parse(im.Value)).First();
                }
                else
                {
                    ema = alpha * candles[i].Close + (1 - alpha) * ema;
                }
            }

            return new List<IndicatorMeta>
            {
                new IndicatorMeta
                {
                    Name = "Valor",
                    Value = ema.ToString(),
                    Type = IndicatorMetaDataType.Double
                }
            };
        }
    }
}

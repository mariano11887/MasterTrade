using _2.Service.Indicator.Interface;
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
                        Name = "Longitud",
                        HtmlName = "Length",
                        Type = IndicatorMetaDataType.Integer
                    }
                };
            }
        }
    }
}

using _2.Service.Indicator;
using _2.Service.Indicator.Interface;
using _4.DTO;
using _4.DTO.Enums;
using System.Collections.Generic;

namespace _2.Service.Service
{
    public class ServiceIndicator
    {
        public List<DTOIndicatorType> GetIndicatorTypes()
        {
            return new List<DTOIndicatorType>
            {
                new DTOIndicatorType { Id = (int)IndicatorType.MovingAverage, Description = "Media móvil" },
                new DTOIndicatorType { Id = (int)IndicatorType.ExponentialMovingAverage, Description = "Media móvil exponencial" },
                new DTOIndicatorType { Id = (int)IndicatorType.CurrentCandle, Description = "Vela actual" },
            };
        }

        public IIndicator GetIndicator(int indicatorTypeId)
        {
            IIndicator indicator;

            switch (indicatorTypeId)
            {
                case (int)IndicatorType.MovingAverage:
                    indicator = new MovingAverage();
                    break;
                case (int)IndicatorType.ExponentialMovingAverage:
                    indicator = new ExponentialMovingAverage();
                    break;
                case (int)IndicatorType.CurrentCandle:
                    indicator = new CurrentCandle();
                    break;
                default:
                    indicator = null;
                    break;
            }

            return indicator;
        }
    }
}

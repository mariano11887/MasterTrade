using System;

namespace _4.DTO
{
    public class DTOBacktestingParameters
    {
        public DTOStrategy Strategy { get; set; }
        public int CryptoPairId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int TemporalityId { get; set; }
    }
}

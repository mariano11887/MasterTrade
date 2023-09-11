using System.Collections.Generic;

namespace _4.DTO
{
    public class DTOIndicator
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public List<DTOIndicatorMeta> Metas { get; set; } = new List<DTOIndicatorMeta>();
    }
}

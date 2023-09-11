using System.Collections.Generic;
using System.Linq;

namespace _4.DTO
{
    public class DTOIndicator
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public bool Removed { get; set; }
        public List<DTOIndicatorMeta> Metas { get; set; } = new List<DTOIndicatorMeta>();

        public override string ToString()
        {
            string result = Name;

            if (Metas.Any())
            {
                result += " (" + string.Join(", ", Metas.Select(m => m.Value)) + ")";
            }

            return result;
        }
    }
}

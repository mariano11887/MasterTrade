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
        public List<DTOIndicatorConfiguration> Configurations { get; set; } = new List<DTOIndicatorConfiguration>();

        public override string ToString()
        {
            string result = Name;

            if (Configurations.Any())
            {
                result += " (" + string.Join(", ", Configurations.Select(m => m.Value)) + ")";
            }

            return result;
        }
    }
}

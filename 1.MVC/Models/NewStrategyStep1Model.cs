using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MasterTrade.Models
{
    public class NewStrategyStep1Model
    {
        public int StrategyId { get; set; }

        [DisplayName("Nombre de la estrategia")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MasterTrade.Models
{
    public class NewStrategyStep1Model
    {
        [DisplayName("Nombre de la estrategia")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
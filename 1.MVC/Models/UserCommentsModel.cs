using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterTrade.Models
{
    public class UserCommentsModel
    {
        [DisplayName("0/2000")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Comments { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MasterTrade.Models
{
    public class CryptoPairModel
    {
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [DisplayName("Proveedor")]
        public int SupplierId { get; set; }

        [DisplayName("Par de criptomonedas")]
        public int CryptoPairId { get; set; }

        public List<SelectListItem> AllSuppliers { get; set; }
        public List<SelectListItem> AllCryptoPairs { get; set; }
    }
}
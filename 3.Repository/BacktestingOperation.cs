//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _3.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class BacktestingOperation
    {
        public int Id { get; set; }
        public int BacktestingId { get; set; }
        public System.DateTime OpenDate { get; set; }
        public System.DateTime CloseDate { get; set; }
        public decimal InitialCapital { get; set; }
        public decimal Revenue { get; set; }
    
        public virtual Backtesting Backtesting { get; set; }
    }
}

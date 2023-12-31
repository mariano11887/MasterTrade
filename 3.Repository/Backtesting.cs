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
    
    public partial class Backtesting
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Backtesting()
        {
            this.BacktestingOperations = new HashSet<BacktestingOperation>();
            this.Indicators = new HashSet<Indicator>();
        }
    
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public int BacktestingBatchId { get; set; }
    
        public virtual BacktestingBatch BacktestingBatch { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BacktestingOperation> BacktestingOperations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Indicator> Indicators { get; set; }
    }
}

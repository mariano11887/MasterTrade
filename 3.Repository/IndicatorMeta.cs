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
    
    public partial class IndicatorMeta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IndicatorMeta()
        {
            this.StrategyConditions = new HashSet<StrategyCondition>();
            this.StrategyConditions1 = new HashSet<StrategyCondition>();
        }
    
        public int Id { get; set; }
        public int IndicatorId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Nullable<int> IndicatorMetaDataTypeId { get; set; }
    
        public virtual Indicator Indicator { get; set; }
        public virtual IndicatorMetaDataType IndicatorMetaDataType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StrategyCondition> StrategyConditions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StrategyCondition> StrategyConditions1 { get; set; }
    }
}

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
    
    public partial class StrategyCondition
    {
        public int Id { get; set; }
        public int StrategyId { get; set; }
        public int FirstIndicatorMetaId { get; set; }
        public int SecondIndicatorMetaId { get; set; }
        public int ComparerId { get; set; }
        public bool IsOpenCondition { get; set; }
        public int ExecutionMomentId { get; set; }
    
        public virtual Comparer Comparer { get; set; }
        public virtual Strategy Strategy { get; set; }
        public virtual ExecutionMoment ExecutionMoment { get; set; }
        public virtual IndicatorMeta FirstIndicatorMeta { get; set; }
        public virtual IndicatorMeta SecondIndicatorMeta { get; set; }
    }
}

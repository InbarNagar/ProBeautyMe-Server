//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BeautyMe
{
    using System;
    using System.Collections.Generic;
    
    public partial class Business_can_give_treatment
    {
        public int Type_treatment_Number { get; set; }
        public int Category_Number { get; set; }
        public int Business_Number { get; set; }
        public decimal Price { get; set; }
        public System.TimeSpan Treatment_duration { get; set; }
    
<<<<<<< HEAD
        public virtual Business Business { get; set; }
=======
>>>>>>> 3c4c6229b5211385e2e6b94eb8dc02baff5fa905
        public virtual Category Category { get; set; }
        public virtual Type_Treatment Type_Treatment { get; set; }
    }
}

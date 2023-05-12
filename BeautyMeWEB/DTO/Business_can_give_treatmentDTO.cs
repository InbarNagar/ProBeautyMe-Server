using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautyMeWEB.DTO
{
    public class Business_can_give_treatmentDTO
    {
        public int Type_treatment_Number;
        public int Category_Number;
        public int Business_Number;
        public decimal Price;
        public System.TimeSpan Treatment_duration;
        public string Name_Type_treatment;
    }
}
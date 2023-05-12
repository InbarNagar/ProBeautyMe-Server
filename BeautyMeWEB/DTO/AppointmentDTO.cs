using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautyMeWEB.DTO
{
    public class AppointmentDTO
    {
        public int Number_appointment { get; set; }
        public System.DateTime Date { get; set; }
        public System.TimeSpan Start_time { get; set; }
        public System.TimeSpan End_time { get; set; }
        public string Is_client_house { get; set; }
        public int Business_Number { get; set; }
        public string Appointment_status { get; set; }
        public string AddressStreet;
        public string AddressCity;
        public string AddressHouseNumber;
        public string BusinessName;//business name
    }
}



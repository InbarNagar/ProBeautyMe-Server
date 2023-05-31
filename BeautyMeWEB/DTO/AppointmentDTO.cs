using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautyMeWEB.DTO
{
    public class AppointmentDTO
    {
        public int Number_appointment;
        public System.DateTime Date;
        public System.TimeSpan Start_time;
        public System.TimeSpan End_time;
        public string Is_client_house;
        public int Business_Number;
        public string Appointment_status;
        public string AddressStreet;
        public string AddressCity;
        public string AddressHouseNumber;
        public string BusinessName;//business name
        public string ID_Client;
        public string First_name;
        public string Last_name;
        public string Email;
        public string phone;

    }
}



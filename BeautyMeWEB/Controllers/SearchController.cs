//using BeautyMe;
//using BeautyMeWEB.DTO;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Mvc;
//using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
//using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
//using RouteAttribute = System.Web.Http.RouteAttribute;
//using System.Data.Entity;

//namespace BeautyMeWEB.Controllers
//{
//    public class SearchController : ApiController
//    {
//        BeautyMeDBContext db = new BeautyMeDBContext();

//        // Post: Search
//        [HttpPost]
//        [Route("api/Search/Searchh")]
//        public HttpResponseMessage GetSearchh([FromBody] SearchDTO x)
//        {
//            if (x.Is_client_house == null && x.gender == null)
//            {
 
//                int number_treatment = db.Type_Treatment.FirstOrDefault(i => i.Name == x.NameTreatment).Type_treatment_Number;
//                List<AppointmentDTO> SearchListForType_treatment = db.Appointment.Include(a => a.Appointment_can_give_treatment) // כדי לקבל את פרטי סוג הטיפול של התור
//                                                                           .Include(a => a.Business) // כדי לקבל את פרטי העסק של התור
//                                                                           .Where(a => a.Appointment_status == "available" && a.Business.AddressCity == x.AddressCity && a.Appointment_can_give_treatment.Any(t => t.Type_treatment_Number == number_treatment))
//                                                                           .Select(a => new AppointmentDTO
//                                                                           {
//                                                                               Number_appointment = a.Number_appointment,
//                                                                               Date = a.Date,
//                                                                               Start_time = a.Start_time,
//                                                                               End_time = a.End_time,
//                                                                               Is_client_house = a.Is_client_house,
//                                                                               Business_Number = a.Business_Number,
//                                                                               Appointment_status = a.Appointment_status,
//                                                                               AddressCity=a.Business.AddressCity,
//                                                                               AddressHouseNumber=a.Business.AddressHouseNumber,
//                                                                               AddressStreet=a.Business.AddressStreet,
//                                                                               BusinessName = a.Business.Name

//                                                                           })
//                                                                           .ToList(); //נוצרה רשימה של תורים לפי סוג הטיפול הרצוי והעיר
//                return Request.CreateResponse(HttpStatusCode.OK, SearchListForType_treatment);
//            }
//            if (x.Is_client_house != null && x.gender == null)
//            {
//                int number_treatment = db.Type_Treatment.FirstOrDefault(i => i.Name == x.NameTreatment).Type_treatment_Number;
//                List<AppointmentDTO> SearchListForType_treatment = db.Appointment.Include(a => a.Appointment_can_give_treatment) // כדי לקבל את פרטי סוג הטיפול של התור
//                                                                           .Include(a => a.Business) // כדי לקבל את פרטי העסק של התור
//                                                                           .Where(a => a.Appointment_status == "available" && a.Business.AddressCity == x.AddressCity && a.Appointment_can_give_treatment.Any(t => t.Type_treatment_Number == number_treatment) && a.Is_client_house == x.Is_client_house)
//                                                                           .Select(a => new AppointmentDTO
//                                                                           {
//                                                                               Number_appointment = a.Number_appointment,
//                                                                               Date = a.Date,
//                                                                               Start_time = a.Start_time,
//                                                                               End_time = a.End_time,
//                                                                               Is_client_house = a.Is_client_house,
//                                                                               Business_Number = a.Business_Number,
//                                                                               AddressCity = a.Business.AddressCity,
//                                                                               AddressHouseNumber = a.Business.AddressHouseNumber,
//                                                                               AddressStreet = a.Business.AddressStreet,
//                                                                               BusinessName = a.Business.Name

//                                                                           })
//                                                                           .ToList(); //נוצרה רשימה של תורים לפי סוג הטיפול הרצוי, העיר והאם רצו בבית הלקוח או בבית העסק
//                return Request.CreateResponse(HttpStatusCode.OK, SearchListForType_treatment);
//            }
//            if (x.Is_client_house == null && x.gender != null)
//            {
//                int number_treatment = db.Type_Treatment.FirstOrDefault(i => i.Name == x.NameTreatment).Type_treatment_Number;
//                List<AppointmentDTO> SearchListForType_treatment = db.Appointment.Include(a => a.Appointment_can_give_treatment) // כדי לקבל את פרטי סוג הטיפול של התור
//                                                                           .Include(a => a.Business) // כדי לקבל את פרטי העסק של התור
//                                                                           .Include(a => a.Business.Professional) // כדי לקבל את פרטי בעל העסק
//                                                                           .Where(a => a.Appointment_status == "available" && a.Business.AddressCity == x.AddressCity && a.Appointment_can_give_treatment.Any(t => t.Type_treatment_Number == number_treatment)
//                                                                                    && a.Business.Professional.gender == x.gender)
//                                                                           .Select(a => new AppointmentDTO
//                                                                           {
//                                                                               Number_appointment = a.Number_appointment,
//                                                                               Date = a.Date,
//                                                                               Start_time = a.Start_time,
//                                                                               End_time = a.End_time,
//                                                                               Is_client_house = a.Is_client_house,
//                                                                               Business_Number = a.Business_Number,
//                                                                               AddressCity = a.Business.AddressCity,
//                                                                               AddressHouseNumber = a.Business.AddressHouseNumber,
//                                                                               AddressStreet = a.Business.AddressStreet,
//                                                                               BusinessName = a.Business.Name

//                                                                           })
//                                                                           .ToList(); //נוצרה רשימה של תורים לפי סוג הטיפול הרצוי, העיר ומין בעל העסק הרצוי
//                return Request.CreateResponse(HttpStatusCode.OK, SearchListForType_treatment);
//            }
//            else //if (x.Is_client_house != null && x.gender != null)
//            {
//                int number_treatment = db.Type_Treatment.FirstOrDefault(i => i.Name == x.NameTreatment).Type_treatment_Number;
//                List<AppointmentDTO> SearchListForType_treatment = db.Appointment.Include(a => a.Appointment_can_give_treatment) // כדי לקבל את פרטי סוג הטיפול של התור
//                                                                           .Include(a => a.Business) // כדי לקבל את פרטי העסק של התור
//                                                                           .Include(a => a.Business.Professional) // כדי לקבל את פרטי בעל העסק
//                                                                           .Where(a => a.Appointment_status == "available" && a.Business.AddressCity == x.AddressCity && a.Appointment_can_give_treatment.Any(t => t.Type_treatment_Number == number_treatment)
//                                                                                    && a.Is_client_house == x.Is_client_house && a.Business.Professional.gender == x.gender)
//                                                                           .Select(a => new AppointmentDTO
//                                                                           {
//                                                                               Number_appointment = a.Number_appointment,
//                                                                               Date = a.Date,
//                                                                               Start_time = a.Start_time,
//                                                                               End_time = a.End_time,
//                                                                               Is_client_house = a.Is_client_house,
//                                                                               Business_Number = a.Business_Number,
//                                                                               AddressCity = a.Business.AddressCity,
//                                                                               AddressHouseNumber = a.Business.AddressHouseNumber,
//                                                                               AddressStreet = a.Business.AddressStreet,
//                                                                               BusinessName=a.Business.Name
//                                                                           })
//                                                                           .ToList(); //נוצרה רשימה של תורים לפי סוג הטיפול הרצוי, העיר, האם רצו בבית הלקוח או בבית העסק, ומין בעל העסק הרצוי
//                return Request.CreateResponse(HttpStatusCode.OK, SearchListForType_treatment);
//            }
//        }
//    }
//}


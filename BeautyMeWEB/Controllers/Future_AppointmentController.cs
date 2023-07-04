//using BeautyMe;
//using BeautyMeWEB.DTO;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Mvc;
//using HttpDeleteAttribute = System.Web.Http.HttpDeleteAttribute;
//using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
//using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
//using HttpPutAttribute = System.Web.Http.HttpPutAttribute;
//using RouteAttribute = System.Web.Http.RouteAttribute;

//namespace BeautyMeWEB.Controllers
//{
//    public class Future_AppointmentController : ApiController
//    {
//        BeautyMeDBContext db = new BeautyMeDBContext();

//        // GET: Future_Appointment
//        [HttpGet]
//        [Route("api/Future_Appointment/AllFuture_Appointment")]
//        public HttpResponseMessage GetAllFuture_Appointment()
//        {
 
//            List<Future_AppointmentDTO> AllFuture_Appointment = db.Future_Appointment.Select(x => new Future_AppointmentDTO
//            {
//                Future_appointment_number = x.Future_appointment_number,
//                AddressStreet = x.AddressStreet,
//                AddressHouseNumber = x.AddressHouseNumber,
//                AddressCity = x.AddressCity,
//                Appointment_status = x.Appointment_status,
//                Client_ID_number = x.Client_ID_number,
//                Type_treatment_Number = x.Type_treatment_Number,
//                Number_appointment = x.Number_appointment,
               
//             }).ToList();
//            if (AllFuture_Appointment != null)
//                return Request.CreateResponse(HttpStatusCode.OK, AllFuture_Appointment);
//            else
//                return Request.CreateResponse(HttpStatusCode.NotFound);
//        }


//        // Post: Future_Appointment
//        [HttpPost]
//        [Route("api/Future_Appointment/AllFuture_AppointmentForClient/{Client_ID_numberr}")]
//        public HttpResponseMessage GetAllFuture_AppointmentForClient(string Client_ID_numberr)
//        {
 
//            List<Future_AppointmentDTO> AllFuture_Appointment = db.Future_Appointment.Where(a => a.Client_ID_number == Client_ID_numberr).Select(x => new Future_AppointmentDTO
//            {
//                Future_appointment_number = x.Future_appointment_number,
//                AddressStreet = x.AddressStreet,
//                AddressHouseNumber = x.AddressHouseNumber,
//                AddressCity = x.AddressCity,
//                Appointment_status = x.Appointment_status,
//                Client_ID_number = x.Client_ID_number,
//                Type_treatment_Number = x.Type_treatment_Number,
//                Number_appointment = x.Number_appointment
//            }).ToList();
//            if (AllFuture_Appointment != null)
//                return Request.CreateResponse(HttpStatusCode.OK, AllFuture_Appointment);
//            else
//                return Request.CreateResponse(HttpStatusCode.NotFound);
//        }


//        // Post: Future_Appointment  מחזיר את כל התורים הקבועים לבעל העסק
//        [HttpPost]
//        [Route("api/Future_Appointment/AllFuture_AppointmentForProfessional/{Business_Numberr}")]
//        public HttpResponseMessage GetAllFuture_AppointmentForProfessional(int Business_Numberr)
//        {

//            List<Future_AppointmentDTO> AllFuture_Appointment = db.Future_Appointment.Include(a => a.Appointment).Include(a => a.Client)
//                                                                                    .Where(a => a.Appointment.Business_Number == Business_Numberr).Select(x => new Future_AppointmentDTO
//            {
//                Future_appointment_number = x.Future_appointment_number,
//                AddressStreet = x.AddressStreet,
//                AddressHouseNumber = x.AddressHouseNumber,
//                AddressCity = x.AddressCity,
//                Appointment_status = x.Appointment_status,
//                Client_ID_number = x.Client_ID_number,
//                Type_treatment_Number = x.Type_treatment_Number,
//                Number_appointment = x.Number_appointment,
//                Date = x.Appointment.Date,
//                Start_time = x.Appointment.Start_time,
//                End_time = x.Appointment.End_time,
//                First_name_client = x.Client.First_name,
//                Last_name_client = x.Client.Last_name

//        }).ToList();
//            if (AllFuture_Appointment != null)
//                return Request.CreateResponse(HttpStatusCode.OK, AllFuture_Appointment);
//            else
//                return Request.CreateResponse(HttpStatusCode.NotFound);
//        }


//        // Post: api/Post
//        [HttpPost]
//        [Route("api/Future_Appointment/NewFuture_Appointment")]
//        public HttpResponseMessage PostNewFuture_Appointment([FromBody] Future_AppointmentDTO x)
//        {
//            try
//            {
 
//                Appointment theScheduledAppointment = db.Appointment.Find(x.Number_appointment);
//                if (theScheduledAppointment != null)
//                {
//                    if (theScheduledAppointment.Appointment_status.ToLower() == "available".ToLower())
//                    {
//                        Future_Appointment newFuture_Appointment = new Future_Appointment()
//                        {
//                            AddressStreet = x.AddressStreet,
//                            AddressHouseNumber = x.AddressHouseNumber,
//                            AddressCity = x.AddressCity,
//                            Appointment_status = "Awaiting_approval",
//                            Client_ID_number = x.Client_ID_number,
//                            Type_treatment_Number = x.Type_treatment_Number,
//                            Number_appointment = x.Number_appointment
//                        };
//                        db.Future_Appointment.Add(newFuture_Appointment);
//                        theScheduledAppointment.Appointment_status = "not available";
//                        db.SaveChanges();
//                        return Request.CreateResponse(HttpStatusCode.OK, "new Future_Appointment added to the dataBase");
                  
//                    }
//                    else
//                    {
//                        return Request.CreateResponse(HttpStatusCode.NotFound, "this appointment not available");
//                    }
//                }
//                else
//                {
//                    return Request.CreateResponse(HttpStatusCode.NotFound, $"this Appointment not found any more"); //אם אין תור כזה.

//                }
//            }
//            catch (DbUpdateException ex)
//            {
//                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred while adding new Future_Appointment to the database: " + ex.InnerException.InnerException.Message);
//            }
//        }


//        // Put: api/Put
//        [HttpPut]
//        [Route("api/Future_Appointment/UpdateFuture_Appointment")]
//        public HttpResponseMessage PutUpdateFuture_Appointment([FromBody] Future_AppointmentDTO x)
//        {
 
//            Future_Appointment Future_AppointmentToUpdate = db.Future_Appointment.FirstOrDefault(a => a.Number_appointment == x.Number_appointment);
//            if (Future_AppointmentToUpdate == null)
//            {
//                return Request.CreateResponse(HttpStatusCode.NotFound, $"Appointment with number {x.Number_appointment} not found.");
//            }

//            else
//            {
//                //Future_AppointmentToUpdate.Future_appointment_number = x.Future_appointment_number;
//                Future_AppointmentToUpdate.AddressStreet = x.AddressStreet;
//                Future_AppointmentToUpdate.AddressHouseNumber = x.AddressHouseNumber;
//                Future_AppointmentToUpdate.AddressCity = x.AddressCity;
//                Future_AppointmentToUpdate.Appointment_status = x.Appointment_status;
//                Future_AppointmentToUpdate.Client_ID_number = x.Client_ID_number;
//                Future_AppointmentToUpdate.Type_treatment_Number = x.Type_treatment_Number;
//                Future_AppointmentToUpdate.Number_appointment = x.Number_appointment;
//                db.SaveChanges();
//                return Request.CreateResponse(HttpStatusCode.OK, "The Appointment update in the dataBase");
//            }
//        }


//        // Delete: api/Delete
//        [HttpDelete]
//        [Route("api/Future_Appointment/CanceleFuture_Appointment")]
//        public IHttpActionResult DeleteCanceleFuture_Appointment([FromBody] Future_AppointmentDTO x)
//        {
 
//            {
//                if (x == null)  // בדיקת תקינות ה-DTO שהתקבל
//                {
//                    return BadRequest("הפרטים שהתקבלו אינם תקינים.");
//                }
             
//                Future_Appointment CanceleFuture_Appointment = db.Future_Appointment.Find(x.Future_appointment_number);   // חיפוש הרשומה המתאימה לפי המזהה שלה
//                if (CanceleFuture_Appointment == null)
//                {
//                    return NotFound();
//                }

//                if(CanceleFuture_Appointment.Appointment_status == "Appointment_ended" || CanceleFuture_Appointment.Appointment_status == "Cancelled")
//                {
//                    return BadRequest("לא ניתן לבטל את התור מכיוון שעבר זמן אפשרות הביטול");
//                }
//                else
//                {
                  
//                    db.Future_Appointment.Remove(CanceleFuture_Appointment);   // מחיקת הרשומה מבסיס הנתונים
//                    Appointment theAppointmentThatCanceled = db.Appointment.FirstOrDefault(a => a.Number_appointment == x.Number_appointment);
                    
//                    theAppointmentThatCanceled.Appointment_status = "available"; //שינוי סטטוס התור לתור פנוי בטבלת "תור"

//                    db.SaveChanges();
//                }
//                return Ok("הנתונים נמחקו בהצלחה.");  // החזרת תשובה מתאימה לפי המצב
//            }
//        }
//    }
//}



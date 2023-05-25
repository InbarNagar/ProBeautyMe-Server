using BeautyMe;
using BeautyMeWEB.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using System.Data.Entity;
using HttpDeleteAttribute = System.Web.Http.HttpDeleteAttribute;
using HttpPutAttribute = System.Web.Http.HttpPutAttribute;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace BeautyMeWEB.Controllers
{
    public class AppointmentController : ApiController
    {
        // GET: Appointment
        BeautyMeDBContext db = new BeautyMeDBContext();

        [HttpGet]
        [Route("api/Appointment/AllAppointment")]
        public HttpResponseMessage GetAllAppointment()
        {
            List<AppointmentDTO> AllAppointment = db.Appointment.Select(x => new AppointmentDTO
            {
                Number_appointment = x.Number_appointment,
                Date = x.Date,
                Start_time = x.Start_time,
                End_time = x.End_time,
                Is_client_house = x.Is_client_house,
                Business_Number = x.Business_Number,
                Appointment_status = x.Appointment_status,
            }).ToList();
            if (AllAppointment != null)
                return Request.CreateResponse(HttpStatusCode.OK, AllAppointment);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // Post: Appointment
        [HttpPost]
        [Route("api/Appointment/AllAppointmentForBussines/{Business_Numberr}")]
        public HttpResponseMessage GetAllAppointmentForBussines(int Business_Numberr)
        {
            List<AppointmentDTO> AllAppointment = db.Appointment.Where(a => a.Business_Number == Business_Numberr).Select(x => new AppointmentDTO
            {
                Number_appointment = x.Number_appointment,
                Date = x.Date,
                Start_time = x.Start_time,
                End_time = x.End_time,
                Is_client_house = x.Is_client_house,
                Business_Number = x.Business_Number,
                Appointment_status = x.Appointment_status,
            }).ToList();
            if (AllAppointment != null)
                return Request.CreateResponse(HttpStatusCode.OK, AllAppointment);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }


        // Post: api/Post
        [HttpPost]
        [Route("api/Appointment/NewAppointment")]
        public HttpResponseMessage PostNewAppointment([FromBody] AppointmentDTO x)
        {
            try
            {
                Appointment newAppointment = new Appointment()
                {
                    //Number_appointment = x.Number_appointment,
                    Date = x.Date,
                    Start_time = x.Start_time,
                    End_time = x.End_time,
                    Is_client_house = x.Is_client_house,
                    Business_Number = x.Business_Number,
                    Appointment_status = x.Appointment_status,

                };
                db.Appointment.Add(newAppointment);
                db.SaveChanges();
                int newAppointmentId = newAppointment.Number_appointment;
                return Request.CreateResponse(HttpStatusCode.OK, new { message = "New appointment added to the database", appointmentId = newAppointmentId });
            }
            catch (DbUpdateException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred while adding new Appointment to the database: " + ex.InnerException.InnerException.Message);
            }
        }
        [HttpPost]
        [Route("api/Appointment/ClientToAppointment")]
        public HttpResponseMessage ClientToAppointment([FromBody] AppointmentDTO x)
        {
            Appointment appointment = db.Appointment.Where(z=>z.Number_appointment== x.Number_appointment && z.Appointment_status== "Available" && z.ID_Client==null).FirstOrDefault();
            if (appointment != null)
            {
                appointment.ID_Client= x.ID_Client;
                appointment.Appointment_status = "Awaiting_approval";
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"{x.ID_Client} is assigned to {appointment.Number_appointment}");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "error");

            }
        }
        ////תורים ללקוח
        //[HttpGet]
        //[Route("api/Appointment/AllAppointmentForClient/{ID_Client}")]
        //public HttpResponseMessage GetAllAppointmentForClient(string ID_Client)
        //{
        //    List<AppointmentDTO> AllAppointment = db.Appointment.Where(a => a.ID_Client == ID_Client && a.ID_Client!=null).Select(x => new AppointmentDTO
        //    {
        //        Number_appointment = x.Number_appointment,
        //        BusinessName=x.Business.Name,
        //        Date = x.Date,
        //        Start_time = x.Start_time,
        //        End_time = x.End_time,
        //        Is_client_house = x.Is_client_house,
        //        Business_Number = x.Business_Number,
        //        Appointment_status = x.Appointment_status,
        //        AddressStreet = x.Business.AddressStreet,
        //        AddressHouseNumber = x.Business.AddressHouseNumber,
        //        AddressCity = x.Business.AddressCity
        //    }).ToList();
        //    if (AllAppointment != null)
        //        return Request.CreateResponse(HttpStatusCode.OK, AllAppointment);
        //    else
        //        return Request.CreateResponse(HttpStatusCode.NotFound);
        //}
        // Put: api/Put
        [HttpPut]
        [Route("api/Appointment/UpdateAppointment")]
        public HttpResponseMessage PutUpdateAppointment([FromBody] AppointmentDTO x)
        {
            Appointment AppointmentToUpdate = db.Appointment.FirstOrDefault(a => a.Number_appointment == x.Number_appointment);
            if (AppointmentToUpdate == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"Appointment with number {x.Number_appointment} not found.");
            }

            else
            {
                //AppointmentToUpdate.Number_appointment = x.Number_appointment;
                AppointmentToUpdate.Date = x.Date;
                AppointmentToUpdate.Start_time = x.Start_time;
                AppointmentToUpdate.End_time = x.End_time;
                AppointmentToUpdate.Is_client_house = x.Is_client_house;
                AppointmentToUpdate.Business_Number = x.Business_Number;
                AppointmentToUpdate.Appointment_status = x.Appointment_status;

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "The Appointment update in the dataBase");
            }
        }


        // Delete: api/Delete
        [HttpDelete]
        [Route("api/Appointment/CanceleAppointment")]
        public IHttpActionResult DeleteCanceleAppointment([FromBody] AppointmentDTO x)
        {
            if (x == null)  // בדיקת תקינות ה-DTO שהתקבל
            {
                return BadRequest("הפרטים שהתקבלו אינם תקינים.");
            }

            Appointment CanceleAppointment = db.Appointment.Find(x.Number_appointment);   // חיפוש הרשומה המתאימה לפי המזהה שלה
            if (CanceleAppointment == null)
            {
                return NotFound();
            }

            db.Appointment.Remove(CanceleAppointment);   // מחיקת הרשומה מבסיס הנתונים

            db.SaveChanges();

            return Ok("הנתונים נמחקו בהצלחה.");  // החזרת תשובה מתאימה לפי המצב
        }
        // קונטרולר לשינוי הסטטוס לפי לקוח
        [HttpPut]
        [Route("api/Appointment/changeStatus/{clientID}")]
        public HttpResponseMessage ChangeStatusByClient(string clientID)
        {
            Appointment AppointmentToChangeStatus = db.Appointment.Where(x => x.ID_Client == clientID && x.Appointment_status== "Awaiting_approval").FirstOrDefault();
            if (AppointmentToChangeStatus == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, $"client ${clientID} has no appointments in dataBase!");
            }

            else
            {
                AppointmentToChangeStatus.Appointment_status = "Appointment_ended";

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"{AppointmentToChangeStatus.Number_appointment} updated in the dataBase");
            }
        }

        [HttpPost]
        [Route("api/Appointment/AllAppointmentForClient")]
        public HttpResponseMessage GetAllAppointmentForClient([FromBody] ClientDTO x)
        {
            string ID_Client = x.ID_number;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
            string query = @"select A.*, p.token,B.*
             from Appointment A inner join Business B ON A.Business_Number=b.Business_Number inner join Client C 
             on A.ID_Client= c.ID_number inner join Professional p on p.ID_number=b.Professional_ID_number
             where ID_Client=@ID_Client";

            SqlDataAdapter adpter = new SqlDataAdapter(query, con);
            adpter.SelectCommand.Parameters.AddWithValue("@ID_Client", ID_Client);
            DataSet ds = new DataSet();
            adpter.Fill(ds, "Appointment");
            DataTable dt = ds.Tables["Appointment"];
            return Request.CreateResponse(HttpStatusCode.OK, dt);
        }

        [HttpPost]
        [Route("api/Appointment/AllAppointment")]
        public HttpResponseMessage GetAllAppointment([FromBody]object x)
        {
           
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
            string query = @"select A.*, p.token,B.*
             from Appointment A inner join Business B ON A.Business_Number=b.Business_Number  inner join Professional p on p.ID_number=b.Professional_ID_number";
            SqlDataAdapter adpter = new SqlDataAdapter(query, con);
  
            DataSet ds = new DataSet();
            adpter.Fill(ds, "Appointment2");
            DataTable dt = ds.Tables["Appointment2"];
            return Request.CreateResponse(HttpStatusCode.OK, dt);
        }


        [HttpGet]
        [Route("api/Appointment/AllAvailableAppointment")]
        public HttpResponseMessage AllAvailableAppointment()
        {
            List<AppointmentDTO> AllAppointment = db.Appointment.Where(x => x.Appointment_status == "available" && x.ID_Client == null).Select(a => new AppointmentDTO
            {
                Number_appointment = a.Number_appointment,
                Date = a.Date,
                Start_time = a.Start_time,
                End_time = a.End_time,
                Is_client_house = a.Is_client_house,
                Business_Number = a.Business_Number,
                Appointment_status = a.Appointment_status,
                AddressCity = a.Business.AddressCity,
                AddressHouseNumber = a.Business.AddressHouseNumber,
                AddressStreet = a.Business.AddressStreet,
                BusinessName = a.Business.Name
            }).ToList();
            if (AllAppointment != null)
                return Request.CreateResponse(HttpStatusCode.OK, AllAppointment);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }


        [HttpDelete]
        [Route("api/Appointment/CanceleAppointmentByClient/{appointmentNumber}")]
        public IHttpActionResult CancelAppointmentByClient(int appointmentNumber)
        {
            if (appointmentNumber.ToString() == null)  // בדיקת תקינות ה-DTO שהתקבל
            {
                return BadRequest("הפרטים שהתקבלו אינם תקינים.");
            }
            else
            {
                Appointment CanceleAppointment = db.Appointment.Where(x => x.Number_appointment == appointmentNumber).FirstOrDefault();   // חיפוש הרשומה המתאימה לפי המזהה שלה
                if (CanceleAppointment != null && CanceleAppointment.Appointment_status == "Awaiting_approval")
                {
                    db.Appointment.Remove(CanceleAppointment);   // מחיקת הרשומה מבסיס הנתונים
                    db.SaveChanges();
                    return Ok("הנתונים נמחקו בהצלחה.");
                }  // החזרת תשובה מתאימה לפי המצב
                else
                {
                    return NotFound();
                }
            }
        }


    }
}




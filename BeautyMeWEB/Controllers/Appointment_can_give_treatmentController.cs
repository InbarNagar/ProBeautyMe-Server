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

namespace BeautyMeWEB.Controllers
{
    public class Appointment_can_give_treatmentController : ApiController
    {
        BeautyMeDBContext db = new BeautyMeDBContext();

        // Post: api/Appointment_can_give_treatmentController/All_the_treatments_appointment_can_give   מחזיר את כל סוגי הטיפולים האפשריים לתור מסויים
        [HttpPost]
        [Route("api/Appointment_can_give_treatmentController/All_the_treatments_appointment_can_give")]
        public HttpResponseMessage GetAll_the_treatments_appointment_can_give([FromBody] int Number_appointmentt)
        {
            List<Appointment_can_give_treatmentDTO> all_the_treatments_appointment_can_give = db.Appointment_can_give_treatment.Where(a => a.Number_appointment == Number_appointmentt).Select(x => new Appointment_can_give_treatmentDTO
            {
                number = x.number,
                Type_treatment_Number = x.Type_treatment_Number,
                Number_appointment = x.Number_appointment,
            }).ToList();
            if (all_the_treatments_appointment_can_give != null)
                return Request.CreateResponse(HttpStatusCode.OK, all_the_treatments_appointment_can_give);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // Post: api/Post //מכניס סוג טיפול חדש לתור
        [HttpPost]
        [Route("api/Appointment_can_give_treatment/NewAppointment_can_give_treatment")]
        public HttpResponseMessage PostNewAppointment_can_give_treatment([FromBody] Appointment_can_give_treatmentDTO x)
        {
            try
            {
                Appointment theAppointment = db.Appointment.Find(x.Number_appointment); //בודק אם קיים תור כזה
                if (theAppointment != null)
                {
                    Appointment_can_give_treatment newAppointment_can_give_treatment = new Appointment_can_give_treatment()
                    {
                        Type_treatment_Number = x.Type_treatment_Number,
                        Number_appointment = x.Number_appointment,
                    };
                    db.Appointment_can_give_treatment.Add(newAppointment_can_give_treatment);
                    theAppointment.Appointment_status = "Available"; //משנה את סטטוס התור מתור בתהליך בנייה לתור פנוי
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "new Appointment_can_give_treatment added to the dataBase");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, $"this Appointment not found any more"); //אם אין תור כזה.

                }
            }
            catch (DbUpdateException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred while adding new Appointment_can_give_treatment to the database: " + ex.InnerException.InnerException.Message);
            }
        }
    }
}
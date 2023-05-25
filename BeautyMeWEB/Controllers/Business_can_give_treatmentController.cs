using BeautyMe;
using BeautyMeWEB.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace BeautyMeWEB.Controllers
{
    public class Business_can_give_treatmentController : ApiController
    {
        // GET: Business_can_give_treatment   //get כללי...מחזיר את כל הטבלה
        BeautyMeDBContext db = new BeautyMeDBContext();
        [HttpGet]
        [Route("api/Business_can_give_treatment/AllTreatmentOfBusiness")]
        public HttpResponseMessage GetAllTreatmentOfBusiness()
        {
            List<Business_can_give_treatmentDTO> AllTreatmentOfBusiness = db.Business_can_give_treatment.Select(x => new Business_can_give_treatmentDTO
            {
                Type_treatment_Number = x.Type_treatment_Number,
                Category_Number = x.Category_Number,
                Business_Number = x.Business_Number,
                Price = x.Price,
                Treatment_duration = x.Treatment_duration,
            }).ToList();
            if (AllTreatmentOfBusiness != null)
                return Request.CreateResponse(HttpStatusCode.OK, AllTreatmentOfBusiness);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }


        // Post: api/Business_can_give_treatmentController/All_the_treatments_bussines_can_give/{Business_Numberr}   //Post ספציפי...מחזיר את כל הטיפולים שיש לעסק מסויים
        [HttpPost]
        [Route("api/Business_can_give_treatmentController/All_the_treatments_appointment_can_give/{Business_Numberr}")]
        public HttpResponseMessage GetAll_the_treatments_businnes_can_give(int Business_Numberr)
        {

            List<Business_can_give_treatmentDTO> all_the_treatments_businnes_can_give = db.Business_can_give_treatment.Where(a => a.Business_Number == Business_Numberr).Select(x => new Business_can_give_treatmentDTO
            {
                Type_treatment_Number = x.Type_treatment_Number,
                Category_Number = x.Category_Number,
                Business_Number = x.Business_Number,
                Price = x.Price,
                Treatment_duration = x.Treatment_duration,
                Name_Type_treatment = x.Type_Treatment.Name

            }).ToList();
            if (all_the_treatments_businnes_can_give != null)


                return Request.CreateResponse(HttpStatusCode.OK, all_the_treatments_businnes_can_give);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // Post: api/Post
        [HttpPost]
        [Route("api/Business_can_give_treatment/PostNewTreatmentOfBussines")]
        public HttpResponseMessage PostNewTreatmentOfBussines([FromBody] Business_can_give_treatmentDTO x)
        {
            try
            {
 
                Business_can_give_treatment newBusiness = new Business_can_give_treatment()
                {
                    Type_treatment_Number = x.Type_treatment_Number,
                    
                    Category_Number = x.Category_Number,
                    Business_Number = x.Business_Number,
                    Price = x.Price,
                    Treatment_duration = x.Treatment_duration,
                };
                db.Business_can_give_treatment.Add(newBusiness);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "new treatment of Business added to the dataBase");
            }
            catch (DbUpdateException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred while adding new treatment of Business to the database: " + ex.InnerException.InnerException.Message);
            }
        }
    }
}


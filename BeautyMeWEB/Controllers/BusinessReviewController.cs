using BeautyMe;
using BeautyMeWEB.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BeautyMeWEB.Controllers
{
    public class BusinessReviewController : ApiController
    {
        BeautyMeDBContext db = new BeautyMeDBContext();

        [HttpPost]
        [Route("api/BusinessReview/NewBusinessReviewByClient")]
        public HttpResponseMessage NewBusinessReviewByClient([FromBody] BusinessReviewDTO b)
        {
            try
            {
                if (b != null)
                {
                    Review_Business newBr = new Review_Business()
                    {
                        Number_appointment = b.Number_appointment,
                        Cleanliness = b.Cleanliness,
                        Professionalism = b.Professionalism,
                        On_time = b.On_time, //Client Side - product variable
                        Overall_rating = b.Overall_rating,
                        Client_ID_number = b.Client_ID_number,
                        Business_Number = b.Business_Number,
                        Comment = b.Comment
                    };
                    db.Review_Business.Add(newBr);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, newBr);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Post request didn't work " + b);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        // מידע על התורים של הלקוח כולל טבלת ביקורות (לשימוש לכפתור דרג עסק) עם אינר ג'וין 
        [HttpGet]
        [Route("api/BusinessReview/AllAppointmentForClient/{clientID}")]
        public HttpResponseMessage ClientAppointment(string clientID)
        {
            string ID_Client = clientID;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
            string query = @"select a.*,br.*
                        from Appointment a left join Review_Business br on br.Number_appointment=a.Number_appointment  
                        where A.ID_Client=@ID_Client";

            SqlDataAdapter adpter = new SqlDataAdapter(query, con);
            adpter.SelectCommand.Parameters.AddWithValue("@ID_Client", ID_Client);
            DataSet ds = new DataSet();
            adpter.Fill(ds, "Review_Business");
            DataTable dt = ds.Tables["Review_Business"];
            return Request.CreateResponse(HttpStatusCode.OK, dt);
        }

        // מידע על התורים של הלקוח כולל טבלת ביקורות (לשימוש לכפתור דרג עסק) עם אינר ג'וין 
        [HttpGet]
        [Route("api/BusinessReview/AllAppointmentForBusiness/{BusinessNumber}")]
        public HttpResponseMessage BusinessAppointment(int BusinessNumber)
        {
            
          List <BusinessReviewDTO> r = db.Review_Business.Where(x=>x.Business_Number == BusinessNumber).Select( a=> new BusinessReviewDTO
          {
                Business_Number = a.Business_Number,
                Review_Number=a.Review_Number,
                Number_appointment= (int)a.Number_appointment,
                Cleanliness=a.Cleanliness,
                Professionalism=a.Professionalism,
                On_time=a.On_time,
                Overall_rating=a.Overall_rating,
                Client_ID_number=a.Client_ID_number,
                Comment=a.Comment
             }).ToList();
            if (r != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, r);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound,$"No Reviews for business number {BusinessNumber}");
            }

        }
    }
}
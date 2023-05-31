using BeautyMe;
using BeautyMeWEB.DTO;
using System;
using System.Collections.Generic;
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

    }
}
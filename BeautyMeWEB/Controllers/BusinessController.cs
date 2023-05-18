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
using HttpDeleteAttribute = System.Web.Http.HttpDeleteAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace BeautyMeWEB.Controllers
{
    public class BusinessController : ApiController
    {
       BeautyMeDBContext db = new BeautyMeDBContext();
        // GET: Business
        [HttpGet]
        [Route("api/Business/AllBusiness")]
        public HttpResponseMessage GetAllBusiness()
        {
            List<BusinessDTO> AllBusiness = db.Business.Select(x => new BusinessDTO
            {
                Business_Number = x.Business_Number,
                Name = x.Name,
                AddressStreet = x.AddressStreet,
                AddressHouseNumber = x.AddressHouseNumber,
                AddressCity = x.AddressCity,
                Is_client_house = x.Is_client_house,
                Professional_ID_number = x.Professional_ID_number,
            }).ToList();
            if (AllBusiness != null)
                return Request.CreateResponse(HttpStatusCode.OK, AllBusiness);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // Post: api/Business/OneBusiness פונקציה שמקבלת מספר עסק ומחזירה את העסק הספציפי
        [HttpPost]
        [Route("api/Business/OneBusiness/{business_num}")]
        public HttpResponseMessage GetOneBusiness(int business_num)
        {
            BusinessDTO oneBusiness = db.Business.Where(a => a.Business_Number == business_num).Select(x => new BusinessDTO
            {
                Business_Number = x.Business_Number,
                About= x.About,
                Name = x.Name,
                AddressStreet = x.AddressStreet,
                AddressHouseNumber = x.AddressHouseNumber,
                AddressCity = x.AddressCity,
                Is_client_house = x.Is_client_house,
                Professional_ID_number = x.Professional_ID_number,
            }).FirstOrDefault();
            if (oneBusiness != null)
                return Request.CreateResponse(HttpStatusCode.OK, oneBusiness);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // Post: api/Post
        [HttpPost]
        [Route("api/Business/NewBusiness")]

        public HttpResponseMessage PostNewBusiness([FromBody] BusinessDTO x)
        {
            try
            {
                Business newBusiness = new Business()
                {
                    //Business_Number = x.Business_Number,
                    Name = x.Name,
                    AddressStreet = x.AddressStreet,
                    AddressHouseNumber = x.AddressHouseNumber,
                    AddressCity = x.AddressCity,
                    Is_client_house = x.Is_client_house,
                    Professional_ID_number = x.Professional_ID_number
                };
                db.Business.Add(newBusiness);
                db.SaveChanges();
                int newBusinessId = newBusiness.Business_Number;
                return Request.CreateResponse(HttpStatusCode.OK, new { message = "new Business added to the dataBase", businessId = newBusinessId });
            }
        
            catch (DbUpdateException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred while adding new bussines to the database: " + ex.InnerException.InnerException.Message);
            }
        }
        // New Calls
        [HttpPost]
        [Route("api/Business/UpdateBusinesss")]
        public HttpResponseMessage UpdateBusiness([FromBody] BusinessDTO newB)
        {
            Business PrevB = db.Business.Where(a => a.Business_Number == newB.Business_Number).FirstOrDefault();
            if (PrevB != null)
            {
                PrevB.AddressStreet = newB.AddressStreet;
                PrevB.AddressCity = newB.AddressCity;
                PrevB.AddressHouseNumber = newB.AddressHouseNumber;
                PrevB.Is_client_house = newB.Is_client_house;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"{newB.Business_Number} details updated!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "couldn't find business to update!!!");
            }
        }

        [HttpDelete]
        [Route("api/Business/DeleteBusinesss")]
        public HttpResponseMessage DeleteBusiness(int businessId)
        {
            Business BtoDel = db.Business.Where(x => x.Business_Number == businessId).FirstOrDefault();
            if (BtoDel != null)
            {
                db.Business.Remove(BtoDel);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"{BtoDel.Business_Number} deleted from dataBse!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "couldn't delete business from data base!");
            }

        }
    }
}


//// Post: api/Post
//[HttpPost]
//[Route("api/Business/NewBusiness")]

//public HttpResponseMessage PostNewBusiness([FromBody] BusinessDTO x)
//{
//    BeautyMeDBContext db = new BeautyMeDBContext();
//    Business newBusiness = new Business()
//    {
//        //Business_Number = x.Business_Number,
//        Name = x.Name,
//        AddressStreet = x.AddressStreet,
//        AddressHouseNumber = x.AddressHouseNumber,
//        AddressCity = x.AddressCity,
//        Is_client_house = x.Is_client_house,
//        Professional_ID_number = x.Professional_ID_number
//    };
//    if (newBusiness != null)
//    {
//        db.Business.Add(newBusiness);
//        db.SaveChanges();
//        return Request.CreateResponse(HttpStatusCode.OK, "new Business added to the dataBase");
//    }
//    else
//        return Request.CreateResponse(HttpStatusCode.NoContent);
//}



//// Post: api/Post
//[HttpPost]
//[Route("api/Business/NewBusiness")]

//public HttpResponseMessage PostNewBusiness([FromBody] BusinessDTO x)
//{
//    BeautyMeDBContext db = new BeautyMeDBContext();
//    Business newBusiness = new Business()
//    {
//        Business_Number = x.Business_Number,
//        Name = x.Name,
//        AddressStreet = x.AddressStreet,
//        AddressHouseNumber = x.AddressHouseNumber,
//        AddressCity = x.AddressCity,
//        Is_client_house = x.Is_client_house,
//        Professional_ID_number = x.Professional_ID_number
//    };
//    if (newBusiness != null)
//    {
//        db.Business.Add(newBusiness);
//        db.SaveChanges();
//        return Request.CreateResponse(HttpStatusCode.OK, "new Business added to the dataBase");
//    }
//    else
//        return Request.CreateResponse(HttpStatusCode.NoContent);
//}
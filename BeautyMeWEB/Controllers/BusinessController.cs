using BeautyMe;
using BeautyMeWEB.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
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
                About= x.About,
                Facebook_link = x.Facebook_link,
                Instagram_link = x.Instagram_link,
                LetCordinate = (double)x.LetCordinate,
                LongCordinate = (double)x.LongCordinate
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
                Facebook_link = x.Facebook_link,
                Instagram_link = x.Instagram_link,
                LetCordinate = (double)x.LetCordinate,
                LongCordinate = (double)x.LongCordinate
            }).FirstOrDefault();
            if (oneBusiness != null)
                return Request.CreateResponse(HttpStatusCode.OK, oneBusiness);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // Post: api/Post
        [HttpPost]
        [Route("api/Business/NewBusiness")]
        //להוסיף About
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
                    Professional_ID_number = x.Professional_ID_number,
                    About= x.About,
                    Facebook_link=x.Facebook_link,
                    Instagram_link=x.Instagram_link,
                    LetCordinate=x.LetCordinate,
                    LongCordinate=x.LongCordinate

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
        //להוסיף about
        [HttpPost]
        [Route("api/Business/UpdateBusinesss")]
        public HttpResponseMessage UpdateBusiness([FromBody] BusinessDTO newB)
        {
            Business PrevB = db.Business.Where(a => a.Business_Number == newB.Business_Number).FirstOrDefault();
            if (PrevB != null)
            {
                PrevB.Name = newB.Name;
                PrevB.AddressStreet = newB.AddressStreet;
                PrevB.AddressCity = newB.AddressCity;
                PrevB.AddressHouseNumber = newB.AddressHouseNumber;
                PrevB.Is_client_house = newB.Is_client_house;
                PrevB.About= newB.About;
                int x = db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"{x} details updated!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "couldn't find business to update!!!");
            }
        }
        [HttpDelete]
        [Route("api/Business/DeleteBusinesss/{businessId}")]
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

        [HttpPost]
        [Route("api/Business/SetPhoto")]
        public HttpResponseMessage SetPhoto([FromBody] Business_PhotosDTO newB)
        {  
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
            try
            {
                string bussinesNumber = newB.Business_number;
                string url = newB.url_photo;
              
                string query = @"insert into Business_Photos values (" +bussinesNumber+ ", '" + url + "')";
                con.Open();
                SqlCommand command = new SqlCommand(query, con);
                int res = command.ExecuteNonQuery();

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
            finally
            {
                con.Close();
            }
        }

        [HttpGet]
        [Route("api/Business/getPhoto/{id}")]
        public HttpResponseMessage getPhoto(int id)
        {
            
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
            string query = @"select *
                          from Business_Photos B
                          where B.Business_number=@id";
            SqlDataAdapter adpter = new SqlDataAdapter(query, con);
            adpter.SelectCommand.Parameters.AddWithValue("@id", id);
            DataSet ds = new DataSet();
            adpter.Fill(ds, "Business_Photos");
            DataTable dt = ds.Tables["Business_Photos"];
            return Request.CreateResponse(HttpStatusCode.OK, dt);
        }
        [HttpGet]
        [Route("api/Business/getRank")]
        public HttpResponseMessage getRate()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
            string query = @"select Business_Number,grade,ROW_NUMBER() OVER(ORDER BY grade desc) AS Business_Rank
from
 (select Business_Number,elementCalculate.Pricerate*0.2+elementCalculate.amount*0.5+elementCalculate.rate*0.2+elementCalculate.Appointmentsate*0.3+elementCalculate.amount_of_links*0.1 as grade
 from
 (select TBLprice.Business_Number,case when TBLprice.Pricerate is null then 0 else Pricerate end as priceRate,
 case when amount is null then 0 else amount end as amount,
 case when total_ave is null then 0 else total_ave end as total_ave,
  case when rate is null then 0 else rate end as rate,
   case when amountOfAppointments is null then 0 else amountOfAppointments end as amountOfAppointments,
    case when Appointmentsate is null then 0 else Appointmentsate end as Appointmentsate,
	 case when amount_of_links is null then 0 else amount_of_links end as amount_of_links
 from
 (select b2.Business_Number, AVG(avarage_Rate) as Pricerate
  from(
  select Business_Number, bt.Type_treatment_Number,b.avarage,bt.Price,case when Price<avarage then 1.0 else 0.0 end as avarage_Rate
  from Business_can_give_treatment bt inner join (SELECT Type_treatment_Number,AVG(Price) as avarage
  FROM Business_can_give_treatment
  group by Type_treatment_Number) as b on b.Type_treatment_Number=bt.Type_treatment_Number 
) as b2
  group by b2.Business_Number
) as TBLprice left join (
select Business_Number,amount,total_ave, case when amount>total_ave then 1.0 else 0.0 end as rate
		 from ( SELECT Business_Number,COUNT(*)*1.0 as amount,1 as jion_col
          FROM Review_Business
          GROUP BY Business_Number) as a
		  inner join
		  (select AVG(amount)as total_ave,1 as jion_col
		  from
		(  SELECT Business_Number,COUNT(*)*1.0 as amount
          FROM Review_Business
          GROUP BY Business_Number)as b) as b2 on b2.jion_col=a.jion_col) as TBLphotos on TBLphotos.Business_Number=TBLprice.Business_Number 
		  left join(
select Business_Number,amountOfAppointments,case when a.amountOfAppointments>0 and a.amountOfAppointments<=10 then 0.2
   when a.amountOfAppointments>10 and a.amountOfAppointments<=25 then 0.4
   when  a.amountOfAppointments>25 and a.amountOfAppointments<=35 then 0.6
      when  a.amountOfAppointments>35 and a.amountOfAppointments<=45 then 0.8
	     when  a.amountOfAppointments>45 then 1.0 end as Appointmentsate
  from
(select Business_Number,count(*)*1.0 as amountOfAppointments
  from Appointment
WHERE Date >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 3, 0)
AND Date < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0) 
group by business_Number
  ) as a) as TBLappointment on TBLappointment.Business_Number=TBLphotos.Business_Number left join (
   SELECT 
  Business_Number,
  CASE 
    WHEN Facebook_link IS NOT NULL AND Instagram_link IS NOT NULL THEN 1.0
    WHEN Instagram_link IS NOT NULL OR Facebook_link IS NOT NULL THEN 0.5 
    ELSE 0.0 
  END AS amount_of_links
FROM Business) as TBLlinks on TBLlinks.Business_Number=TBLappointment.Business_Number) as elementCalculate) as TBLgrade";
            SqlDataAdapter adpter = new SqlDataAdapter(query, con);
            //adpter.SelectCommand.Parameters.AddWithValue("@id", id);
            DataSet ds = new DataSet();
            adpter.Fill(ds, "Business");
            DataTable dt = ds.Tables["Business"];
            return Request.CreateResponse(HttpStatusCode.OK, dt);
        }
        [HttpGet]
        [Route("api/Business/getRankByToday")]
        public HttpResponseMessage getRateOfToday ()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
            string query = @"select distinct Business_Number,bd.Date,grade,ROW_NUMBER() OVER(ORDER BY grade desc) AS Business_Rank
from
 (select Business_Number,elementCalculate.Pricerate*0.2+elementCalculate.amount*0.5+elementCalculate.rate*0.2+elementCalculate.Appointmentsate*0.3+elementCalculate.amount_of_links*0.1 as grade
 from
 (select TBLprice.Business_Number,case when TBLprice.Pricerate is null then 0 else Pricerate end as priceRate,
 case when amount is null then 0 else amount end as amount,
 case when total_ave is null then 0 else total_ave end as total_ave,
  case when rate is null then 0 else rate end as rate,
   case when amountOfAppointments is null then 0 else amountOfAppointments end as amountOfAppointments,
    case when Appointmentsate is null then 0 else Appointmentsate end as Appointmentsate,
	 case when amount_of_links is null then 0 else amount_of_links end as amount_of_links
 from
 (select b2.Business_Number, AVG(avarage_Rate) as Pricerate
  from(
  select Business_Number, bt.Type_treatment_Number,b.avarage,bt.Price,case when Price<avarage then 1.0 else 0.0 end as avarage_Rate
  from Business_can_give_treatment bt inner join (SELECT Type_treatment_Number,AVG(Price) as avarage
  FROM Business_can_give_treatment
  group by Type_treatment_Number) as b on b.Type_treatment_Number=bt.Type_treatment_Number 
) as b2
  group by b2.Business_Number
) as TBLprice left join (
select Business_Number,amount,total_ave, case when amount>total_ave then 1.0 else 0.0 end as rate
		 from ( SELECT Business_Number,COUNT(*)*1.0 as amount,1 as jion_col
          FROM Review_Business
          GROUP BY Business_Number) as a
		  inner join
		  (select AVG(amount)as total_ave,1 as jion_col
		  from
		(  SELECT Business_Number,COUNT(*)*1.0 as amount
          FROM Review_Business
          GROUP BY Business_Number)as b) as b2 on b2.jion_col=a.jion_col) as TBLphotos on TBLphotos.Business_Number=TBLprice.Business_Number 
		  left join(
select Business_Number,amountOfAppointments,case when a.amountOfAppointments>0 and a.amountOfAppointments<=10 then 0.2
   when a.amountOfAppointments>10 and a.amountOfAppointments<=25 then 0.4
   when  a.amountOfAppointments>25 and a.amountOfAppointments<=35 then 0.6
      when  a.amountOfAppointments>35 and a.amountOfAppointments<=45 then 0.8
	     when  a.amountOfAppointments>45 then 1.0 end as Appointmentsate
  from
(select Business_Number,count(*)*1.0 as amountOfAppointments
  from Appointment
WHERE Date >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) - 3, 0)
AND Date < DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0) 
group by business_Number
  ) as a) as TBLappointment on TBLappointment.Business_Number=TBLphotos.Business_Number left join (
   SELECT 
  Business_Number,
  CASE 
    WHEN Facebook_link IS NOT NULL AND Instagram_link IS NOT NULL THEN 1.0
    WHEN Instagram_link IS NOT NULL OR Facebook_link IS NOT NULL THEN 0.5 
    ELSE 0.0 
  END AS amount_of_links
FROM Business) as TBLlinks on TBLlinks.Business_Number=TBLappointment.Business_Number) as elementCalculate) as TBLgrade
inner join BusinessDiary bd on bd.Business_id=TBLgrade.Business_Number
WHERE bd.Date = CONVERT(date, GETDATE()) and bd.Date < DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()) + 1, 0)";
            SqlDataAdapter adpter = new SqlDataAdapter(query, con);
            //adpter.SelectCommand.Parameters.AddWithValue("@id", id);
            DataSet ds = new DataSet();
            adpter.Fill(ds, "Business");
            DataTable dt = ds.Tables["Business"];
            return Request.CreateResponse(HttpStatusCode.OK, dt);
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
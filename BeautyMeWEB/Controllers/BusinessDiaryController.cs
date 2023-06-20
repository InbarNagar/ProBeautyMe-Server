using BeautyMeWEB.DTO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BeautyMeWEB.Controllers
{
    public class BusinessDiaryController : ApiController
    { // פונקציה שמביאה פרטים רלוונטיים אבל בלי פרטי עסק
        //[HttpGet]
        //[Route("api/BusinessDiary/{business_number}")]
        //public HttpResponseMessage GetBusinessDiary(int business_number)
        //{
        //    int BusinessNumber = business_number;
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
        //    string query = @"select a.*,b.*,bd.*
        //                    from Appointment  a inner join Business_can_give_treatment b on a.Business_Number=b.Business_Number inner join BusinessDiary bd on b.Business_Number=bd.Business_id
        //                    where b.Business_Number=@BusinessNumber and a.Date>=GETDATE()
        //                    order by b.Business_Number,bd.Date,bd.Start_time,b.Type_treatment_Number,a.Appointment_status";

        //    SqlDataAdapter adpter = new SqlDataAdapter(query, con);
        //    adpter.SelectCommand.Parameters.AddWithValue("@BusinessNumber", BusinessNumber);
        //    DataSet ds = new DataSet();
        //    adpter.Fill(ds, "BusinessDiary");
        //    DataTable dt = ds.Tables["BusinessDiary"];
        //    return Request.CreateResponse(HttpStatusCode.OK, dt);
        //}
        //פרטי יומן של עסק
        [HttpGet]
        [Route("api/BusinessDiary/{business_number}")]
        public HttpResponseMessage GetBusinessDiary(int business_number) //כולל פרטי עסק
        {
            int BusinessNumber = business_number;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
            string query = @"select a.*,b.*,bd.*,bus.*
                            from Appointment  a inner join Business_can_give_treatment b on a.Business_Number=b.Business_Number inner join BusinessDiary bd on b.Business_Number=bd.Business_id
                            inner join Business bus on bus.Business_Number=b.Business_Number                            
                            where b.Business_Number=@BusinessNumber and a.Date>=GETDATE()
                            order by b.Business_Number,bd.Date,bd.Start_time,b.Type_treatment_Number,a.Appointment_status";

            SqlDataAdapter adpter = new SqlDataAdapter(query, con);
            adpter.SelectCommand.Parameters.AddWithValue("@BusinessNumber", BusinessNumber);
            DataSet ds = new DataSet();
            adpter.Fill(ds, "BusinessDiary");
            DataTable dt = ds.Tables["BusinessDiary"];
            return Request.CreateResponse(HttpStatusCode.OK, dt);
        }

        [HttpPost]
        [Route("api/BusinessDiary/GetAllBusinessDiaryBy_Status_City_TreatmentNumber")]
        public HttpResponseMessage GetAllBusinessDiaryBy_Status_City_TreatmentNumber([FromBody] SearchDTO s)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
            string query = @"select a.*,b.*,bd.*,bus.*
                            from Appointment  a inner join Business_can_give_treatment b on a.Business_Number=b.Business_Number
                            inner join BusinessDiary bd on b.Business_Number=bd.Business_id
                            inner join Business bus on bus.Business_Number=b.Business_Number
                            where a.Appointment_status=@status and bus.AddressCity=@AddressCity and b.Type_treatment_Number=@TreatmentNumber and a.Date>=GETDATE()
                            order by b.Business_Number,bd.Date,bd.Start_time,b.Type_treatment_Number,a.Appointment_status";

            SqlDataAdapter adpter = new SqlDataAdapter(query, con);

            adpter.SelectCommand.Parameters.AddWithValue("@status", "confirmed");
            adpter.SelectCommand.Parameters.AddWithValue("@AddressCity", s.AddressCity);
            adpter.SelectCommand.Parameters.AddWithValue("@TreatmentNumber", s.TreatmentNumber);


            DataSet ds = new DataSet();
            adpter.Fill(ds, "BusinessDiary");
            DataTable dt = ds.Tables["BusinessDiary"];
            return Request.CreateResponse(HttpStatusCode.OK, dt);
        }
        //מביא לפי סוג טיפול ללא עיר 
        //מביא רק את התאריך 2023-06-02
        //וגם את מה שמופיע בסטטוס כנקבע או מחכה לאישור
        [HttpPost]
        [Route("api/BusinessDiary/GetAllBusinessDiaryByTreatmentNumber")]
        public HttpResponseMessage GetAllBusinessDiaryByCity_TreatmentNumber([FromBody] SearchDTO s)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
            string query = @"   SELECT a.*, b.*, bd.*, bus.*
                                FROM Appointment a
                                LEFT JOIN Business_can_give_treatment b ON a.Business_Number = b.Business_Number
                                INNER JOIN BusinessDiary bd ON b.Business_Number = bd.Business_id
                                INNER JOIN Business bus ON bus.Business_Number = b.Business_Number
                                WHERE b.Type_treatment_Number = @TreatmentNumber
                                AND a.Date >= '2023-06-02'
                                AND a.Date < '2023-06-03'
                                AND (a.Appointment_status = 'confirmed' OR a.Appointment_status = 'Awaiting_approval')
                                ORDER BY a.Number_appointment, b.Business_Number, bd.Date, bd.Start_time, b.Type_treatment_Number, a.Appointment_status";

            SqlDataAdapter adpter = new SqlDataAdapter(query, con);

            //adpter.SelectCommand.Parameters.AddWithValue("@AddressCity", s.AddressCity);
            adpter.SelectCommand.Parameters.AddWithValue("@TreatmentNumber", s.TreatmentNumber);


            DataSet ds = new DataSet();
            adpter.Fill(ds, "BusinessDiary");
            DataTable dt = ds.Tables["BusinessDiary"];
            return Request.CreateResponse(HttpStatusCode.OK, dt);
        }
    }
}

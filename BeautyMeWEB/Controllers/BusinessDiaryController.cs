using BeautyMe;
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
    {
        BeautyMeDBContext db = new BeautyMeDBContext();
        // פונקציה שמביאה פרטים רלוונטיים אבל בלי פרטי עסק
        //[HttpGet]
        //[Route("api/BusinessDiary/{business_number}")]
        //public HttpResponseMessage GetBusinessDiary(int business_number)
        //{
        //    int BusinessNumber = business_number;
        //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
        //    string query = @"select a.,b.,bd.*
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

        //הוספה ליומן
        [HttpPost]
        [Route("api/BusinessDiary/AddNewOption")]
        public HttpResponseMessage AddNewOption([FromBody] BusinessDiaryDTO b)
        {
            if (b != null)
            {
                BusinessDiary bd = new BusinessDiary()
                {
                    Business_id = b.Business_id,
                    Date = b.Date,
                    Start_time = b.Start_time,
                    End_time = b.End_time
                };
                db.BusinessDiary.Add(bd);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"You have added availability to your diary in {bd.Date} from {bd.Start_time} to {bd.End_time}");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

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

        [HttpGet]
        [Route("api/BusinessDiary/BusinessDiaryForPro/{Business_id}")] //of today (כרגע מביא את ה 2.6.2023 כי יש שם מידע)
        public HttpResponseMessage BusinessDiaryForPro(int Business_id)
        {
            if (Business_id > 0)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
                string query = @"select a.*,b.*,c.*
                            from Appointment  a inner join BusinessDiary b on a.Business_Number=b.Business_id inner join Client c on a.ID_Client=c.ID_number
                            where a.Business_Number=@Business_Number 
                            AND a.Date >= '2023-06-02'
                            AND a.Date < '2023-06-03'
                            order by a.Date,a.Start_Hour,a.Appointment_status";

                SqlDataAdapter adpter = new SqlDataAdapter(query, con);
                adpter.SelectCommand.Parameters.AddWithValue("@Business_Number", Business_id);

                DataSet ds = new DataSet();
                adpter.Fill(ds, "BusinessDiary");
                DataTable dt = ds.Tables["BusinessDiary"];
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, Business_id);

            }
        }
        [HttpGet]
        [Route("api/BusinessDiary/AllBusinessDiaryForPro/{Business_id}")] //of today (כרגע מביא את ה 2.6.2023 כי יש שם מידע)
        public HttpResponseMessage AllBusinessDiaryForPro(int Business_id)
        {
            if (Business_id > 0)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
                string query = @"select a.*,b.*,c.*
                            from Appointment  a inner join BusinessDiary b on a.Business_Number=b.Business_id inner join Client c on a.ID_Client=c.ID_number
                            where a.Business_Number=@Business_Number 
                            AND a.Date >= '2023-06-02'
                            AND a.Date < '2023-06-03'
                            order by a.Date,a.Start_Hour,a.Appointment_status";

                SqlDataAdapter adpter = new SqlDataAdapter(query, con);
                adpter.SelectCommand.Parameters.AddWithValue("@Business_Number", Business_id);

                DataSet ds = new DataSet();
                adpter.Fill(ds, "BusinessDiary");
                DataTable dt = ds.Tables["BusinessDiary"];
                return Request.CreateResponse(HttpStatusCode.OK, dt);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, Business_id);

            }
        }
        //מביא לפי סוג טיפול ללא עיר 
        //מביא רק את התאריך 2023-06-02
        //וגם את מה שמופיע בסטטוס כנקבע או מחכה לאישור
        [HttpPost]
        [Route("api/BusinessDiary/GetAllBusinessDiaryByCity_TreatmentNumber")]
        public HttpResponseMessage GetAllBusinessDiaryByCity_TreatmentNumber([FromBody] SearchDTO s)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);

            //עיר ללא בית לקוח        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BeautyMeDB"].ConnectionString);
            string query = @"   SELECT a.*, b.*, bd.*, bus.*
                                FROM Appointment a
                                LEFT JOIN Business_can_give_treatment b ON a.Business_Number = b.Business_Number
                                INNER JOIN BusinessDiary bd ON b.Business_Number = bd.Business_id
                                INNER JOIN Business bus ON bus.Business_Number = b.Business_Number
                                WHERE b.Type_treatment_Number = @TreatmentNumber";
            if (s.AddressCity != null)
            {
                query += " AND bus.AddressCity=@AddressCity ";
            }
            if (s.Is_client_house != null)
            {
                query += "  AND bus.Is_client_house=@Is_client_house ";
            }
            query += @" AND a.Date >= '2023-06-02'
                                AND a.Date < '2023-06-03'
                                AND (a.Appointment_status = 'confirmed' OR a.Appointment_status = 'Awaiting_approval')
                                ORDER BY  b.Business_Number,a.Number_appointment, bd.Date, bd.Start_time, b.Type_treatment_Number, a.Appointment_status";

            SqlDataAdapter adpter = new SqlDataAdapter(query, con);
            if (s.AddressCity != null)
            {
                adpter.SelectCommand.Parameters.AddWithValue("@AddressCity", s.AddressCity);
            }
            if (s.Is_client_house != null)
            {
                adpter.SelectCommand.Parameters.AddWithValue("@Is_client_house", s.Is_client_house);
            }
            adpter.SelectCommand.Parameters.AddWithValue("@TreatmentNumber", s.TreatmentNumber);


            DataSet ds = new DataSet();
            adpter.Fill(ds, "BusinessDiary");
            DataTable dt = ds.Tables["BusinessDiary"];
            return Request.CreateResponse(HttpStatusCode.OK, dt);


        }
    }
}
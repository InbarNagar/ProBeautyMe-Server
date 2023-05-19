//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BeautyMe
//{
//    [MetadataType()]
//    public partial class Client
//    {
//    }

//    public class ClientMetaData
//    {
//        //public static bool IsIdNumberUnique(string idNumber)
//        //{

//        //
//        db = new BeautyMeDBContext();

//        //    string id_number = db.Client.FirstOrDefault(i => i.ID_number == idNumber).ID_number;
//        //    if (id_number == null) // אם אין ת.ז כזה בטבלת לקוחות ייחזור TRUE
//        //    {
//        //        return true;
//        //    }
//        //    return false;
//        //}
//        [Required(ErrorMessage = "ID number is required")]
//        [RegularExpression(@"^\d{9}$", ErrorMessage = "ID number should be composed of 9 digits")]
//        public string ID_number;
//        //{
//        //    set {
//        //            if (IsIdNumberUnique(ID_number) == true)
//        //            {
//        //                 value;
//        //            }
//        //            else
//        //            {
//        //                throw new Exception("ID number already exists in database");
//        //            }
//        //        }
//        //    }
//        //}
//        public string First_name;
//        public string Last_name;
//        [Range(18, int.MaxValue, ErrorMessage = "Age must be over 18")]
//        public int Age
//        {
//            get
//            {
//                DateTime today = DateTime.Today;
//                int age = today.Year - birth_date.Year;
//                if (birth_date > today.AddYears(-age))
//                    age--;
//                return age;
//            }
//        }
//        public System.DateTime birth_date;
//        public string gender;
//        public string phone;
//        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]
//        public string Email;
//        public string AddressStreet;
//        public string AddressHouseNumber;
//        public string AddressCity;
//        public string password;

//    }
//}



//פונקציות שהורדתי מכל מיני מקומות:
//// Post: api/Post
//        [HttpPost]
//[Route("api/Future_Appointment/NewFuture_Appointment")]
//public HttpResponseMessage PostNewFuture_Appointment([FromBody] Future_AppointmentDTO x)
//{
//    try
//    {
//        BeautyMeDBContext db = new BeautyMeDBContext();

//        Future_Appointment newFuture_Appointment = new Future_Appointment()
//        {
//            //Future_appointment_number = x.Future_appointment_number,
//            AddressStreet = x.AddressStreet,
//            AddressHouseNumber = x.AddressHouseNumber,
//            AddressCity = x.AddressCity,
//            Appointment_status = x.Appointment_status,
//            Client_ID_number = x.Client_ID_number,
//            Type_treatment_Number = x.Type_treatment_Number,
//            Number_appointment = x.Number_appointment
//        };
//        db.Future_Appointment.Add(newFuture_Appointment);
//        db.SaveChanges();
//        return Request.CreateResponse(HttpStatusCode.OK, "new Future_Appointment added to the dataBase");
//    }
//    catch (DbUpdateException ex)
//    {
//        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred while adding new Future_Appointment to the database: " + ex.InnerException.InnerException.Message);
//    }
//}


//// Delete: api/Delete
//[HttpDelete]
//[Route("api/Future_Appointment/CanceleFuture_Appointment")]
//public IHttpActionResult DeleteCanceleFuture_Appointment([FromBody] Future_AppointmentDTO x)
//{
//    BeautyMeDBContext db = new BeautyMeDBContext();
//    {
//        if (x == null)  // בדיקת תקינות ה-DTO שהתקבל
//        {
//            return BadRequest("הפרטים שהתקבלו אינם תקינים.");
//        }

//        Future_Appointment CanceleFuture_Appointment = db.Future_Appointment.Find(x.Future_appointment_number);   // חיפוש הרשומה המתאימה לפי המזהה שלה
//        if (CanceleFuture_Appointment == null)
//        {
//            return NotFound();
//        }

//        if (CanceleFuture_Appointment.Appointment_status == "Appointment_ended" || CanceleFuture_Appointment.Appointment_status == "Cancelled")
//        {
//            return BadRequest("לא ניתן לבטל את התור מכיוון שעבר זמן אפשרות הביטול");
//        }
//        else
//        {
//            CanceleFuture_Appointment = db.Future_Appointment.Find(x.Future_appointment_number);   // חיפוש הרשומה המתאימה לפי המזהה שלה
//                                                                                                   //db.Entry(CanceleFuture_Appointment).State = EntityState.Detached;
//                                                                                                   //db.Future_Appointment.Remove(CanceleFuture_Appointment);
//            db.Future_Appointment.Remove(CanceleFuture_Appointment);   // מחיקת הרשומה מבסיס הנתונים
//            db.Appointment.Add(CanceleFuture_Appointment.Appointment); //הוספת התור שבוטל לטבלת תורים פנויים (טבלת תור)
//            db.SaveChanges();
//        }
//        return Ok("הנתונים נמחקו בהצלחה.");  // החזרת תשובה מתאימה לפי המצב
//    }



//    // Post: api/Post
//    [HttpPost]
//    [Route("api/Future_Appointment/NewFuture_Appointment")]
//    public HttpResponseMessage PostNewFuture_Appointment([FromBody] Future_AppointmentDTO x)
//    {
//        try
//        {
//            BeautyMeDBContext db = new BeautyMeDBContext();
//            //Future_Appointment newFuture_Appointment = db.Future_Appointment.Find(x.Number_appointment);
//            Appointment theScheduledAppointment = db.Appointment.Find(x.Number_appointment);
//            if (theScheduledAppointment != null)
//            {
//                Future_Appointment newFuture_Appointment = new Future_Appointment()
//                {
//                    AddressStreet = x.AddressStreet,
//                    AddressHouseNumber = x.AddressHouseNumber,
//                    AddressCity = x.AddressCity,
//                    Appointment_status = x.Appointment_status,
//                    Client_ID_number = x.Client_ID_number,
//                    Type_treatment_Number = x.Type_treatment_Number,
//                    Number_appointment = x.Number_appointment
//                };
//                db.Future_Appointment.Add(newFuture_Appointment);

//                //Appointment theScheduledAppointment = db.Appointment.FirstOrDefault(a => a.Number_appointment == x.Number_appointment);
//                //Appointment theScheduledAppointmentThatNeedToDeleteFromAppointmentTable = new Appointment
//                //{
//                //    Date = theScheduledAppointment.Date,
//                //    Start_time = theScheduledAppointment.Start_time,
//                //    End_time = theScheduledAppointment.End_time,
//                //    Is_client_house = theScheduledAppointment.Is_client_house,
//                //    Business_Number = theScheduledAppointment.Business_Number,
//                //};
//                db.Appointment.Remove(theScheduledAppointment); //הוספת התור שבוטל לטבלת תורים פנויים (טבלת תור) התור שחוזר לטבלת התורים הפנויים חוזר בתור תור חדש ומקבל מספר תור חדש!!!

//                //AppointmentController.delete
//                //db.Appointment.Remove(newFuture_Appointment.Appointment);
//                db.SaveChanges();
//                return Request.CreateResponse(HttpStatusCode.OK, "new Future_Appointment added to the dataBase");
//                ////Future_appointment_number = x.Future_appointment_number,
//                //newFuture_Appointment.AddressStreet = x.AddressStreet;
//                //newFuture_Appointment.AddressHouseNumber = x.AddressHouseNumber;
//                //newFuture_Appointment.AddressCity = x.AddressCity;
//                //newFuture_Appointment.Appointment_status = x.Appointment_status;
//                //newFuture_Appointment.Client_ID_number = x.Client_ID_number;
//                //newFuture_Appointment.Type_treatment_Number = x.Type_treatment_Number;
//                //newFuture_Appointment.Number_appointment = x.Number_appointment;
//            }
//            else
//            {
//                return Request.CreateResponse(HttpStatusCode.NotFound, $"this Appointment not found any more"); //אם יש כבר תור עתידי לתור הזה.

//            }
//        }
//        catch (DbUpdateException ex)
//        {
//            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred while adding new Future_Appointment to the database: " + ex.InnerException.InnerException.Message);
//        }
//    }

//    // זאת פונקציית המחיקה של תור עתידי שמוחק את התור העתידי ומוסיך את התור שנמחק לטבלת "תור" בתור תור חדש... הפונקציה עובדת! אבל אחרי זה הבנתי שלא ככה צריך לעשות 
//    // Delete: api/Delete
//    [HttpDelete]
//    [Route("api/Future_Appointment/CanceleFuture_Appointment")]
//    public IHttpActionResult DeleteCanceleFuture_Appointment([FromBody] Future_AppointmentDTO x)
//    {
//        BeautyMeDBContext db = new BeautyMeDBContext();
//        {
//            if (x == null)  // בדיקת תקינות ה-DTO שהתקבל
//            {
//                return BadRequest("הפרטים שהתקבלו אינם תקינים.");
//            }

//            Future_Appointment CanceleFuture_Appointment = db.Future_Appointment.Find(x.Future_appointment_number);   // חיפוש הרשומה המתאימה לפי המזהה שלה
//            if (CanceleFuture_Appointment == null)
//            {
//                return NotFound();
//            }

//            if (CanceleFuture_Appointment.Appointment_status == "Appointment_ended" || CanceleFuture_Appointment.Appointment_status == "Cancelled")
//            {
//                return BadRequest("לא ניתן לבטל את התור מכיוון שעבר זמן אפשרות הביטול");
//            }
//            else
//            {

//                db.Future_Appointment.Remove(CanceleFuture_Appointment);   // מחיקת הרשומה מבסיס הנתונים
//                Appointment theAppointmentThatCanceled = db.Appointment.FirstOrDefault(a => a.Number_appointment == x.Number_appointment);
//                Appointment appointmentToAdd = new Appointment
//                {
//                    Date = theAppointmentThatCanceled.Date,
//                    Start_time = theAppointmentThatCanceled.Start_time,
//                    End_time = theAppointmentThatCanceled.End_time,
//                    Is_client_house = theAppointmentThatCanceled.Is_client_house,
//                    Business_Number = theAppointmentThatCanceled.Business_Number,
//                };
//                db.Appointment.Add(appointmentToAdd); //הוספת התור שבוטל לטבלת תורים פנויים (טבלת תור) התור שחוזר לטבלת התורים הפנויים חוזר בתור תור חדש ומקבל מספר תור חדש!!!

//                db.SaveChanges();
//            }
//            return Ok("הנתונים נמחקו בהצלחה.");  // החזרת תשובה מתאימה לפי המצב
//        }




//// Put: api/Put
//[HttpPut]
//[Route("api/Appointment/UpdateAppointment")]
//public HttpResponseMessage PutUpdateAppointment([FromBody] AppointmentDTO x)
//{
//    BeautyMeDBContext db = new BeautyMeDBContext();
//    Appointment AppointmentToUpdate = db.Appointment.FirstOrDefault(a => a.Number_appointment == x.Number_appointment);
//    if (AppointmentToUpdate == null)
//    {
//        return Request.CreateResponse(HttpStatusCode.NotFound, $"Appointment with number {x.Number_appointment} not found.");
//    }

//    else
//    {
//        //AppointmentToUpdate.Number_appointment = x.Number_appointment;
//        AppointmentToUpdate.Date = x.Date;
//        AppointmentToUpdate.Start_time = x.Start_time;
//        AppointmentToUpdate.End_time = x.End_time;
//        AppointmentToUpdate.Is_client_house = x.Is_client_house;
//        AppointmentToUpdate.Business_Number = x.Business_Number;
//        db.Appointment.Add(AppointmentToUpdate);
//        db.SaveChanges();
//        return Request.CreateResponse(HttpStatusCode.OK, "The Appointment update in the dataBase");
//    }
//}


//// Post: api/Post
//[HttpPost]
//[Route("api/Future_Appointment/NewFuture_Appointment/{Client_ID_number}/{Type_treatment_Number}/{Number_appointmentt}")]
//public HttpResponseMessage PostNewFuture_Appointment(string Client_ID_numberr, string Type_treatment_Numberr, string Number_appointmentt, [FromBody] Future_AppointmentDTO x)
//{
//    BeautyMeDBContext db = new BeautyMeDBContext();
//    Future_Appointment newFuture_Appointment = new Future_Appointment()
//    {
//        //Future_appointment_number = x.Future_appointment_number,
//        AddressStreet = x.AddressStreet,
//        AddressHouseNumber = x.AddressHouseNumber,
//        AddressCity = x.AddressCity,
//        Appointment_status = x.Appointment_status,
//        Client_ID_number = x.Client_ID_number,
//        Type_treatment_Number = x.Type_treatment_Number,
//        Number_appointment = x.Number_appointment
//    };
//    if (newFuture_Appointment != null)
//    {
//        db.Future_Appointment.Add(newFuture_Appointment);
//        db.SaveChanges();
//        return Request.CreateResponse(HttpStatusCode.OK, "new Future_Appointment added to the dataBase");
//    }
//    else
//        return Request.CreateResponse(HttpStatusCode.NoContent);
//}



////public string NameTreatment;
////public string AddressCity;
////public string? gender; 
////public string? Is_client_house;
///


//    BeautyMeDBContext db = new BeautyMeDBContext();
//    string number_treatment;
//    //string number_treatment = db.Type_Treatment.Type_treatment_Number.Where(i => i.Name == NameTreatment).single....;
//    string queryForAppointment = "select *" +
//                                    "from Appointment a join Appointment_can_give_treatment ac on a.Number_appointment = ac.Number_appointment" +
//                                    "join Business b on a.Business_Number = b.Business_Number" +
//                                    $"where b.AddressCity = {x.AddressCity} and ac.Type_treatment_Number = {number_treatment}";
//    // הגדרת פרמטרים עבור השאילתה
//    SqlCommand command = new SqlCommand(queryForAppointment, db);
//    //command.Parameters.AddWithValue("@City", city);
//    //command.Parameters.AddWithValue("@TreatmentType", treatmentType);

//    // קריאה למסד הנתונים וקבלת התוצאות
//    SqlDataReader reader = command.ExecuteReader();

//    // עבודה עם התוצאות ויצירת רשימת התורים המתאימים
//    while (reader.Read())
//    {
//        Treatment appointment = new Treatment();
//        appointment.ID = Convert.ToInt32(reader["מספר_תור"]);
//        appointment.Date = Convert.ToDateTime(reader["תאריך_תור"]);
//        appointment.City = reader["עיר"].ToString();
//        appointment.TreatmentType = reader["שם_סוג_טיפול"].ToString();

//        appointments.Add(appointment);
//    }
//    if (x.gender == null && x.Is_client_house == null)
//    {
//        AppointmentDTO SearchListForType_treatment = db.Treatment_for_appointment.Where(a => a.Type_treatment_Number == number_treatment) && db.Business.Where(i => i.AddressCity == x.AddressCity)
//            .Select(x => new AppointmentDTO
//            {
//                Number_appointment = x.Number_appointment,
//                Date = x.Date,
//                Start_time = x.Start_time,
//                End_time = x.End_time,
//                Is_client_house = x.Is_client_house,
//                Business_Number = x.Business_Number,
//            }).ToList(); // נוצרה רשימה של תורים לפי הסוג טיפול הרצוי.
//    }
//    AppointmentDTO SearchListForType_treatment = db.Treatment_for_appointment.Where(a => a.Type_treatment_Number == number_treatment).Select(x => new AppointmentDTO
//    {
//        Number_appointment = x.Number_appointment,
//        Date = x.Date,
//        Start_time = x.Start_time,
//        End_time = x.End_time,
//        Is_client_house = x.Is_client_house,
//        Business_Number = x.Business_Number,
//    }).ToList(); // נוצרה רשימה של תורים לפי הסוג טיפול הרצוי.

//    AppointmentDTO SearchListForCity =
//        return Request.CreateResponse(HttpStatusCode.OK, c1);
//    else
//        return Request.CreateResponse(HttpStatusCode.NotFound);
//}



//    BeautyMeDBContext db = new BeautyMeDBContext();
//    string number_treatment;
//    //string number_treatment = db.Type_Treatment.Type_treatment_Number.Where(i => i.Name == NameTreatment).single....;
//    if (x.gender == null && x.Is_client_house == null)
//    {
//        AppointmentDTO SearchListForType_treatment = db.Treatment_for_appointment.Where(a => a.Type_treatment_Number == number_treatment) && db.Business.Where(i => i.AddressCity == x.AddressCity)
//            .Select(x => new AppointmentDTO
//        {
//            Number_appointment = x.Number_appointment,
//            Date = x.Date,
//            Start_time = x.Start_time,
//            End_time = x.End_time,
//            Is_client_house = x.Is_client_house,
//            Business_Number = x.Business_Number,
//        }).ToList(); // נוצרה רשימה של תורים לפי הסוג טיפול הרצוי.
//    }
//    AppointmentDTO SearchListForType_treatment = db.Treatment_for_appointment.Where(a => a.Type_treatment_Number == number_treatment).Select(x => new AppointmentDTO
//    {
//        Number_appointment = x.Number_appointment,
//        Date = x.Date,
//        Start_time = x.Start_time,
//        End_time = x.End_time,
//        Is_client_house = x.Is_client_house,
//        Business_Number = x.Business_Number,
//    }).ToList(); // נוצרה רשימה של תורים לפי הסוג טיפול הרצוי.

//    AppointmentDTO SearchListForCity = 
//        return Request.CreateResponse(HttpStatusCode.OK, c1);
//    else
//        return Request.CreateResponse(HttpStatusCode.NotFound);
//}


//    // GET: Search
//    [HttpGet]
//    [Route("api/Search/{NameTreatment}")]
//    public HttpResponseMessage Get(string NameTreatment, [FromBody]string bussinesCity)
//    {
//        BeautyMeDBContext db = new BeautyMeDBContext();
//        string number_treatment;
//        //string number_treatment = db.Type_Treatment.Type_treatment_Number.Where(i => i.Name == NameTreatment).single....;
//        SearchDTO SearchList = db.Treatment_for_appointment.Where(x => x.Type_treatment_Number == number_treatment).Select(a => new ClientDTO
//        {
//            password = a.password,
//            ID_number = a.ID_number,
//            Last_name = a.Last_name,
//            First_name = a.First_name
//        }).FirstOrDefault();
//        if (c1 != null)
//            return Request.CreateResponse(HttpStatusCode.OK, c1);
//        else
//            return Request.CreateResponse(HttpStatusCode.NotFound);
//}




//// Post: api/Post
//[HttpPost]
//[Route("api/Professional/NewProfessional")]
//public HttpResponseMessage PostNewProfessional([FromBody] Professional value)
//{
//    BeautyMeDBContext db = new BeautyMeDBContext();
//    Professional newProfessional = new Professional();
//    newProfessional = value;
//    if (newProfessional != null)
//    {
//        db.Professional.Add(newProfessional);
//        db.SaveChanges();
//        return Request.CreateResponse(HttpStatusCode.OK, "new Professional added to the dataBase");
//    }
//    else
//        return Request.CreateResponse(HttpStatusCode.NoContent);
//}


//// Post: api/Post
//[HttpPost]
//[Route("api/Business_can_give_treatment/PostNewTreatmentOfBussines/{Business_Number}/{Type_treatment_Number}/{Category_Number}")]
//public HttpResponseMessage PostNewTreatmentOfBussines(string Business_Numberr, string Type_treatment_Numberr, string Category_Numberr, [FromBody] Business_can_give_treatmentDTO x)
//{
//    BeautyMeDBContext db = new BeautyMeDBContext();
//    Business_can_give_treatment newBusiness = new Business_can_give_treatment()
//    {
//        Type_treatment_Number = x.Type_treatment_Number,
//        Category_Number = x.Category_Number,
//        Business_Number = x.Business_Number,
//        Price = x.Price,
//        Treatment_duration = x.Treatment_duration,
//    };
//    if (newBusiness != null)
//    {
//        db.Business_can_give_treatment.Add(newBusiness);
//        db.SaveChanges();
//        return Request.CreateResponse(HttpStatusCode.OK, "new Business added to the dataBase");
//    }
//    else
//        return Request.CreateResponse(HttpStatusCode.NoContent);
//}
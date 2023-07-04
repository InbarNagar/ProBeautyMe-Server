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
    public class ProfessionalController : ApiController
    {
        BeautyMeDBContext db = new BeautyMeDBContext();
        // GET: Professional
        [HttpGet]
        [Route("api/Professional/AllProfessional")]
        public HttpResponseMessage GetAllProfessional()
        {
            List<ProfessionalDTO> AllProfessionals = db.Professional.Select(x => new ProfessionalDTO
            {
                ID_number = x.ID_number,
                First_name = x.First_name,
                Last_name = x.Last_name,
                birth_date = x.birth_date,
                gender = x.gender,
                phone = x.phone,
                Email = x.Email,
                AddressStreet = x.AddressStreet,
                AddressHouseNumber = x.AddressHouseNumber,
                AddressCity = x.AddressCity,
                password = x.password
            }).ToList();
            if (AllProfessionals != null)
                return Request.CreateResponse(HttpStatusCode.OK, AllProfessionals);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }
        // Post: api/Professional/OneProfessional פונקציה שמקבלת ת.ז וסיסמה ומחזירה את המשתמש
        [HttpPost]
        [Route("api/Professional/OneProfessional")]
        public HttpResponseMessage GetOneProfessional([FromBody] SearchPeopleDTO v)
        {
            ProfessionalDTO oneProfessional = db.Professional.Where(a => a.ID_number == v.id_number && a.password == v.password).Select(x => new ProfessionalDTO
            {
                ID_number = x.ID_number,
                First_name = x.First_name,
                Last_name = x.Last_name,
                birth_date = x.birth_date,
                gender = x.gender,
                phone = x.phone,
                Email = x.Email,
                AddressStreet = x.AddressStreet,
                AddressHouseNumber = x.AddressHouseNumber,
                AddressCity = x.AddressCity,
                password = x.password,
                token=x.token
                
            }).FirstOrDefault();
            if (oneProfessional != null)
            {
                BusinessDTO number = db.Business.Where(x => x.Professional_ID_number == v.id_number).Select(y => new BusinessDTO
                {
                    Business_Number = y.Business_Number
                }
                    ).FirstOrDefault();
                oneProfessional.Business_Number = number.Business_Number;

                return Request.CreateResponse(HttpStatusCode.OK, oneProfessional);

            }

            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }
        //
        //
        //

        //[HttpPost]
        //[Route("api/Professional/GetProfessional")]
        //public IHttpActionResult GetProfessional([FromBody] ProfessionalDTO user)
        //{
        //    try
        //    {
        //        var userD = db.Professionals.Where(x => x.ID_number == user.ID_number && x.password == user.password).FirstOrDefault();
        //        if (userD == null)
        //        {
        //            return NotFound();
        //        }
        //        ProfessionalDTO newUser = new ProfessionalDTO();
        //        newUser.ID_number = userD.ID_number;
        //        newUser.First_name = userD.First_name;
        //        newUser.Last_name = userD.Last_name;
        //        newUser.birth_date = userD.birth_date;
        //        newUser.gender = userD.gender;
        //        newUser.phone = userD.phone;
        //        newUser.Email = userD.Email;
        //        newUser.AddressStreet = userD.AddressStreet;
        //        newUser.AddressHouseNumber = userD.AddressHouseNumber;
        //        newUser.AddressCity = userD.AddressCity;
        //        return Ok(newUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        // Post: api/Post
        [HttpPost]
        [Route("api/Professional/NewProfessional")]
        public HttpResponseMessage PostNewProfessional([FromBody] ProfessionalDTO x)
        {
            try
            {
                Professional newProfessional = new Professional()
                {
                    ID_number = x.ID_number,
                    First_name = x.First_name,
                    Last_name = x.Last_name,
                    birth_date = x.birth_date,
                    gender = x.gender,
                    phone = x.phone,
                    Email = x.Email,
                    AddressStreet = x.AddressStreet,
                    AddressHouseNumber = x.AddressHouseNumber,
                    AddressCity = x.AddressCity,
                    password = x.password,
                    ProfilPic = x.ProfilPic,
                };
                db.Professional.Add(newProfessional);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "new Professional added to the dataBase");
            }
            catch (DbUpdateException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred while adding new Professional to the database: " + ex.InnerException.InnerException.Message);
            }
        }
        [HttpPost]
        [Route("api/Proffesional/UpdateProffesional")]
        public HttpResponseMessage UpdateProffesional([FromBody] ProfessionalDTO newP)
        {
            Professional prevF = db.Professional.Where(x => x.ID_number == newP.ID_number).FirstOrDefault();
            if (prevF != null)
            {
                prevF.Last_name = newP.Last_name;
                prevF.phone = newP.phone;
                prevF.gender = newP.gender;
                prevF.Email = newP.Email;
                prevF.AddressStreet = newP.AddressStreet;
                prevF.AddressCity = newP.AddressCity;
                prevF.AddressHouseNumber = newP.AddressHouseNumber;
                prevF.birth_date = newP.birth_date;
                prevF.First_name = newP.First_name;
                
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"Proffesional {prevF.ID_number} had been updated!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "could'nt update Proffesional");
            }
        }
        [HttpDelete]
        [Route("api/Proffesional/DeleteProffesional/{proffesionalId}")]
        public HttpResponseMessage DeleteBusiness(string proffesionalId)
        {
            Professional PtoDel = db.Professional.Where(x => x.ID_number == proffesionalId).FirstOrDefault();
            if (PtoDel != null)
            {
                db.Professional.Remove(PtoDel);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"{proffesionalId} deleted from dataBse!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "couldn't delete Proffesional from data base!");
            }



        }

        [HttpPost]
        [Route("api/Client/OneProfessionalToken/{ID_number}/{token}")]
        public HttpResponseMessage SaveTokenUser(string ID_number, string token)
        {
            Professional onepro = db.Professional.Where(a => a.ID_number == ID_number).FirstOrDefault();
            onepro.token = token;
            db.SaveChanges();
            if (onepro != null)
                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}


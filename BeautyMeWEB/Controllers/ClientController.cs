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
    public class ClientController : ApiController
    {

        BeautyMeDBContext db = new BeautyMeDBContext();
        // GET: Client
        [HttpGet]
        [Route("api/Client/AllClient")]
        public HttpResponseMessage GetAllClient()
        {
            List<ClientDTO> AllClient = db.Client.Select(x => new ClientDTO
            {
                ID_number = x.ID_number,
                First_name = x.First_name,
                Last_name = x.Last_name,
                birth_date = x.birth_date,
                phone = x.phone,
                Email = x.Email,
                AddressStreet = x.AddressStreet,
                AddressHouseNumber = x.AddressHouseNumber,
                AddressCity = x.AddressCity,
                Facebook_link = x.Facebook_link,
                Instagram_link = x.Instagram_link,
                password = x.password
            }).ToList();
            if (AllClient != null)
                return Request.CreateResponse(HttpStatusCode.OK, AllClient);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // Post: api/Client/OneClient פונקציה שמקבלת ת.ז ומחזירה את המשתמש
        [HttpPost]
        [Route("api/Client/OneClient/{ID_number}")]
        public HttpResponseMessage GetOneClient(string ID_number)
        {
            ClientDTO oneClient = db.Client.Where(a => a.ID_number == ID_number).Select(x => new ClientDTO
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
                Facebook_link = x.Facebook_link,
                Instagram_link = x.Instagram_link,
                password = x.password,
                token=x.token
            }).FirstOrDefault();
            if (oneClient != null)
                return Request.CreateResponse(HttpStatusCode.OK, oneClient);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        //// GET: api/Client/OneClient
        //[HttpGet]
        //[Route("api/Client/OneClient/{ID_number}/{password}")]
        //public HttpResponseMessage GetOneClient(string ID_number, string password  )
        //{
        //    ClientDTO oneClient = db.Clients.Where(a => a.ID_number == ID_number && a.password == password).Select(x => new ClientDTO
        //    {
        //        ID_number = x.ID_number,
        //        First_name = x.First_name,
        //        Last_name = x.Last_name,
        //        birth_date = x.birth_date,
        //        gender = x.gender,
        //        phone = x.phone,
        //        Email = x.Email,
        //        AddressStreet = x.AddressStreet,
        //        AddressHouseNumber = x.AddressHouseNumber,
        //        AddressCity = x.AddressCity,
        //        password = x.password
        //    }).FirstOrDefault();
        //    if (oneClient != null)
        //        return Request.CreateResponse(HttpStatusCode.OK, oneClient);
        //    else
        //        return Request.CreateResponse(HttpStatusCode.NotFound);
        //}

        // Post: api/Post
        [HttpPost]
        [Route("api/Client/NewClient")]
        public HttpResponseMessage PostNewClient([FromBody] ClientDTO x)
        {
            try
            {
                Client newClient = new Client()
                {
                    ID_number = x.ID_number,
                    First_name = x.First_name,
                    Last_name = x.Last_name,
                    birth_date = x.birth_date,
                    phone = x.phone,
                    gender = x.gender,
                    Email = x.Email,
                    AddressStreet = x.AddressStreet,
                    AddressHouseNumber = x.AddressHouseNumber,
                    AddressCity = x.AddressCity,
                    Facebook_link = x.Facebook_link,
                    Instagram_link = x.Instagram_link,
                    password = x.password

                };
                db.Client.Add(newClient);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "New client added to the database");
            }
            catch (DbUpdateException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred while adding new client to the database: " + ex.InnerException.InnerException.Message);
            }
        }

        [HttpPost]
        [Route("api/Client/UpdateClient")]
        public HttpResponseMessage UpdateClient([FromBody] ClientDTO newC)
        {
            Client prevC = db.Client.Where(x => x.ID_number == newC.ID_number).FirstOrDefault();
            if (prevC != null)
            {
                prevC.phone = newC.phone;
                prevC.gender = newC.gender;
                prevC.Email = newC.Email;
                prevC.AddressStreet = newC.AddressStreet;
                prevC.AddressCity = newC.AddressCity;
                prevC.AddressHouseNumber = newC.AddressHouseNumber;
                prevC.birth_date = newC.birth_date;
                prevC.First_name = newC.First_name;
                prevC.Last_name = newC.Last_name;
                prevC.Instagram_link = newC.Instagram_link;
                prevC.Facebook_link = newC.Facebook_link;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"Client {prevC.ID_number} had been updated!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "could'nt update Client");
            }
        }
        [HttpDelete]
        [Route("api/Client/DeleteClient/{clientId}")]
        public HttpResponseMessage DeleteBusiness(string clientId)
        {
            Client CtoDel = db.Client.Where(x => x.ID_number == clientId).FirstOrDefault();
            if (CtoDel != null)
            {
                db.Client.Remove(CtoDel);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, $"{clientId} deleted from dataBse!");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "couldn't delete Client from data base!");
            }

        }

        [HttpPost]
        [Route("api/Client/OneClientToken/{ID_number}/{token}")]
        public HttpResponseMessage SaveTokenUser(string ID_number, string token)
        {
            Client oneClient = db.Client.Where(a => a.ID_number == ID_number).FirstOrDefault();
            oneClient.token = token;
            db.SaveChanges();
            if (oneClient != null)
                return Request.CreateResponse(HttpStatusCode.OK,"ok");
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        } 
    }
}



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
    public class UserController : ApiController
    {
        // GET: api/User
        BeautyMeDBContext db = new BeautyMeDBContext   ();
        [HttpPost]
        [Route("api/user/checkUser")]
        public HttpResponseMessage CheckUser([FromBody] SearchPeopleDTO user)
        {
            ClientDTO c = db.Client.Where(x => x.ID_number == user.id_number && x.password == user.password).Select(x => new ClientDTO
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
                userType = "Cli"

            }).FirstOrDefault();
            if (c != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, c);
            }
            else
            {
                ProfessionalDTO p = db.Professional.Where(x => x.ID_number == user.id_number && x.password == user.password).Select(x => new ProfessionalDTO
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
                    userType = "Pro"
                }).FirstOrDefault();
                if (p != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, p);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User is not registered as client or as professional!!!");
                }
            }
        }
    }
}

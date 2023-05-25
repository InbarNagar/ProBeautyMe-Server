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
        BeautyMeDBContext db = new BeautyMeDBContext();
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
                Facebook_link = x.Facebook_link,
                Instagram_link = x.Instagram_link,
                password = x.password,
                userType = "Cli",
                token=x.token

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
                    userType = "Pro",
                    token = x.token

                }).FirstOrDefault();
                if (p != null)
                {
                  BusinessDTO number = db.Business.Where(x => x.Professional_ID_number == user.id_number).Select(y => new BusinessDTO
                    {
                        Business_Number = y.Business_Number
                    }
                   ).FirstOrDefault();
                   p.Business_Number = number.Business_Number;
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

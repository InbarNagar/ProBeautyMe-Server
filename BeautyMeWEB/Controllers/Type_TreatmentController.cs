using BeautyMe;
using BeautyMeWEB.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace BeautyMeWEB.Controllers
{
    public class Type_TreatmentController : ApiController
    {
        // GET: Type_Treatment
        BeautyMeDBContext db = new BeautyMeDBContext();
        [HttpGet]
        [Route("api/Type_Treatment/AllCategory")]
        public HttpResponseMessage GetAllType_Treatment()
        {
            List<Type_TreatmentDTO> AllType_Treatment = db.Type_Treatment.Select(x => new Type_TreatmentDTO
            {
                Type_treatment_Number = x.Type_treatment_Number,
                Name = x.Name,
            }).ToList();
            if (AllType_Treatment != null)
                return Request.CreateResponse(HttpStatusCode.OK, AllType_Treatment);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
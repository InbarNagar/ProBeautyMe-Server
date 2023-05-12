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
    public class CategoryController : ApiController
    {
        BeautyMeDBContext db = new BeautyMeDBContext();

        // GET: Category
        [HttpGet]
        [Route("api/Category/AllCategory")]
        public HttpResponseMessage GetAllCategory()
        {
 
            List<CategoryDTO> AllCategory = db.Category.Select(x => new CategoryDTO
            {
                Category_Number = x.Category_Number,
                Name = x.Name
            }).ToList();
            if (AllCategory != null)
                return Request.CreateResponse(HttpStatusCode.OK, AllCategory);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}
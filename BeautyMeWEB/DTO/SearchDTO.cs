using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeautyMeWEB.DTO
{
    public class SearchDTO
    {
        public string NameTreatment; // name of the treatment the client wants
        public string AddressCity; // city of the bussines the client wants
        public string gender; // gender of the professional. can be null
        public string Is_client_house; //if the business can come to the client. can be null
    }
}
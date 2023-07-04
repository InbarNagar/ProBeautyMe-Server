using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing.Constraints;
using BeautyMe;

namespace BeautyMeWEB.DTO
{
    public class BusinessDTO
    {
        public int Business_Number;
        public string Name;
        public string Is_client_house;
        public string Professional_ID_number;
        public string AddressStreet;
        public string AddressHouseNumber;
        public string AddressCity;
        public string About;
        public string Facebook_link;
        public string Instagram_link;
        public double LetCordinate;
        public double LongCordinate;
    }
}
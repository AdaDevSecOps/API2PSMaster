using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Card
{
    //[Serializable]
    public class cmlResCardDwn
    {
        public List<cmlResInfoCard> raCard { get; set; }
        public List<cmlResInfoCardLng> raCardLng { get; set; }
        public List<cmlResInfoCardType> raCardType { get; set; }
        public List<cmlResInfoCardTypeLng> raCardTypeLng { get; set; }
    }
}
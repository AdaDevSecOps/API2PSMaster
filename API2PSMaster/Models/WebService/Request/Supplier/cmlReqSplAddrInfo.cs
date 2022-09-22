using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Supplier
{
    public class cmlReqSplAddrInfo
    {
        ///// <summary>
        ///// Supplier code.
        ///// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        //[MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptSplCode { get; set; }

        ///// <summary>
        ///// Language ID.
        ///// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        //public int pnLngID { get; set; }

        ///// <summary>
        ///// Address group type 1:Supplier 2:Contact 3:Ship to.
        ///// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        //[MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptAddGrpType { get; set; }

        ///// <summary>
        ///// Address sequence no.
        ///// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        //public int pnAddSeqNo { get; set; }

        /// <summary>
        /// Address name.
        /// </summary>
        [MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddName { get; set; }

        /// <summary>
        /// Address tax no.
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddTaxNo { get; set; }

        /// <summary>
        /// Address remark.
        /// </summary>
        [MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddRmk { get; set; }

        /// <summary>
        /// Address country.
        /// </summary>
        [MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddCountry { get; set; }

        /// <summary>
        /// Area code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAreCode { get; set; }

        /// <summary>
        /// Zone code.
        /// </summary>
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptZneCode { get; set; }

        /// <summary>
        /// Address Version 1:Split 2:gather.
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddVersion { get; set; }

        /// <summary>
        /// Address no.
        /// </summary>
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddV1No { get; set; }

        /// <summary>
        /// Address soi.
        /// </summary>
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddV1Soi { get; set; }

        /// <summary>
        /// Address village.
        /// </summary>
        [MaxLength(70, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddV1Village { get; set; }

        /// <summary>
        /// Address Road.
        /// </summary>
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddV1Road { get; set; }

        /// <summary>
        /// Address subdistrict.
        /// </summary>
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddV1SubDist { get; set; }

        /// <summary>
        /// Address district Code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddV1DstCode { get; set; }

        /// <summary>
        /// Address province code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddV1PvnCode { get; set; }

        /// <summary>
        /// Address post code.
        /// </summary>
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddV1PostCode { get; set; }

        /// <summary>
        /// Address description 1.
        /// </summary>
        [MaxLength(255, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddV2Desc1 { get; set; }

        /// <summary>
        /// Address description 2.
        /// </summary>
        [MaxLength(255, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddV2Desc2 { get; set; }

        /// <summary>
        /// Address web site.
        /// </summary>
        [MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddWebSite { get; set; }

        /// <summary>
        /// Address Longitude.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddLongitude { get; set; }

        /// <summary>
        /// Address Latitude.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAddLatitude { get; set; }

        /// <summary>
        ///     Who update.
        /// </summary>
        [MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptWhoUpd { get; set; }
    }
}
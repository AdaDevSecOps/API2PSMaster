using API2PSMaster.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Request.Customer
{
    /// <summary>
    /// Customer information.
    /// </summary>
    public class cmlReqCstSch
    {
        //*Arm 63-04-02
        

        /// <summary>
        /// รหัสสมาชิก
        /// </summary>
        public string ptCstCode { get; set; }

        /// <summary>
        /// ชื่อลูกค้า
        /// </summary>
        public string ptCstName { get; set; }

        /// <summary>
        /// เบอร์โทร
        /// </summary>
        public string ptCstTel { get; set; }

        /// <summary>
        /// หมายเลขบัตรประชาชน
        /// </summary>
        public string ptCstCardID { get; set; }

        /// <summary>
        /// หมายเลขบัตรสมาชิก
        /// </summary>
        public string ptCstCrdNo { get; set; }

        /// <summary>
        /// หมายเลขประจำตัวผู้เสียภาษี
        /// </summary>
        public string ptCstTaxNo { get; set; }

        //+++++++++++++

        
        ///// <summary>
        ///// Customer code.
        ///// </summary>
        //[MaxLength(20, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptCstCode { get; set; }

        ///// <summary>
        ///// Customer name.
        ///// </summary>
        //[MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptCstName { get; set; }

        ///// <summary>
        ///// Customer name other.
        ///// </summary>
        //[MaxLength(200, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptCstNameOth { get; set; }

        ///// <summary>
        ///// Customer telephone.
        ///// </summary>
        //[MaxLength(50, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptCstTel { get; set; }

        ///// <summary>
        ///// Customer email.
        ///// </summary>
        //[MaxLength(100, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptCstEmail { get; set; }

        ///// <summary>
        ///// Operation condition to get data.<br/>
        ///// 1 : Match all.<br/>
        ///// 2 : Match some parts.
        ///// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        //[MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptOprCondition { get; set; }

        ///// <summary>
        ///// Column sort.<br/>
        ///// 1 : Customer code.<br/>
        ///// 2 : Customer name.<br/>
        ///// 3 : Customer name other.<br/>
        ///// 4 : Customer telephone.<br/>
        ///// 5 : Customer email.<br/>
        ///// example : Sort by Customer code and name send data 1,2
        ///// </summary>
        //[MaxLength(15, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptSortBy { get; set; }

        ///// <summary>
        ///// Order by.<br/>
        ///// 1 : Ascending order.<br/>
        ///// Other : Descending order.
        ///// </summary>
        //[MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        //public string ptOrderBy { get; set; }

        ///// <summary>
        ///// Customer languageg ID.
        ///// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        //public Nullable<long> pnLngID { get; set; }

        ///// <summary>
        ///// Page to get data.
        ///// </summary>
        //public int pnPage { get; set; }

        ///// <summary>
        ///// Row data return per page.
        ///// </summary>
        //public int pnRowPerPage { get; set; }
    }
}
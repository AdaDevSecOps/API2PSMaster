using API2PSMaster.Models.WebService.Response.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Coupon
{
    /// <summary>
    /// ข้อมูล Coupon
    /// </summary>
    public class cmlResCpnDwn
    {
        /// <summary>
        /// ข้อมูล CouponHD
        /// </summary>
        public List<cmlResInfoCpnHD> raCpnHD { get; set; }

        /// <summary>
        /// ข้อมูล CouponHD_L
        /// </summary>
        public List<cmlResInfoCpnHD_L> raCpnHD_L { get; set; }

        /// <summary>
        /// ข้อมูล CouponHDBch
        /// </summary>
        public List<cmlResInfoCpnHDBch> raCpnHDBch { get; set; }

        /// <summary>
        /// ข้อมูล CouponHDCstPri
        /// </summary>
        public List<cmlResInfoCpnHDCstPri> raCpnHDCstPri { get; set; }

        /// <summary>
        /// ข้อมูล CouponHDPdt
        /// </summary>
        public List<cmlResInfoCpnHDPdt> raCpnHDPdt { get; set; }

        /// <summary>
        /// ข้อมูล CouponDT
        /// </summary>
        public List<cmlResInfoCpnDT> raCpnDT { get; set; }

        /// <summary>
        /// ข้อมูล CouponType
        /// </summary>
        public List<cmlResInfoCpnType> raCpnType { get; set; }

        /// <summary>
        /// ข้อมูล CouponTyle_L
        /// </summary>
        public List<cmlResInfoCpnType_L> raCpnType_L { get; set; }

        /// <summary>
        /// รูปภาพ
        /// </summary>
        public List<cmlResInfoImgObj> raImage { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Models.WebService.Response.Coupon
{
    /// <summary>
    /// ตาราง TFNTCouponHD
    /// </summary>
    public class cmlResInfoCpnHD
    {
		///<summary>
		///รหัสสาขา
		///</summary>
		public string rtBchCode { get; set; }

		///<summary>
		///รหัสเอกสาร
		///</summary>
		public string rtCphDocNo { get; set; }

		///<summary>
		///รหัสประเภทคูปอง
		///</summary>
		public string rtCptCode { get; set; }

		///<summary>
		///วันที่เอกสาร
		///</summary>
		public DateTime? rdCphDocDate { get; set; }

		///<summary>
		///ประเภทส่วนลด 1: ลดบาท , 2: ลด % 3:ใช้กลุ่มราคา
		///</summary>
		public string rtCphDisType { get; set; }

		///<summary>
		///มูลค่าคูปอง
		///</summary>
		public decimal? rcCphDisValue { get; set; }

		///<summary>
		///รหัสกลุ่มราคา
		///</summary>
		public string rtPplCode { get; set; }

		///<summary>
		///วันที่เริ่มใช้งาน
		///</summary>
		public DateTime? rdCphDateStart { get; set; }

		///<summary>
		///วันสิ้นสุดใช้งาน
		///</summary>
		public DateTime? rdCphDateStop { get; set; }

		///<summary>
		///เวลาเริ่มใช้งาน
		///</summary>
		public string rtCphTimeStart { get; set; }

		///<summary>
		///เวลาสิ้นสุดใช้งาน
		///</summary>
		public string rtCphTimeStop { get; set; }

		///<summary>
		///สถานะหยุดใช้งาน 1:ใช้งาน , 2:หยุดใช้งาน
		///</summary>
		public string rtCphStaClosed { get; set; }

		///<summary>
		///พนักงานคีย์
		///</summary>
		public string rtUsrCode { get; set; }

		///<summary>
		///ผู้อนุมัติ
		///</summary>
		public string rtCphUsrApv { get; set; }

		///<summary>
		///สถานะ เอกสาร  1:สมบูรณ์, 2:ไม่สมบูรณ์, 3:ยกเลิก
		///</summary>
		public string rtCphStaDoc { get; set; }

		///<summary>
		///สถานะ อนุมัติ เอกสาร ว่าง:ยังไม่ทำ, 1:อนุมัติแล้ว
		///</summary>
		public string rtCphStaApv { get; set; }

		///<summary>
		///สถานะ ประมวลผลเอกสาร ว่าง หรือ Null:ยังไม่ทำ, 1:ทำแล้ว
		///</summary>
		public string rtCphStaPrcDoc { get; set; }

		///<summary>
		///สถานะ ลบ MQ ว่าง:ยังไม่ทำ, 1:อนุมัติแล้ว
		///</summary>
		public string rtCphStaDelMQ { get; set; }

		///<summary>
		///ยอดต่ำสุดที่สามารถใช้ได้ 0:ไม่ตรวจสอบ
		///</summary>
		public decimal? rtCphMinValue { get; set; }

		///<summary>
		///คำนวนรวมสินค้าโปรโมชั่น 1:อนญาตคำนวน 2:ไม่อนญาตคำนวน
		///</summary>
		public string rtCphStaOnTopPmt { get; set; }

		///<summary>
		///จำนวนจำกัดการใช้ต่อบิล 0: ไม่จำกัด
		///</summary>
		public Int64? rnCphLimitUsePerBill { get; set; }

		///<summary>
		///รหัสอ้างอิงบัญชีของคูปอง
		///</summary>
		public string rtCphRefAccCode { get; set; }

		///<summary>
		///สถานะตรวจสอบลูกค้า
		///</summary>
		public string rtStaChkMember { get; set; }

		///<summary>
		///วันที่ปรับปรุงรายการล่าสุด
		///</summary>
		public DateTime? rdLastUpdOn { get; set; }

		///<summary>
		///ผู้ปรับปรุงรายการล่าสุด
		///</summary>
		public string rtLastUpdBy { get; set; }

		///<summary>
		///วันที่สร้างรายการ
		///</summary>
		public DateTime? rdCreateOn { get; set; }

		///<summary>
		///ผู้สร้างรายการ
		///</summary>
		public string rtCreateBy { get; set; }



	}
}
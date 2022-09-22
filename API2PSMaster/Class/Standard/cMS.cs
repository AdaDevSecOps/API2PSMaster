using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2PSMaster.Class.Standard
{
    public class cMS
    {
        // Response code.
        public readonly string tMS_RespCode001 = "1";
        public readonly string tMS_RespCode700 = "700";
        public readonly string tMS_RespCode701 = "701";
        public readonly string tMS_RespCode702 = "702";
        public readonly string tMS_RespCode703 = "703";
        public readonly string tMS_RespCode704 = "704";
        public readonly string tMS_RespCode705 = "705";
        public readonly string tMS_RespCode706 = "706";
        public readonly string tMS_RespCode707 = "707";
        public readonly string tMS_RespCode708 = "708";
        public readonly string tMS_RespCode709 = "709";
        public readonly string tMS_RespCode710 = "710"; //*Arm 63-08-08
        public readonly string tMS_RespCode800 = "800";
        public readonly string tMS_RespCode801 = "801";
        public readonly string tMS_RespCode802 = "802";
        public readonly string tMS_RespCode803 = "803";
        public readonly string tMS_RespCode804 = "804";
        public readonly string tMS_RespCode813 = "813";
        public readonly string tMS_RespCode900 = "900";
        public readonly string tMS_RespCode904 = "904";
        public readonly string tMS_RespCode905 = "905";
        public readonly string tMS_RespCode906 = "906";
        public readonly string tMS_RespCode999 = "999";

        // Response descript.
        public readonly string tMS_RespDesc001 = "success.";
        public readonly string tMS_RespDesc700 = "all parameter is null.";
        public readonly string tMS_RespDesc701 = "validate parameter model false.|";
        public readonly string tMS_RespDesc702 = "barcode duplicate.";
        public readonly string tMS_RespDesc703 = "product group chain not found in group chain master.";
        public readonly string tMS_RespDesc704 = "product unit not found in unit master.";
        public readonly string tMS_RespDesc705 = "product factor not allow less than 1.";
        public readonly string tMS_RespDesc706 = "product retail price not allow less than 0.";
        public readonly string tMS_RespDesc707 = "barcode not empty.";
        public readonly string tMS_RespDesc708 = "path folder image not found.";
        public readonly string tMS_RespDesc709 = "This Mac.Address is already registered."; //*Arm 63-07-09
        public readonly string tMS_RespDesc710 = "This POS is already registered."; //*Arm 63-08-08
        public readonly string tMS_RespDesc800 = "data not found.";
        public readonly string tMS_RespDesc801 = "data is duplicate.";
        public readonly string tMS_RespDesc802 = "generate code false.";
        public readonly string tMS_RespDesc803 = "format code not found.";
        public readonly string tMS_RespDesc804 = "code maximum running number.";
        public readonly string tMS_RespDesc813 = "data is referent in another data.";
        public readonly string tMS_RespDesc900 = "service process false.";
        public readonly string tMS_RespDesc904 = "key not allowed to use method.";
        public readonly string tMS_RespDesc905 = "cannot connect database.";
        public readonly string tMS_RespDesc906 = "this time not allowed to use method.";
    }
}
using API2PSMaster.Class.Standard;
using API2PSMaster.Models.WebService.Response.Base;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API2PSMaster.Controllers
{
    /// <summary>
    ///     File manage.
    /// </summary>
    [ApiController]
    [Route(cCS.tCS_APIVer + "/FileManage")]
    public class cFileManageController : ControllerBase
    {
        /// <summary>
        /// Create URL for download file.
        /// </summary>
        /// <param name="ptPathFile"></param>
        /// <returns></returns>
        [Route("CreateURL")]
        [HttpPost]
        public cmlResItem<string> POST_FLEtCreateUrlFile([FromBody] string ptPathFile)
        {
            string tPathDwn = "";
            FileInfo oFile;
            cmlResItem<string> aoResult;
            cMS oMsg =new cMS();
            string tPathFile = "";
            try
            {
                aoResult = new cmlResItem<string>();

                if (ptPathFile == null)
                {
                    aoResult.roItem = tPathDwn;
                    aoResult.rtCode = oMsg.tMS_RespCode700;
                    aoResult.rtDesc = oMsg.tMS_RespDesc700;
                    return aoResult;
                }
                tPathFile = ptPathFile;

                oFile = new FileInfo(tPathFile);
                if (oFile.Exists)
                {
                    tPathDwn = AppDomain.CurrentDomain.BaseDirectory + "FileSend";
                    if (!Directory.Exists(tPathDwn)) Directory.CreateDirectory(tPathDwn);
                    if (!System.IO.File.Exists(tPathDwn + @"\" + oFile.Name))
                    {
                        System.IO.File.Copy(tPathFile, tPathDwn + @"\" + oFile.Name);
                    }
                    else
                    {
                        //*Arm 63-02-26

                        // ถ้ามีไฟล์อยู่ ให้ลบไฟล์เดิมทิ้งก่อน แล้ว Copy ไฟล์ใหม่มาวาง
                        System.IO.File.Delete(Path.Combine(tPathDwn, oFile.Name));
                        System.IO.File.Copy(tPathFile, tPathDwn + @"\" + oFile.Name);

                        //++++++++++++++++
                    }

                    if (System.IO.File.Exists(tPathDwn + @"\" + oFile.Name))
                    {
                        //*Ton 64-05-22
                        //tPathDwn = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Request.ApplicationPath + @"/FileSend/" + oFile.Name;
                        string tRqUrl = HttpContext.Request.Host.Value;
                        string tRqPath = HttpContext.Request.Path.Value;
                        string tFilePath = @"/FileSend/" + oFile.Name;
                        tPathDwn = tRqUrl + tRqPath + tFilePath;

                        aoResult.roItem = tPathDwn;
                        aoResult.rtCode = oMsg.tMS_RespCode001;
                        aoResult.rtDesc = oMsg.tMS_RespDesc001;
                        return aoResult;
                    }
                    else
                    {
                        tPathDwn = "";
                        aoResult.roItem = tPathDwn;
                        aoResult.rtCode = oMsg.tMS_RespCode800;
                        aoResult.rtDesc = oMsg.tMS_RespDesc800;
                        return aoResult;
                    }
                }
                else
                {
                    tPathDwn = "";
                    aoResult.roItem = tPathDwn;
                    aoResult.rtCode = oMsg.tMS_RespCode800;
                    aoResult.rtDesc = oMsg.tMS_RespDesc800;
                    return aoResult;
                }
            }
            catch (Exception oEx)
            {
                tPathDwn = "";
                aoResult = new cmlResItem<string>();
                aoResult.roItem = tPathDwn;
                aoResult.rtCode = oMsg.tMS_RespCode900;
                aoResult.rtDesc = oMsg.tMS_RespDesc900 + " : " + oEx.Message.ToString();
                return aoResult;
            }
        }
    }
}
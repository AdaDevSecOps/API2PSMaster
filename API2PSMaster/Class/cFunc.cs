using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API2PSMaster.Class
{
    //*Ton 64-05-22
    public class cFunc
    {
        public static string C_PRCtPrepareFile(string ptFile,HttpContext poContext)
        {
            string tPathDwn = "";
            FileInfo oFile;
            try
            {
                oFile = new FileInfo(ptFile);
                if (oFile.Exists)
                {
                    tPathDwn = AppDomain.CurrentDomain.BaseDirectory + "MediaFile";
                    if (!Directory.Exists(tPathDwn)) Directory.CreateDirectory(tPathDwn);
                    if (!System.IO.File.Exists(tPathDwn + @"\" + oFile.Name))
                    {
                        System.IO.File.Copy(ptFile, tPathDwn + @"\" + oFile.Name);
                    }

                    if (System.IO.File.Exists(tPathDwn + @"\" + oFile.Name))
                    {
                        //tPathDwn = Request.RequestUri.GetLeftPart(UriPartial.Authority) + HttpContext.Request.ApplicationPath + @"/MediaFile/" + oFile.Name;

                        //*Ton 64-05-22
                        //tPathDwn = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Request.ApplicationPath + @"/MediaFile/" + oFile.Name;
                        string tRqUrl = poContext.Request.Host.Value;
                        string tRqPath = poContext.Request.Path.Value;
                        string tFilePath = @"/MediaFile/" + oFile.Name;
                        tPathDwn = tRqUrl + tRqPath + tFilePath;
                    }
                    else
                    {
                        tPathDwn = "";
                    }
                }
                else
                {
                    tPathDwn = "";
                }
            }
            catch (Exception oExcept)
            {
                tPathDwn = oExcept.Message.ToString();
            }

            return tPathDwn;
        }
    }
}

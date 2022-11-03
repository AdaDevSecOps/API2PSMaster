using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace API2PSMaster.Class
{
    //    public partial class AdaAccEntities : DbContext
    //    {
    //        public AdaAccEntities()
    //            : base("name=AdaAccEntities")
    //        {
    //        }
    //        public AdaAccEntities()
    //        : base("metadata=res://*/EF.AdaAcc.csdl|res://*/EF.AdaAcc.ssdl|res://*/EF.AdaAcc.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=202.44.55.96;initial catalog=SKC_Fullloop2;user id=sa;password=GvFhk@61;MultipleActiveResultSets=True;App=EntityFramework&quot;\" name = \"AdaAccEntities\" providerName = \"System.Data.EntityClient\"")
    //        {
    //        }
    //        public AdaAccEntities(string ptConnStr) : base(ptConnStr)
    //        {
    //        }


    //        protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //        {

    //            throw new UnintentionalCodeFirstException();
    //        }

    //        public virtual DbSet<TCNTPdtTnfDT> TCNTPdtTnfDTs { get; set; }
    //        public virtual DbSet<TCNTPdtTnfDTSrn> TCNTPdtTnfDTSrns { get; set; }
    //        public virtual DbSet<TCNTPdtTnfHD> TCNTPdtTnfHDs { get; set; }
    //        public virtual DbSet<TCNTPdtTnfHDRef> TCNTPdtTnfHDRefs { get; set; }
    //    }
    //}
    //*Ton 64-05-21 สร้าง Class แทน Class ตัวเก่าที่เป็น EF DbContext
    //จะได้ไม่ต้องไปแก้ไข Code ที่ Controller ทุกตัว
    //สร้างเพื่อจุดประสงค์ในการ อ่านค่าจาก ConnectionString จาก appSettings.json
    public class AdaAccEntities : IDisposable
    {
        public cDatabase Database { get; private set; }
        public AdaAccEntities()
        {
            Database = new cDatabase();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public class cDatabase : IDisposable
        {
            public cDatabase()
            {
                Connection = new SqlConnection(cAppSetting.Default.tConnDB);
            }
            public SqlConnection Connection { get; private set; }

            public void Dispose()
            {
                if (Connection.State == System.Data.ConnectionState.Open) Connection.Close();
                Connection.Dispose();
            }
        }

    }
}

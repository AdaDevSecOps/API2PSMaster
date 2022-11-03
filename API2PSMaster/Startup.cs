using API2PSMaster.Class;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace API2PSMaster
{
    public class Startup
    {

        string tC_AppName;
        string tC_AppVer;
        string tC_RunTimeVer;
        public static string tC_VirtualPath;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //*Ton 64-05-20 Populate AppSettings
            Configuration.GetSection("AppSettings").Bind(cAppSetting.Default);
            foreach(PropertyInfo info in cAppSetting.Default.GetType().GetProperties())
            {
                string tEnvName = $"ENV_{info.Name}";
                string tEnvVal = Environment.GetEnvironmentVariable(tEnvName);
                if (!string.IsNullOrEmpty(tEnvVal))
                {
                    info.SetValue(cAppSetting.Default, tEnvVal);
                }
            }
            tC_VirtualPath = Environment.GetEnvironmentVariable("ENV_VirtualPath");
            tC_AppName = Assembly.GetExecutingAssembly().GetName().Name;
            tC_AppVer = Assembly.GetEntryAssembly().GetName().Version.ToString();
            tC_RunTimeVer = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            const string tReqHeaders = "X-Api-Key";
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{tC_AppName} V{tC_AppVer}", Version = $"{tC_RunTimeVer}" });
                c.AddSecurityDefinition(tReqHeaders, new OpenApiSecurityScheme
                {
                    Description = "Api key needed to access the endpoints. X-Api-Key: My_API_Key",
                    In = ParameterLocation.Header,
                    Name = tReqHeaders,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = tReqHeaders,
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = tReqHeaders
                            },
                         },
                         new string[] {}
                     }
                });
            });

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //*Ton 2021-08-25
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{tC_AppName} V{tC_AppVer}"));
            //}

            //app.UseFileServer(new FileServerOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "HomePage")),
            //    RequestPath = "",
            //    EnableDefaultFiles = true
            //});

            //*Ton 2021-08-25
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint($"{tC_VirtualPath}/swagger/v1/swagger.json", $"{tC_AppName} V{tC_AppVer}"); //*Arm 65-03-08 -Comment Code

                //*Arm 65-03-08 -Optimized to support displaying UI pages on Windows Server.
                if (!string.IsNullOrEmpty(tC_VirtualPath))
                {
                    c.SwaggerEndpoint($"{tC_VirtualPath}/swagger/v1/swagger.json", $"{tC_AppName} V{tC_AppVer}");
                }
                else
                {
                    c.SwaggerEndpoint($"v1/swagger.json", $"{tC_AppName} V{tC_AppVer}");
                }
                //+++++++++++++++
            });
        }
    }
}

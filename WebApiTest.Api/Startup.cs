using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebApiTest.Api.Data;
using WebApiTest.Api.Repostories;
using WebApiTest.Api.Services;

namespace WebApiTest.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //ÿһ��http���� ������ ���� 

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });


            //jwt ���� 
            // �����½�һ����Կ 
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("chenNingJsonWebToken"));

            // Ȼ������֤����
            services.AddAuthentication("Bearer")
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true, //1. �Ƿ�����Կ��֤ 
                        IssuerSigningKey = securityKey, //1 . ������֤��Կ

                        ValidateIssuer = true, //2.�Ƿ���֤ ���� �� 
                        ValidIssuer = "chenNingJwtApi", //2. ���� ��  ����

                        ValidateAudience = true, //3.�Ƿ���֤ ���� �� 
                        ValidAudience = "chenNingJwtApp", //3.���� ��  ����

                        RequireExpirationTime = true, //4.�Ƿ���֤ ����ʱ��  
                        ValidateLifetime = true, //5.�������� 
                    };
                });

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //�����쳣ҳ�� 
                app.UseDeveloperExceptionPage();

                ////Ҳ����Ӧ�� �쳣  ���� һЩ ����  ��
                //app.UseExceptionHandler(appbuilder =>
                //{
                //    appbuilder.Run(async context =>
                //    {
                //        context.Response.StatusCode = 500;
                //        await context.Response.WriteAsync("�������д�������");
                //        //ʵ����Ŀ��������Ҫ��¼ ����־ �ġ��� 
                //    });
                //});
            }

            app.UseRouting();

            app.UseAuthentication(); //jwt ����֤ �м� �� һ��Ҫ ��  UseAuthorization


            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
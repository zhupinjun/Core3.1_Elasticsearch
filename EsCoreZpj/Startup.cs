using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EsCoreZpj.ESClass;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EsCoreZpj
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
            services.Configure<EsConfig>(options =>
            {
                options.Urls = Configuration.GetSection("EsConfig:ConnectionStrings").GetChildren().ToList().Select(p => p.Value).ToList();

            });


            services.AddSingleton<IEsClientProvider, EsClientProvider>();
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(p => !p.IsAbstract && (p.GetInterfaces().Any(i => i == typeof(IBaseEsContext)))).ToList();
            types.ForEach(p =>
                    services.AddTransient(p)
                );

            // ע��Swagger����
            services.AddSwaggerGen(c =>
            {
                // ����ĵ���Ϣ
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "CoreWebApi", Version = "v1" });

                // ʹ�÷����ȡxml�ļ�����������ļ���·��
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // ����xmlע��. �÷����ڶ����������ÿ�������ע�ͣ�Ĭ��Ϊfalse.
                //c.IncludeXmlComments(xmlPath, true);
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // ����Swagger�м��
            app.UseSwagger();

            // ����SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoreWebApi");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

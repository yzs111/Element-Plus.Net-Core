using CW_ToyShopping.Common.Cache;
using CW_ToyShopping.Common.Configs;
using CW_ToyShopping.Common.Helpers;
using CW_ToyShopping.DB;
using CW_ToyShopping.Enity.UserModels;
using CW_ToyShopping.Extensions;
using CW_ToyShopping.Filters;
using CW_ToyShopping.IRepository;
using CW_ToyShopping.IService;
using CW_ToyShopping.Repository;
using CW_ToyShopping.Service;
using CW_ToyShopping.Service.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private ConfigHelper _configHelper { get; }
        private IHostEnvironment _env { get; }
        private AppConfig _appConfig { get; }
        // ��������
        private const string DefaultCorsPolicyName = "Allow";
        private IHttpClientFactory _HttpClientFactory { get; set; }

        string filepath = System.IO.Directory.GetCurrentDirectory().Replace("\\", "/") + "/Images";

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _configHelper = new ConfigHelper();
            _env = env;
            _appConfig = _configHelper.Get<AppConfig>("appconfig", env.EnvironmentName) ?? new AppConfig();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region ������

            services.AddControllers()
                .AddNewtonsoftJson(options=> {
                    //����ʱ���ʽ
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    // ����ѭ������
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    // ���ݰ���ԭ����ʽ���
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });

            #endregion

            #region SwaggerApi
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "CW_ToyShopping",
                        Version = "v1",
                        Description = "Swagger Demo for ValuesController"
                    });

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Xml","CW_ToyShopping.xml");

                c.IncludeXmlComments(filePath);

                #region ���Token��ť
                //Bearer ��scheme����
                var securityScheme = new OpenApiSecurityScheme()
                {
                    //�԰�ȫ�����ļ��������CommonMark�﷨�������ڸ��ı���ʾ��
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    //����ġ�Ҫʹ�õı�ͷ����ѯ��cookie���������ơ�
                    Name = "Authorization",
                    //���������ͷ��
                    In = ParameterLocation.Header,
                    //ʹ��Authorizeͷ��
                    Type = SecuritySchemeType.ApiKey,
                    //����Ϊ�� bearer��ͷ
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                //�����з�������Ϊ����bearerͷ����Ϣ
                var securityRequirement = new OpenApiSecurityRequirement
                    {
                        {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "bearerAuth"
                                    }
                                },
                                new string[] {}
                        }
                    };

                //ע�ᵽswagger��
                c.AddSecurityDefinition("bearerAuth", securityScheme);
                c.AddSecurityRequirement(securityRequirement);

                #endregion
            });
            #endregion

            #region �ִ�ʵ��
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            services.AddScoped<IServiceWrapper, ServiceWrapper>();
    
            #endregion

            #region AutoMapperʵ��
            services.AddAutoMapper(typeof(LibraryMappingProfile));
            #endregion

            #region oracleע��ʵ��
            // oracleע��ʵ��
           /* services.AddDbContext<OracleDBContext>(options =>
                 options.UseOracle(Configuration.GetConnectionString("DefaultConnection"), b => b.UseOracleSQLCompatibility("11"))
                 );*/
            // ע��MySql����
            services.AddDbContext<OracleDBContext>(options => options.UseMySql("server=localhost;database=sa;user=root;password=123456;"));
            #endregion

            #region ע����־ʵ��
            services.AddScoped<ILogger>(sp => {
                return sp.GetService<ILogger<Program>>();
            });
            #endregion

            #region �쳣����
            services.AddMvc(confing =>
            {
                confing.Filters.Add<JsonExceptionFilter>();
                confing.ReturnHttpNotAcceptable = true;
            })
            .AddXmlSerializerFormatters();
            
            #endregion

            #region ����
            services.AddMemoryCache();
            services.AddSingleton<ICache, MemoryCache>();
            #endregion

            #region Identity����
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6; // ������С����
                options.Password.RequireNonAlphanumeric = false; // ������������ĸ�ر�
                options.Password.RequireUppercase = false; // ���������д�ر�
            });
            services.AddIdentity<User, Role>()
             .AddEntityFrameworkStores<OracleDBContext>()
             .AddDefaultTokenProviders();

            #region ������������Ȩ
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", policy =>
                {
                    policy.RequireClaim("Delete", "Delete Role");
                    policy.RequireRole("Administrator");
                });
            });
            #endregion

            #endregion

            #region JWT��֤

            var settings = new Jwtconfig();

            _configHelper.Bind("jwtconfig", settings, _env.EnvironmentName, true);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audienec,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSingleton(settings);
            #endregion

            #region Cors����
            if (_appConfig.CorUrls?.Length > 0)
            {
                services.AddCors(options => {

                    options.AddPolicy(DefaultCorsPolicyName, builder =>
                    builder
                    .WithOrigins(_appConfig.CorUrls)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    );
                });
            }
            #endregion

            #region ע��ȫ��΢�ŷ���

            // ����Session
            services.AddSession();

            services.AddHttpContextAccessor();

            services.AddHttpClient("WxClient", config =>
            {
               
                config.BaseAddress = new Uri(Configuration["Wx:baseurl"]);

                config.DefaultRequestHeaders.Add("Accept", "application/json");

            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CW_ToyShopping v1"));
            }

            // ����
            app.UseMiddleware<RequestRateLimitingMiddleware>(); 

            //����
            if (_appConfig.CorUrls?.Length > 0)
            {
                app.UseCors(DefaultCorsPolicyName);
            }
            app.UseStaticFiles(new StaticFileOptions() 
            {
                FileProvider = new PhysicalFileProvider(filepath),//·��
                RequestPath = new PathString("/Upload"),//����ķ���·��
            });
        
            //var aa = app.ApplicationServices.GetService<IHttpClientFactory>;

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            // �Զ�����session
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });        
        }
    }
}

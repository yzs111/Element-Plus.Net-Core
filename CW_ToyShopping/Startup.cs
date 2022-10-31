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
        // 跨域名称
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

            #region 控制器

            services.AddControllers()
                .AddNewtonsoftJson(options=> {
                    //设置时间格式
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    // 忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    // 数据按照原样格式输出
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

                #region 添加Token按钮
                //Bearer 的scheme定义
                var securityScheme = new OpenApiSecurityScheme()
                {
                    //对安全方案的简短描述。CommonMark语法可以用于富文本表示。
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    //必需的。要使用的报头、查询或cookie参数的名称。
                    Name = "Authorization",
                    //参数添加在头部
                    In = ParameterLocation.Header,
                    //使用Authorize头部
                    Type = SecuritySchemeType.ApiKey,
                    //内容为以 bearer开头
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };

                //把所有方法配置为增加bearer头部信息
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

                //注册到swagger中
                c.AddSecurityDefinition("bearerAuth", securityScheme);
                c.AddSecurityRequirement(securityRequirement);

                #endregion
            });
            #endregion

            #region 仓储实现
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            services.AddScoped<IServiceWrapper, ServiceWrapper>();
    
            #endregion

            #region AutoMapper实例
            services.AddAutoMapper(typeof(LibraryMappingProfile));
            #endregion

            #region oracle注册实例
            // oracle注册实例
           /* services.AddDbContext<OracleDBContext>(options =>
                 options.UseOracle(Configuration.GetConnectionString("DefaultConnection"), b => b.UseOracleSQLCompatibility("11"))
                 );*/
            // 注册MySql服务
            services.AddDbContext<OracleDBContext>(options => options.UseMySql("server=localhost;database=sa;user=root;password=123456;"));
            #endregion

            #region 注册日志实例
            services.AddScoped<ILogger>(sp => {
                return sp.GetService<ILogger<Program>>();
            });
            #endregion

            #region 异常处理
            services.AddMvc(confing =>
            {
                confing.Filters.Add<JsonExceptionFilter>();
                confing.ReturnHttpNotAcceptable = true;
            })
            .AddXmlSerializerFormatters();
            
            #endregion

            #region 缓存
            services.AddMemoryCache();
            services.AddSingleton<ICache, MemoryCache>();
            #endregion

            #region Identity服务
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6; // 密码最小长度
                options.Password.RequireNonAlphanumeric = false; // 密码必须包含字母关闭
                options.Password.RequireUppercase = false; // 必须包含大写关闭
            });
            services.AddIdentity<User, Role>()
             .AddEntityFrameworkStores<OracleDBContext>()
             .AddDefaultTokenProviders();

            #region 基于声明的授权
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

            #region JWT验证

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

            #region Cors跨域
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

            #region 注册全局微信服务

            // 开启Session
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

            // 限流
            app.UseMiddleware<RequestRateLimitingMiddleware>(); 

            //跨域
            if (_appConfig.CorUrls?.Length > 0)
            {
                app.UseCors(DefaultCorsPolicyName);
            }
            app.UseStaticFiles(new StaticFileOptions() 
            {
                FileProvider = new PhysicalFileProvider(filepath),//路径
                RequestPath = new PathString("/Upload"),//对外的访问路径
            });
        
            //var aa = app.ApplicationServices.GetService<IHttpClientFactory>;

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            // 自动开启session
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });        
        }
    }
}

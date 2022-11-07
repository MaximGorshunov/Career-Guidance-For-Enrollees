using DataAccess;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.IServices;
using Services.Services;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace CGEService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("null", "http://localhost:3000", "http://localhost:5000", "https://b3namine.github.io")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition");
                });
            });
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddControllers();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            services.AddSwaggerGen(options =>
            {
                var req = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new[] { "readAccess", "writeAccess" }
                    }
                };
                options.AddSecurityRequirement(req);
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Career guidance for enrollees API", Version = "1.0" });
                options.DescribeAllParametersInCamelCase();
                options.IgnoreObsoleteActions();
                options.IgnoreObsoleteProperties();
                if (xmlPath != null)
                    options.IncludeXmlComments(xmlPath);
            });

            services.AddDbContext<DataAccessContext>(options => options.UseNpgsql(Configuration.GetConnectionString("HerokuConnection")));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserResultRepository, UserResultRepository>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<IProfessionRepository, ProfessionRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUniversityRepository, UniversityRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<ICourseRepository, CourseRepository>();
            services.AddTransient<IProfessionCourseRepository, ProfessionCourseRepository>();
            services.AddTransient<IProfessionalTypeRepository, ProfessionalTypeRepository>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserResultService, UserResultService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IProfessionService, ProfessionService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUniversityService, UniversityService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<ICourseService, CourseService>();
            services.AddTransient<IProfessionCourseService, ProfessionCourseService>();
            services.AddTransient<IProfessionalTypeService, ProfessionalTypeService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(sui =>
            {
                sui.SwaggerEndpoint("/swagger/v1/swagger.json", "Career guidance for enrollees API");
                sui.RoutePrefix = "swagger/ui";
                sui.DisplayOperationId();
                sui.DisplayRequestDuration();
                sui.EnableDeepLinking();
                sui.EnableFilter();
                sui.ShowExtensions();
                sui.EnableValidator();
            });

            app.UseMvc();
            app.UseEndpoints(x => x.MapControllers());
        }
    }
}

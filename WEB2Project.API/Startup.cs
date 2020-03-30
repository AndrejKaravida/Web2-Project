using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Net;
using System.Text;
using WEB2Project.API.Data;
using WEB2Project.API.Models;
using WEB2Project.Data;
using WEB2Project.Helpers;

namespace WEB2Project
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
            services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);  
            services.AddAutoMapper(typeof(RentACarRepository).Assembly);
            services.AddCors();
            services.AddTransient<IImageHandler, ImageHandler>();
            services.AddTransient<IImageWriter, ImageWriter>();
            services.AddControllers();
            services.AddScoped<IRentACarRepository, RentACarRepository>();
            services.AddScoped<IFlightsRepository, FlightsRepository>();
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddMvc();

            /*
     IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
     {
         opt.Password.RequireDigit = false;
         opt.Password.RequiredLength = 6;
         opt.Password.RequireNonAlphanumeric = false;
         opt.Password.RequireUppercase = false;
     });
         */
            /*
               services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                             .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                         ValidateIssuer = false,
                         ValidateAudience = false
                     };
                 });

               services.AddAuthorization(options =>
               {
                   options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                   options.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
                   options.AddPolicy("VipOnly", policy => policy.RequireRole("VIP"));

               });

               builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
               builder.AddEntityFrameworkStores<DataContext>();
               builder.AddRoleValidator<RoleValidator<Role>>();
               builder.AddRoleManager<RoleManager<Role>>();
               builder.AddSignInManager<SignInManager<User>>();

               services.AddMvc(options =>

                   var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                         .Build();

                   options.Filters.Add(new AuthorizeFilter(policy));
               });

               */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseRouting();
       //     app.UseAuthentication();
     //       app.UseAuthorization();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources/Images")),

            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            InitialData.Initialize(app);
        }
    }
}

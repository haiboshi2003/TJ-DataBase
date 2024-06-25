using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Oracle.ManagedDataAccess.Client;
using System.Text;
using web.Helpers;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://0.0.0.0:7078");
                webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {

                    string connectionString = hostContext.Configuration.GetConnectionString("OracleConnection");


                    services.AddScoped<OracleConnection>(_ => new OracleConnection(connectionString));


                    services.AddControllers();


                    // services.AddCors(options =>
                    // {
                    //     options.AddDefaultPolicy(builder =>
                    //     {
                    //         builder.AllowAnyOrigin()
                    //               .AllowAnyMethod()
                    //               .AllowAnyHeader();
                    //     });
                    // });


                    services.AddCors(options =>
                    {
                        options.AddDefaultPolicy(builder =>
                        {
                            builder.WithOrigins("http://118.25.23.3") // 指定您的前端应用程序地址
                                  .AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .AllowCredentials(); // 允许携带身份验证信息
                        });
                    });



                    SerialnumHelper.FormSeqTest_Load();

                    services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    }).AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidIssuer = "PoliceApp",
                            ValidateAudience = true,
                            ValidAudience = "PoliceAppUser",
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                                    "123cdefefefeasd14a5445411sds65d4asw65f4e")),
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.FromMinutes(10),
                            RequireExpirationTime = true,
                        };
                    });
                    services.AddHttpContextAccessor();
                    services.AddSwaggerGen(options =>
                    {
                        options.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Version = "v1",
                            Title = "API",
                            Description = "API"
                        });
                    });
                    services.AddSwaggerGen(c =>
                    {

                        c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Id = "Bearer",
                                        Type = ReferenceType.SecurityScheme
                                    }
                                },
                                new List<string>()
                            }
                        });


                        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Description = "Value Bearer {token}",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.ApiKey
                        });
                    });

                })
                .Configure(app =>
                {


                    app.UseRouting();
                    app.UseCors();



                    app.UseAuthentication();
                    app.UseAuthorization();


                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web1");
                    });

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });
                });
            });
}
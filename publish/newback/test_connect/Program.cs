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
                    // ��ȡ���ݿ������ַ���
                    string connectionString = hostContext.Configuration.GetConnectionString("OracleConnection");

                    // ע��Oracle���Ӷ�������ע������
                    services.AddScoped<OracleConnection>(_ => new OracleConnection(connectionString));

                    // ���ӿ�����
                    services.AddControllers();

                    // ���� CORS ����
                    services.AddCors(options =>
                    {
                        options.AddDefaultPolicy(builder =>
                        {
                            builder.AllowAnyOrigin()
                                   .AllowAnyMethod()
                                   .AllowAnyHeader();
                        });
                    });

                    SerialnumHelper.FormSeqTest_Load();
                    // ����JWT
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
                            Title = "API����",
                            Description = "API����"
                        });
                    });
                    services.AddSwaggerGen(c =>
                    {
                        //����Jwt��֤����,��������ͷ��Ϣ
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

                        //���ýӿ�Auth��Ȩ��ť
                        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Description = "Value Bearer {token}",
                            Name = "Authorization",//jwtĬ�ϵĲ�������
                            In = ParameterLocation.Header,//jwtĬ�ϴ��Authorization��Ϣ��λ��(����ͷ��)
                            Type = SecuritySchemeType.ApiKey
                        });
                    });

                })
                .Configure(app =>
                {
                    app.UseRouting();

                    // ���� CORS �м��
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

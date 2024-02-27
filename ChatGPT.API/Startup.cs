using ChatGPT.API.Interfaces;
using ChatGPT.API.Models;
using ChatGPT.API.Repositories;
using Microsoft.ML;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace ChatGPT.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public readonly IWebHostEnvironment env1;
        public Startup(IConfiguration _configuration, IWebHostEnvironment env)
        {
            Configuration = _configuration;
            env1 = env;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MLContext>();

            // Register dependencies for ChatGPTModel
            var modelPath = "your_model_path_here"; // Specify your model path
            var vocabulary = new List<string>(); // Initialize your vocabulary list
            services.AddSingleton(modelPath);
            services.AddSingleton(vocabulary);

            // Register ChatGPTModel
            services.AddSingleton<IChatGPTModel, ChatGPTModel>();


            services.AddCors(options =>
            {
                options.AddPolicy(name: AuthOptions.CORSPolicy, builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()   
                    .AllowAnyMethod();
                });
            });
            // Add services to the container.
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                var version = Configuration.GetSection("Version");
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BuildingInspection API",
                    Version = version?.Value ?? "1.0.0", //provider.ApiVersionDescriptions.LastOrDefault()?.ApiVersion.ToString(),
                    Description = ""
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    Description = "Bearer Authentication with JWT Token"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Token",
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    }, new List <string>()
                }});
                c.MapType<TimeSpan?>(() => new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("00:00:00")
                });
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "ChatGPT.Api.xml");
                if (File.Exists(filePath))
                {
                    c.IncludeXmlComments(filePath);
                }
            });
        }
        public void Configure(WebApplication app)
        {


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CHATGPT Model API v1");
                c.RoutePrefix = "";
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

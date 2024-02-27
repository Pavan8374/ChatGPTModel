using ChatGPT.API;
using ChatGPT.API.Interfaces;
using ChatGPT.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<IChatGPTModel, ChatGPTModel>();

var startup = new Startup(builder.Configuration, builder.Environment);
startup.ConfigureServices(builder.Services);




var app = builder.Build();
startup.Configure(app);

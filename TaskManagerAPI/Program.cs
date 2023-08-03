using TaskManager.Data.Data;
using NLog;
using TaskManagerAPI.Extensions;
using TaskManager.Service.Service;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
LoggerManager logger = new LoggerManager();
builder.Services.ConfigureResponseCaching();
builder.Services.RegisterDependencies();
builder.Services.ConfigureMapping();
builder.Services.ConfigureLoggerService();

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();

//builder.Services.AddDbContext<RepositoryContext>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureControllers();
builder.Services.AddCors(z => {
    z.AddPolicy("Local", x => {
        x.AllowAnyOrigin();
        x.AllowAnyHeader();
        x.AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("Local");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



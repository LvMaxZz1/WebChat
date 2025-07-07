var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;
service.AddControllers().AddControllersAsServices();
service.AddRouting(x => { x.LowercaseUrls = true; });
service.AddEndpointsApiExplorer();
service.AddSwaggerGen();
service.AddCors(x => x.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();
app.Run();
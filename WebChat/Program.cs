var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;
service.AddControllers().AddControllersAsServices();
service.AddRouting(x => { x.LowercaseUrls = true; });
service.AddEndpointsApiExplorer();
service.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
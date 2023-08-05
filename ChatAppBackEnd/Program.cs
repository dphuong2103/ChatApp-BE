using ChatAppBackEnd.Hubs;
using ChatAppBackEnd.Startup;
var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterServices(builder);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}
app.UseSwagger();
app.UseSwaggerUI();
//var host = new WebHostBuilder().UseUrls("http://0.0.0.0:8080");
app.UseCors(DependencyInjectionSetup.AllowCorsOrigin);
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapHub<ChatHub>("/chatHub");
app.MapControllers();

app.Run();

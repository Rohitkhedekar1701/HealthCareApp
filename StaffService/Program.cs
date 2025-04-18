using Microsoft.EntityFrameworkCore;
using StaffService.Context;
using StaffService.HelperClass;

var builder = WebApplication.CreateBuilder(args);
//Db context 
builder.Services.AddDbContext<StaffDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PatientDbConnection")));


// Add services to the container.
builder.Services.AddSingleton<MessagePublisher>();
builder.Services.AddScoped<StaffServiceHandler>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

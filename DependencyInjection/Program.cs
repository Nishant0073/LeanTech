using DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TestService>();
/*
{
  "service1": "99b5f491-92d1-426f-9019-4d26b196196d",
  "service2": "ff3b3035-9e0a-4ec8-adfd-7b1046a7da79"
}*/

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();

app.Run();

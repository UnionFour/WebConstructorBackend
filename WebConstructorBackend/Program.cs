using WebConstructorBackend;
using WebConstructorBackend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();
builder.Services.AddHostedService<TemplateInstallerService>();

builder.Services.Configure<ConstructorOptions>(
	builder.Configuration.GetSection(ConstructorOptions.Name));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
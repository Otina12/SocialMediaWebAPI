using SocialMedia.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureUnitOfWork();
builder.Services.ConfigureServices();
builder.Services.ConfigureSqlServer(builder.Configuration);
builder.Services.AddApplication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
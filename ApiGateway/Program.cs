using ApiGateway;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Ocelot
builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    config
        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        .AddJsonFile(path: "apigateway.json", optional: false, reloadOnChange: true);
});
builder.Services.AddOcelot();
builder.Services.ConfigureDownstreamHostAndPortsPlaceholders(builder.Configuration);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CORS: Allow any Origin
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
        policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod())
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.UseOcelot();


app.Run();

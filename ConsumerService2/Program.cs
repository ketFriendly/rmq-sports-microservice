using dotenv.net;
using ConsumerService2.RMQ;
using ConsumerService2.Services;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: false));
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSingleton<IMessageConsumer, MessageConsumerService>();
builder.Services.AddSingleton<IMatchesCountListSingleton>(serviceProvider => MatchesCountListSingleton.Instance);
builder.Services.AddHostedService<RabbitMQConsumerService>();

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


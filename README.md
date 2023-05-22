# RabbitMQ Sports Microservices
## Description
Two .NET 7.0 applications that communicate via RabbitMQ.
One acts as a producer, queries the DB(MSQL), forms the message and publishes it to "matches" queue. 
The consumer receives the messages one by one and saves them to file/s, until it receives a ```stop``` message that signals it to unsubscribe from the queue and shutdown. 

## Installation
```bash 
dotnet restore 
```
## Start RMQ
``` bash 
docker run -p 5672:5672 -p 15672:15672 rabbitmq:management
```
## Running the app
Adjust the Consumer .env file (see .env.example).
```bash
dotnet run --project ProducerService1/ProducerService1.csproj --launch-profile https
dotnet run --project ConsumerService2/ConsumerService2.csproj --launch-profile https
```
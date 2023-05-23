# RabbitMQ Sports Microservices
## Feature description
Scaled the app so that the producer publishes to a shared queue linked to the ```my_headers_exchange``` which consumer will check for messages. Upon startup consumer instance/s will check the shared queue for a message and use that message's header ```match_id``` value to create an exclusive queue and link it to the ```my_headers_exchange```. This should enable that when you start another instance of the consumer service, services will not process messages for the same match.

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
or navigate to the consumer directory and run the following command to start multiple instances of the service.
```bash
dotnet run --urls "http://127.0.0.1:0"
```
## To do:
* add error handling
* make this feature work
* implement new consumer shutdown logic 
* extract RMQ configs from classes to configuration 
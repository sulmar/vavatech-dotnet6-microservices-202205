# Shopper

## Projekty

| Projekt  	|   	|
|---	|---	|
| CustomerService.Api  	| Web Api MVC  	|
| PaymentService.Api  	| Ardalis.ApiEndpoints  	|
| ProductService.Api   	| Minimal API  	|
| OCRService.Api   	| Middleware  	|
| TrackingService.Api 	| Signal R  	|
| DeliveryService.Api 	| gRPC Server  	|
| ShoppingCartService.Api | REDIS |

## REDIS

- Utworzenie kontenera REDIS
~~~
docker run --name vavatech-redis -d -p 6379:6379 redis
~~~

- Uruchomienie konsoli redis-cli
~~~
docker exec -it vavatech-redis redis-cli
~~~

- Weryfikacja
~~~
ping
~~~

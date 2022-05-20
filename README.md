# Shopper


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

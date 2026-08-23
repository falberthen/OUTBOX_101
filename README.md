![Build](https://github.com/falberthen/Outbox_101/actions/workflows/outbox101-build.yml/badge.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/falberthen/OUTBOX_101/blob/master/LICENSE)


## Outbox 101
This project is an implementation of the transactional outbox pattern to guarantee message delivery between microservices. 

```bash
src
├── Outbox_101.EventConsumer
├── Outbox_101.EventProducer
├── Outbox_101.Domain
├── Outbox_101.Domain.Tests
├── Outbox_101.Infrastructure
├── Outbox_101.Infrastructure.Kafka
├── Outbox_101.Infrastructure.Tests
└── Outbox_101.Infrastructure.Workers
```

<br>

## Technologies used

<ul>
	<li><a href='https://dotnet.microsoft.com/en-us/download/dotnet/10.0' target="_blank">.NET 10</a> and 
	<a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx' target="_blank">C# 14</a></li>
	<li>Kafka (Confluent.Kafka 2.15)</li>
	<li>Entity Framework Core 10 (Npgsql 10.0.3)</li>  
	<li>Postgres</li>  
	<li>MediatR 14</li>
	<li>XUnit / NSubstitute 6</li>
	<li>Docker Compose</li>
	<li>Debezium (optional)</li>
</ul>

<br/>

## What do you need to run 

- The latest <a href="https://dotnet.microsoft.com/download" target="_blank">.NET Core SDK</a>.
- Download Docker: <a href="https://docs.docker.com/docker-for-windows/wsl/" target="_blank">Docker Desktop with support for WLS 2</a>
    
Using a terminal, run:

```console
 $ docker compose up
``` 

You can also set the `docker-compose.dcproj` as a Startup project on Visual Studio if you want to run it while debugging. 
<br/>

## Running tests

```console
 $ dotnet test
```
<br/>

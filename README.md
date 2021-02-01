# Carguero Challenge
This is project for carguero challenge.

## Build
For build this project is necessary install .net core 3.1
[.Net Core SDK](https://dotnetsdk.ms.com).

For build runing command:

    dotnet build

## Test
For running tests run a command belong:
    dotnet tests

## Docker
This project as default running on docker, provider a envrionments belog:

    GOOGLE_MAPS_KEY=KEY_VALUE

* Using docker-compose
    
    docker-compose up --build -d

## Api documentation

### Users
* POST - users

    {
        "username"
    }

* GET - users/{id}

* POST - addresses

    {
        "street": "string",
        "zipCode: "string",
        "number": "string",
        "complement": "string",
        "district": "string"
    }
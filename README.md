# GuildCars
![Github](https://img.shields.io/github/last-commit/think-small/guildcars?color=blue)
![Github](https://img.shields.io/github/repo-size/think-small/guildcars)
![Github](https://img.shields.io/badge/.NET-4.7.2-blue)

Car dealership web application to manage customer experiences, facilitate vehicle transactions, and track sale metrics.

## Installation
```
git clone https://github.com/think-small/guildcars.git

cd GuildCars.UI

mkdir -p Images/Cars/{Honda/{Accord, Civic, Fit}, Mazda/{CX-3, CX-5}}
```
The GuildCars.UI project will need to include an ```Images``` folder with a subdirectory structure resembling ```Images/Cars/{Make_of_Car}/{Model_of_Car}```.
The above code snippet should be modified to reflect the car dealership's vehicle inventory. A default image for vehicles should be included as ```Images/Cars/no-photos.jpg```.

Note that the "Images" folder was not checked in to version control to prevent having a multitude of binary files polluting the repository.

## Solution Architecture
GuildCars was initially built as a monolith for speed of development.  AutoFac was used for dependency injection to facilitate a layered architecture. AutoFac is configured in ```GuildCars.UI/App_Start/ContainerConfig.cs```.
Service modules as well as a JSON configuration file were used for registering types in the services and data layers.

The service layer follows a facade design pattern to prevent clients from directly interacting with the various classes implementing the business logic. As such, the classes
containing the business logic are marked with internal access modifiers and only a few interfaces are provided for clients to utilize.

The choice was made to encapsulate data access logic even though Entity Framework 6 was used. While the argument that Entity Framework itself is a repository and doesn't need further
abstraction is valid, the benefits gained from seprating its use from the rest of the application outweighed the redundant abstraction.  This allowed for greater maintainability
and testability of the codebase.

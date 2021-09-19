# GuildCars
![Github](https://img.shields.io/github/last-commit/think-small/guildcars?color=blue)
![Github](https://img.shields.io/github/repo-size/think-small/guildcars)
![Github](https://img.shields.io/badge/.NET-4.7.2-blue)

Car dealership web application to manage customer experiences, facilitate vehicle transactions, and track sale metrics.

## Installation
```
git clone https://github.com/think-small/guildcars.git

cd GuildCars.UI

mkdir -p Images/{AboutCards,AboutGrid,Banner,Cars/{Honda/{Accord, Civic, Fit}, Mazda/{CX-3, CX-5}}}
```
### Images
The GuildCars.UI project will need to include an ```Images``` folder with a subdirectory structure resembling the following tree. The code snippet above should be modified to reflect the car dealership's vehicle inventory. A default image for vehicles should be included as ```Images/Cars/no-photos.jpg```.
```bash
├───Images
│   ├───AboutCards
│   ├───AboutGrid
│   ├───Banner
│   └───Cars
│       ├───Honda
│       │   ├───Accord
│       │   ├───Civic
│       │   └───Fit
│       └───Mazda
│           ├───CX-3
│           └───CX-5
```
Note that the ```Images``` folder was not checked in to version control to prevent having a multitude of binary files polluting the repository. However, as the project is
migrated to using Azure DevOps for CI/CD a default set of images will likely be uploaded to the repository in the future.

### Config Files
This application is currently using local App.config files in each project.  Unfortunately, this requires generation of these config files after cloning the repository. The next iteration will implement UserSecrets and thus only require generation of a single secrets.xml file. This will ultimately be moved to Azure KeyVault as the application is moved to the cloud.

## Solution Architecture
GuildCars was initially built as a monolith for speed of development.  AutoFac was used for dependency injection to facilitate a layered architecture. AutoFac is configured in ```GuildCars.UI/App_Start/ContainerConfig.cs```.
Modules as well as a JSON configuration file were used for registering types in the service and data layers.

The service layer follows a facade design pattern to prevent clients from directly interacting with the various classes implementing the business logic. As such, the classes
containing the business logic are marked with internal access modifiers and only a few interfaces are provided for clients to utilize.

The choice was made to encapsulate data access logic even though Entity Framework 6 was used. While the argument that Entity Framework itself is a repository and doesn't need further abstraction is valid, the benefits gained from separating its use from the rest of the application outweighed the redundant abstraction.  This allowed for greater maintainability and testability of the codebase.

## Features
#### Vehicle Transactions
A primary requirement of the project is the ability to properly process the sale of vehicles. Supported payment methods needed to include cash sale, external bank finance, and dealer finance. Additionally, every type of sale needed to accommodate the possible inclusion of trade-in vehicles.  Every payment method would result in the generation of a purchase agreement as a pdf file. External bank finance options require the employee to upload a copy of the customer's approval letter from the financing institution. Dealer finance sales require the generation of an amortized repayment schedule.

IronPdf was utilized for the creation of the purchase agreement. Content of the purchase agreement was built with an HTML string, and then converted using IronPdf's HtmlToPdf renderer. The implementation can be found in ```GuildCars.Services.ReceiptGeneratorService.IronPdfReceiptService.cs```.

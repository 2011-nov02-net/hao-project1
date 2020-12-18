# PROJECT NAME: MVC Techland
## Project Description
MVC Techland is a website that facilitates showcasing and vending Techland video games and other merchandises. Its current focus is to advertise for Dying Light 2 and help customers pre-order this new franchise. Because of its ease of use, customers are able to view and purchase all products Techland carries at anytime anywhere. The design pattern follows a standard Model-View-Controller structure where Model is programmed in .Net5, View and Controller are handled in ASP .Net. This project has been temporarily paused because other team project has taken precedence, but it will be updated continuously until the release date. Feel free to use a regular user's login credential JSmith@gmail.com and 52640JSmith, as well as an admin's login credential haoyang439@gmail.com and 52640HYang to access the website at https://hao-techland2.azurewebsites.net. 

## Technologies Used
* .Net Framework 5.0
* ASP .Net Core  3.1
* Azure SQL Sever
* GitHub
* Azure DevOps
* SonarCloud 
* Docker

## Features
* Authentication for regular users and admins setup in Azure Cloud Service
* Client-side validation, server-side validation, anti-forgery tokens setup to provide validation each step along the way and prevent CSRF and overposting
* UI logic that simulates a standard e-commerce website e.g. Amazon.com

## To-do list:
* Split store repository into product, user, order, store repositories, and reduce duplicated logic
* Incorporate asynchronicity into database access, network logic, and unit testing  
* Provide more unit tests on Model and Controller components to reach higher code coverage

## Getting Started
* No commands and setup needed, access it here at https://hao-techland2.azurewebsites.net/ using the credentials mentioned in project description. 

## Usage
* A regular user after his login credential has been verified, is able to select a store and view all products the store currently has in its inventory. He can select a product, put them in a cart, and return to shopping or other activities. His cart will not be emptied out as long as he remains logged in. All purchases made are kept permanently in a remote database and the user is able to view every single order in detail.
* An admin after getting authenticated, is able to select a location where he is authorized and manage all aspects of the store. He can retrieve, add, edit, and delete products, customers, orders, and all changes are made permanently at that specific location.

## Contributors

## License
This project uses the following license: MIT License.

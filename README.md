# Talabat API Backend (ASP.NET Framework)

Welcome to the Ecommerce API Backend repository! This project serves as the backend for an ecommerce platform built using ASP.NET Framework. Below, you'll find an overview of the architecture, features, and usage instructions.

<br>

## Architecture Overview

The project follows a 4-tier layered architecture known as the Onion Architecture. The layers include:

<br>

1- Core: Contains the business logic and domain models.
<br>
2- API: Handles incoming HTTP requests and acts as the entry point to the application.
<br>
3- Repository: Responsible for data access and interacts with the database.
<br>
4- Services: Implements various services such as Token Service for authentication and payment processing using Stripe gateway.

<br>

Additionally, the project utilizes the Unit of Work pattern along with a Generic Repository for efficient database operations.

<br>

## Features

* Specification Design Pattern: Enables creating queries in a generic way, allowing dynamic inclusion of brands and types in product APIs.
* Token Service: Generates tokens upon user login for authentication and access to protected endpoints.
* Stripe Gateway Integration: Facilitates payment processing, allowing users to buy items securely.
* DTO (Data Transfer Objects): Ensures data protection and adds a layer between users and the database for enhanced security.
* Controller Endpoints: Provides endpoints for managing products, brands, types, orders, payments, accounts (registration, login, user retrieval), and handling errors.
* Redis Server Integration: Utilized for basket functionality, enabling users to add items to their basket before login and checkout securely.


## Usage

1- Authentication: Users need to register and login to place orders. Tokens are generated upon login for authentication.
<br>
2- Basket Functionality: Users can add items to their basket even without logging in, thanks to Redis server integration.
<br>
3- Order Placement: When a user creates an order, it is compared with the basket to ensure accuracy before proceeding to the payment gateway for checkout.
<br>
4- Dashboard (MVC Project): An accompanying MVC project allows CRUD operations on products, user and role management, brand addition, and product display.



<br>

## Getting Started

To set up the project locally, follow these steps:

1- Clone the repository.
<br>
2- Ensure you have the necessary dependencies installed (ASP.NET Framework, Redis server, Stripe SDK).
<br>
3- Configure the database connection string and other necessary settings in the project.
<br>
4- Build and run the project.


<br>

## License

This project is licensed under the MIT License.

Thank you for using the Ecommerce API Backend. If you have any questions or issues, please don't hesitate to reach out, 
Email: badreldin6021@gmail.com




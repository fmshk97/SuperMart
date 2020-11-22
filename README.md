# SuperMart Backend Service

Backend service for SuperMart. <br>
SuperMart backend service allows managing stores and their products.
It also allows to search products. <br>

## APIs

- **/stores** - Allows to GET details of all the stores. Also, it allows to register a new store (POST).
- **/stores/{store-name}** - Allows to GET details, update details (PUT), and DELETE details of the specified store.
- **/stores/{store-name}/products** - Allows to GET all the products of the specified store. Also, it allows to add a product (POST) to the specified store.
- **/stores/{store-name}/products/{product-name}** - Allows to GET and DELETE a product from the specified store.
- **/products** - Allows to GET details of all the products from all the stores.
- **/products?searchText={search-text}** - Allows to search products by product name (GET). <br>

### API Input Constraints (Validations in place)
- Store name must be unique
- Product name must be unique for a given store

## Technical Details

- *Programming Language*: C#
- *Framework*: .Net Core 3.1
- *Database*: MySQL 8.0

## Libraries and Dependencies (NuGet Packages)
- Microsoft.AspNetCore.Mvc.NewtonsoftJson
- Swashbuckle.AspNetCore
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.Relational
- Microsoft.EntityFrameworkCore.Tools
- MySql.Data
- MySql.Data.EntityFrameworkCore
- Pomelo.EntityFrameworkCore.MySql

## Database
MySQL 8.0 has been used for storing data.

### EER Diagram
![alt text](https://github.com/fmshk97/SuperMart/blob/master/Diagrams/EER-diagram.png?raw=true)

## Security
No security has been considered in this project. All the APIs have anonymous access.

## Testing
No unit or integration tests are currently present in the project.

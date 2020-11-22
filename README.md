# SuperMart Backend

Backend system for SuperMart. <br>
SuperMart is a platform for stores to maintain and manage their products.
It also allows customers to search products. <br>

## APIs

- **/stores** - Allows to GET details of all the stores. Also, it allows to register a new store (POST).
- **/stores/{store-name}** - Allows to GET details, update details (PUT), and DELETE details of a store having the provided store name.
- **/stores/{store-name}/products** - Allows to GET all the products of the specified store. Also, it allows to add a product (POST) to the specified store.
- **/stores/{store-name}/products/{product-name}** - Allows to GET and DELETE a product from the specified store.
- **/products** - Allows to GET details of all the products from all the stores.
- **/products?searchText={search-text}** - Allows to search products by product name (GET). <br>

### API Input Constraints (Validations already in place)
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

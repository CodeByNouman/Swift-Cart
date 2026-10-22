# SwiftCart: Week 6 build (Sprint 1 + Sprint 2), MySQL version

This version matches the proposal up to **Sprint 2 (Weeks 5-6)**. The database is the **final complete design**; only the parts needed by Sprints 1 and 2 are wired up.

## What works (demo this)
| Sprint | Result |
|---|---|
| Sprint 1: Setup and user accounts | Bootstrap layout with a top navigation bar. A seller can **sign up**, **sign in**, and see their store name in the top bar. Sign out works. |
| Sprint 2: Product display | The homepage **loads real products from SQL Server** and shows them in responsive Bootstrap cards (title, price, category, stock badge, expiry date for products that have one). Works on laptop and phone widths. |

Demo login: **demo@swiftcart.pk / Demo@1234**

## In the database but not active yet (later sprints)
Tables for Categories, Sellers, Products, Orders and OrderItems all exist (see `Database/swiftcart_mysql.sql`), including the product expiry columns. Not wired up yet:
- Sprint 3: search, category filter, product detail page
- Sprint 4: shopping cart
- Sprint 5: checkout, orders, stock reduction, receipt
- Sprint 6: seller/admin panel (add, edit, delete products, order status)

## Run it (MySQL version)
1. Make sure **MySQL Server** is running (Windows: Win+R, `services.msc`, look for **MySQL80** with status Running).
2. Open `appsettings.json` and replace `YOUR_MYSQL_PASSWORD` with the root password you set when installing MySQL.
3. Press F5 in Visual Studio (or run `dotnet run`). The database `swiftcartdb` and demo data are created on the first run.
4. To look at the data: open MySQL Workbench, connect, and open **swiftcartdb > Tables**.

## API in this version
| Route | Purpose |
|---|---|
| `GET /api/products` | Homepage products (hides expired ones) |
| `POST /api/auth/register` | Create a seller account |
| `POST /api/auth/login`, `POST /api/auth/logout`, `GET /api/auth/me` | Cookie login |

## Folder map
`Program.cs` (startup) · `Models/` (tables as C# classes, DTOs) · `Data/` (database context and demo data) · `Controllers/` (`AuthController`, `ProductsController`) · `Database/swiftcart_mysql.sql` · `wwwroot/` (`index.html`, `signup.html`, `signin.html`, `assets/`)

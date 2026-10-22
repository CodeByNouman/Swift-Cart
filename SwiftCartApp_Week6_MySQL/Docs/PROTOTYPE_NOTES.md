# Swift Cart prototype notes (Task 3)

Open `index.html` in a browser. No backend: dummy data lives in `assets/app.js`; cart and seller session use localStorage.

## Pages by module
| Module | Page | Type |
|---|---|---|
| Browsing | index.html (Shop) | Dynamic (search, filter, sort) |
| Browsing | product.html?id= | Dynamic (built from product id) |
| Cart & orders | cart.html | Interactive (qty, remove, live totals) |
| Cart & orders | checkout.html + receipt | Interactive (validated form) |
| Seller auth | signup.html (Create seller account) | Interactive (validation, strength bar) |
| Seller auth | signin.html | Interactive |
| Seller dashboard | dashboard.html | Interactive (product CRUD, order status) |

## Fields and constraints
| Form | Field | Rule |
|---|---|---|
| Sign up | Store name | Required, 3–40 chars |
| Sign up | Owner name | Required, letters/spaces, 3–50 |
| Sign up | Email | Required, email format, unique per store (enforced by backend later) |
| Sign up | Mobile | Required, 03XX-XXXXXXX |
| Sign up | Category, City | Required |
| Sign up | Password | Required, 8+ chars, letter + number |
| Sign up | Confirm password | Required, must match |
| Sign up | Terms | Required checkbox |
| Sign in | Email, Password | Required, email format, 8+ chars |
| Checkout | Name | Required, letters/spaces, 3–50 |
| Checkout | Phone | Required, 03XX-XXXXXXX |
| Checkout | Email | Optional, email format |
| Checkout | Address | Required, 10–150 chars |
| Checkout | City / Postal code | Required / exactly 5 digits |
| Product form | Name | Required, 3–60 |
| Product form | Price | Required, number 1–1,000,000 |
| Product form | Stock | Required, whole number 0–9,999 |
| Product form | Category | Required |
| Product form | Image, Description | Optional (JPG/PNG, max 200 chars) |

Validation uses HTML5 constraint attributes plus Bootstrap `was-validated` feedback; password match is a small JS check.

## Requirement mapping
Browsing (US1): shop page. Cart (US2): cart page. Checkout (US3): checkout + receipt. Inventory (US4): dashboard Products tab. Order tracking (US5): dashboard Orders tab. Extras: stock-aware buttons, password strength bar, print receipt, sort, toasts.

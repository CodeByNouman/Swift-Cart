/* SwiftCart database (MySQL 8). For your presentation or to look at in MySQL Workbench.
   You do NOT need to run this: the app creates the same tables automatically on first run. */

CREATE DATABASE IF NOT EXISTS swiftcartdb CHARACTER SET utf8mb4;
USE swiftcartdb;

CREATE TABLE Categories (
    Id    INT AUTO_INCREMENT PRIMARY KEY,
    Name  VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE Sellers (
    Id            INT AUTO_INCREMENT PRIMARY KEY,
    StoreName     VARCHAR(40)  NOT NULL,
    OwnerName     VARCHAR(50)  NOT NULL,
    Email         VARCHAR(100) NOT NULL UNIQUE,
    Phone         VARCHAR(20)  NOT NULL,
    City          VARCHAR(30)  NOT NULL,
    PasswordHash  LONGTEXT     NOT NULL,
    CategoryId    INT NOT NULL,
    CreatedAt     DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

CREATE TABLE Products (
    Id           INT AUTO_INCREMENT PRIMARY KEY,
    SellerId     INT NOT NULL,
    CategoryId   INT NOT NULL,
    Name         VARCHAR(60)   NOT NULL,
    Description  VARCHAR(200)  NOT NULL DEFAULT '',
    Price        DECIMAL(10,2) NOT NULL,
    Stock        INT NOT NULL,
    Emoji        VARCHAR(10)   NOT NULL DEFAULT '📦',
    BgColor      VARCHAR(10)   NOT NULL DEFAULT '#eef1f7',
    HasExpiry    TINYINT(1) NOT NULL DEFAULT 0,
    ExpiryDate   DATE NULL,
    IsActive     TINYINT(1) NOT NULL DEFAULT 1,
    CreatedAt    DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    RowVersion   DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
    FOREIGN KEY (SellerId)   REFERENCES Sellers(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    CONSTRAINT CK_Products_Price  CHECK (Price > 0),
    CONSTRAINT CK_Products_Stock  CHECK (Stock >= 0),
    CONSTRAINT CK_Products_Expiry CHECK ((HasExpiry = 0 AND ExpiryDate IS NULL) OR (HasExpiry = 1 AND ExpiryDate IS NOT NULL))
);

CREATE TABLE Orders (
    Id             INT AUTO_INCREMENT PRIMARY KEY,
    OrderNumber    VARCHAR(20)   NOT NULL UNIQUE,
    CustomerName   VARCHAR(50)   NOT NULL,
    Phone          VARCHAR(20)   NOT NULL,
    Email          VARCHAR(100)  NULL,
    Address        VARCHAR(150)  NOT NULL,
    City           VARCHAR(30)   NOT NULL,
    PostalCode     CHAR(5)       NOT NULL,
    PaymentMethod  VARCHAR(30)   NOT NULL,
    Subtotal       DECIMAL(12,2) NOT NULL,
    Tax            DECIMAL(12,2) NOT NULL,
    Shipping       DECIMAL(12,2) NOT NULL,
    Total          DECIMAL(12,2) NOT NULL,
    Status         VARCHAR(20)   NOT NULL DEFAULT 'Pending',
    CreatedAt      DATETIME(6)   NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    CONSTRAINT CK_Orders_Status CHECK (Status IN ('Pending','Shipped','Delivered'))
);

CREATE TABLE OrderItems (
    Id           INT AUTO_INCREMENT PRIMARY KEY,
    OrderId      INT NOT NULL,
    ProductId    INT NOT NULL,
    SellerId     INT NOT NULL,
    ProductName  VARCHAR(60)   NOT NULL,
    UnitPrice    DECIMAL(10,2) NOT NULL,
    Quantity     INT NOT NULL,
    FOREIGN KEY (OrderId)   REFERENCES Orders(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (SellerId)  REFERENCES Sellers(Id),
    CONSTRAINT CK_OrderItems_Quantity CHECK (Quantity > 0)
);

INSERT INTO Categories (Name) VALUES ('Electronics'), ('Clothes'), ('Home'), ('Beauty');

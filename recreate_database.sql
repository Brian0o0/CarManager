-- Recreate CarManagement database completely
USE [master]
GO

-- Drop database if exists
IF EXISTS(SELECT * FROM sys.databases WHERE name = 'CarManagement')
BEGIN
    ALTER DATABASE [CarManagement] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE [CarManagement]
    PRINT 'Old CarManagement database dropped'
END
GO

-- Create new database
CREATE DATABASE [CarManagement]
GO

PRINT 'CarManagement database created successfully'
GO

USE [CarManagement]
GO

-- Create Users table
CREATE TABLE [Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Username] [nvarchar](50) NOT NULL UNIQUE,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL UNIQUE,
	[PhoneNumber] [nvarchar](20) NULL,
	[Address] [nvarchar](255) NULL,
	[UserType] [nvarchar](20) NULL DEFAULT ('Buyer'),
	[RegistrationDate] [date] NULL DEFAULT (getdate())
)
GO

-- Create CarTypes table
CREATE TABLE [CarTypes](
	[CarTypeID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[TypeName] [nvarchar](50) NOT NULL UNIQUE,
	[Description] [text] NULL
)
GO

-- Create Cars table
CREATE TABLE [Cars](
	[CarID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Make] [nvarchar](50) NOT NULL,
	[Model] [nvarchar](50) NOT NULL,
	[ManufactureYear] [int] NOT NULL,
	[CarTypeID] [int] NULL,
	[Color] [nvarchar](30) NULL,
	[Mileage] [int] NULL,
	[LicensePlate] [nvarchar](20) NULL UNIQUE,
	[AskingPrice] [decimal](18, 2) NOT NULL,
	[Description] [text] NULL,
	[Status] [nvarchar](20) NULL DEFAULT ('Available'),
	[ListingDate] [date] NULL DEFAULT (getdate()),
	[SellerID] [int] NULL,
	[CarName] [nvarchar](50) NOT NULL,
	FOREIGN KEY([CarTypeID]) REFERENCES [CarTypes]([CarTypeID]),
	FOREIGN KEY([SellerID]) REFERENCES [Users]([UserID])
)
GO

-- Create Transactions table
CREATE TABLE [Transactions](
	[TransactionID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CarID] [int] NOT NULL,
	[BuyerID] [int] NOT NULL,
	[TransactionDate] [datetime] NULL DEFAULT (getdate()),
	[SellingPrice] [decimal](18, 2) NOT NULL,
	[TransactionStatus] [nvarchar](20) NULL DEFAULT ('Completed'),
	FOREIGN KEY([BuyerID]) REFERENCES [Users]([UserID]),
	FOREIGN KEY([CarID]) REFERENCES [Cars]([CarID])
)
GO

PRINT 'Tables created successfully'
GO

-- Add Windows user with full permissions
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'DESKTOP-9IOAPNJ\NMK')
BEGIN
    CREATE USER [DESKTOP-9IOAPNJ\NMK] FOR LOGIN [DESKTOP-9IOAPNJ\NMK]
    PRINT 'User DESKTOP-9IOAPNJ\NMK created'
END
GO

ALTER ROLE [db_owner] ADD MEMBER [DESKTOP-9IOAPNJ\NMK]
GO

PRINT 'Permissions granted to DESKTOP-9IOAPNJ\NMK'
GO

-- Insert sample users
INSERT INTO [Users] ([Username], [PasswordHash], [FullName], [Email], [PhoneNumber], [Address], [UserType], [RegistrationDate]) VALUES 
('seller1', 'hashed_pass_seller1', 'Nguyen Van A', 'nguyenvana@example.com', '0901234567', '123 Le Loi, Quan 1, TP.HCM', 'Seller', '2025-06-25'),
('buyer1', 'hashed_pass_buyer1', 'Tran Thi B', 'tranthib@example.com', '0907654321', '456 Tran Hung Dao, Quan 5, TP.HCM', 'Buyer', '2025-06-25'),
('seller2', 'hashed_pass_seller2', 'Le Van C', 'levanc@example.com', '0912345678', '789 Nguyen Trai, Quan 1, TP.HCM', 'Seller', '2025-06-25'),
('buyer2', 'hashed_pass_buyer2', 'Pham Thi D', 'phamthid@example.com', '0918765432', '101 Vo Van Tan, Quan 3, TP.HCM', 'Buyer', '2025-06-25'),
('admin1', 'hashed_pass_admin1', 'Hoang Quoc E', 'hoangquoce@example.com', '0987654321', 'Admin Office, TP.HCM', 'Admin', '2025-06-25')
GO

-- Insert car types
INSERT INTO [CarTypes] ([TypeName], [Description]) VALUES 
('Sedan', 'A passenger car in a three-box configuration'),
('SUV', 'Sport utility vehicle'),
('Hatchback', 'A car with rear door access to cargo area'),
('Truck', 'A motor vehicle designed to transport cargo'),
('Coupe', 'A two-door car with fixed roof')
GO

-- Insert sample cars
INSERT INTO [Cars] ([Make], [Model], [ManufactureYear], [CarTypeID], [Color], [Mileage], [LicensePlate], [AskingPrice], [Description], [Status], [ListingDate], [SellerID], [CarName]) VALUES 
('Toyota', 'Camry', 2018, 1, 'White', 75000, '51F-123.45', 650000000.00, 'Good condition, regular maintenance', 'Available', '2024-05-10', 1, 'Toyota Camry'),
('Honda', 'CR-V', 2020, 2, 'Black', 40000, '51G-678.90', 800000000.00, 'Like new, full service history', 'Available', '2024-05-15', 1, 'Honda CR-V'),
('BMW', 'X5', 2019, 2, 'Blue', 60000, '51H-111.22', 1200000000.00, 'Luxury SUV, well-maintained', 'Available', '2024-05-20', 3, 'BMW X5'),
('Mazda', 'CX-5', 2017, 3, 'Red', 90000, '51J-333.44', 450000000.00, 'Reliable hatchback', 'Available', '2024-06-01', 3, 'Mazda CX-5'),
('Ford', 'Ranger', 2021, 4, 'Grey', 25000, '51K-555.66', 950000000.00, 'Powerful pickup truck', 'Available', '2024-06-10', 1, 'Ford Ranger')
GO

-- Insert sample transactions  
INSERT INTO [Transactions] ([CarID], [BuyerID], [TransactionDate], [SellingPrice], [TransactionStatus]) VALUES 
(1, 2, '2024-06-20 10:30:00', 640000000.00, 'Completed'),
(2, 4, '2024-06-22 14:00:00', 790000000.00, 'Completed')
GO

PRINT 'Sample data inserted successfully!'
PRINT ''
PRINT 'Database setup completed!'
PRINT 'Test login credentials:'
PRINT 'Email: nguyenvana@example.com'
PRINT 'Password: hashed_pass_seller1'
GO 
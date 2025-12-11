BEGIN TRANSACTION;


----------------------------------------------------
-- Borrar datos de todas las tablas
----------------------------------------------------
DELETE FROM [dbo].[PurchaseItems];
DELETE FROM [dbo].[Purchases];
DELETE FROM [dbo].[PaymentMethod];
DELETE FROM [dbo].[Items];
DELETE FROM [dbo].[ItemTypes];
DELETE FROM [dbo].[Brands];
DELETE FROM [dbo].[AspNetUsers];

----------------------------------------------------
-- Reiniciar los contadores IDENTITY
----------------------------------------------------
DBCC CHECKIDENT ('[dbo].[Brands]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[ItemTypes]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Items]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[PaymentMethod]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Purchases]', RESEED, 0);

----------------------------------------------------
-- AspNetUsers 
----------------------------------------------------
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'1', N'Pepe', N'Gomez', N'Pepe.Gomez', N'Pepe.Gomez', N'pepegomez@example.com', N'pepegomez@example.com', 0, NULL, NULL, NULL, N'684573945', 1, 1, NULL, 0, 0);

INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'2', N'Alejandra', N'Jimenez', N'Alejandra.Jimenez', N'Alejandra.Jimenez', N'Alejandrajimenez@example.com', N'alejandrajimenez@example.com', 0, NULL, NULL, NULL, N'684586744', 1, 1, NULL, 0, 0);

INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'3', N'Javier', N'Hernandez', N'Javier.Hernandez', N'Javier.Hernandez', N'Javierhernandez@example.com', N'Javierhernandez@example.com', 0, NULL, NULL, NULL, N'645686744', 1, 1, NULL, 0, 0);
INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
(N'4', N'Adrian.Sevilla@alu.uclm.es', N'ADRIAN.SEVILLA@ALU.UCLM.ES', N'Adrian.Sevilla@alu.uclm.es', N'ADRIAN.SEVILLA@ALU.UCLM.ES', 1, N'AQAAAAIAAYagAAAAEEwJWcqKoj4QNU8u4/EN1RJse4wBVCXdkwOuqRLRTgZruzrYxiDGuj6lEcgtli2MIQ==', N'E55FBQ3FQMLJ7IWZGSZXNGZZQFROFVY7', N'7fb05bbd-d613-4c09-8f73-288165c4017c', NULL, 0, 0, NULL, 1, 0)

----------------------------------------------------
-- Brands 
----------------------------------------------------
SET IDENTITY_INSERT [dbo].[Brands] ON;

INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (1, N'Nike');
INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (2, N'Adidas');
INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (3, N'Domyos');

SET IDENTITY_INSERT [dbo].[Brands] OFF;

----------------------------------------------------
-- ItemTypes 
----------------------------------------------------
SET IDENTITY_INSERT [dbo].[ItemTypes] ON;
INSERT INTO [dbo].[ItemTypes] ([Id], [Name]) VALUES (1, N'Cardio Equipment')
INSERT INTO [dbo].[ItemTypes] ([Id], [Name]) VALUES (2, N'Strength Equipment')
INSERT INTO [dbo].[ItemTypes] ([Id], [Name]) VALUES (3, N'Accessories')



SET IDENTITY_INSERT [dbo].[ItemTypes] OFF;

----------------------------------------------------
-- Items 
----------------------------------------------------
SET IDENTITY_INSERT [dbo].[Items] ON;

INSERT INTO [dbo].[Items] 
    ([Id], [Description], [Name], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [PurchasePrice], [ItemTypeId], [BrandId]) 
VALUES 
    (1, N'Set of bands', N'Resistance band set', 20, 10, CAST(15.00 AS Decimal(10, 2)), CAST(22.00 AS Decimal(10, 2)), 1, 1);

INSERT INTO [dbo].[Items] 
    ([Id], [Description], [Name], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [PurchasePrice], [ItemTypeId], [BrandId]) 
VALUES 
    (2, N'Foam roller for muscle recovery and massage', N'Foam Roller', 18, 8, CAST(18.00 AS Decimal(10, 2)), CAST(25.00 AS Decimal(10, 2)), 3, 2);

INSERT INTO [dbo].[Items] 
    ([Id], [Description], [Name], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [PurchasePrice], [ItemTypeId], [BrandId]) 
VALUES 
    (3, N'Ideal for strength and endurance training', N'Kettlebell 10 kg', 15, 5, CAST(25.00 AS Decimal(10, 2)), CAST(35.00 AS Decimal(10, 2)), 2, 3);

SET IDENTITY_INSERT [dbo].[Items] OFF;

----------------------------------------------------
-- PaymentMethod 
----------------------------------------------------
SET IDENTITY_INSERT [dbo].[PaymentMethod] ON;

INSERT INTO [dbo].[PaymentMethod] 
    ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) 
VALUES 
    (1, N'1', N'Bizum', 684573945, NULL, NULL, NULL);

INSERT INTO [dbo].[PaymentMethod] 
    ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) 
VALUES 
    (2, N'2', N'CreditCard', NULL, N'4111111111111111', N'2027-12-31 00:00:00', NULL);

INSERT INTO [dbo].[PaymentMethod] 
    ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) 
VALUES 
    (3, N'3', N'PayPal', NULL, NULL, NULL, N'Javierhernandez@example.com');

SET IDENTITY_INSERT [dbo].[PaymentMethod] OFF;

----------------------------------------------------
-- Purchases
----------------------------------------------------
SET IDENTITY_INSERT [dbo].[Purchases] ON;

INSERT INTO [dbo].[Purchases] 
    ([Id], [City], [Country], [Date], [Description], [Street], [Total_prices], [PaymentMethodId]) 
VALUES 
    (1, N'Cuenca', N'Spain', N'2025-10-10 00:00:00', N'Purchase of a foam roller', N'C/Plaza Mayor', CAST(10.00 AS Decimal(10, 2)), 1);

INSERT INTO [dbo].[Purchases] 
    ([Id], [City], [Country], [Date], [Description], [Street], [Total_prices], [PaymentMethodId]) 
VALUES 
    (2, N'Madrid', N'Spain', N'2025-10-06 00:00:00', N'Purchase of resistance band', N'C/Madrid', CAST(50.00 AS Decimal(10, 2)), 1);

SET IDENTITY_INSERT [dbo].[Purchases] OFF;

----------------------------------------------------
-- PurchaseItems 
----------------------------------------------------
INSERT INTO [dbo].[PurchaseItems] 
    ([ItemId], [PurchaseId], [Amount_bought], [Price]) 
VALUES 
    (1, 1, 10, CAST(50.00 AS Decimal(10, 2)));

INSERT INTO [dbo].[PurchaseItems] 
    ([ItemId], [PurchaseId], [Amount_bought], [Price]) 
VALUES 
    (2, 2, 60, CAST(60.00 AS Decimal(10, 2)));


COMMIT TRANSACTION;

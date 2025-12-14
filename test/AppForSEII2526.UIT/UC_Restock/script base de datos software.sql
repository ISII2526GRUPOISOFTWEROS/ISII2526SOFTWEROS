
/* =============================================
   SCRIPT UNIFICADO DE CARGA DE DATOS (SEED)
   Ordenado por dependencias para evitar errores de FK
   ============================================= */

-- 1. INSERTAR USUARIOS (AspNetUsers)
-- Nota: AspNetUsers suele tener PK string (GUID), por lo que NO requiere IDENTITY_INSERT
PRINT 'Insertando AspNetUsers...'
INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES (N'1', N'Pepe', N'Gomez', N'Pepe.Gomez', N'PEPE.GOMEZ', N'pepegomez@example.com', N'PEPEGOMEZ@EXAMPLE.COM', 0, NULL, NULL, NULL, N'684573945', 1, 1, NULL, 0, 0);

INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES (N'2', N'Alejandra', N'Jimenez', N'Alejandra.Jimenez', N'ALEJANDRA.JIMENEZ', N'Alejandrajimenez@example.com', N'ALEJANDRAJIMENEZ@EXAMPLE.COM', 0, NULL, NULL, NULL, N'684586744', 1, 1, NULL, 0, 0);

INSERT INTO [dbo].[AspNetUsers] ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES (N'3', N'Javier', N'Hernandez', N'Javier.Hernandez', N'JAVIER.HERNANDEZ', N'Javierhernandez@example.com', N'JAVIERHERNANDEZ@EXAMPLE.COM', 0, NULL, NULL, NULL, N'645686744', 1, 1, NULL, 0, 0);
GO

-- 2. INSERTAR MARCAS (Brands)
PRINT 'Insertando Brands...'
SET IDENTITY_INSERT [dbo].[Brands] ON;
INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (1, N'Nike');
INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (2, N'Adidas');
INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (3, N'Domyos');
SET IDENTITY_INSERT [dbo].[Brands] OFF;
GO

-- 3. INSERTAR TIPOS DE ITEM (ItemTypes)
PRINT 'Insertando ItemTypes...'
SET IDENTITY_INSERT [dbo].[ItemTypes] ON;
INSERT INTO [dbo].[ItemTypes] ([Id], [Name], [ClassId]) VALUES (1, N'Cardio Equipment', NULL);
INSERT INTO [dbo].[ItemTypes] ([Id], [Name], [ClassId]) VALUES (2, N'Strength Equipment', NULL);
INSERT INTO [dbo].[ItemTypes] ([Id], [Name], [ClassId]) VALUES (3, N'Accessories', NULL);
SET IDENTITY_INSERT [dbo].[ItemTypes] OFF;
GO

-- 4. INSERTAR ITEMS (Items)
-- Dependencia: Brands, ItemTypes
PRINT 'Insertando Items...'
SET IDENTITY_INSERT [dbo].[Items] ON;
INSERT INTO [dbo].[Items] ([Id], [Description], [Name], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [PurchasePrice], [ItemTypeId], [BrandId]) 
VALUES (7, N'Set of bands', N'Resistance band set', 20, 10, CAST(15.00 AS Decimal(10, 2)), CAST(22.00 AS Decimal(10, 2)), 1, 1);

INSERT INTO [dbo].[Items] ([Id], [Description], [Name], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [PurchasePrice], [ItemTypeId], [BrandId]) 
VALUES (12, N'Foam roller for muscle recovery and massage', N'Foam Roller', 18, 8, CAST(18.00 AS Decimal(10, 2)), CAST(25.00 AS Decimal(10, 2)), 3, 2);

INSERT INTO [dbo].[Items] ([Id], [Description], [Name], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [PurchasePrice], [ItemTypeId], [BrandId]) 
VALUES (14, N'Ideal for strength and endurance training', N'Kettlebell 10 kg', 15, 5, CAST(25.00 AS Decimal(10, 2)), CAST(35.00 AS Decimal(10, 2)), 2, 3);
SET IDENTITY_INSERT [dbo].[Items] OFF;
GO

-- 5. INSERTAR RESTOCK (Restock)
-- Dependencia: AspNetUsers
PRINT 'Insertando Restock...'
SET IDENTITY_INSERT [dbo].[Restock] ON;
INSERT INTO [dbo].[Restock] ([Id], [DeliveryAddress], [Description], [ExpectedDate], [RestockDate], [Title], [TotalPrice], [RestockResponsibleId]) 
VALUES (3, N'Casa', N'SinDescripcion', N'2025-10-10 00:00:00', N'2025-12-15 00:00:00', N'Restock1', CAST(100.00 AS Decimal(5, 2)), N'1');

INSERT INTO [dbo].[Restock] ([Id], [DeliveryAddress], [Description], [ExpectedDate], [RestockDate], [Title], [TotalPrice], [RestockResponsibleId]) 
VALUES (5, N'Universidad', N'SinDescripcion', N'2025-11-01 00:00:00', N'2025-11-13 00:00:00', N'Restock2', CAST(200.00 AS Decimal(5, 2)), N'2');

INSERT INTO [dbo].[Restock] ([Id], [DeliveryAddress], [Description], [ExpectedDate], [RestockDate], [Title], [TotalPrice], [RestockResponsibleId]) 
VALUES (6, N'Bar', N'SinDescripcion', N'2025-11-22 00:00:00', N'2025-12-24 00:00:00', N'Restock3', CAST(150.00 AS Decimal(5, 2)), N'3');
SET IDENTITY_INSERT [dbo].[Restock] OFF;
GO

-- 6. INSERTAR DETALLES DE RESTOCK (RestockItem)
-- Dependencia: Items, Restock
-- Nota: Esta tabla suele tener clave compuesta, por lo que NO se usa IDENTITY_INSERT usualmente
PRINT 'Insertando RestockItems...'
INSERT INTO [dbo].[RestockItem] ([ItemId], [RestockId], [Quantity], [RestockPrice]) VALUES (7, 3, 10, CAST(10.00 AS Decimal(5, 2)));
INSERT INTO [dbo].[RestockItem] ([ItemId], [RestockId], [Quantity], [RestockPrice]) VALUES (12, 5, 12, CAST(15.00 AS Decimal(5, 2)));
INSERT INTO [dbo].[RestockItem] ([ItemId], [RestockId], [Quantity], [RestockPrice]) VALUES (14, 6, 15, CAST(10.00 AS Decimal(5, 2)));
GO

PRINT 'Carga de datos finalizada correctamente.'


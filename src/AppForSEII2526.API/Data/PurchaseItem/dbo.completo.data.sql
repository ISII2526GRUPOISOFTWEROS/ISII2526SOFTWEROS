/* ========================================================================
   SCRIPT 1: SEED PURCHASE (TIENDA Y COMPRAS)
   Uso: Prepara la BD para tests de Compra (IDs de items 1, 2, 3)
======================================================================== */

-- 1. LIMPIEZA Y RESETEO
IF OBJECT_ID('dbo.Brands') IS NOT NULL BEGIN TRY SET IDENTITY_INSERT [dbo].[Brands] OFF; END TRY BEGIN CATCH END CATCH;
IF OBJECT_ID('dbo.Classes') IS NOT NULL BEGIN TRY SET IDENTITY_INSERT [dbo].[Classes] OFF; END TRY BEGIN CATCH END CATCH;
IF OBJECT_ID('dbo.ItemTypes') IS NOT NULL BEGIN TRY SET IDENTITY_INSERT [dbo].[ItemTypes] OFF; END TRY BEGIN CATCH END CATCH;
IF OBJECT_ID('dbo.Items') IS NOT NULL BEGIN TRY SET IDENTITY_INSERT [dbo].[Items] OFF; END TRY BEGIN CATCH END CATCH;
IF OBJECT_ID('dbo.PaymentMethod') IS NOT NULL BEGIN TRY SET IDENTITY_INSERT [dbo].[PaymentMethod] OFF; END TRY BEGIN CATCH END CATCH;
IF OBJECT_ID('dbo.Purchases') IS NOT NULL BEGIN TRY SET IDENTITY_INSERT [dbo].[Purchases] OFF; END TRY BEGIN CATCH END CATCH;

BEGIN TRANSACTION;
    -- Borrado en cascada inverso
    DELETE FROM [dbo].[PurchaseItems];
    DELETE FROM [dbo].[RestockItem];
    DELETE FROM [dbo].[PlanItems];
    DELETE FROM [dbo].[Purchases];
    DELETE FROM [dbo].[Restock];
    DELETE FROM [dbo].[Plans];
    DELETE FROM [dbo].[PaymentMethod];
    DELETE FROM [dbo].[Items];
    DELETE FROM [dbo].[ItemTypes];
    DELETE FROM [dbo].[Classes];
    DELETE FROM [dbo].[Brands];
    DELETE FROM [dbo].[AspNetUsers];

    -- Reseteo de contadores
    DBCC CHECKIDENT ('[dbo].[Brands]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[Items]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[PaymentMethod]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[Purchases]', RESEED, 0);
    BEGIN TRY DBCC CHECKIDENT ('[dbo].[Classes]', RESEED, 0); END TRY BEGIN CATCH END CATCH;
    BEGIN TRY DBCC CHECKIDENT ('[dbo].[ItemTypes]', RESEED, 0); END TRY BEGIN CATCH END CATCH;

    -- 2. CARGA DE DATOS BASE (Usuarios, Marcas, Clases Dummy)
    -- Usuarios
 INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'1', N'Pepe', N'Gomez', N'Pepe.Gomez', N'PEPE.GOMEZ', N'pepegomez@example.com', N'PEPEGOMEZ@EXAMPLE.COM', 0, NULL, NULL, NULL, N'684573945', 1, 1, NULL, 0, 0);

-- 2. Alejandra Jimenez
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'2', N'Alejandra', N'Jimenez', N'Alejandra.Jimenez', N'ALEJANDRA.JIMENEZ', N'Alejandrajimenez@example.com', N'ALEJANDRAJIMENEZ@EXAMPLE.COM', 0, NULL, NULL, NULL, N'684586744', 1, 1, NULL, 0, 0);

-- 3. Javier Hernandez
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'3', N'Javier', N'Hernandez', N'Javier.Hernandez', N'JAVIER.HERNANDEZ', N'Javierhernandez@example.com', N'JAVIERHERNANDEZ@EXAMPLE.COM', 0, NULL, NULL, NULL, N'645686744', 1, 1, NULL, 0, 0);

-- 4. Adrian Sevilla (He añadido Name y Surname que faltaban para evitar el error NULL)
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'4', N'Adrian', N'Sevilla', N'Adrian.Sevilla@alu.uclm.es', N'ADRIAN.SEVILLA@ALU.UCLM.ES', N'Adrian.Sevilla@alu.uclm.es', N'ADRIAN.SEVILLA@ALU.UCLM.ES', 1, N'AQAAAAIAAYagAAAAEEwJWcqKoj4QNU8u4/EN1RJse4wBVCXdkwOuqRLRTgZruzrYxiDGuj6lEcgtli2MIQ==', N'E55FBQ3FQMLJ7IWZGSZXNGZZQFROFVY7', N'7fb05bbd-d613-4c09-8f73-288165c4017c', NULL, 0, 0, NULL, 1, 0);

    -- Marcas
    SET IDENTITY_INSERT [dbo].[Brands] ON;
    INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (1, N'Nike'), (2, N'Adidas'), (3, N'Domyos');
    SET IDENTITY_INSERT [dbo].[Brands] OFF;

    -- Clases (Necesarias por FK de ItemTypes, aunque no se usen en Purchase)
    SET IDENTITY_INSERT [dbo].[Classes] ON;
    INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date]) VALUES (1, 10, N'Generic Class', 0, GETDATE());
    SET IDENTITY_INSERT [dbo].[Classes] OFF;

    -- Tipos de Item
    SET IDENTITY_INSERT [dbo].[ItemTypes] ON;
    INSERT INTO [dbo].[ItemTypes] ([Id], [Name], [ClassId]) VALUES 
    (1, N'Cardio Equipment', 1), (2, N'Strength Equipment', 1), (3, N'Accessories', 1);
    SET IDENTITY_INSERT [dbo].[ItemTypes] OFF;

    -- Métodos de Pago (Necesarios para comprar)
    SET IDENTITY_INSERT [dbo].[PaymentMethod] ON;
    INSERT INTO [dbo].[PaymentMethod] ([Id], [UserId], [Discriminator], [TelephoneNumber]) VALUES (1, N'1', N'Bizum', 684573945);
    INSERT INTO [dbo].[PaymentMethod] ([Id], [UserId], [Discriminator], [TelephoneNumber]) VALUES (4, N'4', N'Bizum', 600123456);
    SET IDENTITY_INSERT [dbo].[PaymentMethod] OFF;

    -- 3. DATOS ESPECÍFICOS: ITEMS PARA COMPRA (IDs 1, 2, 3)
    SET IDENTITY_INSERT [dbo].[Items] ON;
    INSERT INTO [dbo].[Items] ([Id], [Description], [Name], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [PurchasePrice], [ItemTypeId], [BrandId]) 
    VALUES 
    (1, N'Set of bands', N'Resistance band set', 9, 10, 15.00, 22.00, 3, 1),
    (2, N'Foam roller for muscle recovery and massage', N'Foam Roller', 9, 8, 18.00, 25.00, 3, 2),
    (3, N'Ideal for strength and endurance training', N'Kettlebell 10 kg', 9, 5, 25.00, 35.00, 2, 3);
    SET IDENTITY_INSERT [dbo].[Items] OFF;

    -- 4. HISTÓRICO DE COMPRAS (Opcional, para tests de listado)
    SET IDENTITY_INSERT [dbo].[Purchases] ON;
    INSERT INTO [dbo].[Purchases] ([Id], [City], [Country], [Date], [Description], [Street], [Total_prices], [PaymentMethodId]) 
    VALUES (1, N'Cuenca', N'Spain', GETDATE(), N'Purchase test', N'C/Mayor', 10.00, 1);
    SET IDENTITY_INSERT [dbo].[Purchases] OFF;

    INSERT INTO [dbo].[PurchaseItems] ([ItemId], [PurchaseId], [Amount_bought], [Price]) VALUES (1, 1, 1, 22.00);

COMMIT TRANSACTION;
PRINT '=== SEED PURCHASE CARGADO ===';
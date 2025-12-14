
IF OBJECT_ID('dbo.Brands') IS NOT NULL BEGIN TRY SET IDENTITY_INSERT [dbo].[Brands] OFF; END TRY BEGIN CATCH END CATCH;
IF OBJECT_ID('dbo.Classes') IS NOT NULL BEGIN TRY SET IDENTITY_INSERT [dbo].[Classes] OFF; END TRY BEGIN CATCH END CATCH;
IF OBJECT_ID('dbo.ItemTypes') IS NOT NULL BEGIN TRY SET IDENTITY_INSERT [dbo].[ItemTypes] OFF; END TRY BEGIN CATCH END CATCH;
IF OBJECT_ID('dbo.Items') IS NOT NULL BEGIN TRY SET IDENTITY_INSERT [dbo].[Items] OFF; END TRY BEGIN CATCH END CATCH;
IF OBJECT_ID('dbo.PaymentMethod') IS NOT NULL BEGIN TRY SET IDENTITY_INSERT [dbo].[PaymentMethod] OFF; END TRY BEGIN CATCH END CATCH;
IF OBJECT_ID('dbo.Purchases') IS NOT NULL BEGIN TRY SET IDENTITY_INSERT [dbo].[Purchases] OFF; END TRY BEGIN CATCH END CATCH;

BEGIN TRANSACTION;
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

    DBCC CHECKIDENT ('[dbo].[Brands]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[Items]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[PaymentMethod]', RESEED, 0);
    DBCC CHECKIDENT ('[dbo].[Purchases]', RESEED, 0);
    BEGIN TRY DBCC CHECKIDENT ('[dbo].[Classes]', RESEED, 0); END TRY BEGIN CATCH END CATCH;
    BEGIN TRY DBCC CHECKIDENT ('[dbo].[ItemTypes]', RESEED, 0); END TRY BEGIN CATCH END CATCH;


INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], 
     [PasswordHash], [SecurityStamp], [ConcurrencyStamp], 
     [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], 
     [LockoutEnd], [LockoutEnabled], [AccessFailedCount], 
     [Name], [Surname]) 
VALUES 
    (N'1', N'Pepe.Gomez', N'PEPE.GOMEZ', N'pepegomez@example.com', N'PEPEGOMEZ@EXAMPLE.COM', 1, 
     N'AQAAAAIAAYagAAAAEBb0aMfbI6rZK+N7+L9lN8nHiyhzml+P3x/PKH1uUpTqvLh1D8KN/7nXZk8oYh+dA==', -- Password123!
     N'YJ56S5T7W5677', N'd613-4c09-8f73-288165c401', N'684573945', 1, 0, NULL, 1, 0, 
     N'Pepe', N'Gomez'),

    (N'2', N'Alejandra.Jimenez', N'ALEJANDRA.JIMENEZ', N'Alejandrajimenez@example.com', N'ALEJANDRAJIMENEZ@EXAMPLE.COM', 1, 
     N'AQAAAAIAAYagAAAAEBb0aMfbI6rZK+N7+L9lN8nHiyhzml+P3x/PKH1uUpTqvLh1D8KN/7nXZk8oYh+dA==', -- Password123!
     N'KJAHS76523BSD', N'2b-4c19-8f8e-9b8e1a7ae', N'684586744', 1, 0, NULL, 1, 0, 
     N'Alejandra', N'Jimenez'),

    (N'3', N'Javier.Hernandez', N'JAVIER.HERNANDEZ', N'Javierhernandez@example.com', N'JAVIERHERNANDEZ@EXAMPLE.COM', 1, 
     N'AQAAAAIAAYagAAAAEBb0aMfbI6rZK+N7+L9lN8nHiyhzml+P3x/PKH1uUpTqvLh1D8KN/7nXZk8oYh+dA==', -- Password123!
     N'MNBV234567890', N'8f73-288165c4017c-d613', N'645686744', 1, 0, NULL, 1, 0, 
     N'Javier', N'Hernandez'),

    (N'4', N'Adrian.Sevilla@alu.uclm.es', N'ADRIAN.SEVILLA@ALU.UCLM.ES', N'Adrian.Sevilla@alu.uclm.es', N'ADRIAN.SEVILLA@ALU.UCLM.ES', 1, 
     N'AQAAAAIAAYagAAAAEEwJWcqKoj4QNU8u4/EN1RJse4wBVCXdkwOuqRLRTgZruzrYxiDGuj6lEcgtli2MIQ==', -- Hash original tuyo
     N'E55FBQ3FQMLJ7IWZGSZXNGZZQFROFVY7', N'7fb05bbd-d613-4c09-8f73-288165c4017c', NULL, 1, 0, NULL, 1, 0, 
     N'Adrian', N'Sevilla'),

    (N'1001', N'test@gmail.com', N'TEST@GMAIL.COM', N'test@gmail.com', N'TEST@GMAIL.COM', 1, 
     N'AQAAAAIAAYagAAAAEBb0aMfbI6rZK+N7+L9lN8nHiyhzml+P3x/PKH1uUpTqvLh1D8KN/7nXZk8oYh+dA==', -- Password123!
     N'8KH7M6JLQPZ5G3YQ34Q7OQKJYH6HMXRI', N'7d63f8fa-3d2b-4c19-8f8e-9b8e1a7aeb11', NULL, 1, 0, NULL, 1, 0, 
     N'Test', N'User');

    SET IDENTITY_INSERT [dbo].[Brands] ON;
    INSERT INTO [dbo].[Brands] ([Id], [Name]) VALUES (1, N'Nike'), (2, N'Adidas'), (3, N'Domyos');
    SET IDENTITY_INSERT [dbo].[Brands] OFF;

    SET IDENTITY_INSERT [dbo].[Classes] ON;
    INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date]) VALUES (1, 10, N'Generic Class', 0, GETDATE());
    SET IDENTITY_INSERT [dbo].[Classes] OFF;

    SET IDENTITY_INSERT [dbo].[ItemTypes] ON;
    INSERT INTO [dbo].[ItemTypes] ([Id], [Name], [ClassId]) VALUES 
    (1, N'Cardio Equipment', 1), (2, N'Strength Equipment', 1), (3, N'Accessories', 1);
    SET IDENTITY_INSERT [dbo].[ItemTypes] OFF;

    SET IDENTITY_INSERT [dbo].[PaymentMethod] ON;
    INSERT INTO [dbo].[PaymentMethod] ([Id], [UserId], [Discriminator], [TelephoneNumber]) VALUES (1, N'1', N'Bizum', 684573945);
    INSERT INTO [dbo].[PaymentMethod] ([Id], [UserId], [Discriminator], [TelephoneNumber]) VALUES (4, N'4', N'Bizum', 600123456);
    SET IDENTITY_INSERT [dbo].[PaymentMethod] OFF;

    SET IDENTITY_INSERT [dbo].[Items] ON;
    INSERT INTO [dbo].[Items] ([Id], [Description], [Name], [QuantityAvailableForPurchase], [QuantityForRestock], [RestockPrice], [PurchasePrice], [ItemTypeId], [BrandId]) 
    VALUES 
    (1, N'Set of bands', N'Resistance band set', 9, 10, 15.00, 22.00, 3, 1),
    (2, N'Foam roller for muscle recovery and massage', N'Foam Roller', 9, 8, 18.00, 25.00, 3, 2),
    (3, N'Ideal for strength and endurance training', N'Kettlebell 10 kg', 9, 5, 25.00, 35.00, 2, 3);
    SET IDENTITY_INSERT [dbo].[Items] OFF;

    SET IDENTITY_INSERT [dbo].[Purchases] ON;
    INSERT INTO [dbo].[Purchases] ([Id], [City], [Country], [Date], [Description], [Street], [Total_prices], [PaymentMethodId]) 
    VALUES (1, N'Cuenca', N'Spain', GETDATE(), N'Purchase test', N'C/Mayor', 10.00, 1);
    SET IDENTITY_INSERT [dbo].[Purchases] OFF;

    INSERT INTO [dbo].[PurchaseItems] ([ItemId], [PurchaseId], [Amount_bought], [Price]) VALUES (1, 1, 1, 22.00);

COMMIT TRANSACTION;
PRINT '=== SEED PURCHASE CARGADO ===';
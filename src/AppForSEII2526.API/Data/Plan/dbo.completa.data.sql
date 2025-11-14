BEGIN TRANSACTION;


----------------------------------------------------
-- Borrar datos de todas las tablas
----------------------------------------------------
DELETE FROM [dbo].[ItemTypes];
DELETE FROM [dbo].[Plans];
DELETE FROM [dbo].[PaymentMethod];
DELETE FROM [dbo].[Classes];
DELETE FROM [dbo].[PlanItems];
DELETE FROM [dbo].[AspNetUsers];

----------------------------------------------------
-- Reiniciar los contadores IDENTITY
----------------------------------------------------
DBCC CHECKIDENT ('[dbo].[Plans]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[ItemTypes]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[Classes]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[PaymentMethod]', RESEED, 0);
DBCC CHECKIDENT ('[dbo].[PlanItems]', RESEED, 0);

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

----------------------------------------------------
-- Plans
----------------------------------------------------
SET IDENTITY_INSERT [dbo].[Plans] ON
INSERT INTO [dbo].[Plans] ([Id], [UserId], [Weeks], [HealthIssues], [Description], [Name], [Totalprice], [CreatedDate]) VALUES (11, N'1', 1, N'', N'', N'BackPlan', CAST(25.00 AS Decimal(10, 2)), N'2025-11-27 10:00:00')
INSERT INTO [dbo].[Plans] ([Id], [UserId], [Weeks], [HealthIssues], [Description], [Name], [Totalprice], [CreatedDate]) VALUES (15, N'2', 2, N'', N'', N'ArmPlan', CAST(26.00 AS Decimal(10, 2)), N'2025-12-09 10:10:00')
INSERT INTO [dbo].[Plans] ([Id], [UserId], [Weeks], [HealthIssues], [Description], [Name], [Totalprice], [CreatedDate]) VALUES (16, N'3', 3, N'', N'', N'LegPlan', CAST(28.00 AS Decimal(10, 2)), N'2025-11-30 10:00:00')
SET IDENTITY_INSERT [dbo].[Plans] OFF


----------------------------------------------------
-- ItemTypes 
----------------------------------------------------
SET IDENTITY_INSERT [dbo].[ItemTypes] ON;

INSERT INTO [dbo].[ItemTypes] ([Id], [Name], [ClassId]) VALUES (1, N'Cardio Equipment', NULL);
INSERT INTO [dbo].[ItemTypes] ([Id], [Name], [ClassId]) VALUES (2, N'Strength Equipment', NULL);
INSERT INTO [dbo].[ItemTypes] ([Id], [Name], [ClassId]) VALUES (3, N'Accessories', NULL);

SET IDENTITY_INSERT [dbo].[ItemTypes] OFF;

----------------------------------------------------
-- Classes
----------------------------------------------------
SET IDENTITY_INSERT [dbo].[Classes] ON
INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date]) VALUES (1, 10, N'Crossfit', CAST(30.00 AS Decimal(10, 2)), N'2025-10-20 10:00:00')
INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date]) VALUES (2, 15, N'spinning', CAST(25.00 AS Decimal(10, 2)), N'2025-11-10 11:00:00')
INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date]) VALUES (3, 8, N'firness', CAST(18.00 AS Decimal(10, 2)), N'2025-11-01 08:00:00')
SET IDENTITY_INSERT [dbo].[Classes] OFF


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
-- PlanItems
----------------------------------------------------
INSERT INTO [dbo].[PlanItems] ([ClassId], [PlanId], [Goal], [Price]) VALUES (1, 11, NULL, CAST(25.00 AS Decimal(10, 2)))
INSERT INTO [dbo].[PlanItems] ([ClassId], [PlanId], [Goal], [Price]) VALUES (2, 15, NULL, CAST(26.00 AS Decimal(10, 2)))
INSERT INTO [dbo].[PlanItems] ([ClassId], [PlanId], [Goal], [Price]) VALUES (3, 16, NULL, CAST(30.00 AS Decimal(10, 2)))

COMMIT TRANSACTION;

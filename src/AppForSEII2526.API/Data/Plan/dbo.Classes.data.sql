
DELETE FROM [dbo].[ItemTypes];

SET IDENTITY_INSERT [dbo].[ItemTypes] ON;

INSERT INTO [dbo].[ItemTypes] ([Id], [Name]) VALUES (1, N'Cardio');
INSERT INTO [dbo].[ItemTypes] ([Id], [Name]) VALUES (2, N'Fuerza');
INSERT INTO [dbo].[ItemTypes] ([Id], [Name]) VALUES (3, N'Flexibilidad');

SET IDENTITY_INSERT [dbo].[ItemTypes] OFF;
DELETE FROM [dbo].[Classes];
SET IDENTITY_INSERT [dbo].[Classes] ON;

-- 1. MORNING YOGA (La que necesita el test ESC8_AF7)
-- Le ponemos la misma fecha que a Spinning y el mismo TypeId
INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date], [ItemTypeId]) 
VALUES (10, 15, N'Morning Yoga', 25.00, '2026-05-17 17:04:00', 1);

-- 2. SPINNING (La que ya sabemos que la web SÍ muestra)
INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date], [ItemTypeId]) 
VALUES (2, 0, N'Spinning', 25.00, '2026-05-17 17:04:00', 1);

SET IDENTITY_INSERT [dbo].[Classes] OFF;
SET IDENTITY_INSERT [dbo].[Classes] ON
INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date]) VALUES (1, 15, N'Cardio', CAST(20.00 AS Decimal(10, 2)), N'2025-11-20 13:00:00')
INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date]) VALUES (2, 20, N'Fitness', CAST(25.00 AS Decimal(10, 2)), N'2025-11-22 10:00:00')
INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date]) VALUES (3, 25, N'CrossFit', CAST(30.00 AS Decimal(10, 2)), N'2025-11-30 08:00:00')
SET IDENTITY_INSERT [dbo].[Classes] OFF

SET IDENTITY_INSERT [dbo].[Classes] ON
INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date]) VALUES (1, 10, N'Crossfit', CAST(30.00 AS Decimal(10, 2)), N'2025-12-20 10:00:00')
INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date]) VALUES (2, 15, N'spinning', CAST(25.00 AS Decimal(10, 2)), N'2025-11-10 11:00:00')
INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date]) VALUES (3, 8, N'firness', CAST(18.00 AS Decimal(10, 2)), N'2025-11-01 08:00:00')
SET IDENTITY_INSERT [dbo].[Classes] OFF

SET IDENTITY_INSERT [dbo].[Restock] ON
INSERT INTO [dbo].[Restock] ([Id], [DeliveryAddress], [Description], [ExpectedDate], [RestockDate], [Title], [TotalPrice], [RestockResponsibleId]) VALUES (3, N'Casa', N'SinDescripcion', N'2025-10-10 00:00:00', N'2025-12-15 00:00:00', N'Restock1', CAST(100.00 AS Decimal(5, 2)), N'1')
INSERT INTO [dbo].[Restock] ([Id], [DeliveryAddress], [Description], [ExpectedDate], [RestockDate], [Title], [TotalPrice], [RestockResponsibleId]) VALUES (5, N'Universidad', N'SinDescripcion', N'2025-11-01 00:00:00', N'2025-11-13 00:00:00', N'Restock2', CAST(200.00 AS Decimal(5, 2)), N'2')
INSERT INTO [dbo].[Restock] ([Id], [DeliveryAddress], [Description], [ExpectedDate], [RestockDate], [Title], [TotalPrice], [RestockResponsibleId]) VALUES (6, N'Bar', N'SinDescripcion', N'2025-11-22 00:00:00', N'2025-12-24 00:00:00', N'Restock3', CAST(150.00 AS Decimal(5, 2)), N'3')
SET IDENTITY_INSERT [dbo].[Restock] OFF

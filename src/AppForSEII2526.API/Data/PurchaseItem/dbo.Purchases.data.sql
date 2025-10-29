SET IDENTITY_INSERT [dbo].[Purchases] ON
INSERT INTO [dbo].[Purchases] ([Id], [City], [Country], [Date], [Description], [Street], [Total_prices], [PaymentMethodId]) VALUES (11, N'Cuenca', N'Spain', N'2025-10-10 00:00:00', N'Purchase of a foam roller', N'C/Plaza Mayor', CAST(10.00 AS Decimal(10, 2)), 1)
INSERT INTO [dbo].[Purchases] ([Id], [City], [Country], [Date], [Description], [Street], [Total_prices], [PaymentMethodId]) VALUES (12, N'Madrid', N'Spain', N'2025-10-06 00:00:00', N'Purchase of resistance band', N'C/Madrid', CAST(50.00 AS Decimal(10, 2)), 1)
SET IDENTITY_INSERT [dbo].[Purchases] OFF

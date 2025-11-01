SET IDENTITY_INSERT [dbo].[Plans] ON
INSERT INTO [dbo].[Plans] ([Id], [Weeks], [HealthIssues], [Description], [Name], [Totalprice], [CreatedDate]) VALUES (3, 1, N'Back', N'Strengthen your back.', N'Pepe', CAST(35.00 AS Decimal(10, 2)), N'2025-10-19 10:00:00')
INSERT INTO [dbo].[Plans] ([Id], [Weeks], [HealthIssues], [Description], [Name], [Totalprice], [CreatedDate]) VALUES (5, 2, N'Leg', N'Strengthen your leg.', N'Maria', CAST(30.00 AS Decimal(10, 2)), N'2025-11-25 20:00:00')
INSERT INTO [dbo].[Plans] ([Id], [Weeks], [HealthIssues], [Description], [Name], [Totalprice], [CreatedDate]) VALUES (6, 3, N'Arm', N'Strengthen your arm.', N'Ramón', CAST(23.00 AS Decimal(10, 2)), N'2025-10-30 09:00:00')
SET IDENTITY_INSERT [dbo].[Plans] OFF

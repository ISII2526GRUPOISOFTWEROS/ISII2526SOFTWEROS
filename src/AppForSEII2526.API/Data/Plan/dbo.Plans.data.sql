SET IDENTITY_INSERT [dbo].[Plans] ON
INSERT INTO [dbo].[Plans] ([Id], [UserId], [Weeks], [HealthIssues], [Description], [Name], [Totalprice], [CreatedDate]) VALUES (11, N'1', 1, N'', N'', N'BackPlan', CAST(25.00 AS Decimal(10, 2)), N'2025-11-27 10:00:00')
INSERT INTO [dbo].[Plans] ([Id], [UserId], [Weeks], [HealthIssues], [Description], [Name], [Totalprice], [CreatedDate]) VALUES (15, N'2', 2, N'', N'', N'ArmPlan', CAST(26.00 AS Decimal(10, 2)), N'2025-12-09 10:10:00')
INSERT INTO [dbo].[Plans] ([Id], [UserId], [Weeks], [HealthIssues], [Description], [Name], [Totalprice], [CreatedDate]) VALUES (16, N'3', 3, N'', N'', N'LegPlan', CAST(28.00 AS Decimal(10, 2)), N'2025-11-30 10:00:00')
SET IDENTITY_INSERT [dbo].[Plans] OFF

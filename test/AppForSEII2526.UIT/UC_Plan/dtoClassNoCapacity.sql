
DELETE FROM [dbo].[Classes] WHERE [Name] = N'Morning Yoga';

SET IDENTITY_INSERT [dbo].[Classes] ON;
INSERT INTO [dbo].[Classes] ([Id], [Capacity], [Name], [Price], [Date], [ItemTypeId]) 
VALUES (10, 0, N'Morning Yoga', 10.00, DATEADD(day, 1, GETDATE()), 1);
SET IDENTITY_INSERT [dbo].[Classes] OFF;
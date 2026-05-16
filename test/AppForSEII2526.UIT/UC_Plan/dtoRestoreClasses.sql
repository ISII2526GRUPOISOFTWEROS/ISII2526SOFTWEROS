UPDATE [dbo].[Classes]
SET [Capacity] = 20
WHERE [Id] IN (SELECT [Id] FROM [dbo].[Classes]);
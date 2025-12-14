UPDATE [dbo].[Classes]
SET [Capacity] = 0
WHERE [Id] IN (SELECT [Id] FROM [dbo].[Classes]);
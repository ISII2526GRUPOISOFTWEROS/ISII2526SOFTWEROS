
-- 1. Pepe Gomez
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'1', N'Pepe', N'Gomez', N'Pepe.Gomez', N'PEPE.GOMEZ', N'pepegomez@example.com', N'PEPEGOMEZ@EXAMPLE.COM', 0, NULL, NULL, NULL, N'684573945', 1, 1, NULL, 0, 0);

-- 2. Alejandra Jimenez
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'2', N'Alejandra', N'Jimenez', N'Alejandra.Jimenez', N'ALEJANDRA.JIMENEZ', N'Alejandrajimenez@example.com', N'ALEJANDRAJIMENEZ@EXAMPLE.COM', 0, NULL, NULL, NULL, N'684586744', 1, 1, NULL, 0, 0);

-- 3. Javier Hernandez
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'3', N'Javier', N'Hernandez', N'Javier.Hernandez', N'JAVIER.HERNANDEZ', N'Javierhernandez@example.com', N'JAVIERHERNANDEZ@EXAMPLE.COM', 0, NULL, NULL, NULL, N'645686744', 1, 1, NULL, 0, 0);

-- 4. Adrian Sevilla (He añadido Name y Surname que faltaban para evitar el error NULL)
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'4', N'Adrian', N'Sevilla', N'Adrian.Sevilla@alu.uclm.es', N'ADRIAN.SEVILLA@ALU.UCLM.ES', N'Adrian.Sevilla@alu.uclm.es', N'ADRIAN.SEVILLA@ALU.UCLM.ES', 1, N'AQAAAAIAAYagAAAAEEwJWcqKoj4QNU8u4/EN1RJse4wBVCXdkwOuqRLRTgZruzrYxiDGuj6lEcgtli2MIQ==', N'E55FBQ3FQMLJ7IWZGSZXNGZZQFROFVY7', N'7fb05bbd-d613-4c09-8f73-288165c4017c', NULL, 0, 0, NULL, 1, 0);

-- 5. Test User
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [Name], [Surname], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
VALUES
    (N'1001', N'Test', N'User', N'test@gmail.com', N'TEST@GMAIL.COM', N'test@gmail.com', N'TEST@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEBb0aMfbI6rZK+N7+L9lN8nHiyhzml+P3x/PKH1uUpTqvLh1D8KN/7nXZk8oYh+dA==', N'8KH7M6JLQPZ5G3YQ34Q7OQKJYH6HMXRI', N'7d63f8fa-3d2b-4c19-8f8e-9b8e1a7aeb11', NULL, 0, 0, NULL, 1, 0);
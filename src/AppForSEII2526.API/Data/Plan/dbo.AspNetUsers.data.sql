
-- Usuario 1: Pepe
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'1', N'Pepe.Gomez', N'PEPE.GOMEZ', N'pepegomez@example.com', N'PEPEGOMEZ@EXAMPLE.COM', 0, NULL, NULL, NULL, N'684573945', 1, 1, NULL, 0, 0);

-- Usuario 2: Alejandra
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'2', N'Alejandra.Jimenez', N'ALEJANDRA.JIMENEZ', N'Alejandrajimenez@example.com', N'ALEJANDRAJIMENEZ@EXAMPLE.COM', 0, NULL, NULL, NULL, N'684586744', 1, 1, NULL, 0, 0);

-- Usuario 3: Javier
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'3', N'Javier.Hernandez', N'JAVIER.HERNANDEZ', N'Javierhernandez@example.com', N'JAVIERHERNANDEZ@EXAMPLE.COM', 0, NULL, NULL, NULL, N'645686744', 1, 1, NULL, 0, 0);

-- Usuario 4: Adrian (Este ya tenía estructura correcta, solo aseguramos mayúsculas en normalizados)
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) 
VALUES 
    (N'4', N'Adrian.Sevilla@alu.uclm.es', N'ADRIAN.SEVILLA@ALU.UCLM.ES', N'Adrian.Sevilla@alu.uclm.es', N'ADRIAN.SEVILLA@ALU.UCLM.ES', 1, N'AQAAAAIAAYagAAAAEEwJWcqKoj4QNU8u4/EN1RJse4wBVCXdkwOuqRLRTgZruzrYxiDGuj6lEcgtli2MIQ==', N'E55FBQ3FQMLJ7IWZGSZXNGZZQFROFVY7', N'7fb05bbd-d613-4c09-8f73-288165c4017c', NULL, 0, 0, NULL, 1, 0);

-- Usuario 1001: Test User
INSERT INTO [dbo].[AspNetUsers] 
    ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
VALUES
    (N'1001', 
     N'test@gmail.com', 
     N'TEST@GMAIL.COM', 
     N'test@gmail.com', 
     N'TEST@GMAIL.COM', 
     1, 
     N'AQAAAAIAAYagAAAAEBb0aMfbI6rZK+N7+L9lN8nHiyhzml+P3x/PKH1uUpTqvLh1D8KN/7nXZk8oYh+dA==', 
     N'8KH7M6JLQPZ5G3YQ34Q7OQKJYH6HMXRI', 
     N'7d63f8fa-3d2b-4c19-8f8e-9b8e1a7aeb11', 
     NULL, 0, 0, NULL, 1, 0);
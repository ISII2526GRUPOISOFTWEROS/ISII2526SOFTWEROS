
DELETE FROM [dbo].[PaymentMethod];

SET IDENTITY_INSERT [dbo].[PaymentMethod] ON;
IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] WHERE [Id] = N'1')
BEGIN
    INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount])
    VALUES (N'1', N'admin@test.com', N'ADMIN@TEST.COM', N'admin@test.com', 1, 0, 0, 1, 0);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] WHERE [Id] = N'2')
BEGIN
    INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [EmailConfirmed], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount])
    VALUES (N'2', N'user@test.com', N'USER@TEST.COM', N'user@test.com', 1, 0, 0, 1, 0);
END

INSERT INTO [dbo].[PaymentMethod] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) 
VALUES (1, N'1', N'Bizum', N'600000000', NULL, NULL, NULL);

INSERT INTO [dbo].[PaymentMethod] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) 
VALUES (2, N'2', N'CreditCard', NULL, N'4111111111111111', N'2027-12-31 00:00:00', NULL);

INSERT INTO [dbo].[PaymentMethod] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) 
VALUES (3, N'3', N'PayPal', NULL, NULL, NULL, N'Javierhernandez@example.com');

SET IDENTITY_INSERT [dbo].[PaymentMethod] OFF;
GO
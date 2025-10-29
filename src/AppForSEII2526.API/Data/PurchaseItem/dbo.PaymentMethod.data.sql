SET IDENTITY_INSERT [dbo].[PaymentMethod] ON
INSERT INTO [dbo].[PaymentMethod] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (1, N'1', N'Bizum', 684573945, NULL, NULL, NULL)
INSERT INTO [dbo].[PaymentMethod] ([Id], [UserId], [Discriminator], [TelephoneNumber], [CreditCardNumber], [ExpirationDate], [Email]) VALUES (2, N'2', N'CreditCard', NULL, N'4111111111111111', N'2027-12-31 00:00:00', NULL)
SET IDENTITY_INSERT [dbo].[PaymentMethod] OFF

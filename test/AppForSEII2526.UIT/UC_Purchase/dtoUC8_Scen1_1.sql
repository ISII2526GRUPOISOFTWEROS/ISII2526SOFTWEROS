UPDATE [dbo].[Purchases]
SET 
    [City] = N'Cuenca',
    [Country] = N'Spain',
    [Date] = CAST(GETDATE() AS DateTime), 
    [Description] = N'', 
    [Street] = N'Calle Mayor',
    [Total_prices] = CAST(75.00 AS Decimal(10, 2)),
    [PaymentMethodId] = 1 
WHERE [Id] = 4
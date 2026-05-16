SELECT 'Usuarios' as Tabla, COUNT(*) as Total FROM AspNetUsers UNION
SELECT 'Tipos' as Tabla, COUNT(*) as Total FROM ItemTypes UNION
SELECT 'MetodosPago' as Tabla, COUNT(*) as Total FROM PaymentMethod UNION
SELECT 'Clases' as Tabla, COUNT(*) as Total FROM Classes;
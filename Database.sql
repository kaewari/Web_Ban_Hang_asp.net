USE master
GO
CREATE DATABASE SNShopOnline
go
use SNShopOnline
go
/****** Object:  Table [dbo].[Banner]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Banner](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [int] NULL,
	[Path] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Banner] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Brand]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brand](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Brand] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[District]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[District](
	[Id] [int] NOT NULL,
	[ProvinceID] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Code] [int] NOT NULL,
 CONSTRAINT [PK_dbo.District] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderId] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] [decimal](18, 0) NULL,
	[UnitPrice] [decimal](18, 0) NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[EmployeeID] [int] NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[SubCategoryID] [int] NOT NULL,
	[BrandID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CPU] [nvarchar](255) NULL,
	[PIN] [nvarchar](255) NULL,
	[OS] [nvarchar](255) NULL,
	[Screen] [nvarchar](255) NULL,
	[RAM] [nvarchar](255) NULL,
	[Color] [nvarchar](255) NULL,
	[Design] [nvarchar](255) NULL,
	[VGA] [nvarchar](255) NULL,
	[Price] [int] NULL,
	[Webcam] [nvarchar](255) NULL,
	[ROM] [nvarchar](255) NULL,
	[QuantityPerUnit] [nvarchar](50) NULL,
	[UnitsInStock] [int] NULL,
	[UnitsOnOrder] [int] NULL,
	[Discontinued] [bit] NULL,
	[Release] [datetime] NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductImage]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductImage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[Thumbnail_Photo] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Product_Image] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Province]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Province](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Code] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Province] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResetPasswordCode]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResetPasswordCode](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Code] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_ResetPasswordCode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubCategory]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_SubCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 10/1/2022 6:19:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ID_Card] [bigint] NULL,
	[Truename] [nvarchar](max) NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[Status] [bit] NULL,
	[Address] [nvarchar](max) NULL,
	[Image] [nvarchar](max) NULL,
	[Facebook] [bit] NULL,
	[ProvinceID] [int] NULL,
	[DistrictID] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Brand] ADD  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Category] ADD  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (NULL) FOR [CPU]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (NULL) FOR [PIN]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (NULL) FOR [OS]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (NULL) FOR [Screen]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (NULL) FOR [RAM]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (NULL) FOR [Color]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (NULL) FOR [Design]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (NULL) FOR [VGA]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (NULL) FOR [Release]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [Facebook]
GO
ALTER TABLE [dbo].[Banner]  WITH CHECK ADD  CONSTRAINT [FK_Banner_Employee] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Banner] CHECK CONSTRAINT [FK_Banner_Employee]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Customer_dbo.Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_dbo.Customer_dbo.Users_UserID]
GO
ALTER TABLE [dbo].[District]  WITH CHECK ADD  CONSTRAINT [FK_dbo.District_dbo.Province_ProvinceID] FOREIGN KEY([ProvinceID])
REFERENCES [dbo].[Province] ([Id])
GO
ALTER TABLE [dbo].[District] CHECK CONSTRAINT [FK_dbo.District_dbo.Province_ProvinceID]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Employee_dbo.Users_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_dbo.Employee_dbo.Users_UserID]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OrderDetails_dbo.Orders_OrderId] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_dbo.OrderDetails_dbo.Orders_OrderId]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.OrderDetails_dbo.Product_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_dbo.OrderDetails_dbo.Product_ProductID]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Orders_dbo.Customer_CustomerID] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_dbo.Orders_dbo.Customer_CustomerID]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Orders_dbo.Employee_EmployeeID] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_dbo.Orders_dbo.Employee_EmployeeID]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Product_dbo.Brand_BrandID] FOREIGN KEY([BrandID])
REFERENCES [dbo].[Brand] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_dbo.Product_dbo.Brand_BrandID]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Product_dbo.Category_CategoryID] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_dbo.Product_dbo.Category_CategoryID]
GO
ALTER TABLE [dbo].[ProductImage]  WITH CHECK ADD  CONSTRAINT [FK_Product_Image_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[ProductImage] CHECK CONSTRAINT [FK_Product_Image_Product]
GO
ALTER TABLE [dbo].[ResetPasswordCode]  WITH CHECK ADD  CONSTRAINT [FK_ResetPasswordCode_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[ResetPasswordCode] CHECK CONSTRAINT [FK_ResetPasswordCode_Users]
GO
ALTER TABLE [dbo].[SubCategory]  WITH CHECK ADD  CONSTRAINT [FK_SubCategory_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[SubCategory] CHECK CONSTRAINT [FK_SubCategory_Category]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserRoles_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_dbo.UserRoles_dbo.Users_UserId]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Roles]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Users_dbo.District_DistrictID] FOREIGN KEY([DistrictID])
REFERENCES [dbo].[District] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo.Users_dbo.District_DistrictID]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Users_dbo.Province_ProvinceID] FOREIGN KEY([ProvinceID])
REFERENCES [dbo].[Province] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_dbo.Users_dbo.Province_ProvinceID]
GO
use SNShopOnline
go
delete from Orders
delete from OrderDetails
delete from UserRoles
delete from Customer
delete from Employee
delete from Users
delete from ProductImage
delete from Product
delete from Roles
delete from UserRoles
delete from Brand
delete from SubCategory
delete from Category
delete from ResetPasswordCode
go
insert into Roles values(1, 'Admin')
insert into Roles values(2, 'Users')
insert into Roles values(3, 'Members')
insert into Category(Name) values(N'Laptop Gaming')
insert into Category(Name) values(N'Laptop Văn phòng')
insert into Category(Name) values(N'Laptop Đồ họa')
insert into Category(Name) values(N'Laptop Mỏng nhẹ')
insert into Category(Name) values(N'Macbook')
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (1, 'Acer/Predator', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (1, 'Asus/ROG', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (1, 'Gigabyte/AORUS', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (1, 'MSI', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (1, 'Lenovo', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (1, 'Dell', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (1, 'HP', GETDATE())

insert into SubCategory(CategoryID, Name, ModifiedDate)
values (2, 'Acer', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (2, 'Asus', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (2, 'Gigabyte', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (2, 'MSI', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (2, 'Lenovo', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (2, 'Dell', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (2, 'HP', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (2, 'LG', GETDATE())

insert into SubCategory(CategoryID, Name, ModifiedDate)
values (3, 'Asus', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (3, 'MSI', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (3, 'Lenovo', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (3, 'Dell', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (3, 'HP', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (3, 'LG', GETDATE())

insert into SubCategory(CategoryID, Name, ModifiedDate)
values (4, 'Asus', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (4, 'Gigabyte', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (4, 'MSI', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (4, 'Lenovo', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (4, 'Dell', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (4, 'HP', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (4, 'LG', GETDATE())

insert into SubCategory(CategoryID, Name, ModifiedDate)
values (5, 'Macbook Air', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (5, 'Macbook Pro 13', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (5, 'Macbook Pro 14', GETDATE())
insert into SubCategory(CategoryID, Name, ModifiedDate)
values (5, 'Macbook Pro 16', GETDATE())

insert into Brand(Name) values('Asus')
insert into Brand(Name) values('Acer')
insert into Brand(Name) values('Gigabyte')
insert into Brand(Name) values('MSI')
insert into Brand(Name) values('Lenovo')
insert into Brand(Name) values('Dell')
insert into Brand(Name) values('HP')
insert into Brand(Name) values('LG')
insert into Brand(Name) values('Apple')

go
DECLARE @cnt INT = 1;
WHILE @cnt <= 10
BEGIN
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(1,2,1,'Laptop Asus Gaming ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(1,1,2,'Laptop Acer Gaming ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(1,4,4,'Laptop MSI Gaming ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(1,3,3,'Laptop Gigabyte Gaming ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(1,6,6,'Laptop Dell Gaming ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(1,5,5,'Laptop Lenovo Gaming ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(1,7,7,'Laptop HP Gaming ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(2,9,1,'Laptop Asus VP ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(2,8,2,'Laptop Acer VP ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(2,11,4,'Laptop MSI VP ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(2,10,3,'Laptop Gigabyte VP ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(2,13,6,'Laptop Dell VP ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(2,12,5,'Laptop Lenovo VP ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(2,14,7,'Laptop HP VP ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(2,15,8,'Laptop LG VP ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(3,16,1,'Laptop Asus DH ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(3,17,4,'Laptop MSI DH ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(3,19,6,'Laptop Dell DH ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(3,18,5,'Laptop Lenovo DH ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(3,20,7,'Laptop HP DH ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(3,21,8,'Laptop LG DH ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())

   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(4,22,1,'Laptop Asus MN' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(4,8,2,'Laptop Acer MN ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(4,24,4,'Laptop MSI MN ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(4,23,3,'Laptop Gigabyte MN ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(4,26,6,'Laptop Dell MN ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(4,25,5,'Laptop Lenovo MN ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(4,27,7,'Laptop HP MN ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(4,28,8,'Laptop LG MN ' + CAST(@cnt as varchar(10)) ,'Intel Core I5 9500H','10000mA','Windows 10','FullHD+','16GB','Black','Full metal','GTX1080Ti',24000000,10,10,10,10,GETDATE(),'FullHD',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(5,29,9,'Macbook Air ' + CAST(@cnt as varchar(10)),'Intel Core I5','10000mA','MacOS','2k+','16GB','Black','Full metal',null,30000000,10,10,10,10,GETDATE(),'2K',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(5,30,9,'Macbook Pro 13_' + CAST(@cnt as varchar(10)),'Apple M1','10000mA','MacOS','2k+','16GB','Black','Full metal',null,35000000,10,10,10,10,GETDATE(),'2K',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(5,31,9,'Macbook Pro 14_' + CAST(@cnt as varchar(10)),'Apple M1','10000mA','MacOS','2k+','16GB','Black','Full metal',null,40000000,10,10,10,10,GETDATE(),'2K',GETDATE())
   insert into Product(CategoryID, SubCategoryID, BrandID, Name, CPU, PIN, OS,Screen,
   RAM,Color,Design,VGA,Price,QuantityPerUnit,UnitsInStock,UnitsOnOrder,Discontinued,Release,Webcam,ModifiedDate)
   values(5,32,9,'Macbook Pro 16_' + CAST(@cnt as varchar(10)),'Apple M1','10000mA','MacOS','2k+','16GB','Black','Full metal',null,50000000,10,10,10,10,GETDATE(),'2K',GETDATE())
   SET @cnt = @cnt + 1;
END;
--Đổi giá trị biến @i và @x
go
DECLARE @count int
SET @count = (SELECT COUNT(Id) FROM Product)
DECLARE @i INT = 1;
DECLARE @j INT = 1;
DECLARE @x INT = (select Top 1(Id) from Product Order by 1 asc)
WHILE @i <= @count
BEGIN
   WHILE @j <= 4
   BEGIN 
		if (@x in (select id from Product where BrandID = 1))
		BEGIN
			insert into ProductImage(ProductID, Thumbnail_Photo, ModifiedDate)
			values(@x, '/Images/Products/Asus/asus - (' + CAST(@j as varchar(10)) + ').jpg', GETDATE())
		END
		if (@x in (select id from Product where BrandID = 2))
		BEGIN
			insert into ProductImage(ProductID, Thumbnail_Photo, ModifiedDate)
			values(@x, '/Images/Products/Acer/acer - (' + CAST(@j as varchar(10)) + ').jpg', GETDATE())
		END
		if (@x in (select id from Product where BrandID = 4))
		BEGIN
			insert into ProductImage(ProductID, Thumbnail_Photo, ModifiedDate)
			values(@x, '/Images/Products/MSI/msi - (' + CAST(@j as varchar(10)) + ').jpg', GETDATE())
		END
		if (@x in (select id from Product where BrandID = 3))
		BEGIN
			insert into ProductImage(ProductID, Thumbnail_Photo, ModifiedDate)
			values(@x, '/Images/Products/Gigabyte/gigabyte - (' + CAST(@j as varchar(10)) + ').png', GETDATE())
		END
		if (@x in (select id from Product where BrandID = 5))
		BEGIN
			insert into ProductImage(ProductID, Thumbnail_Photo, ModifiedDate)
			values(@x, '/Images/Products/Lenovo/lenovo - (' + CAST(@j as varchar(10)) + ').jpg', GETDATE())
		END

		if (@x in (select id from Product where BrandID = 8))
		BEGIN
			insert into ProductImage(ProductID, Thumbnail_Photo, ModifiedDate)
			values(@x, '/Images/Products/LG/LG - (' + CAST(@j as varchar(10)) + ').jpg', GETDATE())
		END
		
		if (@x in (select id from Product where BrandID = 7))
		BEGIN
			insert into ProductImage(ProductID, Thumbnail_Photo, ModifiedDate)
			values(@x, '/Images/Products/HP/HP - (' + CAST(@j as varchar(10)) + ').jpg', GETDATE())
		END
		
		if (@x in (select id from Product where BrandID = 6))
		BEGIN
			insert into ProductImage(ProductID, Thumbnail_Photo, ModifiedDate)
			values(@x, '/Images/Products/Dell/dell - (' + CAST(@j as varchar(10)) + ').jpg', GETDATE())
		END

		if (@x in (select id from Product where BrandID = 9))
		BEGIN
			insert into ProductImage(ProductID, Thumbnail_Photo, ModifiedDate)
			values(@x, '/Images/Products/Macbook/macbook - (' + CAST(@j as varchar(10)) + ').jpg', GETDATE())
		END

		SET @j = @j + 1;
   END
   SET @x = @x + 1;
   SET @i = @i + 1;
   SET @j = 1;
END
go
DECLARE @iUser INT = 1;
WHILE @iUser <= 300
BEGIN
	insert into Users(Email,PasswordHash,Username, Truename)
	values ('sonhoang'+ CAST(@iUser as varchar(1000)) +'@gmail.com', '4297f44b13955235245b2497399d7a93', 
	'SON' + CAST(@iUser as varchar(10)), N'Hoàng Thanh Sơn')
	SET @iUser = @iUser + 1
END
go
DECLARE @iURole INT = 1;
DECLARE @cUser INT = 1;
SET @iURole = (select top 1(Id) from Users)
WHILE @cUser <= 300
BEGIN
	insert into UserRoles(RoleId, UserId) values (2, @iURole)
	insert into Customer(UserID) values (@iURole)
	SET @iURole = @iURole + 1
	SET @cUser = @cUser + 1
END
go
DECLARE @fProduct INT = 1;
DECLARE @uProduct INT = 1;
DECLARE @cProduct INT = 1;
SET @cProduct = (select count(Id) from Product)
SET @fProduct = (select top 1(Id) from Product order by 1)
WHILE @uProduct <= @cProduct
BEGIN
	UPDATE Product
	SET UnitsInStock = Floor(RAND()*(20-5+1)+5), UnitsOnOrder = Floor(RAND()*(20-5+1)+5)
	where Id = @fProduct
	SET @uProduct = @uProduct + 1
	SET @fProduct = @fProduct + 1
END
go
UPDATE Product
SET ROM = '512GB'
go
UPDATE Product
SET VGA = 'Onboard'
where BrandID = 9
go
UPDATE Users
SET ModifiedDate = GETDATE()
go
DECLARE @iUser INT = 1;
DECLARE @cUser INT;
SET @cUser = (select top 1(Id) from Users order by 1 desc) + 1
WHILE @iUser <= 3
BEGIN
	insert into Users(Email,PasswordHash,Username, Truename, ModifiedDate)
	values ('sonhoang'+ CAST(@cUser as varchar(100)) +'@gmail.com', '4297f44b13955235245b2497399d7a93', 
	'SON' + CAST(@cUser as varchar(10)), N'Hoàng Thanh Sơn', GETDATE())
	insert into UserRoles(RoleId, UserId) values (1, @cUser)
	insert into Employee(UserID) values (@cUser)
	SET @iUser = @iUser + 1
	SET @cUser = @cUser + 1
END
go
DECLARE @iUser INT = 1;
DECLARE @cUser INT;
SET @cUser = (select top 1(Id) from Users order by 1 desc) + 1
WHILE @iUser <= 20
BEGIN
	insert into Users(Email,PasswordHash,Username, Truename, ModifiedDate)
	values ('sonhoang'+ CAST(@cUser as varchar(100)) +'@gmail.com', '4297f44b13955235245b2497399d7a93', 
	'SON' + CAST(@cUser as varchar(10)), N'Hoàng Thanh Sơn', GETDATE())
	insert into UserRoles(RoleId, UserId) values (3, @cUser)
	insert into Employee(UserID) values (@cUser)
	SET @iUser = @iUser + 1
	SET @cUser = @cUser + 1
END
go
DECLARE @idUser BIGINT = 1000000000;
DECLARE @cUser INT;
DECLARE @iUser INT = 1;
set @cUser = (select COUNT(Id) from Users)
WHILE @iUser <= @cUser
BEGIN
	UPDATE Users
	SET ID_Card = @idUser
	where Id = @iUser
	SET @idUser = @idUser + 1
	SET @iUser = @iUser + 1
END
go
update Users
set Status = 0
go
insert into Banner
values(1, '/Images/Banners/slideshow (1).jpg', GETDATE())
insert into Banner
values(1, '/Images/Banners/slideshow (2).jpg', GETDATE())
insert into Banner
values(1, '/Images/Banners/slideshow (3).jpg', GETDATE())
insert into Banner
values(1, '/Images/Banners/slideshow (4).jpg', GETDATE())
insert into Banner
values(1, '/Images/Banners/slideshow (5).jpg', GETDATE())
insert into Banner
values(1, '/Images/Banners/slideshow (6).jpg', GETDATE())
insert into Banner
values(1, '/Images/Banners/slideshow (7).jpg', GETDATE())
insert into Banner
values(1, '/Images/Banners/slideshow (8).jpg', GETDATE())
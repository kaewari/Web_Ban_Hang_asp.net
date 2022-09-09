USE [master]
GO
/****** Object:  Database [SNShopOnline]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE DATABASE [SNShopOnline]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SNShopOnline', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MUSICAPP\MSSQL\DATA\SNShopOnline.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SNShopOnline_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MUSICAPP\MSSQL\DATA\SNShopOnline_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [SNShopOnline] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SNShopOnline].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SNShopOnline] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SNShopOnline] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SNShopOnline] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SNShopOnline] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SNShopOnline] SET ARITHABORT OFF 
GO
ALTER DATABASE [SNShopOnline] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SNShopOnline] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SNShopOnline] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SNShopOnline] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SNShopOnline] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SNShopOnline] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SNShopOnline] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SNShopOnline] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SNShopOnline] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SNShopOnline] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SNShopOnline] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SNShopOnline] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SNShopOnline] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SNShopOnline] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SNShopOnline] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SNShopOnline] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [SNShopOnline] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SNShopOnline] SET RECOVERY FULL 
GO
ALTER DATABASE [SNShopOnline] SET  MULTI_USER 
GO
ALTER DATABASE [SNShopOnline] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SNShopOnline] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SNShopOnline] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SNShopOnline] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SNShopOnline] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SNShopOnline] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'SNShopOnline', N'ON'
GO
ALTER DATABASE [SNShopOnline] SET QUERY_STORE = OFF
GO
USE [SNShopOnline]
GO
/****** Object:  Table [dbo].[Brand]    Script Date: 8/31/2022 6:00:48 PM ******/
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
/****** Object:  Table [dbo].[Category]    Script Date: 8/31/2022 6:00:48 PM ******/
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
/****** Object:  Table [dbo].[Customer]    Script Date: 8/31/2022 6:00:48 PM ******/
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
/****** Object:  Table [dbo].[District]    Script Date: 8/31/2022 6:00:48 PM ******/
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
/****** Object:  Table [dbo].[Employee]    Script Date: 8/31/2022 6:00:48 PM ******/
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
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 8/31/2022 6:00:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderId] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[Quantity] bigint NOT NULL,
	[UnitPrice] bigint NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 8/31/2022 6:00:48 PM ******/
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
/****** Object:  Table [dbo].[Product]    Script Date: 8/31/2022 6:00:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[BrandID] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CPU] [varchar](255) NULL,
	[PIN] [varchar](255) NULL,
	[OS] [varchar](255) NULL,
	[Screen] [varchar](255) NULL,
	[RAM] [varchar](255) NULL,
	[Color] [varchar](255) NULL,
	[Design] [varchar](255) NULL,
	[Release] [time](7) NULL,
	[VGA] [varchar](255) NULL,
	[Price] [int] NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product_Image]    Script Date: 8/31/2022 6:00:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product_Image](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[Thumbnail_Photo] [nvarchar](max) NULL,
 CONSTRAINT [PK_Product_Image] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Province]    Script Date: 8/31/2022 6:00:48 PM ******/
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
/****** Object:  Table [dbo].[Roles]    Script Date: 8/31/2022 6:00:48 PM ******/
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
/****** Object:  Table [dbo].[UserClaims]    Script Date: 8/31/2022 6:00:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.UserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 8/31/2022 6:00:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_dbo.UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 8/31/2022 6:00:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[EmailConfirmed] [bit] NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[Username] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[Status] [bit] NULL,
	[Address] [nvarchar](256) NULL,
	[Image] [nvarchar](max) NULL,
	[Facebook] [bit] default 0 NULL,
	[ProvinceID] [int] NULL,
	[DistrictID] [int] NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [BrandNameIndex]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [BrandNameIndex] ON [dbo].[Brand]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [CategoryNameIndex]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [CategoryNameIndex] ON [dbo].[Category]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Id]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_Id] ON [dbo].[Customer]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserID]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserID] ON [dbo].[Customer]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [DistrictNameIndex]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [DistrictNameIndex] ON [dbo].[District]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProvinceID]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProvinceID] ON [dbo].[District]
(
	[ProvinceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Id]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_Id] ON [dbo].[Employee]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserID]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserID] ON [dbo].[Employee]
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderId]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderId] ON [dbo].[OrderDetails]
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductID]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductID] ON [dbo].[OrderDetails]
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CustomerID]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_CustomerID] ON [dbo].[Orders]
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_EmployeeID]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_EmployeeID] ON [dbo].[Orders]
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Id]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_Id] ON [dbo].[Orders]
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [ProductNameIndex]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [ProductNameIndex] ON [dbo].[Product]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [ProvinceNameIndex]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [ProvinceNameIndex] ON [dbo].[Province]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[Roles]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserId]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[UserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RoleId]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_RoleId] ON [dbo].[UserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserId]    Script Date: 8/31/2022 6:00:48 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserId] ON [dbo].[UserRoles]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
ALTER TABLE [dbo].[Product] ADD  DEFAULT (NULL) FOR [Release]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (NULL) FOR [VGA]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [Status]
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
ALTER TABLE [dbo].[Product_Image]  WITH CHECK ADD  CONSTRAINT [FK_Product_Image_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[Product_Image] CHECK CONSTRAINT [FK_Product_Image_Product]
GO
ALTER TABLE [dbo].[UserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserClaims_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserClaims] CHECK CONSTRAINT [FK_dbo.UserClaims_dbo.Users_UserId]
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
USE [master]
GO
ALTER DATABASE [SNShopOnline] SET  READ_WRITE 
GO
use SNShopOnline
go
insert into Roles values(1, 'Admin')
insert into Roles values(2, 'Users')
insert into Roles values(3, 'Members')
insert into Category(Name) values('Laptop Gaming')
insert into Category(Name) values(N'Laptop Văn phòng')
insert into Category(Name) values(N'Laptop Đồ họa')
insert into Category(Name) values(N'Laptop Mỏng nhẹ')
insert into Category(Name) values('Macbook')

insert into Brand(Name) values('Asus')
insert into Brand(Name) values('Acer')
insert into Brand(Name) values('Gigabyte')
insert into Brand(Name) values('MSI')
insert into Brand(Name) values('Lenovo')
insert into Brand(Name) values('Dell')
insert into Brand(Name) values('HP')
insert into Brand(Name) values('LG')
insert into Brand(Name) values('Apple')

insert into Product(CategoryID, BrandID, Name, Price) values(1,1,'Laptop Asus 1',19500000)
insert into Product(CategoryID, BrandID, Name, Price) values(1,2,'Laptop Acer 2',21500000)
insert into Product(CategoryID, BrandID, Name, Price) values(1,1,'Laptop Asus 3',20000000)
insert into Product(CategoryID, BrandID, Name, Price) values(1,2,'Laptop Acer 4',22000000)
insert into Product(CategoryID, BrandID, Name, Price) values(1,1,'Laptop Asus 5',23000000)
insert into Product(CategoryID, BrandID, Name, Price) values(1,2,'Laptop Acer 6',24000000)
insert into Product(CategoryID, BrandID, Name, Price) values(1,1,'Laptop Asus 7',15000000)
insert into Product(CategoryID, BrandID, Name, Price) values(1,2,'Laptop Acer 8',26000000)
insert into Product(CategoryID, BrandID, Name, Price) values(1,1,'Laptop Asus 9',19000000)
insert into Product(CategoryID, BrandID, Name, Price) values(1,2,'Laptop Acer 10',21000000)

insert into Product(CategoryID, BrandID, Name, Price) values(2,1,'Laptop Asus 11',19500000)
insert into Product(CategoryID, BrandID, Name, Price) values(2,2,'Laptop Acer 12',21500000)
insert into Product(CategoryID, BrandID, Name, Price) values(2,1,'Laptop Asus 13',20000000)
insert into Product(CategoryID, BrandID, Name, Price) values(2,2,'Laptop Acer 14',22000000)
insert into Product(CategoryID, BrandID, Name, Price) values(2,1,'Laptop Asus 15',23000000)
insert into Product(CategoryID, BrandID, Name, Price) values(2,2,'Laptop Acer 16',24000000)

insert into Product(CategoryID, BrandID, Name, Price) values(3,1,'Laptop Asus 17',15000000)
insert into Product(CategoryID, BrandID, Name, Price) values(3,2,'Laptop Acer 18',26000000)
insert into Product(CategoryID, BrandID, Name, Price) values(3,1,'Laptop Asus 19',19000000)
insert into Product(CategoryID, BrandID, Name, Price) values(3,2,'Laptop Acer 20',21000000)

insert into Product(CategoryID, BrandID, Name, Price) values(4,1,'Laptop Asus 21',19500000)
insert into Product(CategoryID, BrandID, Name, Price) values(4,2,'Laptop Acer 22',21500000)
insert into Product(CategoryID, BrandID, Name, Price) values(4,1,'Laptop Asus 23',20000000)
insert into Product(CategoryID, BrandID, Name, Price) values(4,2,'Laptop Acer 24',22000000)
insert into Product(CategoryID, BrandID, Name, Price) values(4,1,'Laptop Asus 25',23000000)
insert into Product(CategoryID, BrandID, Name, Price) values(4,2,'Laptop Acer 26',24000000)
insert into Product(CategoryID, BrandID, Name, Price) values(5,1,'Laptop Asus 27',15000000)
insert into Product(CategoryID, BrandID, Name, Price) values(5,2,'Laptop Acer 28',26000000)
insert into Product(CategoryID, BrandID, Name, Price) values(5,1,'Laptop Asus 29',19000000)
insert into Product(CategoryID, BrandID, Name, Price) values(5,2,'Laptop Acer 30',21000000)
USE [master]
GO
/****** Object:  Database [SkyPlanner]    Script Date: 8/17/2022 10:51:06 AM ******/
CREATE DATABASE [SkyPlanner]
GO
ALTER DATABASE [SkyPlanner] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SkyPlanner].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SkyPlanner] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SkyPlanner] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SkyPlanner] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SkyPlanner] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SkyPlanner] SET ARITHABORT OFF 
GO
ALTER DATABASE [SkyPlanner] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SkyPlanner] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SkyPlanner] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SkyPlanner] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SkyPlanner] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SkyPlanner] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SkyPlanner] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SkyPlanner] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SkyPlanner] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SkyPlanner] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SkyPlanner] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SkyPlanner] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SkyPlanner] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SkyPlanner] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SkyPlanner] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SkyPlanner] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SkyPlanner] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SkyPlanner] SET RECOVERY FULL 
GO
ALTER DATABASE [SkyPlanner] SET  MULTI_USER 
GO
ALTER DATABASE [SkyPlanner] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SkyPlanner] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SkyPlanner] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SkyPlanner] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [SkyPlanner] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'SkyPlanner', N'ON'
GO
USE [SkyPlanner]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 8/17/2022 10:51:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Account](
	[accountId] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NOT NULL,
	[phone] [nchar](10) NOT NULL,
	[street] [varchar](100) NOT NULL,
	[city] [varchar](50) NOT NULL,
	[state] [varchar](50) NOT NULL,
	[zip] [nchar](5) NOT NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[accountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 8/17/2022 10:51:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[contactId] [int] IDENTITY(1,1) NOT NULL,
	[accountId] [int] NOT NULL,
	[name] [nchar](200) NOT NULL,
	[phone] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[contactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order]    Script Date: 8/17/2022 10:51:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[orderId] [int] IDENTITY(1,1) NOT NULL,
	[accountId] [int] NOT NULL,
	[createdDate] [date] NOT NULL,
	[subtotal] [numeric](10, 2) NOT NULL,
	[tax] [numeric](10, 2) NOT NULL,
	[total] [numeric](10, 2) NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[orderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[OrderLineItem]    Script Date: 8/17/2022 10:51:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderLineItem](
	[orderLineItemId] [int] IDENTITY(1,1) NOT NULL,
	[orderId] [int] NOT NULL,
	[productId] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[unitPrice] [numeric](10, 2) NOT NULL,
 CONSTRAINT [PK_OrderLineItem] PRIMARY KEY CLUSTERED 
(
	[orderLineItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 8/17/2022 10:51:06 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product](
	[productId] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](200) NOT NULL,
	[price] [numeric](10, 2) NOT NULL,
	[phone] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[productId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_Account] FOREIGN KEY([accountId])
REFERENCES [dbo].[Account] ([accountId])
GO
ALTER TABLE [dbo].[Contact] CHECK CONSTRAINT [FK_Contact_Account]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Account] FOREIGN KEY([accountId])
REFERENCES [dbo].[Account] ([accountId])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Account]
GO
ALTER TABLE [dbo].[OrderLineItem]  WITH CHECK ADD  CONSTRAINT [FK_LineItem_Order] FOREIGN KEY([orderId])
REFERENCES [dbo].[Order] ([orderId])
GO
ALTER TABLE [dbo].[OrderLineItem] CHECK CONSTRAINT [FK_LineItem_Order]
GO
ALTER TABLE [dbo].[OrderLineItem]  WITH CHECK ADD  CONSTRAINT [FK_LineItem_Product] FOREIGN KEY([productId])
REFERENCES [dbo].[Product] ([productId])
GO
ALTER TABLE [dbo].[OrderLineItem] CHECK CONSTRAINT [FK_LineItem_Product]
GO
USE [master]
GO
ALTER DATABASE [SkyPlanner] SET  READ_WRITE 
GO


USE [SkyPlanner]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

GO
INSERT [dbo].[Account] ([accountId], [name], [phone], [street], [city], [state], [zip]) VALUES (1, N'SkyPlanner', N'3052228888', N'2200 SW', N'Coral Gables', N'FL', N'33134')
GO
INSERT [dbo].[Account] ([accountId], [name], [phone], [street], [city], [state], [zip]) VALUES (2, N'E-Business Innovator', N'3052163888', N'5100 W', N'Coral Gables', N'FL', N'33134')
GO
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[Contact] ON 

GO
INSERT [dbo].[Contact] ([contactId], [accountId], [name], [phone]) VALUES (1, 1, N'Contact SkyPlanner', N'3052228888')
GO
INSERT [dbo].[Contact] ([contactId], [accountId], [name], [phone]) VALUES (3, 2, N'Contact E-Business Innovator', N'3052163888')
GO
SET IDENTITY_INSERT [dbo].[Contact] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

GO
INSERT [dbo].[Product] ([productId], [name], [price], [phone]) VALUES (1, N'Laptop', CAST(900.00 AS Numeric(10, 2)), N'3053263265')
GO
INSERT [dbo].[Product] ([productId], [name], [price], [phone]) VALUES (2, N'Mouse', CAST(10.00 AS Numeric(10, 2)), N'3569877452')
GO
INSERT [dbo].[Product] ([productId], [name], [price], [phone]) VALUES (3, N'Keyboard', CAST(15.00 AS Numeric(10, 2)), N'3652145896')
GO
SET IDENTITY_INSERT [dbo].[Product] OFF
GO

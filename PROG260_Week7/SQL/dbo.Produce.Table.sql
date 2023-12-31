USE [PROG260FA23]
GO
/****** Object:  Table [dbo].[Produce]    Script Date: 10/26/2023 8:52:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Produce](
	[Name] [nvarchar](50) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[Price] [float] NOT NULL,
	[UoM] [nvarchar](50) NOT NULL,
	[Sell_by_Date] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO

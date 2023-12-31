USE [PROG260FA23]
GO
/****** Object:  Table [dbo].[Chars]    Script Date: 10/26/2023 8:52:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chars](
	[Character] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Map_Location] [nvarchar](50) NULL,
	[Original_Character] [bit] NOT NULL,
	[Sword_Fighter] [bit] NULL,
	[Magic_User] [bit] NULL,
 CONSTRAINT [PK_Chars] PRIMARY KEY CLUSTERED 
(
	[Character] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

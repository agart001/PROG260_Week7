USE [PROG260FA23]
GO
/****** Object:  Table [dbo].[Chars_Magic]    Script Date: 10/26/2023 8:52:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Chars_Magic](
	[M_Char] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Map_Location] [nvarchar](50) NULL,
	[Original_Character] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Chars_Magic]  WITH CHECK ADD  CONSTRAINT [FK_Chars_Magic_Chars] FOREIGN KEY([M_Char])
REFERENCES [dbo].[Chars] ([Character])
GO
ALTER TABLE [dbo].[Chars_Magic] CHECK CONSTRAINT [FK_Chars_Magic_Chars]
GO

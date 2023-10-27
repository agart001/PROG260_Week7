USE [PROG260FA23]
GO
/****** Object:  Table [dbo].[Game_Stats]    Script Date: 10/26/2023 8:52:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Game_Stats](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[GameID] [int] NOT NULL,
	[Sold] [int] NULL,
	[Rating] [float] NULL,
 CONSTRAINT [PK_Game_Stats] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Game_Stats]  WITH CHECK ADD  CONSTRAINT [FK_Game_Stats_Game] FOREIGN KEY([GameID])
REFERENCES [dbo].[Game] ([ID])
GO
ALTER TABLE [dbo].[Game_Stats] CHECK CONSTRAINT [FK_Game_Stats_Game]
GO

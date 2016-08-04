USE [library_test]
GO
/****** Object:  Table [dbo].[authors]    Script Date: 8/3/2016 9:02:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[authors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[books]    Script Date: 8/3/2016 9:02:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[books](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[title] [varchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[books_authors]    Script Date: 8/3/2016 9:02:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[books_authors](
	[book_id] [int] NULL,
	[author_id] [int] NULL
) ON [PRIMARY]

GO

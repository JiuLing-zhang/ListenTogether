```sql
USE [ListenTogether]
GO
/****** Object:  Table [dbo].[LogDetail]    Script Date: 2022/10/13 15:25:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogDetail](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserBaseId] [int] NOT NULL,
	[LogType] [nvarchar](10) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[LogTime] [datetime] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_LogDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Music]    Script Date: 2022/10/13 15:25:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Music](
	[InnerId] [int] IDENTITY(1,1) NOT NULL,
	[Platform] [int] NOT NULL,
	[PlatformInnerId] [nvarchar](100) NOT NULL,
	[Id] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Alias] [nvarchar](50) NOT NULL,
	[Artist] [nvarchar](50) NOT NULL,
	[Album] [nvarchar](50) NOT NULL,
	[ImageUrl] [nvarchar](400) NOT NULL,
	[ExtendData] [nvarchar](400) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_MusicDetail] PRIMARY KEY CLUSTERED 
(
	[InnerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MyFavorite]    Script Date: 2022/10/13 15:25:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MyFavorite](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[UserBaseId] [int] NOT NULL,
	[ImageUrl] [nvarchar](400) NOT NULL,
	[EditTime] [datetime] NOT NULL,
 CONSTRAINT [PK_MyFavorite] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MyFavoriteDetail]    Script Date: 2022/10/13 15:25:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MyFavoriteDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MyFavoriteId] [int] NOT NULL,
	[PlatformName] [nvarchar](20) NOT NULL,
	[MusicId] [nvarchar](50) NOT NULL,
	[MusicName] [nvarchar](50) NOT NULL,
	[MusicArtist] [nvarchar](50) NOT NULL,
	[MusicAlbum] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_MyFavoriteDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshToken]    Script Date: 2022/10/13 15:25:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshToken](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserBaseId] [int] NOT NULL,
	[DeviceId] [nvarchar](36) NOT NULL,
	[Token] [nvarchar](200) NOT NULL,
	[ExpireTime] [datetime] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserBase]    Script Date: 2022/10/13 15:25:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserBase](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Nickname] [nvarchar](50) NOT NULL,
	[Avatar] [nvarchar](200) NOT NULL,
	[Salt] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[IsEnable] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Music] ADD  CONSTRAINT [DF_Music_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO

```
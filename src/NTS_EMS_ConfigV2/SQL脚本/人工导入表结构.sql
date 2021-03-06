SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ImportExcel]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ImportExcel](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ExcelPath] [nvarchar](500) NULL,
	[UploadTime] [smalldatetime] NULL CONSTRAINT [DF_ImportExcel_UploadTime]  DEFAULT (getdate()),
	[HandleStatus] [int] NULL CONSTRAINT [DF_ImportExcel_HandleStatus]  DEFAULT ((0)),
	[HandleErrorInfo] [text] NULL,
	[ImportResult] [int] NULL CONSTRAINT [DF_ImportExcel_ImportResult]  DEFAULT ((-1)),
	[IsArea] [int] NULL,
	[MonthType] [int] NULL,
 CONSTRAINT [PK_ImportExcel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ImportHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ImportHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectID] [int] NULL,
	[IsArea] [int] NULL,
	[StartTime] [smalldatetime] NULL,
	[EndTime] [smalldatetime] NULL,
	[ImportValue] [decimal](18, 4) NULL,
	[ItemCode] [nvarchar](50) NULL,
	[MonthType] [int] NULL,
	[OpTime] [smalldatetime] NULL CONSTRAINT [DF_ImportHistory_OpTime]  DEFAULT (getdate()),
	[excelid] [int] NULL,
 CONSTRAINT [PK_ImportHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ImportTemp]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ImportTemp](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[objectid] [int] NULL,
	[isarea] [int] NULL,
	[starttime] [smalldatetime] NULL,
	[endtime] [smalldatetime] NULL,
	[importvalue] [decimal](18, 4) NULL,
	[itemcode] [nvarchar](50) NULL,
	[monthtype] [int] NULL,
	[excelid] [int] NULL,
 CONSTRAINT [PK_ImportTemp] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ImportErrorInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ImportErrorInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ErrorType] [nvarchar](50) NULL,
	[ErrorContent] [nvarchar](2000) NULL,
	[ExcelId] [int] NULL,
 CONSTRAINT [PK_ImportErrorInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END

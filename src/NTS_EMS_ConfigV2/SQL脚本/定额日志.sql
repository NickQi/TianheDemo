
/****** 对象:  Table [dbo].[TS_QUOTA_LOG]    脚本日期: 04/18/2014 18:32:21 ******/
if(exists(select * from sysobjects where name = 'TS_QUOTA_LOG'))
drop table TS_QUOTA_LOG
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TS_QUOTA_LOG](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[QUOTAID] [int] NOT NULL,
	[USERNAME] [varchar](32) COLLATE Chinese_PRC_CI_AS NULL,
	[LOGTIME] [datetime] NULL,
	[QUOTAVALUE] [float] NULL,
	[RESERVED] [varchar](12) COLLATE Chinese_PRC_CI_AS NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
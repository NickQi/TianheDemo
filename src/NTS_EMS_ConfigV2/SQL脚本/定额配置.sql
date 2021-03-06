
/****** 对象:  Table [dbo].[TB_QUOTA]    脚本日期: 04/18/2014 18:30:30 ******/
if(exists(select * from sysobjects where name = 'TB_QUOTA'))
drop table TB_QUOTA
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TB_QUOTA](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[QUOTAID] [int] NOT NULL,
	[OBJECTTYPE] [smallint] NULL,
	[OBJECTID] [int] NOT NULL,
	[OBJECTDESC] [varchar](32) COLLATE Chinese_PRC_CI_AS NULL,
	[ITEMCODE] [varchar](5) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[QUOTATYPE] [smallint] NULL,
	[QUOTATIME] [datetime] NULL,
	[QUOTAVALUE] [float] NULL,
	[RESERVED] [varchar](12) COLLATE Chinese_PRC_CI_AS NULL,
PRIMARY KEY CLUSTERED 
(
	[QUOTAID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
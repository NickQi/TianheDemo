
/****** 对象:  Table [dbo].[SystemLog]    脚本日期: 04/18/2014 18:25:53 ******/
if(exists(select * from sysobjects where name = 'SystemLog'))
drop table SystemLog
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemLog](
	[SysNo] [int] IDENTITY(1,1) NOT NULL,
	[ModelName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[LogContent] [text] COLLATE Chinese_PRC_CI_AS NULL,
	[LogTime] [datetime] NULL,
	[OpType] [smallint] NOT NULL CONSTRAINT [DF__SystemLog__OpTyp__5AD01DD6]  DEFAULT (1),
	[UserName] [varchar](32) COLLATE Chinese_PRC_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
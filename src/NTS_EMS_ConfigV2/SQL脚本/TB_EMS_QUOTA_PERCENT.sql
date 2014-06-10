create table TB_EMS_QUOTA_PERCENT(
	ID int identity(1,1) primary key not null,
	ALARMTYPE smallint null,
	OBJECTTYPE smallint null,
	OBJECTID int null,
	OBJECTDESC varchar(32) null,
	QUOTATYPE smallint null,
	ITEMCODE varchar(5) null,
	[PERCENT] float null
)
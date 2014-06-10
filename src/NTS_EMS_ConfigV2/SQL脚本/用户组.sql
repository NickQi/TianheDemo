
/****** 对象:  Table [dbo].[TB_USERGROUPOBJECTRIGHT]    脚本日期: 04/18/2014 18:18:33 ******/
if(exists(select * from sysobjects where name = 'TB_USERGROUPOBJECTRIGHT'))
drop table TB_USERGROUPOBJECTRIGHT
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_USERGROUPOBJECTRIGHT](
	[UserGroupID] [int] NULL,
	[AreaID] [int] NULL,
	[Type] [smallint] NULL
) ON [PRIMARY]
GO

/****** 对象:  用户组保存存储过程    脚本日期: 04/18/2014 18:18:33 ******/
if (exists (select * from sysobjects where name = 'Pro_SaveUserGroup')) 
   drop proc Pro_SaveUserGroup
GO

create procedure Pro_SaveUserGroup
  @id int,                               --用户组ID
  @name varchar(64),                     --用户组名称
  @description varchar(128),             --用户组描述
  @groups varchar(1024),                 --不用
  @menuRightIds varchar(2500),           --菜单ids
  @objectRightIds varchar(max),          --对象ids
  @objectTypes varchar(max),             --对象types
  @countNum int output                   --返回值
as
  set nocount on                         --不计算影响行数
  set xact_abort on                      --事物全部回滚
  
  declare @currentId int                 --需要更新或者插入的GroupID值
  declare @menuId int                    --需要插入的菜单ID
  declare @objectId int                  --需要插入的对象ID
  declare @type smallint                 --需要插入的对象类型
  declare @index int                     --需要插入的菜单/对象索引
  declare @indexType int                 --需要插入的对象类型索引
  set @menuRightIds=ltrim(rtrim(@menuRightIds))
  set @objectRightIds=ltrim(rtrim(@objectRightIds))
  set @objectTypes=ltrim(rtrim(@objectTypes)) 

  ---开始事务
  begin transaction
  ---存在更新数据
    if(exists(select * from Tb_UserGroup where [id]=@id))
      begin
        set @currentId=@id
        update Tb_UserGroup 
        set [cname]=@name,
            [description]=@description,
            [groups]=@groups
        where [id]=@id
      end
  ---插入数据
    else
      begin
      select @currentId=isnull(max([id]),0)+1 from Tb_UserGroup
      insert into Tb_UserGroup([id], [cname], [description],[groups]) values(@currentId, @name, @description, @groups)
      end
  --开始对Tb_userGroupMenuRight及Tb_UserGroupObject处理
     begin
  --先删除数据
      delete from Tb_userGroupMenuRight where UserGroupId=@currentId
      delete from Tb_userGroupObjectRight where UserGroupId=@currentId

  -- Tb_userGroupMenuRight
      select @index=charindex(',',@menuRightIds)
      while @index<>0
        begin
        select @menuId=cast(substring(@menuRightIds,0,@index) as int)
        insert into Tb_userGroupMenuRight values(@currentId, @menuId)
        set @menuRightIds=substring(@menuRightIds,@index+1,len(@menuRightIds)-@index)
        set @index=charindex(',',@menuRightIds)
        end
      if len(@menuRightIds)<>0
      begin
      set @menuId=cast(@menuRightIds as int)
      insert into Tb_userGroupMenuRight values(@currentId, @menuId)
      end

      -----Tb_UserGroupObject
      set @index=charindex(',',@objectRightIds)
      set @indexType=charindex(',',@objectTypes)
      while @index<>0
        begin
        select @objectId=cast(substring(@objectRightIds,0,@index) as int)
        select @type=cast(substring(@objectTypes,0,@indexType) as int)
        insert into Tb_UserGroupObjectRight values(@currentId, @objectId,@type)
        set @objectRightIds=substring(@objectRightIds,@index+1,len(@objectRightIds)-@index)
        set @index=charindex(',',@objectRightIds)
        set @objectTypes=substring(@objectTypes,@indexType+1,len(@objectTypes)-@indexType)
        set @indexType=charindex(',',@objectTypes)
        end
      if len(@objectRightIds)<>0
      begin
      set @objectId=cast(@objectRightIds as int)
      set @type=cast(@objectTypes as int)
      insert into Tb_UserGroupObjectRight values(@currentId, @objectId,@type)
      end
     end


  if @@error<> 0
    begin
      rollback transaction
     set @countNum = 0
    end
  else
    begin
    commit transaction
    set @countNum = 1
    end
GO

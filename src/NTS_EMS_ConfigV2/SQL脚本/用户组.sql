
/****** ����:  Table [dbo].[TB_USERGROUPOBJECTRIGHT]    �ű�����: 04/18/2014 18:18:33 ******/
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

/****** ����:  �û��鱣��洢����    �ű�����: 04/18/2014 18:18:33 ******/
if (exists (select * from sysobjects where name = 'Pro_SaveUserGroup')) 
   drop proc Pro_SaveUserGroup
GO

create procedure Pro_SaveUserGroup
  @id int,                               --�û���ID
  @name varchar(64),                     --�û�������
  @description varchar(128),             --�û�������
  @groups varchar(1024),                 --����
  @menuRightIds varchar(2500),           --�˵�ids
  @objectRightIds varchar(max),          --����ids
  @objectTypes varchar(max),             --����types
  @countNum int output                   --����ֵ
as
  set nocount on                         --������Ӱ������
  set xact_abort on                      --����ȫ���ع�
  
  declare @currentId int                 --��Ҫ���»��߲����GroupIDֵ
  declare @menuId int                    --��Ҫ����Ĳ˵�ID
  declare @objectId int                  --��Ҫ����Ķ���ID
  declare @type smallint                 --��Ҫ����Ķ�������
  declare @index int                     --��Ҫ����Ĳ˵�/��������
  declare @indexType int                 --��Ҫ����Ķ�����������
  set @menuRightIds=ltrim(rtrim(@menuRightIds))
  set @objectRightIds=ltrim(rtrim(@objectRightIds))
  set @objectTypes=ltrim(rtrim(@objectTypes)) 

  ---��ʼ����
  begin transaction
  ---���ڸ�������
    if(exists(select * from Tb_UserGroup where [id]=@id))
      begin
        set @currentId=@id
        update Tb_UserGroup 
        set [cname]=@name,
            [description]=@description,
            [groups]=@groups
        where [id]=@id
      end
  ---��������
    else
      begin
      select @currentId=isnull(max([id]),0)+1 from Tb_UserGroup
      insert into Tb_UserGroup([id], [cname], [description],[groups]) values(@currentId, @name, @description, @groups)
      end
  --��ʼ��Tb_userGroupMenuRight��Tb_UserGroupObject����
     begin
  --��ɾ������
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

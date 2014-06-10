/*
���ݴ����itemcodeid����ȡ���е���itemcodeid���������Լ�
��ʽΪ �ش���ʽΪ: TABLE
*/

IF (EXISTS (SELECT *
            FROM   sys.sysobjects
            WHERE  name = 'GetAllChildren'))
  DROP FUNCTION GetAllChildren
GO

CREATE FUNCTION GetAllChildren
               (@parentId int)
RETURNS @result TABLE(ID INT)
AS
  BEGIN
    INSERT @result SELECT itemcodeid FROM becm_itemcode
    WHERE parentid=@parentid
    WHILE (@@ROWCOUNT >0)
      BEGIN
        INSERT @result SELECT A.itemcodeid FROM becm_itemcode A
					     INNER JOIN  @result B
                         ON A.parentid=B.ID
                       WHERE A.itemcodeid NOT IN(SELECT ID FROM @result)             
               
      END 
    RETURN
  END
GO

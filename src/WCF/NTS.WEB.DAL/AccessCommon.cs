using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.WEB.Model;
using NTS.WEB.ProductInteface;

namespace NTS.WEB.DAL
{
    public class AccessCommon : IAccessCommon
    {

        public TB_AreaInfo GetAreaInfo(int areaId)
        {
            try
            {
                var cmd = new DataCommand("GetAreaInfo", new SqlCustomDbCommand());
                cmd.SetParameterValue("@ID", areaId);
                return cmd.ExecuteEntity<TB_AreaInfo>();
            }
            catch (Exception ee)
            {
                throw ee;
            }

        }


        public List<TB_AreaInfo> GetAreaList(List<int> areaId )
        {
            try
            {
                string strArea = listToString(areaId);
                var cmd = new DataCommand("GetAreaList", new SqlCustomDbCommand());
                cmd.SetParameterValue("@AREAID", strArea);

                return cmd.ExecuteEntityList<TB_AreaInfo>();
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }


        public  String listToString(List<int> stringList){
	        if (stringList==null) {
	            return null;
	        }
	        StringBuilder result=new StringBuilder();
	        bool flag=false;
	        for (int i=0; i<  stringList.Count;i++) {
	            if (flag) {
	                result.Append(",");
	            }else {
	                flag=true;
	            }
                result.Append(stringList[i].ToString());
	        }
	        return result.ToString();
	    }


        public int GetUserGroupID(string username)
        {
            try
            {
                var cmd = new DataCommand("GetUserGroupID", new SqlCustomDbCommand());
                cmd.SetParameterValue("@cname", username);
                return int.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception ee)
            {
                throw ee;
            }

        }

        public decimal GetFeePrice(string itemcode)
        {
            try
            {
                var cmd = new DataCommand("GetFeePrice", new SqlCustomDbCommand());
                cmd.SetParameterValue("@itemcode", itemcode);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return Convert.ToDecimal(result);
                }
                throw new Exception("该分项未设置费率");


            }
            catch (Exception ee)
            {
                throw ee;
            }

        }
    }
}

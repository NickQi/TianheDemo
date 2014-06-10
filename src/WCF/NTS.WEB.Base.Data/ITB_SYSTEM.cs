using System;
using System.Data;
namespace NTS.WEB.ProductInteface
{
	/// <summary>
	/// 接口层TB_SYSTEM
	/// </summary>
	public interface ITB_SYSTEM
	{
		#region  成员方法
		
		/// <summary>
		/// 获得数据列表
		/// </summary>
		DataSet GetList(string strWhere);
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		DataSet GetList(int Top,string strWhere,string filedOrder);
	
		#endregion  成员方法
	} 
}

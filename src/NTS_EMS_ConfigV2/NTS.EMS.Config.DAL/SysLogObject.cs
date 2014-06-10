using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Framework.Data;
using NTS.EMS.Config.ProductInteface;
using PostSharp.Laos;

namespace NTS.EMS.Config.DAL
{
    public class SysLogObject : ISysLogObject
    {

        public List<Model.SysLog> GetSysLogList(string whereStr)
        {
            var cmd = new DataCommand("GetSysLog", new SqlCustomDbCommand());
            cmd.ReplaceParameterValue("#whereStr#", whereStr);
            return cmd.ExecuteEntityList<Model.SysLog>();
        }
    }

    /*
    [Serializable]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class BusinessLogAttribute : LogAttribute
    {
        public OperateType OpType { get; set; }
        public string UserName { get; set; }

        public override void OnEntry(MethodExecutionEventArgs eventArgs)
        {
            DataCommand command = new DataCommand("InsertSystemLog", new SqlCustomDbCommand());
            command.SetParameterValue("@modelname", this.ModelName);
            command.SetParameterValue("@logtime", DateTime.Now);
            command.SetParameterValue("@logcontent", string.IsNullOrEmpty(this.LogContent) ? (this.ModelName + "执行了操作。") : this.LogContent);
            command.SetParameterValue("@optype",(int)OpType == 0? 1: (int)this.OpType);
            command.SetParameterValue("@username", UserName);
            command.ExecuteNonQuery();

            base.OnEntry(eventArgs);
        }

        public override void OnExit(MethodExecutionEventArgs eventArgs)
        {
            //Console.Write("end....");
            base.OnExit(eventArgs);

        }

    }
    public enum OperateType
    {
        Operate = 1,
        Configure = 2
    }
     * */
}

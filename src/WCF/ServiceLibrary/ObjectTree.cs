using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using NTS.WEB.DataContact;
using NTS.WEB.ResultView;
using NTS.WEB.ServiceInterface;

namespace ServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ObjectTree : IObjectTree
    {
        public NTS.WEB.ResultView.ObjectTree GetObjectTree()
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.BaseTree().GetObjectTree();
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new NTS.WEB.ResultView.ObjectTree() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new NTS.WEB.ResultView.ObjectTree() { ActionInfo = pAction };
            }
        }


        public NTS.WEB.ResultView.ObjectTree GetDeviceTree()
        {
            var pAction = new ExecuteProcess();
            try
            {
                var result = new NTS.WEB.BLL.BaseTree().GetDeviceTree();
                if (result == null)
                {
                    pAction.Success = false;
                    pAction.ExceptionMsg = "暂无数据信息";
                    return new NTS.WEB.ResultView.ObjectTree() { ActionInfo = pAction };
                }
                pAction.Success = true;
                result.ActionInfo = pAction;
                return result;
            }
            catch (Exception e)
            {
                pAction.Success = false;
                pAction.ExceptionMsg = e.Message;
                return new NTS.WEB.ResultView.ObjectTree() { ActionInfo = pAction };
            }
        }



        public string GetDeviceListByArea(QueryDevice query)
        {
             return new NTS.WEB.BLL.BaseTree().GetDeviceListByArea(query);
        }

      

    }
}

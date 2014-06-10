using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;
using System.Web;
using NTS.WEB.Common;

namespace NTS.WEB.Common
{
    /// <summary>
    /// 验证类,提供表单的验证
    /// </summary>
    public class Validate
    {
        #region 属性

        private bool isValidated = true;
        /// <summary>
        /// 是否验证成功
        /// </summary>
        public bool IsValidated
        {
            get { return isValidated; }
            set { isValidated = value; }
        }

        private StringBuilder errorMessage;
        /// <summary>
        /// 返回的错误消息
        /// </summary>
        public StringBuilder ErrorMessage
        {
            get { return errorMessage; }
        }

        private System.Web.UI.Page vPage;
        /// <summary>
        /// 当前的页面
        /// </summary>
        public System.Web.UI.Page VPage
        {
            get { return vPage; }
            set { vPage = value; }
        }

        private EDisplay eDisplayType = EDisplay.None;
        /// <summary>
        /// 显示的样式枚举
        /// </summary>
        public EDisplay EDisplayType
        {
            get { return eDisplayType; }
            set { eDisplayType = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Validate()
        {
            errorMessage = new StringBuilder();
        }

        /// <summary>
        /// 重载的构造函数
        /// </summary>
        /// <param name="page"></param>
        /// <param name="eDisplayType">显示的样式</param>
        public Validate(EDisplay displayType)
        {
            this.vPage = (System.Web.UI.Page)HttpContext.Current.Handler;
            this.eDisplayType = displayType;
            errorMessage = new StringBuilder();
        }
        #endregion

        /// <summary>
        /// 执行验证，调用Alerts的Exec方法，执行js脚本
        /// 建议: 当EDisplay为Alert时调用
        /// </summary>
        /// <returns></returns>
        public bool DoValidate()
        {
            if (!isValidated)
                Alerts.exec(errorMessage.ToString());
            return isValidated;
        }

        /// <summary>
        /// 返回格式化后的文本
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public string GetValidateTmp(TextBox tb, string errMsg)
        {
            if (tb != null && eDisplayType == EDisplay.Alert)//如果控件不是Null
            {
                return string.Format("alert('{0}');document.getElementById(\"{1}\").focus();document.getElementById(\"{1}\").select();", new object[] { errMsg, tb.ClientID });
            }
            else if (tb == null && eDisplayType == EDisplay.Alert)
            {
                return string.Format("alert('{0}');", new object[] { errMsg });
            }
            else
            {
                return string.Format("{0}", new object[] { errMsg });
            }
        }

        /// <summary>
        /// 验证控件的方法
        /// </summary>
        /// <param name="tb">TextBox控件</param>
        /// <param name="emptErrMsg">为空时的提示语句</param>
        /// <param name="eType">验证类型</param>
        /// <param name="validateErrMsg">验证错误后的错误提示</param>
        /// <returns></returns>
        public bool TxtValidate(TextBox tb, string emptErrMsg, EValidateType eType, string validateErrMsg)
        {
            return TxtValidate(tb, tb.Text, emptErrMsg, eType, validateErrMsg);
        }

        /// <summary>
        /// 验证控件的方法,不验证
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="emptErrMsg">为空时的提示语句</param>
        /// <returns></returns>
        public void TxtValidate(TextBox tb, string emptErrMsg)
        {
            TxtValidate(tb, emptErrMsg, EValidateType.None, null);
        }

        /// <summary>
        /// 验证String的方法,不验证
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="emptErrMsg">为空时的提示语句</param>
        /// <returns></returns>
        public void TxtValidate(string tbTxt, string emptErrMsg)
        {
            TxtValidate(null, tbTxt, emptErrMsg, EValidateType.None, null);
        }

        /// <summary>
        /// 验证，文本
        /// </summary>
        /// <param name="tbTxt">要验证的文本</param>
        /// <param name="emptErrMsg">为空的提示</param>
        /// <param name="eType">文本类型</param>
        /// <param name="validateErrMsg">验证错误的提示</param>
        /// <returns></returns>
        public bool TxtValidate(string tbTxt, string emptErrMsg, EValidateType eType, string validateErrMsg)
        {
            return TxtValidate(null, tbTxt, emptErrMsg, eType, validateErrMsg);
        }

        /// <summary>
        /// 验证参数是否合法
        /// </summary>
        /// <param name="tb">当显示类型为Alert时,需要得焦点的控件</param>
        /// <param name="tbTxt">验证的文本</param>
        /// <param name="emptErrMsg">为空提示的信息</param>
        /// <param name="eType">类型验证</param>
        /// <param name="validateErrMsg">类型验证错误信息</param>
        ///<param name="resShow">显示样式</param>
        /// <returns></returns>
        private bool TxtValidate(TextBox tb, string tbTxt, string emptErrMsg, EValidateType eType, string validateErrMsg)
        {
            if (isValidated)
            {
                string tbText = string.Empty;

                if (!string.IsNullOrEmpty(tbTxt))
                {
                    tbText = tbTxt.Trim();
                    tbText = Strings.GetSafeStr(tbText);
                    tbText = Strings.FilterHTML(tbText);
                    #region 根据类型验证
                    switch (eType)
                    {
                        case EValidateType.IsChinese:
                            if (!Regexs.IsChinese(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsCode:
                            if (!Regexs.IsCode(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsDateTime:
                            if (!Regexs.IsDateTime(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsDoubleByteChar:
                            if (!Regexs.IsDoubleByteChar(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsEMail:
                            if (!Regexs.IsEMail(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsFloat:
                            if (!Regexs.IsFloat(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsImage:
                            if (!Regexs.IsImage(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsInt:
                            if (!Regexs.IsInt(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsIPAddress:
                            if (!Regexs.IsIPAddress(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsMobil:
                            if (!Regexs.IsMobil(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsNumber:
                            if (!Regexs.IsNumber(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsOnlyLetterAndDigit:
                            if (!Regexs.IsOnlyLetterAndDigit(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsPhoneNumber:
                            if (!Regexs.IsPhoneNumber(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsPostCode:
                            if (!Regexs.IsPostCode(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsRegUserName:
                            if (!Regexs.IsRegUserName(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsTelepone:
                            if (!Regexs.IsTelepone(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.IsURLAddress:
                            if (!Regexs.IsURLAddress(tbText))
                            {
                                isValidated = false;
                                errorMessage.Append(GetValidateTmp(tb, validateErrMsg));
                                return false;
                            }
                            return true;
                        case EValidateType.None:
                            return true;
                    }
                    #endregion
                    return true;
                }
                else
                {
                    isValidated = false;
                    errorMessage.Append(GetValidateTmp(tb, emptErrMsg));
                    return false;
                }
            }
            return false;
        }

        public bool ExpValidate(TextBox tb, string emptErrMsg, EValidateType eType, string validateErrMsg, bool exp, string equalsErrMsg)
        {
            return ExpValidate(tb, exp, equalsErrMsg) && TxtValidate(tb, emptErrMsg, eType, validateErrMsg);
        }

        /// <summary>
        /// 验证控件,当这个控件为选填的时候,即(exp=(控件.Text.Length==0))
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="eType"></param>
        /// <param name="validateErrMsg"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool ExpValidate(TextBox tb, EValidateType eType, string validateErrMsg, bool exp)
        {
            if (isValidated)
            {
                if (exp)
                {
                    return true;
                }
                else
                {
                    return TxtValidate(tb, null, eType, validateErrMsg);
                }
            }
            return false;
        }

        /// <summary>
        /// 特殊的,控件或者文本可以为空,即满足exp就不验证了,如选填的项
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="eType"></param>
        /// <param name="validateErrMsg"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool ExpValidate(string tb, EValidateType eType, string validateErrMsg, bool exp)
        {
            if (isValidated)
            {
                if (exp)
                {
                    return true;
                }
                else
                {
                    return TxtValidate(tb, null, eType, validateErrMsg);
                }
            }
            return false;
        }

        /// <summary>
        /// 验证参数是否合法，验证参数类型,不允许空
        /// </summary>
        /// <param name="tbTxt">验证的文本</param>
        /// <param name="emptErrMsg">为空提示的信息</param>
        /// <param name="eType">类型验证</param>
        /// <param name="validateErrMsg">类型验证错误信息</param>
        /// <param name="exp">验证表达式,验证通过是true,不通过是</param>
        /// <param name="equalsErrMsg">验证表达式错误信息</param>
        /// <returns></returns>
        public bool ExpValidate(string tbTxt, string emptErrMsg, EValidateType eType, string validateErrMsg, bool exp, string equalsErrMsg)
        {
            return ExpValidate(null, exp, equalsErrMsg) && TxtValidate(tbTxt, emptErrMsg, eType, validateErrMsg);
        }

        /// <summary>
        /// 验证参数是否合法,不验证参数类型,不允许空
        /// </summary>
        /// <param name="tb">验证的文本框控件</param>
        /// <param name="emptErrMsg">为空提示的信息</param>
        /// <param name="exp">验证表达式,验证通过是true,不通过是false,true时alert</param>
        /// <param name="equalsErrMsg">验证表达式错误信息</param>
        /// <returns></returns>
        public bool ExpValidate(TextBox tb, string emptErrMsg, bool exp, string equalsErrMsg)
        {
            return ExpValidate(tb, emptErrMsg, EValidateType.None, null, exp, equalsErrMsg);
        }

        /// <summary>
        /// 验证参数是否合法,不验证参数类型,不允许空
        /// </summary>
        /// <param name="tbTxt">为空提示的信息</param>
        /// <param name="emptErrMsg">为空提示的信息</param>
        /// <param name="exp">验证表达式,验证通过是true,不通过是false,true时alert</param>
        /// <param name="equalsErrMsg">验证表达式错误信息</param>
        /// <returns></returns>
        public bool ExpValidate(string tbTxt, string emptErrMsg, bool exp, string equalsErrMsg)
        {
            return ExpValidate(tbTxt, emptErrMsg, EValidateType.None, null, exp, equalsErrMsg);
        }

        /// <summary>
        /// 验证参数是否合法,不验证参数类型,允许空
        /// </summary>
        /// <param name="exp">验证表达式，如1=1，'a'='a',验证通过是true,不通过是false,true时alert</param>
        /// <param name="equalsErrMsg">验证表达式错误信息</param>
        /// <returns></returns>
        public bool ExpValidate(bool exp, string equalsErrMsg)
        {
            return ExpValidate(null, exp, equalsErrMsg);
        }

        /// <summary>
        /// 验证参数是否合法,不验证参数类型,允许空
        /// </summary>
        /// <param name="tb">的焦点的控件</param>
        /// <param name="exp">验证表达式，如1=1，'a'='a',验证通过是true,不通过是false,true时alert</param>
        /// <param name="equalsErrMsg">验证表达式错误信息</param>
        /// <returns></returns>
        public bool ExpValidate(TextBox tb, bool exp, string equalsErrMsg)
        {
            if (isValidated)
            {
                //DataTable dt = new DataTable();
                //dt.Rows.Add(dt.NewRow());
                // if (dt.Select(string.Format("'{0}'", tbTxt) + txtEquals).Length > 0)
                if (!exp)
                {
                    isValidated = false;
                    errorMessage.Append(GetValidateTmp(tb, equalsErrMsg));
                    // errorMessage.Append(equalsErrMsg + "\\n");
                }
            }
            return exp;
        }

        /// <summary>
        /// 过滤字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string FilterString(string str)
        {
            if (str == null)
            {
                str = string.Empty;
            }
            string tbText = Strings.GetSafeStr(str.Trim());
            return Strings.FilterHTML(tbText);
        }

        public void Redirect(string url)
        {
            HttpContext.Current.Session["BX_CRM_ERROR"] = errorMessage.ToString();
            HttpContext.Current.Response.Redirect(url);
        }
    }
}

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
    /// ��֤��,�ṩ������֤
    /// </summary>
    public class Validate
    {
        #region ����

        private bool isValidated = true;
        /// <summary>
        /// �Ƿ���֤�ɹ�
        /// </summary>
        public bool IsValidated
        {
            get { return isValidated; }
            set { isValidated = value; }
        }

        private StringBuilder errorMessage;
        /// <summary>
        /// ���صĴ�����Ϣ
        /// </summary>
        public StringBuilder ErrorMessage
        {
            get { return errorMessage; }
        }

        private System.Web.UI.Page vPage;
        /// <summary>
        /// ��ǰ��ҳ��
        /// </summary>
        public System.Web.UI.Page VPage
        {
            get { return vPage; }
            set { vPage = value; }
        }

        private EDisplay eDisplayType = EDisplay.None;
        /// <summary>
        /// ��ʾ����ʽö��
        /// </summary>
        public EDisplay EDisplayType
        {
            get { return eDisplayType; }
            set { eDisplayType = value; }
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        public Validate()
        {
            errorMessage = new StringBuilder();
        }

        /// <summary>
        /// ���صĹ��캯��
        /// </summary>
        /// <param name="page"></param>
        /// <param name="eDisplayType">��ʾ����ʽ</param>
        public Validate(EDisplay displayType)
        {
            this.vPage = (System.Web.UI.Page)HttpContext.Current.Handler;
            this.eDisplayType = displayType;
            errorMessage = new StringBuilder();
        }
        #endregion

        /// <summary>
        /// ִ����֤������Alerts��Exec������ִ��js�ű�
        /// ����: ��EDisplayΪAlertʱ����
        /// </summary>
        /// <returns></returns>
        public bool DoValidate()
        {
            if (!isValidated)
                Alerts.exec(errorMessage.ToString());
            return isValidated;
        }

        /// <summary>
        /// ���ظ�ʽ������ı�
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public string GetValidateTmp(TextBox tb, string errMsg)
        {
            if (tb != null && eDisplayType == EDisplay.Alert)//����ؼ�����Null
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
        /// ��֤�ؼ��ķ���
        /// </summary>
        /// <param name="tb">TextBox�ؼ�</param>
        /// <param name="emptErrMsg">Ϊ��ʱ����ʾ���</param>
        /// <param name="eType">��֤����</param>
        /// <param name="validateErrMsg">��֤�����Ĵ�����ʾ</param>
        /// <returns></returns>
        public bool TxtValidate(TextBox tb, string emptErrMsg, EValidateType eType, string validateErrMsg)
        {
            return TxtValidate(tb, tb.Text, emptErrMsg, eType, validateErrMsg);
        }

        /// <summary>
        /// ��֤�ؼ��ķ���,����֤
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="emptErrMsg">Ϊ��ʱ����ʾ���</param>
        /// <returns></returns>
        public void TxtValidate(TextBox tb, string emptErrMsg)
        {
            TxtValidate(tb, emptErrMsg, EValidateType.None, null);
        }

        /// <summary>
        /// ��֤String�ķ���,����֤
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="emptErrMsg">Ϊ��ʱ����ʾ���</param>
        /// <returns></returns>
        public void TxtValidate(string tbTxt, string emptErrMsg)
        {
            TxtValidate(null, tbTxt, emptErrMsg, EValidateType.None, null);
        }

        /// <summary>
        /// ��֤���ı�
        /// </summary>
        /// <param name="tbTxt">Ҫ��֤���ı�</param>
        /// <param name="emptErrMsg">Ϊ�յ���ʾ</param>
        /// <param name="eType">�ı�����</param>
        /// <param name="validateErrMsg">��֤�������ʾ</param>
        /// <returns></returns>
        public bool TxtValidate(string tbTxt, string emptErrMsg, EValidateType eType, string validateErrMsg)
        {
            return TxtValidate(null, tbTxt, emptErrMsg, eType, validateErrMsg);
        }

        /// <summary>
        /// ��֤�����Ƿ�Ϸ�
        /// </summary>
        /// <param name="tb">����ʾ����ΪAlertʱ,��Ҫ�ý���Ŀؼ�</param>
        /// <param name="tbTxt">��֤���ı�</param>
        /// <param name="emptErrMsg">Ϊ����ʾ����Ϣ</param>
        /// <param name="eType">������֤</param>
        /// <param name="validateErrMsg">������֤������Ϣ</param>
        ///<param name="resShow">��ʾ��ʽ</param>
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
                    #region ����������֤
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
        /// ��֤�ؼ�,������ؼ�Ϊѡ���ʱ��,��(exp=(�ؼ�.Text.Length==0))
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
        /// �����,�ؼ������ı�����Ϊ��,������exp�Ͳ���֤��,��ѡ�����
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
        /// ��֤�����Ƿ�Ϸ�����֤��������,�������
        /// </summary>
        /// <param name="tbTxt">��֤���ı�</param>
        /// <param name="emptErrMsg">Ϊ����ʾ����Ϣ</param>
        /// <param name="eType">������֤</param>
        /// <param name="validateErrMsg">������֤������Ϣ</param>
        /// <param name="exp">��֤���ʽ,��֤ͨ����true,��ͨ����</param>
        /// <param name="equalsErrMsg">��֤���ʽ������Ϣ</param>
        /// <returns></returns>
        public bool ExpValidate(string tbTxt, string emptErrMsg, EValidateType eType, string validateErrMsg, bool exp, string equalsErrMsg)
        {
            return ExpValidate(null, exp, equalsErrMsg) && TxtValidate(tbTxt, emptErrMsg, eType, validateErrMsg);
        }

        /// <summary>
        /// ��֤�����Ƿ�Ϸ�,����֤��������,�������
        /// </summary>
        /// <param name="tb">��֤���ı���ؼ�</param>
        /// <param name="emptErrMsg">Ϊ����ʾ����Ϣ</param>
        /// <param name="exp">��֤���ʽ,��֤ͨ����true,��ͨ����false,trueʱalert</param>
        /// <param name="equalsErrMsg">��֤���ʽ������Ϣ</param>
        /// <returns></returns>
        public bool ExpValidate(TextBox tb, string emptErrMsg, bool exp, string equalsErrMsg)
        {
            return ExpValidate(tb, emptErrMsg, EValidateType.None, null, exp, equalsErrMsg);
        }

        /// <summary>
        /// ��֤�����Ƿ�Ϸ�,����֤��������,�������
        /// </summary>
        /// <param name="tbTxt">Ϊ����ʾ����Ϣ</param>
        /// <param name="emptErrMsg">Ϊ����ʾ����Ϣ</param>
        /// <param name="exp">��֤���ʽ,��֤ͨ����true,��ͨ����false,trueʱalert</param>
        /// <param name="equalsErrMsg">��֤���ʽ������Ϣ</param>
        /// <returns></returns>
        public bool ExpValidate(string tbTxt, string emptErrMsg, bool exp, string equalsErrMsg)
        {
            return ExpValidate(tbTxt, emptErrMsg, EValidateType.None, null, exp, equalsErrMsg);
        }

        /// <summary>
        /// ��֤�����Ƿ�Ϸ�,����֤��������,�����
        /// </summary>
        /// <param name="exp">��֤���ʽ����1=1��'a'='a',��֤ͨ����true,��ͨ����false,trueʱalert</param>
        /// <param name="equalsErrMsg">��֤���ʽ������Ϣ</param>
        /// <returns></returns>
        public bool ExpValidate(bool exp, string equalsErrMsg)
        {
            return ExpValidate(null, exp, equalsErrMsg);
        }

        /// <summary>
        /// ��֤�����Ƿ�Ϸ�,����֤��������,�����
        /// </summary>
        /// <param name="tb">�Ľ���Ŀؼ�</param>
        /// <param name="exp">��֤���ʽ����1=1��'a'='a',��֤ͨ����true,��ͨ����false,trueʱalert</param>
        /// <param name="equalsErrMsg">��֤���ʽ������Ϣ</param>
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
        /// �����ַ���
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

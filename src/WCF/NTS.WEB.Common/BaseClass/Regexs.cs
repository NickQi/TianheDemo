using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NTS.WEB.Common
{
    public class Regexs
    {
        #region У���ַ����Ƿ�ֻ������ĸ������
        /// <summary>
        /// У���ַ����Ƿ�ֻ������ĸ������
        /// </summary>
        /// <param name="toVerified">��ҪУ����ַ���</param>
        /// <returns>true��ʾ����Ҫ��false��ʾ������Ҫ��</returns>
        public static bool IsOnlyLetterAndDigit(string toVerified)
        {
            Regex rx = new Regex(@"^[A-Za-z0-9]*$");
            return rx.IsMatch(toVerified.Trim(), 0);
        }
        #endregion

        #region ��֤�Ƿ���ͼƬ��������չ����
        /// <summary>
        /// ��֤�Ƿ���ͼƬ��������չ����
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsImage(string str)
        {
            Regex rx = new Regex(@"(.*)\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$");
            return rx.IsMatch(str.Trim(), 0);
        }
        #endregion

        #region �����Ƿ�������
        /// <summary>
        /// �����Ƿ�������
        /// </summary>
        /// <param name="str">��Ҫ������ַ���</param>
        /// <returns>�Ƿ�Ϊ������true��������false������</returns>
        public static bool IsInt(string str)
        {
            Regex rx = new Regex(@"^[0123456789]+$");
            return rx.IsMatch(str);
        }
        #endregion

        #region У���Ƿ�Ϊ���ĸ�����
        /// <summary>
        /// У���Ƿ�Ϊ���ĸ�����
        /// </summary>
        /// <param name="price">��Ҫ������ַ���</param>
        /// <returns>�Ƿ�Ϊ�����㣬�Ƿ���true�����򷵻�false</returns>
        public static bool IsFloat(string str)
        {
            Regex rx = new Regex(@"^[0-9]*(.)?[0-9]+$", RegexOptions.IgnoreCase);
            return rx.IsMatch(str.Trim());
        }
        #endregion

        #region �����Ƿ�Ϊ����
        /// <summary>
        /// �����Ƿ�Ϊ����
        /// </summary>
        /// <param name="str">��Ҫ������ַ���</param>
        /// <returns>�Ƿ�Ϊ���֣�true�����ǣ�false�����</returns>
        public static bool IsNumber(string str)
        {

            Regex rx = new Regex(@"^[+-]?[0123456789]*[.]?[0123456789]*$");
            return rx.IsMatch(str);
        }
        #endregion

        #region �����ַ����Ƿ�Ϊ����ʱ��
        /// <summary>
        /// �����ַ����Ƿ�Ϊ����ʱ��
        /// </summary>
        /// <param name="str">��Ҫ������ַ���</param>
        /// <returns>�Ƿ�Ϊ����ʱ�䣺true�����ǣ�false�����</returns>
        public static bool IsDateTime(string str)
        {
            DateTime dt = DateTime.Now;
            return DateTime.TryParse(str, out dt);
        }
        #endregion

        #region �����ַ����Ƿ�Ϊ��������
        /// <summary>
        /// �����ַ����Ƿ�Ϊ��������
        /// </summary>
        /// <param name="str">��Ҫ������ַ���</param>
        /// <returns>�Ƿ�Ϊ�������룺true�����ǣ�false�����</returns>
        public static bool IsPostCode(string str)
        {
            Regex rx = new Regex(@"^[0123456789]{6}$");
            return rx.IsMatch(str);
        }
        #endregion

        #region �����ַ����Ƿ�Ϊ���֤��
        /// <summary>
        /// �����ַ����Ƿ�Ϊ���֤��
        /// </summary>
        /// <param name="str">��Ҫ������ַ���</param>
        /// <returns>�Ƿ�Ϊ���֤�ţ�true�����ǣ�false�����</returns>
        public static bool IsCode(string str)
        {
            //DXIDCard dxid = new DXIDCard(str);

            Regex rx = new Regex(@"^([\d]{15}|[\d]{18}|[\d]{17}[x|X])$");
            return rx.IsMatch(str.ToLower());
            //return dxid.Valid;
        }
        #endregion

        #region �����ַ����Ƿ�Ϊ�����ʼ�
        /// <summary>
        /// �����ַ����Ƿ�Ϊ�����ʼ�
        /// </summary>
        /// <param name="str">��Ҫ������ַ���</param>
        /// <returns>�Ƿ�Ϊ�����ʼ���true�����ǣ�false�����</returns>
        public static bool IsEMail(string str)
        {
            Regex rx = new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");
            return rx.IsMatch(str);
        }
        #endregion

        #region �����ַ����Ƿ�Ϊ�й������ĵ绰����
        /// <summary>
        /// �����ַ����Ƿ�Ϊ�й������ĵ绰����
        /// </summary>
        /// <param name="str">��Ҫ������ַ���</param>
        /// <returns>�Ƿ�Ϊ�й������ĵ绰���룺true�����ǣ�false�����</returns>
        public static bool IsPhoneNumber(string str)
        {
            Regex rx = new Regex(@"^\d{3,4}-\d{7,8}$");
            return rx.IsMatch(str);
        }
        #endregion

        #region �����ַ����Ƿ�Ϊ����
        /// <summary>
        /// �����ַ����Ƿ�Ϊ����
        /// </summary>
        /// <param name="str">��Ҫ������ַ���</param>
        /// <returns>�Ƿ�Ϊ���֣�true�����ǣ�false�����</returns>
        public static bool IsChinese(string str)
        {
            Regex rx = new Regex(@"u4e00-u9fa5");
            return rx.IsMatch(str);
        }
        #endregion

        #region �����ַ����Ƿ�Ϊ˫�ֽ��ַ�(��������)
        /// <summary>
        /// �����ַ����Ƿ�Ϊ˫�ֽ��ַ�(��������)
        /// </summary>
        /// <param name="str">��Ҫ������ַ���</param>
        /// <returns>�Ƿ�Ϊ˫�ֽ��ַ���true�����ǣ�false�����</returns>
        public static bool IsDoubleByteChar(string str)
        {
            Regex rx = new Regex(@"[^x00-xff]");
            return rx.IsMatch(str);
        }
        #endregion

        #region �����ַ����Ƿ�ΪURL��ַ
        /// <summary>
        /// �����ַ����Ƿ�ΪURL��ַ
        /// </summary>
        /// <param name="str">��Ҫ������ַ���</param>
        /// <returns>�Ƿ�ΪURL��ַ��true�����ǣ�false�����</returns>
        public static bool IsURLAddress(string str)
        {
            Regex rx = new Regex(@"[a-zA-z]+://[^s]*");
            return rx.IsMatch(str);
        }
        #endregion

        #region �����ַ����Ƿ�ΪIP��ַ
        /// <summary>
        /// �����ַ����Ƿ�ΪIP��ַ
        /// </summary>
        /// <param name="str">��Ҫ������ַ���</param>
        /// <returns>�Ƿ�ΪIP��ַ��true�����ǣ�false�����</returns>
        public static bool IsIPAddress(string str)
        {
            string p = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
            Regex rx = new Regex(p);
            return rx.IsMatch(str);
        }
        #endregion

        #region У���ֻ����룺���������ֿ�ͷ���������⣬�ɺ��С�-��
        /// <summary>
        /// У���ֻ����룺���������ֿ�ͷ���������⣬�ɺ��С�-�� 
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobil(string mobile)
        {
            Regex objAlphaNumericPattern = new Regex(@"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$");
            return objAlphaNumericPattern.IsMatch(mobile);
        }
        #endregion

        #region У����ͨ�绰��������룺���ԡ�+����ͷ���������⣬�ɺ��С�-��
        /// <summary>
        /// У����ͨ�绰��������룺���ԡ�+����ͷ���������⣬�ɺ��С�-�� 
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public static bool IsTelepone(string tel)
        {
            Regex objAlphaNumericPattern = new Regex(@"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$");
            return objAlphaNumericPattern.IsMatch(tel);
        }
        #endregion

        #region У���¼����ֻ������5-20������ĸ��ͷ���ɴ����֡���_������.�����ִ�
        /// <summary>
        /// У���¼����ֻ������5-20������ĸ��ͷ���ɴ����֡���_������.�����ִ�
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool IsRegUserName(string userName)
        {
            Regex objAlphaNumericPattern = new Regex("^[a-zA-Z]{1}([a-zA-Z0-9]|[._]){5,19}$");
            return objAlphaNumericPattern.IsMatch(userName);
        }
        #endregion
    }
}

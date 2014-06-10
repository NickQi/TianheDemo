using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NTS.WEB.Common
{
    public class Regexs
    {
        #region 校验字符串是否只包含字母与数字
        /// <summary>
        /// 校验字符串是否只包含字母与数字
        /// </summary>
        /// <param name="toVerified">需要校验的字符串</param>
        /// <returns>true表示符合要求，false表示不符合要求</returns>
        public static bool IsOnlyLetterAndDigit(string toVerified)
        {
            Regex rx = new Regex(@"^[A-Za-z0-9]*$");
            return rx.IsMatch(toVerified.Trim(), 0);
        }
        #endregion

        #region 验证是否是图片（根据扩展名）
        /// <summary>
        /// 验证是否是图片（根据扩展名）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsImage(string str)
        {
            Regex rx = new Regex(@"(.*)\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$");
            return rx.IsMatch(str.Trim(), 0);
        }
        #endregion

        #region 检验是否是整数
        /// <summary>
        /// 检验是否是整数
        /// </summary>
        /// <param name="str">需要检验的字符串</param>
        /// <returns>是否为整数：true是整数，false非整数</returns>
        public static bool IsInt(string str)
        {
            Regex rx = new Regex(@"^[0123456789]+$");
            return rx.IsMatch(str);
        }
        #endregion

        #region 校验是否为正的浮点数
        /// <summary>
        /// 校验是否为正的浮点数
        /// </summary>
        /// <param name="price">需要检验的字符串</param>
        /// <returns>是否为正浮点，是返回true，否则返回false</returns>
        public static bool IsFloat(string str)
        {
            Regex rx = new Regex(@"^[0-9]*(.)?[0-9]+$", RegexOptions.IgnoreCase);
            return rx.IsMatch(str.Trim());
        }
        #endregion

        #region 检验是否为数字
        /// <summary>
        /// 检验是否为数字
        /// </summary>
        /// <param name="str">需要检验的字符串</param>
        /// <returns>是否为数字：true代表是，false代表否</returns>
        public static bool IsNumber(string str)
        {

            Regex rx = new Regex(@"^[+-]?[0123456789]*[.]?[0123456789]*$");
            return rx.IsMatch(str);
        }
        #endregion

        #region 检验字符串是否为日期时间
        /// <summary>
        /// 检验字符串是否为日期时间
        /// </summary>
        /// <param name="str">需要检验的字符串</param>
        /// <returns>是否为日期时间：true代表是，false代表否</returns>
        public static bool IsDateTime(string str)
        {
            DateTime dt = DateTime.Now;
            return DateTime.TryParse(str, out dt);
        }
        #endregion

        #region 检验字符串是否为邮政编码
        /// <summary>
        /// 检验字符串是否为邮政编码
        /// </summary>
        /// <param name="str">需要检验的字符串</param>
        /// <returns>是否为邮政编码：true代表是，false代表否</returns>
        public static bool IsPostCode(string str)
        {
            Regex rx = new Regex(@"^[0123456789]{6}$");
            return rx.IsMatch(str);
        }
        #endregion

        #region 检验字符串是否为身份证号
        /// <summary>
        /// 检验字符串是否为身份证号
        /// </summary>
        /// <param name="str">需要检验的字符串</param>
        /// <returns>是否为身份证号：true代表是，false代表否</returns>
        public static bool IsCode(string str)
        {
            //DXIDCard dxid = new DXIDCard(str);

            Regex rx = new Regex(@"^([\d]{15}|[\d]{18}|[\d]{17}[x|X])$");
            return rx.IsMatch(str.ToLower());
            //return dxid.Valid;
        }
        #endregion

        #region 检验字符串是否为电子邮件
        /// <summary>
        /// 检验字符串是否为电子邮件
        /// </summary>
        /// <param name="str">需要检验的字符串</param>
        /// <returns>是否为电子邮件：true代表是，false代表否</returns>
        public static bool IsEMail(string str)
        {
            Regex rx = new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");
            return rx.IsMatch(str);
        }
        #endregion

        #region 检验字符串是否为中国地区的电话号码
        /// <summary>
        /// 检验字符串是否为中国地区的电话号码
        /// </summary>
        /// <param name="str">需要检验的字符串</param>
        /// <returns>是否为中国地区的电话号码：true代表是，false代表否</returns>
        public static bool IsPhoneNumber(string str)
        {
            Regex rx = new Regex(@"^\d{3,4}-\d{7,8}$");
            return rx.IsMatch(str);
        }
        #endregion

        #region 检验字符串是否为汉字
        /// <summary>
        /// 检验字符串是否为汉字
        /// </summary>
        /// <param name="str">需要检验的字符串</param>
        /// <returns>是否为汉字：true代表是，false代表否</returns>
        public static bool IsChinese(string str)
        {
            Regex rx = new Regex(@"u4e00-u9fa5");
            return rx.IsMatch(str);
        }
        #endregion

        #region 检验字符串是否为双字节字符(包括汉字)
        /// <summary>
        /// 检验字符串是否为双字节字符(包括汉字)
        /// </summary>
        /// <param name="str">需要检验的字符串</param>
        /// <returns>是否为双字节字符：true代表是，false代表否</returns>
        public static bool IsDoubleByteChar(string str)
        {
            Regex rx = new Regex(@"[^x00-xff]");
            return rx.IsMatch(str);
        }
        #endregion

        #region 检验字符串是否为URL地址
        /// <summary>
        /// 检验字符串是否为URL地址
        /// </summary>
        /// <param name="str">需要检验的字符串</param>
        /// <returns>是否为URL地址：true代表是，false代表否</returns>
        public static bool IsURLAddress(string str)
        {
            Regex rx = new Regex(@"[a-zA-z]+://[^s]*");
            return rx.IsMatch(str);
        }
        #endregion

        #region 检验字符串是否为IP地址
        /// <summary>
        /// 检验字符串是否为IP地址
        /// </summary>
        /// <param name="str">需要检验的字符串</param>
        /// <returns>是否为IP地址：true代表是，false代表否</returns>
        public static bool IsIPAddress(string str)
        {
            string p = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
            Regex rx = new Regex(p);
            return rx.IsMatch(str);
        }
        #endregion

        #region 校验手机号码：必须以数字开头，除数字外，可含有“-”
        /// <summary>
        /// 校验手机号码：必须以数字开头，除数字外，可含有“-” 
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsMobil(string mobile)
        {
            Regex objAlphaNumericPattern = new Regex(@"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$");
            return objAlphaNumericPattern.IsMatch(mobile);
        }
        #endregion

        #region 校验普通电话、传真号码：可以“+”开头，除数字外，可含有“-”
        /// <summary>
        /// 校验普通电话、传真号码：可以“+”开头，除数字外，可含有“-” 
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public static bool IsTelepone(string tel)
        {
            Regex objAlphaNumericPattern = new Regex(@"^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$");
            return objAlphaNumericPattern.IsMatch(tel);
        }
        #endregion

        #region 校验登录名：只能输入5-20个以字母开头、可带数字、“_”、“.”的字串
        /// <summary>
        /// 校验登录名：只能输入5-20个以字母开头、可带数字、“_”、“.”的字串
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

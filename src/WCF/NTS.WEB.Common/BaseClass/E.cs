using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.WEB.Common
{
    /// <summary>
    /// 验证的枚举
    /// </summary>
    public enum EValidateType
    {
        /// <summary>
        /// 校验字符串是否只包含字母与数字
        /// </summary>
        IsOnlyLetterAndDigit,
        /// <summary>
        /// 验证是否是图片（根据扩展名）
        /// </summary>
        IsImage,
        /// <summary>
        /// 检验是否是整数
        /// </summary>
        IsInt,
        /// <summary>
        /// 校验是否为正的浮点数
        /// </summary>
        IsFloat,
        /// <summary>
        /// 检验是否为数字
        /// </summary>
        IsNumber,
        /// <summary>
        /// 检验字符串是否为日期时间
        /// </summary>
        IsDateTime,
        /// <summary>
        /// 检验字符串是否为邮政编码
        /// </summary>
        IsPostCode,
        /// <summary>
        /// 检验字符串是否为身份证号
        /// </summary>
        IsCode,
        /// <summary>
        /// 检验字符串是否为电子邮件
        /// </summary>
        IsEMail,
        /// <summary>
        /// 检验字符串是否为中国地区的电话号码
        /// </summary>
        IsPhoneNumber,
        /// <summary>
        /// 检验字符串是否为汉字
        /// </summary>
        IsChinese,
        /// <summary>
        /// 检验字符串是否为双字节字符(包括汉字)
        /// </summary>
        IsDoubleByteChar,
        /// <summary>
        /// 检验字符串是否为URL地址
        /// </summary>
        IsURLAddress,
        /// <summary>
        /// 检验字符串是否为IP地址
        /// </summary>
        IsIPAddress,
        /// <summary>
        ///  校验手机号码：必须以数字开头，除数字外，可含有“-”
        /// </summary>
        IsMobil,
        /// <summary>
        /// 校验普通电话、传真号码：可以“+”开头，除数字外，可含有“-”
        /// </summary>
        IsTelepone,
        /// <summary>
        /// 校验登录名：只能输入5-20个以字母开头、可带数字、“_”、“.”的字串
        /// </summary>
        IsRegUserName,
        /// <summary>
        /// 没有任何验证
        /// </summary>
        None
    }
    /// <summary>
    /// 表现形式
    /// </summary>
    public enum EDisplay
    {
        /// <summary>
        /// 弹出框,如果是基本控件,会获取焦点
        /// </summary>
        Alert,
        /// <summary>
        /// 不执行任何操作
        /// </summary>
        None
    }
}

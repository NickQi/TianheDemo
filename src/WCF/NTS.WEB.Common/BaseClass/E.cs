using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.WEB.Common
{
    /// <summary>
    /// ��֤��ö��
    /// </summary>
    public enum EValidateType
    {
        /// <summary>
        /// У���ַ����Ƿ�ֻ������ĸ������
        /// </summary>
        IsOnlyLetterAndDigit,
        /// <summary>
        /// ��֤�Ƿ���ͼƬ��������չ����
        /// </summary>
        IsImage,
        /// <summary>
        /// �����Ƿ�������
        /// </summary>
        IsInt,
        /// <summary>
        /// У���Ƿ�Ϊ���ĸ�����
        /// </summary>
        IsFloat,
        /// <summary>
        /// �����Ƿ�Ϊ����
        /// </summary>
        IsNumber,
        /// <summary>
        /// �����ַ����Ƿ�Ϊ����ʱ��
        /// </summary>
        IsDateTime,
        /// <summary>
        /// �����ַ����Ƿ�Ϊ��������
        /// </summary>
        IsPostCode,
        /// <summary>
        /// �����ַ����Ƿ�Ϊ���֤��
        /// </summary>
        IsCode,
        /// <summary>
        /// �����ַ����Ƿ�Ϊ�����ʼ�
        /// </summary>
        IsEMail,
        /// <summary>
        /// �����ַ����Ƿ�Ϊ�й������ĵ绰����
        /// </summary>
        IsPhoneNumber,
        /// <summary>
        /// �����ַ����Ƿ�Ϊ����
        /// </summary>
        IsChinese,
        /// <summary>
        /// �����ַ����Ƿ�Ϊ˫�ֽ��ַ�(��������)
        /// </summary>
        IsDoubleByteChar,
        /// <summary>
        /// �����ַ����Ƿ�ΪURL��ַ
        /// </summary>
        IsURLAddress,
        /// <summary>
        /// �����ַ����Ƿ�ΪIP��ַ
        /// </summary>
        IsIPAddress,
        /// <summary>
        ///  У���ֻ����룺���������ֿ�ͷ���������⣬�ɺ��С�-��
        /// </summary>
        IsMobil,
        /// <summary>
        /// У����ͨ�绰��������룺���ԡ�+����ͷ���������⣬�ɺ��С�-��
        /// </summary>
        IsTelepone,
        /// <summary>
        /// У���¼����ֻ������5-20������ĸ��ͷ���ɴ����֡���_������.�����ִ�
        /// </summary>
        IsRegUserName,
        /// <summary>
        /// û���κ���֤
        /// </summary>
        None
    }
    /// <summary>
    /// ������ʽ
    /// </summary>
    public enum EDisplay
    {
        /// <summary>
        /// ������,����ǻ����ؼ�,���ȡ����
        /// </summary>
        Alert,
        /// <summary>
        /// ��ִ���κβ���
        /// </summary>
        None
    }
}

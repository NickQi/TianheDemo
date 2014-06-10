#region ?Copyright 2005, Rudy.net's Stuff - Rudy Guzman, XmlHelper
// XmlHelper
// 
// ?Copyright 2005, Rudy.net's Stuff - Rudy Guzman
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution. 
//  * Neither the name of Rudy.net, XmlHelper, nor the names of its contributors 
//    may be used to endorse or promote products derived from this software
//    without specific prior written permission. 
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
// EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Xml.Serialization;


namespace NTS.WEB.Common
{
    public static class XmlHelper
    {
        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "    ";
            using (XmlWriter write = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(write, o);
                write.Close();
            }


        }

        /// <summary>
        /// ��һ���������л�ΪXML�ַ���
        /// </summary>
        /// <param name="o">Ҫ���л��Ķ���</param>
        /// <param name="encoding">���뷽ʽ</param>
        /// <returns>���л�������XML�ַ���</returns>
        public static string XmlSerialize(object o, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, o, encoding);
                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// ��һ������XML���л��ķ�ʽд�뵽һ���ļ�
        /// </summary>
        /// <param name="o">Ҫ���л��Ķ���</param>
        /// <param name="encoding">���뷽ʽ</param>
        /// <param name="path">�����ļ�·��</param>
        public static void XmlSerialize2File(object o, Encoding encoding, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlSerializeInternal(file, o, encoding);
            }
        }

        /// <summary>
        /// ��XML�ַ����з����л�����
        /// </summary>
        /// <typeparam name="T">�����������</typeparam>
        /// <param name="s">���������XML�ַ���</param>
        /// <param name="endoding">���뷽ʽ</param>
        /// <returns>�����л��õ��Ķ���</returns>
        public static T XmlDeserialize<T>(string s, Encoding endoding)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("s");
            }
            if (endoding == null)
            {
                throw new ArgumentNullException("encoding");

            }
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(endoding.GetBytes(s)))
            {
                using (StreamReader sr = new StreamReader(ms, endoding))
                {
                    return (T)mySerializer.Deserialize(sr);
                }
            }
        }

        /// <summary>
        /// ����һ���ļ�������XML�ķ�ʽ�����л����� 
        /// </summary>
        /// <typeparam name="T">�����������</typeparam>
        /// <param name="path">�ļ�·��</param>
        /// <param name="encoding">���뷽ʽ</param>
        /// <returns>�����л��õ��Ķ���</returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");

            }
            string xml = File.ReadAllText(path, encoding);
            return XmlDeserialize<T>(xml, encoding);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTS.WEB.ResultView
{
    public class ResultReal
    {
        public ExecuteProcess ActionInfo { get; set; }
        public DeviceInfo Info { get; set; }

        public RealData  Data{ get; set; }
        
    }


    public class RealData
    {
        public List<dataUnit> Analog { get; set; }
        public List<dataUnit> Pulse { get; set; }
        public List<dataUnit> Switch { get; set; }

    }


    public class dataUnit
    {
        public int Id { get; set; }
        public string  DataName { get; set; }
        public decimal Value { get; set; }
        public string Unit { get; set; }
    }

    public class DeviceInfo
    {
        /// <summary>
        /// 通讯状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string DevType { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 性质
        /// </summary>
        public string Nature { get; set; }

      

        /// <summary>
        /// 隶属机构
        /// </summary>
        public string Affiliations { get; set; }

        /// <summary>
        /// 安装位置
        /// </summary>
        public string Location { get; set; }


        /// <summary>
        /// 额定功率
        /// </summary>
        public string Rating { get; set; }
    }
}

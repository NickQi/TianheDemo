namespace NTS.WEB.Model
{
    /*显示的图形的种类*/
    public enum ChartType
    {
        /// <summary>
        /// 统计图
        /// </summary>
        CommChart=0, // 统计图
        /// <summary>
        /// 对比图
        /// </summary>
        ComparisonChart, // 对比图
        /// <summary>
        /// 数据表
        /// </summary>
        TableChart, // 数据表
        /// <summary>
        /// 饼图
        /// </summary>
        PieChart, // 饼图
        /// <summary>
        /// 导出Excel
        /// </summary>
        ExcelData
    }
}

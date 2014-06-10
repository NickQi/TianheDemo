using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace NTS.WEB.Common
{
    public class ExportHelper
    {
        private static HSSFWorkbook hssfworkbook;

        /// <summary>
        /// 导出Excel文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path"></param>
        public static void ExportExcel(DataTable dt, string path)
        {
            hssfworkbook = new HSSFWorkbook();

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            hssfworkbook.DocumentSummaryInformation = dsi;

            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            hssfworkbook.SummaryInformation = si;


            ISheet sheet1 = hssfworkbook.CreateSheet(string.IsNullOrEmpty(dt.TableName) ? "Sheet1" : dt.TableName);
            IRow rowTitle = sheet1.CreateRow(0);
            ICell cellTitle;

            ICellStyle style = hssfworkbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.CENTER;
            style.VerticalAlignment = VerticalAlignment.CENTER;

            for (int m = 0; m < dt.Columns.Count; m++)
            {
                cellTitle = rowTitle.CreateCell(m);
                cellTitle.CellStyle = style;
                cellTitle.SetCellValue(dt.Columns[m].ColumnName);
            }

            IRow row;
            ICell cell;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    row = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.CellStyle = style;
                        string obj = dt.Rows[i][j].ToString();
                        cell.SetCellValue(obj);
                    }
                }
            }

            var file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
        }

        /// <summary>
        /// 导出Excel文件,并合并指定单元格
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path"></param>
        /// <param name="param"></param>
        public static void ExportExcel(DataTable dt, string path, List<MergeCellParam> param)
        {
            hssfworkbook = new HSSFWorkbook();

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            hssfworkbook.DocumentSummaryInformation = dsi;

            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            hssfworkbook.SummaryInformation = si;


            ISheet sheet1 = hssfworkbook.CreateSheet(string.IsNullOrEmpty(dt.TableName) ? "Sheet1" : dt.TableName);
            IRow rowTitle = sheet1.CreateRow(0);
            ICell cellTitle;

            ICellStyle style = hssfworkbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.CENTER;
            style.VerticalAlignment = VerticalAlignment.CENTER;


            for (int m = 0; m < dt.Columns.Count; m++)
            {
                cellTitle = rowTitle.CreateCell(m);
                cellTitle.CellStyle = style;
                cellTitle.SetCellValue(dt.Columns[m].ColumnName);
            }

            IRow row;
            ICell cell;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    row = sheet1.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        cell = row.CreateCell(j);
                        cell.CellStyle = style;
                        string obj = dt.Rows[i][j].ToString();
                        cell.SetCellValue(obj);
                    }
                }
            }

            if (param.Count > 0)
            {
                for (int i = 0; i < param.Count; i++)
                {
                    sheet1.AddMergedRegion(new CellRangeAddress(param[i].FirstRow, param[i].LastRow, param[i].FirstColumn, param[i].LastColumn));
                }
            }

            var file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
        }

        /// <summary>
        /// 从模版中导出Excel文件
        /// </summary>
        /// <param name="dt">数据源DataTable</param>
        /// <param name="path">导出文件路径</param>
        /// <param name="templatePath">模版路径</param>
        /// <param name="param"></param>
        public static void ExportExcel(DataTable dt,string path ,string templatePath, TemplateParam param)
        {
            FileStream templateFile = new FileStream(templatePath, FileMode.Open, FileAccess.Read);//读取模版文件
            hssfworkbook = new HSSFWorkbook(templateFile);

            ISheet sheet1 = hssfworkbook.GetSheet(string.IsNullOrEmpty(dt.TableName) ? "费用分摊表" : dt.TableName);//获取第一张工作表

            ICellStyle style = hssfworkbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.CENTER;
            style.VerticalAlignment = VerticalAlignment.CENTER;

            //插入标题
            if (param.TitleCell != null)
            {
                IRow rowTitle = sheet1.GetRow(param.TitleCell.Row);
                ICell cellTitle = rowTitle.GetCell(param.TitleCell.Cell);
                if (!string.IsNullOrEmpty(param.Title))
                    cellTitle.SetCellValue(param.Title + cellTitle.StringCellValue);
            }

            //插入副标题
            if (param.SubTitleCell != null)
            {
                IRow rowSubTitle = sheet1.GetRow(param.SubTitleCell.Row);
                ICell cellSubTitle = rowSubTitle.GetCell(param.SubTitleCell.Cell);
                if (!string.IsNullOrEmpty(param.SubTitle))
                    cellSubTitle.SetCellValue(param.SubTitle);
            }

            //插入单位
            if (param.ItemUnitCell != null)
            {
                IRow rowItemUnit = sheet1.GetRow(param.ItemUnitCell.Row);
                ICell cellItemUnit = rowItemUnit.GetCell(param.ItemUnitCell.Cell);
                if (!string.IsNullOrEmpty(param.ItemUnit))
                    cellItemUnit.SetCellValue(param.ItemUnit);
            }

            int columnCount = param.DataColumn.Length > 0 ? param.DataColumn.Length : dt.Columns.Count;
            //插入表格内容
            if (param.DataCell != null)
            {
                if (param.DataTitle)
                {
                    IRow rowDataTitle = sheet1.GetRow(param.DataCell.Row);
                    ICell cellDataTitle;
                    for (int m = 0; m < columnCount; m++)
                    {
                        cellDataTitle = rowDataTitle.GetCell(m);
                        cellDataTitle.CellStyle = style;
                        string title = dt.Columns[param.DataColumn[m]].ColumnName;
                        if (m == param.SortColumn)
                            title = "排序";
                        cellDataTitle.SetCellValue(title);
                    }
                }

                IRow row;
                ICell cell;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        row = sheet1.GetRow(i + param.DataCell.Row + 1) ?? sheet1.CreateRow(i + param.DataCell.Row + 1);
                        for (int j = 0; j < columnCount; j++)
                        {
                            cell = row.GetCell(j) ?? row.CreateCell(j);
                            cell.CellStyle = style;
                            object obj = dt.Rows[i][param.DataColumn[j]];

                            if (j == param.SortColumn)
                                obj = i + 1;

                            if (param.SpecialColumn.Count > 0)
                            {
                                if (param.SpecialColumn.ContainsKey(j))
                                {
                                    obj = param.SpecialColumn[j];
                                }
                            }

                            if (param.FormatDateColumn.Count > 0)
                            {
                                if (param.FormatDateColumn.ContainsKey(j))
                                {
                                    DateTime date = Convert.ToDateTime(dt.Rows[i][param.DataColumn[j]].ToString());
                                    obj = date.ToString(param.FormatDateColumn[j]);
                                }
                            }

                            SetCellValue(cell, obj, dt.Columns[param.DataColumn[j]]);
                        }
                    }
                }
            }

            sheet1.ForceFormulaRecalculation = true;

            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
        }

        /// <summary>
        /// 导出Txt文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path"></param>
        public static void ExportTxt(DataTable dt, string path)
        {
            string st = null;
            var fsTxtFile = new FileStream(path, FileMode.Create, FileAccess.Write);
            var swTxtFile = new StreamWriter(fsTxtFile, Encoding.GetEncoding("gb2312"));

            for (int rows = 0; rows < dt.Rows.Count; rows++)
            {
                if (rows == 0)
                {
                    for (int cols = 0; cols < dt.Columns.Count; cols++)
                    {
                        st = st + dt.Columns[cols].ColumnName + ",\t";
                    }
                    st = st + "\r\n";
                }
                for (int cols = 0; cols < dt.Columns.Count; cols++)
                {
                    st = st + dt.Rows[rows][cols] + ",\t";
                }
                st = st + "\r\n";
            }
            swTxtFile.Write(st);
            swTxtFile.Flush();
            swTxtFile.Close();
            fsTxtFile.Close();
        }

        /// <summary>
        /// 导出HTML文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path"></param>
        public static void ExportHtml(DataTable dt, string path)
        {
            var fsTxtFile = new FileStream(path, FileMode.Create, FileAccess.Write);
            var swTxtFile = new StreamWriter(fsTxtFile, Encoding.GetEncoding("gb2312"));

            var data = new StringBuilder();
            data.Append("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            data.Append("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            foreach (DataColumn column in dt.Columns)
            {
                data.Append("<td>");
                data.Append(column.ColumnName);
                data.Append("</td>");
            }
            data.Append("</tr>");
            foreach (DataRow row in dt.Rows)
            {
                data.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    data.Append("<td>");
                    data.Append(row[column]);
                    data.Append("</td>");
                }
                data.Append("</tr>");
            }
            data.Append("</table>");

            swTxtFile.Write(data.ToString());
            swTxtFile.Flush();
            swTxtFile.Close();
            fsTxtFile.Close();
        }

        /// <summary>
        /// 根据数据类型设置单元格内容
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="obj"></param>
        /// <param name="dc"></param>
        private static void SetCellValue(ICell cell, object obj, DataColumn dc)
        {
            if (dc.DataType == typeof(bool))
            {
                cell.SetCellValue((bool)obj);
            }
            else if (dc.DataType == typeof(int))
            {
                cell.SetCellValue((Int32)obj);
            }
            else if (dc.DataType == typeof(decimal))
            {
                cell.SetCellValue(Convert.ToDouble(obj));
            }
            else
            {
                cell.SetCellValue(obj.ToString());
            }
        }
    }

    public class MergeCellParam
    {
        public MergeCellParam(int firstRow, int lastRow, int firstColumn, int lastColumn)
        {
            FirstRow = firstRow;
            FirstColumn = firstColumn;
            LastRow = lastRow;
            LastColumn = lastColumn;
        }

        private int _FirstRow;
        private int _FirstColumn;
        private int _LastRow;
        private int _LastColumn;

        /// <summary>
        /// 区域中第一个单元格的行号
        /// </summary>
        public int FirstRow
        {
            get { return _FirstRow; }
            set { _FirstRow = value; }
        }

        /// <summary>
        /// 区域中第一个单元格的列号
        /// </summary>
        public int FirstColumn
        {
            get { return _FirstColumn; }
            set { _FirstColumn = value; }
        }

        /// <summary>
        /// 区域中最后一个单元格的行号
        /// </summary>
        public int LastRow
        {
            get { return _LastRow; }
            set { _LastRow = value; }
        }

        /// <summary>
        /// 区域中最后一个单元格的列号
        /// </summary>
        public int LastColumn
        {
            get { return _LastColumn; }
            set { _LastColumn = value; }
        }
    }
    public class CellParam
    {
        public CellParam(int row, int cell)
        {
            Row = row;
            Cell = cell;
        }

        private int m_Row;
        private int m_Cell;

        public int Row
        {
            get { return m_Row; }
            set { m_Row = value; }
        }

        public int Cell
        {
            get { return m_Cell; }
            set { m_Cell = value; }
        }
    }
    public class TemplateParam
    {
        public TemplateParam()
        {
        }

        /// <summary>
        /// Excel模板导出参数
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="titleCell">标题单元格位置</param>
        /// <param name="subTitle">副标题</param>
        /// <param name="subTitleCell">副标题单元格位置</param>
        /// <param name="dataTitle">显示数据列标题</param>
        /// <param name="dataCell">数据单元格位置</param>
        public TemplateParam(string title, CellParam titleCell, string subTitle, CellParam subTitleCell, bool dataTitle, CellParam dataCell)
        {
            Title = title;
            TitleCell = titleCell;
            SubTitle = subTitle;
            SubTitleCell = subTitleCell;
            DataTitle = dataTitle;
            DataCell = dataCell;
        }

        private string m_Title;//标题
        private CellParam m_TitleCell;//标题单元格位置
        private string m_SubTitle;//副标题
        private CellParam m_SubTitleCell;//副标题单元格位置
        private string m_ItemUnit;//数据单位
        private CellParam m_ItemUnitCell;//数据单位单元格位置
        private bool m_DataTitle;//显示数据列标题
        private int m_SortColumn = -1;//排序列
        private CellParam m_DataCell;//数据单元格位置
        private int[] m_DataColumn;//DataTable中要现实的数据列
        private Dictionary<int, string> m_SpecialColumn = new Dictionary<int, string>();//设置特殊列的值
        private Dictionary<int, string> m_FormatDateColumn = new Dictionary<int, string>();//设置格式化列的值

        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        public CellParam TitleCell
        {
            get { return m_TitleCell; }
            set { m_TitleCell = value; }
        }

        public string SubTitle
        {
            get { return m_SubTitle; }
            set { m_SubTitle = value; }
        }

        public CellParam SubTitleCell
        {
            get { return m_SubTitleCell; }
            set { m_SubTitleCell = value; }
        }

        public CellParam DataCell
        {
            get { return m_DataCell; }
            set { m_DataCell = value; }
        }

        public int[] DataColumn
        {
            get { return m_DataColumn; }
            set { m_DataColumn = value; }
        }

        public bool DataTitle
        {
            get { return m_DataTitle; }
            set { m_DataTitle = value; }
        }

        public int SortColumn
        {
            get { return m_SortColumn; }
            set { m_SortColumn = value; }
        }

        public string ItemUnit
        {
            get { return m_ItemUnit; }
            set { m_ItemUnit = value; }
        }

        public CellParam ItemUnitCell
        {
            get { return m_ItemUnitCell; }
            set { m_ItemUnitCell = value; }
        }

        public Dictionary<int, string> SpecialColumn
        {
            get { return m_SpecialColumn; }
            set { m_SpecialColumn = value; }
        }

        public Dictionary<int, string> FormatDateColumn
        {
            get { return m_FormatDateColumn; }
            set { m_FormatDateColumn = value; }
        }
    }
}
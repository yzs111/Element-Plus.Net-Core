using Microsoft.AspNetCore.Mvc;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;

namespace CW_ToyShopping.Common.Helpers
{
   public class ExcelHelper
    {
        /// <summary>
        /// 描 述：Excel导入导出设置
        /// </summary>
        public class ExcelConfig
        {
            /// <summary>
            /// 标题
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 前景色
            /// </summary>
            public Color ForeColor { get; set; }

            /// <summary>
            /// 背景色
            /// </summary>
            public Color Background { get; set; }
      
            private short _titlepoint;
            /// <summary>
            /// 标题字号
            /// </summary>
            public short TitlePoint
            {
                get
                {
                    if (_titlepoint == 0)
                    {
                        return 20;
                    }
                    else
                    {
                        return _titlepoint;
                    }
                }
                set { _titlepoint = value; }
            }
            
            private short _headpoint;
            /// <summary>
            /// 列头字号
            /// </summary>
            public short HeadPoint
            {
                get
                {
                    if (_headpoint == 0)
                    {
                        return 10;
                    }
                    else
                    {
                        return _headpoint;
                    }
                }
                set { _headpoint = value; }
            }
            /// <summary>
            /// 标题高度
            /// </summary>
            public short TitleHeight { get; set; }
            /// <summary>
            /// 列标题高度
            /// </summary>
            public short HeadHeight { get; set; }

            private string _titlefont;

            /// <summary>
            /// 标题字体
            /// </summary>
            public string TitleFont
            {
                get
                {
                    if (_titlefont == null)
                    {
                        return "微软雅黑";
                    }
                    else
                    {
                        return _titlefont;
                    }
                }
                set { _titlefont = value; }
            }
        
            private string _headfont;

            /// <summary>
            /// 列头字体
            /// </summary>
            public string HeadFont
            {
                get
                {
                    if (_headfont == null)
                    {
                        return "微软雅黑";
                    }
                    else
                    {
                        return _headfont;
                    }
                }
                set { _headfont = value; }
            }
            /// <summary>
            /// 是否按内容长度来适应表格宽度
            /// </summary>
            public bool IsAllSizeColumn { get; set; }

            /// <summary>
            /// 列设置
            /// </summary>
            public List<ColumnModel> ColumnEntity { get; set; }
        }

        /// <summary>
        /// 描 述：Excel导入导出列设置模型
        /// </summary>
        public class ColumnModel
        {
            /// <summary>
            /// 列名
            /// </summary>
            public string Column { get; set; }
            /// <summary>
            /// Excel列名
            /// </summary>
            public string ExcelColumn { get; set; }
            /// <summary>
            /// 宽度
            /// </summary>
            public int Width { get; set; }
            /// <summary>
            /// 前景色
            /// </summary>
            public Color ForeColor { get; set; }
            /// <summary>
            /// 背景色
            /// </summary>
            public Color Background { get; set; }
            /// <summary>
            /// 字体
            /// </summary>
            public string Font { get; set; }
            /// <summary>
            /// 字号
            /// </summary>
            public short Point { get; set; }
            /// <summary>
            ///对齐方式
            ///left 左
            ///center 中间
            ///right 右
            ///fill 填充
            ///justify 两端对齐
            ///centerselection 跨行居中
            ///distributed
            /// </summary>
            public string Alignment { get; set; }

            /// <summary>
            ///  当前值类型
            /// </summary>
            public string DataType { get; set; }

            /// <summary>
            /// 列是否隐藏
            /// </summary>
            public bool Hidden { get; set; }
        }

        public class ExcelGridModel
        {
            /// <summary>
            /// 属性名称
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// excel列名
            /// </summary>
            public string label { get; set; }
            /// <summary>
            /// 宽度
            /// </summary>
            public string width { get; set; }
            /// <summary>
            /// 对其方式
            /// </summary>
            public string align { get; set; }
            /// <summary>
            /// 高度
            /// </summary>
            public string height { get; set; }
            /// <summary>
            /// 是否隐藏
            /// </summary>
            public string hidden { get; set; }
        }

        public class NPOIMemoryStream : MemoryStream
        {
            /// <summary>
            /// 获取流是否关闭
            /// </summary>
            public bool IsColse
            {
                get;
                set;
            }
            public NPOIMemoryStream()
            {
                IsColse = true;
            }
            public override void Close()
           {
               if (IsColse)
               {
                   base.Close();
               }
           }
        }

        public class Sheet
        {
            public string Name { get; set; }
            public List<ColumnModel> Columns { get; set; }
            public DataTable DataSource { get; set; }

            public Sheet()
            { }

            public Sheet(string name, List<ColumnModel> columns, DataTable dataSource)
            {
                Name = name;
                Columns = columns;
                DataSource = dataSource;
            }
        }

        public XSSFWorkbook workbook;

        public ExcelHelper()
        {
            workbook = new XSSFWorkbook();
        }

        public void RenderToExcelNew(Sheet sheetInfo)
        {
                if (string.IsNullOrWhiteSpace(sheetInfo.Name)) sheetInfo.Name = "Sheet" + workbook.NumberOfSheets + 1;
                XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet(sheetInfo.Name); // 创建一个新的Excel工作簿

                #region 右击文件 属性信息

                //{
                //    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                //    dsi.Company = "广西磐瑞科技有限公司@Excel";
                //    workbook.DocumentSummaryInformation = dsi;

                //    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                //    si.Author = "广西磐瑞科技有限公司"; //填加xls文件作者信息
                //    si.ApplicationName = "广西磐瑞科技有限公司"; //填加xls文件创建程序信息
                //    si.LastAuthor = "广西磐瑞科技有限公司"; //填加xls文件最后保存者信息
                //    si.Comments = "yzs"; //填加xls文件作者信息
                //    si.Title = "导出Excel数据信息"; //填加xls文件标题信息
                //    si.Subject = "导出Excel数据信息";//填加文件主题信息
                //    si.CreateDateTime = System.DateTime.Now;
                //    workbook.SummaryInformation = si;
                //}

                #endregion 右击文件 属性信息

                #region 内容列的样式

                XSSFCellStyle CellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                CellStyle.Alignment = HorizontalAlignment.Center;//居中显示
                CellStyle.BorderBottom = BorderStyle.Thin;//边框
                CellStyle.BorderLeft = BorderStyle.Thin;//边框
                CellStyle.BorderRight = BorderStyle.Thin;//边框
                CellStyle.BorderTop = BorderStyle.Thin;//边框
                XSSFFont font = (XSSFFont)workbook.CreateFont();
                font.FontHeightInPoints = 11;
                font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                font.FontName = "宋体";
                CellStyle.SetFont(font);
                #endregion 内容列的样式

                int rowIndex = 0; //当前行索引

                #region 新建表，填充表头，填充列头，样式
                XSSFRow headerRow = (XSSFRow)sheet.CreateRow(rowIndex); // 创建标题行
                headerRow.HeightInPoints = 20;
                var columIndex = 0; // 当前列索引
                foreach (var column in sheetInfo.Columns)
                {
                    var headCell = headerRow.CreateCell(columIndex); // 创建标题单元格
                    headCell.SetCellValue(column.ExcelColumn);  //获取或设置列的标题。
                    headerRow.GetCell(columIndex).CellStyle = CellStyle;

                //设置列宽
                sheet.SetColumnWidth(columIndex, column.Width * 256);
                //sheet.AutoSizeColumn((short)columIndex);
                    sheet.SetColumnHidden(columIndex, column.Hidden);
                    columIndex++;
                }

                #endregion 新建表，填充表头，填充列头，样式

                #region 填充内容

                #region 内容样式
                XSSFCellStyle contentStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                contentStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                contentStyle.BorderBottom = BorderStyle.Thin;//边框
                contentStyle.BorderLeft = BorderStyle.Thin;//边框
                contentStyle.BorderRight = BorderStyle.Thin;//边框
                contentStyle.BorderTop = BorderStyle.Thin;//边框
                contentStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
           
                XSSFFont font1 = (XSSFFont)workbook.CreateFont();
                font.FontHeightInPoints = 11;
                font.FontName = "宋体";
                contentStyle.SetFont(font);
                #endregion 内容样式

                rowIndex = 1; // 从标题行下面开始填充数据

                foreach (DataRow row in sheetInfo.DataSource.Rows)
                {
                    XSSFRow dataRow = (XSSFRow)sheet.CreateRow(rowIndex); // 创建行,从第一行开始创建

                    var columnIndex = 0;

                    foreach (var column in sheetInfo.Columns)
                    {
                        XSSFCell newCell = (XSSFCell)dataRow.CreateCell(columnIndex);
                      
                        if (!sheetInfo.DataSource.Columns.Contains(column.Column))
                        {
                            newCell.SetCellValue("");
                      
                        }
                        else
                        {
                            string drValue = row[column.Column].ToString();

                            switch (column.DataType.ToUpper())
                            {
                                case "S"://字符串类型
                                    newCell.SetCellValue(drValue);    
                                break;
                                case "D"://日期类型
                                    System.DateTime dateV;
                                    System.DateTime.TryParse(drValue, out dateV);
                                    newCell.SetCellValue(dateV);
                                break;
                               case "B"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(drValue, out boolV);
                                    break;
                                case "I"://整型
                                    int intV = 0;
                                    int.TryParse(drValue, out intV);
                                    newCell.SetCellValue(intV);
                                    break;

                                case "F"://浮点型
                                    double doubV = 0;
                                    double.TryParse(drValue, out doubV);
                                    newCell.SetCellValue(doubV);
                                    break;
                                default:
                                    newCell.SetCellValue(drValue);
                                    break;
                            }
                            newCell.CellStyle = contentStyle;
                    }
                        columnIndex++;
                    }
                    rowIndex++;
                }
                #endregion 填充内容

        }

        public bool isExcel2007 = false;//生成excel2007

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sheetname">sheet名字</param>
        /// <param name="strColumns">导出的列名对象,数据格式是“[{"header":" 进货单号","field":"SHDH"},{"header":" 当前状态","field":"STATENAME"}]”</param>
        public byte[] ToExcelByDataTable2007(DataTable dt, Dictionary<string,string> strColumns)
        {
            if (dt == null || dt.Rows.Count <= 0 || strColumns.Count<=0)
            {
                return null;
            }
            NPOIMemoryStream ms = new NPOIMemoryStream();
            byte[] bt = null;
            isExcel2007 = true;
            Sheet sheet = new Sheet();
            sheet.Name = "sheet1";
            List<ColumnModel> columns = new List<ColumnModel>();
            foreach (var column1 in strColumns)
            {
                ColumnModel column = new ColumnModel();
                column.ExcelColumn = column1.Key.ToString();
                column.Column = column1.Value.ToString();
                column.DataType = "S";
                column.Width = 15;
                column.Hidden = false;
                columns.Add(column);
            }
            sheet.Columns = columns;
            sheet.DataSource = dt;
            this.RenderToExcelNew(sheet);
            ms.IsColse = false;
            workbook.Write(ms);
            ms.Position = 0;
            ms.Flush();
            ms.IsColse = true;
            bt = ms.ToArray();
            return bt;
        }


        /// <summary>
        /// 打印Excel(这些只是一些使用方法,并无功能效果)
        /// </summary>
        /// <param name="LocalFilePath_zhu"></param>
        /// <returns></returns>
        public ActionResult Print(DataTable data)
        {
            string FileName = Guid.NewGuid().ToString("N");//重命名
            AjaxMsgModel result = null;
            string BaseUrl = Directory.GetCurrentDirectory();
            string SaveFilePath = BaseUrl + "/Excel/" + FileName + ".xlsx";//存放路径
            string LocalFilePath_zhu = BaseUrl + "/Excel/用户列表.xlsx";

            #region 填充数据
            using (FileStream file = new FileStream(LocalFilePath_zhu, FileMode.Open, FileAccess.Read))
            {
                XSSFWorkbook xssfworkbook = new XSSFWorkbook(file);
                XSSFSheet sheet = (XSSFSheet)xssfworkbook.GetSheetAt(1);//读取当前表数据


                //sheet.GetRow(0).GetCell(32).SetCellValue(data.Rows[0]["FILEH"].ToString());//文件编号
                sheet.GetRow(2).GetCell(0).SetCellValue("");//适用车型
                sheet.GetRow(1).GetCell(28).SetCellValue(data.Rows[0]["SCCJ"].ToString());//生产车间
                sheet.GetRow(2).GetCell(17).SetCellValue(data.Rows[0]["HXTEAM"].ToString());//核心小组




                #region 合并列
                int BeganRowIndex = 0; // 开始行索引
                int LastRowIndex = 0; // 结束行索引
                int BeganColIndex = 0; // 开始列索引
                int LastColIndex = 0; // 结束列索引
                var BJ = new NPOI.SS.Util.CellRangeAddress(BeganRowIndex, LastRowIndex, BeganColIndex, LastColIndex); //合并列


                sheet.AddMergedRegion(BJ);  // 合并单元格

               
                NPOI.SS.Util.RegionUtil.SetBorderBottom(1, BJ, sheet);  // 设置合并单元格的边框

                NPOI.SS.Util.RegionUtil.SetBorderBottom(1, BJ, sheet); //设置合并单元格的边框
                #endregion

                #region 设置内容
                sheet.GetRow(BeganRowIndex).GetCell(0).SetCellFormula(""); //设置小数或者是整数的内容
                sheet.GetRow(BeganRowIndex).GetCell(0).SetCellType(CellType.Formula); // 设置内容的类型
                sheet.GetRow(BeganRowIndex).GetCell(0).SetCellValue(""); // 设置为字符型的内容
                #endregion

                #region 单元格样式

                XSSFCellStyle cellStyle = xssfworkbook.CreateCellStyle() as XSSFCellStyle; //转换
                XSSFFont font1 = xssfworkbook.CreateFont() as XSSFFont;

                font1.FontName = "宋体"; // 设置字体
                font1.FontHeightInPoints = 11; // 设置字体大小
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.Alignment = HorizontalAlignment.Center;
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
                cellStyle.WrapText = true;//设置自动换行

                cellStyle.SetFont(font1);

                XSSFRow xssfRow = sheet.GetRow(BeganRowIndex) as XSSFRow;

                xssfRow.GetCell(0).CellStyle = cellStyle; // 设置单元格样式

                xssfRow.HeightInPoints = (float)39.9;//设置行号
                #endregion 单元格样式
                FileStream fs = System.IO.File.OpenWrite(SaveFilePath);//创建打印表格
                xssfworkbook.Write(fs);//打开写入数据
                fs.Close();//关闭释放资源
            }

           
            result = new AjaxMsgModel
            {
                BackUrl = "/Upload/Temporary/" + FileName + ".xlsx",//文件地址
                Data = "关键工序清单打印文件_" + FileName + ".xlsx",//文件名称
                Msg = "打印成功！",
                Statu = AjaxStatu.ok
            };
            #endregion 填充数据
            return null;
        }

    }
}

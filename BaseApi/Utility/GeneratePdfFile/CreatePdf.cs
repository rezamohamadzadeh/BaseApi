﻿using PdfRpt.Core.Contracts;
using PdfRpt.FluentInterface;
using System;
using System.Collections.Generic;
using System.IO;
namespace BaseApi.Utility.GeneratePdfFile
{
    public class CreatePdf<TEntity> where TEntity : class
    {
       /// <summary>
       /// This Method generetate pdf file
       /// </summary>
       /// <param name="wwwroot">for get pdf header image and fonts</param>
       /// <param name="list">set list in method and return pdf file</param>
       /// <returns></returns>
        public static PdfReport createReport(string wwwroot,List<TEntity> list,bool hasSum ,string Title, params string[] colName)
        {
            return new PdfReport().DocumentPreferences(doc =>
            {
                doc.RunDirection(PdfRunDirection.LeftToRight);
                doc.Orientation(PageOrientation.Portrait);
                doc.PageSize(PdfPageSize.A4);
                doc.DocumentMetadata(new DocumentMetadata { Author = "Reza", Application = "PdfRpt", Keywords = "IList Rpt.", Subject = "Test Rpt", Title = "Test" });
                doc.Compression(new CompressionSettings
                {
                    EnableCompression = true,
                    EnableFullCompression = true
                });
            })
                .DefaultFonts(fonts =>
                {
                    fonts.Path(Path.Combine(wwwroot, "fonts", "verdana.ttf"),
                        Path.Combine(wwwroot, "fonts", "tahoma.ttf"));
                    fonts.Size(10);
                    fonts.Color(System.Drawing.Color.Black);
                })
                .PagesFooter(footer =>
                {
                    footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy"));
                })
                .PagesHeader(header =>
                {
                    
                    header.CacheHeader(cache: true); // It's a default setting to improve the performance.
                    header.DefaultHeader(defaultHeader =>
                    {
                        defaultHeader.RunDirection(PdfRunDirection.LeftToRight);
                        defaultHeader.ImagePath(Path.Combine(wwwroot, "images", "AffiliateSellReportHeader.Png"));
                        defaultHeader.Message(Title);
                        
                    });
                })
                .MainTableTemplate(template =>
                {
                    template.BasicTemplate(BasicTemplate.SilverTemplate);
                })
                .MainTablePreferences(table =>
                {
                    table.ColumnsWidthsType(TableColumnWidthType.Relative);
                })
                .MainTableDataSource(dataSource =>
                {
                    //var listOfRows = new List<AffiliateReportDto>();
                    //for (int i = 0; i < 40; i++)
                    //{
                    //    listOfRows.Add(new AffiliateReportDto {  AffiliateEmail = i.ToString(),  AffiliateCode = "LastName " + i,  RegisteredCount = i,  SumSell = i + 1000 });
                    //}
                    dataSource.StronglyTypedList(list);
                })
                .MainTableSummarySettings(summarySettings =>
                {
                    if(hasSum)
                    {
                        summarySettings.OverallSummarySettings("Summary");
                        summarySettings.PreviousPageSummarySettings("Previous Page Summary");
                        summarySettings.PageSummarySettings("Summary Page");
                    }
                    
                })
                .MainTableColumns(columns =>
                {
                    //string[] values = { "AffiliateCode","AffiliateEmail", "RegisteredCount"};
                    columns.AddColumn(column =>
                    {
                        column.PropertyName("rowNo");
                        column.IsRowNumber(true);
                        column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                        column.IsVisible(true);
                        column.Order(0);
                        column.Width(1);
                        column.HeaderCell("#");
                    });
                    
                    int count = 1;
                    foreach (var item in colName)
                    {
                        columns.AddColumn(column =>
                        {
                            column.PropertyName(item);
                            column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                            column.IsVisible(true);
                            column.Order(1);
                            column.Width(2);
                            column.HeaderCell(item);
                        });
                        count++;
                    }
                    if (hasSum)
                    {
                        columns.AddColumn(column =>
                        {
                            column.PropertyName("SumSell");
                            column.CellsHorizontalAlignment(HorizontalAlignment.Center);
                            column.IsVisible(true);
                            column.Order(4);
                            column.Width(2);
                            column.HeaderCell("SumSell");
                            column.ColumnItemsTemplate(template =>
                            {
                                template.TextBlock();
                                template.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                    ? string.Empty : string.Format("{0:n0}", obj));
                            });
                            column.AggregateFunction(aggregateFunction =>
                            {
                                aggregateFunction.NumericAggregateFunction(AggregateFunction.Sum);
                                aggregateFunction.DisplayFormatFormula(obj => obj == null || string.IsNullOrEmpty(obj.ToString())
                                    ? string.Empty : string.Format("{0:n0}", obj));
                            });
                        });
                    }
                    

                })
                .MainTableEvents(events =>
                {
                    events.DataSourceIsEmpty(message: "There is no data available to display.");
                })
                .Export(export =>
                {
                    export.ToExcel();
                });
        }
    }
}

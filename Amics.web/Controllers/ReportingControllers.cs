using DevExpress.Compatibility.System.Web;
using DevExpress.DataAccess.Sql;
using System.Collections.Generic;
using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer.Native.Services;
using DevExpress.AspNetCore.Reporting.ReportDesigner.Native.Services;
using DevExpress.AspNetCore.Reporting.QueryBuilder.Native.Services;
using DevExpress.XtraReports.Web.ReportDesigner;
using Microsoft.AspNetCore.Mvc;

namespace Amics.web.Controllers
{
    public class CustomWebDocumentViewerController : WebDocumentViewerController
    {
        public CustomWebDocumentViewerController(IWebDocumentViewerMvcControllerService controllerService) : base(controllerService)
        {
        }
    }


    public class CustomReportDesignerController : ReportDesignerController
    {
        public CustomReportDesignerController(IReportDesignerMvcControllerService controllerService) : base(controllerService)
        {
        }

        [HttpPost("[action]")]
        public IActionResult GetDesignerModel([FromForm] string reportUrl, [FromServices] IReportDesignerClientSideModelGenerator modelGenerator)
        {
            var dataSources = new Dictionary<string, object>();
            var ds = new SqlDataSource("localhost_amicsperaton_Connection");

            // Create a SQL query to access the list_items data table.
            SelectQuery query = SelectQueryFluentBuilder.AddTable("list_items").SelectAllColumnsFromTable().Build("list_items");
            ds.Queries.Add(query);
            ds.RebuildResultSchema();
            dataSources.Add("list_items", ds);

            SelectQuery query2 = SelectQueryFluentBuilder.AddTable("translog").SelectAllColumnsFromTable().Build("list_items");
            ds.Queries.Add(query2);
            ds.RebuildResultSchema();
            dataSources.Add("translog", ds);


            var model = modelGenerator.GetModel(reportUrl, dataSources, ReportDesignerController.DefaultUri, WebDocumentViewerController.DefaultUri, QueryBuilderController.DefaultUri);
            return DesignerModel(model);
        }
    }


    public class CustomQueryBuilderController : QueryBuilderController
    {
        public CustomQueryBuilderController(IQueryBuilderMvcControllerService controllerService) : base(controllerService)
        {
        }
    }
}

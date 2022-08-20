using DevExpress.XtraReports.Services;
using DevExpress.XtraReports.UI;
using Amics.web.PredefinedReports;
using System;
using System.Web;
namespace Amics.web.Services
{
    public class CustomReportProvider : IReportProvider
    {
        public XtraReport GetReport(string id, ReportProviderContext context)
        {
            // Parse the string with the report name and parameter values.
            string[] parts = id.Split('?');
            string reportName = parts[0];
            string parametersQueryString = parts.Length > 1 ? parts[1] : String.Empty;

            // Create a report instance.
            XtraReport report = null;

            if (reportName == "ListItems")
            {
                report = new ListItems();
            }
            else if (reportName == "ListItemsRpt")
            {
                report = new ListItemsRpt();
            }
            else if (reportName == "ListItemsCS")
            {
                report = new ListItemsCS();
            }
            
            else
            {
                throw new DevExpress.XtraReports.Web.ClientControls.FaultException(
                    string.Format("Could not find report '{0}'.", reportName)
                );
            }

            // Apply the parameter values to the report.
            var parameters = HttpUtility.ParseQueryString(parametersQueryString);

            foreach (string parameterName in parameters.AllKeys)
            {
                report.Parameters[parameterName].Value = Convert.ChangeType(
                    parameters.Get(parameterName), report.Parameters[parameterName].Type);
            }

            // Disable the Visible property for all report parameters
            // to hide the Parameters Panel in the viewer.
            foreach (var parameter in report.Parameters)
            {
                parameter.Visible = false;
            }

            // If you do not hide the panel, disable the report's RequestParameters property.
            // report.RequestParameters = false;

            return report;
        }
    }
}

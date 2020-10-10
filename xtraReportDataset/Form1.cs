using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xtraReportDataset.model;

namespace xtraReportDataset
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnShowReport_Click(object sender, EventArgs e)
        {
            rpt.XtraReport1 report = new rpt.XtraReport1();
            report.RequestParameters = false;
            report.DataSource = await FillDataset(dateTimePicker1.Value, dateTimePicker2.Value);

            report.Parameters["HireDateRange"].Value = "From " + dateTimePicker1.Value.ToShortDateString() + " to " + dateTimePicker2.Value.ToShortDateString();

            report.ExportToPdf(@"C:\sampleData\employee.pdf");

            MessageBox.Show("File successfully created");
        }

        private async Task<DataSet> FillDataset(DateTime from, DateTime to)
        {
            using (AdventureWorksDW2017Entities entities = new AdventureWorksDW2017Entities())
            {
                var data = await entities.DimEmployee.Where(x => x.HireDate >= from && x.HireDate <= to).ToListAsync();

                DataSet dataSet1 = new DataSet();
                dataSet1.DataSetName = "EmployeeDataset";

                DataTable dataTable1 = new DataTable();

                dataSet1.Tables.Add(dataTable1);

                dataTable1.TableName = "Employee";
                dataTable1.Columns.Add("FirstName", typeof(string));
                dataTable1.Columns.Add("LastName", typeof(string));
                dataTable1.Columns.Add("HireDate", typeof(DateTime));

                foreach (var item in data)
                {
                    dataSet1.Tables["Employee"].Rows.Add(new Object[] { item.FirstName, item.LastName, item.HireDate });
                }

                return dataSet1;
            }
        }
    }
}

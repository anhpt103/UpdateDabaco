using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.DataProcessing.InMemoryDataProcessor;

namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public partial class UC_FRAME_DONGBO : UserControl
    {
        public UC_FRAME_DONGBO()
        {
            InitializeComponent();
            dateTimeDenNgay.Format = DateTimePickerFormat.Custom;
            dateTimeDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeDenNgay.Value = DateTime.Now;
            dateTimeTuNgay.Format = DateTimePickerFormat.Custom;
            dateTimeTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeTuNgay.Value = DateTime.Now.AddDays(-7);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            GetDataFromSql();}

        private void btnDongBo_Click(object sender, EventArgs e)
        {

        }

        private void GetDataFromSql()
        {
            DateTime fromDate = dateTimeTuNgay.Value;
            DateTime toDate = dateTimeDenNgay.Value;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = @"SELECT * FROM NVGDQUAY_ASYNCCLIENT WHERE (NGAYTAO BETWEEN @fromDate  AND toDate)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("fromDate", SqlDbType.DateTime).Value = fromDate;
                    cmd.Parameters.Add("toDate", SqlDbType.DateTime).Value = toDate;
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        while (dataReader.Read())
                        {
                            
                        }
                    }
                }
            }
        }

        private void btnFilterXml_Click(object sender, EventArgs e)
        {

        }
    }
}

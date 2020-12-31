using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using BTS.SP.BANLE.Common;
using BTS.SP.BANLE.Dto;
using Oracle.ManagedDataAccess.Client;

namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public partial class frmDongBo : Form
    {
        private List<NVGDQUAY_ASYNCCLIENT_DTO> lstData = new List<NVGDQUAY_ASYNCCLIENT_DTO>();
        private List<NVGDQUAY_ASYNCCLIENT_DTO> lstDataOracle = new List<NVGDQUAY_ASYNCCLIENT_DTO>();
        public frmDongBo()
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
            lstData = GetDataFromSql();
            if (Config.CheckConnectToServer()) // nếu có mạng lan
            {
                lstDataOracle = GetDataFromOracle();
            }
        }
        private void btnDongBo_Click(object sender, EventArgs e)
        {
            int hdSV = 0;
            int mhSV = 0;
            int hdMT = 0;
            int mhMT = 0;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                if (Config.CheckConnectToServer()) // nếu có mạng lan
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        foreach (NVGDQUAY_ASYNCCLIENT_DTO item in lstData)
                        {
                            try
                            {
                                OracleCommand orlCommand = new OracleCommand();
                                orlCommand.Connection = connection;
                                orlCommand.CommandText = @"SELECT * FROM NVGDQUAY_ASYNCCLIENT WHERE MAGIAODICHQUAYPK = '" + item.MAGIAODICHQUAYPK + "'";
                                orlCommand.CommandType = CommandType.Text;
                                try
                                {
                                    OracleDataReader reader = orlCommand.ExecuteReader();
                                    if (!reader.HasRows)
                                    {
                                        try
                                        {
                                            string queryInsert = "";
                                            OracleCommand cmd = new OracleCommand();
                                            cmd.Connection = connection;
                                            queryInsert =
                                                string.Format(
                                                    @"INSERT INTO NVGDQUAY_ASYNCCLIENT (ID,MAGIAODICH,MAGIAODICHQUAYPK,MADONVI,LOAIGIAODICH,NGAYTAO,MANGUOITAO,
                                    NGUOITAO,MAQUAYBAN,NGAYPHATSINH,HINHTHUCTHANHTOAN,MAVOUCHER,TIENKHACHDUA,TIENVOUCHER,TIENTHEVIP,TIENTRALAI,TIENTHE,TIENCOD,TIENMAT,TTIENCOVAT,
                                    THOIGIAN,MAKHACHHANG,I_CREATE_DATE,I_CREATE_BY,I_STATE,UNITCODE) VALUES (
                                    :ID,:MAGIAODICH,:MAGIAODICHQUAYPK,:MADONVI,:LOAIGIAODICH,:NGAYTAO,:MANGUOITAO,
                                    :NGUOITAO,:MAQUAYBAN,:NGAYPHATSINH,:HINHTHUCTHANHTOAN,:MAVOUCHER,:TIENKHACHDUA,:TIENVOUCHER,:TIENTHEVIP,:TIENTRALAI,:TIENTHE,:TIENCOD,:TIENMAT,:TTIENCOVAT,
                                    :THOIGIAN,:MAKHACHHANG,:I_CREATE_DATE,:I_CREATE_BY,:I_STATE,:UNITCODE)");
                                            cmd.CommandText = queryInsert;
                                            cmd.CommandType = CommandType.Text;
                                            cmd.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = item.ID;
                                            cmd.Parameters.Add("MAGIAODICH", OracleDbType.NVarchar2, 50).Value = item.MAGIAODICH;
                                            cmd.Parameters.Add("MAGIAODICHQUAYPK", OracleDbType.NVarchar2, 50).Value = item.MAGIAODICHQUAYPK;
                                            cmd.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value = item.UNITCODE;
                                            cmd.Parameters.Add("LOAIGIAODICH", OracleDbType.Int32).Value = item.LOAIGIAODICH;
                                            cmd.Parameters.Add("NGAYTAO", OracleDbType.Date).Value = item.NGAYTAO;
                                            cmd.Parameters.Add("MANGUOITAO", OracleDbType.NVarchar2, 300).Value = item.MANGUOITAO;
                                            cmd.Parameters.Add("NGUOITAO", OracleDbType.NVarchar2, 300).Value = item.NGUOITAO;
                                            cmd.Parameters.Add("MAQUAYBAN", OracleDbType.NVarchar2, 300).Value = item.MAQUAYBAN;
                                            cmd.Parameters.Add("NGAYPHATSINH", OracleDbType.Date).Value = item.NGAYPHATSINH;
                                            cmd.Parameters.Add("HINHTHUCTHANHTOAN", OracleDbType.NVarchar2, 200).Value = item.HINHTHUCTHANHTOAN;
                                            cmd.Parameters.Add("MAVOUCHER", OracleDbType.NVarchar2, 50).Value = item.MAVOUCHER;
                                            cmd.Parameters.Add("TIENKHACHDUA", OracleDbType.Decimal).Value = item.TIENKHACHDUA;
                                            cmd.Parameters.Add("TIENVOUCHER", OracleDbType.Decimal).Value = item.TIENVOUCHER;
                                            cmd.Parameters.Add("TIENTHEVIP", OracleDbType.Decimal).Value = item.TIENTHEVIP;
                                            cmd.Parameters.Add("TIENTRALAI", OracleDbType.Decimal).Value = item.TIENTRALAI;
                                            cmd.Parameters.Add("TIENTHE ", OracleDbType.Decimal).Value = item.TIENTHE;
                                            cmd.Parameters.Add("TIENCOD", OracleDbType.Decimal).Value = item.TIENCOD;
                                            cmd.Parameters.Add("TIENMAT", OracleDbType.Decimal).Value = item.TIENMAT;
                                            cmd.Parameters.Add("TTIENCOVAT", OracleDbType.Decimal).Value = item.TTIENCOVAT;
                                            cmd.Parameters.Add("THOIGIAN", OracleDbType.NVarchar2, 150).Value = item.THOIGIAN;
                                            cmd.Parameters.Add("MAKHACHHANG", OracleDbType.NVarchar2, 50).Value = item.MAKHACHHANG;
                                            cmd.Parameters.Add("I_CREATE_DATE", OracleDbType.Date).Value = item.NGAYTAO;
                                            cmd.Parameters.Add("I_CREATE_BY", OracleDbType.NVarchar2, 50).Value = item.MANGUOITAO;
                                            cmd.Parameters.Add("I_STATE", OracleDbType.NVarchar2, 50).Value = "40";
                                            cmd.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = item.UNITCODE;
                                            if (cmd.ExecuteNonQuery() > 0)
                                            {
                                                hdSV++;
                                                int tempId = 0;
                                                foreach (NVHANGGDQUAY_ASYNCCLIENT itemData in item.LST_DETAILS)
                                                {
                                                    tempId++;
                                                    OracleCommand command2 = new OracleCommand();
                                                    command2.Connection = connection;
                                                    string queryInsertItem =
                                                        string.Format(
                                                            @"INSERT INTO NVHANGGDQUAY_ASYNCCLIENT (ID,MAGDQUAYPK,MAKHOHANG,MADONVI,MAVATTU,BARCODE,TENDAYDU,NGUOITAO,MABOPK,NGAYTAO,NGAYPHATSINH,
                                                SOLUONG,TTIENCOVAT,VATBAN,GIABANLECOVAT,MAKHACHHANG,MAKEHANG,MACHUONGTRINHKM,LOAIKHUYENMAI,TIENCHIETKHAU,TYLECHIETKHAU,TYLEKHUYENMAI,TIENKHUYENMAI,TYLEVOUCHER,
                                                TIENVOUCHER,TYLELAILE,GIAVON,ISBANAM,MAVAT) VALUES (:ID,:MAGDQUAYPK,:MAKHOHANG,:MADONVI,:MAVATTU,:BARCODE,:TENDAYDU,:NGUOITAO,:MABOPK,:NGAYTAO,:NGAYPHATSINH,
                                                :SOLUONG,:TTIENCOVAT,:VATBAN,:GIABANLECOVAT,:MAKHACHHANG,:MAKEHANG,:MACHUONGTRINHKM,:LOAIKHUYENMAI,:TIENCHIETKHAU,:TYLECHIETKHAU,:TYLEKHUYENMAI,:TIENKHUYENMAI,:TYLEVOUCHER,
                                                :TIENVOUCHER,:TYLELAILE,:GIAVON,:ISBANAM,:MAVAT)");
                                                    command2.CommandText = queryInsertItem;
                                                    command2.CommandType = CommandType.Text;
                                                    command2.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = Guid.NewGuid().ToString();
                                                    command2.Parameters.Add("MAGDQUAYPK", OracleDbType.NVarchar2, 50).Value = itemData.MAGDQUAYPK;
                                                    command2.Parameters.Add("MAKHOHANG", OracleDbType.NVarchar2, 50).Value = itemData.MAKHOHANG;
                                                    command2.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value = itemData.MADONVI;
                                                    command2.Parameters.Add("MAVATTU", OracleDbType.NVarchar2, 50).Value = itemData.MAVATTU;
                                                    command2.Parameters.Add("BARCODE", OracleDbType.NVarchar2, 2000).Value = itemData.BARCODE;
                                                    command2.Parameters.Add("TENDAYDU", OracleDbType.NVarchar2, 300).Value = itemData.TENDAYDU;
                                                    command2.Parameters.Add("NGUOITAO", OracleDbType.NVarchar2, 300).Value = item.MANGUOITAO;
                                                    command2.Parameters.Add("MABOPK", OracleDbType.NVarchar2, 50).Value = itemData.MABOPK;
                                                    command2.Parameters.Add("NGAYTAO", OracleDbType.Date).Value = itemData.NGAYTAO;
                                                    command2.Parameters.Add("NGAYPHATSINH", OracleDbType.Date).Value = itemData.NGAYPHATSINH;
                                                    command2.Parameters.Add("SOLUONG", OracleDbType.Int32).Value =
                                                        itemData.SOLUONG;
                                                    command2.Parameters.Add("TTIENCOVAT", OracleDbType.Decimal).Value =
                                                        itemData.TTIENCOVAT;
                                                    command2.Parameters.Add("VATBAN", OracleDbType.Decimal).Value =
                                                        itemData.VATBAN;
                                                    command2.Parameters.Add("GIABANLECOVAT", OracleDbType.Decimal).Value =
                                                        itemData.GIABANLECOVAT;
                                                    command2.Parameters.Add("MAKHACHHANG", OracleDbType.NVarchar2, 50).Value =
                                                        itemData.MAKHACHHANG;
                                                    command2.Parameters.Add("MAKEHANG", OracleDbType.NVarchar2, 50).Value =
                                                        itemData.MAKEHANG;
                                                    command2.Parameters.Add("MACHUONGTRINHKM", OracleDbType.NVarchar2, 50).Value
                                                        = itemData.MACHUONGTRINHKM;
                                                    command2.Parameters.Add("LOAIKHUYENMAI", OracleDbType.NVarchar2, 50).Value =
                                                        itemData.LOAIKHUYENMAI;
                                                    command2.Parameters.Add("TIENCHIETKHAU", OracleDbType.Decimal).Value =
                                                        itemData.TIENCHIETKHAU;
                                                    command2.Parameters.Add("TYLECHIETKHAU", OracleDbType.Decimal).Value =
                                                        itemData.TYLECHIETKHAU;
                                                    command2.Parameters.Add("TYLEKHUYENMAI", OracleDbType.Decimal).Value =
                                                        itemData.TYLEKHUYENMAI;
                                                    command2.Parameters.Add("TIENKHUYENMAI", OracleDbType.Decimal).Value =
                                                        itemData.TIENKHUYENMAI;
                                                    command2.Parameters.Add("TYLEVOUCHER", OracleDbType.Decimal).Value =
                                                        itemData.TYLEVOUCHER;
                                                    command2.Parameters.Add("TIENVOUCHER", OracleDbType.Decimal).Value =
                                                        itemData.TIENVOUCHER;
                                                    command2.Parameters.Add("TYLELAILE", OracleDbType.Decimal).Value =
                                                        itemData.TYLELAILE;
                                                    command2.Parameters.Add("GIAVON", OracleDbType.Decimal).Value =
                                                        itemData.GIAVON;
                                                    command2.Parameters.Add("ISBANAM", OracleDbType.Decimal).Value = 0;
                                                    command2.Parameters.Add("MAVAT", OracleDbType.NVarchar2, 50).Value =
                                                        itemData.MAVAT;
                                                    try
                                                    {
                                                        command2.ExecuteNonQuery();
                                                        mhSV++;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        WriteLogs.LogError(ex);
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            WriteLogs.LogError(ex);
                                        }
                                    }
                                }
                                catch (Exception x) { }
                            }
                            catch (Exception ex)
                            {
                                WriteLogs.LogError(ex);
                            }
                        }
                    }
                }
            }
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    foreach (NVGDQUAY_ASYNCCLIENT_DTO item in lstDataOracle)
                    {
                        try
                        {
                            SqlCommand sqlCommand = new SqlCommand();
                            sqlCommand.Connection = connection;
                            sqlCommand.CommandText = @"SELECT * FROM NVGDQUAY_ASYNCCLIENT WHERE MAGIAODICHQUAYPK = '" + item.MAGIAODICHQUAYPK + "'";
                            sqlCommand.CommandType = CommandType.Text;
                            try
                            {
                                SqlDataReader reader = sqlCommand.ExecuteReader();
                                if (!reader.HasRows)
                                {
                                    try
                                    {
                                        string queryInsert = "";
                                        SqlCommand cmd = new SqlCommand();
                                        cmd.Connection = connection;
                                        queryInsert =
                                            string.Format(
                                                @"INSERT INTO NVGDQUAY_ASYNCCLIENT (ID,MAGIAODICH,MAGIAODICHQUAYPK,MADONVI,LOAIGIAODICH,NGAYTAO,MANGUOITAO,
                                    NGUOITAO,MAQUAYBAN,NGAYPHATSINH,HINHTHUCTHANHTOAN,TIENKHACHDUA,TIENVOUCHER,TIENTHEVIP,TIENTRALAI,TIENTHE,TIENCOD,TIENMAT,TTIENCOVAT,
                                    THOIGIAN,MAKHACHHANG,UNITCODE) VALUES (
                                    @ID,@MAGIAODICH,@MAGIAODICHQUAYPK,@MADONVI,@LOAIGIAODICH,@NGAYTAO,@MANGUOITAO,
                                    @NGUOITAO,@MAQUAYBAN,@NGAYPHATSINH,@HINHTHUCTHANHTOAN,@TIENKHACHDUA,@TIENVOUCHER,@TIENTHEVIP,@TIENTRALAI,@TIENTHE,@TIENCOD,@TIENMAT,@TTIENCOVAT,
                                    @THOIGIAN,@MAKHACHHANG,@UNITCODE)");
                                        cmd.CommandText = queryInsert;
                                        cmd.CommandType = CommandType.Text;
                                        cmd.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = item.ID;
                                        cmd.Parameters.Add("MAGIAODICH", SqlDbType.VarChar, 50).Value = item.MAGIAODICH;
                                        cmd.Parameters.Add("MAGIAODICHQUAYPK", SqlDbType.VarChar, 50).Value = item.MAGIAODICHQUAYPK;
                                        cmd.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value = item.UNITCODE;
                                        cmd.Parameters.Add("LOAIGIAODICH", SqlDbType.Int).Value = item.LOAIGIAODICH;
                                        cmd.Parameters.Add("NGAYTAO", SqlDbType.Date).Value = item.NGAYTAO;
                                        cmd.Parameters.Add("MANGUOITAO", SqlDbType.NVarChar, 300).Value = item.MANGUOITAO;
                                        cmd.Parameters.Add("NGUOITAO", SqlDbType.NVarChar, 300).Value = item.NGUOITAO;
                                        cmd.Parameters.Add("MAQUAYBAN", SqlDbType.NVarChar, 300).Value = item.MAQUAYBAN;
                                        cmd.Parameters.Add("NGAYPHATSINH", SqlDbType.Date).Value = item.NGAYPHATSINH;
                                        cmd.Parameters.Add("HINHTHUCTHANHTOAN", SqlDbType.NVarChar, 200).Value = item.HINHTHUCTHANHTOAN;
                                        cmd.Parameters.Add("TIENKHACHDUA", SqlDbType.Decimal).Value = item.TIENKHACHDUA;
                                        cmd.Parameters.Add("TIENVOUCHER", SqlDbType.Decimal).Value = item.TIENVOUCHER;
                                        cmd.Parameters.Add("TIENTHEVIP", SqlDbType.Decimal).Value = item.TIENTHEVIP;
                                        cmd.Parameters.Add("TIENTRALAI", SqlDbType.Decimal).Value = item.TIENTRALAI;
                                        cmd.Parameters.Add("TIENTHE ", SqlDbType.Decimal).Value = item.TIENTHE;
                                        cmd.Parameters.Add("TIENCOD", SqlDbType.Decimal).Value = item.TIENCOD;
                                        cmd.Parameters.Add("TIENMAT", SqlDbType.Decimal).Value = item.TIENMAT;
                                        cmd.Parameters.Add("TTIENCOVAT", SqlDbType.Decimal).Value = item.TTIENCOVAT;
                                        cmd.Parameters.Add("THOIGIAN", SqlDbType.NVarChar, 150).Value = item.THOIGIAN;
                                        cmd.Parameters.Add("MAKHACHHANG", SqlDbType.NVarChar, 50).Value = item.MAKHACHHANG;
                                        cmd.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = item.UNITCODE;
                                        if (cmd.ExecuteNonQuery() > 0)
                                        {
                                            hdMT++;
                                            int tempId = 0;
                                            foreach (NVHANGGDQUAY_ASYNCCLIENT itemData in item.LST_DETAILS)
                                            {
                                                tempId++;
                                                SqlCommand command2 = new SqlCommand();
                                                command2.Connection = connection;
                                                string queryInsertItem = string.Format(@"INSERT INTO [dbo].[NVHANGGDQUAY_ASYNCCLIENT]([ID],[MAGDQUAYPK],[MAKHOHANG],[MADONVI],[MAVATTU],[MANGUOITAO],[NGUOITAO],[MABOPK],[NGAYTAO],[NGAYPHATSINH],[SOLUONG],[TTIENCOVAT],[GIABANLECOVAT],[TYLECHIETKHAU],[TIENCHIETKHAU],[TYLEKHUYENMAI],[TIENKHUYENMAI],[TYLEVOUCHER],[TIENVOUCHER],[TYLELAILE],[GIAVON],[MAVAT],[VATBAN],[MACHUONGTRINHKM],[UNITCODE]) 
                                    VALUES (@ID,@MAGDQUAYPK,@MAKHOHANG,@MADONVI,@MAVATTU,@MANGUOITAO,@NGUOITAO,@MABOPK,@NGAYTAO,@NGAYPHATSINH,@SOLUONG,@TTIENCOVAT,@GIABANLECOVAT,@TYLECHIETKHAU,@TIENCHIETKHAU,@TYLEKHUYENMAI,@TIENKHUYENMAI,@TYLEVOUCHER,@TIENVOUCHER,@TYLELAILE,@GIAVON,@MAVAT,@VATBAN,@MACHUONGTRINHKM,@UNITCODE)");
                                                command2.CommandText = queryInsertItem;
                                                command2.CommandType = CommandType.Text;
                                                command2.Parameters.Add("ID", SqlDbType.NVarChar, 50).Value = itemData.ID;
                                                command2.Parameters.Add("MAGDQUAYPK", SqlDbType.NVarChar, 50).Value = itemData.MAGDQUAYPK;
                                                command2.Parameters.Add("MAKHOHANG", SqlDbType.NVarChar, 50).Value = itemData.MAKHOHANG;
                                                command2.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value = itemData.MADONVI;
                                                command2.Parameters.Add("MAVATTU", SqlDbType.NVarChar, 50).Value = itemData.MAVATTU;
                                                command2.Parameters.Add("MANGUOITAO", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentMaNhanVien;
                                                command2.Parameters.Add("NGUOITAO", SqlDbType.NVarChar, 50).Value = itemData.NGUOITAO;
                                                command2.Parameters.Add("MABOPK", SqlDbType.NVarChar, 50).Value = itemData.MABOPK;
                                                command2.Parameters.Add("NGAYTAO", SqlDbType.Date).Value = itemData.NGAYTAO;
                                                command2.Parameters.Add("NGAYPHATSINH", SqlDbType.Date).Value = itemData.NGAYPHATSINH;
                                                command2.Parameters.Add("SOLUONG", SqlDbType.Decimal).Value = itemData.SOLUONG;
                                                command2.Parameters.Add("TTIENCOVAT", SqlDbType.Decimal).Value = itemData.TTIENCOVAT;
                                                command2.Parameters.Add("GIABANLECOVAT", SqlDbType.Decimal).Value = itemData.GIABANLECOVAT;
                                                command2.Parameters.Add("TIENCHIETKHAU", SqlDbType.Decimal).Value = itemData.TIENCHIETKHAU;
                                                command2.Parameters.Add("TYLECHIETKHAU", SqlDbType.Decimal).Value = itemData.TYLECHIETKHAU;
                                                command2.Parameters.Add("TYLEKHUYENMAI", SqlDbType.Decimal).Value = itemData.TYLEKHUYENMAI;
                                                command2.Parameters.Add("TIENKHUYENMAI", SqlDbType.Decimal).Value = itemData.TIENKHUYENMAI;
                                                command2.Parameters.Add("TYLEVOUCHER", SqlDbType.Decimal).Value = itemData.TYLEVOUCHER;
                                                command2.Parameters.Add("TIENVOUCHER", SqlDbType.Decimal).Value = itemData.TIENVOUCHER;
                                                command2.Parameters.Add("TYLELAILE", SqlDbType.Decimal).Value = itemData.TYLELAILE;
                                                command2.Parameters.Add("GIAVON", SqlDbType.Decimal).Value = itemData.GIAVON;
                                                command2.Parameters.Add("MAVAT", SqlDbType.NVarChar, 50).Value = itemData.MAVAT;
                                                command2.Parameters.Add("VATBAN", SqlDbType.Decimal).Value = itemData.VATBAN;
                                                command2.Parameters.Add("MACHUONGTRINHKM", SqlDbType.NVarChar, 50).Value = itemData.MACHUONGTRINHKM;
                                                command2.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = itemData.UNITCODE != null? itemData.UNITCODE : ";";
                                                try
                                                {
                                                    command2.ExecuteNonQuery();
                                                    mhMT++;
                                                }
                                                catch (Exception ex)
                                                {
                                                    WriteLogs.LogError(ex);
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteLogs.LogError(ex);
                                    }
                                }
                            }
                            catch (Exception x) { }
                        }
                        catch (Exception ex)
                        {
                            WriteLogs.LogError(ex);
                        }
                    }
                }
            }
            MessageBox.Show("Đã đồng bộ " + hdSV + " hóa đơn trên server với " + mhSV + " mặt hàng \n " + hdMT +" hóa đơn sql với " + mhMT +"mặt hàng");
        }
        private List<NVGDQUAY_ASYNCCLIENT_DTO> GetDataFromSql()
        {
            List<NVGDQUAY_ASYNCCLIENT_DTO> lstGD = new List<NVGDQUAY_ASYNCCLIENT_DTO>();
            DateTime fromDate = dateTimeTuNgay.Value;
            DateTime toDate = dateTimeDenNgay.Value;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    int hd = 0 ;
                    int mh = 0;
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = @"SELECT * FROM NVGDQUAY_ASYNCCLIENT WHERE (NGAYTAO BETWEEN @fromDate  AND @toDate)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("fromDate", SqlDbType.DateTime).Value = fromDate;
                    cmd.Parameters.Add("toDate", SqlDbType.DateTime).Value = toDate;
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        DateTime ngayTao, ngayPhatSinh = DateTime.Now;
                        int loaiGD = 0;
                        decimal tienKhachDua,
                            tienVoucher,
                            tienTheVip,
                            tienTraLai,
                            tienThe,
                            tienCOD,
                            tienMat,
                            ttienCoVAT = 0;
                        while (dataReader.Read())
                        {
                            hd++;
                            NVGDQUAY_ASYNCCLIENT_DTO gdTemp = new NVGDQUAY_ASYNCCLIENT_DTO();
                            gdTemp.ID = dataReader["ID"].ToString();
                            gdTemp.MAGIAODICH = dataReader["MAGIAODICH"].ToString();
                            gdTemp.MAGIAODICHQUAYPK = dataReader["MAGIAODICHQUAYPK"].ToString();
                            gdTemp.MADONVI = dataReader["MADONVI"].ToString();
                            DateTime.TryParse(dataReader["NGAYTAO"].ToString(), out ngayTao);
                            gdTemp.NGAYTAO = ngayTao;
                            DateTime.TryParse(dataReader["NGAYPHATSINH"].ToString(), out ngayPhatSinh);
                            gdTemp.NGAYPHATSINH = ngayPhatSinh;
                            gdTemp.MANGUOITAO = dataReader["MANGUOITAO"].ToString();
                            gdTemp.NGUOITAO = dataReader["NGUOITAO"].ToString();
                            gdTemp.MAQUAYBAN = dataReader["MAQUAYBAN"].ToString();
                            int.TryParse(dataReader["LOAIGIAODICH"].ToString(), out loaiGD);
                            gdTemp.LOAIGIAODICH = loaiGD;
                            gdTemp.HINHTHUCTHANHTOAN = dataReader["HINHTHUCTHANHTOAN"].ToString();
                            decimal.TryParse(dataReader["TIENKHACHDUA"].ToString(), out tienKhachDua);
                            gdTemp.TIENKHACHDUA = tienKhachDua;
                            decimal.TryParse(dataReader["TIENVOUCHER"].ToString(), out tienVoucher);
                            gdTemp.TIENVOUCHER = tienVoucher;
                            decimal.TryParse(dataReader["TIENTHEVIP"].ToString(), out tienTheVip);
                            gdTemp.TIENTHEVIP = tienTheVip;
                            decimal.TryParse(dataReader["TIENTRALAI"].ToString(), out tienTraLai);
                            gdTemp.TIENTRALAI = tienTraLai;
                            decimal.TryParse(dataReader["TIENTHE"].ToString(), out tienThe);
                            gdTemp.TIENTHE = tienThe;
                            decimal.TryParse(dataReader["TIENCOD"].ToString(), out tienCOD);
                            gdTemp.TIENCOD = tienCOD;
                            decimal.TryParse(dataReader["TIENMAT"].ToString(), out tienMat);
                            gdTemp.TIENMAT = tienMat;
                            decimal.TryParse(dataReader["TTIENCOVAT"].ToString(), out ttienCoVAT);
                            gdTemp.TTIENCOVAT = ttienCoVAT;
                            gdTemp.THOIGIAN = dataReader["THOIGIAN"].ToString();
                            gdTemp.MAKHACHHANG = dataReader["MAKHACHHANG"].ToString();
                            gdTemp.UNITCODE = dataReader["UNITCODE"].ToString();
                            SqlCommand command = new SqlCommand();
                            command.Connection = connection;
                            command.CommandText = @"SELECT * FROM NVHANGGDQUAY_ASYNCCLIENT WHERE MAGDQUAYPK = @maGDPK";
                            command.CommandType = CommandType.Text;
                            command.Parameters.Add("maGDPK", SqlDbType.NVarChar, 50).Value = gdTemp.MAGIAODICHQUAYPK;
                            SqlDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                decimal giaBanLeCoVat,
                                    tyLeCK,
                                    tienCK,
                                    tyLeKM,
                                    tienKM,
                                    tyLeVoucher,
                                    tyLeLai,
                                    giaVon, vatBan = 0;
                                decimal soLuong, tongSoLuong = 0;
                                while (reader.Read())
                                {
                                    mh++;
                                    NVHANGGDQUAY_ASYNCCLIENT gd = new NVHANGGDQUAY_ASYNCCLIENT();
                                    gd.ID = reader["ID"].ToString();
                                    gd.MAGDQUAYPK = reader["MAGDQUAYPK"].ToString();
                                    gd.MAKHOHANG = reader["MAKHOHANG"].ToString();
                                    gd.MADONVI = reader["MADONVI"].ToString();
                                    gd.MAVATTU = reader["MAVATTU"].ToString();
                                    gd.NGUOITAO = reader["NGUOITAO"].ToString();
                                    gd.MABOPK = reader["MABOPK"].ToString();
                                    DateTime.TryParse(reader["NGAYTAO"].ToString(), out ngayTao);
                                    gd.NGAYTAO = ngayTao;
                                    DateTime.TryParse(reader["NGAYPHATSINH"].ToString(), out ngayPhatSinh);
                                    gd.NGAYPHATSINH = ngayPhatSinh;
                                    decimal.TryParse(reader["SOLUONG"].ToString(), out soLuong);
                                    gd.SOLUONG = soLuong;
                                    decimal.TryParse(reader["TTIENCOVAT"].ToString(), out ttienCoVAT);
                                    gd.TTIENCOVAT = ttienCoVAT;
                                    decimal.TryParse(reader["GIABANLECOVAT"].ToString(), out giaBanLeCoVat);
                                    gd.GIABANLECOVAT = giaBanLeCoVat;
                                    decimal.TryParse(reader["TYLECHIETKHAU"].ToString(), out tyLeCK);
                                    gd.TYLECHIETKHAU = tyLeCK;
                                    decimal.TryParse(reader["TIENCHIETKHAU"].ToString(), out tienCK);
                                    gd.TIENCHIETKHAU = tienCK;
                                    decimal.TryParse(reader["TYLEKHUYENMAI"].ToString(), out tyLeKM);
                                    gd.TYLEKHUYENMAI = tyLeKM;
                                    decimal.TryParse(reader["TIENKHUYENMAI"].ToString(), out tienKM);
                                    gd.TIENKHUYENMAI = tienKM;
                                    decimal.TryParse(reader["TYLEVOUCHER"].ToString(), out tyLeVoucher);
                                    gd.TYLEVOUCHER = tyLeVoucher;
                                    decimal.TryParse(reader["TIENVOUCHER"].ToString(), out tienVoucher);
                                    gd.TIENVOUCHER = tienVoucher;
                                    decimal.TryParse(reader["TYLELAILE"].ToString(), out tyLeLai);
                                    gd.TYLELAILE = tyLeLai;
                                    decimal.TryParse(reader["GIAVON"].ToString(), out giaVon);
                                    gd.GIAVON = giaVon;
                                    gd.MAVAT = reader["MAVAT"].ToString();
                                    decimal.TryParse(reader["VATBAN"].ToString(), out vatBan);
                                    gd.VATBAN = vatBan;
                                    gd.MACHUONGTRINHKM = reader["MACHUONGTRINHKM"].ToString();
                                    gd.UNITCODE = reader["UNITCODE"].ToString();
                                    tongSoLuong += gd.SOLUONG;
                                    gdTemp.LST_DETAILS.Add(gd);
                                }
                                gdTemp.TONGSOLUONG = tongSoLuong;
                            }
                            lstGD.Add(gdTemp);
                        }
                    }
                    lblHoaDonMT.Text = hd.ToString();
                    lblMatHangMT.Text = mh.ToString();
                }
                connection.Close();
            }
            return lstGD;
        }
        private List<NVGDQUAY_ASYNCCLIENT_DTO> GetDataFromOracle()
        {
            List<NVGDQUAY_ASYNCCLIENT_DTO> lstGD = new List<NVGDQUAY_ASYNCCLIENT_DTO>();
            DateTime fromDate = dateTimeTuNgay.Value;
            DateTime toDate = dateTimeDenNgay.Value;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    int hd = 0;
                    int mh = 0;
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = @"SELECT * FROM NVGDQUAY_ASYNCCLIENT WHERE (NGAYTAO BETWEEN :fromDate  AND :toDate)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("fromDate", OracleDbType.Date).Value = fromDate;
                    cmd.Parameters.Add("toDate", OracleDbType.Date).Value = toDate;
                    OracleDataReader dataReader = cmd.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        DateTime ngayTao, ngayPhatSinh = DateTime.Now;
                        int loaiGD = 0;
                        decimal tienKhachDua,
                            tienVoucher,
                            tienTheVip,
                            tienTraLai,
                            tienThe,
                            tienCOD,
                            tienMat,
                            ttienCoVAT = 0;
                        while (dataReader.Read())
                        {
                            hd++;
                            NVGDQUAY_ASYNCCLIENT_DTO gdTemp = new NVGDQUAY_ASYNCCLIENT_DTO();
                            gdTemp.ID = dataReader["ID"].ToString();
                            gdTemp.MAGIAODICH = dataReader["MAGIAODICH"].ToString();
                            gdTemp.MAGIAODICHQUAYPK = dataReader["MAGIAODICHQUAYPK"].ToString();
                            gdTemp.MADONVI = dataReader["MADONVI"].ToString();
                            DateTime.TryParse(dataReader["NGAYTAO"].ToString(), out ngayTao);
                            gdTemp.NGAYTAO = ngayTao;
                            DateTime.TryParse(dataReader["NGAYPHATSINH"].ToString(), out ngayPhatSinh);
                            gdTemp.NGAYPHATSINH = ngayPhatSinh;
                            gdTemp.MANGUOITAO = dataReader["MANGUOITAO"].ToString();
                            gdTemp.NGUOITAO = dataReader["NGUOITAO"].ToString();
                            gdTemp.MAQUAYBAN = dataReader["MAQUAYBAN"].ToString();
                            int.TryParse(dataReader["LOAIGIAODICH"].ToString(), out loaiGD);
                            gdTemp.LOAIGIAODICH = loaiGD;
                            gdTemp.HINHTHUCTHANHTOAN = dataReader["HINHTHUCTHANHTOAN"].ToString();
                            decimal.TryParse(dataReader["TIENKHACHDUA"].ToString(), out tienKhachDua);
                            gdTemp.TIENKHACHDUA = tienKhachDua;
                            decimal.TryParse(dataReader["TIENVOUCHER"].ToString(), out tienVoucher);
                            gdTemp.TIENVOUCHER = tienVoucher;
                            decimal.TryParse(dataReader["TIENTHEVIP"].ToString(), out tienTheVip);
                            gdTemp.TIENTHEVIP = tienTheVip;
                            decimal.TryParse(dataReader["TIENTRALAI"].ToString(), out tienTraLai);
                            gdTemp.TIENTRALAI = tienTraLai;
                            decimal.TryParse(dataReader["TIENTHE"].ToString(), out tienThe);
                            gdTemp.TIENTHE = tienThe;
                            decimal.TryParse(dataReader["TIENCOD"].ToString(), out tienCOD);
                            gdTemp.TIENCOD = tienCOD;
                            decimal.TryParse(dataReader["TIENMAT"].ToString(), out tienMat);
                            gdTemp.TIENMAT = tienMat;
                            decimal.TryParse(dataReader["TTIENCOVAT"].ToString(), out ttienCoVAT);
                            gdTemp.TTIENCOVAT = ttienCoVAT;
                            gdTemp.THOIGIAN = dataReader["THOIGIAN"].ToString();
                            gdTemp.MAKHACHHANG = dataReader["MAKHACHHANG"].ToString();
                            gdTemp.UNITCODE = dataReader["UNITCODE"].ToString();
                            OracleCommand command = new OracleCommand();
                            command.Connection = connection;
                            command.CommandText = @"SELECT * FROM NVHANGGDQUAY_ASYNCCLIENT WHERE MAGDQUAYPK = :maGDPK";
                            command.CommandType = CommandType.Text;
                            command.Parameters.Add("maGDPK", OracleDbType.NVarchar2, 50).Value = gdTemp.MAGIAODICHQUAYPK;
                            OracleDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                decimal giaBanLeCoVat,
                                    tyLeCK,
                                    tienCK,
                                    tyLeKM,
                                    tienKM,
                                    tyLeVoucher,
                                    tyLeLai,
                                    giaVon, vatBan = 0;
                                decimal soLuong, tongSoLuong = 0;
                                while (reader.Read())
                                {
                                    mh++;
                                    NVHANGGDQUAY_ASYNCCLIENT gd = new NVHANGGDQUAY_ASYNCCLIENT();
                                    gd.ID = reader["ID"].ToString();
                                    gd.MAGDQUAYPK = reader["MAGDQUAYPK"].ToString();
                                    gd.MAKHOHANG = reader["MAKHOHANG"].ToString();
                                    gd.MADONVI = reader["MADONVI"].ToString();
                                    gd.MAVATTU = reader["MAVATTU"].ToString();
                                    gd.NGUOITAO = reader["NGUOITAO"].ToString();
                                    gd.MABOPK = reader["MABOPK"].ToString();
                                    DateTime.TryParse(reader["NGAYTAO"].ToString(), out ngayTao);
                                    gd.NGAYTAO = ngayTao;
                                    DateTime.TryParse(reader["NGAYPHATSINH"].ToString(), out ngayPhatSinh);
                                    gd.NGAYPHATSINH = ngayPhatSinh;
                                    decimal.TryParse(reader["SOLUONG"].ToString(), out soLuong);
                                    gd.SOLUONG = soLuong;
                                    decimal.TryParse(reader["TTIENCOVAT"].ToString(), out ttienCoVAT);
                                    gd.TTIENCOVAT = ttienCoVAT;
                                    decimal.TryParse(reader["GIABANLECOVAT"].ToString(), out giaBanLeCoVat);
                                    gd.GIABANLECOVAT = giaBanLeCoVat;
                                    decimal.TryParse(reader["TYLECHIETKHAU"].ToString(), out tyLeCK);
                                    gd.TYLECHIETKHAU = tyLeCK;
                                    decimal.TryParse(reader["TIENCHIETKHAU"].ToString(), out tienCK);
                                    gd.TIENCHIETKHAU = tienCK;
                                    decimal.TryParse(reader["TYLEKHUYENMAI"].ToString(), out tyLeKM);
                                    gd.TYLEKHUYENMAI = tyLeKM;
                                    decimal.TryParse(reader["TIENKHUYENMAI"].ToString(), out tienKM);
                                    gd.TIENKHUYENMAI = tienKM;
                                    decimal.TryParse(reader["TYLEVOUCHER"].ToString(), out tyLeVoucher);
                                    gd.TYLEVOUCHER = tyLeVoucher;
                                    decimal.TryParse(reader["TIENVOUCHER"].ToString(), out tienVoucher);
                                    gd.TIENVOUCHER = tienVoucher;
                                    decimal.TryParse(reader["TYLELAILE"].ToString(), out tyLeLai);
                                    gd.TYLELAILE = tyLeLai;
                                    decimal.TryParse(reader["GIAVON"].ToString(), out giaVon);
                                    gd.GIAVON = giaVon;
                                    gd.MAVAT = reader["MAVAT"].ToString();
                                    decimal.TryParse(reader["VATBAN"].ToString(), out vatBan);
                                    gd.VATBAN = vatBan;
                                    gd.MACHUONGTRINHKM = reader["MACHUONGTRINHKM"].ToString();
                                    tongSoLuong += gd.SOLUONG;
                                    gdTemp.LST_DETAILS.Add(gd);
                                }
                                gdTemp.TONGSOLUONG = tongSoLuong;
                            }
                            lstGD.Add(gdTemp);
                        }
                    }
                    lblHoaDonSV.Text = hd.ToString();
                    lblMatHangSV.Text = mh.ToString();
                }
                connection.Close();
            }
            return lstGD;
        }

    }
}

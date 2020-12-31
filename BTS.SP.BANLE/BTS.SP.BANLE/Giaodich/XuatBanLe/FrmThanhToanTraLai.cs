using BTS.SP.BANLE.Common;
using BTS.SP.BANLE.Dto;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public partial class FrmThanhToanTraLai : Form
    {
        public XuLyThanhToan handler;
        private const int CP_NOCLOSE_BUTTON = 0x200;
        private decimal TONGTIEN_KHACHTRA = 0;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        //FORM THANH TOÁN BÁN LẺ TRẢ LẠI
        NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL = new NVGDQUAY_ASYNCCLIENT_DTO();
        NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = new NVGDQUAY_ASYNCCLIENT_DTO();
        public FrmThanhToanTraLai(NVGDQUAY_ASYNCCLIENT_DTO NVGDQUAY_ASYNCCLIENT_DTO, NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_BILL)
        {
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL = NVGDQUAY_ASYNCCLIENT_DTO;
            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = _NVGDQUAY_ASYNCCLIENT_BILL;
            InitializeComponent();
            int _currentUcFrame = FrmMain._currentUcFrame;
            this.Text = "THANH TOÁN HÓA ĐƠN TRẢ LẠI " + (_currentUcFrame + 1);
            txtThanhToan_MaGiaoDich.Text = _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.MAGIAODICH;
            txtThanhToan_TienThanhToan.Text = FormatCurrency.FormatMoney(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TTIENCOVAT);
            txtThanhToan_TienMat.Text = FormatCurrency.FormatMoney(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TTIENCOVAT);
            txtThanhToan_TienMat.Focus();
            this.ActiveControl = txtThanhToan_TienMat;
            txtThanhToan_TienMat.SelectAll();
            decimal.TryParse(txtThanhToan_TienMat.Text.Trim(), out TONGTIEN_KHACHTRA);
        }
        public void SetHanler(XuLyThanhToan xuLy)
        {
            this.handler = xuLy;
        }

        private void btnThanhToan_Exit_Click(object sender, EventArgs e)
        {
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL = new NVGDQUAY_ASYNCCLIENT_DTO();
            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = new NVGDQUAY_ASYNCCLIENT_DTO();
            this.Close();
        }
        /// <summary>
        /// Lưu dữ liệu bán lẻ
        /// </summary>
        /// <param name="header"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        private int SAVE_DATA_TO_ORACLE(NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO)
        {
            int countCheckInsertSuccess = 0;
            if (_NVGDQUAY_ASYNCCLIENT_DTO != null)
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
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
                                cmd.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.ID;
                                cmd.Parameters.Add("MAGIAODICH", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICH;
                                cmd.Parameters.Add("MAGIAODICHQUAYPK", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK;
                                cmd.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MADONVI;
                                cmd.Parameters.Add("LOAIGIAODICH", OracleDbType.Int32).Value = _NVGDQUAY_ASYNCCLIENT_DTO.LOAIGIAODICH;
                                cmd.Parameters.Add("NGAYTAO", OracleDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.NGAYTAO;
                                cmd.Parameters.Add("MANGUOITAO", OracleDbType.NVarchar2, 300).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MANGUOITAO;
                                cmd.Parameters.Add("NGUOITAO", OracleDbType.NVarchar2, 300).Value = _NVGDQUAY_ASYNCCLIENT_DTO.NGUOITAO;
                                cmd.Parameters.Add("MAQUAYBAN", OracleDbType.NVarchar2, 300).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAQUAYBAN;
                                cmd.Parameters.Add("NGAYPHATSINH", OracleDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH;
                                cmd.Parameters.Add("HINHTHUCTHANHTOAN", OracleDbType.NVarchar2, 200).Value = _NVGDQUAY_ASYNCCLIENT_DTO.HINHTHUCTHANHTOAN;
                                cmd.Parameters.Add("MAVOUCHER", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAVOUCHER;
                                cmd.Parameters.Add("TIENKHACHDUA", OracleDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACHDUA;
                                cmd.Parameters.Add("TIENVOUCHER", OracleDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENVOUCHER;
                                cmd.Parameters.Add("TIENTHEVIP", OracleDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHEVIP;
                                cmd.Parameters.Add("TIENTRALAI", OracleDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENTRALAI;
                                cmd.Parameters.Add("TIENTHE ", OracleDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHE >= _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT ? _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT : _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHE;
                                cmd.Parameters.Add("TIENCOD", OracleDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENCOD;
                                cmd.Parameters.Add("TIENMAT", OracleDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENMAT >= _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT ? _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT : _NVGDQUAY_ASYNCCLIENT_DTO.TIENMAT;
                                cmd.Parameters.Add("TTIENCOVAT", OracleDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT;
                                cmd.Parameters.Add("THOIGIAN", OracleDbType.NVarchar2, 150).Value = _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN;
                                cmd.Parameters.Add("MAKHACHHANG", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG;
                                cmd.Parameters.Add("I_CREATE_DATE", OracleDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE;
                                cmd.Parameters.Add("I_CREATE_BY", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY;
                                cmd.Parameters.Add("I_STATE", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.I_STATE;
                                cmd.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE;
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    countCheckInsertSuccess++;
                                    INSERT_DATA_HANG_GIAODICHQUAY(_NVGDQUAY_ASYNCCLIENT_DTO, ref countCheckInsertSuccess);

                                }
                                //chạy store tăng xuất nhập tồn        
                                if (!string.IsNullOrEmpty(Session.Session.CurrentTableNamePeriod))
                                {
                                    OracleCommand cmdTruTon = new OracleCommand();
                                    cmdTruTon.Connection = connection;
                                    cmdTruTon.CommandText = @"TBNETERP.XNT.XNT_TANG_PHIEU";
                                    cmdTruTon.CommandType = CommandType.StoredProcedure;
                                    cmdTruTon.Parameters.Add("P_TABLENAME", OracleDbType.Varchar2, 50).Value = Session.Session.CurrentTableNamePeriod;
                                    cmdTruTon.Parameters.Add("P_NAM", OracleDbType.Varchar2, 50).Value = Session.Session.CurrentYear;
                                    cmdTruTon.Parameters.Add("P_KY", OracleDbType.Decimal).Value = decimal.Parse(Session.Session.CurrentPeriod);
                                    cmdTruTon.Parameters.Add("P_ID", OracleDbType.Varchar2, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.ID;
                                    cmdTruTon.ExecuteNonQuery();
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLogs.LogError(ex);
                            }
                            finally
                            {
                                SAVE_DATA_TO_SQL_HAVE_INTERNET(_NVGDQUAY_ASYNCCLIENT_DTO);
                                connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
            return countCheckInsertSuccess;
        }
        private int SAVE_DATA_TO_SQL_HAVE_INTERNET(NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO)
        {
            int countCheckInsertSuccess = 0;
            if (_NVGDQUAY_ASYNCCLIENT_DTO != null)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryInsert = "";
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = connection;
                                queryInsert = string.Format(@"INSERT INTO [dbo].[NVGDQUAY_ASYNCCLIENT]([ID],[MAGIAODICH],[MAGIAODICHQUAYPK],[MADONVI],[NGAYTAO],[NGAYPHATSINH],[MANGUOITAO],[NGUOITAO],[MAQUAYBAN],[LOAIGIAODICH],
                                    [HINHTHUCTHANHTOAN],[TIENKHACHDUA],[TIENVOUCHER],[TIENTHEVIP],[TIENTRALAI],[TIENTHE],[TIENCOD],[TIENMAT],[TTIENCOVAT],[THOIGIAN],[MAKHACHHANG],[UNITCODE]) VALUES (
                                   @ID,@MAGIAODICH,@MAGIAODICHQUAYPK,@MADONVI,@NGAYTAO,@NGAYPHATSINH,@MANGUOITAO,@NGUOITAO,@MAQUAYBAN,@LOAIGIAODICH,@HINHTHUCTHANHTOAN,@TIENKHACHDUA,@TIENVOUCHER,@TIENTHEVIP,@TIENTRALAI,@TIENTHE,@TIENCOD,@TIENMAT,@TTIENCOVAT,@THOIGIAN,@MAKHACHHANG,@UNITCODE)");
                                cmd.CommandText = queryInsert;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add("ID", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.ID;
                                cmd.Parameters.Add("MAGIAODICH", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICH;
                                cmd.Parameters.Add("MAGIAODICHQUAYPK", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK;
                                cmd.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MADONVI;
                                cmd.Parameters.Add("NGAYTAO", SqlDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.NGAYTAO;
                                cmd.Parameters.Add("NGAYPHATSINH", SqlDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH;
                                cmd.Parameters.Add("MANGUOITAO", SqlDbType.NVarChar, 300).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MANGUOITAO;
                                cmd.Parameters.Add("NGUOITAO", SqlDbType.NVarChar, 300).Value = _NVGDQUAY_ASYNCCLIENT_DTO.NGUOITAO;
                                cmd.Parameters.Add("MAQUAYBAN", SqlDbType.NVarChar, 300).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAQUAYBAN;
                                cmd.Parameters.Add("LOAIGIAODICH", SqlDbType.Int).Value = _NVGDQUAY_ASYNCCLIENT_DTO.LOAIGIAODICH;
                                cmd.Parameters.Add("HINHTHUCTHANHTOAN", SqlDbType.NVarChar, 200).Value = _NVGDQUAY_ASYNCCLIENT_DTO.HINHTHUCTHANHTOAN;
                                cmd.Parameters.Add("MAVOUCHER", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAVOUCHER;
                                cmd.Parameters.Add("TIENKHACHDUA", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACHDUA;
                                cmd.Parameters.Add("TIENVOUCHER", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENVOUCHER;
                                cmd.Parameters.Add("TIENTHEVIP", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHEVIP;
                                cmd.Parameters.Add("TIENTRALAI", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENTRALAI;
                                cmd.Parameters.Add("TIENTHE ", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHE >= _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT ? _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT : _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHE;
                                cmd.Parameters.Add("TIENCOD", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENCOD;
                                cmd.Parameters.Add("TIENMAT", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENMAT >= _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT ? _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT : _NVGDQUAY_ASYNCCLIENT_DTO.TIENMAT;
                                cmd.Parameters.Add("TTIENCOVAT", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT;
                                cmd.Parameters.Add("THOIGIAN", SqlDbType.NVarChar, 150).Value = _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN;
                                cmd.Parameters.Add("MAKHACHHANG", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG;
                                cmd.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE;
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    countCheckInsertSuccess++;
                                    INSERT_DATA_HANG_GIAODICHQUAY_SQL(_NVGDQUAY_ASYNCCLIENT_DTO, ref countCheckInsertSuccess);
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLogs.LogError(ex);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
            return countCheckInsertSuccess;
        }

        private int SAVE_DATA_TO_SQL(NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO)
        {
            int countCheckInsertSuccess = 0;
            if (_NVGDQUAY_ASYNCCLIENT_DTO != null)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryInsert = "";
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = connection;
                                queryInsert = string.Format(@"INSERT INTO [dbo].[NVGDQUAY_ASYNCCLIENT]([ID],[MAGIAODICH],[MAGIAODICHQUAYPK],[MADONVI],[NGAYTAO],[NGAYPHATSINH],[MANGUOITAO],[NGUOITAO],[MAQUAYBAN],[LOAIGIAODICH],
                                    [HINHTHUCTHANHTOAN],[TIENKHACHDUA],[TIENVOUCHER],[TIENTHEVIP],[TIENTRALAI],[TIENTHE],[TIENCOD],[TIENMAT],[TTIENCOVAT],[THOIGIAN],[MAKHACHHANG],[UNITCODE]) VALUES (
                                    @ID,@MAGIAODICH,@MAGIAODICHQUAYPK,@MADONVI,@NGAYTAO,@NGAYPHATSINH,@MANGUOITAO,@NGUOITAO,@MAQUAYBAN,@LOAIGIAODICH,@HINHTHUCTHANHTOAN,@TIENKHACHDUA,@TIENVOUCHER,@TIENTHEVIP,@TIENTRALAI,@TIENTHE,@TIENCOD,@TIENMAT,@TTIENCOVAT,@THOIGIAN,@MAKHACHHANG,@UNITCODE)");
                                cmd.CommandText = queryInsert;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add("ID", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.ID;
                                cmd.Parameters.Add("MAGIAODICH", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICH;
                                cmd.Parameters.Add("MAGIAODICHQUAYPK", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK;
                                cmd.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MADONVI;
                                cmd.Parameters.Add("NGAYTAO", SqlDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.NGAYTAO;
                                cmd.Parameters.Add("NGAYPHATSINH", SqlDbType.Date).Value = _NVGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH;
                                cmd.Parameters.Add("MANGUOITAO", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MANGUOITAO;
                                cmd.Parameters.Add("NGUOITAO", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.NGUOITAO;
                                cmd.Parameters.Add("MAQUAYBAN", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAQUAYBAN;
                                cmd.Parameters.Add("LOAIGIAODICH", SqlDbType.Int).Value = _NVGDQUAY_ASYNCCLIENT_DTO.LOAIGIAODICH;
                                cmd.Parameters.Add("HINHTHUCTHANHTOAN", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.HINHTHUCTHANHTOAN;
                                cmd.Parameters.Add("MAVOUCHER", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAVOUCHER;
                                cmd.Parameters.Add("TIENKHACHDUA", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACHDUA;
                                cmd.Parameters.Add("TIENVOUCHER", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENVOUCHER;
                                cmd.Parameters.Add("TIENTHEVIP", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHEVIP;
                                cmd.Parameters.Add("TIENTRALAI", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENTRALAI;
                                cmd.Parameters.Add("TIENTHE ", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHE >= _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT ? _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT : _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHE;
                                cmd.Parameters.Add("TIENCOD", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENCOD;
                                cmd.Parameters.Add("TIENMAT", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TIENMAT >= _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT ? _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT : _NVGDQUAY_ASYNCCLIENT_DTO.TIENMAT;
                                cmd.Parameters.Add("TTIENCOVAT", SqlDbType.Decimal).Value = _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT;
                                cmd.Parameters.Add("THOIGIAN", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN;
                                cmd.Parameters.Add("MAKHACHHANG", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG;
                                cmd.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE;
                                if (cmd.ExecuteNonQuery() > 0)
                                {
                                    countCheckInsertSuccess++;
                                    INSERT_DATA_HANG_GIAODICHQUAY_SQL(_NVGDQUAY_ASYNCCLIENT_DTO, ref countCheckInsertSuccess);

                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLogs.LogError(ex);
                            }
                            finally
                            {
                                connection.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
            return countCheckInsertSuccess;
        }

        private void LUU_DULIEU(NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO)
        {
            try
            {
                int countSave = 0;
                string msg = Config.CheckConnectToServer(out bool result);
                if (msg.Length > 0) { MessageBox.Show(msg); return; }

                if (result) countSave = SAVE_DATA_TO_ORACLE(_NVGDQUAY_ASYNCCLIENT_DTO);
                else countSave = SAVE_DATA_TO_SQL(_NVGDQUAY_ASYNCCLIENT_DTO);
                if (countSave >= 2) NotificationLauncher.ShowNotification("Thông báo", "Hoàn thành giao dịch", 1, "0x1", "0x8", "normal");
            }
            catch (Exception ex)
            {
                WriteLogs.LogError(ex);
            }
        }
        private void THANHTOAN_HOADON_BANLE_TRALAI()
        {
            string TONGTIEN_BANGCHU = ConvertSoThanhChu.ChuyenDoiSoThanhChu(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TTIENCOVAT);
            try
            {
                LUU_DULIEU(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL);
            }
            catch
            {
                MessageBox.Show("CẢNH BÁO ! XẢY RA LỖI KHI LƯU HÓA ĐƠN NÀY, HÃY LƯU LẠI HÓA ĐƠN ĐỂ KIỂM TRA ! XIN CẢM ƠN ");
            }
            try
            {
                string MA_TEN_KHACHHANG = "";

                string msg = Config.CheckConnectToServer(out bool result);
                if (msg.Length > 0) { MessageBox.Show(msg); return; }

                if (result) MA_TEN_KHACHHANG = FrmThanhToanService.LAY_MA_TEN_KHACHHANG_FROM_ORACLE(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG);
                else MA_TEN_KHACHHANG = FrmThanhToanService.LAY_MA_TEN_KHACHHANG_FROM_SQLSERVER(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG);

                using (frmPrintBill_TraLai frmBanTraLai = new frmPrintBill_TraLai())
                {
                    try
                    {
                        BILL_DTO infoBill = new BILL_DTO()
                        {
                            ADDRESS = Session.Session.CurrentAddress,
                            CONLAI = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENTRALAI,
                            PHONE = Session.Session.CurrentPhone,
                            MAKH = MA_TEN_KHACHHANG,
                            DIEM = 0,
                            INFOTHUNGAN = "THU NGÂN: " + _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.NGUOITAO + "\t QUẦY: " + _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAQUAYBAN,
                            MAGIAODICH = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAGIAODICH,
                            THANHTIENCHU = TONGTIEN_BANGCHU,
                            TIENKHACHTRA = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENKHACHDUA,
                            QUAYHANG = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAQUAYBAN,
                        };
                        frmBanTraLai.PrintInvoice_BanLeTraLai(infoBill, _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL);
                    }
                    catch
                    {
                    }
                    finally
                    {
                        this.handler(true);
                        this.Dispose();
                        frmBanTraLai.Dispose();
                        frmBanTraLai.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogs.LogError(ex);
            }
        }
        private void FrmThanhToan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL = new NVGDQUAY_ASYNCCLIENT_DTO();
                _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = new NVGDQUAY_ASYNCCLIENT_DTO();
            }
            if (e.KeyCode == Keys.Enter)
            {
                decimal tienThanhToan = 0;
                decimal.TryParse(txtThanhToan_TienTraLai.Text, out tienThanhToan);
                if (tienThanhToan >= 0)
                {
                    THANHTOAN_HOADON_BANLE_TRALAI();
                }
                else
                {
                    NotificationLauncher.ShowNotificationError("Thông báo", "Sai tiền !", 1, "0x1", "0x8", "normal");
                }
            }
        }
        private void btnThanhToan_Save_Click(object sender, EventArgs e)
        {
            decimal tienThanhToan = 0;
            decimal.TryParse(txtThanhToan_TienTraLai.Text.ToString(), out tienThanhToan);
            if (tienThanhToan >= 0)
            {
                THANHTOAN_HOADON_BANLE_TRALAI();
            }
            else
            {
                NotificationLauncher.ShowNotificationError("Thông báo", "Sai tiền !", 1, "0x1", "0x8", "normal");
            }
        }
        private void txtThanhToan_TienMat_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (txtThanhToan_TienMat.Text.Trim().Length > 0)
            {
                decimal TIENMAT_KHACHTRA, TONGTIEN_HOADON, TIEN_TRALAI = 0;
                decimal.TryParse(txtThanhToan_TienMat.Text.Trim(), out TIENMAT_KHACHTRA);
                decimal.TryParse(txtThanhToan_TienThanhToan.Text.Trim(), out TONGTIEN_HOADON);
                TIEN_TRALAI = TIENMAT_KHACHTRA - TONGTIEN_HOADON;
                if (TIEN_TRALAI >= 0 && TIEN_TRALAI < 1000000)
                {
                    txtThanhToan_TienTraLai.Text = FormatCurrency.FormatMoney(TIEN_TRALAI);
                    txtThanhToan_TraLai_BangChu.Text = ConvertSoThanhChu.ChuyenDoiSoThanhChu(TIEN_TRALAI);
                    btnThanhToan_Save.Focus();
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENMAT = TIENMAT_KHACHTRA;
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENTRALAI = TIEN_TRALAI;
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENKHACHDUA = TIENMAT_KHACHTRA;
                }
                else if (TIEN_TRALAI < 0)
                {
                    txtThanhToan_TienMat.SelectAll();
                }
                else
                {
                    DialogResult result = MessageBox.Show("SỐ TIỀN TRẢ LẠI QUÁ LỚN ?", "THAO TÁC SAI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (result == DialogResult.OK)
                    {
                        TIEN_TRALAI = TIEN_TRALAI / 10;
                        txtThanhToan_TienTraLai.Text = FormatCurrency.FormatMoney(TIEN_TRALAI);
                        txtThanhToan_TraLai_BangChu.Text = ConvertSoThanhChu.ChuyenDoiSoThanhChu(TIEN_TRALAI);
                        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENMAT = TIENMAT_KHACHTRA;
                        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENTRALAI = TIEN_TRALAI;
                        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENKHACHDUA = TIENMAT_KHACHTRA;
                    }
                }
            }
        }
        private void txtThanhToan_TienMat_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                if (!txtThanhToan_TienMat.Text.Equals(txtThanhToan_TienThanhToan.Text))
                {
                    decimal THANHTOAN_TIENTHE = 0;
                    string THANHTOAN_TIENMAT_STRING = txtThanhToan_TienMat.Text;
                    int start = txtThanhToan_TienMat.Text.Length - txtThanhToan_TienMat.SelectionStart;
                    THANHTOAN_TIENMAT_STRING = THANHTOAN_TIENMAT_STRING.Replace(",", "");
                    decimal THANHTOAN_TIENMAT = 0;
                    decimal.TryParse(THANHTOAN_TIENMAT_STRING, out THANHTOAN_TIENMAT);
                    TONGTIEN_KHACHTRA = THANHTOAN_TIENMAT;
                    decimal THANHTOAN_TONGTIEN = 0;
                    decimal.TryParse(txtThanhToan_TienThanhToan.Text.Trim().Replace(",", ""), out THANHTOAN_TONGTIEN);
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENMAT = THANHTOAN_TIENMAT;
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENKHACHDUA = THANHTOAN_TIENMAT;
                    decimal THANHTOAN_TIENTRALAI = THANHTOAN_TIENTHE + THANHTOAN_TIENMAT - THANHTOAN_TONGTIEN;
                    txtThanhToan_TienMat.Text = FormatCurrency.FormatMoney(THANHTOAN_TIENMAT_STRING);
                    txtThanhToan_TienMat.SelectionStart = -start + txtThanhToan_TienMat.Text.Length;
                    if (THANHTOAN_TIENTRALAI > 0)
                    {
                        txtThanhToan_TienTraLai.Text = FormatCurrency.FormatMoney(THANHTOAN_TIENTRALAI);
                        txtThanhToan_TraLai_BangChu.Text = ConvertSoThanhChu.ChuyenDoiSoThanhChu(THANHTOAN_TIENTRALAI);
                    }
                    else
                    {
                        txtThanhToan_TienTraLai.Text = "-" + FormatCurrency.FormatMoney(THANHTOAN_TIENTRALAI);
                    }
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENTRALAI = THANHTOAN_TIENTRALAI;
                }
                else
                {
                    return;
                }
            }
            else
            {
                decimal tienThanhToan = 0;
                decimal.TryParse(txtThanhToan_TienTraLai.Text, out tienThanhToan);
                if (tienThanhToan >= 0)
                {
                    THANHTOAN_HOADON_BANLE_TRALAI();
                }
                else
                {
                    NotificationLauncher.ShowNotificationError("THÔNG BÁO", "SỐ TIỀN SAI !", 1, "0x1", "0x8", "normal");
                }
            }
        }

        private void INSERT_DATA_HANG_GIAODICHQUAY(NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO, ref int countCheckInsertSuccess)
        {
            if (_NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Count > 0)
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            foreach (NVHANGGDQUAY_ASYNCCLIENT ITEM in _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS)
                            {
                                OracleCommand command = new OracleCommand();
                                command.Connection = connection;
                                string queryInsertItem = string.Format(@"INSERT INTO NVHANGGDQUAY_ASYNCCLIENT (ID,MAGDQUAYPK,MAKHOHANG,MADONVI,MAVATTU,BARCODE,TENDAYDU,NGUOITAO,MABOPK,NGAYTAO,NGAYPHATSINH,
                                    SOLUONG,TTIENCOVAT,VATBAN,GIABANLECOVAT,MAKHACHHANG,MAKEHANG,MACHUONGTRINHKM,LOAIKHUYENMAI,TIENCHIETKHAU,TYLECHIETKHAU,TYLEKHUYENMAI,TIENKHUYENMAI,TYLEVOUCHER,
                                    TIENVOUCHER,TYLELAILE,GIAVON,ISBANAM,MAVAT) VALUES (:ID,:MAGDQUAYPK,:MAKHOHANG,:MADONVI,:MAVATTU,:BARCODE,:TENDAYDU,:NGUOITAO,:MABOPK,:NGAYTAO,:NGAYPHATSINH,
                                    :SOLUONG,:TTIENCOVAT,:VATBAN,:GIABANLECOVAT,:MAKHACHHANG,:MAKEHANG,:MACHUONGTRINHKM,:LOAIKHUYENMAI,:TIENCHIETKHAU,:TYLECHIETKHAU,:TYLEKHUYENMAI,:TIENKHUYENMAI,:TYLEVOUCHER,
                                    :TIENVOUCHER,:TYLELAILE,:GIAVON,:ISBANAM,:MAVAT)");
                                command.CommandText = queryInsertItem;
                                command.CommandType = CommandType.Text;
                                command.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = ITEM.ID;
                                command.Parameters.Add("MAGDQUAYPK", OracleDbType.NVarchar2, 50).Value = ITEM.MAGDQUAYPK;
                                command.Parameters.Add("MAKHOHANG", OracleDbType.NVarchar2, 50).Value = ITEM.MAKHOHANG;
                                command.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value = ITEM.MADONVI;
                                command.Parameters.Add("MAVATTU", OracleDbType.NVarchar2, 50).Value = ITEM.MAVATTU;
                                command.Parameters.Add("BARCODE", OracleDbType.NVarchar2, 2000).Value = ITEM.BARCODE;
                                command.Parameters.Add("TENDAYDU", OracleDbType.NVarchar2, 300).Value = ITEM.TENDAYDU;
                                command.Parameters.Add("NGUOITAO", OracleDbType.NVarchar2, 300).Value = ITEM.NGUOITAO;
                                command.Parameters.Add("MABOPK", OracleDbType.NVarchar2, 50).Value = ITEM.MABOPK;
                                command.Parameters.Add("NGAYTAO", OracleDbType.Date).Value = ITEM.NGAYTAO;
                                command.Parameters.Add("NGAYPHATSINH", OracleDbType.Date).Value = ITEM.NGAYPHATSINH;
                                command.Parameters.Add("SOLUONG", OracleDbType.Int32).Value = ITEM.SOLUONG;
                                command.Parameters.Add("TTIENCOVAT", OracleDbType.Decimal).Value = ITEM.TTIENCOVAT;
                                command.Parameters.Add("VATBAN", OracleDbType.Decimal).Value = ITEM.VATBAN;
                                command.Parameters.Add("GIABANLECOVAT", OracleDbType.Decimal).Value = ITEM.GIABANLECOVAT;
                                command.Parameters.Add("MAKHACHHANG", OracleDbType.NVarchar2, 50).Value = ITEM.MAKHACHHANG;
                                command.Parameters.Add("MAKEHANG", OracleDbType.NVarchar2, 50).Value = ITEM.MAKEHANG;
                                command.Parameters.Add("MACHUONGTRINHKM", OracleDbType.NVarchar2, 50).Value = ITEM.MACHUONGTRINHKM;
                                command.Parameters.Add("LOAIKHUYENMAI", OracleDbType.NVarchar2, 50).Value = ITEM.LOAIKHUYENMAI;
                                command.Parameters.Add("TIENCHIETKHAU", OracleDbType.Decimal).Value = ITEM.TIENCHIETKHAU;
                                command.Parameters.Add("TYLECHIETKHAU", OracleDbType.Decimal).Value = ITEM.TYLECHIETKHAU;
                                command.Parameters.Add("TYLEKHUYENMAI", OracleDbType.Decimal).Value = ITEM.TYLEKHUYENMAI;
                                command.Parameters.Add("TIENKHUYENMAI", OracleDbType.Decimal).Value = ITEM.TIENKHUYENMAI;
                                command.Parameters.Add("TYLEVOUCHER", OracleDbType.Decimal).Value = ITEM.TYLEVOUCHER;
                                command.Parameters.Add("TIENVOUCHER", OracleDbType.Decimal).Value = ITEM.TIENVOUCHER;
                                command.Parameters.Add("TYLELAILE", OracleDbType.Decimal).Value = ITEM.TYLELAILE;
                                command.Parameters.Add("GIAVON", OracleDbType.Decimal).Value = ITEM.GIAVON;
                                command.Parameters.Add("ISBANAM", OracleDbType.Decimal).Value = ITEM.ISBANAM;
                                command.Parameters.Add("MAVAT", OracleDbType.NVarchar2, 50).Value = ITEM.MAVAT;
                                try
                                {
                                    if (command.ExecuteNonQuery() > 0) countCheckInsertSuccess++;
                                }
                                catch (Exception e)
                                {
                                    WriteLogs.LogError(e);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLogs.LogError(ex);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }
                    }
                    else
                    {
                        NotificationLauncher.ShowNotificationError("THÔNG BÁO", "KHÔNG CÓ KẾT NỐI VỚI CƠ SỞ DỮ LIỆU ORACLE", 1, "0x1", "0x8", "normal");
                    }
                }

            }
        }

        private void INSERT_DATA_HANG_GIAODICHQUAY_SQL(NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO, ref int countCheckInsertSuccess)
        {
            if (_NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            foreach (NVHANGGDQUAY_ASYNCCLIENT ITEM in _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS)
                            {
                                SqlCommand command = new SqlCommand();
                                command.Connection = connection;
                                string queryInsertItem = string.Format(@"INSERT INTO [dbo].[NVHANGGDQUAY_ASYNCCLIENT]([ID],[MAGDQUAYPK],[MAKHOHANG],[MADONVI],[MAVATTU],[MANGUOITAO],[NGUOITAO],[MABOPK],[NGAYTAO],[NGAYPHATSINH],[SOLUONG],[TTIENCOVAT],[GIABANLECOVAT],[TYLECHIETKHAU],[TIENCHIETKHAU],[TYLEKHUYENMAI],[TIENKHUYENMAI],[TYLEVOUCHER],[TIENVOUCHER],[TYLELAILE],[GIAVON],[MAVAT],[VATBAN],[MACHUONGTRINHKM],[UNITCODE]) 
                                    VALUES (@ID,@MAGDQUAYPK,@MAKHOHANG,@MADONVI,@MAVATTU,@MANGUOITAO,@NGUOITAO,@MABOPK,@NGAYTAO,@NGAYPHATSINH,@SOLUONG,@TTIENCOVAT,@GIABANLECOVAT,@TYLECHIETKHAU,@TIENCHIETKHAU,@TYLEKHUYENMAI,@TIENKHUYENMAI,@TYLEVOUCHER,@TIENVOUCHER,@TYLELAILE,@GIAVON,@MAVAT,@VATBAN,@MACHUONGTRINHKM,@UNITCODE)");
                                command.CommandText = queryInsertItem;
                                command.CommandType = CommandType.Text;
                                command.Parameters.Add("ID", SqlDbType.NVarChar, 50).Value = ITEM.ID;
                                command.Parameters.Add("MAGDQUAYPK", SqlDbType.NVarChar, 50).Value = ITEM.MAGDQUAYPK;
                                command.Parameters.Add("MAKHOHANG", SqlDbType.NVarChar, 50).Value = ITEM.MAKHOHANG;
                                command.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value = ITEM.MADONVI;
                                command.Parameters.Add("MAVATTU", SqlDbType.NVarChar, 50).Value = ITEM.MAVATTU;
                                command.Parameters.Add("MANGUOITAO", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentMaNhanVien;
                                command.Parameters.Add("NGUOITAO", SqlDbType.NVarChar, 50).Value = ITEM.NGUOITAO;
                                command.Parameters.Add("MABOPK", SqlDbType.NVarChar, 50).Value = ITEM.MABOPK;
                                command.Parameters.Add("NGAYTAO", SqlDbType.Date).Value = ITEM.NGAYTAO;
                                command.Parameters.Add("NGAYPHATSINH", SqlDbType.Date).Value = ITEM.NGAYPHATSINH;
                                command.Parameters.Add("SOLUONG", SqlDbType.Decimal).Value = ITEM.SOLUONG;
                                command.Parameters.Add("TTIENCOVAT", SqlDbType.Decimal).Value = ITEM.TTIENCOVAT;
                                command.Parameters.Add("GIABANLECOVAT", SqlDbType.Decimal).Value = ITEM.GIABANLECOVAT;
                                command.Parameters.Add("TIENCHIETKHAU", SqlDbType.Decimal).Value = ITEM.TIENCHIETKHAU;
                                command.Parameters.Add("TYLECHIETKHAU", SqlDbType.Decimal).Value = ITEM.TYLECHIETKHAU;
                                command.Parameters.Add("TYLEKHUYENMAI", SqlDbType.Decimal).Value = ITEM.TYLEKHUYENMAI;
                                command.Parameters.Add("TIENKHUYENMAI", SqlDbType.Decimal).Value = ITEM.TIENKHUYENMAI;
                                command.Parameters.Add("TYLEVOUCHER", SqlDbType.Decimal).Value = ITEM.TYLEVOUCHER;
                                command.Parameters.Add("TIENVOUCHER", SqlDbType.Decimal).Value = ITEM.TIENVOUCHER;
                                command.Parameters.Add("TYLELAILE", SqlDbType.Decimal).Value = ITEM.TYLELAILE;
                                command.Parameters.Add("GIAVON", SqlDbType.Decimal).Value = ITEM.GIAVON;
                                command.Parameters.Add("MAVAT", SqlDbType.NVarChar, 50).Value = ITEM.MAVAT;
                                command.Parameters.Add("VATBAN", SqlDbType.Decimal).Value = ITEM.VATBAN;
                                command.Parameters.Add("MACHUONGTRINHKM", SqlDbType.NVarChar, 50).Value = ITEM.MACHUONGTRINHKM;
                                command.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = ITEM.UNITCODE != null ? ITEM.UNITCODE : Session.Session.CurrentUnitCode;
                                try
                                {
                                    if (command.ExecuteNonQuery() > 0) countCheckInsertSuccess++;
                                }
                                catch (Exception e)
                                {
                                    WriteLogs.LogError(e);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteLogs.LogError(ex);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }
                    }
                    else
                    {
                        NotificationLauncher.ShowNotificationError("Thông báo", "Không có kế nối với cơ sở dữ liệu", 1, "0x1", "0x8", "normal");
                    }
                }

            }
        }
    }
}

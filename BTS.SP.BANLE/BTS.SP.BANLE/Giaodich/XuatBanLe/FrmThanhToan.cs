using BTS.SP.BANLE.Common;
using BTS.SP.BANLE.Danhmuc;
using BTS.SP.BANLE.Dto;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public delegate void STATUS_TIMKIEM_KHACHHANG(KHACHHANG_DTO _KHACHHANG_DTO);
    public delegate void STATUS_THEMMOI_KHACHHANG(KHACHHANG_DTO _KHACHHANG_DTO);
    public partial class FrmThanhToan : Form
    {
        public STATUS_THEMMOI_KHACHHANG _STATUS_THEMMOI_KHACHHANG;
        public XuLyThanhToan handler;
        private EnumCommon.MethodSelect HINHTHUC_THANHTOAN_TIENMAT;
        private EnumCommon.MethodSelect HINHTHUC_THANHTOAN_TIENTHE;
        private EnumCommon.MethodSelect HINHTHUC_THANHTOAN_PHIEU;
        private EnumCommon.LoaiGiaoDich LOAIGIAODICH_GLOBAL;
        private List<VATTU_DTO.ViewModelGrid> dataDetails = new List<VATTU_DTO.ViewModelGrid>();
        private const int CP_NOCLOSE_BUTTON = 0x200;
        private int LOAIGD = 0;
        private bool LOG_THANHTOAN = false;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        //FORM THANH TOÁN BÁN LẺ
        NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL = new NVGDQUAY_ASYNCCLIENT_DTO();
        NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = new NVGDQUAY_ASYNCCLIENT_DTO();
        NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO_CLONE = new NVGDQUAY_ASYNCCLIENT_DTO();
        NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_BILL_CLONE = new NVGDQUAY_ASYNCCLIENT_DTO();

        decimal GLOBAL_TONGTIEN_HOADON_BANDAU = new decimal();
        public FrmThanhToan(NVGDQUAY_ASYNCCLIENT_DTO NVGDQUAY_ASYNCCLIENT_DTO, NVGDQUAY_ASYNCCLIENT_DTO NVGDQUAY_ASYNCCLIENT_BILL, EnumCommon.LoaiGiaoDich loaiGiaoDich, decimal TONGTIEN_HOADON_BANDAU)
        {
            InitializeComponent();
            LOAIGIAODICH_GLOBAL = loaiGiaoDich;
            int _currentUcFrame = FrmMain._currentUcFrame;
            this.Text = "THANH TOÁN HÓA ĐƠN - XUẤT BÁN LẺ " + (_currentUcFrame + 1);
            txtThanhToan_MaGiaoDich.Text = UC_Frame_BanLe.THANHTOAN_MAGIAODICH;
            txtThanhToan_TienThanhToan.Text = FormatCurrency.FormatMoney(UC_Frame_BanLe.THANHTOAN_TONGTIEN_THANHTOAN);
            txtThanhToan_TienMat.Text = FormatCurrency.FormatMoney(UC_Frame_BanLe.THANHTOAN_TONGTIEN_THANHTOAN);
            txtThanhToan_TienQuyDoiDiem.Text = "0";
            txtThanhToan_TienPhaiTra.Text = FormatCurrency.FormatMoney(UC_Frame_BanLe.THANHTOAN_TONGTIEN_THANHTOAN);
            LOAIGD = UC_Frame_BanLe.LOAIGIAODICH;
            txtThanhToan_TienMat.Focus();
            this.ActiveControl = txtThanhToan_TienMat;
            HINHTHUC_THANHTOAN_TIENMAT = EnumCommon.MethodSelect.SUDUNG;
            txtThanhToan_TienMat.SelectAll();
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL = NVGDQUAY_ASYNCCLIENT_DTO;
            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = NVGDQUAY_ASYNCCLIENT_BILL;
            GLOBAL_TONGTIEN_HOADON_BANDAU = TONGTIEN_HOADON_BANDAU;
            txtThanhToan_MaKhachHang.ReadOnly = true;
            btnF10_QuyDoi.Visible = false;
            btnF10_QuyDoi.Enabled = false;
            txtThanhToan_QuyDoi_ToiDa.Visible = false;
            _NVGDQUAY_ASYNCCLIENT_DTO_CLONE = new NVGDQUAY_ASYNCCLIENT_DTO();
            _NVGDQUAY_ASYNCCLIENT_DTO_CLONE = ObjectCopier.Clone(NVGDQUAY_ASYNCCLIENT_DTO);

            _NVGDQUAY_ASYNCCLIENT_BILL_CLONE = new NVGDQUAY_ASYNCCLIENT_DTO();
            _NVGDQUAY_ASYNCCLIENT_BILL_CLONE = ObjectCopier.Clone(NVGDQUAY_ASYNCCLIENT_BILL);
        }
        public void SetHanler(XuLyThanhToan xuLy)
        {
            this.handler = xuLy;
        }
        public void SETHANDLER_STATUS_THEMMOI_KHACHHANG(STATUS_THEMMOI_KHACHHANG _STATUS_THEMMOI_KHACHHANG)
        {
            this._STATUS_THEMMOI_KHACHHANG = _STATUS_THEMMOI_KHACHHANG;
        }
        private void btnThanhToan_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UPDATE_KHACHHANG_TO_ORACLE(string MAKHACHHANG, decimal QUYDOI_TIEN_THANH_DIEM, decimal TONGTIEN, string HANG_KHACHHANG)
        {
            if (!string.IsNullOrEmpty(MAKHACHHANG) && TONGTIEN > 0)
            {
                //trừ điểm hiện tại trước
                decimal SODIEM_MOI = 0;
                SODIEM_MOI = _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.SODIEM - _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.DIEMQUYDOI;
                decimal DIEM_TICH_LUY = 0;
                if (QUYDOI_TIEN_THANH_DIEM != 0) DIEM_TICH_LUY = SODIEM_MOI + decimal.Round(TONGTIEN / QUYDOI_TIEN_THANH_DIEM, 2);
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryUpdate = "";
                                OracleCommand cmd = new OracleCommand();
                                cmd.Connection = connection;
                                if (string.IsNullOrEmpty(HANG_KHACHHANG) || HANG_KHACHHANG.Equals(""))
                                {
                                    queryUpdate = string.Format(@"UPDATE DM_KHACHHANG SET HANGKHACHHANG = '" + _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG_MOI + "' ,SODIEM = NVL(" + DIEM_TICH_LUY + ", 0), TONGTIEN = NVL(TONGTIEN, 0) + " + TONGTIEN + " WHERE MAKH = '" + MAKHACHHANG + "'");
                                }
                                else
                                {
                                    queryUpdate = string.Format(@"UPDATE DM_KHACHHANG SET SODIEM = NVL(" + DIEM_TICH_LUY + ", 0), TONGTIEN = NVL(TONGTIEN, 0) + " + TONGTIEN + " WHERE MAKH = '" + MAKHACHHANG + "'");
                                }
                                cmd.CommandText = queryUpdate;
                                cmd.CommandType = CommandType.Text;
                                int count = cmd.ExecuteNonQuery();
                                if (count > 0)
                                {
                                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.SODIEM = DIEM_TICH_LUY;
                                    decimal SOTIEN_LENHANG = 0;
                                    queryUpdate = string.Format(@"SELECT MAHANGKH,SOTIEN FROM (SELECT MAHANGKH,SOTIEN FROM DM_HANGKHACHHANG WHERE SOTIEN > " + _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.SOTIEN_LENHANG + " ORDER BY SOTIEN ASC) WHERE ROWNUM = 1");
                                    cmd.CommandText = queryUpdate;
                                    cmd.CommandType = CommandType.Text;
                                    OracleDataReader dataReaderHangKhachHang = null;
                                    dataReaderHangKhachHang = cmd.ExecuteReader();
                                    if (dataReaderHangKhachHang.HasRows)
                                    {
                                        while (dataReaderHangKhachHang.Read())
                                        {
                                            if (dataReaderHangKhachHang["SOTIEN"] != null) decimal.TryParse(dataReaderHangKhachHang["SOTIEN"].ToString(), out SOTIEN_LENHANG);
                                            string MAHANGKH = dataReaderHangKhachHang["MAHANGKH"] != null ? dataReaderHangKhachHang["MAHANGKH"].ToString() : "";
                                            if (_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TONGTIEN_KHACHHANG + TONGTIEN >= SOTIEN_LENHANG)
                                            {
                                                TANG_HANG_KHACHHANG(MAKHACHHANG, _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG_MOI, MAHANGKH);
                                            }
                                        }
                                    }
                                };
                            }
                            catch (Exception ex)
                            {
                                WriteLogs.LogError(ex);
                            }
                            finally
                            {
                                UPDATE_KHACHHANG_TO_SQLSERVER(MAKHACHHANG, QUYDOI_TIEN_THANH_DIEM, TONGTIEN, HANG_KHACHHANG);
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
        }
        private void UPDATE_KHACHHANG_TO_SQLSERVER(string MAKHACHHANG, decimal QUYDOI_TIEN_THANH_DIEM, decimal TONGTIEN, string HANG_KHACHHANG)
        {
            if (!string.IsNullOrEmpty(MAKHACHHANG) && TONGTIEN > 0)
            {
                //trừ điểm hiện tại trước
                decimal SODIEM_MOI = 0;
                SODIEM_MOI = _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.SODIEM - _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.DIEMQUYDOI;
                decimal DIEM_TICH_LUY = 0;
                if (QUYDOI_TIEN_THANH_DIEM != 0) DIEM_TICH_LUY = SODIEM_MOI + decimal.Round(TONGTIEN / QUYDOI_TIEN_THANH_DIEM, 2);
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                string queryUpdate = "";
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = connection;
                                if (string.IsNullOrEmpty(HANG_KHACHHANG) || HANG_KHACHHANG.Equals(""))
                                {
                                    queryUpdate = string.Format(@"UPDATE dbo.DM_KHACHHANG SET HANGKHACHHANG = '" + _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG_MOI + "' ,SODIEM = ISNULL(@SODIEM, 0), TONGTIEN = ISNULL(TONGTIEN, 0) + " + TONGTIEN + " WHERE MAKH = @MAKH");
                                }
                                else
                                {
                                    queryUpdate = string.Format(@"UPDATE dbo.DM_KHACHHANG SET SODIEM = ISNULL(@SODIEM, 0), TONGTIEN = ISNULL(TONGTIEN, 0) + @TONGTIEN WHERE MAKH = @MAKH");
                                }
                                cmd.CommandText = queryUpdate;
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.Add("MAKH", SqlDbType.NVarChar, 50).Value = MAKHACHHANG;
                                cmd.Parameters.Add("SODIEM", SqlDbType.Decimal).Value = DIEM_TICH_LUY;
                                cmd.Parameters.Add("TONGTIEN", SqlDbType.Decimal).Value = TONGTIEN;
                                int count = cmd.ExecuteNonQuery();
                                if (count > 0)
                                {
                                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.SODIEM = DIEM_TICH_LUY;
                                };
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
        }


        private void TANG_HANG_KHACHHANG(string MAKHACHHANG, string HANG_HIENTAI, string HANG_MOI)
        {
            if (!string.IsNullOrEmpty(MAKHACHHANG) && !string.IsNullOrEmpty(HANG_HIENTAI))
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
                                string queryUpdate = "";
                                if (!string.IsNullOrEmpty(HANG_MOI))
                                {
                                    OracleCommand cmdUpdateHangKhachHang = new OracleCommand();
                                    cmdUpdateHangKhachHang.Connection = connection;
                                    queryUpdate = string.Format(@"UPDATE DM_KHACHHANG SET HANGKHACHHANG = :HANGKHACHHANG_NEW, HANGKHACHHANGCU = :HANG_HIENTAI WHERE MAKH = :MAKHACHHANG AND UNITCODE = :UNITCODE");
                                    cmdUpdateHangKhachHang.CommandType = CommandType.Text;
                                    cmdUpdateHangKhachHang.Parameters.Add("HANGKHACHHANG_NEW", OracleDbType.NVarchar2, 50).Value = HANG_MOI;
                                    cmdUpdateHangKhachHang.Parameters.Add("HANG_HIENTAI", OracleDbType.NVarchar2, 50).Value = HANG_HIENTAI;
                                    cmdUpdateHangKhachHang.Parameters.Add("MAKHACHHANG", OracleDbType.NVarchar2, 50).Value = MAKHACHHANG;
                                    cmdUpdateHangKhachHang.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                    int count = cmdUpdateHangKhachHang.ExecuteNonQuery();
                                    if (count > 0)
                                    {
                                        NotificationLauncher.ShowNotification("Thông báo lên hạng", "Chúc mừng khách hàng đã được lên hạng mới", 1, "0x1", "0x8", "normal");
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
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                }
            }
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
                                    string.Format(@"INSERT INTO NVGDQUAY_ASYNCCLIENT (ID,MAGIAODICH,MAGIAODICHQUAYPK,MADONVI,LOAIGIAODICH,NGAYTAO,MANGUOITAO,
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
                                //chạy store trừ xuất nhập tồn        
                                if (!string.IsNullOrEmpty(Session.Session.CurrentTableNamePeriod))
                                {
                                    OracleCommand cmdTruTon = new OracleCommand();
                                    cmdTruTon.Connection = connection;
                                    cmdTruTon.CommandText = @"TBNETERP.XNT.XNT_GIAM_PHIEU";
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

                if (result)
                {
                    countSave = SAVE_DATA_TO_ORACLE(_NVGDQUAY_ASYNCCLIENT_DTO);
                }
                else
                {
                    countSave = SAVE_DATA_TO_SQL(_NVGDQUAY_ASYNCCLIENT_DTO);
                }
                if (countSave >= 2)
                {
                    NotificationLauncher.ShowNotification("Thông báo", "Hoàn thành giao dịch", 1, "0x1", "0x8", "normal");
                }
            }
            catch (Exception ex)
            {
                WriteLogs.LogError(ex);
            }
            ////UPDATE KHÁCH HÀNG
            if (!string.IsNullOrEmpty(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.MAKHACHHANG))
            {
                try
                {
                    string msg = Config.CheckConnectToServer(out bool result);
                    if (msg.Length > 0) { MessageBox.Show(msg); return; }

                    if (result)
                    {
                        UPDATE_KHACHHANG_TO_ORACLE(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.MAKHACHHANG, _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOITIEN_THANH_DIEM, _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT, _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG);
                    }
                    else
                    {
                        UPDATE_KHACHHANG_TO_SQLSERVER(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.MAKHACHHANG, _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOITIEN_THANH_DIEM, _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT, _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG);
                    }
                }
                catch { }
            }
        }
        private HANGKHACHHANG_DTO TINHTOAN_DULIEU_HANGKHACHHANG(string MaHangKhachHang)
        {
            HANGKHACHHANG_DTO _HANGKHACHHANG_DTO = new HANGKHACHHANG_DTO();
            try
            {
                if (string.IsNullOrEmpty(MaHangKhachHang) || MaHangKhachHang.Equals(""))
                {
                    string msg = Config.CheckConnectToServer(out bool result);
                    if (msg.Length > 0) { MessageBox.Show(msg); return _HANGKHACHHANG_DTO; }

                    if (result)
                    {
                        _HANGKHACHHANG_DTO = FrmThanhToanService.LAY_QUYDOI_TIEN_THANH_DIEM_HANGKHACHHANG_KHOIDAU_FROM_ORACLE();
                    }
                    else
                    {
                        _HANGKHACHHANG_DTO = FrmThanhToanService.LAY_QUYDOI_TIEN_THANH_DIEM_HANGKHACHHANG_KHOIDAU_FROM_SQLSERVER();
                    }
                }
                else
                {
                    string msg = Config.CheckConnectToServer(out bool result);
                    if (msg.Length > 0) { MessageBox.Show(msg); return _HANGKHACHHANG_DTO; }

                    if (result)
                    {
                        _HANGKHACHHANG_DTO = FrmThanhToanService.LAY_QUYDOI_THEOHANGKH_FROM_ORACLE(MaHangKhachHang.Trim());
                    }
                    else
                    {
                        _HANGKHACHHANG_DTO = FrmThanhToanService.LAY_QUYDOI_THEOHANGKH_FROM_SQLSERVER(MaHangKhachHang.Trim());
                    }
                }
            }
            catch
            {
            }
            return _HANGKHACHHANG_DTO;
        }
        #region Tính toán trừ trực tiếp tiền trên hóa đơn theo hạng khách hàng - Tạm thời comment phòng sau này sử dụng
        //private decimal TINHTOAN_DULIEU_CHIETKHAU_HANGKHACHHANG(string MaKhachHang)
        //{
        //    decimal TTIENCOVAT_BANDAU = GLOBAL_TONGTIEN_HOADON_BANDAU;
        //    KHACHHANG_GIAMGIA_DTO _KHACHHANG_GIAMGIA_DTO = new KHACHHANG_GIAMGIA_DTO();
        //    _KHACHHANG_GIAMGIA_DTO = LAY_HANG_KHACHHANG_FROM_DMKHACHHANG(MaKhachHang);
        //    if (_KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA >= 0)
        //    {
        //        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENTHE = TTIENCOVAT_BANDAU * _KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA / 100;
        //        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TTIENCOVAT = TTIENCOVAT_BANDAU * (1 - (_KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA / 100));
        //        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENMAT = _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TTIENCOVAT;
        //        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.MAKHACHHANG = MaKhachHang;
        //        //KHỞI TẠO DỮ LIỆU BILL
        //        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENTHE = _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENTHE;
        //        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TTIENCOVAT = _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TTIENCOVAT;
        //        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENMAT = _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TTIENCOVAT;
        //        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG = MaKhachHang;
        //        //
        //        txtThanhToan_TienThanhToan.Text = FormatCurrency.FormatMoney(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TTIENCOVAT);
        //        txtTienThanhToanCoChietKhau.Visible = true;
        //        txtTienThanhToanCoChietKhau.Enabled = true;
        //        txtTienThanhToanCoChietKhau.Text = FormatCurrency.FormatMoney(TTIENCOVAT_BANDAU) + " CHIẾT KHẤU:" + _KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA + "%" + " CÒN:" + FormatCurrency.FormatMoney(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TTIENCOVAT);
        //        if (!string.IsNullOrEmpty(txtThanhToan_TienMat.Text) && !string.IsNullOrEmpty(txtThanhToan_TienTraLai.Text))
        //        {
        //            decimal TTIENCOVAT = 0;
        //            decimal.TryParse(txtThanhToan_TienThanhToan.Text, out TTIENCOVAT);
        //            txtThanhToan_TienMat.Text = FormatCurrency.FormatMoney(TTIENCOVAT);
        //            txtThanhToan_TienTraLai.Text = "0";

        //        }
        //        if (_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.LST_DETAILS.Count > 0)
        //        {
        //            foreach (NVHANGGDQUAY_ASYNCCLIENT _NVHANGGDQUAY_ASYNCCLIENT in _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.LST_DETAILS)
        //            {
        //                decimal TTIENCOVAT_HANG = _NVGDQUAY_ASYNCCLIENT_DTO_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAVATTU == _NVHANGGDQUAY_ASYNCCLIENT.MAVATTU && x.MABOPK == _NVHANGGDQUAY_ASYNCCLIENT.MABOPK).TTIENCOVAT;
        //                decimal TIENCHIETKHAU_HANG = _NVGDQUAY_ASYNCCLIENT_DTO_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAVATTU == _NVHANGGDQUAY_ASYNCCLIENT.MAVATTU && x.MABOPK == _NVHANGGDQUAY_ASYNCCLIENT.MABOPK).TIENCHIETKHAU;
        //                decimal TYLECHIETKHAU_HANG = _NVGDQUAY_ASYNCCLIENT_DTO_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAVATTU == _NVHANGGDQUAY_ASYNCCLIENT.MAVATTU && x.MABOPK == _NVHANGGDQUAY_ASYNCCLIENT.MABOPK).TYLECHIETKHAU;
        //                _NVHANGGDQUAY_ASYNCCLIENT.LOAIKHUYENMAI = "THE";
        //                _NVHANGGDQUAY_ASYNCCLIENT.MACHUONGTRINHKM = "THE";
        //                decimal TIENCHIETKHAU_THE = TTIENCOVAT_HANG * (_KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA / 100);
        //                _NVHANGGDQUAY_ASYNCCLIENT.TIENCHIETKHAU = TIENCHIETKHAU_HANG + TIENCHIETKHAU_THE;
        //                _NVHANGGDQUAY_ASYNCCLIENT.TYLECHIETKHAU = TYLECHIETKHAU_HANG + _KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA;
        //                _NVHANGGDQUAY_ASYNCCLIENT.TTIENCOVAT = TTIENCOVAT_HANG * (1 - (_KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA / 100));
        //                _NVHANGGDQUAY_ASYNCCLIENT.MAKHACHHANG = MaKhachHang;
        //            }
        //        }
        //        else
        //        {
        //            string NOTIFICATION = string.Format(@"XẢY RA LỖI ! XIN THỰC HIỆN LẠI GIAO DỊCH NÀY");
        //            MessageBox.Show(NOTIFICATION);
        //        }
        //        if (_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS.Count > 0)
        //        {
        //            foreach (NVHANGGDQUAY_ASYNCCLIENT _NVHANGGDQUAY_ASYNCCLIENT in _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS)
        //            {
        //                decimal TTIENCOVAT_HANG = _NVGDQUAY_ASYNCCLIENT_BILL_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAVATTU == _NVHANGGDQUAY_ASYNCCLIENT.MAVATTU && x.MABOPK == _NVHANGGDQUAY_ASYNCCLIENT.MABOPK).TTIENCOVAT;
        //                decimal TIENCHIETKHAU_HANG = _NVGDQUAY_ASYNCCLIENT_BILL_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAVATTU == _NVHANGGDQUAY_ASYNCCLIENT.MAVATTU && x.MABOPK == _NVHANGGDQUAY_ASYNCCLIENT.MABOPK).TIENCHIETKHAU;
        //                decimal TYLECHIETKHAU_HANG = _NVGDQUAY_ASYNCCLIENT_BILL_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAVATTU == _NVHANGGDQUAY_ASYNCCLIENT.MAVATTU && x.MABOPK == _NVHANGGDQUAY_ASYNCCLIENT.MABOPK).TYLECHIETKHAU;
        //                _NVHANGGDQUAY_ASYNCCLIENT.LOAIKHUYENMAI = "THE";
        //                _NVHANGGDQUAY_ASYNCCLIENT.MACHUONGTRINHKM = "THE";
        //                decimal TIENCHIETKHAU_THE = TTIENCOVAT_HANG * (_KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA / 100);
        //                _NVHANGGDQUAY_ASYNCCLIENT.TIENCHIETKHAU = TIENCHIETKHAU_HANG + TIENCHIETKHAU_THE;
        //                _NVHANGGDQUAY_ASYNCCLIENT.TYLECHIETKHAU = TYLECHIETKHAU_HANG + _KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA;
        //                _NVHANGGDQUAY_ASYNCCLIENT.TTIENCOVAT = TTIENCOVAT_HANG * (1 - (_KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA / 100));
        //                _NVHANGGDQUAY_ASYNCCLIENT.MAKHACHHANG = MaKhachHang;
        //            }
        //        }
        //        //LOG_THANHTOAN
        //        LOG_THANHTOAN = true;
        //    }
        //    else
        //    {
        //        string NOTIFICATION = string.Format(@"KHÁCH HÀNG '{0}' CHƯA ĐỦ ĐIỀU KIỆN CHIẾT KHẤU", MaKhachHang);
        //        MessageBox.Show(NOTIFICATION);
        //        //LOG_THANHTOAN
        //        LOG_THANHTOAN = false;
        //    }
        //    return _KHACHHANG_GIAMGIA_DTO.TYLEGIAMGIA;
        //}
        #endregion

        private void THANHTOAN_HOADON_BANLE()
        {
            if (LOG_THANHTOAN)
            {
                LOG_THANHTOAN = false;
                txtThanhToan_TienMat.Focus(); txtThanhToan_TienMat.SelectAll();
                return;
            }
            else
            {
                bool success = false;
                try
                {
                    if (!string.IsNullOrEmpty(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.MAKHACHHANG) && _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENTHE > 0)
                    {
                        decimal TIEN_CHIADEU_MATHANG = 0;
                        if (_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.LST_DETAILS.Count > 0)
                            TIEN_CHIADEU_MATHANG = decimal.Round(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENTHE / _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.LST_DETAILS.Count, 2);
                        if (_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.LST_DETAILS.Count > 0)
                        {
                            foreach (NVHANGGDQUAY_ASYNCCLIENT row in _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.LST_DETAILS)
                            {
                                NVHANGGDQUAY_ASYNCCLIENT rowClone = _NVGDQUAY_ASYNCCLIENT_DTO_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAVATTU == row.MAVATTU && x.MABOPK == row.MABOPK);
                                if (rowClone != null)
                                {
                                    decimal TTIENCOVAT_HANG = rowClone.TTIENCOVAT;
                                    if (row.SOLUONG > 0)
                                    {
                                        row.TIENVOUCHER = decimal.Round(TIEN_CHIADEU_MATHANG, 2);
                                    }
                                    else
                                    {
                                        MessageBox.Show("CẢNH BÁO ! MÃ '" + rowClone.MAVATTU + "', CÓ SỐ LƯỢNG = '0' ! XIN KIỂM TRA LẠI HOẶC GIỮ LẠI HÓA ĐƠN ĐỂ KIỂM TRA");
                                    }
                                    row.TTIENCOVAT = TTIENCOVAT_HANG - row.TIENVOUCHER;
                                }
                            }
                        }
                        if (_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS.Count > 0)
                        {
                            decimal TIEN_CHIADEU_MATHANG_BILL = 0;
                            if (_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENTHE > 0)
                            {
                                TIEN_CHIADEU_MATHANG_BILL = decimal.Round(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENTHE / _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS.Count, 2);
                            }
                            foreach (NVHANGGDQUAY_ASYNCCLIENT rowBill in _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS)
                            {
                                NVHANGGDQUAY_ASYNCCLIENT rowBillClone = _NVGDQUAY_ASYNCCLIENT_BILL_CLONE.LST_DETAILS.FirstOrDefault(x => x.MAVATTU == rowBill.MAVATTU && x.MABOPK == rowBill.MABOPK);
                                if (rowBillClone != null)
                                {
                                    decimal TTIENCOVAT_HANG = rowBillClone.TTIENCOVAT;
                                    if (rowBill.SOLUONG > 0)
                                    {
                                        rowBill.TIENVOUCHER = decimal.Round(TIEN_CHIADEU_MATHANG_BILL, 2);
                                    }
                                    else
                                    {
                                        MessageBox.Show("CẢNH BÁO ! MÃ '" + rowBillClone.MAVATTU + "', CÓ SỐ LƯỢNG = '0' ! XIN KIỂM TRA LẠI HOẶC GIỮ LẠI HÓA ĐƠN ĐỂ KIỂM TRA");
                                    }
                                    rowBill.TTIENCOVAT = TTIENCOVAT_HANG - rowBill.TIENVOUCHER;
                                }
                            }
                        }
                    }
                    LUU_DULIEU(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL);
                }
                catch
                {
                    MessageBox.Show("CẢNH BÁO ! XẢY RA LỖI KHI LƯU HÓA ĐƠN NÀY, HÃY LƯU LẠI HÓA ĐƠN ĐỂ KIỂM TRA ! XIN CẢM ƠN ");
                }
                try
                {
                    THUCHIEN_IN_HOADON();
                }
                catch (Exception ex)
                {
                    WriteLogs.LogError(ex);
                }
            }
        }
        private void THUCHIEN_IN_HOADON()
        {
            string TONGTIEN_BANGCHU = ConvertSoThanhChu.ChuyenDoiSoThanhChu(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TTIENCOVAT);
            string msg = Config.CheckConnectToServer(out bool result);
            if (msg.Length > 0) { MessageBox.Show(msg); return; }

            string MA_TEN_KHACHHANG;
            if (result)
            {
                MA_TEN_KHACHHANG = FrmThanhToanService.LAY_MA_TEN_KHACHHANG_FROM_ORACLE(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG);
            }
            else
            {
                MA_TEN_KHACHHANG = FrmThanhToanService.LAY_MA_TEN_KHACHHANG_FROM_SQLSERVER(_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG);
            }
            using (frmPrintBill frm = new frmPrintBill())
            {
                try
                {
                    BILL_DTO infoBill = new BILL_DTO()
                    {
                        ADDRESS = Session.Session.CurrentAddress,
                        CONLAI = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENTRALAI,
                        PHONE = Session.Session.CurrentPhone,
                        MAKH = MA_TEN_KHACHHANG,
                        DIEM = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.SODIEM,
                        INFOTHUNGAN = "THU NGÂN: " + _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.NGUOITAO + "\t QUẦY: " + _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAQUAYBAN,
                        MAGIAODICH = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAGIAODICH,
                        THANHTIENCHU = TONGTIEN_BANGCHU,
                        TIENKHACHTRA = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENKHACHDUA,
                        QUAYHANG = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAQUAYBAN,
                    };
                    frm.PrintInvoice_BanLe(infoBill, _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL);
                }
                catch
                {
                }
                finally
                {
                    this.handler(true);
                    this.Dispose();
                    frm.Dispose();
                    frm.Refresh();
                }
            }
        }
        private const int WM_KEYDOWN = 256;
        private const int WM_KEYDOWN_2 = 260;
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                if ((Keys)m.WParam == Keys.Escape)
                {
                    this.Close();
                    this.Dispose();
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL = new NVGDQUAY_ASYNCCLIENT_DTO();
                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = new NVGDQUAY_ASYNCCLIENT_DTO();
                    GLOBAL_TONGTIEN_HOADON_BANDAU = new decimal();
                    LOG_THANHTOAN = false;
                }
                if ((Keys)m.WParam == Keys.F9)
                {
                    txtThanhToan_MaKhachHang.Focus();
                    txtThanhToan_MaKhachHang.ReadOnly = false;
                    txtThanhToan_MaKhachHang.Focus();
                    txtThanhToan_MaKhachHang.SelectAll();
                    txtThanhToan_TienQuyDoiDiem.Enabled = false;
                }
                if ((Keys)m.WParam == Keys.F8)
                {
                    FrmDmKhachHang frmDmKhachHang = new FrmDmKhachHang();
                    frmDmKhachHang.SET_HANDLER_STATUS_THEMMOI_KHACHHANG(BINDING_DATA_KHACHHANG_TO_THANHTOAN);
                    frmDmKhachHang.ShowDialog();
                }
            }
            if (m.Msg == WM_KEYDOWN_2)
            {
                if ((Keys)m.WParam == Keys.F10)
                {
                    if (btnF10_QuyDoi.Visible && btnF10_QuyDoi.Enabled)
                    {
                        txtThanhToan_TienQuyDoiDiem.Text = "";
                        txtThanhToan_TienQuyDoiDiem.Enabled = true;
                        txtThanhToan_TienQuyDoiDiem.Focus();
                        txtThanhToan_TienQuyDoiDiem.Focus();
                        txtThanhToan_TienQuyDoiDiem.SelectAll();
                    }
                }
            }
            return base.ProcessKeyPreview(ref m);
        }
        private void FrmThanhToan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL = new NVGDQUAY_ASYNCCLIENT_DTO();
                _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL = new NVGDQUAY_ASYNCCLIENT_DTO();
                GLOBAL_TONGTIEN_HOADON_BANDAU = new decimal();
                LOG_THANHTOAN = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                decimal tienThanhToan = 0;
                decimal.TryParse(txtThanhToan_TienTraLai.Text.ToString(), out tienThanhToan);
                if (tienThanhToan >= 0)
                {
                    THANHTOAN_HOADON_BANLE();
                }
                else
                {
                    NotificationLauncher.ShowNotificationError("Thông báo", "Sai tiền !", 1, "0x1", "0x8", "normal");
                }
            }

            if (e.KeyCode == Keys.F9)
            {
                txtThanhToan_MaKhachHang.ReadOnly = false;
                txtThanhToan_MaKhachHang.Focus();
                txtThanhToan_TienQuyDoiDiem.Enabled = false;
            }
        }
        private void btnThanhToan_Save_Click(object sender, EventArgs e)
        {
            decimal tienThanhToan = 0;
            decimal.TryParse(txtThanhToan_TienTraLai.Text.ToString(), out tienThanhToan);
            if (tienThanhToan >= 0)
            {
                THANHTOAN_HOADON_BANLE();
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
                decimal.TryParse(txtThanhToan_TienPhaiTra.Text.Trim(), out TONGTIEN_HOADON);
                TIEN_TRALAI = TIENMAT_KHACHTRA - TONGTIEN_HOADON;
                if (TIEN_TRALAI >= 0 && TIEN_TRALAI < 1000000)
                {
                    txtThanhToan_TienTraLai.Text = FormatCurrency.FormatMoney(TIEN_TRALAI);
                    txtThanhToan_TraLai_BangChu.Text = ConvertSoThanhChu.ChuyenDoiSoThanhChu(TIEN_TRALAI);
                    btnThanhToan_Save.Focus();
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENMAT = TIENMAT_KHACHTRA;
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENTRALAI = TIEN_TRALAI;
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENKHACHDUA = TIENMAT_KHACHTRA;

                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENMAT = TIENMAT_KHACHTRA;
                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENTRALAI = TIEN_TRALAI;
                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENKHACHDUA = TIENMAT_KHACHTRA;

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

                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENMAT = TIENMAT_KHACHTRA;
                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENTRALAI = TIEN_TRALAI;
                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENKHACHDUA = TIENMAT_KHACHTRA;
                    }
                }
            }
        }
        private void txtThanhToan_TienMat_KeyUp(object sender, KeyEventArgs e)
        {
            decimal TONGTIEN_KHACHTRA = 0; if (e.KeyCode != Keys.Enter)
            {
                if (!txtThanhToan_TienMat.Text.Equals(txtThanhToan_TienThanhToan.Text))
                {
                    string THANHTOAN_TIENMAT_STRING = txtThanhToan_TienMat.Text;
                    int start = txtThanhToan_TienMat.Text.Length - txtThanhToan_TienMat.SelectionStart;
                    THANHTOAN_TIENMAT_STRING = THANHTOAN_TIENMAT_STRING.Replace(",", "");
                    decimal THANHTOAN_TIENMAT = 0;
                    decimal.TryParse(THANHTOAN_TIENMAT_STRING, out THANHTOAN_TIENMAT);
                    TONGTIEN_KHACHTRA = THANHTOAN_TIENMAT;
                    decimal THANHTOAN_TONGTIEN = 0;
                    decimal.TryParse(txtThanhToan_TienPhaiTra.Text.Trim().Replace(",", ""), out THANHTOAN_TONGTIEN);
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENMAT = THANHTOAN_TIENMAT;
                    _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENKHACHDUA = THANHTOAN_TIENMAT;
                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENMAT = THANHTOAN_TIENMAT;
                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENKHACHDUA = THANHTOAN_TIENMAT;
                    decimal THANHTOAN_TIENTRALAI = THANHTOAN_TIENMAT - THANHTOAN_TONGTIEN;

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
                    _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENTRALAI = THANHTOAN_TIENTRALAI;
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
                    THANHTOAN_HOADON_BANLE();
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
                                command.Parameters.Add("TENDAYDU", OracleDbType.NVarchar2, 300).Value = ITEM.TENDAYDU != null ? ITEM.TENDAYDU : "";
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
                                command.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = ITEM.UNITCODE;
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
                        NotificationLauncher.ShowNotificationError("THÔNG BÁO", "KHÔNG CÓ KẾT NỐI VỚI CƠ SỞ DỮ LIỆU SQL", 1, "0x1", "0x8", "normal");
                    }
                }
            }
        }
        public void BINDING_DATA_CHANGE_FROM_GRID_TO_TEXT(KHACHHANG_DTO _KHACHHANG_DTO)
        {
            txtThanhToan_MaKhachHang.ReadOnly = false;
            txtThanhToan_MaKhachHang.Text = _KHACHHANG_DTO.MAKH;

            txtThanhToan_TenKhachHang.Visible = true;
            txtThanhToan_TenKhachHang.Text = "TÊN: " + _KHACHHANG_DTO.TENKH;

            txtThanhToan_SoDienThoai.Visible = true;
            txtThanhToan_SoDienThoai.Text = "SĐT: " + _KHACHHANG_DTO.DIENTHOAI;

            txtThanhToan_DiemTichLuy.Visible = true;
            txtThanhToan_DiemTichLuy.Text = "ĐIỂM: " + _KHACHHANG_DTO.SODIEM;

            txtThanhToan_DiaChi.Visible = true;
            txtThanhToan_DiaChi.Text = "Đ/C: " + _KHACHHANG_DTO.DIACHI;

            txtThanhToan_QuyDoi_ToiDa.Visible = true;
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.MAKHACHHANG = _KHACHHANG_DTO.MAKH;
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.SODIEM = _KHACHHANG_DTO.SODIEM;
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TONGTIEN_KHACHHANG = _KHACHHANG_DTO.TONGTIEN;
            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG = _KHACHHANG_DTO.MAKH;
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG = _KHACHHANG_DTO.HANGKHACHHANG;
            HANGKHACHHANG_DTO _HANGKHACHHANG_DTO = new HANGKHACHHANG_DTO();
            _HANGKHACHHANG_DTO = TINHTOAN_DULIEU_HANGKHACHHANG(_KHACHHANG_DTO.HANGKHACHHANG);
            if (!string.IsNullOrEmpty(_HANGKHACHHANG_DTO.MAHANGKH))
            {
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOITIEN_THANH_DIEM = _HANGKHACHHANG_DTO.QUYDOITIEN_THANH_DIEM;
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOIDIEM_THANH_TIEN = _HANGKHACHHANG_DTO.QUYDOIDIEM_THANH_TIEN;
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG_MOI = _HANGKHACHHANG_DTO.MAHANGKH;
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.SOTIEN_LENHANG = _HANGKHACHHANG_DTO.SOTIEN;
                decimal QUYDOI = decimal.Round(_KHACHHANG_DTO.SODIEM * _HANGKHACHHANG_DTO.QUYDOIDIEM_THANH_TIEN, 2);
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOI_TOIDA = QUYDOI;
                txtThanhToan_QuyDoi_ToiDa.Text = "QUY ĐỔI TỐI ĐA : " + FormatCurrency.FormatMoney(QUYDOI) + " VNĐ";
                if (QUYDOI > 0)
                {
                    btnF10_QuyDoi.Visible = true;
                    btnF10_QuyDoi.Enabled = true;
                }
            }
            else
            {
                txtThanhToan_QuyDoi_ToiDa.Text = "QUY ĐỔI TỐI ĐA : 0 VNĐ";
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG = _HANGKHACHHANG_DTO.MAHANGKH;
                btnF10_QuyDoi.Visible = false;
                btnF10_QuyDoi.Enabled = false;
            }
            txtThanhToan_TienMat.Focus();
            txtThanhToan_TienMat.SelectAll();
            LOG_THANHTOAN = true;
        }

        public void BINDING_DATA_KHACHHANG_TO_THANHTOAN(KHACHHANG_DTO _KHACHHANG_DTO)
        {
            txtThanhToan_MaKhachHang.ReadOnly = false;
            txtThanhToan_MaKhachHang.Text = _KHACHHANG_DTO.MAKH;

            txtThanhToan_TenKhachHang.Visible = true;
            txtThanhToan_TenKhachHang.Text = "TÊN: " + _KHACHHANG_DTO.TENKH;

            txtThanhToan_SoDienThoai.Visible = true;
            txtThanhToan_SoDienThoai.Text = "SĐT: " + _KHACHHANG_DTO.DIENTHOAI;

            txtThanhToan_DiemTichLuy.Visible = true;
            txtThanhToan_DiemTichLuy.Text = "ĐIỂM: " + _KHACHHANG_DTO.SODIEM;

            txtThanhToan_DiaChi.Visible = true;
            txtThanhToan_DiaChi.Text = "Đ/C: " + _KHACHHANG_DTO.DIACHI;

            txtThanhToan_QuyDoi_ToiDa.Visible = true;
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.MAKHACHHANG = _KHACHHANG_DTO.MAKH;
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.SODIEM = _KHACHHANG_DTO.SODIEM;
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TONGTIEN_KHACHHANG = _KHACHHANG_DTO.TONGTIEN;
            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG = _KHACHHANG_DTO.MAKH;
            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG = _KHACHHANG_DTO.HANGKHACHHANG;
            HANGKHACHHANG_DTO _HANGKHACHHANG_DTO = new HANGKHACHHANG_DTO();
            _HANGKHACHHANG_DTO = TINHTOAN_DULIEU_HANGKHACHHANG(_KHACHHANG_DTO.HANGKHACHHANG);
            if (!string.IsNullOrEmpty(_HANGKHACHHANG_DTO.MAHANGKH))
            {
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOITIEN_THANH_DIEM = _HANGKHACHHANG_DTO.QUYDOITIEN_THANH_DIEM;
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOIDIEM_THANH_TIEN = _HANGKHACHHANG_DTO.QUYDOIDIEM_THANH_TIEN;
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG_MOI = _HANGKHACHHANG_DTO.MAHANGKH;
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.SOTIEN_LENHANG = _HANGKHACHHANG_DTO.SOTIEN;
                decimal QUYDOI = decimal.Round(_KHACHHANG_DTO.SODIEM * _HANGKHACHHANG_DTO.QUYDOIDIEM_THANH_TIEN, 2);
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOI_TOIDA = QUYDOI;
                txtThanhToan_QuyDoi_ToiDa.Text = "QUY ĐỔI TỐI ĐA : " + FormatCurrency.FormatMoney(QUYDOI) + " VNĐ";
                if (QUYDOI > 0)
                {
                    btnF10_QuyDoi.Visible = true;
                    btnF10_QuyDoi.Enabled = true;
                }
            }
            else
            {
                txtThanhToan_QuyDoi_ToiDa.Text = "QUY ĐỔI TỐI ĐA : 0 VNĐ";
                _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG = _HANGKHACHHANG_DTO.MAHANGKH;
                btnF10_QuyDoi.Visible = false;
                btnF10_QuyDoi.Enabled = false;
            }
            txtThanhToan_TienMat.Focus();
            txtThanhToan_TienMat.SelectAll();
            LOG_THANHTOAN = true;
        }
        private void txtThanhToan_MaKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtThanhToan_MaKhachHang.Text))
                {
                    string MaKhachHang = txtThanhToan_MaKhachHang.Text.Trim();
                    List<KHACHHANG_DTO> _LST_KHACHHANG_DTO = FrmThanhToanService.TIMKIEM_KHACHHANG_FROM_ORACLE(MaKhachHang, 1, 0, Session.Session.CurrentUnitCode);
                    if (_LST_KHACHHANG_DTO.Count > 1)
                    {
                        FrmTimKiemKhachHang frmTimKiemKhachHang = new FrmTimKiemKhachHang(MaKhachHang);
                        frmTimKiemKhachHang.SetHanlerTimKiemKhachHang(BINDING_DATA_CHANGE_FROM_GRID_TO_TEXT);
                        frmTimKiemKhachHang.ShowDialog();
                        txtThanhToan_TienMat.Focus();
                        txtThanhToan_TienMat.SelectAll();
                    }
                    else if (_LST_KHACHHANG_DTO.Count == 1)
                    {
                        txtThanhToan_MaKhachHang.ReadOnly = false;
                        txtThanhToan_MaKhachHang.Text = _LST_KHACHHANG_DTO[0].MAKH;

                        txtThanhToan_TenKhachHang.Visible = true;
                        txtThanhToan_TenKhachHang.Text = "TÊN: " + _LST_KHACHHANG_DTO[0].TENKH;

                        txtThanhToan_SoDienThoai.Visible = true;
                        txtThanhToan_SoDienThoai.Text = "SĐT: " + _LST_KHACHHANG_DTO[0].DIENTHOAI;

                        txtThanhToan_DiemTichLuy.Visible = true;
                        txtThanhToan_DiemTichLuy.Text = "ĐIỂM: " + _LST_KHACHHANG_DTO[0].SODIEM;

                        txtThanhToan_DiaChi.Visible = true;
                        txtThanhToan_DiaChi.Text = "Đ/C: " + _LST_KHACHHANG_DTO[0].DIACHI;

                        txtThanhToan_QuyDoi_ToiDa.Visible = true;

                        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.MAKHACHHANG = _LST_KHACHHANG_DTO[0].MAKH;
                        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.SODIEM = _LST_KHACHHANG_DTO[0].SODIEM;
                        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TONGTIEN_KHACHHANG = _LST_KHACHHANG_DTO[0].TONGTIEN;
                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.MAKHACHHANG = _LST_KHACHHANG_DTO[0].MAKH;
                        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG = _LST_KHACHHANG_DTO[0].HANGKHACHHANG;
                        HANGKHACHHANG_DTO _HANGKHACHHANG_DTO = new HANGKHACHHANG_DTO();
                        _HANGKHACHHANG_DTO = TINHTOAN_DULIEU_HANGKHACHHANG(_LST_KHACHHANG_DTO[0].HANGKHACHHANG);
                        if (!string.IsNullOrEmpty(_HANGKHACHHANG_DTO.MAHANGKH))
                        {
                            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOITIEN_THANH_DIEM = _HANGKHACHHANG_DTO.QUYDOITIEN_THANH_DIEM;
                            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOIDIEM_THANH_TIEN = _HANGKHACHHANG_DTO.QUYDOIDIEM_THANH_TIEN;
                            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG_MOI = _HANGKHACHHANG_DTO.MAHANGKH;
                            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.SOTIEN_LENHANG = _HANGKHACHHANG_DTO.SOTIEN;
                            decimal QUYDOI = decimal.Round(_LST_KHACHHANG_DTO[0].SODIEM * _HANGKHACHHANG_DTO.QUYDOIDIEM_THANH_TIEN, 2);
                            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOI_TOIDA = QUYDOI;
                            txtThanhToan_QuyDoi_ToiDa.Text = "QUY ĐỔI TỐI ĐA : " + FormatCurrency.FormatMoney(QUYDOI) + " VNĐ";
                            if (QUYDOI > 0)
                            {
                                btnF10_QuyDoi.Visible = true;
                                btnF10_QuyDoi.Enabled = true;
                            }
                        }
                        else
                        {
                            txtThanhToan_QuyDoi_ToiDa.Text = "QUY ĐỔI TỐI ĐA : 0 VNĐ";
                            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.HANGKHACHHANG = _HANGKHACHHANG_DTO.MAHANGKH;
                            btnF10_QuyDoi.Visible = false;
                            btnF10_QuyDoi.Enabled = false;
                        }
                        txtThanhToan_TienMat.Focus();
                        txtThanhToan_TienMat.SelectAll();
                    }
                    else
                    {
                        MessageBox.Show("THÔNG BÁO! KHÔNG TÌM THẤY THÔNG TIN KHÁCH HÀNG '" + txtThanhToan_MaKhachHang.Text + "' ");
                        FrmTimKiemKhachHang frmTimKiemKhachHang = new FrmTimKiemKhachHang(MaKhachHang);
                        frmTimKiemKhachHang.SetHanlerTimKiemKhachHang(BINDING_DATA_CHANGE_FROM_GRID_TO_TEXT);
                        frmTimKiemKhachHang.ShowDialog();
                        txtThanhToan_TienMat.Focus();
                        txtThanhToan_TienMat.SelectAll();
                    }
                }
            }
            LOG_THANHTOAN = true;
        }
        private void btnThanhToan_ThemKhachHang_Click(object sender, EventArgs e)
        {
            FrmDmKhachHang frmDmKhachHang = new FrmDmKhachHang();
            frmDmKhachHang.SET_HANDLER_STATUS_THEMMOI_KHACHHANG(BINDING_DATA_KHACHHANG_TO_THANHTOAN);
            frmDmKhachHang.ShowDialog();
        }

        private void btnF9KhachHang_Click(object sender, EventArgs e)
        {
            txtThanhToan_MaKhachHang.ReadOnly = false;
            txtThanhToan_MaKhachHang.Focus();
            txtThanhToan_TienQuyDoiDiem.Enabled = false;
        }

        private void btnF10_QuyDoi_Click(object sender, EventArgs e)
        {
            if (btnF10_QuyDoi.Visible && btnF10_QuyDoi.Enabled)
            {
                txtThanhToan_TienQuyDoiDiem.Text = "";
                txtThanhToan_TienQuyDoiDiem.Enabled = true;
                txtThanhToan_TienQuyDoiDiem.Focus();
                txtThanhToan_TienQuyDoiDiem.Focus();
                txtThanhToan_TienQuyDoiDiem.SelectAll();
            }
        }

        private void txtThanhToan_TienQuyDoiDiem_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                string THANHTOAN_TIENQUYDOI_STRING = txtThanhToan_TienQuyDoiDiem.Text;
                int start = txtThanhToan_TienQuyDoiDiem.Text.Length - txtThanhToan_TienQuyDoiDiem.SelectionStart;
                THANHTOAN_TIENQUYDOI_STRING = THANHTOAN_TIENQUYDOI_STRING.Replace(",", "");
                txtThanhToan_TienQuyDoiDiem.Text = FormatCurrency.FormatMoney(THANHTOAN_TIENQUYDOI_STRING);
                txtThanhToan_TienQuyDoiDiem.SelectionStart = -start + txtThanhToan_TienMat.Text.Length;
            }
            else
            {
                if (txtThanhToan_TienQuyDoiDiem.Text.Length > 0)
                {
                    decimal TIEN_QUYDOI = 0;
                    decimal.TryParse(txtThanhToan_TienQuyDoiDiem.Text != null ? txtThanhToan_TienQuyDoiDiem.Text : "", out TIEN_QUYDOI);
                    if (TIEN_QUYDOI > _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOI_TOIDA)
                    {
                        txtThanhToan_TienQuyDoiDiem.Text = FormatCurrency.FormatMoney(_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOI_TOIDA);
                    }
                    else
                    {
                        decimal TIEN_PHAITRA = GLOBAL_TONGTIEN_HOADON_BANDAU - TIEN_QUYDOI;
                        if (TIEN_PHAITRA != 0) txtThanhToan_TienPhaiTra.Text = FormatCurrency.FormatMoney(TIEN_PHAITRA);
                        txtThanhToan_TienMat.Text = FormatCurrency.FormatMoney(TIEN_PHAITRA);
                        string THANHTOAN_TIENMAT_STRING = txtThanhToan_TienMat.Text;
                        THANHTOAN_TIENMAT_STRING = THANHTOAN_TIENMAT_STRING.Replace(",", "");
                        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TTIENCOVAT = TIEN_PHAITRA;
                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TTIENCOVAT = TIEN_PHAITRA; _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENTHE = TIEN_QUYDOI;
                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENTHE = TIEN_QUYDOI;
                        decimal TIENMAT_KHACHTRA, TIEN_TRALAI = 0;

                        decimal.TryParse(THANHTOAN_TIENMAT_STRING, out TIENMAT_KHACHTRA);
                        TIEN_TRALAI = TIENMAT_KHACHTRA - TIEN_PHAITRA;
                        if (TIEN_TRALAI >= 0 && TIEN_TRALAI < 1000000)
                        {
                            txtThanhToan_TienTraLai.Text = FormatCurrency.FormatMoney(TIEN_TRALAI.ToString());
                            txtThanhToan_TraLai_BangChu.Text = ConvertSoThanhChu.ChuyenDoiSoThanhChu(TIEN_TRALAI);
                            btnThanhToan_Save.Focus();
                            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENMAT = TIENMAT_KHACHTRA;
                            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENTRALAI = TIEN_TRALAI;
                            _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.TIENKHACHDUA = TIENMAT_KHACHTRA;

                            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENMAT = TIENMAT_KHACHTRA;
                            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENTRALAI = TIEN_TRALAI;
                            _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.TIENKHACHDUA = TIENMAT_KHACHTRA;

                        }
                        else if (TIEN_TRALAI < 0)
                        {
                            txtThanhToan_TienQuyDoiDiem.SelectAll();
                        }
                        else
                        {
                            DialogResult result = MessageBox.Show("SỐ TIỀN TRẢ LẠI QUÁ LỚN ?", "THAO TÁC SAI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        decimal DIEMQUYDOI = 0; if (_NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOIDIEM_THANH_TIEN != 0)
                        {
                            DIEMQUYDOI = decimal.Round(TIEN_QUYDOI / _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.QUYDOIDIEM_THANH_TIEN, 2);
                        }
                        _NVGDQUAY_ASYNCCLIENT_DTO_GLOBAL.DIEMQUYDOI = DIEMQUYDOI;
                        _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.DIEMQUYDOI = DIEMQUYDOI;
                        txtThanhToan_TienMat.Focus();
                        txtThanhToan_TienMat.SelectAll();
                    }
                }
                else
                {
                    txtThanhToan_TienPhaiTra.Text = FormatCurrency.FormatMoney(GLOBAL_TONGTIEN_HOADON_BANDAU);
                }
            }
        }
    }
}

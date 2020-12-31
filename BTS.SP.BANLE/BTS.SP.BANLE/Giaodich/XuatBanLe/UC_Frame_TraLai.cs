using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using BTS.SP.BANLE.Common;
using BTS.SP.BANLE.Dto;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public delegate void XuLyThanhToanBanTraLai(bool statusBanTraLai);
    public delegate void Status_TimKiem(string MaGiaoDichQuay, DateTime TuNgay, DateTime DenNgay, int DieuKienLoc);
    public partial class UC_Frame_TraLai : UserControl
    {
        int currentRowDgvTraLai = -1;
        private int VALUE_SELECTED_CHANGE = 0;
        public UC_Frame_TraLai()
        {
            InitializeComponent();
            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã giao dịch" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Số tiền" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Thu ngân tạo" });
            cboDieuKienChon.DataSource = _comboItems;
            cboDieuKienChon.DisplayMember = "TEXT";
            cboDieuKienChon.ValueMember = "VALUE";
            //default combobox
            cboDieuKienChon.SelectedIndex = VALUE_SELECTED_CHANGE;
            dateTimeDenNgay.Format = DateTimePickerFormat.Custom;
            dateTimeDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeDenNgay.Value = DateTime.Now;
            dateTimeTuNgay.Format = DateTimePickerFormat.Custom;
            dateTimeTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeTuNgay.Value = DateTime.Now.AddDays(-7);
            txtTraLai_ThuNgan.Text = Session.Session.CurrentTenNhanVien;
            txtTraLai_MaVatTu.SelectAll();
            txtTraLai_SoLuong.SelectAll();
            txtKeySearch.Enabled = false;
            dateTimeTuNgay.Enabled = false;
            dateTimeDenNgay.Enabled = false;
            btnTimKiem.Enabled = false;
            btnF1ThanhToanTraLai.Enabled = false;
            btnF5LamMoiGiaoDich.Enabled = false;
            btnF3GiamHang.Enabled = false;
            btnF4TangHang.Enabled = false;
            btnF2ThemMoiTraLai.Enabled = true;
        }

        //TÌM KIẾM GIAO DỊCH TRƯỜNG HỢP CÓ MẠNG
        public static List<NVGDQUAY_ASYNCCLIENT_DTO> TIMKIEM_GIAODICHQUAY(string KeySearch, DateTime TuNgay, DateTime DenNgay, int DieuKienLoc)
        {
            NVGDQUAY_ASYNCCLIENT_DTO GDQUAY_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
            List<NVGDQUAY_ASYNCCLIENT_DTO> listReturn = new List<NVGDQUAY_ASYNCCLIENT_DTO>();
            try
            {
                using (
                    OracleConnection connection =
                        new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString)
                    )
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = connection;
                            cmd.InitialLONGFetchSize = 1000;
                            cmd.CommandText = "TIMKIEM_GIAODICHQUAY";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("P_KEYSEARCH", OracleDbType.Varchar2).Value = KeySearch;
                            cmd.Parameters.Add("P_TUNGAY", OracleDbType.Date).Value = TuNgay;
                            cmd.Parameters.Add("P_DENNGAY", OracleDbType.Date).Value = DenNgay;
                            cmd.Parameters.Add("P_DIEUKIENLOC", OracleDbType.Int32).Value = DieuKienLoc;
                            cmd.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2).Value = Session.Session.CurrentUnitCode;
                            cmd.Parameters.Add("CURSOR_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                            OracleDataReader dataReader = null;
                            dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {

                                while (dataReader.Read())
                                {
                                    int LoaiGD = 0;
                                    decimal TIENKHACHDUA, TIENVOUCHER, TIENTHEVIP, TIENTRALAI, TIENTHE, TIENCOD, TIENMAT, TTIENCOVAT = 0;
                                    int.TryParse(dataReader["LOAIGIAODICH"].ToString(), out LoaiGD);
                                    GDQUAY_DTO.MAGIAODICH = dataReader["MAGIAODICH"].ToString();
                                    GDQUAY_DTO.MAGIAODICHQUAYPK = dataReader["MAGIAODICHQUAYPK"].ToString();
                                    GDQUAY_DTO.LOAIGIAODICH = LoaiGD;
                                    GDQUAY_DTO.NGAYTAO = DateTime.Parse(dataReader["NGAYTAO"].ToString());
                                    GDQUAY_DTO.MANGUOITAO = dataReader["MANGUOITAO"].ToString();
                                    GDQUAY_DTO.NGUOITAO = dataReader["NGUOITAO"].ToString();
                                    GDQUAY_DTO.MAQUAYBAN = dataReader["MAQUAYBAN"].ToString();
                                    GDQUAY_DTO.NGAYPHATSINH = DateTime.Parse(dataReader["NGAYTAO"].ToString());
                                    GDQUAY_DTO.HINHTHUCTHANHTOAN = dataReader["HINHTHUCTHANHTOAN"].ToString();
                                    if (dataReader["MAVOUCHER"] != null)
                                    {
                                        GDQUAY_DTO.MAVOUCHER = dataReader["MAVOUCHER"].ToString();
                                    }
                                    else
                                    {
                                        GDQUAY_DTO.MAVOUCHER = ";";
                                    }
                                    decimal.TryParse(dataReader["TIENKHACHDUA"].ToString(), out TIENKHACHDUA);
                                    GDQUAY_DTO.TIENKHACHDUA = TIENKHACHDUA;
                                    decimal.TryParse(dataReader["TIENVOUCHER"].ToString(), out TIENVOUCHER);
                                    GDQUAY_DTO.TIENVOUCHER = TIENVOUCHER;
                                    decimal.TryParse(dataReader["TIENTHEVIP"].ToString(), out TIENTHEVIP);
                                    GDQUAY_DTO.TIENTHEVIP = TIENTHEVIP;
                                    decimal.TryParse(dataReader["TIENTRALAI"].ToString(), out TIENTRALAI);
                                    GDQUAY_DTO.TIENTRALAI = TIENTRALAI;
                                    decimal.TryParse(dataReader["TIENTHE"].ToString(), out TIENTHE);
                                    GDQUAY_DTO.TIENTHE = TIENTHE;
                                    decimal.TryParse(dataReader["TIENCOD"].ToString(), out TIENCOD);
                                    GDQUAY_DTO.TIENCOD = TIENCOD;
                                    decimal.TryParse(dataReader["TIENMAT"].ToString(), out TIENMAT);
                                    GDQUAY_DTO.TIENMAT = TIENMAT;
                                    decimal.TryParse(dataReader["TTIENCOVAT"].ToString(), out TTIENCOVAT);
                                    GDQUAY_DTO.TTIENCOVAT = TTIENCOVAT;
                                    GDQUAY_DTO.THOIGIAN = dataReader["THOIGIAN"].ToString();
                                    GDQUAY_DTO.MAKHACHHANG = dataReader["MAKHACHHANG"].ToString();
                                    GDQUAY_DTO.UNITCODE = dataReader["UNITCODE"].ToString();
                                    listReturn.Add(GDQUAY_DTO);
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("Thông báo", "KHÔNG TÌM THẤY MÃ GIAO DỊCH", 1, "0x1", "0x8", "normal");
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogs.LogError(ex);
                NotificationLauncher.ShowNotificationError("Thông báo", "Xảy ra lỗi", 1, "0x1", "0x8", "normal");
            }
            return listReturn;
        }
        //TÌM KIẾM GIAO DỊCH TRƯỜNG HỢP MẤT MẠNG
        public static List<NVGDQUAY_ASYNCCLIENT_DTO> TIMKIEM_GIAODICHQUAY_FROM_SQL(string KeySearch, DateTime TuNgay, DateTime DenNgay, int DieuKienLoc)
        {
            NVGDQUAY_ASYNCCLIENT_DTO GDQUAY_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
            List<NVGDQUAY_ASYNCCLIENT_DTO> listReturn = new List<NVGDQUAY_ASYNCCLIENT_DTO>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT [MAGIAODICH],[MAGIAODICHQUAYPK],[MADONVI],[NGAYTAO],[NGAYPHATSINH],[MANGUOITAO],[NGUOITAO],[MAQUAYBAN],[LOAIGIAODICH],[HINHTHUCTHANHTOAN],[TIENKHACHDUA],[TIENVOUCHER],[TIENTHEVIP],[TIENTRALAI],[TIENTHE],[TIENCOD],[TIENMAT],[TTIENCOVAT],[THOIGIAN],[MAKHACHHANG],[UNITCODE] FROM [dbo].[NVGDQUAY_ASYNCCLIENT]";
                        SqlDataReader dataReader = null;
                        dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {

                            while (dataReader.Read())
                            {
                                int LoaiGD = 0;
                                decimal TIENKHACHDUA, TIENVOUCHER, TIENTHEVIP, TIENTRALAI, TIENTHE, TIENCOD, TIENMAT, TTIENCOVAT = 0;
                                int.TryParse(dataReader["LOAIGIAODICH"].ToString(), out LoaiGD);
                                GDQUAY_DTO.MAGIAODICH = dataReader["MAGIAODICH"].ToString();
                                GDQUAY_DTO.MAGIAODICHQUAYPK = dataReader["MAGIAODICHQUAYPK"].ToString();
                                GDQUAY_DTO.LOAIGIAODICH = LoaiGD;
                                GDQUAY_DTO.NGAYTAO = DateTime.Parse(dataReader["NGAYTAO"].ToString());
                                GDQUAY_DTO.MANGUOITAO = dataReader["MANGUOITAO"].ToString();
                                GDQUAY_DTO.NGUOITAO = dataReader["NGUOITAO"].ToString();
                                GDQUAY_DTO.MAQUAYBAN = dataReader["MAQUAYBAN"].ToString();
                                GDQUAY_DTO.NGAYPHATSINH = DateTime.Parse(dataReader["NGAYTAO"].ToString());
                                GDQUAY_DTO.HINHTHUCTHANHTOAN = dataReader["HINHTHUCTHANHTOAN"].ToString();
                                decimal.TryParse(dataReader["TIENKHACHDUA"].ToString(), out TIENKHACHDUA);
                                GDQUAY_DTO.TIENKHACHDUA = TIENKHACHDUA;
                                decimal.TryParse(dataReader["TIENVOUCHER"].ToString(), out TIENVOUCHER);
                                GDQUAY_DTO.TIENVOUCHER = TIENVOUCHER;
                                decimal.TryParse(dataReader["TIENTHEVIP"].ToString(), out TIENTHEVIP);
                                GDQUAY_DTO.TIENTHEVIP = TIENTHEVIP;
                                decimal.TryParse(dataReader["TIENTRALAI"].ToString(), out TIENTRALAI);
                                GDQUAY_DTO.TIENTRALAI = TIENTRALAI;
                                decimal.TryParse(dataReader["TIENTHE"].ToString(), out TIENTHE);
                                GDQUAY_DTO.TIENTHE = TIENTHE;
                                decimal.TryParse(dataReader["TIENCOD"].ToString(), out TIENCOD);
                                GDQUAY_DTO.TIENCOD = TIENCOD;
                                decimal.TryParse(dataReader["TIENMAT"].ToString(), out TIENMAT);
                                GDQUAY_DTO.TIENMAT = TIENMAT;
                                decimal.TryParse(dataReader["TTIENCOVAT"].ToString(), out TTIENCOVAT);
                                GDQUAY_DTO.TTIENCOVAT = TTIENCOVAT;
                                GDQUAY_DTO.THOIGIAN = dataReader["THOIGIAN"].ToString();
                                GDQUAY_DTO.MAKHACHHANG = dataReader["MAKHACHHANG"].ToString();
                                GDQUAY_DTO.UNITCODE = dataReader["UNITCODE"].ToString();
                                listReturn.Add(GDQUAY_DTO);
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
            return listReturn;
        }
        public static IEnumerable<DateTime> EachDay(DateTime fromDate, DateTime toDate)
        {
            for (var day = fromDate.Date; day.Date <= toDate.Date; day = day.AddDays(1))
                yield return day;
        }

        public static NVGDQUAY_ASYNCCLIENT_DTO SEARCH_BY_CODE_PAY_FROM_SQLSERVER(string MaGiaoDich)
        {
            //Kết nối với SQL
            NVGDQUAY_ASYNCCLIENT_DTO GDQUAY_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
            decimal ChietKhau = 0;
            string MaGiaoDichQuayPk = MaGiaoDich.Trim() + "." + Session.Session.CurrentUnitCode.Split('-')[1];
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        string KieuThanhToan = null;
                        SqlCommand cmdParent = new SqlCommand();
                        cmdParent.Connection = connection;
                        cmdParent.CommandText = string.Format(@"SELECT [MAGIAODICH],[MAGIAODICHQUAYPK],[MADONVI],[NGAYTAO],[NGAYPHATSINH],[MANGUOITAO],[NGUOITAO],[MAQUAYBAN],[LOAIGIAODICH],[HINHTHUCTHANHTOAN],[TIENKHACHDUA],[TIENVOUCHER],[TIENTHEVIP],[TIENTRALAI],[TIENTHE],[TIENCOD],[TIENMAT],[TTIENCOVAT],[THOIGIAN],[MAKHACHHANG],[UNITCODE] FROM [dbo].[NVGDQUAY_ASYNCCLIENT] WHERE MAGIAODICHQUAYPK = @MAGIAODICHQUAYPK AND UNITCODE = @UNITCODE");
                        cmdParent.CommandType = CommandType.Text;
                        cmdParent.Parameters.Add("MAGIAODICHQUAYPK", SqlDbType.NVarChar, 50).Value = MaGiaoDichQuayPk.Trim();
                        cmdParent.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentUnitCode;
                        SqlDataReader dataReaderParent = null;
                        dataReaderParent = cmdParent.ExecuteReader();
                        if (dataReaderParent.HasRows)
                        {
                            int LoaiGD = 0;
                            decimal TIENKHACHDUA, TIENVOUCHER, TIENTHEVIP, TIENTRALAI, TIENTHE, TIENCOD, TIENMAT, TTIENCOVAT = 0;
                            while (dataReaderParent.Read())
                            {
                                int.TryParse(dataReaderParent["LOAIGIAODICH"].ToString(), out LoaiGD);
                                GDQUAY_DTO.LOAIGIAODICH = LoaiGD;
                                GDQUAY_DTO.MAGIAODICH = dataReaderParent["MAGIAODICH"].ToString().Trim();
                                GDQUAY_DTO.MAGIAODICHQUAYPK = dataReaderParent["MAGIAODICHQUAYPK"].ToString().Trim();
                                GDQUAY_DTO.NGAYTAO = DateTime.Parse(dataReaderParent["NGAYTAO"].ToString());
                                GDQUAY_DTO.MANGUOITAO = dataReaderParent["MANGUOITAO"].ToString();
                                GDQUAY_DTO.NGUOITAO = dataReaderParent["NGUOITAO"].ToString();
                                GDQUAY_DTO.MAQUAYBAN = dataReaderParent["MAQUAYBAN"].ToString();
                                GDQUAY_DTO.NGAYPHATSINH = DateTime.Parse(dataReaderParent["NGAYPHATSINH"].ToString());
                                GDQUAY_DTO.HINHTHUCTHANHTOAN = dataReaderParent["HINHTHUCTHANHTOAN"].ToString();
                                decimal.TryParse(dataReaderParent["TIENKHACHDUA"].ToString(), out TIENKHACHDUA);
                                GDQUAY_DTO.TIENKHACHDUA = TIENKHACHDUA;
                                decimal.TryParse(dataReaderParent["TIENVOUCHER"].ToString(), out TIENVOUCHER);
                                GDQUAY_DTO.TIENVOUCHER = TIENVOUCHER;
                                decimal.TryParse(dataReaderParent["TIENTHEVIP"].ToString(), out TIENTHEVIP);
                                GDQUAY_DTO.TIENTHEVIP = TIENTHEVIP;
                                decimal.TryParse(dataReaderParent["TIENTRALAI"].ToString(), out TIENTRALAI);
                                GDQUAY_DTO.TIENTRALAI = TIENTRALAI;
                                decimal.TryParse(dataReaderParent["TIENTHE"].ToString(), out TIENTHE);
                                GDQUAY_DTO.TIENTHE = TIENTHE;
                                decimal.TryParse(dataReaderParent["TIENCOD"].ToString(), out TIENCOD);
                                GDQUAY_DTO.TIENCOD = TIENCOD;
                                decimal.TryParse(dataReaderParent["TIENMAT"].ToString(), out TIENMAT);
                                GDQUAY_DTO.TIENMAT = TIENMAT;
                                decimal.TryParse(dataReaderParent["TTIENCOVAT"].ToString(), out TTIENCOVAT);
                                GDQUAY_DTO.TTIENCOVAT = TTIENCOVAT;
                                GDQUAY_DTO.THOIGIAN = dataReaderParent["THOIGIAN"].ToString();
                                GDQUAY_DTO.MAKHACHHANG = dataReaderParent["MAKHACHHANG"].ToString();
                                GDQUAY_DTO.UNITCODE = dataReaderParent["UNITCODE"].ToString();
                            }
                        }

                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT [MAGDQUAYPK],[MAKHOHANG],[MADONVI],[MAVATTU],[MANGUOITAO],[NGUOITAO],[MABOPK],[NGAYTAO],[NGAYPHATSINH] ,[SOLUONG],[TTIENCOVAT],[GIABANLECOVAT],[TYLECHIETKHAU]
                        ,[TIENCHIETKHAU],[TYLEKHUYENMAI],[TIENKHUYENMAI],[TYLEVOUCHER],[TIENVOUCHER],[TYLELAILE],[GIAVON],[MAVAT],[VATBAN],[MACHUONGTRINHKM],[UNITCODE] FROM [dbo].[NVHANGGDQUAY_ASYNCCLIENT] WHERE MAGDQUAYPK = @MAGDQUAYPK AND MADONVI = @MADONVI");
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("MAGDQUAYPK", SqlDbType.NVarChar, 50).Value = GDQUAY_DTO.MAGIAODICHQUAYPK;
                        cmd.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value = GDQUAY_DTO.UNITCODE;
                        SqlDataReader dataReader = dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            decimal giaVon, giaBanLeVAT, VATBAN, TIENCHIETKHAU, TYLECHIETKHAU, TYLEKHUYENMAI, TIENKHUYENMAI, TYLEVOUCHER, TIENVOUCHER, TYLELAILE, GIAVON, TTIENCOVAT = 0;
                            decimal soLuong = 0;
                            int i = 0;
                            List<BOHANG_DTO> listBoHang = new List<BOHANG_DTO>();
                            while (dataReader.Read())
                            {
                                NVHANGGDQUAY_ASYNCCLIENT item = new NVHANGGDQUAY_ASYNCCLIENT();
                                string MaBoHangPk = dataReader["MABOPK"].ToString().Trim();
                                if (!string.IsNullOrEmpty(MaBoHangPk) && !MaBoHangPk.Equals("BH"))
                                {
                                    decimal SOLUONG, GIABANLECOVAT, THANHTIENCOVAT = 0;
                                    string MaVatTu = dataReader["MAVATTU"].ToString().ToUpper().Trim();
                                    decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                    decimal.TryParse(dataReader["GIABANLECOVAT"].ToString(), out GIABANLECOVAT);
                                    decimal.TryParse(dataReader["TTIENCOVAT"].ToString(), out THANHTIENCOVAT);
                                    List<BOHANG_DETAILS_DTO> listBoHangMatHangExist = new List<BOHANG_DETAILS_DTO>();
                                    BOHANG_DTO boHangExist = listBoHang.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk));
                                    if (boHangExist == null)
                                    {
                                        BOHANG_DTO boHang = new BOHANG_DTO();
                                        boHang.MABOHANG = MaBoHangPk;
                                        boHang.TTIENCOVAT = THANHTIENCOVAT;
                                        boHang.TONGSL = SOLUONG;
                                        BOHANG_DETAILS_DTO MatHangExist = listBoHangMatHangExist.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAVATTU.Equals(MaVatTu));
                                        if (MatHangExist == null)
                                        {
                                            BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                            mathang.MABOHANG = MaBoHangPk;
                                            mathang.MAVATTU = MaVatTu;
                                            mathang.SOLUONG = SOLUONG;
                                            mathang.GIABANLECOVAT = GIABANLECOVAT;
                                            mathang.TTIENCOVAT = THANHTIENCOVAT;
                                            boHang.MATHANG_BOHANG.Add(mathang);
                                        }
                                        listBoHang.Add(boHang);
                                    }
                                    else
                                    {
                                        boHangExist.TTIENCOVAT += THANHTIENCOVAT;
                                        boHangExist.TONGSL += SOLUONG;
                                        BOHANG_DETAILS_DTO MatHangExist = boHangExist.MATHANG_BOHANG.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAVATTU.Equals(MaVatTu));
                                        if (MatHangExist == null)
                                        {
                                            BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                            mathang.MABOHANG = MaBoHangPk;
                                            mathang.MAVATTU = MaVatTu;
                                            mathang.SOLUONG = SOLUONG;
                                            mathang.GIABANLECOVAT = GIABANLECOVAT;
                                            mathang.TTIENCOVAT = THANHTIENCOVAT;
                                            boHangExist.MATHANG_BOHANG.Add(mathang);
                                        }
                                    }
                                }
                                else
                                {
                                    item.MAVATTU = dataReader["MAVATTU"].ToString();
                                    EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
                                    _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(item.MAVATTU,GDQUAY_DTO.UNITCODE);
                                    item.TENDAYDU = _EXTEND_VATTU_DTO.TENVATTU;
                                    decimal.TryParse(dataReader["SOLUONG"].ToString(), out soLuong);
                                    item.SOLUONG = soLuong;
                                    item.BARCODE = _EXTEND_VATTU_DTO.BARCODE;
                                    decimal.TryParse(dataReader["GIABANLECOVAT"].ToString(), out giaBanLeVAT);
                                    item.GIABANLECOVAT = giaBanLeVAT;
                                    decimal.TryParse(dataReader["GIAVON"].ToString(), out giaVon);
                                    item.GIAVON = giaVon;
                                    item.MABOPK = dataReader["MABOPK"].ToString();
                                    decimal.TryParse(dataReader["VATBAN"].ToString(), out VATBAN);
                                    item.VATBAN = VATBAN;
                                    item.MAKHACHHANG = dataReader["MAKHACHHANG"].ToString();
                                    item.MAKEHANG = _EXTEND_VATTU_DTO.MAKEHANG;
                                    decimal.TryParse(dataReader["TIENCHIETKHAU"].ToString(), out TIENCHIETKHAU);
                                    item.TIENCHIETKHAU = TIENCHIETKHAU;
                                    decimal.TryParse(dataReader["TYLECHIETKHAU"].ToString(), out TYLECHIETKHAU);
                                    item.TYLECHIETKHAU = TYLECHIETKHAU;
                                    decimal.TryParse(dataReader["TYLEKHUYENMAI"].ToString(), out TYLEKHUYENMAI);
                                    item.TYLEKHUYENMAI = TYLEKHUYENMAI;
                                    decimal.TryParse(dataReader["TIENKHUYENMAI"].ToString(), out TIENKHUYENMAI);
                                    item.TIENKHUYENMAI = TIENKHUYENMAI;
                                    decimal.TryParse(dataReader["TYLEVOUCHER"].ToString(), out TYLEVOUCHER);
                                    item.TYLEVOUCHER = TYLEVOUCHER;
                                    decimal.TryParse(dataReader["TIENVOUCHER"].ToString(), out TIENVOUCHER);
                                    item.TIENVOUCHER = TIENVOUCHER;
                                    decimal.TryParse(dataReader["TYLELAILE"].ToString(), out TYLELAILE);
                                    item.TYLELAILE = TYLELAILE;
                                    decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                    item.GIAVON = GIAVON;
                                    decimal.TryParse(dataReader["TTIENCOVAT"].ToString(), out TTIENCOVAT);
                                    item.TTIENCOVAT = TTIENCOVAT;
                                    item.MAVAT = dataReader["MAVAT"].ToString();
                                    item.DONVITINH = _EXTEND_VATTU_DTO.DONVITINH;
                                    GDQUAY_DTO.LST_DETAILS.Add(item);
                                }
                            }
                            //Add mã bó hàng vào list
                            if (listBoHang.Count > 0)
                            {
                                foreach (BOHANG_DTO row in listBoHang)
                                {
                                    NVHANGGDQUAY_ASYNCCLIENT item = new NVHANGGDQUAY_ASYNCCLIENT();
                                    decimal TONGLE = 0;
                                    decimal SUM_SOLUONG_BO = 0;
                                    item.MAVATTU = row.MABOHANG;
                                    SqlCommand commamdBoHang = new SqlCommand();
                                    commamdBoHang.Connection = connection;
                                    commamdBoHang.CommandText = string.Format(@"SELECT DM_BOHANG.MABOHANG,DM_BOHANG.TENBOHANG,SUM(DM_BOHANGCHITIET.SOLUONG) AS TONGSOLUONG,SUM(DM_BOHANGCHITIET.TONGLE) AS TONGLE FROM DM_BOHANG INNER JOIN DM_BOHANGCHITIET ON DM_BOHANG.MABOHANG = DM_BOHANGCHITIET.MABOHANG WHERE DM_BOHANG.MABOHANG = @MABOHANG AND DM_BOHANG.UNITCODE = @UNITCODE GROUP BY DM_BOHANG.MABOHANG,DM_BOHANG.TENBOHANG");
                                    commamdBoHang.Parameters.Add("MABOHANG", SqlDbType.NVarChar, 50).Value = row.MABOHANG;
                                    commamdBoHang.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentUnitCode;
                                    SqlDataReader dataReaderBoHang = commamdBoHang.ExecuteReader();
                                    if (dataReaderBoHang.HasRows)
                                    {
                                        while (dataReaderBoHang.Read())
                                        {
                                            item.TENDAYDU = dataReaderBoHang["TENBOHANG"].ToString();
                                            decimal.TryParse(dataReaderBoHang["TONGSOLUONG"].ToString(), out SUM_SOLUONG_BO);
                                            decimal.TryParse(dataReaderBoHang["TONGLE"].ToString(), out TONGLE);
                                        }
                                    }
                                    decimal.TryParse((row.TONGSL / SUM_SOLUONG_BO).ToString(), out soLuong);
                                    item.SOLUONG = soLuong;
                                    item.BARCODE = "";
                                    decimal.TryParse(row.TTIENCOVAT.ToString(), out giaBanLeVAT);
                                    item.GIABANLECOVAT = giaBanLeVAT;
                                    item.DONVITINH = "BÓ";
                                    item.GIAVON = 0;
                                    item.MABOPK = row.MABOHANG.ToString();
                                    item.VATBAN = 0;
                                    item.MAKHACHHANG = "";
                                    item.MAKEHANG = ";";
                                    item.MACHUONGTRINHKM = "";
                                    item.LOAIKHUYENMAI = "0";
                                    item.TIENCHIETKHAU = 0;
                                    item.TYLECHIETKHAU = 0;
                                    item.TYLEKHUYENMAI = Decimal.Round((row.TTIENCOVAT - TONGLE) / row.TTIENCOVAT);
                                    item.TIENKHUYENMAI = row.TTIENCOVAT - TONGLE;
                                    item.TYLEVOUCHER = 0;
                                    item.TIENVOUCHER = 0;
                                    item.TYLELAILE = 0;
                                    item.GIAVON = 0;
                                    item.TTIENCOVAT = giaBanLeVAT;
                                    item.MAVAT = "";
                                    GDQUAY_DTO.LST_DETAILS.Add(item);
                                }
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
                    NotificationLauncher.ShowNotificationError("Thông báo", "Không có kết nối với cơ sở dữ liệu", 1, "0x1", "0x8", "normal");
                }
            }
            return GDQUAY_DTO;
        }

        public static NVGDQUAY_ASYNCCLIENT_DTO SEARCH_BY_CODE_PAY_FROM_ORACLE(string MaGiaoDich)
        {
            NVGDQUAY_ASYNCCLIENT_DTO GDQUAY_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
            decimal ChietKhau = 0;
            //BEGIN LOGIN
            string MaGiaoDichQuayPk = MaGiaoDich.Trim() + "." + Session.Session.CurrentUnitCode.Split('-')[1];
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        string KieuThanhToan = null;
                        OracleCommand cmdParent = new OracleCommand();
                        cmdParent.Connection = connection;
                        cmdParent.CommandText = string.Format(@"SELECT * FROM NVGDQUAY_ASYNCCLIENT WHERE MAGIAODICH = :MAGD AND UNITCODE = :UNITCODE");
                        cmdParent.CommandType = CommandType.Text;
                        cmdParent.Parameters.Add("MAGD", OracleDbType.NVarchar2, 50).Value = MaGiaoDich.Trim();
                        cmdParent.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                        OracleDataReader dataReaderParent = null;
                        dataReaderParent = cmdParent.ExecuteReader();
                        if (dataReaderParent.HasRows)
                        {
                            int LoaiGD = 0;
                            decimal TIENKHACHDUA, TIENVOUCHER, TIENTHEVIP, TIENTRALAI, TIENTHE, TIENCOD, TIENMAT, TTIENCOVAT = 0;
                            while (dataReaderParent.Read())
                            {
                                int.TryParse(dataReaderParent["LOAIGIAODICH"].ToString(), out LoaiGD);
                                GDQUAY_DTO.LOAIGIAODICH = LoaiGD;
                                GDQUAY_DTO.MAGIAODICH = dataReaderParent["MAGIAODICH"].ToString().Trim();
                                GDQUAY_DTO.MAGIAODICHQUAYPK = dataReaderParent["MAGIAODICHQUAYPK"].ToString().Trim();
                                GDQUAY_DTO.NGAYTAO = DateTime.Parse(dataReaderParent["NGAYTAO"].ToString());
                                GDQUAY_DTO.MANGUOITAO = dataReaderParent["MANGUOITAO"].ToString();
                                GDQUAY_DTO.NGUOITAO = dataReaderParent["NGUOITAO"].ToString();
                                GDQUAY_DTO.MAQUAYBAN = dataReaderParent["MAQUAYBAN"].ToString();
                                GDQUAY_DTO.NGAYPHATSINH = DateTime.Parse(dataReaderParent["NGAYTAO"].ToString());
                                GDQUAY_DTO.HINHTHUCTHANHTOAN = dataReaderParent["HINHTHUCTHANHTOAN"].ToString();
                                decimal.TryParse(dataReaderParent["TIENKHACHDUA"].ToString(), out TIENKHACHDUA);
                                GDQUAY_DTO.TIENKHACHDUA = TIENKHACHDUA;
                                decimal.TryParse(dataReaderParent["TIENVOUCHER"].ToString(), out TIENVOUCHER);
                                GDQUAY_DTO.TIENVOUCHER = TIENVOUCHER;
                                decimal.TryParse(dataReaderParent["TIENTHEVIP"].ToString(), out TIENTHEVIP);
                                GDQUAY_DTO.TIENTHEVIP = TIENTHEVIP;
                                decimal.TryParse(dataReaderParent["TIENTRALAI"].ToString(), out TIENTRALAI);
                                GDQUAY_DTO.TIENTRALAI = TIENTRALAI;
                                decimal.TryParse(dataReaderParent["TIENTHE"].ToString(), out TIENTHE);
                                GDQUAY_DTO.TIENTHE = TIENTHE;
                                decimal.TryParse(dataReaderParent["TIENCOD"].ToString(), out TIENCOD);
                                GDQUAY_DTO.TIENCOD = TIENCOD;
                                decimal.TryParse(dataReaderParent["TIENMAT"].ToString(), out TIENMAT);
                                GDQUAY_DTO.TIENMAT = TIENMAT;
                                decimal.TryParse(dataReaderParent["TTIENCOVAT"].ToString(), out TTIENCOVAT);
                                GDQUAY_DTO.TTIENCOVAT = TTIENCOVAT;
                                GDQUAY_DTO.THOIGIAN = dataReaderParent["THOIGIAN"].ToString();
                                GDQUAY_DTO.MAKHACHHANG = dataReaderParent["MAKHACHHANG"].ToString();
                                GDQUAY_DTO.UNITCODE = dataReaderParent["UNITCODE"].ToString();
                            }
                        }

                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT * FROM NVHANGGDQUAY_ASYNCCLIENT WHERE MAGDQUAYPK = :MAGDPK AND MADONVI = :MADONVI");
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("MAGDPK", OracleDbType.NVarchar2, 50).Value = MaGiaoDichQuayPk;
                        cmd.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                        OracleDataReader dataReader = null;
                        dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            decimal giaVon, giaBanLeVAT, VATBAN, TIENCHIETKHAU, TYLECHIETKHAU, TYLEKHUYENMAI, TIENKHUYENMAI, TYLEVOUCHER, TIENVOUCHER, TYLELAILE, GIAVON, TTIENCOVAT = 0;
                            decimal soLuong = 0;
                            int i = 0;
                            List<BOHANG_DTO> listBoHang = new List<BOHANG_DTO>();
                            while (dataReader.Read())
                            {
                                NVHANGGDQUAY_ASYNCCLIENT item = new NVHANGGDQUAY_ASYNCCLIENT();
                                string MaBoHangPk = dataReader["MABOPK"].ToString().Trim();
                                if (!string.IsNullOrEmpty(MaBoHangPk) && !MaBoHangPk.Equals("BH"))
                                {
                                    decimal SOLUONG, GIABANLECOVAT, THANHTIENCOVAT = 0;
                                    string MaVatTu = dataReader["MAVATTU"].ToString().ToUpper().Trim();
                                    decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                    decimal.TryParse(dataReader["GIABANLECOVAT"].ToString(), out GIABANLECOVAT);
                                    decimal.TryParse(dataReader["TTIENCOVAT"].ToString(), out THANHTIENCOVAT);
                                    List<BOHANG_DETAILS_DTO> listBoHangMatHangExist = new List<BOHANG_DETAILS_DTO>();
                                    BOHANG_DTO boHangExist = listBoHang.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk));
                                    if (boHangExist == null)
                                    {
                                        BOHANG_DTO boHang = new BOHANG_DTO();
                                        boHang.MABOHANG = MaBoHangPk;
                                        boHang.TTIENCOVAT = THANHTIENCOVAT;
                                        boHang.TONGSL = SOLUONG;
                                        BOHANG_DETAILS_DTO MatHangExist = listBoHangMatHangExist.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAVATTU.Equals(MaVatTu));
                                        if (MatHangExist == null)
                                        {
                                            BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                            mathang.MABOHANG = MaBoHangPk;
                                            mathang.MAVATTU = MaVatTu;
                                            mathang.SOLUONG = SOLUONG;
                                            mathang.GIABANLECOVAT = GIABANLECOVAT;
                                            mathang.TTIENCOVAT = THANHTIENCOVAT;
                                            boHang.MATHANG_BOHANG.Add(mathang);
                                        }
                                        listBoHang.Add(boHang);
                                    }
                                    else
                                    {
                                        boHangExist.TTIENCOVAT += THANHTIENCOVAT;
                                        boHangExist.TONGSL += SOLUONG;
                                        BOHANG_DETAILS_DTO MatHangExist = boHangExist.MATHANG_BOHANG.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAVATTU.Equals(MaVatTu));
                                        if (MatHangExist == null)
                                        {
                                            BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                            mathang.MABOHANG = MaBoHangPk;
                                            mathang.MAVATTU = MaVatTu;
                                            mathang.SOLUONG = SOLUONG;
                                            mathang.GIABANLECOVAT = GIABANLECOVAT;
                                            mathang.TTIENCOVAT = THANHTIENCOVAT;
                                            boHangExist.MATHANG_BOHANG.Add(mathang);
                                        }
                                    }
                                }
                                else
                                {
                                    item.MAVATTU = dataReader["MAVATTU"].ToString();
                                    item.TENDAYDU = dataReader["TENDAYDU"].ToString();
                                    decimal.TryParse(dataReader["SOLUONG"].ToString(), out soLuong);
                                    item.SOLUONG = soLuong;
                                    item.BARCODE = dataReader["BARCODE"].ToString();
                                    decimal.TryParse(dataReader["GIABANLECOVAT"].ToString(), out giaBanLeVAT);
                                    item.GIABANLECOVAT = giaBanLeVAT;
                                    decimal.TryParse(dataReader["GIAVON"].ToString(), out giaVon);
                                    item.GIAVON = giaVon;
                                    item.MABOPK = dataReader["MABOPK"].ToString();
                                    decimal.TryParse(dataReader["VATBAN"].ToString(), out VATBAN);
                                    item.VATBAN = VATBAN;
                                    item.MAKHACHHANG = dataReader["MAKHACHHANG"].ToString();
                                    if (dataReader["MAKEHANG"] != null)
                                    {
                                        item.MAKEHANG = dataReader["MAKEHANG"].ToString();
                                    }
                                    else
                                    {
                                        item.MAKEHANG = ";";
                                    }
                                    if (dataReader["MACHUONGTRINHKM"] != null)
                                    {
                                        item.MACHUONGTRINHKM = dataReader["MACHUONGTRINHKM"].ToString();
                                    }
                                    else
                                    {
                                        item.MACHUONGTRINHKM = ";";
                                    }
                                    if (dataReader["LOAIKHUYENMAI"] != null)
                                    {
                                        item.LOAIKHUYENMAI = dataReader["LOAIKHUYENMAI"].ToString();
                                    }
                                    else
                                    {
                                        item.LOAIKHUYENMAI = ";";
                                    }
                                    decimal.TryParse(dataReader["TIENCHIETKHAU"].ToString(), out TIENCHIETKHAU);
                                    item.TIENCHIETKHAU = TIENCHIETKHAU;
                                    decimal.TryParse(dataReader["TYLECHIETKHAU"].ToString(), out TYLECHIETKHAU);
                                    item.TYLECHIETKHAU = TYLECHIETKHAU;
                                    decimal.TryParse(dataReader["TYLEKHUYENMAI"].ToString(), out TYLEKHUYENMAI);
                                    item.TYLEKHUYENMAI = TYLEKHUYENMAI;
                                    decimal.TryParse(dataReader["TIENKHUYENMAI"].ToString(), out TIENKHUYENMAI);
                                    item.TIENKHUYENMAI = TIENKHUYENMAI;
                                    decimal.TryParse(dataReader["TYLEVOUCHER"].ToString(), out TYLEVOUCHER);
                                    item.TYLEVOUCHER = TYLEVOUCHER;
                                    decimal.TryParse(dataReader["TIENVOUCHER"].ToString(), out TIENVOUCHER);
                                    item.TIENVOUCHER = TIENVOUCHER;
                                    decimal.TryParse(dataReader["TYLELAILE"].ToString(), out TYLELAILE);
                                    item.TYLELAILE = TYLELAILE;
                                    decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                    item.GIAVON = GIAVON;
                                    decimal.TryParse(dataReader["TTIENCOVAT"].ToString(), out TTIENCOVAT);
                                    item.TTIENCOVAT = TTIENCOVAT;
                                    item.MAVAT = dataReader["MAVAT"].ToString();
                                    OracleCommand commamdVatTu = new OracleCommand();
                                    commamdVatTu.Connection = connection;
                                    commamdVatTu.CommandText = string.Format(@"SELECT DONVITINH FROM V_VATTU_GIABAN WHERE MAHANG = :MAVATTU AND MADONVI = :MADONVI");
                                    commamdVatTu.Parameters.Add("MAVATTU", OracleDbType.NVarchar2, 50).Value = item.MAVATTU;
                                    commamdVatTu.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                    OracleDataReader dataReaderVatTu = commamdVatTu.ExecuteReader();
                                    if (dataReaderVatTu.HasRows)
                                    {
                                        if (dataReaderVatTu.HasRows)
                                        {
                                            while (dataReaderVatTu.Read())
                                            {
                                                item.DONVITINH = dataReaderVatTu["DONVITINH"].ToString();
                                            }
                                        }
                                    }
                                    GDQUAY_DTO.LST_DETAILS.Add(item);
                                }
                            }
                            //Add mã bó hàng vào list
                            if (listBoHang.Count > 0)
                            {
                                foreach (BOHANG_DTO row in listBoHang)
                                {
                                    NVHANGGDQUAY_ASYNCCLIENT item = new NVHANGGDQUAY_ASYNCCLIENT();
                                    decimal TONGLE = 0;
                                    decimal SUM_SOLUONG_BO = 0;
                                    item.MAVATTU = row.MABOHANG;
                                    OracleCommand commamdBoHang = new OracleCommand();
                                    commamdBoHang.Connection = connection;
                                    commamdBoHang.CommandText = string.Format(@"SELECT a.MABOHANG,a.TENBOHANG,SUM(b.SOLUONG) AS TONGSOLUONG,SUM(b.TONGLE) AS TONGLE FROM DM_BOHANG a INNER JOIN DM_BOHANGCHITIET b ON a.MABOHANG = b.MABOHANG WHERE a.MABOHANG = :MABOHANG AND a.UNITCODE = :UNITCODE GROUP BY a.MABOHANG,a.TENBOHANG");
                                    commamdBoHang.Parameters.Add("MABOHANG", OracleDbType.NVarchar2, 50).Value = row.MABOHANG;
                                    commamdBoHang.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                    OracleDataReader dataReaderBoHang = commamdBoHang.ExecuteReader();
                                    if (dataReaderBoHang.HasRows)
                                    {
                                        while (dataReaderBoHang.Read())
                                        {
                                            item.TENDAYDU = dataReaderBoHang["TENBOHANG"].ToString();
                                            decimal.TryParse(dataReaderBoHang["TONGSOLUONG"].ToString(), out SUM_SOLUONG_BO);
                                            decimal.TryParse(dataReaderBoHang["TONGLE"].ToString(), out TONGLE);
                                        }
                                    }
                                    decimal.TryParse((row.TONGSL / SUM_SOLUONG_BO).ToString(), out soLuong);
                                    item.SOLUONG = soLuong;
                                    item.BARCODE = "";
                                    item.GIABANLECOVAT = TONGLE;
                                    item.DONVITINH = "BÓ";
                                    item.GIAVON = 0;
                                    item.MABOPK = row.MABOHANG;
                                    item.VATBAN = row.TYLEVAT_BO;
                                    item.MAKHACHHANG = "";
                                    item.MAKEHANG = "";
                                    item.MACHUONGTRINHKM = "";
                                    item.LOAIKHUYENMAI = "";
                                    item.TIENCHIETKHAU = 0;
                                    item.TYLECHIETKHAU = 0;
                                    item.TIENKHUYENMAI = 0;
                                    item.TYLEVOUCHER = 0;
                                    item.TIENVOUCHER = 0;
                                    item.TYLELAILE = 0;
                                    if (row.MATHANG_BOHANG.Count > 0)
                                    {
                                        decimal SUM_TTIENCOVAT_MATHANG_BOHANG = 0;
                                        foreach (BOHANG_DETAILS_DTO _BOHANG_DETAILS_DTO in row.MATHANG_BOHANG)
                                        {
                                            SUM_TTIENCOVAT_MATHANG_BOHANG += _BOHANG_DETAILS_DTO.TTIENCOVAT;
                                        }
                                        item.TTIENCOVAT = SUM_TTIENCOVAT_MATHANG_BOHANG;
                                    }
                                    item.GIAVON = 0;
                                    item.TTIENCOVAT = row.TTIENCOVAT;
                                    item.MAVAT = row.MAVAT_BO;
                                    GDQUAY_DTO.LST_DETAILS.Add(item);
                                }
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
                    NotificationLauncher.ShowNotificationError("Thông báo", "Không có kết nối với cơ sở dữ liệu", 1, "0x1", "0x8", "normal");
                }
            }
            return GDQUAY_DTO;
            //END LOGIN
        }
        private void BINDING_DATA_TO_DATAGRIDVIEW(string MaGiaoDichQuay)
        {
            NVGDQUAY_ASYNCCLIENT_DTO data = new NVGDQUAY_ASYNCCLIENT_DTO();
            data.LST_DETAILS = new List<NVHANGGDQUAY_ASYNCCLIENT>();
            try
            {
                if (!string.IsNullOrEmpty(MaGiaoDichQuay))
                {
                    try
                    {
                        if (Config.CheckConnectToServer())
                        {
                            data = SEARCH_BY_CODE_PAY_FROM_ORACLE(MaGiaoDichQuay);
                        }
                        else
                        {
                            data = SEARCH_BY_CODE_PAY_FROM_SQLSERVER(MaGiaoDichQuay);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("THÔNG BÁO ! XẢY RA LỖI KHI LẤY DỮ LIỆU TỪ CƠ SỞ DỮ LIỆU");
                    }
                    if (data != null)
                    {
                        dgvDetails_Tab.Rows.Clear();
                        dgvDetails_Tab.DataSource = null;
                        dgvDetails_Tab.Refresh();
                        decimal SUM_SOLUONG = 0;
                        decimal SUM_TIENKHUYENMAI = 0;
                        foreach (NVHANGGDQUAY_ASYNCCLIENT vattu in data.LST_DETAILS)
                        {
                            SUM_TIENKHUYENMAI += vattu.TIENKHUYENMAI;
                            SUM_SOLUONG += vattu.SOLUONG;
                            int idx = dgvDetails_Tab.Rows.Add();
                            DataGridViewRow rowData = dgvDetails_Tab.Rows[idx];
                            rowData.Cells["KEY"].Value = idx + 1;
                            rowData.Cells["MAVATTU"].Value = vattu.MAVATTU;
                            rowData.Cells["TENVATTU"].Value = vattu.TENDAYDU;
                            rowData.Cells["DONVITINH"].Value = vattu.DONVITINH;
                            rowData.Cells["DONGIA"].Value = FormatCurrency.FormatMoney(vattu.GIABANLECOVAT);
                            rowData.Cells["SOLUONG"].Value = FormatCurrency.FormatMoney(vattu.SOLUONG);
                            rowData.Cells["GIAVON"].Value = FormatCurrency.FormatMoney(vattu.GIAVON);
                            rowData.Cells["TTIENCOVAT"].Value = FormatCurrency.FormatMoney(vattu.TTIENCOVAT);
                            rowData.Cells["TIENKHUYENMAI"].Value = FormatCurrency.FormatMoney(vattu.TIENKHUYENMAI);
                            if (dgvDetails_Tab.RowCount > 0)
                            {
                                this.dgvDetails_Tab.Sort(this.dgvDetails_Tab.Columns["KEY"], ListSortDirection.Ascending);
                                this.dgvDetails_Tab.ClearSelection();
                                this.dgvDetails_Tab.Rows[0].Selected = true;
                            }
                        }
                        txtChiTiet_ThuNgan.Text = data.NGUOITAO;
                        txtChiTiet_ThoiGian.Text = data.NGAYPHATSINH.ToString("dd/MM/yyyy") + " - " + data.THOIGIAN;
                        txtChiTiet_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG);
                        txtChiTiet_TienTraLai.Text = FormatCurrency.FormatMoney(data.TIENTRALAI);
                        txtChiTiet_TongTien.Text = FormatCurrency.FormatMoney(data.TTIENCOVAT);
                        txtChiTiet_TienChietKhau.Text = FormatCurrency.FormatMoney(SUM_TIENKHUYENMAI);
                    }
                }
            }
            catch
            {
                MessageBox.Show("THÔNG BÁO ! XẢY RA LỖI KHI ĐẨY DỮ LIỆU DATAGRIDVIEW ");
            }
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string MaGiaoDich = txtKeySearch.Text.Trim();
            BINDING_DATA_TO_DATAGRIDVIEW(MaGiaoDich);
        }
        public void RefreshGiaoDichBanTraLai(bool state)
        {
            if (state)
            {
                txtKeySearch.Focus();
                btnF2ThemMoiTraLai.Enabled = true;
                btnF1ThanhToanTraLai.Enabled = false;
                btnF3GiamHang.Enabled = false;
                btnF4TangHang.Enabled = false;
                btnF5LamMoiGiaoDich.Enabled = false;
                txtKeySearch.Enabled = false;
                dateTimeTuNgay.Enabled = false;
                dateTimeDenNgay.Enabled = false;
            }
        }
        const int WM_KEYDOWN = 0x0101;
        //const int WM_KEYUP = 0x0100;
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                if ((Keys)m.WParam == Keys.F1)
                {
                    NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
                    NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_BILL = new NVGDQUAY_ASYNCCLIENT_DTO();
                    if (Config.CheckConnectToServer())
                    {
                        _NVGDQUAY_ASYNCCLIENT_DTO = FrmThanhToanTraLaiService.KHOITAO_DULIEU_THANHTOAN_BANTRALAI_FROM_ORACLE(txtTraLai_MaGiaoDich.Text.Trim(), txtTraLai_TongTien.Text, dgvTraLai);
                        _NVGDQUAY_ASYNCCLIENT_BILL = FrmThanhToanTraLaiService.KHOITAO_BILL_THANHTOAN_BANTRALAI_FROM_ORACLE(txtTraLai_MaGiaoDich.Text.Trim(), txtTraLai_TongTien.Text, dgvTraLai);
                    }
                    else
                    {
                        _NVGDQUAY_ASYNCCLIENT_DTO = FrmThanhToanTraLaiService.KHOITAO_DULIEU_THANHTOAN_BANTRALAI_FROM_SQLSERVER(txtTraLai_MaGiaoDich.Text.Trim(), txtTraLai_TongTien.Text, dgvTraLai);
                        _NVGDQUAY_ASYNCCLIENT_BILL = FrmThanhToanTraLaiService.KHOITAO_BILL_THANHTOAN_BANTRALAI_FROM_SQLSERVER(txtTraLai_MaGiaoDich.Text.Trim(), txtTraLai_TongTien.Text, dgvTraLai);
                    }
                    FrmThanhToanTraLai frmThanhToanTraLai = new FrmThanhToanTraLai(_NVGDQUAY_ASYNCCLIENT_DTO, _NVGDQUAY_ASYNCCLIENT_BILL);
                    frmThanhToanTraLai.SetHanler(RefreshGiaoDichBanTraLai); //Set sự kiện khi đóng form Thanh toán 
                    frmThanhToanTraLai.ShowDialog();
                }
                if ((Keys)m.WParam == Keys.F2)
                {

                    if (string.IsNullOrEmpty(txtTraLai_MaGiaoDich.Text.Trim()) && dgvTraLai.RowCount == 0 && dgvDetails_Tab.RowCount == 0)
                    {
                        btnF2ThemMoiTraLai.Enabled = false;
                        txtKeySearch.Enabled = true;
                        txtKeySearch.Focus();
                        dateTimeTuNgay.Enabled = true;
                        dateTimeDenNgay.Enabled = true;
                        btnTimKiem.Enabled = true;

                    }

                    if (btnF2ThemMoiTraLai.Enabled && dgvTraLai.RowCount > 0)
                    {
                        txtKeySearch.Text = "";
                        txtChiTiet_TienChietKhau.Text = "";
                        txtChiTiet_ThoiGian.Text = "";
                        txtChiTiet_ThuNgan.Text = "";
                        txtChiTiet_TienChietKhau.Text = "";
                        txtChiTiet_TienTraLai.Text = "";
                        txtChiTiet_TongTien.Text = "";
                        txtTraLai_SoLuong.Text = "";
                        txtTraLai_SoLuong.Text = "";
                        txtTraLai_MaGiaoDich.Text = "";
                        txtTraLai_ThuNgan.Text = "";
                        txtChiTiet_TongSoLuong.Text = "";
                        txtTraLai_TongSoLuong.Text = "";
                        txtTraLai_MaVatTu.Text = "";
                        txtTraLai_TongTien.Text = "";
                        if (dgvDetails_Tab.RowCount > 0)
                        {
                            dgvDetails_Tab.Rows.Clear();
                            dgvDetails_Tab.DataSource = null;
                            dgvDetails_Tab.Refresh();
                        }
                        if (dgvTraLai.RowCount > 0)
                        {
                            dgvTraLai.Rows.Clear();
                            dgvTraLai.DataSource = null;
                            dgvTraLai.Refresh();
                            SUM_SOLUONG_BAN_TRALAI(dgvTraLai);
                            SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai);
                        }
                    }
                }
                //currentRowDgvTraLai
                if ((Keys)m.WParam == Keys.F3)
                {
                    if (currentRowDgvTraLai != -1 && dgvTraLai.RowCount > 0)
                    {
                        decimal SoLuongDangChon = 0;
                        decimal.TryParse(dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value.ToString(), out SoLuongDangChon);
                        SoLuongDangChon = SoLuongDangChon - 1;
                        if (SoLuongDangChon > 0)
                        {
                            decimal SoLuongGioiHanNhap = 0;
                            string MaVatTuDangChon = dgvTraLai.Rows[currentRowDgvTraLai].Cells["MAVATTU_TRALAI"].Value.ToString().ToUpper().Trim();
                            if (!string.IsNullOrEmpty(MaVatTuDangChon) && dgvDetails_Tab.RowCount > 0)
                            {
                                foreach (DataGridViewRow rowCheck in dgvDetails_Tab.Rows)
                                {
                                    string code = rowCheck.Cells["MAVATTU"].Value.ToString();
                                    if (!string.IsNullOrEmpty(code) &&
                                        code.Trim().ToUpper() == MaVatTuDangChon.Trim().ToUpper())
                                    {
                                        decimal.TryParse(
                                            dgvDetails_Tab.Rows[rowCheck.Index].Cells["SOLUONG"].Value.ToString(),
                                            out SoLuongGioiHanNhap);
                                    }
                                }
                            }
                            if (SoLuongDangChon > SoLuongGioiHanNhap)
                            {
                                NotificationLauncher.ShowNotificationError("Cảnh báo", "Quá số lượng cho phép", 1, "0x1",
                                    "0x8", "normal");
                            }
                            else
                            {
                                decimal DONGIA = 0;
                                decimal.TryParse(
                                    dgvTraLai.Rows[currentRowDgvTraLai].Cells["DONGIA_TRALAI"].Value.ToString(),
                                    out DONGIA);
                                dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value =
                                    FormatCurrency.FormatMoney(SoLuongDangChon);
                                dgvTraLai.Rows[currentRowDgvTraLai].Cells["THANHTIEN_TRALAI"].Value =
                                    FormatCurrency.FormatMoney(DONGIA * SoLuongDangChon);
                                txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                                txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                            }
                        }
                        else
                        {
                            DialogResult result = MessageBox.Show("BẠN MUỐN XÓA MÃ NÀY ?", "CẢNH BÁO", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                            if (result == DialogResult.Yes)
                            {
                                if (dgvTraLai.Rows.Count == 1)
                                {
                                    dgvTraLai.Rows.Clear();
                                    dgvTraLai.DataSource = null;
                                    dgvTraLai.Refresh();

                                    txtTraLai_SoLuong.Text = "";
                                    txtTraLai_MaVatTu.Text = "";
                                    txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                                    txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                                }
                                else
                                {
                                    dgvTraLai.Rows.RemoveAt(currentRowDgvTraLai);
                                }
                            }
                            else if (result == DialogResult.No)
                            {
                                //code for No
                            }
                            else if (result == DialogResult.Cancel)
                            {
                                //code for Cancel
                            }
                        }
                        if (this.dgvTraLai.Rows.Count > 0)
                        {
                            this.dgvTraLai.Sort(this.dgvTraLai.Columns["KEY_TRALAI"], ListSortDirection.Ascending);
                            this.dgvTraLai.ClearSelection();
                            this.dgvTraLai.Rows[0].Selected = true;
                            txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                            txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                        }
                    }
                }
                if ((Keys)m.WParam == Keys.F4)
                {
                    if (currentRowDgvTraLai != -1 && dgvTraLai.RowCount > 0)
                    {
                        decimal SoLuongDangChon = 0;
                        decimal.TryParse(dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value.ToString(), out SoLuongDangChon);
                        SoLuongDangChon = SoLuongDangChon + 1;
                        decimal SoLuongGioiHanNhap = 0;
                        string MaVatTuDangChon = dgvTraLai.Rows[currentRowDgvTraLai].Cells["MAVATTU_TRALAI"].Value.ToString().ToUpper().Trim();
                        if (!string.IsNullOrEmpty(MaVatTuDangChon) && dgvDetails_Tab.RowCount > 0)
                        {
                            foreach (DataGridViewRow rowCheck in dgvDetails_Tab.Rows)
                            {
                                string code = rowCheck.Cells["MAVATTU"].Value.ToString();
                                if (!string.IsNullOrEmpty(code) && code.Trim().ToUpper() == MaVatTuDangChon.Trim().ToUpper())
                                {
                                    decimal.TryParse(dgvDetails_Tab.Rows[rowCheck.Index].Cells["SOLUONG"].Value.ToString(), out SoLuongGioiHanNhap);
                                }
                            }
                        }
                        if (SoLuongDangChon > SoLuongGioiHanNhap)
                        {
                            NotificationLauncher.ShowNotificationError("CẢNH BÁO", "QUÁ SỐ LƯỢNG CHO PHÉP", 1, "0x1", "0x8", "normal");
                        }
                        else
                        {
                            decimal DONGIA = 0; decimal.TryParse(dgvTraLai.Rows[currentRowDgvTraLai].Cells["DONGIA_TRALAI"].Value.ToString(), out DONGIA);
                            dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value = FormatCurrency.FormatMoney(SoLuongDangChon);
                            dgvTraLai.Rows[currentRowDgvTraLai].Cells["THANHTIEN_TRALAI"].Value = FormatCurrency.FormatMoney(DONGIA * SoLuongDangChon);
                            txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                            txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                        }
                    }
                }
                if ((Keys)m.WParam == Keys.F5)
                {
                    if (dgvTraLai.RowCount > 0)
                    {
                        DialogResult result = MessageBox.Show("BẠN MUỐN LÀM MỚI GIAO DỊCH ?", "CẢNH BÁO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            dgvTraLai.Rows.Clear();
                            dgvTraLai.DataSource = null;
                            dgvTraLai.Refresh();
                            SUM_SOLUONG_BAN_TRALAI(dgvTraLai);
                            SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai);

                            btnF1ThanhToanTraLai.Enabled = false;
                            btnF3GiamHang.Enabled = false;
                            btnF4TangHang.Enabled = false;
                            btnF5LamMoiGiaoDich.Enabled = false;
                        }
                    }
                }
                if ((Keys)m.WParam == Keys.F6)
                {
                    FrmTimKiemGiaoDich frmSearch = new FrmTimKiemGiaoDich(txtKeySearch.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                    frmSearch.SetHanlerGiaoDichSearch(LoadDataSearch);
                    frmSearch.ShowDialog();
                }
            }
            return base.ProcessKeyPreview(ref m);
        }
        public void LoadDataSearch(string MaGiaoDichTimKiem, DateTime TuNgay, DateTime DenNgay, int DieuKienLoc)
        {
            cboDieuKienChon.SelectedIndex = DieuKienLoc;
            dateTimeDenNgay.Format = DateTimePickerFormat.Custom;
            dateTimeDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeDenNgay.Value = DenNgay;
            dateTimeTuNgay.Format = DateTimePickerFormat.Custom;
            dateTimeTuNgay.CustomFormat = "dd/MM/yyyy"; this.dateTimeTuNgay.Value = TuNgay;
            NVGDQUAY_ASYNCCLIENT_DTO data = new NVGDQUAY_ASYNCCLIENT_DTO();
            data.LST_DETAILS = new List<NVHANGGDQUAY_ASYNCCLIENT>();
            try
            {
                if (!string.IsNullOrEmpty(MaGiaoDichTimKiem))
                {
                    if (Config.CheckConnectToServer())
                    {
                        data = SEARCH_BY_CODE_PAY_FROM_ORACLE(MaGiaoDichTimKiem);
                    }
                    else
                    {
                        data = SEARCH_BY_CODE_PAY_FROM_SQLSERVER(MaGiaoDichTimKiem);
                    }
                    if (data != null)
                    {
                        dgvDetails_Tab.Rows.Clear();
                        dgvDetails_Tab.DataSource = null;
                        dgvDetails_Tab.Refresh();
                        decimal SUM_SOLUONG = 0;
                        decimal SUM_TIENKHUYENMAI = 0;
                        foreach (NVHANGGDQUAY_ASYNCCLIENT vattu in data.LST_DETAILS)
                        {
                            SUM_TIENKHUYENMAI += vattu.TIENKHUYENMAI;
                            SUM_SOLUONG += vattu.SOLUONG;
                            int idx = dgvDetails_Tab.Rows.Add();
                            DataGridViewRow rowData = dgvDetails_Tab.Rows[idx];
                            rowData.Cells["KEY"].Value = idx + 1;
                            rowData.Cells["MAVATTU"].Value = vattu.MAVATTU;
                            rowData.Cells["TENVATTU"].Value = vattu.TENDAYDU;
                            rowData.Cells["DONVITINH"].Value = vattu.DONVITINH;
                            rowData.Cells["DONGIA"].Value = FormatCurrency.FormatMoney(vattu.GIABANLECOVAT);
                            rowData.Cells["SOLUONG"].Value = FormatCurrency.FormatMoney(vattu.SOLUONG);
                            rowData.Cells["GIAVON"].Value = FormatCurrency.FormatMoney(vattu.GIAVON);
                            rowData.Cells["TTIENCOVAT"].Value = FormatCurrency.FormatMoney(vattu.TTIENCOVAT);
                            rowData.Cells["TIENKHUYENMAI"].Value = FormatCurrency.FormatMoney(vattu.TIENKHUYENMAI);
                            if (dgvDetails_Tab.RowCount > 0)
                            {
                                this.dgvDetails_Tab.Sort(this.dgvDetails_Tab.Columns["KEY"], ListSortDirection.Ascending);
                                this.dgvDetails_Tab.ClearSelection();
                                this.dgvDetails_Tab.Rows[0].Selected = true;
                            }
                        }
                        if (dgvTraLai.RowCount <= 0)
                        {
                            btnF1ThanhToanTraLai.Enabled = false;
                            btnF3GiamHang.Enabled = false;
                            btnF4TangHang.Enabled = false;
                            btnF5LamMoiGiaoDich.Enabled = false;
                        }
                        txtChiTiet_ThuNgan.Text = data.NGUOITAO;
                        txtChiTiet_ThoiGian.Text = data.NGAYPHATSINH.ToString("dd/MM/yyyy") + " - " + data.THOIGIAN;
                        txtChiTiet_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG);
                        txtChiTiet_TienTraLai.Text = FormatCurrency.FormatMoney(data.TIENTRALAI);
                        txtChiTiet_TongTien.Text = FormatCurrency.FormatMoney(data.TTIENCOVAT);
                        txtChiTiet_TienChietKhau.Text = FormatCurrency.FormatMoney(SUM_TIENKHUYENMAI);
                        txtTraLai_MaGiaoDich.Text = data.MAGIAODICH.Trim() + "-TRL";
                        txtKeySearch.Enabled = true;
                        dateTimeTuNgay.Enabled = true;
                        dateTimeDenNgay.Enabled = true;
                        btnTimKiem.Enabled = true; txtKeySearch.Text = "";

                    }
                }
            }
            catch (Exception ex)
            {
                txtKeySearch.Text = "";
                WriteLogs.LogError(ex);
            }
        }

        private decimal SUM_TIEN_THANHTOAN_TRALAI(DataGridView dgvCheck)
        {
            decimal SUM_THANHTIEN = 0;
            if (dgvCheck.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
                {
                    decimal THANHTIEN_ROW = 0;
                    decimal.TryParse(rowCheck.Cells["THANHTIEN_TRALAI"].Value.ToString(), out THANHTIEN_ROW);
                    SUM_THANHTIEN = SUM_THANHTIEN + THANHTIEN_ROW;
                }
            }
            return SUM_THANHTIEN;
        }
        private decimal SUM_SOLUONG_BAN_TRALAI(DataGridView dgvCheck)
        {
            decimal SUM_SOLUONG = 0;
            if (dgvCheck.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
                {
                    decimal SOLUONG_ROW = 0;
                    decimal.TryParse(rowCheck.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG_ROW);
                    SUM_SOLUONG = SUM_SOLUONG + SOLUONG_ROW;
                }
            }
            return SUM_SOLUONG;
        }

        private void dgvDetails_Tab_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            currentRowDgvTraLai = 0;
            string maVatTuChange = dgvDetails_Tab.Rows[e.RowIndex].Cells["MAVATTU"].Value.ToString().Trim();
            if (CheckExistInDgvTraLai(dgvTraLai, maVatTuChange) != -1)
            {
                NotificationLauncher.ShowNotificationWarning("Thông báo", "Mã này đã chọn !", 1, "0x1", "0x8", "normal");
                txtTraLai_SoLuong.SelectAll();
                txtTraLai_SoLuong.Focus();
            }
            else
            {
                if (e.RowIndex != -1)
                {
                    currentRowDgvTraLai = e.RowIndex;
                    int idx = 0;
                    int index = e.RowIndex;
                    try
                    {
                        idx = dgvTraLai.Rows.Add();
                    }
                    catch (Exception ex)
                    {

                    }
                    DataGridViewRow rowDataTraLai = dgvTraLai.Rows[idx];
                    rowDataTraLai.Cells["KEY_TRALAI"].Value = idx + 1;
                    string MaVatTu = dgvDetails_Tab.Rows[index].Cells["MAVATTU"].Value.ToString().Trim();
                    rowDataTraLai.Cells["MAVATTU_TRALAI"].Value = MaVatTu;
                    string TenVatTu = dgvDetails_Tab.Rows[index].Cells["TENVATTU"].Value.ToString().Trim();
                    rowDataTraLai.Cells["TENVATTU_TRALAI"].Value = TenVatTu;
                    decimal SoLuong = 0; decimal.TryParse(dgvDetails_Tab.Rows[index].Cells["SOLUONG"].Value.ToString(), out SoLuong);
                    rowDataTraLai.Cells["SOLUONG_TRALAI"].Value = FormatCurrency.FormatMoney(SoLuong);
                    decimal DonGia = 0; decimal.TryParse(dgvDetails_Tab.Rows[index].Cells["DONGIA"].Value.ToString(), out DonGia);
                    rowDataTraLai.Cells["DONGIA_TRALAI"].Value = FormatCurrency.FormatMoney(DonGia);
                    string DonViTinh = dgvDetails_Tab.Rows[index].Cells["DONVITINH"].Value != null ? dgvDetails_Tab.Rows[index].Cells["DONVITINH"].Value.ToString() : "";
                    rowDataTraLai.Cells["DONVITINH_TRALAI"].Value = DonViTinh;
                    decimal TienKhuyenMai = 0; decimal.TryParse(dgvDetails_Tab.Rows[index].Cells["TIENKHUYENMAI"].Value.ToString(), out TienKhuyenMai);
                    rowDataTraLai.Cells["TIENKM_TRALAI"].Value = FormatCurrency.FormatMoney(TienKhuyenMai);
                    decimal ThanhTien = 0; decimal.TryParse(dgvDetails_Tab.Rows[index].Cells["TTIENCOVAT"].Value.ToString(), out ThanhTien);
                    rowDataTraLai.Cells["THANHTIEN_TRALAI"].Value = FormatCurrency.FormatMoney(ThanhTien);
                    decimal GiaVon = 0; decimal.TryParse(dgvDetails_Tab.Rows[index].Cells["GIAVON"].Value.ToString(), out GiaVon);
                    rowDataTraLai.Cells["GIAVON_TRALAI"].Value = FormatCurrency.FormatMoney(GiaVon);
                    if (dgvTraLai.RowCount > 0)
                    {
                        this.dgvTraLai.Sort(this.dgvTraLai.Columns["KEY_TRALAI"], ListSortDirection.Ascending);
                        this.dgvTraLai.ClearSelection();
                        this.dgvTraLai.Rows[0].Selected = true;
                        txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                        txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                    }
                    btnF1ThanhToanTraLai.Enabled = true;
                    btnF3GiamHang.Enabled = true;
                    btnF4TangHang.Enabled = true;
                    btnF5LamMoiGiaoDich.Enabled = true;
                    txtTraLai_MaVatTu.Text = MaVatTu;
                    txtTraLai_SoLuong.Text = FormatCurrency.FormatMoney(SoLuong);
                    txtTraLai_SoLuong.SelectAll();
                    txtTraLai_SoLuong.Focus();
                }
            }
        }

        private int CheckExistInDgvTraLai(DataGridView dgvTraLai, string maVatTu)
        {
            int result = -1;
            foreach (DataGridViewRow rowCheck in dgvTraLai.Rows)
            {
                string code = string.Empty;
                code = rowCheck.Cells["MAVATTU_TRALAI"].Value.ToString();
                if (!string.IsNullOrEmpty(code) && code.Trim().ToUpper() == maVatTu.Trim().ToUpper())
                {
                    result = rowCheck.Index;
                }
            }
            return result;
        }

        //Setcurrent Row
        // Sự kiện này để thay đổi giá trị điều kiện tìm kiếm
        private void cboDieuKienChon_SelectedValueChanged(object sender, EventArgs e)
        {
            VALUE_SELECTED_CHANGE = (int)cboDieuKienChon.SelectedIndex;
        }

        private void txtTraLai_SoLuong_Validating(object sender, CancelEventArgs e)
        {
            //index dòng đang chọn trên datagridview bán trả lại
            if (currentRowDgvTraLai != -1 && dgvTraLai.RowCount > 0)
            {
                decimal SoLuongNhapVao = 0;
                decimal SoLuongGioiHanNhap = 0;
                decimal.TryParse(txtTraLai_SoLuong.Text, out SoLuongNhapVao);
                string MaVatTuDangChon = dgvTraLai.Rows[currentRowDgvTraLai].Cells["MAVATTU_TRALAI"].Value.ToString().ToUpper().Trim();
                if (!string.IsNullOrEmpty(MaVatTuDangChon) && dgvDetails_Tab.RowCount > 0)
                {
                    foreach (DataGridViewRow rowCheck in dgvDetails_Tab.Rows)
                    {
                        string code = rowCheck.Cells["MAVATTU"].Value.ToString();
                        if (!string.IsNullOrEmpty(code) && code.Trim().ToUpper() == MaVatTuDangChon.Trim().ToUpper())
                        {
                            decimal.TryParse(dgvDetails_Tab.Rows[rowCheck.Index].Cells["SOLUONG"].Value.ToString(), out SoLuongGioiHanNhap);
                        }
                    }
                }
                if (SoLuongNhapVao > SoLuongGioiHanNhap)
                {
                    //NotificationLauncher.ShowNotification("Cảnh báo", "Quá số lượng cho phép", 1, "0x1", "0x8","normal");
                }
                else
                {
                    decimal DONGIA = 0; decimal.TryParse(dgvTraLai.Rows[currentRowDgvTraLai].Cells["DONGIA_TRALAI"].Value.ToString(), out DONGIA);
                    dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value = FormatCurrency.FormatMoney(SoLuongNhapVao);
                    dgvTraLai.Rows[currentRowDgvTraLai].Cells["THANHTIEN_TRALAI"].Value = FormatCurrency.FormatMoney(DONGIA * SoLuongNhapVao);
                    txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                    txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                }
            }
        }

        //Setcurrent Row
        // Sự kiện này để thay đổi rowIndex chọn
        private void dgvTraLai_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTraLai.CurrentRow.Index != currentRowDgvTraLai)
            {
                currentRowDgvTraLai = dgvTraLai.CurrentRow.Index;
                txtTraLai_MaVatTu.Text = dgvTraLai.Rows[currentRowDgvTraLai].Cells["MAVATTU_TRALAI"].Value != null ? dgvTraLai.Rows[currentRowDgvTraLai].Cells["MAVATTU_TRALAI"].Value.ToString().ToUpper().Trim() : "";
                txtTraLai_SoLuong.Text = dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value != null ? dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value.ToString().ToUpper().Trim() : "0";
            }
        }

        //txtKeySearch_KeyDown == txtKeySearch_Validating
        private void txtKeySearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtKeySearch.Text.Length > 3)
            {
                try
                {
                    //SỰ KIỆN NHẤN ENTER TÌM KIẾM GIAO DỊCH
                    if (Config.CheckConnectToServer()) // CÓ MẠNG INTERNET
                    {
                        List<NVGDQUAY_ASYNCCLIENT_DTO> listData = TIMKIEM_GIAODICHQUAY(txtKeySearch.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                        if (listData.Count > 0 && listData.Count == 1)
                        {
                            //TÌM ĐÚNG MÃ GIAO DỊCH THÌ BINDING DỮ LIỆU VÀO GRIDVIEW
                            BINDING_DATA_TO_DATAGRIDVIEW(listData[0].MAGIAODICH.Trim());
                            txtTraLai_MaGiaoDich.Text = listData[0].MAGIAODICH.Trim() + "-TRL";
                        }
                        else
                        {
                            FrmTimKiemGiaoDich frmSearch = new FrmTimKiemGiaoDich(txtKeySearch.Text.Trim(),
                                dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                            frmSearch.SetHanlerGiaoDichSearch(LoadDataSearch);
                            frmSearch.ShowDialog();
                        }
                    }
                    else // MẤT MẠNG INTERNET
                    {
                        List<NVGDQUAY_ASYNCCLIENT_DTO> listData = TIMKIEM_GIAODICHQUAY_FROM_SQL(txtKeySearch.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                        if (listData.Count > 0 && listData.Count == 1)
                        {
                            //TÌM ĐÚNG MÃ GIAO DỊCH THÌ BINDING DỮ LIỆU VÀO GRIDVIEW
                            BINDING_DATA_TO_DATAGRIDVIEW(listData[0].MAGIAODICH.Trim());
                            txtTraLai_MaGiaoDich.Text = listData[0].MAGIAODICH.Trim() + "-TRL";
                        }
                    }
                    //Trường hợp nhấn tìm kiếm nhiều lần thì clear datagridView dgvTraLai
                    if (dgvTraLai.RowCount > 0)
                    {
                        dgvTraLai.Rows.Clear();
                        dgvTraLai.DataSource = null;
                        dgvTraLai.Refresh();
                        SUM_SOLUONG_BAN_TRALAI(dgvTraLai);
                        SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai);
                    }
                }
                catch
                {
                    MessageBox.Show("THÔNG BÁO ! XẢY RA LỖI KHI TÌM KIẾM !");
                }
            }
        }
        //txtKeySearch_KeyDown == txtKeySearch_Validating
        private void txtKeySearch_Validating(object sender, CancelEventArgs e)
        {
            if (txtKeySearch.Text.Length > 3)
            {
                //SỰ KIỆN NHẤN ENTER TÌM KIẾM GIAO DỊCH
                if (Config.CheckConnectToServer()) // CÓ MẠNG INTERNET
                {
                    List<NVGDQUAY_ASYNCCLIENT_DTO> listData = TIMKIEM_GIAODICHQUAY(txtKeySearch.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                    if (listData.Count > 0 && listData.Count == 1)
                    {
                        //TÌM ĐÚNG MÃ GIAO DỊCH THÌ BINDING DỮ LIỆU VÀO GRIDVIEW
                        BINDING_DATA_TO_DATAGRIDVIEW(listData[0].MAGIAODICH.Trim());
                        txtTraLai_MaGiaoDich.Text = listData[0].MAGIAODICH.Trim() + "-TRL";
                    }
                    else
                    {
                        FrmTimKiemGiaoDich frmSearch = new FrmTimKiemGiaoDich(txtKeySearch.Text.Trim(),
                            dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                        frmSearch.SetHanlerGiaoDichSearch(LoadDataSearch);
                        frmSearch.ShowDialog();
                    }
                }
                else // MẤT MẠNG INTERNET
                {
                    List<NVGDQUAY_ASYNCCLIENT_DTO> listData = TIMKIEM_GIAODICHQUAY_FROM_SQL(txtKeySearch.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                    if (listData.Count > 0 && listData.Count == 1)
                    {
                        //TÌM ĐÚNG MÃ GIAO DỊCH THÌ BINDING DỮ LIỆU VÀO GRIDVIEW
                        BINDING_DATA_TO_DATAGRIDVIEW(listData[0].MAGIAODICH);
                        txtTraLai_MaGiaoDich.Text = listData[0].MAGIAODICH.Trim() + "-TRL";
                    }
                }
                //Trường hợp nhấn tìm kiếm nhiều lần thì clear datagridView dgvTraLai
                if (dgvTraLai.RowCount > 0)
                {
                    dgvTraLai.Rows.Clear();
                    dgvTraLai.DataSource = null;
                    dgvTraLai.Refresh();
                    SUM_SOLUONG_BAN_TRALAI(dgvTraLai);
                    SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai);
                }
            }
        }

        private void txtTraLai_SoLuong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //index dòng đang chọn trên datagridview bán trả lại
                if (currentRowDgvTraLai != -1 && dgvTraLai.RowCount > 0)
                {
                    decimal SoLuongNhapVao = 0;
                    decimal SoLuongGioiHanNhap = 0;
                    decimal.TryParse(txtTraLai_SoLuong.Text, out SoLuongNhapVao);
                    string MaVatTuDangChon = dgvTraLai.Rows[currentRowDgvTraLai].Cells["MAVATTU_TRALAI"].Value.ToString().ToUpper().Trim();
                    if (!string.IsNullOrEmpty(MaVatTuDangChon) && dgvDetails_Tab.RowCount > 0)
                    {
                        foreach (DataGridViewRow rowCheck in dgvDetails_Tab.Rows)
                        {
                            string code = rowCheck.Cells["MAVATTU"].Value.ToString();
                            if (!string.IsNullOrEmpty(code) && code.Trim().ToUpper() == MaVatTuDangChon.Trim().ToUpper())
                            {
                                decimal.TryParse(dgvDetails_Tab.Rows[rowCheck.Index].Cells["SOLUONG"].Value.ToString(), out SoLuongGioiHanNhap);
                            }
                        }
                    }
                    if (SoLuongNhapVao > SoLuongGioiHanNhap)
                    {
                        NotificationLauncher.ShowNotificationWarning("Cảnh báo", "Quá số lượng cho phép", 1, "0x1", "0x8", "normal");
                    }
                    else
                    {
                        decimal DONGIA = 0; decimal.TryParse(dgvTraLai.Rows[currentRowDgvTraLai].Cells["DONGIA_TRALAI"].Value.ToString(), out DONGIA);
                        dgvTraLai.Rows[currentRowDgvTraLai].Cells["SOLUONG_TRALAI"].Value = FormatCurrency.FormatMoney(SoLuongNhapVao);
                        dgvTraLai.Rows[currentRowDgvTraLai].Cells["THANHTIEN_TRALAI"].Value = FormatCurrency.FormatMoney(DONGIA * SoLuongNhapVao);
                        txtTraLai_TongSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN_TRALAI(dgvTraLai));
                        txtTraLai_TongTien.Text = FormatCurrency.FormatMoney(SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai));
                    }
                }
            }
        }

        private void btnF2ThemMoiTraLai_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTraLai_MaGiaoDich.Text.Trim()) && dgvTraLai.RowCount == 0 && dgvDetails_Tab.RowCount == 0)
            {
                btnF2ThemMoiTraLai.Enabled = false;
                txtKeySearch.Enabled = true;
                txtKeySearch.Focus();
                dateTimeTuNgay.Enabled = true;
                dateTimeDenNgay.Enabled = true;
                btnTimKiem.Enabled = true;

            }

            if (btnF2ThemMoiTraLai.Enabled && dgvTraLai.RowCount > 0)
            {
                txtKeySearch.Text = "";
                txtChiTiet_TienChietKhau.Text = "";
                txtChiTiet_ThoiGian.Text = "";
                txtChiTiet_ThuNgan.Text = "";
                txtChiTiet_TienChietKhau.Text = "";
                txtChiTiet_TienTraLai.Text = "";
                txtChiTiet_TongTien.Text = "";
                txtTraLai_SoLuong.Text = "";
                txtTraLai_SoLuong.Text = "";
                txtTraLai_MaGiaoDich.Text = "";
                txtTraLai_ThuNgan.Text = "";
                txtChiTiet_TongSoLuong.Text = "";
                txtTraLai_TongSoLuong.Text = "";
                txtTraLai_MaVatTu.Text = "";
                txtTraLai_TongTien.Text = "";
                if (dgvDetails_Tab.RowCount > 0)
                {
                    dgvDetails_Tab.Rows.Clear();
                    dgvDetails_Tab.DataSource = null;
                    dgvDetails_Tab.Refresh();
                }
                if (dgvTraLai.RowCount > 0)
                {
                    dgvTraLai.Rows.Clear();
                    dgvTraLai.DataSource = null;
                    dgvTraLai.Refresh();
                    SUM_SOLUONG_BAN_TRALAI(dgvTraLai);
                    SUM_TIEN_THANHTOAN_TRALAI(dgvTraLai);
                }
            }
        }
    }
}

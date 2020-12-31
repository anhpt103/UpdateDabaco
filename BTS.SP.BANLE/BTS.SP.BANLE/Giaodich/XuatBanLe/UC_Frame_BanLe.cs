using BTS.SP.BANLE.Common;
using BTS.SP.BANLE.Dto;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public delegate void XuLyThanhToan(bool state);
    public delegate void SearchVatTu(string filter);
    public delegate void RePrintBill(string BillId, DateTime toDate, DateTime fromDate);
    public delegate void SELECT_VATTU(int index);

    public partial class UC_Frame_BanLe : UserControl
    {
        private Keys CurrentKey = new Keys();
        private bool FlagTangHang = true;
        private int MethodPrice = 0;
        public static decimal CURRENT_ROW_GIABANLECOVAT = 0;
        public static decimal THANHTOAN_TONGTIEN_THANHTOAN = 0;
        public static string THANHTOAN_MAGIAODICH = string.Empty;
        public static DataGridView gridDataTemp = new DataGridView();
        public static int LOAIGIAODICH = 1;
        private List<VATTU_DTO> listData_TrungMa = new List<VATTU_DTO>();
        private FrmThanhToan frmThanhToan;
        private FrmTimKiemGiaoDich frmSearch;
        private frmSelectVatTu frmSelect;
        public static decimal CURRENT_SOLUONG_MACAN = 0;
        public UC_Frame_BanLe()
        {
            InitializeComponent();
            try
            {
                string msg = Config.CheckConnectToServer(out bool result);
                if (msg.Length > 0) { MessageBox.Show(msg); return; }

                if (result)
                {
                    lblNgayPhatSinhPeriod.Text = "TRẠNG THÁI BÁN: ONLINE";
                    lblNgayPhatSinhPeriod.ForeColor = Color.ForestGreen;
                    ROLE_STATE GET_ACCESS_MENU = Session.Session.GET_ROLE_BY_USERNAME(Session.Session.CurrentUnitCode, Session.Session.CurrentUserName, "banLeQuayThuNgan");
                    if (GET_ACCESS_MENU != null)
                    {
                        if (!GET_ACCESS_MENU.Banbuon) cbbLoaiGiaoDich.Items.Remove("Bán buôn");
                        if (!GET_ACCESS_MENU.Giavon) cbbLoaiGiaoDich.Items.Remove("Giá vốn");
                        if (!GET_ACCESS_MENU.Banchietkhau)
                        {
                            lblCKDon.Visible = false;
                            lblCKLe.Visible = false;
                            lblChietKhauLe.Visible = false;
                            txtChietKhauToanDon.Visible = false;
                            dgvDetails_Tab.Columns["CHIETKHAU"].Visible = false;
                        }
                        if (!GET_ACCESS_MENU.Approve)
                        {
                            btnF7.Visible = false;
                            //btnF7.Visible = true;
                        }
                        //tạm thời khóa chức năng bán giá vốn
                        cbbLoaiGiaoDich.Items.Remove("Giá vốn");
                    }
                }
                else
                {
                    lblNgayPhatSinhPeriod.Text = "TRẠNG THÁI BÁN: OFFLINE";
                    lblNgayPhatSinhPeriod.ForeColor = Color.Red;
                    lblCKDon.Visible = false;
                    lblCKLe.Visible = false;
                    lblChietKhauLe.Visible = false;
                    txtChietKhauToanDon.Visible = false;
                    dgvDetails_Tab.Columns["CHIETKHAU"].Visible = false;
                    btnF7.Visible = false;
                    cbbLoaiGiaoDich.Items.Remove("Giá vốn");
                    cbbLoaiGiaoDich.Items.Remove("Bán buôn");
                }
            }
            catch
            {

            }
            dgvDetails_Tab.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvDetails_Tab.RowTemplate.Height = 30;
            dgvDetails_Tab.Columns["STT"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["MAVATTU"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["TENVATTU"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["DONVITINH"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["GIABANLECOVAT"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["SOLUONG"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["GIAVON"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["TIENKM"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["CHIETKHAU"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["TTIENCOVAT"].HeaderCell.Style.Font = new Font("Arial", 15F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            dgvDetails_Tab.Columns["LAMACAN"].Visible = false;
            dgvDetails_Tab.Columns["TONCUOIKYSL"].Visible = false;
            //KHỞI TẠO DROPDOW BUTTON CHỌN HÌNH THỨC GIÁ BÁN
            btnF1ThanhToan.Enabled = false;
            this.btnF1ThanhToan.BackColor = Color.Gray;
            btnF2ThemMoi.Enabled = true;
            this.btnF2ThemMoi.BackColor = Color.CadetBlue;
            btnF3GiamHang.Enabled = false;
            this.btnF3GiamHang.BackColor = Color.Gray;
            btnF4TangHang.Enabled = false;
            this.btnF4TangHang.BackColor = Color.Gray;
            btnF5LamMoi.Enabled = false;
            this.btnF5LamMoi.BackColor = Color.Gray;
            btnSearchAll.Enabled = false;
            this.btnSearchAll.BackColor = Color.Gray;
            btnF7.Enabled = true;
            this.btnF7.BackColor = Color.CadetBlue;
            lblNgayPhatSinhPeriod.Text = Session.Session.CurrentNgayPhatSinh.ToString("dd/MM/yyyy");
            //Mặc định chọn Bán theo giá bán lẻ vat
            cbbLoaiGiaoDich.SelectedIndex = 0;
            MethodPrice = 1;
            LOAIGIAODICH = 1;
            dgvDetails_Tab.Columns["GIAVON"].Visible = false;
            Session.Session.CurrentLoaiGiaoDich = "BANLE";
        }

        private decimal SUM_TTIENCOVAT_THANHTOAN(DataGridView dgvCheck)
        {
            decimal SUM_TTIENCOVAT = 0;
            if (dgvCheck.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
                {
                    decimal TTIENCOVAT_ROW = 0;
                    decimal.TryParse(rowCheck.Cells["TTIENCOVAT"].Value.ToString(), out TTIENCOVAT_ROW);
                    SUM_TTIENCOVAT = SUM_TTIENCOVAT + TTIENCOVAT_ROW;
                }
            }
            return SUM_TTIENCOVAT;
        }

        private decimal SUM_SOLUONG_BAN(DataGridView dgvCheck)
        {
            decimal SUM_SOLUONG = 0;
            if (dgvCheck.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
                {
                    decimal SOLUONG_ROW = 0;
                    decimal.TryParse(rowCheck.Cells["SOLUONG"].Value.ToString(), out SOLUONG_ROW);
                    SUM_SOLUONG = SUM_SOLUONG + SOLUONG_ROW;
                }
            }
            return SUM_SOLUONG;
        }

        private decimal SUM_TONGTIENKHUYENMAI(DataGridView dgvCheck)
        {
            decimal SUM_TIENKHUYENMAI = 0;
            if (dgvCheck.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
                {
                    decimal TIENKHUYENMAI_ROW = 0;
                    decimal.TryParse(rowCheck.Cells["TIENKM"].Value.ToString(), out TIENKHUYENMAI_ROW);
                    SUM_TIENKHUYENMAI = SUM_TIENKHUYENMAI + TIENKHUYENMAI_ROW;
                }
            }
            return SUM_TIENKHUYENMAI;
        }
        private void SUM_TONGTIEN_CHIETKHAU_LE(DataGridView dgvCheck)
        {

            decimal SUM_TIENCHIETKHAU = 0;
            if (dgvCheck.Rows.Count > 0)
            {
                foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
                {
                    decimal TIENCK = 0;
                    decimal TIEN_CHIETKHAU_ROW = 0;
                    decimal SOLUONG = 0;
                    decimal GIABANLECOVAT = 0;
                    decimal TTIENCOVAT = 0;
                    decimal TIENKM = 0;
                    decimal chietKhau = 0;

                    string CHIETKHAU_PERCENT = string.Empty;
                    if (rowCheck.Cells["CHIETKHAU"].Value != null)
                    {
                        CHIETKHAU_PERCENT = rowCheck.Cells["CHIETKHAU"].Value.ToString().Trim();
                    }
                    else
                    {
                        CHIETKHAU_PERCENT = "0";
                        rowCheck.Cells["CHIETKHAU"].Value = CHIETKHAU_PERCENT;
                    }
                    if (CHIETKHAU_PERCENT.Contains('%'))
                    {
                        CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                    }
                    decimal.TryParse(CHIETKHAU_PERCENT, out TIEN_CHIETKHAU_ROW);

                    // item.TIENCK = item.SOLUONG * item.GIABANLECOVAT * chietKhau / 100;
                    decimal.TryParse(rowCheck.Cells["SOLUONG"].Value.ToString(), out SOLUONG);
                    decimal.TryParse(rowCheck.Cells["GIABANLECOVAT"].Value.ToString(), out GIABANLECOVAT);
                    decimal.TryParse(rowCheck.Cells["TTIENCOVAT"].Value.ToString(), out TTIENCOVAT);
                    decimal.TryParse(rowCheck.Cells["TIENKM"].Value.ToString(), out TIENKM);
                    if (TIEN_CHIETKHAU_ROW <= 100 && TIEN_CHIETKHAU_ROW > 0 && TIEN_CHIETKHAU_ROW <= TTIENCOVAT)
                    {
                        TIENCK = SOLUONG * GIABANLECOVAT * TIEN_CHIETKHAU_ROW / 100;
                    }
                    else if (TIEN_CHIETKHAU_ROW > TTIENCOVAT)
                    {
                        MessageBox.Show("CHIẾT KHẤU KHÔNG HỢP LỆ !");
                        TIENCK = 0;
                        rowCheck.Cells["CHIETKHAU"].Value = TIENCK;
                    }
                    else //Tiền chiết khấu
                    {
                        TIENCK = TIEN_CHIETKHAU_ROW;
                    }
                    rowCheck.Cells["TTIENCOVAT"].Value = FormatCurrency.FormatMoney(GIABANLECOVAT * SOLUONG - TIENKM - TIENCK);
                    SUM_TIENCHIETKHAU = SUM_TIENCHIETKHAU + TIENCK;
                }
                lblChietKhauLe.Text = FormatCurrency.FormatMoney(SUM_TIENCHIETKHAU);
            }
        }
        public string CONVERT_FROM_KEY_TO_CODEVATTU(string KEY)
        {
            string _RESULT = "";
            if (KEY.Length >= 4)
            {
                string beginCharacter = string.Empty;
                if (!string.IsNullOrEmpty(KEY)) beginCharacter = KEY.Substring(0, 2);
                if (beginCharacter.Equals(ConfigurationSettings.AppSettings["KEYMACAN"]) && KEY.Length > 9)
                {
                    string itemCode = string.Empty; if (!string.IsNullOrEmpty(KEY)) itemCode = KEY.Substring(2, 5);

                    string msg = Config.CheckConnectToServer(out bool result);
                    if (msg.Length > 0) { MessageBox.Show(msg); return _RESULT; }

                    if (result)
                    {
                        _RESULT = FrmXuatBanLeService.CONVERT_MACAN_TO_MAVATTU_ORACLE(itemCode, Session.Session.CurrentUnitCode);
                    }
                    else
                    {
                        _RESULT = FrmXuatBanLeService.CONVERT_MACAN_TO_MAVATTU_SQLSERVER(itemCode, Session.Session.CurrentUnitCode);
                    }
                }
                else if (beginCharacter == "BH")
                {
                    _RESULT = KEY;
                }
                else
                {
                    string msg = Config.CheckConnectToServer(out bool result);
                    if (msg.Length > 0) { MessageBox.Show(msg); return _RESULT; }

                    if (result)
                    {
                        _RESULT = FrmXuatBanLeService.CONVERT_BARCODE_TO_MAVATTU_ORACLE(KEY, Session.Session.CurrentUnitCode);
                    }
                    else
                    {
                        _RESULT = FrmXuatBanLeService.CONVERT_BARCODE_TO_MAVATTU_SQLSERVER(KEY, Session.Session.CurrentUnitCode);
                    }
                }
            }
            return _RESULT;
        }
        private int CHECK_ROW_EXIST_DATAGRIDVIEW(DataGridView dgvCheck, string maVatTu)
        {
            int result = -1;
            if (maVatTu.Length != 7)
            {
                maVatTu = CONVERT_FROM_KEY_TO_CODEVATTU(maVatTu);
            }
            foreach (DataGridViewRow rowCheck in dgvCheck.Rows)
            {
                string code = string.Empty;
                code = rowCheck.Cells["MAVATTU"].Value.ToString();
                if (!string.IsNullOrEmpty(code) && code.Trim().ToUpper() == maVatTu.Trim().ToUpper())
                {
                    result = rowCheck.Index;
                }
            }
            return result;
        }

        private void TINHTOAN_TONGTIEN_TOANHOADON(DataGridView gridView)
        {
            lblTongTienThanhToan.Text = FormatCurrency.FormatMoney(SUM_TTIENCOVAT_THANHTOAN(gridView).ToString());
            lblSumSoLuong.Text = FormatCurrency.FormatMoney(SUM_SOLUONG_BAN(gridView).ToString());
            lblTongTienKhuyenMai.Text = FormatCurrency.FormatMoney(SUM_TONGTIENKHUYENMAI(gridView).ToString());
            if (gridView.Rows.Count == 0)
            {
                txtSoLuong.Text = "0";
                txtMaHang.Text = string.Empty;
            }
        }

        //KHỞI TẠO DỮ LIỆU PASSING SANG FORM THANH TOÁN

        public NVGDQUAY_ASYNCCLIENT_DTO KHOITAO_DULIEU_BILL_BANLE(DataGridView DataGridViewBanLe)
        {
            NVGDQUAY_ASYNCCLIENT_DTO NVGDQUAY_ASYNCCLIENT_BILL = new NVGDQUAY_ASYNCCLIENT_DTO();
            NVGDQUAY_ASYNCCLIENT_BILL.MAGIAODICH = lblMaGiaoDichQuay.Text.Trim();
            NVGDQUAY_ASYNCCLIENT_BILL.MAGIAODICHQUAYPK = NVGDQUAY_ASYNCCLIENT_BILL.MAGIAODICH + "." + Session.Session.CurrentCodeStore;
            NVGDQUAY_ASYNCCLIENT_BILL.LOAIGIAODICH = LOAIGIAODICH;
            NVGDQUAY_ASYNCCLIENT_BILL.NGAYTAO = DateTime.Now;
            NVGDQUAY_ASYNCCLIENT_BILL.NGAYPHATSINH = Session.Session.CurrentNgayPhatSinh;
            NVGDQUAY_ASYNCCLIENT_BILL.THOIGIAN = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            NVGDQUAY_ASYNCCLIENT_BILL.MANGUOITAO = Session.Session.CurrentMaNhanVien;
            NVGDQUAY_ASYNCCLIENT_BILL.NGUOITAO = Session.Session.CurrentTenNhanVien;
            NVGDQUAY_ASYNCCLIENT_BILL.MAQUAYBAN = Environment.MachineName;
            NVGDQUAY_ASYNCCLIENT_BILL.TIENKHACHDUA = 0;
            NVGDQUAY_ASYNCCLIENT_BILL.TIENVOUCHER = 0;
            NVGDQUAY_ASYNCCLIENT_BILL.TIENTHEVIP = 0;
            NVGDQUAY_ASYNCCLIENT_BILL.TIENTRALAI = 0;
            NVGDQUAY_ASYNCCLIENT_BILL.TIENTHE = 0;
            NVGDQUAY_ASYNCCLIENT_BILL.TIENCOD = 0;
            decimal TTIENCOVAT = 0; decimal.TryParse(lblTongTienThanhToan.Text, out TTIENCOVAT);
            NVGDQUAY_ASYNCCLIENT_BILL.TIENMAT = TTIENCOVAT;
            NVGDQUAY_ASYNCCLIENT_BILL.TTIENCOVAT = TTIENCOVAT;
            NVGDQUAY_ASYNCCLIENT_BILL.MAKHACHHANG = "";
            decimal TONGSOLUONG = 0; decimal.TryParse(lblSumSoLuong.Text, out TONGSOLUONG);
            NVGDQUAY_ASYNCCLIENT_BILL.TONGSOLUONG = TONGSOLUONG;
            if (DataGridViewBanLe.RowCount > 0)
            {
                foreach (DataGridViewRow dataRow in DataGridViewBanLe.Rows)
                {
                    NVHANGGDQUAY_ASYNCCLIENT NVHANGGDQUAY_ASYNCCLIENT_BILL = new NVHANGGDQUAY_ASYNCCLIENT();
                    NVHANGGDQUAY_ASYNCCLIENT_BILL.MAVATTU = dataRow.Cells["MAVATTU"].Value != null ? dataRow.Cells["MAVATTU"].Value.ToString().ToUpper().Trim() : "";
                    NVHANGGDQUAY_ASYNCCLIENT_BILL.TENDAYDU = dataRow.Cells["TENVATTU"].Value != null ? dataRow.Cells["TENVATTU"].Value.ToString().ToUpper().Trim() : "";
                    decimal SOLUONG = 0;
                    if (dataRow.Cells["SOLUONG"].Value != null)
                    {
                        decimal.TryParse(dataRow.Cells["SOLUONG"].Value.ToString(), out SOLUONG);
                    }
                    NVHANGGDQUAY_ASYNCCLIENT_BILL.SOLUONG = SOLUONG;
                    decimal TTIENCOVAT_DETAIL = 0;
                    if (dataRow.Cells["TTIENCOVAT"].Value != null)
                    {
                        decimal.TryParse(dataRow.Cells["TTIENCOVAT"].Value.ToString(), out TTIENCOVAT_DETAIL);
                    }
                    NVHANGGDQUAY_ASYNCCLIENT_BILL.TTIENCOVAT = TTIENCOVAT_DETAIL;
                    decimal GIABANLECOVAT_DETAIL = 0;
                    if (dataRow.Cells["GIABANLECOVAT"].Value != null)
                    {
                        decimal.TryParse(dataRow.Cells["GIABANLECOVAT"].Value.ToString(), out GIABANLECOVAT_DETAIL);
                    }
                    NVHANGGDQUAY_ASYNCCLIENT_BILL.GIABANLECOVAT = GIABANLECOVAT_DETAIL;

                    decimal TIENCHIETKHAU_DETAIL = 0;
                    if (dataRow.Cells["CHIETKHAU"].Value != null)
                    {
                        string CHIETKHAU_PERCENT = dataRow.Cells["CHIETKHAU"].Value.ToString().Trim();
                        if (CHIETKHAU_PERCENT.Contains('%'))
                        {
                            CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                            decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out TIENCHIETKHAU_DETAIL);
                        }
                        else
                        {
                            decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out TIENCHIETKHAU_DETAIL);
                        }
                    }



                    decimal TIENKHUYENMAI_DETAIL = 0;
                    if (dataRow.Cells["TIENKM"].Value != null)
                    {
                        decimal.TryParse(dataRow.Cells["TIENKM"].Value.ToString(), out TIENKHUYENMAI_DETAIL);
                    }
                    NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENKHUYENMAI = TIENKHUYENMAI_DETAIL;

                    if (TIENCHIETKHAU_DETAIL <= 100)
                    {
                        NVHANGGDQUAY_ASYNCCLIENT_BILL.TYLECHIETKHAU = TIENCHIETKHAU_DETAIL;
                        NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENCHIETKHAU = decimal.Round((TIENCHIETKHAU_DETAIL / 100) * (NVHANGGDQUAY_ASYNCCLIENT_BILL.GIABANLECOVAT * NVHANGGDQUAY_ASYNCCLIENT_BILL.SOLUONG - NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENKHUYENMAI), 2);
                    }
                    else
                    {
                        NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENCHIETKHAU = TIENCHIETKHAU_DETAIL;
                        NVHANGGDQUAY_ASYNCCLIENT_BILL.TYLECHIETKHAU = decimal.Round((100 * TIENCHIETKHAU_DETAIL) / (NVHANGGDQUAY_ASYNCCLIENT_BILL.GIABANLECOVAT * NVHANGGDQUAY_ASYNCCLIENT_BILL.SOLUONG - NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENKHUYENMAI), 2);
                    }
                    if (dataRow.Cells["MAVATTU"].Value != null)
                    {
                        string MAVATTU = dataRow.Cells["MAVATTU"].Value.ToString().ToUpper().Trim();
                        if (MAVATTU.Substring(0, 2).Equals("BH"))
                        {
                            NVHANGGDQUAY_ASYNCCLIENT_BILL.MABOPK = MAVATTU;
                            EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();

                            string msg = Config.CheckConnectToServer(out bool result);
                            if (msg.Length > 0) { MessageBox.Show(msg); return NVGDQUAY_ASYNCCLIENT_BILL; }

                            if (result) _EXTEND_VAT_BOHANG = FrmXuatBanLeService.LAYDULIEU_VAT_BOHANG_FROM_DATABASE_ORACLE(NVHANGGDQUAY_ASYNCCLIENT_BILL.MABOPK, Session.Session.CurrentUnitCode);
                            else _EXTEND_VAT_BOHANG = FrmXuatBanLeService.LAYDULIEU_VAT_BOHANG_FROM_DATABASE_SQLSERVER(NVHANGGDQUAY_ASYNCCLIENT_BILL.MABOPK, Session.Session.CurrentUnitCode);

                            NVHANGGDQUAY_ASYNCCLIENT_BILL.VATBAN = _EXTEND_VAT_BOHANG.TYLEVATRA;
                            NVHANGGDQUAY_ASYNCCLIENT_BILL.MAVAT = _EXTEND_VAT_BOHANG.MAVATRA;
                        }
                        else
                        {

                            NVHANGGDQUAY_ASYNCCLIENT_BILL.MABOPK = "BH";
                            EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();

                            string msg = Config.CheckConnectToServer(out bool result);
                            if (msg.Length > 0) { MessageBox.Show(msg); return NVGDQUAY_ASYNCCLIENT_BILL; }

                            if (result) _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_ORACLE(NVHANGGDQUAY_ASYNCCLIENT_BILL.MAVATTU, Session.Session.CurrentUnitCode);
                            else _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(NVHANGGDQUAY_ASYNCCLIENT_BILL.MAVATTU, Session.Session.CurrentUnitCode);

                            NVHANGGDQUAY_ASYNCCLIENT_BILL.VATBAN = _EXTEND_VATTU_DTO.TYLEVATRA;
                            NVHANGGDQUAY_ASYNCCLIENT_BILL.MAVAT = _EXTEND_VATTU_DTO.MAVATRA;
                        }
                    }

                    if (NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENKHUYENMAI != 0)
                    {
                        if (NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENCHIETKHAU != 0)
                        {
                            NVHANGGDQUAY_ASYNCCLIENT_BILL.THANHTIENFULL = string.Format(@"KM:" + FormatCurrency.FormatMoney(NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENKHUYENMAI) + ";" + "CK:" + FormatCurrency.FormatMoney(NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENCHIETKHAU) + " " + FormatCurrency.FormatMoney(NVHANGGDQUAY_ASYNCCLIENT_BILL.TTIENCOVAT));
                        }
                        else
                        {
                            NVHANGGDQUAY_ASYNCCLIENT_BILL.THANHTIENFULL = string.Format(@"KM:" + FormatCurrency.FormatMoney(NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENKHUYENMAI) + " " + FormatCurrency.FormatMoney(NVHANGGDQUAY_ASYNCCLIENT_BILL.TTIENCOVAT));
                        }
                    }
                    else
                    {
                        NVHANGGDQUAY_ASYNCCLIENT_BILL.THANHTIENFULL = FormatCurrency.FormatMoney(NVHANGGDQUAY_ASYNCCLIENT_BILL.TTIENCOVAT);
                    }

                    if (NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENCHIETKHAU != 0)
                    {
                        if (NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENKHUYENMAI != 0)
                        {
                            NVHANGGDQUAY_ASYNCCLIENT_BILL.THANHTIENFULL = string.Format(@"KM:" + FormatCurrency.FormatMoney(NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENKHUYENMAI) + ";" + "CK:" + FormatCurrency.FormatMoney(NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENCHIETKHAU) + " " + FormatCurrency.FormatMoney(NVHANGGDQUAY_ASYNCCLIENT_BILL.TTIENCOVAT));
                        }
                        else
                        {
                            NVHANGGDQUAY_ASYNCCLIENT_BILL.THANHTIENFULL = string.Format(@"CK " + FormatCurrency.FormatMoney(NVHANGGDQUAY_ASYNCCLIENT_BILL.TIENCHIETKHAU) + " " + FormatCurrency.FormatMoney(NVHANGGDQUAY_ASYNCCLIENT_BILL.TTIENCOVAT));
                        }
                    }
                    else
                    {
                        NVHANGGDQUAY_ASYNCCLIENT_BILL.THANHTIENFULL = FormatCurrency.FormatMoney(NVHANGGDQUAY_ASYNCCLIENT_BILL.TTIENCOVAT);
                    }
                    NVHANGGDQUAY_ASYNCCLIENT_BILL.TTIENCOVAT_CHUA_GIAMGIA = NVHANGGDQUAY_ASYNCCLIENT_BILL.GIABANLECOVAT * NVHANGGDQUAY_ASYNCCLIENT_BILL.SOLUONG;
                    NVGDQUAY_ASYNCCLIENT_BILL.LST_DETAILS.Add(NVHANGGDQUAY_ASYNCCLIENT_BILL);
                }
            }
            return NVGDQUAY_ASYNCCLIENT_BILL;
        }

        public NVGDQUAY_ASYNCCLIENT_DTO KHOITAO_DULIEU_THANHTOAN_BANLE(DataGridView DataGridViewBanLe)
        {
            NVGDQUAY_ASYNCCLIENT_DTO NVGDQUAY_ASYNCCLIENT_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
            NVGDQUAY_ASYNCCLIENT_DTO.ID = Guid.NewGuid().ToString();
            NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICH = lblMaGiaoDichQuay.Text.Trim();
            NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK = NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICH + "." + Session.Session.CurrentCodeStore;
            NVGDQUAY_ASYNCCLIENT_DTO.MADONVI = Session.Session.CurrentUnitCode;
            NVGDQUAY_ASYNCCLIENT_DTO.LOAIGIAODICH = LOAIGIAODICH;
            NVGDQUAY_ASYNCCLIENT_DTO.NGAYTAO = DateTime.Now;
            NVGDQUAY_ASYNCCLIENT_DTO.MANGUOITAO = Session.Session.CurrentMaNhanVien;
            NVGDQUAY_ASYNCCLIENT_DTO.NGUOITAO = Session.Session.CurrentTenNhanVien;
            NVGDQUAY_ASYNCCLIENT_DTO.MAQUAYBAN = Environment.MachineName;
            NVGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH = Session.Session.CurrentNgayPhatSinh;
            NVGDQUAY_ASYNCCLIENT_DTO.HINHTHUCTHANHTOAN = "TIENMAT";
            NVGDQUAY_ASYNCCLIENT_DTO.MAVOUCHER = "";
            NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACHDUA = 0;
            NVGDQUAY_ASYNCCLIENT_DTO.TIENVOUCHER = 0;
            NVGDQUAY_ASYNCCLIENT_DTO.TIENTHEVIP = 0; NVGDQUAY_ASYNCCLIENT_DTO.TIENTRALAI = 0;
            NVGDQUAY_ASYNCCLIENT_DTO.TIENTHE = 0;
            NVGDQUAY_ASYNCCLIENT_DTO.TIENCOD = 0;
            decimal TTIENCOVAT = 0; decimal.TryParse(lblTongTienThanhToan.Text, out TTIENCOVAT);
            NVGDQUAY_ASYNCCLIENT_DTO.TIENMAT = TTIENCOVAT;
            NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT = TTIENCOVAT;
            NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
            NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE = DateTime.Now;
            NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY = Session.Session.CurrentMaNhanVien;
            NVGDQUAY_ASYNCCLIENT_DTO.I_UPDATE_DATE = DateTime.Now;
            NVGDQUAY_ASYNCCLIENT_DTO.I_UPDATE_BY = Session.Session.CurrentMaNhanVien;
            NVGDQUAY_ASYNCCLIENT_DTO.I_STATE = "50";
            NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE = Session.Session.CurrentUnitCode;
            decimal TONGSOLUONG = 0; decimal.TryParse(lblSumSoLuong.Text, out TONGSOLUONG);
            NVGDQUAY_ASYNCCLIENT_DTO.TONGSOLUONG = TONGSOLUONG;
            //KHỞI TẠO DỮ LIỆU DÒNG CHI TIẾT TỪ DATAGRIDVIEW
            int i = 0;
            if (DataGridViewBanLe.RowCount > 0)
            {
                foreach (DataGridViewRow dataRow in DataGridViewBanLe.Rows)
                {
                    if (dataRow.Cells["MAVATTU"].Value != null)
                    {
                        string MAVATTU_MABO = dataRow.Cells["MAVATTU"].Value.ToString().ToUpper().Trim();
                        if (!MAVATTU_MABO.Substring(0, 2).Equals("BH"))
                        {
                            NVHANGGDQUAY_ASYNCCLIENT NVHANGGDQUAY_ASYNCCLIENT_DTO = new NVHANGGDQUAY_ASYNCCLIENT();
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.ID = Guid.NewGuid().ToString() + "-" + i;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.MAGDQUAYPK = NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.MAKHOHANG = Session.Session.CurrentWareHouse;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.MADONVI = NVGDQUAY_ASYNCCLIENT_DTO.MADONVI;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.MAVATTU = MAVATTU_MABO;
                            EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();

                            string msg = Config.CheckConnectToServer(out bool result);
                            if (msg.Length > 0) { MessageBox.Show(msg); return NVGDQUAY_ASYNCCLIENT_DTO; }

                            if (result)
                            {
                                _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_ORACLE(MAVATTU_MABO, NVHANGGDQUAY_ASYNCCLIENT_DTO.MADONVI);
                            }
                            else
                            {
                                _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(MAVATTU_MABO, NVHANGGDQUAY_ASYNCCLIENT_DTO.MADONVI);
                            }

                            NVHANGGDQUAY_ASYNCCLIENT_DTO.DONVITINH = _EXTEND_VATTU_DTO.DONVITINH;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.BARCODE = _EXTEND_VATTU_DTO.BARCODE;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.TENDAYDU = _EXTEND_VATTU_DTO.TENVATTU;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.NGUOITAO = NVGDQUAY_ASYNCCLIENT_DTO.NGUOITAO;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.MABOPK = "BH";
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.NGAYTAO = NVGDQUAY_ASYNCCLIENT_DTO.NGAYTAO;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH = NVGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH;
                            decimal SOLUONG_DETAILS = 0;
                            if (dataRow.Cells["SOLUONG"].Value != null)
                            {
                                decimal.TryParse(dataRow.Cells["SOLUONG"].Value.ToString(), out SOLUONG_DETAILS);
                            }
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.SOLUONG = SOLUONG_DETAILS;
                            decimal TTIENCOVAT_DETAILS = 0;
                            if (dataRow.Cells["TTIENCOVAT"].Value != null)
                            {
                                decimal.TryParse(dataRow.Cells["TTIENCOVAT"].Value.ToString(), out TTIENCOVAT_DETAILS);
                            }
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT = TTIENCOVAT_DETAILS;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.VATBAN = _EXTEND_VATTU_DTO.TYLEVATRA;
                            decimal GIABANLECOVAT = 0;
                            if (dataRow.Cells["GIABANLECOVAT"].Value != null)
                            {
                                decimal.TryParse(dataRow.Cells["GIABANLECOVAT"].Value.ToString(), out GIABANLECOVAT);
                            }
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.GIABANLECOVAT = GIABANLECOVAT;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.MAKEHANG = _EXTEND_VATTU_DTO.MAKEHANG;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.MACHUONGTRINHKM = "";
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.LOAIKHUYENMAI = "";
                            decimal TIENKHUYENMAI = 0;
                            if (dataRow.Cells["TIENKM"].Value != null)
                            {
                                decimal.TryParse(dataRow.Cells["TIENKM"].Value.ToString(), out TIENKHUYENMAI);
                            }
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENKHUYENMAI = TIENKHUYENMAI;
                            decimal TYLEKHUYENMAI_DETAILS = 0;
                            if (NVHANGGDQUAY_ASYNCCLIENT_DTO.GIABANLECOVAT != 0)
                            {
                                TYLEKHUYENMAI_DETAILS = NVHANGGDQUAY_ASYNCCLIENT_DTO.SOLUONG *
                                                        (NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENKHUYENMAI /
                                                         NVHANGGDQUAY_ASYNCCLIENT_DTO.GIABANLECOVAT);
                            }
                            else
                            {
                                NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLEKHUYENMAI = 0;
                                string NOTIFI_WARNING = string.Format(@"CẢNH BÁO ! MÃ HÀNG '{0}' CÓ GIÁ BÁN LẺ BẰNG 0", NVHANGGDQUAY_ASYNCCLIENT_DTO.MAVATTU);
                                MessageBox.Show(NOTIFI_WARNING);
                            }
                            decimal CHIETKHAU = 0;
                            if (dataRow.Cells["CHIETKHAU"].Value != null)
                            {
                                string CHIETKHAU_PERCENT = dataRow.Cells["CHIETKHAU"].Value.ToString().Trim();
                                if (CHIETKHAU_PERCENT.Contains('%'))
                                {
                                    CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                                    decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out CHIETKHAU);
                                }
                                else
                                {
                                    decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out CHIETKHAU);
                                }
                            }
                            if (CHIETKHAU <= 100)
                            {
                                NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLECHIETKHAU = CHIETKHAU;
                                NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENCHIETKHAU = decimal.Round((NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLECHIETKHAU / 100) * (NVHANGGDQUAY_ASYNCCLIENT_DTO.GIABANLECOVAT * NVHANGGDQUAY_ASYNCCLIENT_DTO.SOLUONG - NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENKHUYENMAI), 2);
                            }
                            else
                            {
                                NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENCHIETKHAU = CHIETKHAU;
                                NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLECHIETKHAU = decimal.Round((100 * NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENCHIETKHAU) / (NVHANGGDQUAY_ASYNCCLIENT_DTO.GIABANLECOVAT * NVHANGGDQUAY_ASYNCCLIENT_DTO.SOLUONG - NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENKHUYENMAI), 2);
                            }

                            NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLEKHUYENMAI = TYLEKHUYENMAI_DETAILS;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLEVOUCHER = 0;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENVOUCHER = 0;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLELAILE = _EXTEND_VATTU_DTO.TYLELAILE;
                            decimal GIAVON = 0;
                            if (dataRow.Cells["GIAVON"].Value != null)
                            {
                                decimal.TryParse(dataRow.Cells["GIAVON"].Value.ToString(), out GIAVON);
                            }
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.GIAVON = GIAVON;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.ISBANAM = 0;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.MAVAT = _EXTEND_VATTU_DTO.MAVATRA;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.UNITCODE = NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE;
                            NVHANGGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT_CHUA_GIAMGIA = NVHANGGDQUAY_ASYNCCLIENT_DTO.GIABANLECOVAT * NVHANGGDQUAY_ASYNCCLIENT_DTO.SOLUONG;
                            NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(NVHANGGDQUAY_ASYNCCLIENT_DTO);
                            i++;
                        }
                        else
                        {
                            //LÀ MÃ BÓ HÀNG
                            List<EXTEND_BOHANGCHITIET_DTO> _LST_EXTEND_BOHANGCHITIET_DTO = new List<EXTEND_BOHANGCHITIET_DTO>();
                            string msg = Config.CheckConnectToServer(out bool result);
                            if (msg.Length > 0) { MessageBox.Show(msg); return NVGDQUAY_ASYNCCLIENT_DTO; }

                            if (result)
                            {
                                _LST_EXTEND_BOHANGCHITIET_DTO = FrmXuatBanLeService.LAYDULIEU_BOHANGCHITIET_FROM_DATABASE_ORACLE(MAVATTU_MABO, NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE);
                            }
                            else
                            {
                                _LST_EXTEND_BOHANGCHITIET_DTO = FrmXuatBanLeService.LAYDULIEU_BOHANGCHITIET_FROM_DATABASE_SQLSERVER(MAVATTU_MABO, NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE);
                            }

                            if (_LST_EXTEND_BOHANGCHITIET_DTO.Count > 0)
                            {
                                foreach (EXTEND_BOHANGCHITIET_DTO rowBoHang in _LST_EXTEND_BOHANGCHITIET_DTO)
                                {
                                    NVHANGGDQUAY_ASYNCCLIENT NVHANGGDQUAY_ASYNCCLIENT_DTO = new NVHANGGDQUAY_ASYNCCLIENT();
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.ID = Guid.NewGuid().ToString() + "-" + i;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.MAGDQUAYPK = NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.MAKHOHANG = Session.Session.CurrentWareHouse;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.MADONVI = NVGDQUAY_ASYNCCLIENT_DTO.MADONVI;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.MAVATTU = rowBoHang.MAHANG;
                                    EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();

                                    msg = Config.CheckConnectToServer(out result);
                                    if (msg.Length > 0) { MessageBox.Show(msg); return NVGDQUAY_ASYNCCLIENT_DTO; }

                                    if (result)
                                    {
                                        _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_ORACLE(rowBoHang.MAHANG, NVHANGGDQUAY_ASYNCCLIENT_DTO.MADONVI);
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(rowBoHang.MAHANG, NVHANGGDQUAY_ASYNCCLIENT_DTO.MADONVI);
                                    }
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.DONVITINH = _EXTEND_VATTU_DTO.DONVITINH;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.BARCODE = _EXTEND_VATTU_DTO.BARCODE;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.TENDAYDU = _EXTEND_VATTU_DTO.TENVATTU;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.NGUOITAO = NVGDQUAY_ASYNCCLIENT_DTO.NGUOITAO;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.MABOPK = MAVATTU_MABO;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.NGAYTAO = NVGDQUAY_ASYNCCLIENT_DTO.NGAYTAO;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH = NVGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH;
                                    decimal SOLUONG_DETAILS = 0;
                                    if (dataRow.Cells["SOLUONG"].Value != null)
                                    {
                                        decimal.TryParse(dataRow.Cells["SOLUONG"].Value.ToString(), out SOLUONG_DETAILS);
                                    }
                                    //SỐ LƯỢNG MÃ HÀNG TRONG BÓ = SỐ LƯỢNG BÓ MUA * SỐ LƯỢNG MẶT HÀNG TRONG BÓ (DM_BOHANGCHITIET)
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.SOLUONG = SOLUONG_DETAILS * rowBoHang.SOLUONG;

                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.VATBAN = _EXTEND_VATTU_DTO.TYLEVATRA;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.GIABANLECOVAT = _EXTEND_VATTU_DTO.GIABANLEVAT;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.MAKEHANG = _EXTEND_VATTU_DTO.MAKEHANG;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.MACHUONGTRINHKM = "";
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.LOAIKHUYENMAI = "";
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLECHIETKHAU = 0;
                                    //KHUYẾN MÃI MÃ HÀNG TRONG BÓ (DM_BOHANGCHITIET)
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENKHUYENMAI = SOLUONG_DETAILS * (_EXTEND_VATTU_DTO.GIABANLEVAT * rowBoHang.SOLUONG - rowBoHang.TONGLE);
                                    if (rowBoHang.TONGLE != 0)
                                    {
                                        NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLEKHUYENMAI = decimal.Round((100 * (SOLUONG_DETAILS * (_EXTEND_VATTU_DTO.GIABANLEVAT * rowBoHang.SOLUONG - rowBoHang.TONGLE))) / rowBoHang.TONGLE, 2);
                                    }
                                    else
                                    {
                                        NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLEKHUYENMAI = 0;
                                        string NOTIFI_WARNING = string.Format(@"CẢNH BÁO ! MÃ HÀNG '{0}' TRONG BÓ '{1}' CÓ GIÁ BÁN LẺ BẰNG 0", rowBoHang.MAHANG, MAVATTU_MABO);
                                        MessageBox.Show(NOTIFI_WARNING);
                                    }

                                    decimal TIENCHIETKHAU = 0;
                                    if (dataRow.Cells["CHIETKHAU"].Value != null)
                                    {
                                        string CHIETKHAU_PERCENT = dataRow.Cells["CHIETKHAU"].Value.ToString().Trim();
                                        if (CHIETKHAU_PERCENT.Contains('%'))
                                        {
                                            CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                                            decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out TIENCHIETKHAU);
                                        }
                                        else
                                        {
                                            decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out TIENCHIETKHAU);
                                        }
                                    }
                                    if (TIENCHIETKHAU <= 100)
                                    {
                                        NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLECHIETKHAU = TIENCHIETKHAU;
                                        NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENCHIETKHAU = decimal.Round((TIENCHIETKHAU / 100) * (SOLUONG_DETAILS * rowBoHang.SOLUONG * NVHANGGDQUAY_ASYNCCLIENT_DTO.GIABANLECOVAT - NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENKHUYENMAI), 2);
                                    }
                                    else
                                    {
                                        NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENCHIETKHAU = TIENCHIETKHAU;
                                        NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLECHIETKHAU = decimal.Round((100 * TIENCHIETKHAU) / (SOLUONG_DETAILS * rowBoHang.SOLUONG * NVHANGGDQUAY_ASYNCCLIENT_DTO.GIABANLECOVAT - NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENKHUYENMAI), 2);
                                    }

                                    //TÍNH TIỀN MÃ HÀNG TRONG BÓ HÀNG
                                    decimal TTIENCOVAT_DETAILS = 0;
                                    TTIENCOVAT_DETAILS = _EXTEND_VATTU_DTO.GIABANLEVAT * rowBoHang.SOLUONG * SOLUONG_DETAILS - NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENKHUYENMAI - NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENCHIETKHAU;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT = TTIENCOVAT_DETAILS;
                                    //
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLEVOUCHER = 0;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.TIENVOUCHER = 0;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.TYLELAILE = _EXTEND_VATTU_DTO.TYLELAILE;
                                    decimal GIAVON = 0;
                                    if (dataRow.Cells["GIAVON"].Value != null)
                                    {
                                        decimal.TryParse(dataRow.Cells["GIAVON"].Value.ToString(), out GIAVON);
                                    }
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.GIAVON = GIAVON;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.ISBANAM = 0;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.MAVAT = _EXTEND_VATTU_DTO.MAVATRA;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.UNITCODE = NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE;
                                    NVHANGGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT_CHUA_GIAMGIA = NVHANGGDQUAY_ASYNCCLIENT_DTO.GIABANLECOVAT * NVHANGGDQUAY_ASYNCCLIENT_DTO.SOLUONG;
                                    NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(NVHANGGDQUAY_ASYNCCLIENT_DTO);
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("LỖI KHÔNG KHỞI TẠO ĐƯỢC DỮ LIỆU ĐỂ LƯU TRỮ VÀO HỆ THỐNG");
            }
            return NVGDQUAY_ASYNCCLIENT_DTO;
        }

        private const int WM_KEYDOWN = 0x0101;
        //const int WM_KEYUP = 0x0100;
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                if ((Keys)m.WParam == Keys.F2 && btnF2ThemMoi.Enabled)
                {
                    lblTongTienThanhToan.Text = "";
                    lblSumSoLuong.Text = ""; lblTongTienKhuyenMai.Text = ""; lblChietKhauLe.Text = ""; lblTongTienThanhToan.Text = "";
                    dgvDetails_Tab.Rows.Clear();
                    dgvDetails_Tab.Refresh();
                    CurrentKey = Keys.F2;
                    btnF2ThemMoi.Enabled = false;
                    btnF2ThemMoi.BackColor = Color.Gray;
                    lblMaGiaoDichQuay.Text = FrmXuatBanLeService.INIT_CODE_TRADE();
                    txtMaHang.Enabled = true;
                    txtMaHang.Focus();
                    btnF1ThanhToan.Enabled = true;
                    btnF1ThanhToan.BackColor = Color.CadetBlue;
                    btnF3GiamHang.Enabled = true;
                    btnF3GiamHang.BackColor = Color.CadetBlue;
                    btnF4TangHang.Enabled = true;
                    btnF4TangHang.BackColor = Color.CadetBlue;
                    btnF5LamMoi.Enabled = true;
                    btnF5LamMoi.BackColor = Color.CadetBlue;
                    btnSearchAll.Enabled = true;
                    btnSearchAll.BackColor = Color.CadetBlue;
                    btnF7.Enabled = true;
                    this.btnF7.BackColor = Color.CadetBlue;
                }
                if ((Keys)m.WParam == Keys.F3 && !btnF2ThemMoi.Enabled && dgvDetails_Tab.RowCount > 0)
                {
                    FlagTangHang = false;
                    if (dgvDetails_Tab.Rows.Count > 0)
                    {
                        if (CurrentKey == Keys.F4 || (CurrentKey == Keys.F2))
                        {
                            CurrentKey = Keys.F3;
                            btnStatus.Text = "- GIẢM HÀNG";
                            btnStatus.BackColor = Color.DarkOrange;
                        }
                    }
                }
                if ((Keys)m.WParam == Keys.F4 && !btnF2ThemMoi.Enabled && dgvDetails_Tab.RowCount > 0)
                {
                    FlagTangHang = true;
                    TangHangGridView();
                }
                if ((Keys)m.WParam == Keys.F5 && !btnF2ThemMoi.Enabled && dgvDetails_Tab.RowCount > 0)
                {
                    CurrentKey = Keys.F4;
                    btnStatus.Text = "+ TĂNG HÀNG";
                    btnStatus.BackColor = Color.DarkTurquoise;
                    FlagTangHang = true;
                    if (dgvDetails_Tab.Rows.Count > 0)
                    {
                        DialogResult result = MessageBox.Show("THAO TÁC NÀY SẼ XÓA TOÀN BỘ CÁC MÃ ĐANG SCAN ! BẠN CÓ CHẮC CHẮN ?", "THÔNG BÁO", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            dgvDetails_Tab.Rows.Clear();
                            TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
                            HienThiManHinhLCD("", "0", "0", "0");
                        }
                        lblChietKhauLe.Text = "0";
                        txtMaHang.Focus();
                    }
                }
                if ((Keys)m.WParam == Keys.F1 && !btnF2ThemMoi.Enabled)
                {
                    if (dgvDetails_Tab.Rows.Count > 0)
                    {
                        decimal.TryParse(lblTongTienThanhToan.Text.Trim(), out THANHTOAN_TONGTIEN_THANHTOAN);
                        THANHTOAN_MAGIAODICH = lblMaGiaoDichQuay.Text.Trim();
                        NVGDQUAY_ASYNCCLIENT_DTO NVGDQUAY_ASYNCCLIENT_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
                        NVGDQUAY_ASYNCCLIENT_DTO = KHOITAO_DULIEU_THANHTOAN_BANLE(dgvDetails_Tab);
                        NVGDQUAY_ASYNCCLIENT_DTO NVGDQUAY_ASYNCCLIENT_BILL = new NVGDQUAY_ASYNCCLIENT_DTO();
                        NVGDQUAY_ASYNCCLIENT_BILL = KHOITAO_DULIEU_BILL_BANLE(dgvDetails_Tab);
                        HienThiManHinhLCD("Tiền phải trả:", "", "", FormatCurrency.FormatMoney(NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT));
                        frmThanhToan = new FrmThanhToan(NVGDQUAY_ASYNCCLIENT_DTO, NVGDQUAY_ASYNCCLIENT_BILL, EnumCommon.LoaiGiaoDich.BANLE, NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT);
                        frmThanhToan.SetHanler(ResetState); //SỰ KIỆN KHI THANH TOÁN XONG
                        frmThanhToan.ShowDialog();
                    }
                }
                if ((Keys)m.WParam == Keys.Down && !btnF2ThemMoi.Enabled) //Sự kiện phím xuống
                {
                    int index = 0;
                    if (dgvDetails_Tab.SelectedRows[0].Index == dgvDetails_Tab.Rows.Count - 1)
                    {
                        dgvDetails_Tab.Rows[index].Selected = true;
                    }
                    else
                    {
                        index = dgvDetails_Tab.SelectedRows[0].Index + 1;
                        dgvDetails_Tab.Rows[index].Selected = true;
                    }
                }
                if ((Keys)m.WParam == Keys.Up && !btnF2ThemMoi.Enabled) //Sự kiện phím xuống
                {
                    int index = 0;
                    if (dgvDetails_Tab.SelectedRows[0].Index == 0)
                    {
                        index = dgvDetails_Tab.Rows.Count - 1;
                        dgvDetails_Tab.Rows[index].Selected = true;
                    }
                    else
                    {
                        index = dgvDetails_Tab.SelectedRows[0].Index - 1;
                        dgvDetails_Tab.Rows[index].Selected = true;
                    }
                }
                if ((Keys)m.WParam == Keys.F6 && !btnF2ThemMoi.Enabled)
                {
                    frmTimKiemVatTu frm = new frmTimKiemVatTu();
                    frm.handlerSearchVatTu(SelectedSearch);
                    frm.ShowDialog();
                }
                if ((Keys)m.WParam == Keys.F7 && btnF2ThemMoi.Enabled)
                {
                    FrmTimKiemGiaoDich frmSearch = new FrmTimKiemGiaoDich(true);
                    frmSearch.SetHandlerBill(TimKIemGiaoDichQuayBanLe);
                    frmSearch.ShowDialog();
                }
            }
            //int _currentUcFrame = FrmMain._currentUcFrame;
            return base.ProcessKeyPreview(ref m);
        }

        private void btnF1ThanhToan_Click(object sender, EventArgs e)
        {
            if (!btnF2ThemMoi.Enabled)
            {
                if (dgvDetails_Tab.Rows.Count > 0)
                {
                    decimal.TryParse(lblTongTienThanhToan.Text.Trim(), out THANHTOAN_TONGTIEN_THANHTOAN);
                    THANHTOAN_MAGIAODICH = lblMaGiaoDichQuay.Text.Trim();
                    NVGDQUAY_ASYNCCLIENT_DTO NVGDQUAY_ASYNCCLIENT_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
                    NVGDQUAY_ASYNCCLIENT_DTO = KHOITAO_DULIEU_THANHTOAN_BANLE(dgvDetails_Tab);
                    NVGDQUAY_ASYNCCLIENT_DTO NVGDQUAY_ASYNCCLIENT_BILL = new NVGDQUAY_ASYNCCLIENT_DTO();
                    NVGDQUAY_ASYNCCLIENT_BILL = KHOITAO_DULIEU_BILL_BANLE(dgvDetails_Tab);
                    frmThanhToan = new FrmThanhToan(NVGDQUAY_ASYNCCLIENT_DTO, NVGDQUAY_ASYNCCLIENT_BILL, EnumCommon.LoaiGiaoDich.BANLE, NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT);
                    frmThanhToan.SetHanler(ResetState); //Set sự kiện khi đóng form Thanh toán 
                    frmThanhToan.ShowDialog();
                }
            }
        }
        /// <summary>
        /// Set lại các thuộc tính khi thanh toán xong 
        /// </summary>
        /// <param name="state"></param>
        public void ResetState(bool state) //set lại trạng thái trước khi thanh toán 
        {
            if (state)
            {
                btnF1ThanhToan.Enabled = false;
                btnF1ThanhToan.BackColor = Color.Gray;
                btnF1ThanhToan.ForeColor = Color.Black;
                btnF2ThemMoi.Enabled = true;
                btnF2ThemMoi.BackColor = Color.CadetBlue;
                btnF2ThemMoi.Text = "F2-> Thêm mới";
                btnF3GiamHang.Enabled = false;
                btnF3GiamHang.BackColor = Color.Gray;
                btnF3GiamHang.ForeColor = Color.Black;
                btnF4TangHang.Enabled = false;
                btnF4TangHang.BackColor = Color.Gray;
                btnF4TangHang.ForeColor = Color.Black;
                btnF5LamMoi.Enabled = false;
                btnF5LamMoi.BackColor = Color.Gray;
                btnF5LamMoi.ForeColor = Color.Black;
                btnSearchAll.Enabled = false;
                btnSearchAll.BackColor = Color.Gray;
                btnSearchAll.ForeColor = Color.Black;
                CurrentKey = Keys.F4;
                btnStatus.Text = "+ TĂNG HÀNG";
                btnStatus.BackColor = Color.DarkTurquoise;
                btnF7.BackColor = Color.CadetBlue;
                btnF1ThanhToan.ForeColor = Color.White;
                FlagTangHang = true;
            }
        }
        private void dgvDetails_Tab_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDetails_Tab.Rows.Count > 1)
            {
                decimal START_SOLUONG = 0;
                decimal.TryParse(dgvDetails_Tab.CurrentRow.Cells["SOLUONG"].Value.ToString(), out START_SOLUONG);
                txtSoLuong.Text = START_SOLUONG.ToString();
            }
            else
            {
                btnStatus.Text = "+ TĂNG HÀNG";
                btnStatus.BackColor = Color.DarkTurquoise;
                FlagTangHang = true;
            }
        }
        private void cbbLoaiGiaoDich_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = this.cbbLoaiGiaoDich.GetItemText(this.cbbLoaiGiaoDich.SelectedItem);
            int index = 0;
            if (selected.Equals("Bán lẻ"))
            {
                index = (int)EnumCommon.MethodGetPrice.GIABANLECOVAT;
                Session.Session.CurrentLoaiGiaoDich = "BANLE";
            }
            else
            {
                index = (int)EnumCommon.MethodGetPrice.GIABANBUONCOVAT;
                if (selected.Equals("Bán buôn"))
                {
                    Session.Session.CurrentLoaiGiaoDich = "BANBUON";
                }
                else
                {
                    Session.Session.CurrentLoaiGiaoDich = "GIAVON";
                }
            }
            MethodPrice = index;
            LOAIGIAODICH = index;
            if (MethodPrice == (int)EnumCommon.MethodGetPrice.GIABANLECOVAT) // Trường hợp bán lẻ
            {
                dgvDetails_Tab.Columns["GIAVON"].Visible = false;
            }
            else if (MethodPrice == (int)EnumCommon.MethodGetPrice.GIABANBUONCOVAT)
            {
                dgvDetails_Tab.Columns["GIAVON"].Visible = true;

            }
        }

        private void WriteDataToDataGridView()
        {
            List<VATTU_DTO> listData = new List<VATTU_DTO>();
            decimal SoLuong = 1, RETURN_TIENKHUYENMAI = 0;
            string MaVatTu = txtMaHang.Text.Trim().ToUpper();

            string msg = Config.CheckConnectToServer(out bool result);
            if (msg.Length > 0) { MessageBox.Show(msg); return; }

            if (result) //Nếu có mạng lan
            {
                listData = FrmXuatBanLeService.GET_DATA_VATTU_FROM_CSDL_ORACLE(MaVatTu, (EnumCommon.MethodGetPrice)MethodPrice);
                txtSoLuong.Focus();
                StatusConnect.Text = "TRẠNG THÁI BÁN: ONLINE";
                StatusConnect.ForeColor = Color.ForestGreen;
            }
            else
            {
                listData = FrmXuatBanLeService.GET_DATA_VATTU_FROM_CSDL_SQL(MaVatTu, (EnumCommon.MethodGetPrice)MethodPrice);
                txtSoLuong.Focus(); //Bán SQL
                StatusConnect.Text = "TRẠNG THÁI BÁN: OFFLINE";
                StatusConnect.ForeColor = Color.Red;
            }
            if (listData.Count > 1)
            {
                listData_TrungMa = listData;
                frmSelect = new frmSelectVatTu(listData);
                frmSelect.setHandler(INSERT_VATTU_GRIDVIEW);
                frmSelect.ShowDialog();
            }
            else if (listData.Count == 1)
            {
                if (listData[0].GIABANLEVAT == 0)
                {
                    string NOTIFICATION_WARNING = string.Format(@"CẢNH BÁO MÃ '{0}' CÓ GIÁ BÁN BẰNG 0 ! KHÔNG THỂ BÁN", listData[0].MAVATTU);
                    MessageBox.Show(NOTIFICATION_WARNING);
                    return;
                }
                else
                {
                    if (listData[0].TONCUOIKYSL <= 0)
                    {
                        if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_ORACLE() == 1)
                        {
                            NotificationLauncher.ShowNotificationWarning("Thông báo", "Hết hàng trong kho ! Không thể bán", 1, "0x1", "0x8", "normal");
                            return;
                        }
                    }
                    INSERT_DATA(listData[0]);
                    if (listData[0].LAMACAN)
                    {
                        CURRENT_SOLUONG_MACAN = listData[0].SOLUONG;
                    }
                    txtSoLuong.Text = SoLuong.ToString();
                }
                HienThiManHinhLCD(listData[0].TENVATTU, FormatCurrency.FormatMoney(SoLuong), FormatCurrency.FormatMoney(listData[0].GIABANLEVAT), FormatCurrency.FormatMoney(SoLuong * listData[0].GIABANLEVAT));
            }
            else
            {
                //string NOTIFICATION_WARNING = string.Format(@"THÔNG BÁO ! KHÔNG TÌM THẤY HÀNG HÓA, VẬT TƯ '{0}'", MaVatTu);
                //MessageBox.Show(NOTIFICATION_WARNING);
                if (txtMaHang.Text != "" && txtMaHang.Text.Length > 2)
                {
                    frmTimKiemVatTu frm = new frmTimKiemVatTu(txtMaHang.Text.Trim().ToUpper());
                    frm.handlerSearchVatTu(SelectedSearch);
                    frm.ShowDialog();
                }
                txtMaHang.Text = "";
            }
            if (this.dgvDetails_Tab.Rows.Count > 0)
            {
                txtSoLuong.Enabled = true;
                lblTongTienKhuyenMai.Enabled = true;
                lblSumSoLuong.Enabled = true;
                lblTongTienThanhToan.Enabled = true;
                txtChietKhauToanDon.Enabled = true;
                this.dgvDetails_Tab.Sort(this.dgvDetails_Tab.Columns["STT"], ListSortDirection.Descending);
                this.dgvDetails_Tab.ClearSelection();
                this.dgvDetails_Tab.Rows[0].Selected = true;
            }
            TINHTOAN_TONGTIEN_TOANHOADON(this.dgvDetails_Tab);
        }

        private void txtMaHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (!btnF2ThemMoi.Enabled)
                {
                    if (FlagTangHang)
                    {
                        WriteDataToDataGridView();
                        txtMaHang.Text = string.Empty;
                        txtMaHang.Focus();
                    }
                    else
                    {
                        int indexExist = CHECK_ROW_EXIST_DATAGRIDVIEW(dgvDetails_Tab, txtMaHang.Text.Trim());
                        if (indexExist >= 0)
                        {
                            dgvDetails_Tab.Rows[indexExist].Selected = true;
                            GiamHangGridView();
                            txtMaHang.Text = string.Empty;
                            txtMaHang.Focus();
                        }
                        else
                        {
                            NotificationLauncher.ShowNotificationError("CHÚ Ý", "ĐANG GIẢM HÀNG", 1, "0x1", "0x8", "normal");
                        }
                    }
                }
                else
                {
                    NotificationLauncher.ShowNotification("Thông báo", "Nhấn F2 thêm mới giao dịch", 1, "0x1", "0x8", "normal");
                }
            }
        }
        private void txtMaHang_Validating(object sender, CancelEventArgs e)
        {
            txtSoLuong.SelectAll();
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            frmTimKiemVatTu frm = new frmTimKiemVatTu();
            frm.handlerSearchVatTu(SelectedSearch);
            frm.ShowDialog();
        }

        public void SelectedSearch(string maHang)
        {
            txtMaHang.Text = maHang;
            WriteDataToDataGridView();
            txtMaHang.Text = "";
        }

        private void dgvDetails_Tab_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (dgvDetails_Tab.SelectedRows[0].Index == dgvDetails_Tab.Rows.Count)
            {

            }
        }

        private void txtMaHang_TextChanged(object sender, EventArgs e)
        {
            txtMaHang.Focus();
        }

        private void txtSoLuong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    decimal CURENT_ROW_TONCUOIKYSL = 0;
                    decimal CURRENT_ROW_TIENKM = 0;
                    string CURENT_ROW_MAVATTU = "";
                    decimal RETURN_TIENKHUYENMAI = 0;
                    decimal.TryParse(dgvDetails_Tab.SelectedRows[0].Cells["TONCUOIKYSL"].Value.ToString(), out CURENT_ROW_TONCUOIKYSL);
                    if (dgvDetails_Tab.SelectedRows[0].Cells["MAVATTU"] != null)
                    {
                        CURENT_ROW_MAVATTU = dgvDetails_Tab.SelectedRows[0].Cells["MAVATTU"].Value.ToString();
                        CURRENT_ROW_TIENKM = FrmXuatBanLeService.TINHTOAN_KHUYENMAI(CURENT_ROW_MAVATTU, (EnumCommon.MethodGetPrice)MethodPrice);
                    }
                    decimal SUM_TONGCHIETKHAU_LE = 0;

                    decimal CURENT_ROW_SOLUONG = 1;
                    decimal CHIETKHAU = 0;
                    decimal CURRENT_ROW_TTIENCOVAT = 0;
                    decimal CURRENT_ROW_GIABANLECOVAT = 0;
                    decimal.TryParse(txtSoLuong.Text.Trim(), out CURENT_ROW_SOLUONG);
                    if (CURENT_ROW_SOLUONG > CURENT_ROW_TONCUOIKYSL)
                    {
                        if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_ORACLE() == 1)
                        {
                            NotificationLauncher.ShowNotificationWarning("Thông báo", "Chỉ còn " + CURENT_ROW_TONCUOIKYSL + " sản phẩm trong kho", 1, "0x1", "0x8", "normal");
                            CURENT_ROW_SOLUONG = CURENT_ROW_TONCUOIKYSL;
                            txtSoLuong.Text = CURENT_ROW_TONCUOIKYSL.ToString();
                        }
                    }
                    dgvDetails_Tab.SelectedRows[0].Cells["SOLUONG"].Value = CURENT_ROW_SOLUONG.ToString();
                    decimal.TryParse(dgvDetails_Tab.SelectedRows[0].Cells["TTIENCOVAT"].Value.ToString(), out CURRENT_ROW_TTIENCOVAT);
                    decimal.TryParse(dgvDetails_Tab.SelectedRows[0].Cells["GIABANLECOVAT"].Value.ToString(), out CURRENT_ROW_GIABANLECOVAT);
                    string CHIETKHAU_PERCENT = dgvDetails_Tab.SelectedRows[0].Cells["CHIETKHAU"].Value.ToString().Trim();
                    if (CHIETKHAU_PERCENT.Contains('%'))
                    {
                        CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                    }
                    decimal.TryParse(CHIETKHAU_PERCENT, out CHIETKHAU);
                    //decimal.TryParse(dgvDetails_Tab.SelectedRows[0].Cells["TIENKM"].Value.ToString(), out CURRENT_ROW_TIENKM);
                    //Lưu lại số lượng vào trong lst vattu
                    if (CURRENT_ROW_TIENKM == 0)
                    {
                        if (CHIETKHAU <= 100)
                        {
                            dgvDetails_Tab.SelectedRows[0].Cells["TTIENCOVAT"].Value =
                                FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLECOVAT - CURRENT_ROW_TIENKM -
                                                           CHIETKHAU * CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLECOVAT / 100);
                        }
                        else
                        {
                            dgvDetails_Tab.SelectedRows[0].Cells["TTIENCOVAT"].Value =
                                FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLECOVAT - CURRENT_ROW_TIENKM -
                                                           CHIETKHAU * CURENT_ROW_SOLUONG);
                        }
                    }
                    else if (CURRENT_ROW_TIENKM < 100)
                    {
                        decimal SOTIEN_KHUYENMAI = (CURENT_ROW_SOLUONG * CURRENT_ROW_TIENKM * CURRENT_ROW_GIABANLECOVAT) / 100;
                        dgvDetails_Tab.SelectedRows[0].Cells["TIENKM"].Value = FormatCurrency.FormatMoney(SOTIEN_KHUYENMAI);
                        if (CHIETKHAU <= 100)
                        {
                            dgvDetails_Tab.SelectedRows[0].Cells["TTIENCOVAT"].Value =
                                FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLECOVAT - SOTIEN_KHUYENMAI -
                                                           CHIETKHAU * CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLECOVAT / 100);
                        }
                        else
                        {
                            dgvDetails_Tab.SelectedRows[0].Cells["TTIENCOVAT"].Value =
                                FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLECOVAT - SOTIEN_KHUYENMAI -
                                                           CHIETKHAU * CURENT_ROW_SOLUONG);
                        }
                    }
                    else
                    {
                        decimal SOTIEN_KHUYENMAI = 0;
                        SOTIEN_KHUYENMAI = CURRENT_ROW_TIENKM * CURENT_ROW_SOLUONG;
                        dgvDetails_Tab.SelectedRows[0].Cells["TIENKM"].Value =
                            FormatCurrency.FormatMoney(SOTIEN_KHUYENMAI);
                        if (CHIETKHAU <= 100)
                        {
                            dgvDetails_Tab.SelectedRows[0].Cells["TTIENCOVAT"].Value =
                                FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLECOVAT - SOTIEN_KHUYENMAI - CHIETKHAU * CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLECOVAT / 100);
                        }
                        else
                        {
                            dgvDetails_Tab.SelectedRows[0].Cells["TTIENCOVAT"].Value =
                                FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLECOVAT - SOTIEN_KHUYENMAI -
                                                           CHIETKHAU * CURENT_ROW_SOLUONG);
                        }
                    }
                    lblChietKhauLe.Text = FormatCurrency.FormatMoney(SUM_TONGCHIETKHAU_LE);
                    TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
                    HienThiManHinhLCD(dgvDetails_Tab.SelectedRows[0].Cells["TENVATTU"].Value.ToString(), FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG), FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLECOVAT), FormatCurrency.FormatMoney(CURENT_ROW_SOLUONG * CURRENT_ROW_GIABANLECOVAT));
                    txtMaHang.Focus();
                }
                catch (Exception)
                {
                    NotificationLauncher.ShowNotification("Thông báo", "Nhập sai số lượng", 1, "0x1", "0x8", "normal");
                }
            }
        }

        /// <summary>
        /// Sửa chiết khấu trên gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private bool flagChangeCk = false;
        private void dgvDetails_Tab_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dgvDetails_Tab.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.Name;
            if (columnName.Equals("CHIETKHAU") && e.RowIndex >= 0 && !flagChangeCk)
            {
                flagChangeCk = true;
                SUM_TONGTIEN_CHIETKHAU_LE(dgvDetails_Tab);
            }
            TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
            flagChangeCk = false;
            lblChietKhauLe.Enabled = true;
            txtMaHang.Focus();
        }

        /// <summary>
        /// Sự kiện giảm hàng
        /// </summary>
        private void GiamHangGridView()
        {
            if (CurrentKey == Keys.F4 || (CurrentKey == Keys.F2))
            {
                CurrentKey = Keys.F3;
                btnStatus.Text = "- GIẢM HÀNG";
                btnStatus.BackColor = Color.DarkOrange;
            }
            else
            {
                decimal SUM_TONGCHIETKHAU_LE = 0;
                if (dgvDetails_Tab.Rows.Count > 0)
                {
                    decimal CURRENT_ROW_SOLUONG_NEW = 0;
                    decimal CURRENT_ROW_SOLUONG,
                        CURRENT_ROW_GIABANLECOVAT,
                        CURRENT_TIENKHUYENMAI_NEW,
                        CURRENT_ROW_GIATRI,
                        CHIETKHAU = 0;
                    int rowIndex = dgvDetails_Tab.SelectedRows[0].Index;
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["GIABANLECOVAT"].Value.ToString(),
                        out CURRENT_ROW_GIABANLECOVAT);
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["SOLUONG"].Value.ToString(),
                        out CURRENT_ROW_SOLUONG);
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["TIENKM"].Value.ToString(),
                        out CURRENT_ROW_GIATRI);
                    if (dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value != null)
                    {
                        string CHIETKHAU_PERCENT = dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value.ToString().Trim();
                        if (CHIETKHAU_PERCENT.Contains('%'))
                        {
                            CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                            decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out CHIETKHAU);
                        }
                    }
                    else
                    {
                        dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value = CHIETKHAU;
                    }
                    CurrentKey = Keys.F3;
                    btnStatus.Text = "- GIẢM HÀNG";
                    btnStatus.BackColor = Color.DarkOrange;
                    if (dgvDetails_Tab.Rows[rowIndex].Cells["LAMACAN"] != null)
                    {
                        string laMaCan = dgvDetails_Tab.Rows[rowIndex].Cells["LAMACAN"].Value.ToString();
                        if (laMaCan.Equals("True")) CURRENT_ROW_SOLUONG_NEW = CURRENT_ROW_SOLUONG - CURRENT_SOLUONG_MACAN;
                        else
                        {
                            CURRENT_ROW_SOLUONG_NEW = CURRENT_ROW_SOLUONG - 1;
                        }
                    }
                    else
                    {
                        CURRENT_ROW_SOLUONG_NEW = CURRENT_ROW_SOLUONG - 1;
                    }

                    if (CURRENT_ROW_SOLUONG_NEW > 0)
                    {
                        CURRENT_TIENKHUYENMAI_NEW = (CURRENT_ROW_GIATRI / CURRENT_ROW_SOLUONG) * CURRENT_ROW_SOLUONG_NEW;
                        dgvDetails_Tab.Rows[rowIndex].Cells["TIENKM"].Value =
                            FormatCurrency.FormatMoney(CURRENT_TIENKHUYENMAI_NEW);
                        dgvDetails_Tab.Rows[rowIndex].Cells["SOLUONG"].Value =
                            FormatCurrency.FormatMoney(CURRENT_ROW_SOLUONG_NEW);
                        if (CHIETKHAU <= 100)
                        {
                            dgvDetails_Tab.Rows[rowIndex].Cells["TTIENCOVAT"].Value =
                                FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLECOVAT * CURRENT_ROW_SOLUONG_NEW -
                                                           CURRENT_TIENKHUYENMAI_NEW -
                                                           CHIETKHAU * CURRENT_ROW_SOLUONG_NEW * CURRENT_ROW_GIABANLECOVAT /
                                                           100);
                        }
                        else
                        {
                            dgvDetails_Tab.Rows[rowIndex].Cells["TTIENCOVAT"].Value =
                                FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLECOVAT * CURRENT_ROW_SOLUONG_NEW -
                                                           CURRENT_TIENKHUYENMAI_NEW - CHIETKHAU * CURRENT_ROW_SOLUONG_NEW);
                            dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value = CHIETKHAU * CURRENT_ROW_SOLUONG_NEW;
                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Bạn muốn xóa mã này?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            int maxIndex = int.Parse(dgvDetails_Tab.Rows[0].Cells["STT"].Value.ToString());
                            string maVatTu = dgvDetails_Tab.Rows[rowIndex].Cells["MAVATTU"].Value.ToString();
                            dgvDetails_Tab.Rows.RemoveAt(rowIndex);
                            if (rowIndex > 0)
                            {
                                for (int i = 0; i < rowIndex; i++) // đánh lại số thứ tự trên dgv.
                                {
                                    maxIndex = maxIndex - 1;
                                    dgvDetails_Tab.Rows[i].Cells[0].Value = maxIndex;
                                }
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
                        if (this.dgvDetails_Tab.Rows.Count > 0)
                        {
                            this.dgvDetails_Tab.Sort(this.dgvDetails_Tab.Columns["STT"], ListSortDirection.Descending);
                            this.dgvDetails_Tab.ClearSelection();
                            this.dgvDetails_Tab.Rows[0].Selected = true;
                        }
                    }
                    txtSoLuong.Text = CURRENT_ROW_SOLUONG_NEW.ToString();
                    if (this.dgvDetails_Tab.Rows.Count > 0)
                    {
                        HienThiManHinhLCD(dgvDetails_Tab.CurrentRow.Cells["TENVATTU"].Value.ToString(), FormatCurrency.FormatMoney(CURRENT_ROW_SOLUONG_NEW), FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLECOVAT), FormatCurrency.FormatMoney(CURRENT_ROW_SOLUONG_NEW * CURRENT_ROW_GIABANLECOVAT));
                    }
                    else
                    {
                        HienThiManHinhLCD("", "0", "0", "0");
                    }
                    txtMaHang.Text = "";
                }
                TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
                lblChietKhauLe.Text = FormatCurrency.FormatMoney(SUM_TONGCHIETKHAU_LE);
            }
        }


        /// <summary>
        /// Sự kiện tăng hàng
        /// </summary>
        private void TangHangGridView()
        {
            if (dgvDetails_Tab.Rows.Count > 0)
            {
                if (CurrentKey == Keys.F3 || (CurrentKey == Keys.F2))
                {
                    CurrentKey = Keys.F4;
                    btnStatus.Text = "+ TĂNG HÀNG";
                    btnStatus.BackColor = Color.DarkTurquoise;
                }
                else
                {
                    decimal SUM_TONGCHIETKHAU_LE = 0;
                    decimal CURRENT_ROW_SOLUONG_NEW = 0;
                    decimal CURRENT_TONCUOIKYSL = 0;
                    decimal CURRENT_ROW_SOLUONG,
                        CURRENT_ROW_GIABANLECOVAT,
                        CURRENT_TYLEKHUYENMAI_NEW,
                        CURRENT_TIENKHUYENMAI_NEW,
                        CURRENT_ROW_GIATRI,
                        CHIETKHAU = 0;
                    int rowIndex = dgvDetails_Tab.SelectedRows[0].Index;
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["GIABANLECOVAT"].Value.ToString(),
                        out CURRENT_ROW_GIABANLECOVAT);
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["SOLUONG"].Value.ToString(),
                        out CURRENT_ROW_SOLUONG);
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["TIENKM"].Value.ToString(),
                        out CURRENT_ROW_GIATRI);
                    if (dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value != null)
                    {
                        string CHIETKHAU_PERCENT = dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value.ToString().Trim();
                        if (CHIETKHAU_PERCENT.Contains('%'))
                        {
                            CHIETKHAU_PERCENT = CHIETKHAU_PERCENT.Remove(CHIETKHAU_PERCENT.Length - 1);
                            decimal.TryParse(CHIETKHAU_PERCENT.ToString(), out CHIETKHAU);
                        }
                    }
                    else
                    {
                        dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value = CHIETKHAU;
                    }
                    CurrentKey = Keys.F4;
                    btnStatus.Text = "+ TĂNG HÀNG";
                    btnStatus.BackColor = Color.DarkTurquoise;
                    decimal.TryParse(dgvDetails_Tab.Rows[rowIndex].Cells["TONCUOIKYSL"].Value.ToString(), out CURRENT_TONCUOIKYSL);
                    if (dgvDetails_Tab.Rows[rowIndex].Cells["LAMACAN"] != null)
                    {
                        string laMaCan = dgvDetails_Tab.Rows[rowIndex].Cells["LAMACAN"].Value.ToString();
                        if (laMaCan.Equals("True"))
                        {
                            if (CURRENT_ROW_SOLUONG + CURRENT_SOLUONG_MACAN > CURRENT_TONCUOIKYSL)
                            {
                                if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_ORACLE() == 1)
                                {
                                    NotificationLauncher.ShowNotificationWarning("Thông báo", "Hết hàng trong kho ! Không thể bán", 1, "0x1", "0x8", "normal");
                                    return;
                                }
                            }
                            CURRENT_ROW_SOLUONG_NEW = CURRENT_ROW_SOLUONG + CURRENT_SOLUONG_MACAN;
                        }
                        else
                        {
                            if (CURRENT_ROW_SOLUONG + 1 > CURRENT_TONCUOIKYSL)
                            {
                                if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_ORACLE() == 1)
                                {
                                    NotificationLauncher.ShowNotificationWarning("Thông báo", "Hết hàng trong kho ! Không thể bán", 1, "0x1", "0x8", "normal");
                                    return;
                                }
                            }
                            CURRENT_ROW_SOLUONG_NEW = CURRENT_ROW_SOLUONG + 1;
                        }
                    }
                    else
                    {
                        if (CURRENT_ROW_SOLUONG + 1 > CURRENT_TONCUOIKYSL)
                        {
                            if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_ORACLE() == 1)
                            {
                                NotificationLauncher.ShowNotificationWarning("Thông báo", "Hết hàng trong kho ! Không thể bán", 1, "0x1", "0x8", "normal");
                                return;
                            }
                        }
                        CURRENT_ROW_SOLUONG_NEW = CURRENT_ROW_SOLUONG + 1;
                    }
                    dgvDetails_Tab.Rows[rowIndex].Cells["SOLUONG"].Value = FormatCurrency.FormatMoney(CURRENT_ROW_SOLUONG_NEW);
                    CURRENT_TYLEKHUYENMAI_NEW = 100 * ((CURRENT_ROW_GIATRI / CURRENT_ROW_SOLUONG) * CURRENT_ROW_SOLUONG_NEW) / (CURRENT_ROW_GIABANLECOVAT * CURRENT_ROW_SOLUONG_NEW);
                    CURRENT_TIENKHUYENMAI_NEW = (CURRENT_ROW_GIATRI / CURRENT_ROW_SOLUONG) * CURRENT_ROW_SOLUONG_NEW;
                    dgvDetails_Tab.Rows[rowIndex].Cells["TIENKM"].Value = FormatCurrency.FormatMoney(CURRENT_TIENKHUYENMAI_NEW);
                    if (CHIETKHAU <= 100)
                    {
                        dgvDetails_Tab.Rows[rowIndex].Cells["TTIENCOVAT"].Value =
                            FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLECOVAT * CURRENT_ROW_SOLUONG_NEW - CURRENT_TIENKHUYENMAI_NEW - CHIETKHAU * CURRENT_ROW_SOLUONG_NEW * CURRENT_ROW_GIABANLECOVAT / 100);
                    }
                    else
                    {
                        dgvDetails_Tab.Rows[rowIndex].Cells["TTIENCOVAT"].Value =
                            FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLECOVAT * CURRENT_ROW_SOLUONG_NEW -
                                                       CURRENT_TIENKHUYENMAI_NEW - CHIETKHAU * CURRENT_ROW_SOLUONG_NEW);
                        dgvDetails_Tab.Rows[rowIndex].Cells["CHIETKHAU"].Value = CHIETKHAU * CURRENT_ROW_SOLUONG_NEW;
                    }
                    TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
                    if (this.dgvDetails_Tab.Rows.Count > 0)
                    {
                        HienThiManHinhLCD(dgvDetails_Tab.Rows[rowIndex].Cells["TENVATTU"].Value.ToString(), FormatCurrency.FormatMoney(CURRENT_ROW_SOLUONG_NEW), FormatCurrency.FormatMoney(CURRENT_ROW_GIABANLECOVAT), FormatCurrency.FormatMoney(CURRENT_ROW_SOLUONG_NEW * CURRENT_ROW_GIABANLECOVAT));
                    }
                    else
                    {
                        HienThiManHinhLCD("", "0", "0", "0");
                    }
                    lblChietKhauLe.Text = FormatCurrency.FormatMoney(SUM_TONGCHIETKHAU_LE);
                    txtSoLuong.Text = CURRENT_ROW_SOLUONG_NEW.ToString();
                }
            }
        }

        private void txtChietKhauToanDon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                decimal TONGTIEN_SAUCT = 0;
                decimal TONGTIEN_HOADON = 0;
                decimal CHIETKHAU_TOANDON = 0;
                decimal.TryParse(lblTongTienThanhToan.Text.ToString(), out TONGTIEN_HOADON);
                decimal.TryParse(txtChietKhauToanDon.Text.ToString(), out CHIETKHAU_TOANDON);
                if (TONGTIEN_HOADON > 0 && dgvDetails_Tab.RowCount > 0)
                {
                    decimal SUM_TONGCHIETKHAU_LE = 0;
                    if (CHIETKHAU_TOANDON <= 100) //Phần trăm chiết khấu
                    {
                        TONGTIEN_SAUCT = TONGTIEN_HOADON - TONGTIEN_HOADON * CHIETKHAU_TOANDON / 100;
                        lblTongTienThanhToan.Text = FormatCurrency.FormatMoney(TONGTIEN_SAUCT);
                        if (dgvDetails_Tab.RowCount > 0)
                        {
                            foreach (DataGridViewRow rowData in dgvDetails_Tab.Rows)
                            {
                                decimal CURRENT_SOLUONG = 0; decimal.TryParse(rowData.Cells["SOLUONG"].Value.ToString(), out CURRENT_SOLUONG);
                                decimal CURRENT_GIABANLECOVAT = 0; decimal.TryParse(rowData.Cells["GIABANLECOVAT"].Value.ToString(), out CURRENT_GIABANLECOVAT);
                                decimal CURRENT_TIENKM = 0; decimal.TryParse(rowData.Cells["TIENKM"].Value.ToString(), out CURRENT_TIENKM);
                                decimal TYLE_CHIETKHAU_TOANDON = decimal.Round(CHIETKHAU_TOANDON / 100, 2);
                                decimal TIEN_CHIETKHAU_TOANDON = TYLE_CHIETKHAU_TOANDON * (CURRENT_SOLUONG * CURRENT_GIABANLECOVAT - CURRENT_TIENKM);
                                decimal TIEN_TTIENCOVAT = (CURRENT_SOLUONG * CURRENT_GIABANLECOVAT - CURRENT_TIENKM) * (1 - TYLE_CHIETKHAU_TOANDON);
                                rowData.Cells["CHIETKHAU"].Value = (TYLE_CHIETKHAU_TOANDON * 100) + "%";
                                rowData.Cells["TTIENCOVAT"].Value = FormatCurrency.FormatMoney(TIEN_TTIENCOVAT);
                                SUM_TONGCHIETKHAU_LE += TIEN_CHIETKHAU_TOANDON;
                            }
                        }
                    }
                    else
                    {
                        if (dgvDetails_Tab.RowCount > 0)
                        {
                            foreach (DataGridViewRow rowData in dgvDetails_Tab.Rows)
                            {
                                decimal CURRENT_SOLUONG = 0; decimal.TryParse(rowData.Cells["SOLUONG"].Value.ToString(), out CURRENT_SOLUONG);
                                decimal CURRENT_GIABANLECOVAT = 0; decimal.TryParse(rowData.Cells["GIABANLECOVAT"].Value.ToString(), out CURRENT_GIABANLECOVAT);
                                decimal CURRENT_TIENKM = 0; decimal.TryParse(rowData.Cells["TIENKM"].Value.ToString(), out CURRENT_TIENKM);
                                decimal TYLE_CHIETKHAU_TOANDON = decimal.Round(CHIETKHAU_TOANDON / TONGTIEN_HOADON, 2);
                                decimal TIEN_CHIETKHAU_TOANDON = CHIETKHAU_TOANDON;
                                decimal TIEN_TTIENCOVAT = (CURRENT_SOLUONG * CURRENT_GIABANLECOVAT - CURRENT_TIENKM) * (1 - TYLE_CHIETKHAU_TOANDON);
                                rowData.Cells["CHIETKHAU"].Value = (TYLE_CHIETKHAU_TOANDON * 100) + "%";
                                rowData.Cells["TTIENCOVAT"].Value = FormatCurrency.FormatMoney(TIEN_TTIENCOVAT);
                                SUM_TONGCHIETKHAU_LE += TIEN_CHIETKHAU_TOANDON;
                            }
                        }
                    }
                    lblChietKhauLe.Text = FormatCurrency.FormatMoney(SUM_TONGCHIETKHAU_LE);
                }
            }
            TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
        }

        private void btnF2ThemMoi_Click(object sender, EventArgs e)
        {
            lblTongTienThanhToan.Text = "";
            lblTongTienKhuyenMai.Text = "";
            lblSumSoLuong.Text = "";
            dgvDetails_Tab.Rows.Clear();
            dgvDetails_Tab.Refresh();
            CurrentKey = Keys.F2;
            btnF2ThemMoi.Enabled = false;
            btnF2ThemMoi.BackColor = Color.Gray;
            lblMaGiaoDichQuay.Text = FrmXuatBanLeService.INIT_CODE_TRADE();
            txtMaHang.Enabled = true;
            txtMaHang.Focus();
            btnF1ThanhToan.Enabled = true;
            btnF1ThanhToan.BackColor = Color.CadetBlue;
            btnF3GiamHang.Enabled = true;
            btnF3GiamHang.BackColor = Color.CadetBlue;
            btnF4TangHang.Enabled = true;
            btnF4TangHang.BackColor = Color.CadetBlue;
            btnF5LamMoi.Enabled = true;
            btnF5LamMoi.BackColor = Color.CadetBlue;
            btnSearchAll.Enabled = true;
            btnSearchAll.BackColor = Color.CadetBlue;
        }

        private void btnF3GiamHang_Click(object sender, EventArgs e)
        {
            if (!btnF2ThemMoi.Enabled && dgvDetails_Tab.RowCount > 0)
            {
                FlagTangHang = false;
                GiamHangGridView();
            }
        }

        private void btnF4TangHang_Click(object sender, EventArgs e)
        {
            if (!btnF2ThemMoi.Enabled && dgvDetails_Tab.RowCount > 0)
            {
                FlagTangHang = true;
                TangHangGridView();
            }
        }

        private void btnF5LamMoi_Click(object sender, EventArgs e)
        {
            CurrentKey = Keys.F4;
            btnStatus.Text = "+ TĂNG HÀNG";
            btnStatus.BackColor = Color.DarkTurquoise;
            FlagTangHang = true;
            if (dgvDetails_Tab.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Thao tác này sẽ xóa toàn bộ mã hàng đang scan ! Bạn có chắc chắn ?", "Warning",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    dgvDetails_Tab.Rows.Clear();
                    HienThiManHinhLCD("", "0", "0", "0");
                    TINHTOAN_TONGTIEN_TOANHOADON(dgvDetails_Tab);
                }
                else if (result == DialogResult.No)
                {
                    //code for No
                }
                else if (result == DialogResult.Cancel)
                {
                    //code for Cancel
                }
                lblChietKhauLe.Text = "0";
                txtMaHang.Focus();
            }
        }

        private void btnSearchAll_Click_1(object sender, EventArgs e)
        {
            frmTimKiemVatTu frm = new frmTimKiemVatTu();
            frm.handlerSearchVatTu(SelectedSearch);
            frm.ShowDialog();
        }

        private void btnF7_Click(object sender, EventArgs e)
        {
            frmSearch = new FrmTimKiemGiaoDich(true);
            frmSearch.SetHandlerBill(TimKIemGiaoDichQuayBanLe);
            frmSearch.ShowDialog();
        }
        public NVGDQUAY_ASYNCCLIENT_DTO KHOITAO_DULIEU_INLAI_HOADON_FROM_ORACLE(string MAGIAODICHQUAYPK)
        {
            NVGDQUAY_ASYNCCLIENT_DTO _DATA_INLAI_HOADON = new NVGDQUAY_ASYNCCLIENT_DTO();
            OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string KieuThanhToan = null;
                    OracleCommand cmdParent = new OracleCommand();
                    cmdParent.Connection = connection;
                    cmdParent.CommandText = string.Format(@"SELECT * FROM NVGDQUAY_ASYNCCLIENT WHERE MAGIAODICHQUAYPK = :MAGIAODICHQUAYPK AND UNITCODE = :UNITCODE");
                    cmdParent.CommandType = CommandType.Text;
                    cmdParent.Parameters.Add("MAGIAODICHQUAYPK", OracleDbType.NVarchar2, 50).Value = MAGIAODICHQUAYPK.Trim();
                    cmdParent.Parameters.Add("UNITCODE", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                    OracleDataReader dataReaderParent = null;
                    dataReaderParent = cmdParent.ExecuteReader();
                    if (dataReaderParent.HasRows)
                    {
                        while (dataReaderParent.Read())
                        {
                            _DATA_INLAI_HOADON.MAGIAODICH = dataReaderParent["MAGIAODICH"].ToString();
                            _DATA_INLAI_HOADON.MAGIAODICHQUAYPK = dataReaderParent["MAGIAODICHQUAYPK"].ToString();
                            if (dataReaderParent["NGAYTAO"] != null)
                            {
                                string NGAYTAO = dataReaderParent["NGAYTAO"].ToString();
                                _DATA_INLAI_HOADON.NGAYTAO = DateTime.Parse(NGAYTAO);
                            }
                            else
                            {
                                _DATA_INLAI_HOADON.NGAYTAO = DateTime.Now;
                            }
                            if (dataReaderParent["NGAYPHATSINH"] != null)
                            {
                                string NGAYPHATSINH = dataReaderParent["NGAYPHATSINH"].ToString();
                                _DATA_INLAI_HOADON.NGAYPHATSINH = DateTime.Parse(NGAYPHATSINH);
                            }
                            else
                            {
                                _DATA_INLAI_HOADON.NGAYPHATSINH = DateTime.Now;
                            }
                            _DATA_INLAI_HOADON.MANGUOITAO = dataReaderParent["MANGUOITAO"] != null ? dataReaderParent["MANGUOITAO"].ToString() : "";
                            _DATA_INLAI_HOADON.NGUOITAO = dataReaderParent["NGUOITAO"] != null ? dataReaderParent["NGUOITAO"].ToString() : "";
                            _DATA_INLAI_HOADON.MAQUAYBAN = dataReaderParent["MAQUAYBAN"] != null ? dataReaderParent["MAQUAYBAN"].ToString() : "";
                            decimal TIENKHACHDUA = 0; decimal.TryParse(dataReaderParent["TIENKHACHDUA"].ToString(), out TIENKHACHDUA);
                            _DATA_INLAI_HOADON.TIENKHACHDUA = TIENKHACHDUA;
                            decimal TIENTRALAI = 0; decimal.TryParse(dataReaderParent["TIENTRALAI"].ToString(), out TIENTRALAI);
                            _DATA_INLAI_HOADON.TIENTRALAI = TIENTRALAI;
                            decimal TTIENCOVAT = 0; decimal.TryParse(dataReaderParent["TTIENCOVAT"].ToString(), out TTIENCOVAT);
                            _DATA_INLAI_HOADON.TTIENCOVAT = TTIENCOVAT;
                            decimal TIENMAT = 0; decimal.TryParse(dataReaderParent["TIENMAT"].ToString(), out TIENMAT);
                            _DATA_INLAI_HOADON.TIENMAT = TIENMAT;
                        }
                    }
                    OracleCommand cmdChildren = new OracleCommand();
                    cmdChildren.Connection = connection;
                    cmdChildren.CommandText = string.Format(@"SELECT * FROM NVHANGGDQUAY_ASYNCCLIENT WHERE MAGDQUAYPK = :MAGDQUAYPK AND MADONVI = :MADONVI");
                    cmdChildren.CommandType = CommandType.Text;
                    cmdChildren.Parameters.Add("MAGDQUAYPK", OracleDbType.NVarchar2, 50).Value = MAGIAODICHQUAYPK;
                    cmdChildren.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                    OracleDataReader dataReader = null;
                    dataReader = cmdChildren.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        List<BOHANG_DTO> listBoHang = new List<BOHANG_DTO>();
                        while (dataReader.Read())
                        {
                            decimal GIABANLEVAT, VATBAN, TIENCHIETKHAU, TYLECHIETKHAU, TYLEKHUYENMAI, TIENKHUYENMAI, TYLEVOUCHER, TIENVOUCHER, TYLELAILE, GIAVON, TTIENCOVAT, SOLUONG = 0;
                            NVHANGGDQUAY_ASYNCCLIENT item = new NVHANGGDQUAY_ASYNCCLIENT();
                            string MaBoHangPk = dataReader["MABOPK"].ToString().Trim();
                            if (!string.IsNullOrEmpty(MaBoHangPk) && !MaBoHangPk.Equals("BH"))
                            {
                                decimal SOLUONG_BOHANG, GIABANLECOVAT_BOHANG, THANHTIENCOVAT_BOHANG = 0;
                                string MaVatTu = dataReader["MAVATTU"].ToString().ToUpper().Trim();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG_BOHANG);
                                decimal.TryParse(dataReader["GIABANLECOVAT"].ToString(), out GIABANLECOVAT_BOHANG);
                                decimal.TryParse(dataReader["TTIENCOVAT"].ToString(), out THANHTIENCOVAT_BOHANG);
                                List<BOHANG_DETAILS_DTO> listBoHangMatHangExist = new List<BOHANG_DETAILS_DTO>();
                                BOHANG_DTO boHangExist = listBoHang.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk));
                                if (boHangExist == null)
                                {
                                    BOHANG_DTO boHang = new BOHANG_DTO();
                                    boHang.MABOHANG = MaBoHangPk;
                                    boHang.TTIENCOVAT = THANHTIENCOVAT_BOHANG;
                                    boHang.TONGSL = SOLUONG_BOHANG;
                                    BOHANG_DETAILS_DTO MatHangExist = listBoHangMatHangExist.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAVATTU.Equals(MaVatTu));
                                    if (MatHangExist == null)
                                    {
                                        BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                        mathang.MABOHANG = MaBoHangPk;
                                        mathang.MAVATTU = MaVatTu;
                                        mathang.SOLUONG = SOLUONG_BOHANG;
                                        mathang.GIABANLECOVAT = GIABANLECOVAT_BOHANG;
                                        mathang.TTIENCOVAT = THANHTIENCOVAT_BOHANG;
                                        boHang.MATHANG_BOHANG.Add(mathang);
                                    }
                                    listBoHang.Add(boHang);
                                }
                                else
                                {
                                    boHangExist.TTIENCOVAT += THANHTIENCOVAT_BOHANG;
                                    boHangExist.TONGSL += SOLUONG_BOHANG;
                                    BOHANG_DETAILS_DTO MatHangExist = boHangExist.MATHANG_BOHANG.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAVATTU.Equals(MaVatTu));
                                    if (MatHangExist == null)
                                    {
                                        BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                        mathang.MABOHANG = MaBoHangPk;
                                        mathang.MAVATTU = MaVatTu;
                                        mathang.SOLUONG = SOLUONG_BOHANG;
                                        mathang.GIABANLECOVAT = GIABANLECOVAT_BOHANG;
                                        mathang.TTIENCOVAT = THANHTIENCOVAT_BOHANG;
                                        boHangExist.MATHANG_BOHANG.Add(mathang);
                                    }
                                }
                            }
                            else
                            {
                                item.MAVATTU = dataReader["MAVATTU"].ToString();
                                item.TENDAYDU = dataReader["TENDAYDU"].ToString();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                item.SOLUONG = SOLUONG;
                                decimal.TryParse(dataReader["GIABANLECOVAT"].ToString(), out GIABANLEVAT);
                                item.GIABANLECOVAT = GIABANLEVAT;
                                decimal.TryParse(dataReader["VATBAN"].ToString(), out VATBAN);
                                item.VATBAN = VATBAN;
                                decimal.TryParse(dataReader["TIENCHIETKHAU"].ToString(), out TIENCHIETKHAU);
                                item.TIENCHIETKHAU = TIENCHIETKHAU;
                                decimal.TryParse(dataReader["TYLECHIETKHAU"].ToString(), out TYLECHIETKHAU);
                                item.TYLECHIETKHAU = TYLECHIETKHAU;
                                decimal.TryParse(dataReader["TYLEKHUYENMAI"].ToString(), out TYLEKHUYENMAI);
                                item.TYLEKHUYENMAI = TYLEKHUYENMAI;
                                decimal.TryParse(dataReader["TIENKHUYENMAI"].ToString(), out TIENKHUYENMAI);
                                item.TIENKHUYENMAI = TIENKHUYENMAI;
                                decimal.TryParse(dataReader["TTIENCOVAT"].ToString(), out TTIENCOVAT);
                                item.TTIENCOVAT = TTIENCOVAT;
                                item.MAVAT = dataReader["MAVAT"].ToString();
                                _DATA_INLAI_HOADON.LST_DETAILS.Add(item);
                            }
                        }
                        //Add mã bó hàng vào list
                        if (listBoHang.Count > 0)
                        {
                            foreach (BOHANG_DTO row in listBoHang)
                            {
                                decimal SOLUONG_BOHANG_BAN = 0;
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
                                decimal.TryParse((row.TONGSL / SUM_SOLUONG_BO).ToString(), out SOLUONG_BOHANG_BAN);
                                item.SOLUONG = SOLUONG_BOHANG_BAN;
                                item.GIABANLECOVAT = TONGLE;
                                EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();
                                _EXTEND_VAT_BOHANG = FrmXuatBanLeService.LAYDULIEU_VAT_BOHANG_FROM_DATABASE_ORACLE(row.MABOHANG, Session.Session.CurrentUnitCode);
                                item.VATBAN = _EXTEND_VAT_BOHANG.TYLEVATRA;
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
                                item.MAVAT = _EXTEND_VAT_BOHANG.MAVATRA;
                                _DATA_INLAI_HOADON.LST_DETAILS.Add(item);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return _DATA_INLAI_HOADON;
        }


        public NVGDQUAY_ASYNCCLIENT_DTO KHOITAO_DULIEU_INLAI_HOADON_FROM_SQLSERVER(string MAGIAODICHQUAYPK)
        {
            NVGDQUAY_ASYNCCLIENT_DTO _DATA_INLAI_HOADON = new NVGDQUAY_ASYNCCLIENT_DTO();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string KieuThanhToan = null;
                    SqlCommand cmdParent = new SqlCommand();
                    cmdParent.Connection = connection;
                    cmdParent.CommandText = string.Format(@"SELECT [MAGIAODICH],[MAGIAODICHQUAYPK],[MADONVI],[NGAYTAO],[NGAYPHATSINH],[MANGUOITAO],[NGUOITAO],[MAQUAYBAN],[LOAIGIAODICH]
                    ,[HINHTHUCTHANHTOAN],[TIENKHACHDUA],[TIENVOUCHER],[TIENTHEVIP],[TIENTRALAI],[TIENTHE],[TIENCOD],[TIENMAT],[TTIENCOVAT],[THOIGIAN],[MAKHACHHANG],[UNITCODE] FROM [dbo].[NVGDQUAY_ASYNCCLIENT] WHERE MAGIAODICHQUAYPK = @MAGIAODICHQUAYPK AND UNITCODE = @UNITCODE");
                    cmdParent.CommandType = CommandType.Text;
                    cmdParent.Parameters.Add("MAGIAODICHQUAYPK", SqlDbType.NVarChar, 50).Value = MAGIAODICHQUAYPK.Trim();
                    cmdParent.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentUnitCode;
                    SqlDataReader dataReaderParent = null;
                    dataReaderParent = cmdParent.ExecuteReader();
                    if (dataReaderParent.HasRows)
                    {
                        while (dataReaderParent.Read())
                        {
                            _DATA_INLAI_HOADON.MAGIAODICH = dataReaderParent["MAGIAODICH"].ToString();
                            _DATA_INLAI_HOADON.MAGIAODICHQUAYPK = dataReaderParent["MAGIAODICHQUAYPK"].ToString();
                            if (dataReaderParent["NGAYTAO"] != null)
                            {
                                string NGAYTAO = dataReaderParent["NGAYTAO"].ToString();
                                _DATA_INLAI_HOADON.NGAYTAO = DateTime.Parse(NGAYTAO);
                            }
                            else
                            {
                                _DATA_INLAI_HOADON.NGAYTAO = DateTime.Now;
                            }
                            if (dataReaderParent["NGAYPHATSINH"] != null)
                            {
                                string NGAYPHATSINH = dataReaderParent["NGAYPHATSINH"].ToString();
                                _DATA_INLAI_HOADON.NGAYPHATSINH = DateTime.Parse(NGAYPHATSINH);
                            }
                            else
                            {
                                _DATA_INLAI_HOADON.NGAYPHATSINH = DateTime.Now;
                            }
                            _DATA_INLAI_HOADON.MANGUOITAO = dataReaderParent["MANGUOITAO"] != null ? dataReaderParent["MANGUOITAO"].ToString() : "";
                            _DATA_INLAI_HOADON.NGUOITAO = dataReaderParent["NGUOITAO"] != null ? dataReaderParent["NGUOITAO"].ToString() : "";
                            _DATA_INLAI_HOADON.MAQUAYBAN = dataReaderParent["MAQUAYBAN"] != null ? dataReaderParent["MAQUAYBAN"].ToString() : "";
                            decimal TIENKHACHDUA = 0; decimal.TryParse(dataReaderParent["TIENKHACHDUA"].ToString(), out TIENKHACHDUA);
                            _DATA_INLAI_HOADON.TIENKHACHDUA = TIENKHACHDUA;
                            decimal TIENTRALAI = 0; decimal.TryParse(dataReaderParent["TIENTRALAI"].ToString(), out TIENTRALAI);
                            _DATA_INLAI_HOADON.TIENTRALAI = TIENTRALAI;
                            decimal TTIENCOVAT = 0; decimal.TryParse(dataReaderParent["TTIENCOVAT"].ToString(), out TTIENCOVAT);
                            _DATA_INLAI_HOADON.TTIENCOVAT = TTIENCOVAT;
                            decimal TIENMAT = 0; decimal.TryParse(dataReaderParent["TIENMAT"].ToString(), out TIENMAT);
                            _DATA_INLAI_HOADON.TIENMAT = TIENMAT;
                            _DATA_INLAI_HOADON.UNITCODE = dataReaderParent["UNITCODE"] != null ? dataReaderParent["UNITCODE"].ToString() : Session.Session.CurrentUnitCode;
                        }
                    }
                    SqlCommand cmdChildren = new SqlCommand();
                    cmdChildren.Connection = connection;
                    cmdChildren.CommandText = string.Format(@"SELECT [MAGDQUAYPK],[MAKHOHANG],[MADONVI],[MAVATTU],[MANGUOITAO],[NGUOITAO],[MABOPK],[NGAYTAO],[NGAYPHATSINH] ,[SOLUONG],[TTIENCOVAT],[GIABANLECOVAT],[TYLECHIETKHAU]
                    ,[TIENCHIETKHAU],[TYLEKHUYENMAI],[TIENKHUYENMAI],[TYLEVOUCHER],[TIENVOUCHER],[TYLELAILE],[GIAVON],[MAVAT],[VATBAN],[MACHUONGTRINHKM],[UNITCODE] FROM [dbo].[NVHANGGDQUAY_ASYNCCLIENT] WHERE MAGDQUAYPK = @MAGDQUAYPK AND MADONVI = @MADONVI");
                    cmdChildren.CommandType = CommandType.Text;
                    cmdChildren.Parameters.Add("MAGDQUAYPK", SqlDbType.NVarChar, 50).Value = MAGIAODICHQUAYPK;
                    cmdChildren.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value =
                        Session.Session.CurrentUnitCode;
                    SqlDataReader dataReader = null;
                    dataReader = cmdChildren.ExecuteReader();
                    if (dataReader.HasRows)
                    {
                        List<BOHANG_DTO> listBoHang = new List<BOHANG_DTO>();
                        while (dataReader.Read())
                        {
                            decimal GIABANLEVAT, VATBAN, TIENCHIETKHAU, TYLECHIETKHAU, TYLEKHUYENMAI, TIENKHUYENMAI, TTIENCOVAT, SOLUONG = 0;
                            NVHANGGDQUAY_ASYNCCLIENT item = new NVHANGGDQUAY_ASYNCCLIENT();
                            string MaBoHangPk = dataReader["MABOPK"].ToString().Trim();
                            if (!string.IsNullOrEmpty(MaBoHangPk) && !MaBoHangPk.Equals("BH"))
                            {
                                decimal SOLUONG_BOHANG, GIABANLECOVAT_BOHANG, THANHTIENCOVAT_BOHANG = 0;
                                string MaVatTu = dataReader["MAVATTU"].ToString().ToUpper().Trim();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG_BOHANG);
                                decimal.TryParse(dataReader["GIABANLECOVAT"].ToString(), out GIABANLECOVAT_BOHANG);
                                decimal.TryParse(dataReader["TTIENCOVAT"].ToString(), out THANHTIENCOVAT_BOHANG);
                                List<BOHANG_DETAILS_DTO> listBoHangMatHangExist = new List<BOHANG_DETAILS_DTO>();
                                BOHANG_DTO boHangExist = listBoHang.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk));
                                if (boHangExist == null)
                                {
                                    BOHANG_DTO boHang = new BOHANG_DTO();
                                    boHang.MABOHANG = MaBoHangPk;
                                    boHang.TTIENCOVAT = THANHTIENCOVAT_BOHANG;
                                    boHang.TONGSL = SOLUONG_BOHANG;
                                    BOHANG_DETAILS_DTO MatHangExist = listBoHangMatHangExist.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAVATTU.Equals(MaVatTu));
                                    if (MatHangExist == null)
                                    {
                                        BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                        mathang.MABOHANG = MaBoHangPk;
                                        mathang.MAVATTU = MaVatTu;
                                        mathang.SOLUONG = SOLUONG_BOHANG;
                                        mathang.GIABANLECOVAT = GIABANLECOVAT_BOHANG;
                                        mathang.TTIENCOVAT = THANHTIENCOVAT_BOHANG;
                                        boHang.MATHANG_BOHANG.Add(mathang);
                                    }
                                    listBoHang.Add(boHang);
                                }
                                else
                                {
                                    boHangExist.TTIENCOVAT += THANHTIENCOVAT_BOHANG;
                                    boHangExist.TONGSL += SOLUONG_BOHANG;
                                    BOHANG_DETAILS_DTO MatHangExist = boHangExist.MATHANG_BOHANG.FirstOrDefault(x => x.MABOHANG.Equals(MaBoHangPk) && x.MAVATTU.Equals(MaVatTu));
                                    if (MatHangExist == null)
                                    {
                                        BOHANG_DETAILS_DTO mathang = new BOHANG_DETAILS_DTO();
                                        mathang.MABOHANG = MaBoHangPk;
                                        mathang.MAVATTU = MaVatTu;
                                        mathang.SOLUONG = SOLUONG_BOHANG;
                                        mathang.GIABANLECOVAT = GIABANLECOVAT_BOHANG;
                                        mathang.TTIENCOVAT = THANHTIENCOVAT_BOHANG;
                                        boHangExist.MATHANG_BOHANG.Add(mathang);
                                    }
                                }
                            }
                            else
                            {
                                item.MAVATTU = dataReader["MAVATTU"].ToString();
                                EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
                                _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(item.MAVATTU, _DATA_INLAI_HOADON.UNITCODE);
                                item.TENDAYDU = _EXTEND_VATTU_DTO.TENVATTU;
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                item.SOLUONG = SOLUONG;
                                decimal.TryParse(dataReader["GIABANLECOVAT"].ToString(), out GIABANLEVAT);
                                item.GIABANLECOVAT = GIABANLEVAT;
                                decimal.TryParse(dataReader["VATBAN"].ToString(), out VATBAN);
                                item.VATBAN = VATBAN;
                                decimal.TryParse(dataReader["TIENCHIETKHAU"].ToString(), out TIENCHIETKHAU);
                                item.TIENCHIETKHAU = TIENCHIETKHAU;
                                decimal.TryParse(dataReader["TYLECHIETKHAU"].ToString(), out TYLECHIETKHAU);
                                item.TYLECHIETKHAU = TYLECHIETKHAU;
                                decimal.TryParse(dataReader["TYLEKHUYENMAI"].ToString(), out TYLEKHUYENMAI);
                                item.TYLEKHUYENMAI = TYLEKHUYENMAI;
                                decimal.TryParse(dataReader["TIENKHUYENMAI"].ToString(), out TIENKHUYENMAI);
                                item.TIENKHUYENMAI = TIENKHUYENMAI;
                                decimal.TryParse(dataReader["TTIENCOVAT"].ToString(), out TTIENCOVAT);
                                item.TTIENCOVAT = TTIENCOVAT;
                                item.MAVAT = dataReader["MAVAT"].ToString();
                                _DATA_INLAI_HOADON.LST_DETAILS.Add(item);
                            }
                        }
                        //Add mã bó hàng vào list
                        if (listBoHang.Count > 0)
                        {
                            foreach (BOHANG_DTO row in listBoHang)
                            {
                                decimal SOLUONG_BOHANG_BAN = 0;
                                NVHANGGDQUAY_ASYNCCLIENT item = new NVHANGGDQUAY_ASYNCCLIENT();
                                decimal TONGLE = 0;
                                decimal SUM_SOLUONG_BO = 0;
                                item.MAVATTU = row.MABOHANG;
                                SqlCommand commamdBoHang = new SqlCommand();
                                commamdBoHang.Connection = connection;
                                commamdBoHang.CommandText = string.Format(@"SELECT DM_BOHANG.MABOHANG,DM_BOHANG.TENBOHANG,SUM(DM_BOHANGCHITIET.SOLUONG) AS TONGSOLUONG,SUM(DM_BOHANGCHITIET.TONGLE) AS TONGLE FROM DM_BOHANG INNER JOIN DM_BOHANGCHITIET ON DM_BOHANG.MABOHANG = DM_BOHANGCHITIET.MABOHANG WHERE DM_BOHANG.MABOHANG = @MABOHANG AND DM_BOHANG.UNITCODE = @UNITCODE GROUP BY DM_BOHANG.MABOHANG,DM_BOHANG.TENBOHANG");
                                commamdBoHang.Parameters.Add("MABOHANG", SqlDbType.NVarChar, 50).Value = row.MABOHANG;
                                commamdBoHang.Parameters.Add("UNITCODE", SqlDbType.NVarChar, 50).Value = _DATA_INLAI_HOADON.UNITCODE;
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
                                decimal.TryParse((row.TONGSL / SUM_SOLUONG_BO).ToString(), out SOLUONG_BOHANG_BAN);
                                item.SOLUONG = SOLUONG_BOHANG_BAN;
                                item.GIABANLECOVAT = TONGLE;
                                EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();
                                _EXTEND_VAT_BOHANG = FrmXuatBanLeService.LAYDULIEU_VAT_BOHANG_FROM_DATABASE_SQLSERVER(row.MABOHANG, Session.Session.CurrentUnitCode);
                                item.VATBAN = _EXTEND_VAT_BOHANG.TYLEVATRA;
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
                                item.MAVAT = _EXTEND_VAT_BOHANG.MAVATRA;
                                _DATA_INLAI_HOADON.LST_DETAILS.Add(item);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return _DATA_INLAI_HOADON;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BillId"></param>
        /// <param name="toDate"></param>
        /// <param name="fromDate"></param>
        public void TimKIemGiaoDichQuayBanLe(string BillId, DateTime toDate, DateTime fromDate)
        {
            BILL_DTO headerBill = new BILL_DTO();
            string MaGiaoDichQuayPk = BillId.Trim() + "." + Session.Session.CurrentUnitCode.Split('-')[1];
            NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_BILL = new NVGDQUAY_ASYNCCLIENT_DTO();
            string MA_TEN_KHACHHANG = "";

            string msg = Config.CheckConnectToServer(out bool result);
            if (msg.Length > 0) { MessageBox.Show(msg); return; }

            if (result)
            {
                _NVGDQUAY_ASYNCCLIENT_BILL = KHOITAO_DULIEU_INLAI_HOADON_FROM_ORACLE(MaGiaoDichQuayPk);
                MA_TEN_KHACHHANG = FrmThanhToanService.LAY_MA_TEN_KHACHHANG_FROM_ORACLE(_NVGDQUAY_ASYNCCLIENT_BILL.MAKHACHHANG);
            }
            else
            {
                _NVGDQUAY_ASYNCCLIENT_BILL = KHOITAO_DULIEU_INLAI_HOADON_FROM_SQLSERVER(MaGiaoDichQuayPk);
                MA_TEN_KHACHHANG = FrmThanhToanService.LAY_MA_TEN_KHACHHANG_FROM_SQLSERVER(_NVGDQUAY_ASYNCCLIENT_BILL.MAKHACHHANG);
            }
            using (frmPrintInLaiBill frmInLai = new frmPrintInLaiBill())
            {
                try
                {
                    BILL_DTO infoBill = new BILL_DTO()
                    {
                        ADDRESS = Session.Session.CurrentAddress,
                        CONLAI = _NVGDQUAY_ASYNCCLIENT_BILL.TIENTRALAI,
                        PHONE = Session.Session.CurrentPhone,
                        MAKH = MA_TEN_KHACHHANG,
                        DIEM = 0,
                        INFOTHUNGAN = "THU NGÂN: " + _NVGDQUAY_ASYNCCLIENT_BILL.NGUOITAO + "\t QUẦY: " + _NVGDQUAY_ASYNCCLIENT_BILL.MAQUAYBAN,
                        MAGIAODICH = _NVGDQUAY_ASYNCCLIENT_BILL.MAGIAODICH,
                        THANHTIENCHU = ConvertSoThanhChu.ChuyenDoiSoThanhChu(_NVGDQUAY_ASYNCCLIENT_BILL.TTIENCOVAT),
                        TIENKHACHTRA = _NVGDQUAY_ASYNCCLIENT_BILL.TIENKHACHDUA,
                        QUAYHANG = _NVGDQUAY_ASYNCCLIENT_BILL.MAQUAYBAN,
                    };
                    frmInLai.PrintInvoice_BanLeInLai(infoBill, _NVGDQUAY_ASYNCCLIENT_BILL);
                }
                catch
                {
                }
                finally
                {
                    frmInLai.Close();
                    frmInLai.Dispose();
                    frmInLai.Refresh();
                }
            }
        }

        public void INSERT_VATTU_GRIDVIEW(int index)
        {
            VATTU_DTO vattu = new VATTU_DTO();
            vattu = listData_TrungMa[index];
            INSERT_DATA(vattu);
            frmSelect.Dispose();
        }

        private void INSERT_DATA(VATTU_DTO vattuDto)
        {
            decimal SoLuong = 1, RETURN_TIENKHUYENMAI = 0;
            btnStatus.Text = "+ TĂNG HÀNG";
            btnStatus.BackColor = Color.DarkTurquoise; CurrentKey = Keys.F4;
            decimal TEMP_SOLUONG = 0, TEMP_GIABANLEVAT = 0, SOTIEN_KHUYENMAI = 0;
            string maVatTuCheck = vattuDto.MAVATTU.Trim().ToUpper();
            //TÍNH TOÁN KHUYẾN MÃI -- CHỈ ĐỐI VỚI BÁN GIÁ BÁN LẺ -- VỚI BÁN GIÁ BÁN BUÔN VÀ GIÁ VỐN THÌ KHÔNG TÍNH KHUYẾN MẠI
            if (MethodPrice == (int)EnumCommon.MethodGetPrice.GIABANLECOVAT)
            {
                RETURN_TIENKHUYENMAI = FrmXuatBanLeService.TINHTOAN_KHUYENMAI(maVatTuCheck, (EnumCommon.MethodGetPrice)MethodPrice);
                vattuDto.TIENKHUYENMAIRETURN = RETURN_TIENKHUYENMAI;
            }
            //CHECK TRÙNG TRONG GRIDVIEW
            int indexRowExists = CHECK_ROW_EXIST_DATAGRIDVIEW(dgvDetails_Tab, maVatTuCheck);
            if (indexRowExists != -1)
            {
                dgvDetails_Tab.Rows[indexRowExists].Cells["LAMACAN"].Value = vattuDto.LAMACAN;
                dgvDetails_Tab.Rows[indexRowExists].Cells["TONCUOIKYSL"].Value = vattuDto.TONCUOIKYSL;
                decimal SOLUONG_OLD, SOLUONG_NEW = 0;
                decimal.TryParse(dgvDetails_Tab.Rows[indexRowExists].Cells["SOLUONG"].Value.ToString(),
                    out SOLUONG_OLD);
                dgvDetails_Tab.Rows[indexRowExists].Cells["GIABANLECOVAT"].Value = FormatCurrency.FormatMoney(vattuDto.GIABANLEVAT);
                if (vattuDto.SOLUONG == 0)
                {
                    TEMP_SOLUONG = SoLuong;
                }
                else
                {
                    TEMP_SOLUONG = vattuDto.SOLUONG;
                }
                if (SOLUONG_OLD + TEMP_SOLUONG > vattuDto.TONCUOIKYSL)
                {
                    if (FrmXuatBanLeService.GET_THAMSO_KHOABANAM_FROM_ORACLE() == 1)
                    {
                        NotificationLauncher.ShowNotificationWarning("Thông báo", "Hết hàng trong kho ! Không thể bán", 1, "0x1", "0x8", "normal");
                        return;
                    }
                }
                SOLUONG_NEW = SOLUONG_OLD + TEMP_SOLUONG;
                dgvDetails_Tab.Rows[indexRowExists].Cells["SOLUONG"].Value = FormatCurrency.FormatMoney(SOLUONG_NEW);
                if (RETURN_TIENKHUYENMAI < 100)
                {
                    SOTIEN_KHUYENMAI = vattuDto.GIABANLEVAT * RETURN_TIENKHUYENMAI * SOLUONG_NEW / 100;
                    dgvDetails_Tab.Rows[indexRowExists].Cells["TIENKM"].Value =
                        FormatCurrency.FormatMoney(SOTIEN_KHUYENMAI);
                }
                else
                {
                    SOTIEN_KHUYENMAI = RETURN_TIENKHUYENMAI * SOLUONG_NEW;
                    dgvDetails_Tab.Rows[indexRowExists].Cells["TIENKM"].Value =
                        FormatCurrency.FormatMoney(SOTIEN_KHUYENMAI);
                }
                dgvDetails_Tab.Rows[indexRowExists].Cells["TTIENCOVAT"].Value =
                    FormatCurrency.FormatMoney(SOLUONG_NEW * vattuDto.GIABANLEVAT - SOTIEN_KHUYENMAI);
                string index = dgvDetails_Tab.Rows[indexRowExists].Cells[0].Value.ToString();
                int temp = 0;
                dgvDetails_Tab.Rows[indexRowExists].Cells["STT"].Value = dgvDetails_Tab.Rows.Count;
                //txtSoLuong.Text = TEMP_SOLUONG.ToString();
                for (int i = dgvDetails_Tab.Rows.Count; i > int.Parse(index); i--)
                {
                    dgvDetails_Tab.Rows[temp++].Cells[0].Value = i - 1;
                }
                dgvDetails_Tab.Sort(dgvDetails_Tab.Columns["STT"], ListSortDirection.Ascending);
                txtSoLuong.Text = SOLUONG_NEW.ToString();
            }
            else
            {
                if (vattuDto.GIABANLEVAT != 0)
                {
                    if (TEMP_SOLUONG == 0)
                    {
                        TEMP_SOLUONG = SoLuong;
                    }
                    if (vattuDto.SOLUONG == 0)
                    {
                        TEMP_SOLUONG = SoLuong;
                    }
                    else
                    {
                        TEMP_SOLUONG = vattuDto.SOLUONG;
                    }
                    if (RETURN_TIENKHUYENMAI < 100)
                    {
                        SOTIEN_KHUYENMAI = vattuDto.GIABANLEVAT * RETURN_TIENKHUYENMAI * TEMP_SOLUONG / 100;

                    }
                    else
                    {
                        SOTIEN_KHUYENMAI = RETURN_TIENKHUYENMAI * TEMP_SOLUONG;
                    }
                }
                else
                {
                    NotificationLauncher.ShowNotification("Thông báo", "Lỗi khi thêm mã hàng", 1, "0x1", "0x8", "normal");
                    return;
                }
                int indexRowNew = 1;
                if (dgvDetails_Tab.Rows.Count > 0)
                {
                    indexRowNew = int.Parse(dgvDetails_Tab.Rows[0].Cells["STT"].Value.ToString()) + 1;
                }
                int idx = dgvDetails_Tab.Rows.Add();
                DataGridViewRow rowData = dgvDetails_Tab.Rows[idx];
                rowData.Cells["STT"].Value = indexRowNew;
                rowData.Cells["LAMACAN"].Value = vattuDto.LAMACAN;
                rowData.Cells["TONCUOIKYSL"].Value = vattuDto.TONCUOIKYSL;
                rowData.Cells["MAVATTU"].Value = vattuDto.MAVATTU.Trim().ToUpper();
                rowData.Cells["TENVATTU"].Value = vattuDto.TENVATTU.Trim();
                rowData.Cells["DONVITINH"].Value = vattuDto.DONVITINH.Trim().ToUpper();
                rowData.Cells["GIABANLECOVAT"].Value = FormatCurrency.FormatMoney(vattuDto.GIABANLEVAT);
                if (vattuDto.SOLUONG == 0)
                {
                    TEMP_SOLUONG = SoLuong;
                }
                else
                {
                    TEMP_SOLUONG = vattuDto.SOLUONG;
                }
                rowData.Cells["SOLUONG"].Value = FormatCurrency.FormatMoney(TEMP_SOLUONG);
                rowData.Cells["GIAVON"].Value = FormatCurrency.FormatMoney(vattuDto.GIAVON);
                if (RETURN_TIENKHUYENMAI < 100)
                {
                    SOTIEN_KHUYENMAI = (vattuDto.GIABANLEVAT * RETURN_TIENKHUYENMAI * TEMP_SOLUONG) / 100;
                    rowData.Cells["TIENKM"].Value = FormatCurrency.FormatMoney(SOTIEN_KHUYENMAI);
                }
                else
                {
                    SOTIEN_KHUYENMAI = RETURN_TIENKHUYENMAI * TEMP_SOLUONG;
                    rowData.Cells["TIENKM"].Value = FormatCurrency.FormatMoney(SOTIEN_KHUYENMAI);
                }
                TEMP_GIABANLEVAT = TEMP_SOLUONG * vattuDto.GIABANLEVAT - SOTIEN_KHUYENMAI;
                rowData.Cells["TTIENCOVAT"].Value = FormatCurrency.FormatMoney(TEMP_GIABANLEVAT);
                rowData.Cells["CHIETKHAU"].Value = 0;
                lblChietKhauLe.Text = "0";
                txtSoLuong.Text = TEMP_SOLUONG.ToString();
            }
        }
        private System.Drawing.Printing.PrintDocument docToPrint = new System.Drawing.Printing.PrintDocument();
        private void btnPrintDialog_Click(object sender, EventArgs e)
        {
            printDialog.AllowSomePages = true;
            printDialog.ShowHelp = true;
            printDialog.Document = docToPrint;
            DialogResult result = printDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                docToPrint.Print();
            }
        }

        #region Hiển thị màn hình LCD
        static SerialPort _serialPort;
        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public void HienThiManHinhLCD(string TenVatTu, string SoLuong, string GiaBanLeVat, string ThanhTien)
        {
            try
            {
                DisplayLCD20x2(TenVatTu, SoLuong, GiaBanLeVat, ThanhTien);
            }
            catch { }
        }
        public void DisplayLCD20x2(string TenHang, string SoLuong, string DonGia, string ThanhTien)
        {
            try
            {
                SettingSerialPort();
                string Row2 = "";
                string Row1 = convertToUnSign3(TenHang);
                Row2 = SoLuong + ";" + DonGia + ";" + ThanhTien;
                if (DonGia == "") Row2 = ThanhTien;
                var lineOne = new char[20];
                var lineSecond = new char[20];
                var arrayOne = Row1.ToArray();
                var arraySecond = Row2.ToArray();
                for (int i = 0; i < 20; i++)
                {
                    lineOne[i] = (arrayOne.Length > i + 1) ? arrayOne[i] : ' ';
                    lineSecond[i] = (arraySecond.Length > i + 1) ? arraySecond[i] : ' ';
                }
                _serialPort.Write("\f");
                string result1 = String.Join("", lineOne);
                string result2 = String.Join("", lineSecond);
                _serialPort.Write(result1);
                _serialPort.Write(result2);
                _serialPort.Close();
                _serialPort.Close();
            }
            catch
            {
            }
        }
        public static void ClearDisplay()
        {
            _serialPort.Write("                                                 ");
            _serialPort.WriteLine("                                             ");
        }
        public static void SettingSerialPort()
        {
            string GetPortCOM = "";
            _serialPort = new SerialPort();
            foreach (string s in SerialPort.GetPortNames())
            {
                if (s != null)
                {
                    GetPortCOM = s;
                    _serialPort.PortName = GetPortCOM;
                }
            }
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;
            _serialPort.Encoding = Encoding.GetEncoding("UTF-8");
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            if (GetPortCOM != "") _serialPort.Open();
            else return;
        }
        #endregion

    }
}

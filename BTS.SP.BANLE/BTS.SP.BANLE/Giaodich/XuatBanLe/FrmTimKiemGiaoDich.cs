using BTS.SP.BANLE.Common;
using BTS.SP.BANLE.Dto;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public partial class FrmTimKiemGiaoDich : Form
    {
        public Status_TimKiem statusSearch;
        public DataTable dtLoadSearch;
        public RePrintBill GetBillOld;
        private int VALUE_SELECTED_CHANGE = 0;
        private bool printBill = false;
        public FrmTimKiemGiaoDich()
        {
            InitializeComponent();
            //khởi tạo giá trị chọn lựa điều kiện tìm kiếm
            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã giao dịch" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Số tiền" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Thu ngân tạo" });
            cboDieuKienTimKiem.DataSource = _comboItems;
            cboDieuKienTimKiem.DisplayMember = "TEXT";
            cboDieuKienTimKiem.ValueMember = "VALUE";
            //default combobox
            cboDieuKienTimKiem.SelectedIndex = VALUE_SELECTED_CHANGE;

            dateTimeDenNgay.Format = DateTimePickerFormat.Custom;
            dateTimeDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeDenNgay.Value = DateTime.Now;
            dateTimeTuNgay.Format = DateTimePickerFormat.Custom;
            dateTimeTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeTuNgay.Value = DateTime.Now.AddDays(-7);
        }

        public FrmTimKiemGiaoDich(bool rePrintBill)
        {
            InitializeComponent();
            //khởi tạo giá trị chọn lựa điều kiện tìm kiếm
            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã giao dịch" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Số tiền" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Thu ngân tạo" });
            cboDieuKienTimKiem.DataSource = _comboItems;
            cboDieuKienTimKiem.DisplayMember = "TEXT";
            cboDieuKienTimKiem.ValueMember = "VALUE";
            //default combobox
            cboDieuKienTimKiem.SelectedIndex = VALUE_SELECTED_CHANGE;
            printBill = rePrintBill;
            dateTimeDenNgay.Format = DateTimePickerFormat.Custom;
            dateTimeDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeDenNgay.Value = DateTime.Now;
            dateTimeTuNgay.Format = DateTimePickerFormat.Custom;
            dateTimeTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeTuNgay.Value = DateTime.Now.AddDays(-2);
            StartForm();
        }

        public void SetHandlerBill(RePrintBill handler)
        {
            this.GetBillOld = handler;
        }
        public FrmTimKiemGiaoDich(string KeySearch, DateTime TuNgay, DateTime DenNgay, int DieuKienLoc)
        {
            InitializeComponent();
            //khởi tạo giá trị chọn lựa điều kiện tìm kiếm
            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã giao dịch" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Số tiền" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Thu ngân tạo" });
            cboDieuKienTimKiem.DataSource = _comboItems;
            cboDieuKienTimKiem.DisplayMember = "TEXT";
            cboDieuKienTimKiem.ValueMember = "VALUE";
            //default combobox
            cboDieuKienTimKiem.SelectedIndex = DieuKienLoc;

            dateTimeDenNgay.Format = DateTimePickerFormat.Custom;
            dateTimeDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeDenNgay.Value = DenNgay;
            dateTimeTuNgay.Format = DateTimePickerFormat.Custom;
            dateTimeTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dateTimeTuNgay.Value = TuNgay;

            txtDieuKienTimKiem.Text = KeySearch.Trim();
            try
            {
                using (
                    var connection =
                        new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    connection.OpenAsync();
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
                        dgvDanhSachGiaoDichChiTiet.Rows.Clear();
                        dgvDanhSachGiaoDichChiTiet.DataSource = null;
                        dgvDanhSachGiaoDichChiTiet.Refresh();
                        while (dataReader.Read())
                        {
                            int LOAIGIAODICH = 0;
                            int idx = dgvDanhSachGiaoDichChiTiet.Rows.Add();
                            DataGridViewRow rowData = dgvDanhSachGiaoDichChiTiet.Rows[idx];
                            rowData.Cells["MAGIAODICH"].Value = dataReader["MAGIAODICH"];
                            int.TryParse(dataReader["LOAIGIAODICH"].ToString(), out LOAIGIAODICH);
                            rowData.Cells["LOAIGIAODICH"].Value = LOAIGIAODICH == 1 ? "XBLE" : "TRL";
                            rowData.Cells["NGAYPHATSINH"].Value = dataReader["NGAYPHATSINH"];
                            rowData.Cells["MANGUOITAO"].Value = dataReader["MANGUOITAO"];
                            rowData.Cells["NGUOITAO"].Value = dataReader["NGUOITAO"];
                            rowData.Cells["MAQUAYBAN"].Value = dataReader["MAQUAYBAN"];
                            rowData.Cells["HINHTHUCTHANHTOAN"].Value = dataReader["HINHTHUCTHANHTOAN"];
                            rowData.Cells["TIENKHACHDUA"].Value = FormatCurrency.FormatMoney(dataReader["TIENKHACHDUA"]);
                            rowData.Cells["TIENTRALAI"].Value = FormatCurrency.FormatMoney(dataReader["TIENTRALAI"]);
                            rowData.Cells["TTIENCOVAT"].Value = FormatCurrency.FormatMoney(dataReader["TTIENCOVAT"]);
                            rowData.Cells["THOIGIAN"].Value = FormatCurrency.FormatMoney(dataReader["THOIGIAN"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLogs.LogError(ex);
                NotificationLauncher.ShowNotification("Thông báo", "Không tìm thấy", 1, "0x1", "0x8", "normal");
            }
        }
        public static List<NVGDQUAY_ASYNCCLIENT_DTO> TIMKIEM_GIAODICHQUAY(string KeySearch, DateTime TuNgay, DateTime DenNgay, int DieuKienLoc)
        {
            List<NVGDQUAY_ASYNCCLIENT_DTO> listReturn = new List<NVGDQUAY_ASYNCCLIENT_DTO>();
            try
            {
                using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    connection.OpenAsync();
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
                            NVGDQUAY_ASYNCCLIENT_DTO GDQUAY_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
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
                            GDQUAY_DTO.NGAYPHATSINH = DateTime.Parse(dataReader["NGAYPHATSINH"].ToString());
                            GDQUAY_DTO.HINHTHUCTHANHTOAN = dataReader["HINHTHUCTHANHTOAN"].ToString();
                            GDQUAY_DTO.MAVOUCHER = dataReader["MAVOUCHER"].ToString();
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
            catch (Exception ex)
            {
                WriteLogs.LogError(ex);
                NotificationLauncher.ShowNotification("Thông báo", "Xảy ra lỗi", 1, "0x1", "0x8", "normal");
            }
            return listReturn;
        }
        private void txtDieuKienTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtDieuKienTimKiem.Text.Length > 3)
            {
                List<NVGDQUAY_ASYNCCLIENT_DTO> dataReturn = new List<NVGDQUAY_ASYNCCLIENT_DTO>();
                dataReturn = TIMKIEM_GIAODICHQUAY(txtDieuKienTimKiem.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                if (dataReturn.Count > 0)
                {
                    dgvDanhSachGiaoDichChiTiet.Rows.Clear();
                    dgvDanhSachGiaoDichChiTiet.Refresh();
                    foreach (NVGDQUAY_ASYNCCLIENT_DTO dataRow in dataReturn)
                    {
                        int LOAIGIADICH = 0;
                        decimal TIENKHACHDUA, TIENTRALAI, TTIENCOVAT = 0;
                        int idx = dgvDanhSachGiaoDichChiTiet.Rows.Add();
                        DataGridViewRow rowData = dgvDanhSachGiaoDichChiTiet.Rows[idx];
                        rowData.Cells["MAGIAODICH"].Value = dataRow.MAGIAODICH;
                        int.TryParse(dataRow.LOAIGIAODICH.ToString(), out LOAIGIADICH);
                        rowData.Cells["LOAIGIAODICH"].Value = LOAIGIADICH == 1 ? "XBLE" : "TRL";
                        rowData.Cells["NGAYPHATSINH"].Value = dataRow.NGAYPHATSINH.ToString("dd/MM/yyyy");
                        rowData.Cells["MANGUOITAO"].Value = dataRow.MANGUOITAO;
                        rowData.Cells["NGUOITAO"].Value = dataRow.NGUOITAO;
                        rowData.Cells["MAQUAYBAN"].Value = dataRow.MAQUAYBAN;
                        rowData.Cells["HINHTHUCTHANHTOAN"].Value = dataRow.HINHTHUCTHANHTOAN;
                        decimal.TryParse(dataRow.TIENKHACHDUA.ToString(), out TIENKHACHDUA);
                        rowData.Cells["TIENKHACHDUA"].Value = FormatCurrency.FormatMoney(TIENKHACHDUA);
                        decimal.TryParse(dataRow.TIENTRALAI.ToString(), out TIENTRALAI);
                        rowData.Cells["TIENTRALAI"].Value = FormatCurrency.FormatMoney(TIENTRALAI);
                        decimal.TryParse(dataRow.TTIENCOVAT.ToString(), out TTIENCOVAT);
                        rowData.Cells["TTIENCOVAT"].Value = FormatCurrency.FormatMoney(TTIENCOVAT);
                        rowData.Cells["THOIGIAN"].Value = dataRow.THOIGIAN;
                    }
                }
            }
        }
        private void cboDieuKienTimKiem_SelectedValueChanged(object sender, EventArgs e)
        {
            VALUE_SELECTED_CHANGE = (int)cboDieuKienTimKiem.SelectedIndex;
        }
        public void SetHanlerGiaoDichSearch(Status_TimKiem LoadDataSearch)
        {
            statusSearch = LoadDataSearch;
        }
        //DOUBLE CLICK BY DATA TO UC_FRAME_BANLE_TRALAI
        private void dgvDanhSachGiaoDichChiTiet_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string MaGiaoDichChange = dgvDanhSachGiaoDichChiTiet.Rows[e.RowIndex].Cells["MAGIAODICH"].Value.ToString().Trim();
            if (!printBill)
            {
                statusSearch(MaGiaoDichChange, dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
            }
            else
            {
                this.GetBillOld(MaGiaoDichChange, dateTimeTuNgay.Value, dateTimeDenNgay.Value);
            }
            this.Close();
        }
        private void txtDieuKienTimKiem_Validating(object sender, CancelEventArgs e)
        {
            if (txtDieuKienTimKiem.Text.Length > 3)
            {
                List<NVGDQUAY_ASYNCCLIENT_DTO> dataReturn = new List<NVGDQUAY_ASYNCCLIENT_DTO>();
                string msg = Config.CheckConnectToServer(out bool result);
                if (msg.Length > 0) { MessageBox.Show(msg); return; }

                if (result) dataReturn = TIMKIEM_GIAODICHQUAY(txtDieuKienTimKiem.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                else dataReturn = UC_Frame_TraLai.TIMKIEM_GIAODICHQUAY_FROM_SQL(txtDieuKienTimKiem.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);

                if (dataReturn.Count > 0)
                {
                    dgvDanhSachGiaoDichChiTiet.Rows.Clear();
                    dgvDanhSachGiaoDichChiTiet.Refresh();
                    foreach (NVGDQUAY_ASYNCCLIENT_DTO dataRow in dataReturn)
                    {
                        int LOAIGIADICH = 0;
                        decimal TIENKHACHDUA, TIENTRALAI, TTIENCOVAT = 0;
                        int idx = dgvDanhSachGiaoDichChiTiet.Rows.Add();
                        DataGridViewRow rowData = dgvDanhSachGiaoDichChiTiet.Rows[idx];
                        rowData.Cells["MAGIAODICH"].Value = dataRow.MAGIAODICH;
                        int.TryParse(dataRow.LOAIGIAODICH.ToString(), out LOAIGIADICH);
                        rowData.Cells["LOAIGIAODICH"].Value = LOAIGIADICH == 1 ? "XBLE" : "TRL";
                        rowData.Cells["NGAYPHATSINH"].Value = dataRow.NGAYPHATSINH.ToString("dd/MM/yyyy");
                        rowData.Cells["MANGUOITAO"].Value = dataRow.MANGUOITAO;
                        rowData.Cells["NGUOITAO"].Value = dataRow.NGUOITAO;
                        rowData.Cells["MAQUAYBAN"].Value = dataRow.MAQUAYBAN;
                        rowData.Cells["HINHTHUCTHANHTOAN"].Value = dataRow.HINHTHUCTHANHTOAN;
                        decimal.TryParse(dataRow.TIENKHACHDUA.ToString(), out TIENKHACHDUA);
                        rowData.Cells["TIENKHACHDUA"].Value = FormatCurrency.FormatMoney(TIENKHACHDUA);
                        decimal.TryParse(dataRow.TIENTRALAI.ToString(), out TIENTRALAI);
                        rowData.Cells["TIENTRALAI"].Value = FormatCurrency.FormatMoney(TIENTRALAI);
                        decimal.TryParse(dataRow.TTIENCOVAT.ToString(), out TTIENCOVAT);
                        rowData.Cells["TTIENCOVAT"].Value = FormatCurrency.FormatMoney(TTIENCOVAT);
                        rowData.Cells["THOIGIAN"].Value = dataRow.THOIGIAN;
                    }
                }
            }
        }

        private void StartForm()
        {
            List<NVGDQUAY_ASYNCCLIENT_DTO> dataReturn = new List<NVGDQUAY_ASYNCCLIENT_DTO>();
            string msg = Config.CheckConnectToServer(out bool result);
            if (msg.Length > 0) { MessageBox.Show(msg); return; }

            if (result) dataReturn = TIMKIEM_GIAODICHQUAY("", dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
            else dataReturn = UC_Frame_TraLai.TIMKIEM_GIAODICHQUAY_FROM_SQL("", dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);

            if (dataReturn.Count > 0)
            {
                dgvDanhSachGiaoDichChiTiet.Rows.Clear();
                dgvDanhSachGiaoDichChiTiet.Refresh();
                foreach (NVGDQUAY_ASYNCCLIENT_DTO dataRow in dataReturn)
                {
                    int LOAIGIADICH = 0;
                    decimal TIENKHACHDUA, TIENTRALAI, TTIENCOVAT = 0;
                    int idx = dgvDanhSachGiaoDichChiTiet.Rows.Add();
                    DataGridViewRow rowData = dgvDanhSachGiaoDichChiTiet.Rows[idx];
                    rowData.Cells["MAGIAODICH"].Value = dataRow.MAGIAODICH;
                    int.TryParse(dataRow.LOAIGIAODICH.ToString(), out LOAIGIADICH);
                    rowData.Cells["LOAIGIAODICH"].Value = LOAIGIADICH == 1 ? "XBLE" : "TRL";
                    rowData.Cells["NGAYPHATSINH"].Value = dataRow.NGAYPHATSINH.ToString("dd/MM/yyyy");
                    rowData.Cells["MANGUOITAO"].Value = dataRow.MANGUOITAO;
                    rowData.Cells["NGUOITAO"].Value = dataRow.NGUOITAO;
                    rowData.Cells["MAQUAYBAN"].Value = dataRow.MAQUAYBAN;
                    rowData.Cells["HINHTHUCTHANHTOAN"].Value = dataRow.HINHTHUCTHANHTOAN;
                    decimal.TryParse(dataRow.TIENKHACHDUA.ToString(), out TIENKHACHDUA);
                    rowData.Cells["TIENKHACHDUA"].Value = FormatCurrency.FormatMoney(TIENKHACHDUA);
                    decimal.TryParse(dataRow.TIENTRALAI.ToString(), out TIENTRALAI);
                    rowData.Cells["TIENTRALAI"].Value = FormatCurrency.FormatMoney(TIENTRALAI);
                    decimal.TryParse(dataRow.TTIENCOVAT.ToString(), out TTIENCOVAT);
                    rowData.Cells["TTIENCOVAT"].Value = FormatCurrency.FormatMoney(TTIENCOVAT);
                    rowData.Cells["THOIGIAN"].Value = dataRow.THOIGIAN;
                }
            }
        }

        private void btnTimKiemGiaoDich_Click(object sender, EventArgs e)
        {
            if (txtDieuKienTimKiem.Text.Length > 3)
            {
                List<NVGDQUAY_ASYNCCLIENT_DTO> dataReturn = new List<NVGDQUAY_ASYNCCLIENT_DTO>();
                string msg = Config.CheckConnectToServer(out bool result);
                if (msg.Length > 0) { MessageBox.Show(msg); return; }

                if (result) dataReturn = TIMKIEM_GIAODICHQUAY(txtDieuKienTimKiem.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);
                else dataReturn = UC_Frame_TraLai.TIMKIEM_GIAODICHQUAY_FROM_SQL(txtDieuKienTimKiem.Text.Trim(), dateTimeTuNgay.Value, dateTimeDenNgay.Value, VALUE_SELECTED_CHANGE);

                if (dataReturn.Count > 0)
                {
                    dgvDanhSachGiaoDichChiTiet.Rows.Clear();
                    dgvDanhSachGiaoDichChiTiet.Refresh();
                    foreach (NVGDQUAY_ASYNCCLIENT_DTO dataRow in dataReturn)
                    {
                        int LOAIGIADICH = 0;
                        decimal TIENKHACHDUA, TIENTRALAI, TTIENCOVAT = 0;
                        int idx = dgvDanhSachGiaoDichChiTiet.Rows.Add();
                        DataGridViewRow rowData = dgvDanhSachGiaoDichChiTiet.Rows[idx];
                        rowData.Cells["MAGIAODICH"].Value = dataRow.MAGIAODICH;
                        int.TryParse(dataRow.LOAIGIAODICH.ToString(), out LOAIGIADICH);
                        rowData.Cells["LOAIGIAODICH"].Value = LOAIGIADICH == 1 ? "XBLE" : "TRL";
                        rowData.Cells["NGAYPHATSINH"].Value = dataRow.NGAYPHATSINH.ToString("dd/MM/yyyy");
                        rowData.Cells["MANGUOITAO"].Value = dataRow.MANGUOITAO;
                        rowData.Cells["NGUOITAO"].Value = dataRow.NGUOITAO;
                        rowData.Cells["MAQUAYBAN"].Value = dataRow.MAQUAYBAN;
                        rowData.Cells["HINHTHUCTHANHTOAN"].Value = dataRow.HINHTHUCTHANHTOAN;
                        decimal.TryParse(dataRow.TIENKHACHDUA.ToString(), out TIENKHACHDUA);
                        rowData.Cells["TIENKHACHDUA"].Value = FormatCurrency.FormatMoney(TIENKHACHDUA);
                        decimal.TryParse(dataRow.TIENTRALAI.ToString(), out TIENTRALAI);
                        rowData.Cells["TIENTRALAI"].Value = FormatCurrency.FormatMoney(TIENTRALAI);
                        decimal.TryParse(dataRow.TTIENCOVAT.ToString(), out TTIENCOVAT);
                        rowData.Cells["TTIENCOVAT"].Value = FormatCurrency.FormatMoney(TTIENCOVAT);
                        rowData.Cells["THOIGIAN"].Value = dataRow.THOIGIAN;
                    }
                }
            }
        }

        private void FrmTimKiemGiaoDich_Load(object sender, EventArgs e)
        {
            dgvDanhSachGiaoDichChiTiet.Columns["TIENKHACHDUA"].Visible = false;
            dgvDanhSachGiaoDichChiTiet.Columns["TIENTRALAI"].Visible = false;
            dgvDanhSachGiaoDichChiTiet.Columns["TTIENCOVAT"].Visible = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using BTS.SP.BANLE.Dto;
namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public partial class FrmTimKiemKhachHang : Form
    {
        public STATUS_TIMKIEM_KHACHHANG statusTimKiemKhachHang;
        public void BINDING_DATA_KHACHHANG_TO_GRIDVIEW(List<KHACHHANG_DTO> _LST_KHACHHANG_DTO)
        {
            if (_LST_KHACHHANG_DTO.Count > 0)
            {
                dgvThongTinKhachHang.Rows.Clear();
                dgvThongTinKhachHang.DataSource = null;
                dgvThongTinKhachHang.Refresh();
                decimal SUM_SOLUONG = 0;
                decimal SUM_TIENKHUYENMAI = 0;
                foreach (KHACHHANG_DTO _KHACHHANG_DTO in _LST_KHACHHANG_DTO)
                {
                    int idx = dgvThongTinKhachHang.Rows.Add();
                    DataGridViewRow rowData = dgvThongTinKhachHang.Rows[idx];
                    rowData.Cells["STT"].Value = idx + 1;
                    rowData.Cells["MAKH"].Value = _KHACHHANG_DTO.MAKH;
                    rowData.Cells["TENKH"].Value = _KHACHHANG_DTO.TENKH;
                    rowData.Cells["DIACHI"].Value = _KHACHHANG_DTO.DIACHI;
                    rowData.Cells["DIENTHOAI"].Value = _KHACHHANG_DTO.DIENTHOAI;
                    rowData.Cells["CMTND"].Value = _KHACHHANG_DTO.CMTND;
                    rowData.Cells["EMAIL"].Value = _KHACHHANG_DTO.EMAIL;
                    rowData.Cells["NGAYCAPTHE"].Value = _KHACHHANG_DTO.NGAYCAPTHE;
                    rowData.Cells["NGAYSINH"].Value = _KHACHHANG_DTO.NGAYSINH;
                    rowData.Cells["SODIEM"].Value = _KHACHHANG_DTO.SODIEM;
                    rowData.Cells["HANGKHACHHANG"].Value = _KHACHHANG_DTO.HANGKHACHHANG;
                    rowData.Cells["HANGKHACHHANGCU"].Value = _KHACHHANG_DTO.HANGKHACHHANGCU;
                    rowData.Cells["TONGTIEN"].Value = _KHACHHANG_DTO.TONGTIEN;
                }
            }
        }
        public FrmTimKiemKhachHang(string P_KEYSEARCH)
        {
            InitializeComponent();
            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã thẻ" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Mã khách hàng" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Tên khách hàng" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 3, TEXT = "Số điện thoại" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 4, TEXT = "Số chứng minh thư" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 5, TEXT = "Số điểm" });
            cboDieuKienTimKiem.DataSource = _comboItems;
            cboDieuKienTimKiem.DisplayMember = "TEXT";
            cboDieuKienTimKiem.ValueMember = "VALUE";
            cboDieuKienTimKiem.SelectedIndex = 0;
            txtDieuKienTimKiem.Text = P_KEYSEARCH;
            List<KHACHHANG_DTO> _LST_KHACHHANG_DTO = new List<KHACHHANG_DTO>();
            _LST_KHACHHANG_DTO = FrmThanhToanService.TIMKIEM_KHACHHANG_FROM_ORACLE(P_KEYSEARCH, 1, cboDieuKienTimKiem.SelectedIndex, Session.Session.CurrentUnitCode);
            BINDING_DATA_KHACHHANG_TO_GRIDVIEW(_LST_KHACHHANG_DTO);
        }

        private void btnTimKiemKhachHang_Click(object sender, System.EventArgs e)
        {
            string P_KEYSEARCH = txtDieuKienTimKiem.Text;
            int P_USE_TIMKIEM_ALL = 0;
            int P_DIEUKIEN_TIMKIEM = cboDieuKienTimKiem.SelectedIndex;
            List<KHACHHANG_DTO> _LST_KHACHHANG_DTO = new List<KHACHHANG_DTO>();
            _LST_KHACHHANG_DTO = FrmThanhToanService.TIMKIEM_KHACHHANG_FROM_ORACLE(P_KEYSEARCH, P_USE_TIMKIEM_ALL,P_DIEUKIEN_TIMKIEM, Session.Session.CurrentUnitCode);
            BINDING_DATA_KHACHHANG_TO_GRIDVIEW(_LST_KHACHHANG_DTO);
        }

        public void SetHanlerTimKiemKhachHang(STATUS_TIMKIEM_KHACHHANG BINDING_DATA_CHANGE_FROM_GRID_TO_TEXT)
        {
            statusTimKiemKhachHang = BINDING_DATA_CHANGE_FROM_GRID_TO_TEXT;
        }

        private void dgvThongTinKhachHang_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            KHACHHANG_DTO _KHACHHANG_DTO= new KHACHHANG_DTO();
            _KHACHHANG_DTO.MAKH = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["MAKH"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["MAKH"].Value.ToString() : "";
            _KHACHHANG_DTO.TENKH = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["TENKH"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["TENKH"].Value.ToString() : "";
            _KHACHHANG_DTO.DIACHI = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["DIACHI"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["DIACHI"].Value.ToString() : "";
            _KHACHHANG_DTO.DIENTHOAI = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["DIENTHOAI"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["DIENTHOAI"].Value.ToString() : "";
            _KHACHHANG_DTO.CMTND = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["CMTND"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["CMTND"].Value.ToString() : "";
            _KHACHHANG_DTO.EMAIL = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["EMAIL"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["EMAIL"].Value.ToString() : "";
            _KHACHHANG_DTO.HANGKHACHHANG = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["HANGKHACHHANG"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["HANGKHACHHANG"].Value.ToString() : "";
            _KHACHHANG_DTO.HANGKHACHHANGCU = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["HANGKHACHHANGCU"].Value != null ? dgvThongTinKhachHang.Rows[e.RowIndex].Cells["HANGKHACHHANGCU"].Value.ToString() : "";
            _KHACHHANG_DTO.NGAYCAPTHE = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["NGAYCAPTHE"].Value != null ? DateTime.Parse(dgvThongTinKhachHang.Rows[e.RowIndex].Cells["NGAYCAPTHE"].Value.ToString()) : (DateTime?)null;
            _KHACHHANG_DTO.NGAYSINH = dgvThongTinKhachHang.Rows[e.RowIndex].Cells["NGAYSINH"].Value != null ? DateTime.Parse(dgvThongTinKhachHang.Rows[e.RowIndex].Cells["NGAYSINH"].Value.ToString()) : (DateTime?)null;
            decimal SODIEM = 0;
            if (dgvThongTinKhachHang.Rows[e.RowIndex].Cells["SODIEM"].Value != null)
            {
                decimal.TryParse(dgvThongTinKhachHang.Rows[e.RowIndex].Cells["SODIEM"].Value.ToString(), out SODIEM);
            }
            _KHACHHANG_DTO.SODIEM = SODIEM;
            decimal TONGTIEN = 0;
            if (dgvThongTinKhachHang.Rows[e.RowIndex].Cells["TONGTIEN"].Value != null)
            {
                decimal.TryParse(dgvThongTinKhachHang.Rows[e.RowIndex].Cells["TONGTIEN"].Value.ToString(), out TONGTIEN);
            }
            _KHACHHANG_DTO.TONGTIEN = TONGTIEN;
            statusTimKiemKhachHang(_KHACHHANG_DTO);
            this.Close();
            this.Dispose();
        }

        private void txtDieuKienTimKiem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtDieuKienTimKiem.Text))
                {
                    string P_KEYSEARCH = txtDieuKienTimKiem.Text;
                    int P_USE_TIMKIEM_ALL = 1;
                    int P_DIEUKIEN_TIMKIEM = cboDieuKienTimKiem.SelectedIndex;
                    List<KHACHHANG_DTO> _LST_KHACHHANG_DTO = new List<KHACHHANG_DTO>();
                    _LST_KHACHHANG_DTO = FrmThanhToanService.TIMKIEM_KHACHHANG_FROM_ORACLE(P_KEYSEARCH, P_USE_TIMKIEM_ALL, P_DIEUKIEN_TIMKIEM, Session.Session.CurrentUnitCode);
                    BINDING_DATA_KHACHHANG_TO_GRIDVIEW(_LST_KHACHHANG_DTO);
                }
            }
        }

        private void txtDieuKienTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDieuKienTimKiem.Text))
            {
                string P_KEYSEARCH = txtDieuKienTimKiem.Text;
                int P_USE_TIMKIEM_ALL = 1;
                int P_DIEUKIEN_TIMKIEM = cboDieuKienTimKiem.SelectedIndex;
                List<KHACHHANG_DTO> _LST_KHACHHANG_DTO = new List<KHACHHANG_DTO>();
                _LST_KHACHHANG_DTO = FrmThanhToanService.TIMKIEM_KHACHHANG_FROM_ORACLE(P_KEYSEARCH, P_USE_TIMKIEM_ALL, P_DIEUKIEN_TIMKIEM, Session.Session.CurrentUnitCode);
                BINDING_DATA_KHACHHANG_TO_GRIDVIEW(_LST_KHACHHANG_DTO);
            }
        }
    }
}

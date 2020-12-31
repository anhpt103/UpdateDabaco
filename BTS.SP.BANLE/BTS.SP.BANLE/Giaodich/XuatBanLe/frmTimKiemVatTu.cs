﻿using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using BTS.SP.BANLE.Common;
using Oracle.ManagedDataAccess.Client;
using BTS.SP.BANLE.Dto;
using System.Collections.Generic;
using System.ComponentModel;

namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public partial class frmTimKiemVatTu : Form
    {
        public SearchVatTu searchVatTu;
        public frmTimKiemVatTu()
        {
            InitializeComponent();
            dgvResultSearch.Columns["MANHACUNGCAP"].Visible = false;
            dgvResultSearch.Columns["TENNHACUNGCAP"].Visible = false;
            dgvResultSearch.Columns["MALOAIVATTU"].Visible = false;
            dgvResultSearch.Columns["MANHOMVATTU"].Visible = false;
            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã vật tư" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Barcode" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Itemcode" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 3, TEXT = "Tên vật tư" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 4, TEXT = "Mã bó hàng" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 5, TEXT = "Mã hàng trong bó" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 6, TEXT = "Giá bán" });
            cboDieuKienTimKiem.DataSource = _comboItems;
            cboDieuKienTimKiem.DisplayMember = "TEXT";
            cboDieuKienTimKiem.ValueMember = "VALUE";
            cboDieuKienTimKiem.SelectedIndex = 0;
        }
        public frmTimKiemVatTu(string MaHang)
        {
            InitializeComponent();
            dgvResultSearch.Columns["MANHACUNGCAP"].Visible = false;
            dgvResultSearch.Columns["TENNHACUNGCAP"].Visible = false;
            dgvResultSearch.Columns["MALOAIVATTU"].Visible = false;
            dgvResultSearch.Columns["MANHOMVATTU"].Visible = false;

            BindingList<TIMKIEM_DTO> _comboItems = new BindingList<TIMKIEM_DTO>();
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 0, TEXT = "Mã vật tư" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 1, TEXT = "Barcode" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 2, TEXT = "Itemcode" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 3, TEXT = "Tên vật tư" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 4, TEXT = "Mã bó hàng" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 5, TEXT = "Mã hàng trong bó" });
            _comboItems.Add(new TIMKIEM_DTO { VALUE = 6, TEXT = "Giá bán" });
            cboDieuKienTimKiem.DataSource = _comboItems;
            cboDieuKienTimKiem.DisplayMember = "TEXT";
            cboDieuKienTimKiem.ValueMember = "VALUE";
            cboDieuKienTimKiem.SelectedIndex = 0;
            txtFilterSearch.Text = MaHang;
            List<TIMKIEM_VATTU_DTO> LST_TIMKIEM_VATTU_DTO = new List<TIMKIEM_VATTU_DTO>();
            LST_TIMKIEM_VATTU_DTO = TIMKIEM_DULIEU_HANGHOA_DATABASE_ORACLE(MaHang, 1, cboDieuKienTimKiem.SelectedIndex, Session.Session.CurrentUnitCode);
            BINDING_DATA_TO_GRIDVIEW(LST_TIMKIEM_VATTU_DTO);
        }
        private const int WM_KEYDOWN = 256;
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                if ((Keys)m.WParam == Keys.Escape)
                {
                    this.Close();
                    this.Dispose();
                }
            }
            return base.ProcessKeyPreview(ref m);
        }
        public void handlerSearchVatTu(SearchVatTu search)
        {
            this.searchVatTu = search;
        }
        public List<TIMKIEM_VATTU_DTO> TIMKIEM_DULIEU_HANGHOA_DATABASE_ORACLE(string DIEUKIENLOC,int SUDUNG_TIMKIEM_ALL,int DIEUKIENCHON,string UNITCODE)
        {
            List<TIMKIEM_VATTU_DTO> LST_TIMKIEM_VATTU_DTO = new List<TIMKIEM_VATTU_DTO>();
            if (!string.IsNullOrEmpty(DIEUKIENLOC))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand command = new OracleCommand();
                            command.Connection = connection;
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = @"BANLE_TIMKIEM_BOHANG_MAHANG";
                            command.Parameters.Add(@"P_MADONVI", OracleDbType.NVarchar2, 50).Value = UNITCODE;
                            command.Parameters.Add(@"P_TUKHOA", OracleDbType.NVarchar2, 50).Value = DIEUKIENLOC.ToString().ToUpper().Trim();
                            command.Parameters.Add(@"P_SUDUNG_TIMKIEM_ALL", OracleDbType.Int32).Value = SUDUNG_TIMKIEM_ALL;
                            command.Parameters.Add(@"P_DIEUKIENCHON", OracleDbType.Int32).Value = DIEUKIENCHON;
                            command.Parameters.Add(@"CURSOR_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                            OracleDataReader dataReader = command.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    TIMKIEM_VATTU_DTO TIMKIEM_VATTU_DTO = new TIMKIEM_VATTU_DTO();
                                    if (dataReader["MAVATTU"] != null)
                                    {
                                        TIMKIEM_VATTU_DTO.MAVATTU = dataReader["MAVATTU"].ToString();
                                    }
                                    if (dataReader["MAKHAC"] != null)
                                    {
                                        TIMKIEM_VATTU_DTO.MAKHAC = dataReader["MAKHAC"].ToString();
                                    }
                                    if (dataReader["TENVATTU"] != null)
                                    {
                                        TIMKIEM_VATTU_DTO.TENVATTU = dataReader["TENVATTU"].ToString();
                                    }
                                    if (dataReader["MALOAIVATTU"] != null)
                                    {
                                        TIMKIEM_VATTU_DTO.MALOAIVATTU = dataReader["MALOAIVATTU"].ToString();
                                    }
                                    if (dataReader["MANHOMVATTU"] != null)
                                    {
                                        TIMKIEM_VATTU_DTO.MANHOMVATTU = dataReader["MANHOMVATTU"].ToString();
                                    }
                                    if (dataReader["DONVITINH"] != null)
                                    {
                                        TIMKIEM_VATTU_DTO.DONVITINH = dataReader["DONVITINH"].ToString();
                                    }
                                    if (dataReader["MANHACUNGCAP"] != null)
                                    {
                                        TIMKIEM_VATTU_DTO.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                    }
                                    if (dataReader["TENNHACUNGCAP"] != null)
                                    {
                                        TIMKIEM_VATTU_DTO.TENNHACUNGCAP = dataReader["TENNHACUNGCAP"].ToString();
                                    }
                                    if (dataReader["MAKEHANG"] != null)
                                    {
                                        TIMKIEM_VATTU_DTO.MAKEHANG = dataReader["MAKEHANG"].ToString();
                                    }
                                    if (dataReader["GIABANLEVAT"] != null)
                                    {
                                        decimal GIABANLECOVAT = 0;
                                        decimal.TryParse(dataReader["GIABANLEVAT"].ToString(), out GIABANLECOVAT);
                                        TIMKIEM_VATTU_DTO.GIABANLECOVAT = GIABANLECOVAT;
                                    }
                                    if (dataReader["ITEMCODE"] != null)
                                    {
                                        TIMKIEM_VATTU_DTO.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                    }
                                    if (dataReader["BARCODE"] != null)
                                    {
                                        TIMKIEM_VATTU_DTO.BARCODE = dataReader["BARCODE"].ToString();
                                    }
                                    LST_TIMKIEM_VATTU_DTO.Add(TIMKIEM_VATTU_DTO);
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        WriteLogs.LogError(ex);
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            return LST_TIMKIEM_VATTU_DTO;
        }

        public void BINDING_DATA_TO_GRIDVIEW(List<TIMKIEM_VATTU_DTO> LST_TIMKIEM_VATTU_DTO)
        {
            if(LST_TIMKIEM_VATTU_DTO.Count > 0)
            {
                dgvResultSearch.Rows.Clear();
                dgvResultSearch.DataSource = null;
                dgvResultSearch.Refresh();
                foreach (TIMKIEM_VATTU_DTO TIMKIEM_VATTU_DTO in LST_TIMKIEM_VATTU_DTO)
                {
                    int idx = dgvResultSearch.Rows.Add();
                    DataGridViewRow rowData = dgvResultSearch.Rows[idx];
                    rowData.Cells["STT"].Value = idx + 1;
                    rowData.Cells["MAVATTU"].Value = TIMKIEM_VATTU_DTO.MAVATTU;
                    rowData.Cells["MAKHAC"].Value = TIMKIEM_VATTU_DTO.MAKHAC;
                    rowData.Cells["TENVATTU"].Value = TIMKIEM_VATTU_DTO.TENVATTU;
                    rowData.Cells["MALOAIVATTU"].Value = TIMKIEM_VATTU_DTO.MALOAIVATTU;
                    rowData.Cells["MANHOMVATTU"].Value = TIMKIEM_VATTU_DTO.MANHOMVATTU;
                    rowData.Cells["DONVITINH"].Value = TIMKIEM_VATTU_DTO.DONVITINH;
                    rowData.Cells["MANHACUNGCAP"].Value = TIMKIEM_VATTU_DTO.MANHACUNGCAP;
                    rowData.Cells["TENNHACUNGCAP"].Value = TIMKIEM_VATTU_DTO.TENNHACUNGCAP;
                    rowData.Cells["MAKEHANG"].Value = TIMKIEM_VATTU_DTO.MAKEHANG;
                    rowData.Cells["GIABANLECOVAT"].Value = FormatCurrency.FormatMoney(TIMKIEM_VATTU_DTO.GIABANLECOVAT);
                    rowData.Cells["ITEMCODE"].Value = TIMKIEM_VATTU_DTO.ITEMCODE;
                    rowData.Cells["BARCODE"].Value = TIMKIEM_VATTU_DTO.BARCODE;
                }
            }
        }
        private void txtFilterSearch_TextChanged(object sender, EventArgs e)
        {
            if (Config.CheckConnectToServer()) // nếu có mạng lan
            {
                int P_DIEUKIEN_TIMKIEM = cboDieuKienTimKiem.SelectedIndex;
                List<TIMKIEM_VATTU_DTO> LST_TIMKIEM_VATTU_DTO = TIMKIEM_DULIEU_HANGHOA_DATABASE_ORACLE(txtFilterSearch.Text, 0 ,P_DIEUKIEN_TIMKIEM,Session.Session.CurrentUnitCode);
                if(LST_TIMKIEM_VATTU_DTO.Count > 0)
                {
                    BINDING_DATA_TO_GRIDVIEW(LST_TIMKIEM_VATTU_DTO);
                }
            }
            else
            {
                MessageBox.Show("Không có kết nối database !");
            }
        }

        private void dgvResultSearch_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string maHang = dgvResultSearch.Rows[e.RowIndex].Cells["MAVATTU"].Value.ToString();
            this.searchVatTu(maHang);
            this.Dispose();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (Config.CheckConnectToServer()) // nếu có mạng lan
            {
                int P_DIEUKIEN_TIMKIEM = cboDieuKienTimKiem.SelectedIndex;
                List<TIMKIEM_VATTU_DTO> LST_TIMKIEM_VATTU_DTO = TIMKIEM_DULIEU_HANGHOA_DATABASE_ORACLE(txtFilterSearch.Text, 0 , P_DIEUKIEN_TIMKIEM, Session.Session.CurrentUnitCode);
                if (LST_TIMKIEM_VATTU_DTO.Count > 0)
                {
                    BINDING_DATA_TO_GRIDVIEW(LST_TIMKIEM_VATTU_DTO);
                }}
            else
            {
                MessageBox.Show("Không có kết nối database !");
            }
        }
    }
}

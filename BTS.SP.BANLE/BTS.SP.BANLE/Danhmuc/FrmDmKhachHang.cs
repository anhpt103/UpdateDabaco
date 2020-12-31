﻿using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using BTS.SP.BANLE.Common;
using BTS.SP.BANLE.Dto;
using BTS.SP.BANLE.Giaodich.XuatBanLe;
using Oracle.ManagedDataAccess.Client;

namespace BTS.SP.BANLE.Danhmuc
{
    public partial class FrmDmKhachHang : Form
    {
        public STATUS_THEMMOI_KHACHHANG _STATUS_THEMMOI_KHACHHANG;
        public FrmDmKhachHang()
        {
            InitializeComponent();
            txtMaKhachHang.Focus();
            dateNgaySinh.Format = DateTimePickerFormat.Custom;
            dateNgaySinh.CustomFormat = "dd/MM/yyyy";
            txtMaKhachHang.Text = BUILD_MAKHACHHANG_FROM_ORACLE();
        }

        public void SET_HANDLER_STATUS_THEMMOI_KHACHHANG(STATUS_THEMMOI_KHACHHANG BINDING_DATA_KHACHHANG_TO_THANHTOAN)
        {
            _STATUS_THEMMOI_KHACHHANG = BINDING_DATA_KHACHHANG_TO_THANHTOAN;
        }

        public int CHECK_FILL_FIELD_INPUT_TEXT()
        {
            int RESULT = 0;
            if (!string.IsNullOrEmpty(txtMaKhachHang.Text))
            {RESULT ++;
            }
            if (!string.IsNullOrEmpty(txtTenKhachHang.Text))
            {
                RESULT++;
            }
            return RESULT;
        }
        public int SAVE_DATA_KHACHHANG_TO_ORACLE()
        {
            int RESULT = 0;
            //SAVE ORACLE
            string MAKHACHHANG = SAVE_MAKHACHHANG_TO_ORACLE();
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        KHACHHANG_DTO _KHACHHANG_DTO = new KHACHHANG_DTO();
                        _KHACHHANG_DTO.MAKH = MAKHACHHANG;
                        _KHACHHANG_DTO.TENKH = txtTenKhachHang.Text;
                        _KHACHHANG_DTO.DIACHI = txtDiaChi.Text;
                        _KHACHHANG_DTO.DIENTHOAI = txtDienThoai.Text;
                        _KHACHHANG_DTO.CMTND = txtChungMinhThu.Text;
                        _KHACHHANG_DTO.EMAIL = "";
                        _KHACHHANG_DTO.SODIEM = 0;
                        _KHACHHANG_DTO.TONGTIEN = 0;
                        _KHACHHANG_DTO.NGAYCAPTHE = DateTime.Now;
                        _KHACHHANG_DTO.NGAYHETHAN = DateTime.Now.AddYears(1);
                        _KHACHHANG_DTO.NGAYSINH = dateNgaySinh.Value;
                        _KHACHHANG_DTO.UNITCODE = Session.Session.CurrentUnitCode;
                        _KHACHHANG_DTO.MATHE = txtMaThe.Text;

                        OracleCommand command = new OracleCommand();
                        command.Connection = connection;
                        string queryInsertItem = string.Format(@"INSERT INTO DM_KHACHHANG (ID,MAKH,TENKH,DIACHI,DIENTHOAI,CMTND,SODIEM,TONGTIEN,NGAYCAPTHE,NGAYHETHAN,NGAYSINH,UNITCODE,TRANGTHAI,MATHE) VALUES (:ID,:MAKH,:TENKH,:DIACHI,:DIENTHOAI,:CMTND,:SODIEM,:TONGTIEN,:NGAYCAPTHE,:NGAYHETHAN,:NGAYSINH,:UNITCODE,:TRANGTHAI,:MATHE)");
                        command.CommandText = queryInsertItem;
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add("ID", OracleDbType.NVarchar2, 50).Value = Guid.NewGuid();
                        command.Parameters.Add("MAKH", OracleDbType.NVarchar2, 50).Value = _KHACHHANG_DTO.MAKH;
                        command.Parameters.Add("TENKH", OracleDbType.NVarchar2, 50).Value = _KHACHHANG_DTO.TENKH;
                        command.Parameters.Add("DIACHI", OracleDbType.NVarchar2, 50).Value = _KHACHHANG_DTO.DIACHI;
                        command.Parameters.Add("DIENTHOAI", OracleDbType.NVarchar2, 50).Value = _KHACHHANG_DTO.DIENTHOAI;
                        command.Parameters.Add("CMTND", OracleDbType.NVarchar2, 50).Value = _KHACHHANG_DTO.CMTND;
                        command.Parameters.Add("SODIEM", OracleDbType.Decimal).Value = _KHACHHANG_DTO.SODIEM;
                        command.Parameters.Add("TONGTIEN", OracleDbType.Decimal).Value = _KHACHHANG_DTO.TONGTIEN;
                        command.Parameters.Add("NGAYCAPTHE", OracleDbType.Date).Value = _KHACHHANG_DTO.NGAYCAPTHE;
                        command.Parameters.Add("NGAYHETHAN", OracleDbType.Date).Value = _KHACHHANG_DTO.NGAYHETHAN;
                        command.Parameters.Add("NGAYSINH", OracleDbType.Date).Value = _KHACHHANG_DTO.NGAYSINH;
                        command.Parameters.Add("UNITCODE", OracleDbType.NVarchar2,50).Value = _KHACHHANG_DTO.UNITCODE;
                        command.Parameters.Add("TRANGTHAI", OracleDbType.Int32).Value = 10;
                        command.Parameters.Add("MATHE", OracleDbType.NVarchar2, 50).Value = _KHACHHANG_DTO.MATHE;
                        try
                        {
                            if (command.ExecuteNonQuery() > 0) RESULT++;
                            _STATUS_THEMMOI_KHACHHANG(_KHACHHANG_DTO);
                        }
                        catch (Exception e)
                        {
                            WriteLogs.LogError(e);
                            NotificationLauncher.ShowNotificationError("THÔNG BÁO", "XẢY RA LỖI KHI LƯU THÔNG TIN KHÁCH HÀNG", 1, "0x1", "0x8", "normal");
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
                    NotificationLauncher.ShowNotificationError("THÔNG BÁO", "KHÔNG CÓ KẾT NỐI VỚI CƠ SỞ DỮ LIỆU TỚI ORACLE", 1, "0x1", "0x8", "normal");
                }
            }
            return RESULT;
        }
        public string ADD_STRING(string INPUT, int LENGTH, string CHARACTOR)
        {
            var RESULT = INPUT;
            while (RESULT.Length < LENGTH)
            {
                RESULT = string.Format("{0}{1}", CHARACTOR, RESULT);
            }
            return RESULT;
        }
        public string GENERATE_NUMBER(string CURRENT)
        {
            var RESULT = "";
            int NUMBER;
            var LENGTH = CURRENT.Length;
            if (int.TryParse(CURRENT, out NUMBER))
            {
                RESULT = string.Format("{0}", NUMBER + 1);
                RESULT = ADD_STRING(RESULT, LENGTH, "0");
            }
            return RESULT;
        }

        public string BUILD_MAKHACHHANG_FROM_ORACLE()
        {
            string RESULT = "";
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        int CURRENT = 0;
                        OracleCommand command = new OracleCommand();
                        command.Connection = connection;
                        string querySelectItem = "SELECT \"TYPE\",CODE,\"CURRENT\" FROM MD_ID_BUILDER WHERE CODE = 'KH'";
                        command.CommandText = querySelectItem;
                        command.CommandType = CommandType.Text;
                        OracleDataReader dataReaderBuildCode = command.ExecuteReader();
                        if (dataReaderBuildCode.HasRows)
                        {
                            while (dataReaderBuildCode.Read())
                            {
                                MD_ID_BUILDER _MD_ID_BUILDER = new MD_ID_BUILDER();
                                _MD_ID_BUILDER.ID = Guid.NewGuid().ToString();
                                _MD_ID_BUILDER.TYPE = "KH";
                                _MD_ID_BUILDER.CODE = "KH";
                                _MD_ID_BUILDER.CURRENT = "0000";
                                _MD_ID_BUILDER.UNITCODE = Session.Session.CurrentUnitCode;
                                if (dataReaderBuildCode["CURRENT"] != null)
                                {
                                    int CURRENT_NUMBER = 0;
                                    int.TryParse(dataReaderBuildCode["CURRENT"].ToString(), out CURRENT_NUMBER);
                                    string SO_MA = GENERATE_NUMBER(CURRENT_NUMBER.ToString());
                                    _MD_ID_BUILDER.CURRENT = SO_MA;
                                    RESULT = string.Format("{0}{1}", _MD_ID_BUILDER.CODE, SO_MA);
                                }
                            }
                        }
                        else
                        {
                            MD_ID_BUILDER _MD_ID_BUILDER = new MD_ID_BUILDER();
                            _MD_ID_BUILDER.ID = Guid.NewGuid().ToString();
                            _MD_ID_BUILDER.TYPE = "KH";
                            _MD_ID_BUILDER.CODE = "KH";
                            _MD_ID_BUILDER.CURRENT = "0000";
                            _MD_ID_BUILDER.UNITCODE = Session.Session.CurrentUnitCode;
                            string SO_MA = GENERATE_NUMBER(_MD_ID_BUILDER.CURRENT.ToString());
                            _MD_ID_BUILDER.CURRENT = SO_MA;
                            RESULT = string.Format("{0}{1}", _MD_ID_BUILDER.CODE, SO_MA);
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
            return RESULT;
        }
        public string SAVE_MAKHACHHANG_TO_ORACLE()
        {
            string RESULT = "";
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        int CURRENT = 0;
                        OracleCommand command = new OracleCommand();
                        command.Connection = connection;
                        string querySelectItem = "SELECT \"TYPE\",CODE,\"CURRENT\" FROM MD_ID_BUILDER WHERE CODE = 'KH'";
                        command.CommandText = querySelectItem;
                        command.CommandType = CommandType.Text;
                        OracleDataReader dataReaderBuildCode = command.ExecuteReader();
                        if (dataReaderBuildCode.HasRows)
                        {
                            while (dataReaderBuildCode.Read())                            {
                                MD_ID_BUILDER _MD_ID_BUILDER = new MD_ID_BUILDER();
                                _MD_ID_BUILDER.ID = Guid.NewGuid().ToString();
                                _MD_ID_BUILDER.TYPE = "KH";
                                _MD_ID_BUILDER.CODE = "KH";
                                _MD_ID_BUILDER.CURRENT = "0000";
                                _MD_ID_BUILDER.UNITCODE = Session.Session.CurrentUnitCode;
                                int CURRENT_NUMBER = 0;
                                int.TryParse(dataReaderBuildCode["CURRENT"].ToString(), out CURRENT_NUMBER);
                                string SO_MA = GENERATE_NUMBER(CURRENT_NUMBER.ToString());
                                _MD_ID_BUILDER.CURRENT = SO_MA;
                                OracleCommand commandUpdate = new OracleCommand();
                                commandUpdate.Connection = connection;
                                string queryUpdateItem = "UPDATE MD_ID_BUILDER SET \"CURRENT\" = '" + _MD_ID_BUILDER.CURRENT + "' WHERE \"TYPE\" = '" + _MD_ID_BUILDER.TYPE + "' AND \"CODE\" = '" + _MD_ID_BUILDER.CODE + "' ";
                                commandUpdate.CommandText = queryUpdateItem;
                                commandUpdate.CommandType = CommandType.Text;
                                commandUpdate.ExecuteNonQuery();
                                RESULT = string.Format("{0}{1}", _MD_ID_BUILDER.CODE, SO_MA);
                            }
                        }
                        else
                        {
                            MD_ID_BUILDER _MD_ID_BUILDER = new MD_ID_BUILDER();
                            _MD_ID_BUILDER.ID = Guid.NewGuid().ToString();
                            _MD_ID_BUILDER.TYPE = "KH";
                            _MD_ID_BUILDER.CODE = "KH";
                            _MD_ID_BUILDER.CURRENT = "0000";
                            _MD_ID_BUILDER.UNITCODE = Session.Session.CurrentUnitCode;
                            string SO_MA = GENERATE_NUMBER(_MD_ID_BUILDER.CURRENT);
                            _MD_ID_BUILDER.CURRENT = SO_MA;
                            OracleCommand commandInsert = new OracleCommand();
                            commandInsert.Connection = connection;
                            string queryInsertItem = "INSERT INTO MD_ID_BUILDER(ID,\"TYPE\",CODE,\"CURRENT\",\"UNITCODE\",NGAYTAO) VALUES ('" + _MD_ID_BUILDER.ID + "','" + _MD_ID_BUILDER.TYPE + "','" + _MD_ID_BUILDER.TYPE + "','" + _MD_ID_BUILDER.CURRENT + "','" + _MD_ID_BUILDER.UNITCODE + "','" + DateTime.Now + "')";
                            commandInsert.CommandText = queryInsertItem;
                            commandInsert.CommandType = CommandType.Text;
                            commandInsert.ExecuteNonQuery();
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
            return RESULT;
        }
        private void FrmDmKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CHECK_FILL_FIELD_INPUT_TEXT() >= 2)
                {
                    int i = SAVE_DATA_KHACHHANG_TO_ORACLE();
                    if(i == 1) NotificationLauncher.ShowNotification("THÔNG BÁO", "THÊM MỚI THÀNH CÔNG", 1, "0x1", "0x8", "normal");
                    this.Close();
                    this.Dispose();
                }
                else
                {
                    NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "PHẢI KHAI BÁO ĐỦ THÔNG TIN", 1, "0x1", "0x8", "normal");
                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                this.Dispose();
            }
        }

        private void btnExits_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CHECK_FILL_FIELD_INPUT_TEXT() >= 2)
            {
                int i = SAVE_DATA_KHACHHANG_TO_ORACLE();
                if (i == 1) NotificationLauncher.ShowNotification("THÔNG BÁO", "THÊM MỚI THÀNH CÔNG", 1, "0x1", "0x8", "normal");
                this.Close();
                this.Dispose();
            }
            else
            {
                NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "PHẢI KHAI BÁO ĐỦ THÔNG TIN", 1, "0x1", "0x8", "normal");
            }
        }
    }
}

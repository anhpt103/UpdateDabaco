using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;
using BTS.SP.BANLE.Common;
using BTS.SP.BANLE.Dto;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public class FrmThanhToanTraLaiService
    {
        public static decimal LAY_SOLUONG_MATHANG_TRONGBO_FROM_ORACLE(string MaBoHang, string MaHang)
        {
            decimal SOLUONG_RESULT = 0;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandText =
                            string.Format(
                                @"SELECT b.SOLUONG FROM DM_BOHANG a INNER JOIN DM_BOHANGCHITIET b ON a.MABOHANG = b.MABOHANG WHERE a.MABOHANG = '" + MaBoHang + "' AND b.MAHANG = '" + MaHang + "' AND a.UNITCODE = '" + Session.Session.CurrentUnitCode + "' ");
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {while (dataReader.Read())
                            {
                                if (dataReader["SOLUONG"] != null)
                                {
                                    decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG_RESULT);
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("XẢY RA LỖI KHI LẤY DỮ LIỆU MẶT HÀNG TRONG BÓ BÁN TRẢ LẠI ONLINE");
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
            return SOLUONG_RESULT;
        }

        public static decimal LAY_SOLUONG_MATHANG_TRONGBO_FROM_SQLSERVER(string MaBoHang, string MaHang)
        {
            decimal SOLUONG_RESULT = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText =
                            string.Format(
                                @"SELECT DM_BOHANGCHITIET.SOLUONG FROM dbo.DM_BOHANG INNER JOIN dbo.DM_BOHANGCHITIET ON DM_BOHANG.MABOHANG = DM_BOHANGCHITIET.MABOHANG WHERE DM_BOHANG.MABOHANG = '" + MaBoHang + "' AND DM_BOHANGCHITIET.MAHANG = '" + MaHang + "' AND DM_BOHANG.UNITCODE = '" + Session.Session.CurrentUnitCode + "' ");
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["SOLUONG"] != null)
                                {
                                    decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG_RESULT);
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("XẢY RA LỖI KHI LẤY DỮ LIỆU MẶT HÀNG TRONG BÓ BÁN TRẢ LẠI OFFLINE");
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
            return SOLUONG_RESULT;
        }

        public static NVGDQUAY_ASYNCCLIENT_DTO KHOITAO_DULIEU_THANHTOAN_BANTRALAI_FROM_ORACLE(string MaGiaoDichTraLai,string TongTienTraLai,DataGridView dgvTraLai)
        {
            NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
            if (dgvTraLai.RowCount > 0)
            {
                _NVGDQUAY_ASYNCCLIENT_DTO.ID = Guid.NewGuid() + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Hour + DateTime.Now.Minute;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICH = MaGiaoDichTraLai.Trim();
                _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK = MaGiaoDichTraLai.Trim() + "." + Session.Session.CurrentUnitCode.Split('-')[1];
                _NVGDQUAY_ASYNCCLIENT_DTO.MADONVI = Session.Session.CurrentUnitCode;
                _NVGDQUAY_ASYNCCLIENT_DTO.LOAIGIAODICH = 2;
                _NVGDQUAY_ASYNCCLIENT_DTO.NGAYTAO = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.MANGUOITAO = Session.Session.CurrentMaNhanVien;
                _NVGDQUAY_ASYNCCLIENT_DTO.NGUOITAO = Session.Session.CurrentTenNhanVien;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAQUAYBAN = Environment.MachineName;
                _NVGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH = Session.Session.CurrentNgayPhatSinh;
                _NVGDQUAY_ASYNCCLIENT_DTO.HINHTHUCTHANHTOAN = "TIENMAT";
                _NVGDQUAY_ASYNCCLIENT_DTO.MAVOUCHER = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACHDUA = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENVOUCHER = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHEVIP = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENTRALAI = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHE = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENCOD = 0;
                decimal TTIENCOVAT = 0;
                decimal.TryParse(TongTienTraLai.Trim(), out TTIENCOVAT);
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENMAT = TTIENCOVAT;
                _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT = TTIENCOVAT;
                _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY = Session.Session.CurrentUserName;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_UPDATE_DATE = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_UPDATE_BY = Session.Session.CurrentUserName;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_STATE = "50";
                _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE = Session.Session.CurrentUnitCode;
                _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS = new List<NVHANGGDQUAY_ASYNCCLIENT>();
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            int i = 0;
                            foreach (DataGridViewRow rowData in dgvTraLai.Rows)
                            {
                                string MaVatTu = rowData.Cells["MAVATTU_TRALAI"].Value.ToString().ToUpper().Trim();
                                if (MaVatTu.Contains("BH"))
                                {
                                    string MaGiaoDichQuayPk = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK.Substring(0, _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK.LastIndexOf('-')) + "." + Session.Session.CurrentUnitCode.Split('-')[1];
                                    OracleCommand cmdGiaoDichQuayChiTiet = new OracleCommand();
                                    cmdGiaoDichQuayChiTiet.Connection = connection;
                                    cmdGiaoDichQuayChiTiet.CommandText = string.Format(@"SELECT MAVATTU,MABOPK,MAKHACHHANG,MAKEHANG,TENDAYDU,MACHUONGTRINHKM,SOLUONG,TTIENCOVAT,BARCODE,VATBAN,GIABANLECOVAT,TIENCHIETKHAU,TYLECHIETKHAU,TYLEKHUYENMAI,TIENKHUYENMAI,TYLEVOUCHER,TIENVOUCHER,TYLELAILE,GIAVON,MAVAT FROM NVHANGGDQUAY_ASYNCCLIENT WHERE MAGDQUAYPK = :MAGDQUAYPK AND MADONVI = :MADONVI AND MABOPK = :MABOPK");
                                    cmdGiaoDichQuayChiTiet.CommandType = CommandType.Text;
                                    cmdGiaoDichQuayChiTiet.Parameters.Add("MAGDQUAYPK", OracleDbType.NVarchar2, 50).Value = MaGiaoDichQuayPk;
                                    cmdGiaoDichQuayChiTiet.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                    cmdGiaoDichQuayChiTiet.Parameters.Add("MABOPK", OracleDbType.NVarchar2, 50).Value = MaVatTu;
                                    OracleDataReader dataReaderGiaoDichQuayChiTiet = null;
                                    dataReaderGiaoDichQuayChiTiet = cmdGiaoDichQuayChiTiet.ExecuteReader();
                                    if (dataReaderGiaoDichQuayChiTiet.HasRows)
                                    {
                                        while (dataReaderGiaoDichQuayChiTiet.Read())
                                        {
                                            NVHANGGDQUAY_ASYNCCLIENT VatTuDto = new NVHANGGDQUAY_ASYNCCLIENT();
                                            VatTuDto.ID = Guid.NewGuid() + "-" + i;
                                            VatTuDto.MAGDQUAYPK = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK;
                                            VatTuDto.MAKHOHANG = Session.Session.CurrentWareHouse;
                                            VatTuDto.MADONVI = Session.Session.CurrentUnitCode;
                                            decimal SOLUONG, VATBAN, GIABANLECOVAT, TYLELAILE = 0;
                                            decimal TIENCHIETKHAU = 0;
                                            decimal TIENKHUYENMAI = 0;
                                            decimal TYLECHIETKHAU = 0;
                                            decimal TYLEKHUYENMAI = 0;
                                            decimal TYLEVOUCHER = 0;
                                            decimal TIENVOUCHER = 0;
                                            decimal GIAVON = 0;
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["SOLUONG"].ToString(), out SOLUONG);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["VATBAN"].ToString(), out VATBAN);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["GIABANLECOVAT"].ToString(), out GIABANLECOVAT);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TIENCHIETKHAU"].ToString(), out TIENCHIETKHAU);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLECHIETKHAU"].ToString(), out TYLECHIETKHAU);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLEKHUYENMAI"].ToString(), out TYLEKHUYENMAI);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TIENKHUYENMAI"].ToString(), out TIENKHUYENMAI);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLEVOUCHER"].ToString(), out TYLEVOUCHER);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TIENVOUCHER"].ToString(), out TIENVOUCHER);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLELAILE"].ToString(), out TYLELAILE);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["GIAVON"].ToString(), out GIAVON);
                                            VatTuDto.TENDAYDU = dataReaderGiaoDichQuayChiTiet["TENDAYDU"].ToString();
                                            VatTuDto.BARCODE = dataReaderGiaoDichQuayChiTiet["BARCODE"].ToString();
                                            VatTuDto.MAVATTU = dataReaderGiaoDichQuayChiTiet["MAVATTU"].ToString();
                                            VatTuDto.DONVITINH = "";
                                            VatTuDto.NGUOITAO = Session.Session.CurrentTenNhanVien;
                                            VatTuDto.MABOPK = MaVatTu;
                                            VatTuDto.NGAYTAO = DateTime.Now;
                                            VatTuDto.NGAYPHATSINH = Session.Session.CurrentNgayPhatSinh;
                                            decimal SOLUONG_TRALAI = 0;
                                            decimal SOLUONGHANG_TRONGBOHANG = LAY_SOLUONG_MATHANG_TRONGBO_FROM_ORACLE(VatTuDto.MABOPK, VatTuDto.MAVATTU);
                                            decimal.TryParse(rowData.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG_TRALAI);
                                            VatTuDto.SOLUONG = SOLUONG_TRALAI * SOLUONGHANG_TRONGBOHANG;
                                            VatTuDto.TTIENCOVAT = GIABANLECOVAT * VatTuDto.SOLUONG - (TIENCHIETKHAU / SOLUONG) * VatTuDto.SOLUONG - (TIENKHUYENMAI / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.GIABANLECOVAT = GIABANLECOVAT;
                                            VatTuDto.MAKHACHHANG = dataReaderGiaoDichQuayChiTiet["MAKHACHHANG"].ToString();
                                            if (dataReaderGiaoDichQuayChiTiet["MAKEHANG"] != null)
                                            {
                                                VatTuDto.MAKEHANG =
                                                    dataReaderGiaoDichQuayChiTiet["MAKEHANG"].ToString();
                                            }
                                            else
                                            {
                                                VatTuDto.MAKEHANG = ";";
                                            }
                                            VatTuDto.MACHUONGTRINHKM = dataReaderGiaoDichQuayChiTiet["MACHUONGTRINHKM"].ToString();
                                            VatTuDto.LOAIKHUYENMAI = ";";
                                            VatTuDto.TIENCHIETKHAU = (TIENCHIETKHAU / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.TYLECHIETKHAU = 0;
                                            VatTuDto.TYLEKHUYENMAI = 0;
                                            VatTuDto.TIENKHUYENMAI = (TIENKHUYENMAI / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.TYLEVOUCHER = 0;
                                            VatTuDto.TIENVOUCHER = (TIENVOUCHER / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.GIAVON = GIAVON;
                                            VatTuDto.MAVAT = dataReaderGiaoDichQuayChiTiet["MAVAT"].ToString();
                                            VatTuDto.VATBAN = VATBAN;
                                            VatTuDto.ISBANAM = 0;
                                            _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(VatTuDto);
                                            i++;
                                        }
                                    }
                                }
                                else
                                {
                                    NVHANGGDQUAY_ASYNCCLIENT VatTuDto = new NVHANGGDQUAY_ASYNCCLIENT();
                                    VatTuDto.ID = Guid.NewGuid() + "-" + i;
                                    VatTuDto.MAGDQUAYPK = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK;
                                    VatTuDto.MAKHOHANG = Session.Session.CurrentWareHouse;
                                    VatTuDto.MADONVI = Session.Session.CurrentUnitCode;
                                    VatTuDto.MAVATTU = MaVatTu;
                                    VatTuDto.DONVITINH = rowData.Cells["DONVITINH_TRALAI"].Value.ToString().ToUpper().Trim();
                                    VatTuDto.TENDAYDU = rowData.Cells["TENVATTU_TRALAI"].Value.ToString().ToUpper().Trim();
                                    VatTuDto.NGUOITAO = Session.Session.CurrentTenNhanVien;
                                    VatTuDto.MABOPK = "BH";
                                    VatTuDto.NGAYTAO = DateTime.Now;
                                    VatTuDto.NGAYPHATSINH = Session.Session.CurrentNgayPhatSinh;
                                    decimal SOLUONG = 0;
                                    decimal.TryParse(rowData.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG);
                                    VatTuDto.SOLUONG = SOLUONG;
                                    decimal DONGIA = 0;
                                    decimal.TryParse(rowData.Cells["DONGIA_TRALAI"].Value.ToString(), out DONGIA);
                                    VatTuDto.GIABANLECOVAT = DONGIA;
                                    decimal GIAVON = 0;
                                    decimal.TryParse(rowData.Cells["GIAVON_TRALAI"].Value.ToString(), out GIAVON);
                                    VatTuDto.GIAVON = GIAVON;
                                    VatTuDto.ISBANAM = 0;
                                    string MaGiaoDichQuayPk = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK.Substring(0, _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK.LastIndexOf('-')) + "." + Session.Session.CurrentUnitCode.Split('-')[1];
                                    OracleCommand cmdGiaoDichQuayDetails = new OracleCommand();
                                    cmdGiaoDichQuayDetails.Connection = connection;
                                    cmdGiaoDichQuayDetails.CommandText = string.Format(@"SELECT MAVATTU,MABOPK,TENDAYDU,MACHUONGTRINHKM,MAKHACHHANG,MAKEHANG,LOAIKHUYENMAI,SOLUONG,TTIENCOVAT,BARCODE,VATBAN,GIABANLECOVAT,TIENCHIETKHAU,TYLECHIETKHAU,TYLEKHUYENMAI,TIENKHUYENMAI,TYLEVOUCHER,TIENVOUCHER,TYLELAILE,GIAVON,MAVAT FROM NVHANGGDQUAY_ASYNCCLIENT WHERE MAGDQUAYPK = :MAGDQUAYPK AND MADONVI = :MADONVI AND MAVATTU = :MAVATTU");
                                    cmdGiaoDichQuayDetails.CommandType = CommandType.Text;
                                    cmdGiaoDichQuayDetails.Parameters.Add("MAGDQUAYPK", OracleDbType.NVarchar2, 50).Value = MaGiaoDichQuayPk;
                                    cmdGiaoDichQuayDetails.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                    cmdGiaoDichQuayDetails.Parameters.Add("MAVATTU", OracleDbType.NVarchar2, 50).Value = MaVatTu;
                                    OracleDataReader dataReaderGdQuayDetails = null;
                                    dataReaderGdQuayDetails = cmdGiaoDichQuayDetails.ExecuteReader();
                                    decimal SOLUONG_TRONG_GIAODICH = 0;
                                    if (dataReaderGdQuayDetails.HasRows)
                                    {
                                        while (dataReaderGdQuayDetails.Read())
                                        {
                                            VatTuDto.MAKHACHHANG = dataReaderGdQuayDetails["MAKHACHHANG"].ToString().Trim();
                                            VatTuDto.MACHUONGTRINHKM = dataReaderGdQuayDetails["MACHUONGTRINHKM"].ToString().Trim();
                                            if (dataReaderGdQuayDetails["LOAIKHUYENMAI"] != null)
                                            {
                                                VatTuDto.LOAIKHUYENMAI = dataReaderGdQuayDetails["LOAIKHUYENMAI"].ToString();
                                            }
                                            else
                                            {
                                                VatTuDto.LOAIKHUYENMAI = ";";
                                            }
                                            decimal TIENCHIETKHAU, TYLECHIETKHAU, TYLEKHUYENMAI, TYLEVOUCHER, TIENVOUCHER, TIENKHUYENMAI_TRONG_GD, GIABANLECOVAT_TRONG_GD = 0;
                                            decimal.TryParse(dataReaderGdQuayDetails["TIENCHIETKHAU"].ToString(), out TIENCHIETKHAU);
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLECHIETKHAU"].ToString(), out TYLECHIETKHAU);
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLEKHUYENMAI"].ToString(), out TYLEKHUYENMAI);
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLEVOUCHER"].ToString(), out TYLEVOUCHER);
                                            decimal.TryParse(dataReaderGdQuayDetails["TIENVOUCHER"].ToString(), out TIENVOUCHER);
                                            decimal.TryParse(dataReaderGdQuayDetails["SOLUONG"].ToString(), out SOLUONG_TRONG_GIAODICH);
                                            decimal.TryParse(dataReaderGdQuayDetails["TIENKHUYENMAI"].ToString(), out TIENKHUYENMAI_TRONG_GD);
                                            decimal.TryParse(dataReaderGdQuayDetails["GIABANLECOVAT"].ToString(), out GIABANLECOVAT_TRONG_GD);
                                            VatTuDto.BARCODE = dataReaderGdQuayDetails["BARCODE"].ToString().Trim();
                                            VatTuDto.TIENCHIETKHAU = (TIENCHIETKHAU / SOLUONG_TRONG_GIAODICH) * VatTuDto.SOLUONG;
                                            VatTuDto.TIENKHUYENMAI = (TIENKHUYENMAI_TRONG_GD / SOLUONG_TRONG_GIAODICH) * VatTuDto.SOLUONG;
                                            VatTuDto.TYLECHIETKHAU = 0;
                                            VatTuDto.TYLEKHUYENMAI = 0;
                                            decimal TYLEVATRA, TYLELAILE = 0;
                                            decimal.TryParse(dataReaderGdQuayDetails["VATBAN"].ToString(), out TYLEVATRA);
                                            VatTuDto.VATBAN = TYLEVATRA;
                                            if (dataReaderGdQuayDetails["MAKEHANG"] != null)
                                            {
                                                VatTuDto.MAKEHANG =
                                                    dataReaderGdQuayDetails["MAKEHANG"].ToString().Trim();
                                            }
                                            else
                                            {
                                                VatTuDto.MAKEHANG = ";";
                                            }
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLELAILE"].ToString(), out TYLELAILE);
                                            VatTuDto.TYLELAILE = TYLELAILE;
                                            VatTuDto.MAVAT = dataReaderGdQuayDetails["MAVAT"].ToString().Trim();
                                            VatTuDto.TYLEVOUCHER = TYLEVOUCHER;
                                            VatTuDto.TIENVOUCHER = TIENVOUCHER;
                                            VatTuDto.TTIENCOVAT = GIABANLECOVAT_TRONG_GD * VatTuDto.SOLUONG - VatTuDto.TIENKHUYENMAI - VatTuDto.TIENCHIETKHAU;
                                        }
                                    }
                                    _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(VatTuDto);
                                    i++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("XẢY RA LỖI KHI KHỞI TẠO DỮ LIỆU LƯU TRỮ BÁN TRẢ LẠI ONLINE");
                            WriteLogs.LogError(ex);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }
                    }
                }
            }
            return _NVGDQUAY_ASYNCCLIENT_DTO;
        }


        public static NVGDQUAY_ASYNCCLIENT_DTO KHOITAO_DULIEU_THANHTOAN_BANTRALAI_FROM_SQLSERVER(string MaGiaoDichTraLai, string TongTienTraLai, DataGridView dgvTraLai)
        {
            NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
            if (dgvTraLai.RowCount > 0)
            {
                _NVGDQUAY_ASYNCCLIENT_DTO.ID = Guid.NewGuid() + "-" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Hour + DateTime.Now.Minute;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICH = MaGiaoDichTraLai.Trim();
                _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK = MaGiaoDichTraLai.Trim() + "." + Session.Session.CurrentUnitCode.Split('-')[1];
                _NVGDQUAY_ASYNCCLIENT_DTO.MADONVI = Session.Session.CurrentUnitCode;
                _NVGDQUAY_ASYNCCLIENT_DTO.LOAIGIAODICH = 2;
                _NVGDQUAY_ASYNCCLIENT_DTO.NGAYTAO = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.MANGUOITAO = Session.Session.CurrentMaNhanVien;
                _NVGDQUAY_ASYNCCLIENT_DTO.NGUOITAO = Session.Session.CurrentTenNhanVien;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAQUAYBAN = Environment.MachineName;
                _NVGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH = Session.Session.CurrentNgayPhatSinh;
                _NVGDQUAY_ASYNCCLIENT_DTO.HINHTHUCTHANHTOAN = "TIENMAT";
                _NVGDQUAY_ASYNCCLIENT_DTO.MAVOUCHER = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACHDUA = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENVOUCHER = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHEVIP = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENTRALAI = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENTHE = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENCOD = 0;
                decimal TTIENCOVAT = 0;
                decimal.TryParse(TongTienTraLai.Trim(), out TTIENCOVAT);
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENMAT = TTIENCOVAT;
                _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT = TTIENCOVAT;
                _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_DATE = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_CREATE_BY = Session.Session.CurrentUserName;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_UPDATE_DATE = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_UPDATE_BY = Session.Session.CurrentUserName;
                _NVGDQUAY_ASYNCCLIENT_DTO.I_STATE = "50";
                _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE = Session.Session.CurrentUnitCode;
                _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS = new List<NVHANGGDQUAY_ASYNCCLIENT>();
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            int i = 0;
                            foreach (DataGridViewRow rowData in dgvTraLai.Rows)
                            {
                                string MaVatTu = rowData.Cells["MAVATTU_TRALAI"].Value.ToString().ToUpper().Trim();
                                if (MaVatTu.Contains("BH"))
                                {
                                    string MaGiaoDichQuayPk = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK.Substring(0, _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK.LastIndexOf('-')) + "." + Session.Session.CurrentUnitCode.Split('-')[1];
                                    SqlCommand cmdGiaoDichQuayChiTiet = new SqlCommand();
                                    cmdGiaoDichQuayChiTiet.Connection = connection;
                                    cmdGiaoDichQuayChiTiet.CommandText = string.Format(@"SELECT [MAGDQUAYPK],[MAKHOHANG],[MADONVI],[MAVATTU],[MANGUOITAO],[NGUOITAO],[MABOPK],[NGAYTAO],[NGAYPHATSINH] ,[SOLUONG],[TTIENCOVAT],[GIABANLECOVAT],[TYLECHIETKHAU]
                                    ,[TIENCHIETKHAU],[TYLEKHUYENMAI],[TIENKHUYENMAI],[TYLEVOUCHER],[TIENVOUCHER],[TYLELAILE],[GIAVON],[MAVAT],[VATBAN],[MACHUONGTRINHKM],[UNITCODE] FROM [dbo].[NVHANGGDQUAY_ASYNCCLIENT] WHERE MAGDQUAYPK = @MAGDQUAYPK AND MADONVI = @MADONVI AND MABOPK = @MABOPK");
                                    cmdGiaoDichQuayChiTiet.CommandType = CommandType.Text;
                                    cmdGiaoDichQuayChiTiet.Parameters.Add("MAGDQUAYPK", SqlDbType.NVarChar, 50).Value = MaGiaoDichQuayPk;
                                    cmdGiaoDichQuayChiTiet.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentUnitCode;
                                    cmdGiaoDichQuayChiTiet.Parameters.Add("MABOPK", SqlDbType.NVarChar, 50).Value = MaVatTu;
                                    SqlDataReader dataReaderGiaoDichQuayChiTiet = null;
                                    dataReaderGiaoDichQuayChiTiet = cmdGiaoDichQuayChiTiet.ExecuteReader();
                                    if (dataReaderGiaoDichQuayChiTiet.HasRows)
                                    {
                                        while (dataReaderGiaoDichQuayChiTiet.Read())
                                        {
                                            NVHANGGDQUAY_ASYNCCLIENT VatTuDto = new NVHANGGDQUAY_ASYNCCLIENT();
                                            VatTuDto.ID = Guid.NewGuid() + "-" + i;
                                            VatTuDto.MAGDQUAYPK = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK;
                                            VatTuDto.MAKHOHANG = Session.Session.CurrentWareHouse;
                                            VatTuDto.MADONVI = Session.Session.CurrentUnitCode;
                                            decimal SOLUONG, VATBAN, GIABANLECOVAT, TYLELAILE = 0;
                                            decimal TIENCHIETKHAU = 0;
                                            decimal TIENKHUYENMAI = 0;
                                            decimal TYLECHIETKHAU = 0;
                                            decimal TYLEKHUYENMAI = 0;
                                            decimal TYLEVOUCHER = 0;
                                            decimal TIENVOUCHER = 0;
                                            decimal GIAVON = 0;
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["SOLUONG"].ToString(), out SOLUONG);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["VATBAN"].ToString(), out VATBAN);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["GIABANLECOVAT"].ToString(), out GIABANLECOVAT);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TIENCHIETKHAU"].ToString(), out TIENCHIETKHAU);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLECHIETKHAU"].ToString(), out TYLECHIETKHAU);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLEKHUYENMAI"].ToString(), out TYLEKHUYENMAI);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TIENKHUYENMAI"].ToString(), out TIENKHUYENMAI);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLEVOUCHER"].ToString(), out TYLEVOUCHER);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TIENVOUCHER"].ToString(), out TIENVOUCHER);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["TYLELAILE"].ToString(), out TYLELAILE);
                                            decimal.TryParse(dataReaderGiaoDichQuayChiTiet["GIAVON"].ToString(), out GIAVON);
                                            VatTuDto.MAVATTU = dataReaderGiaoDichQuayChiTiet["MAVATTU"].ToString();
                                            EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
                                            _EXTEND_VATTU_DTO = FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(VatTuDto.MAVATTU, _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE);
                                            VatTuDto.TENDAYDU = _EXTEND_VATTU_DTO.TENVATTU;
                                            VatTuDto.BARCODE = _EXTEND_VATTU_DTO.BARCODE;
                                            VatTuDto.DONVITINH = _EXTEND_VATTU_DTO.DONVITINH;
                                            VatTuDto.NGUOITAO = Session.Session.CurrentTenNhanVien;
                                            VatTuDto.MABOPK = MaVatTu;
                                            VatTuDto.NGAYTAO = DateTime.Now;
                                            VatTuDto.NGAYPHATSINH = Session.Session.CurrentNgayPhatSinh;
                                            decimal SOLUONG_TRALAI = 0;
                                            decimal SOLUONGHANG_TRONGBOHANG = LAY_SOLUONG_MATHANG_TRONGBO_FROM_SQLSERVER(VatTuDto.MABOPK, VatTuDto.MAVATTU);
                                            decimal.TryParse(rowData.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG_TRALAI);
                                            VatTuDto.SOLUONG = SOLUONG_TRALAI * SOLUONGHANG_TRONGBOHANG;
                                            VatTuDto.TTIENCOVAT = GIABANLECOVAT * VatTuDto.SOLUONG - (TIENCHIETKHAU / SOLUONG) * VatTuDto.SOLUONG - (TIENKHUYENMAI / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.GIABANLECOVAT = GIABANLECOVAT;
                                            VatTuDto.MAKHACHHANG = dataReaderGiaoDichQuayChiTiet["MAKHACHHANG"].ToString();
                                            VatTuDto.MAKEHANG = _EXTEND_VATTU_DTO.MAKEHANG;
                                            VatTuDto.MACHUONGTRINHKM = dataReaderGiaoDichQuayChiTiet["MACHUONGTRINHKM"].ToString();
                                            VatTuDto.LOAIKHUYENMAI = ";";
                                            VatTuDto.TIENCHIETKHAU = (TIENCHIETKHAU / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.TYLECHIETKHAU = 0;
                                            VatTuDto.TYLEKHUYENMAI = 0;
                                            VatTuDto.TIENKHUYENMAI = (TIENKHUYENMAI / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.TYLEVOUCHER = 0;
                                            VatTuDto.TIENVOUCHER = (TIENVOUCHER / SOLUONG) * VatTuDto.SOLUONG;
                                            VatTuDto.GIAVON = GIAVON;
                                            VatTuDto.MAVAT = dataReaderGiaoDichQuayChiTiet["MAVAT"].ToString();
                                            VatTuDto.VATBAN = VATBAN;
                                            VatTuDto.ISBANAM = 0;
                                            _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(VatTuDto);
                                            i++;
                                        }
                                    }
                                }
                                else
                                {
                                    NVHANGGDQUAY_ASYNCCLIENT VatTuDto = new NVHANGGDQUAY_ASYNCCLIENT();
                                    VatTuDto.ID = Guid.NewGuid() + "-" + i;
                                    VatTuDto.MAGDQUAYPK = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK;
                                    VatTuDto.MAKHOHANG = Session.Session.CurrentWareHouse;
                                    VatTuDto.MADONVI = Session.Session.CurrentUnitCode;
                                    VatTuDto.MAVATTU = MaVatTu;
                                    VatTuDto.DONVITINH = rowData.Cells["DONVITINH_TRALAI"].Value.ToString().ToUpper().Trim();
                                    VatTuDto.TENDAYDU = rowData.Cells["TENVATTU_TRALAI"].Value.ToString().ToUpper().Trim();
                                    VatTuDto.NGUOITAO = Session.Session.CurrentTenNhanVien;
                                    VatTuDto.MABOPK = "BH";
                                    VatTuDto.NGAYTAO = DateTime.Now;
                                    VatTuDto.NGAYPHATSINH = Session.Session.CurrentNgayPhatSinh;
                                    decimal SOLUONG = 0;
                                    decimal.TryParse(rowData.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG);
                                    VatTuDto.SOLUONG = SOLUONG;
                                    decimal DONGIA = 0;
                                    decimal.TryParse(rowData.Cells["DONGIA_TRALAI"].Value.ToString(), out DONGIA);
                                    VatTuDto.GIABANLECOVAT = DONGIA;
                                    decimal GIAVON = 0;
                                    decimal.TryParse(rowData.Cells["GIAVON_TRALAI"].Value.ToString(), out GIAVON);
                                    VatTuDto.GIAVON = GIAVON;
                                    VatTuDto.ISBANAM = 0;
                                    string MaGiaoDichQuayPk = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK.Substring(0, _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK.LastIndexOf('-')) + "." + Session.Session.CurrentUnitCode.Split('-')[1];
                                    SqlCommand cmdGiaoDichQuayDetails = new SqlCommand();
                                    cmdGiaoDichQuayDetails.Connection = connection;
                                    cmdGiaoDichQuayDetails.CommandText = string.Format(@"SELECT [MAGDQUAYPK],[MAKHOHANG],[MADONVI],[MAVATTU],[MANGUOITAO],[NGUOITAO],[MABOPK],[NGAYTAO],[NGAYPHATSINH] ,[SOLUONG],[TTIENCOVAT],[GIABANLECOVAT],[TYLECHIETKHAU]
                                    ,[TIENCHIETKHAU],[TYLEKHUYENMAI],[TIENKHUYENMAI],[TYLEVOUCHER],[TIENVOUCHER],[TYLELAILE],[GIAVON],[MAVAT],[VATBAN],[MACHUONGTRINHKM],[UNITCODE] 
                                    FROM [dbo].[NVHANGGDQUAY_ASYNCCLIENT] WHERE MAGDQUAYPK = :MAGDQUAYPK AND MADONVI = @MADONVI AND MAVATTU = @MAVATTU");
                                    cmdGiaoDichQuayDetails.CommandType = CommandType.Text;
                                    cmdGiaoDichQuayDetails.Parameters.Add("MAGDQUAYPK", SqlDbType.NVarChar, 50).Value = MaGiaoDichQuayPk;
                                    cmdGiaoDichQuayDetails.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentUnitCode;
                                    cmdGiaoDichQuayDetails.Parameters.Add("MAVATTU", SqlDbType.NVarChar, 50).Value = MaVatTu;
                                    SqlDataReader dataReaderGdQuayDetails = null;
                                    dataReaderGdQuayDetails = cmdGiaoDichQuayDetails.ExecuteReader();
                                    decimal SOLUONG_TRONG_GIAODICH = 0;
                                    if (dataReaderGdQuayDetails.HasRows)
                                    {
                                        while (dataReaderGdQuayDetails.Read())
                                        {
                                            if (dataReaderGdQuayDetails["MACHUONGTRINHKM"] != null)
                                            {
                                                VatTuDto.MACHUONGTRINHKM = dataReaderGdQuayDetails["MACHUONGTRINHKM"].ToString().Trim();
                                            }
                                           
                                            if (dataReaderGdQuayDetails["LOAIKHUYENMAI"] != null)
                                            {
                                                VatTuDto.LOAIKHUYENMAI = dataReaderGdQuayDetails["LOAIKHUYENMAI"].ToString();
                                            }
                                            else
                                            {
                                                VatTuDto.LOAIKHUYENMAI = ";";
                                            }
                                            decimal TIENCHIETKHAU, TYLECHIETKHAU, TYLEKHUYENMAI, TYLEVOUCHER, TIENVOUCHER, TIENKHUYENMAI_TRONG_GD, GIABANLECOVAT_TRONG_GD = 0;
                                            decimal.TryParse(dataReaderGdQuayDetails["TIENCHIETKHAU"].ToString(), out TIENCHIETKHAU);
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLECHIETKHAU"].ToString(), out TYLECHIETKHAU);
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLEKHUYENMAI"].ToString(), out TYLEKHUYENMAI);
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLEVOUCHER"].ToString(), out TYLEVOUCHER);
                                            decimal.TryParse(dataReaderGdQuayDetails["TIENVOUCHER"].ToString(), out TIENVOUCHER);
                                            decimal.TryParse(dataReaderGdQuayDetails["SOLUONG"].ToString(), out SOLUONG_TRONG_GIAODICH);
                                            decimal.TryParse(dataReaderGdQuayDetails["TIENKHUYENMAI"].ToString(), out TIENKHUYENMAI_TRONG_GD);
                                            decimal.TryParse(dataReaderGdQuayDetails["GIABANLECOVAT"].ToString(), out GIABANLECOVAT_TRONG_GD);
                                            EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
                                            _EXTEND_VATTU_DTO =
                                                FrmXuatBanLeService.LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(MaVatTu,
                                                    _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE);
                                            VatTuDto.BARCODE = _EXTEND_VATTU_DTO.BARCODE;
                                            VatTuDto.TIENCHIETKHAU = (TIENCHIETKHAU / SOLUONG_TRONG_GIAODICH) * VatTuDto.SOLUONG;
                                            VatTuDto.TIENKHUYENMAI = (TIENKHUYENMAI_TRONG_GD / SOLUONG_TRONG_GIAODICH) * VatTuDto.SOLUONG;
                                            VatTuDto.TYLECHIETKHAU = 0;
                                            VatTuDto.TYLEKHUYENMAI = 0;
                                            decimal TYLEVATRA, TYLELAILE = 0;
                                            decimal.TryParse(dataReaderGdQuayDetails["VATBAN"].ToString(), out TYLEVATRA);
                                            VatTuDto.VATBAN = TYLEVATRA;
                                            if (dataReaderGdQuayDetails["MAKEHANG"] != null)
                                            {
                                                VatTuDto.MAKEHANG =
                                                    dataReaderGdQuayDetails["MAKEHANG"].ToString().Trim();
                                            }
                                            else
                                            {
                                                VatTuDto.MAKEHANG = ";";
                                            }
                                            decimal.TryParse(dataReaderGdQuayDetails["TYLELAILE"].ToString(), out TYLELAILE);
                                            VatTuDto.TYLELAILE = TYLELAILE;
                                            VatTuDto.MAVAT = dataReaderGdQuayDetails["MAVAT"].ToString().Trim();
                                            VatTuDto.TYLEVOUCHER = TYLEVOUCHER;
                                            VatTuDto.TIENVOUCHER = TIENVOUCHER;
                                            VatTuDto.TTIENCOVAT = GIABANLECOVAT_TRONG_GD * VatTuDto.SOLUONG - VatTuDto.TIENKHUYENMAI - VatTuDto.TIENCHIETKHAU;
                                        }
                                    }
                                    _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(VatTuDto);
                                    i++;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("XẢY RA LỖI KHI KHỞI TẠO DỮ LIỆU LƯU TRỮ BÁN TRẢ LẠI OFFLINE");
                            WriteLogs.LogError(ex);
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }
                    }
                }
            }
            return _NVGDQUAY_ASYNCCLIENT_DTO;
        }

        public static NVGDQUAY_ASYNCCLIENT_DTO KHOITAO_BILL_THANHTOAN_BANTRALAI_FROM_ORACLE(string MaGiaoDichTraLai, string TongTienTraLai, DataGridView dgvTraLai)
        {
            NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
            if (dgvTraLai.RowCount > 0)
            {
                _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICH = MaGiaoDichTraLai.Trim();
                _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK = MaGiaoDichTraLai.Trim() + "." + Session.Session.CurrentUnitCode.Split('-')[1];
                _NVGDQUAY_ASYNCCLIENT_DTO.NGAYTAO = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.MANGUOITAO = Session.Session.CurrentMaNhanVien;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.NGUOITAO = Session.Session.CurrentTenNhanVien;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAQUAYBAN = Environment.MachineName;
                _NVGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH = Session.Session.CurrentNgayPhatSinh;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACHDUA = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENTRALAI = 0;
                decimal TTIENCOVAT = 0;
                decimal.TryParse(TongTienTraLai, out TTIENCOVAT);
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENMAT = TTIENCOVAT;
                _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT = TTIENCOVAT;
                _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE = Session.Session.CurrentUnitCode;
                _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS = new List<NVHANGGDQUAY_ASYNCCLIENT>();
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            int i = 0;
                            foreach (DataGridViewRow rowData in dgvTraLai.Rows)
                            {
                                NVHANGGDQUAY_ASYNCCLIENT _NVHANGGDQUAY_ASYNCCLIENT = new NVHANGGDQUAY_ASYNCCLIENT();
                                _NVHANGGDQUAY_ASYNCCLIENT.ID = Guid.NewGuid() + "-" + i;
                                _NVHANGGDQUAY_ASYNCCLIENT.MAGDQUAYPK = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK;
                                _NVHANGGDQUAY_ASYNCCLIENT.MADONVI = _NVGDQUAY_ASYNCCLIENT_DTO.MADONVI;
                                _NVHANGGDQUAY_ASYNCCLIENT.MAVATTU = rowData.Cells["MAVATTU_TRALAI"].Value.ToString().ToUpper().Trim();
                                _NVHANGGDQUAY_ASYNCCLIENT.DONVITINH = rowData.Cells["DONVITINH_TRALAI"].Value.ToString().ToUpper().Trim();
                                _NVHANGGDQUAY_ASYNCCLIENT.TENDAYDU = rowData.Cells["TENVATTU_TRALAI"].Value.ToString().ToUpper().Trim();
                                _NVHANGGDQUAY_ASYNCCLIENT.NGUOITAO = _NVGDQUAY_ASYNCCLIENT_DTO.NGUOITAO;
                                _NVHANGGDQUAY_ASYNCCLIENT.NGAYTAO = _NVGDQUAY_ASYNCCLIENT_DTO.NGAYTAO;
                                _NVHANGGDQUAY_ASYNCCLIENT.NGAYPHATSINH = _NVGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH;
                                decimal SOLUONG = 0;
                                decimal.TryParse(rowData.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG);
                                _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG = SOLUONG;
                                decimal THANHTIEN = 0;
                                decimal.TryParse(rowData.Cells["THANHTIEN_TRALAI"].Value.ToString(), out THANHTIEN);
                                _NVHANGGDQUAY_ASYNCCLIENT.TTIENCOVAT = THANHTIEN;
                                decimal DONGIA = 0;
                                decimal.TryParse(rowData.Cells["DONGIA_TRALAI"].Value.ToString(), out DONGIA);
                                _NVHANGGDQUAY_ASYNCCLIENT.GIABANLECOVAT = DONGIA;
                                decimal TIENCK = 0;
                                decimal.TryParse(rowData.Cells["TIENKM_TRALAI"].Value.ToString(), out TIENCK);
                                _NVHANGGDQUAY_ASYNCCLIENT.TIENKHUYENMAI = TIENCK;
                                decimal GIAVON = 0;
                                decimal.TryParse(rowData.Cells["GIAVON_TRALAI"].Value.ToString(), out GIAVON);
                                _NVHANGGDQUAY_ASYNCCLIENT.GIAVON = GIAVON;
                                _NVHANGGDQUAY_ASYNCCLIENT.ISBANAM = 0;
                                string MaGiaoDichQuayPk = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK.Substring(0, _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK.LastIndexOf('-')) + "." + Session.Session.CurrentUnitCode.Split('-')[1];
                                OracleCommand cmdGiaoDichQuayDetails = new OracleCommand();
                                cmdGiaoDichQuayDetails.Connection = connection;
                                cmdGiaoDichQuayDetails.CommandText = string.Format(@"SELECT MAVATTU,MABOPK,TENDAYDU,MACHUONGTRINHKM,MAKHACHHANG,MAKEHANG,LOAIKHUYENMAI,SOLUONG,TTIENCOVAT,BARCODE,VATBAN,GIABANLECOVAT,TIENCHIETKHAU,TYLECHIETKHAU,TYLEKHUYENMAI,TIENKHUYENMAI,TYLEVOUCHER,TIENVOUCHER,TYLELAILE,GIAVON,MAVAT FROM NVHANGGDQUAY_ASYNCCLIENT WHERE MAGDQUAYPK = :MAGDQUAYPK AND MADONVI = :MADONVI AND MAVATTU = :MAVATTU");
                                cmdGiaoDichQuayDetails.CommandType = CommandType.Text;
                                cmdGiaoDichQuayDetails.Parameters.Add("MAGDQUAYPK", OracleDbType.NVarchar2, 50).Value = MaGiaoDichQuayPk;
                                cmdGiaoDichQuayDetails.Parameters.Add("MADONVI", OracleDbType.NVarchar2, 50).Value = Session.Session.CurrentUnitCode;
                                cmdGiaoDichQuayDetails.Parameters.Add("MAVATTU", OracleDbType.NVarchar2, 50).Value = _NVHANGGDQUAY_ASYNCCLIENT.MAVATTU;
                                OracleDataReader dataReaderGdQuayDetails = null;
                                dataReaderGdQuayDetails = cmdGiaoDichQuayDetails.ExecuteReader();
                                if (dataReaderGdQuayDetails.HasRows)
                                {
                                    while (dataReaderGdQuayDetails.Read())
                                    {
                                        decimal TYLEVATRA, TYLELAILE, TIEN_KHUYENMAI_GD, TIEN_CK_GD, SOLUONG_GD, GIABANLECOVAT_GD, GIAVON_GD = 0;
                                        decimal.TryParse(dataReaderGdQuayDetails["VATBAN"].ToString(), out TYLEVATRA);
                                        _NVHANGGDQUAY_ASYNCCLIENT.VATBAN = TYLEVATRA;
                                        decimal.TryParse(dataReaderGdQuayDetails["TYLELAILE"].ToString(), out TYLELAILE);
                                        _NVHANGGDQUAY_ASYNCCLIENT.TYLELAILE = TYLELAILE;
                                        _NVHANGGDQUAY_ASYNCCLIENT.MAVAT = dataReaderGdQuayDetails["MAVAT"].ToString().Trim();
                                        decimal.TryParse(dataReaderGdQuayDetails["TIENKHUYENMAI"].ToString(), out TIEN_KHUYENMAI_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["TIENCHIETKHAU"].ToString(), out TIEN_CK_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["SOLUONG"].ToString(), out SOLUONG_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["GIABANLECOVAT"].ToString(), out GIABANLECOVAT_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["GIAVON"].ToString(), out GIAVON_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.TTIENCOVAT = _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * GIABANLECOVAT_GD - _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_KHUYENMAI_GD / SOLUONG_GD) - _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_CK_GD / SOLUONG_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.GIABANLECOVAT = GIABANLECOVAT_GD;
                                        _NVHANGGDQUAY_ASYNCCLIENT.TIENKHUYENMAI = _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_KHUYENMAI_GD / SOLUONG_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.TIENCHIETKHAU = _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_CK_GD / SOLUONG_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.GIAVON = GIAVON_GD;
                                    }
                                }
                                _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(_NVHANGGDQUAY_ASYNCCLIENT);
                                i++;
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
                }
            }
            return _NVGDQUAY_ASYNCCLIENT_DTO;
        }

        public static NVGDQUAY_ASYNCCLIENT_DTO KHOITAO_BILL_THANHTOAN_BANTRALAI_FROM_SQLSERVER(string MaGiaoDichTraLai, string TongTienTraLai, DataGridView dgvTraLai)
        {
            NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_DTO = new NVGDQUAY_ASYNCCLIENT_DTO();
            if (dgvTraLai.RowCount > 0)
            {
                _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICH = MaGiaoDichTraLai.Trim();
                _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK = MaGiaoDichTraLai.Trim() + "." + Session.Session.CurrentUnitCode.Split('-')[1];
                _NVGDQUAY_ASYNCCLIENT_DTO.NGAYTAO = DateTime.Now;
                _NVGDQUAY_ASYNCCLIENT_DTO.MANGUOITAO = Session.Session.CurrentMaNhanVien;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.NGUOITAO = Session.Session.CurrentTenNhanVien;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAQUAYBAN = Environment.MachineName;
                _NVGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH = Session.Session.CurrentNgayPhatSinh;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENKHACHDUA = 0;
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENTRALAI = 0;
                decimal TTIENCOVAT = 0;
                decimal.TryParse(TongTienTraLai, out TTIENCOVAT);
                _NVGDQUAY_ASYNCCLIENT_DTO.TIENMAT = TTIENCOVAT;
                _NVGDQUAY_ASYNCCLIENT_DTO.TTIENCOVAT = TTIENCOVAT;
                _NVGDQUAY_ASYNCCLIENT_DTO.THOIGIAN = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                _NVGDQUAY_ASYNCCLIENT_DTO.MAKHACHHANG = "";
                _NVGDQUAY_ASYNCCLIENT_DTO.UNITCODE = Session.Session.CurrentUnitCode;
                _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS = new List<NVHANGGDQUAY_ASYNCCLIENT>();
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        try
                        {
                            int i = 0;
                            foreach (DataGridViewRow rowData in dgvTraLai.Rows)
                            {
                                NVHANGGDQUAY_ASYNCCLIENT _NVHANGGDQUAY_ASYNCCLIENT = new NVHANGGDQUAY_ASYNCCLIENT();
                                _NVHANGGDQUAY_ASYNCCLIENT.ID = Guid.NewGuid() + "-" + i;
                                _NVHANGGDQUAY_ASYNCCLIENT.MAGDQUAYPK = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK;
                                _NVHANGGDQUAY_ASYNCCLIENT.MADONVI = _NVGDQUAY_ASYNCCLIENT_DTO.MADONVI;
                                _NVHANGGDQUAY_ASYNCCLIENT.MAVATTU = rowData.Cells["MAVATTU_TRALAI"].Value.ToString().ToUpper().Trim();
                                _NVHANGGDQUAY_ASYNCCLIENT.DONVITINH = rowData.Cells["DONVITINH_TRALAI"].Value.ToString().ToUpper().Trim();
                                _NVHANGGDQUAY_ASYNCCLIENT.TENDAYDU = rowData.Cells["TENVATTU_TRALAI"].Value.ToString().ToUpper().Trim();
                                _NVHANGGDQUAY_ASYNCCLIENT.NGUOITAO = _NVGDQUAY_ASYNCCLIENT_DTO.NGUOITAO;
                                _NVHANGGDQUAY_ASYNCCLIENT.NGAYTAO = _NVGDQUAY_ASYNCCLIENT_DTO.NGAYTAO;
                                _NVHANGGDQUAY_ASYNCCLIENT.NGAYPHATSINH = _NVGDQUAY_ASYNCCLIENT_DTO.NGAYPHATSINH;
                                decimal SOLUONG = 0;
                                decimal.TryParse(rowData.Cells["SOLUONG_TRALAI"].Value.ToString(), out SOLUONG);
                                _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG = SOLUONG;
                                decimal THANHTIEN = 0;
                                decimal.TryParse(rowData.Cells["THANHTIEN_TRALAI"].Value.ToString(), out THANHTIEN);
                                _NVHANGGDQUAY_ASYNCCLIENT.TTIENCOVAT = THANHTIEN;
                                decimal DONGIA = 0;
                                decimal.TryParse(rowData.Cells["DONGIA_TRALAI"].Value.ToString(), out DONGIA);
                                _NVHANGGDQUAY_ASYNCCLIENT.GIABANLECOVAT = DONGIA;
                                decimal TIENCK = 0;
                                decimal.TryParse(rowData.Cells["TIENKM_TRALAI"].Value.ToString(), out TIENCK);
                                _NVHANGGDQUAY_ASYNCCLIENT.TIENKHUYENMAI = TIENCK;
                                decimal GIAVON = 0;
                                decimal.TryParse(rowData.Cells["GIAVON_TRALAI"].Value.ToString(), out GIAVON);
                                _NVHANGGDQUAY_ASYNCCLIENT.GIAVON = GIAVON;
                                _NVHANGGDQUAY_ASYNCCLIENT.ISBANAM = 0;
                                string MaGiaoDichQuayPk = _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK.Substring(0, _NVGDQUAY_ASYNCCLIENT_DTO.MAGIAODICHQUAYPK.LastIndexOf('-')) + "." + Session.Session.CurrentUnitCode.Split('-')[1];
                                SqlCommand cmdGiaoDichQuayDetails = new SqlCommand();
                                cmdGiaoDichQuayDetails.Connection = connection;
                                cmdGiaoDichQuayDetails.CommandText = string.Format(@"SELECT [MAGDQUAYPK],[MAKHOHANG],[MADONVI],[MAVATTU],[MANGUOITAO],[NGUOITAO],[MABOPK],[NGAYTAO],
                                    [NGAYPHATSINH] ,[SOLUONG],[TTIENCOVAT],[GIABANLECOVAT],[TYLECHIETKHAU]
                                  ,[TIENCHIETKHAU],[TYLEKHUYENMAI],[TIENKHUYENMAI],[TYLEVOUCHER],[TIENVOUCHER],[TYLELAILE],[GIAVON],[MAVAT],[VATBAN],[MACHUONGTRINHKM],[UNITCODE] FROM [dbo].[NVHANGGDQUAY_ASYNCCLIENT]
                                    WHERE MAGDQUAYPK = @MAGDQUAYPK AND MADONVI = @MADONVI AND MAVATTU = @MAVATTU");
                                cmdGiaoDichQuayDetails.CommandType = CommandType.Text;
                                cmdGiaoDichQuayDetails.Parameters.Add("MAGDQUAYPK", SqlDbType.NVarChar, 50).Value = MaGiaoDichQuayPk;
                                cmdGiaoDichQuayDetails.Parameters.Add("MADONVI", SqlDbType.NVarChar, 50).Value = Session.Session.CurrentUnitCode;
                                cmdGiaoDichQuayDetails.Parameters.Add("MAVATTU", SqlDbType.NVarChar, 50).Value = _NVHANGGDQUAY_ASYNCCLIENT.MAVATTU;
                                SqlDataReader dataReaderGdQuayDetails = null;
                                dataReaderGdQuayDetails = cmdGiaoDichQuayDetails.ExecuteReader();
                                if (dataReaderGdQuayDetails.HasRows)
                                {
                                    while (dataReaderGdQuayDetails.Read())
                                    {
                                        decimal TYLEVATRA, TYLELAILE, TIEN_KHUYENMAI_GD, TIEN_CK_GD, SOLUONG_GD, GIABANLECOVAT_GD, GIAVON_GD = 0;
                                        decimal.TryParse(dataReaderGdQuayDetails["VATBAN"].ToString(), out TYLEVATRA);
                                        _NVHANGGDQUAY_ASYNCCLIENT.VATBAN = TYLEVATRA;
                                        decimal.TryParse(dataReaderGdQuayDetails["TYLELAILE"].ToString(), out TYLELAILE);
                                        _NVHANGGDQUAY_ASYNCCLIENT.TYLELAILE = TYLELAILE;
                                        _NVHANGGDQUAY_ASYNCCLIENT.MAVAT = dataReaderGdQuayDetails["MAVAT"].ToString().Trim();
                                        decimal.TryParse(dataReaderGdQuayDetails["TIENKHUYENMAI"].ToString(), out TIEN_KHUYENMAI_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["TIENCHIETKHAU"].ToString(), out TIEN_CK_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["SOLUONG"].ToString(), out SOLUONG_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["GIABANLECOVAT"].ToString(), out GIABANLECOVAT_GD);
                                        decimal.TryParse(dataReaderGdQuayDetails["GIAVON"].ToString(), out GIAVON_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.TTIENCOVAT = _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * GIABANLECOVAT_GD - _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_KHUYENMAI_GD / SOLUONG_GD) - _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_CK_GD / SOLUONG_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.GIABANLECOVAT = GIABANLECOVAT_GD;
                                        _NVHANGGDQUAY_ASYNCCLIENT.TIENKHUYENMAI = _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_KHUYENMAI_GD / SOLUONG_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.TIENCHIETKHAU = _NVHANGGDQUAY_ASYNCCLIENT.SOLUONG * (TIEN_CK_GD / SOLUONG_GD);
                                        _NVHANGGDQUAY_ASYNCCLIENT.GIAVON = GIAVON_GD;
                                    }
                                }
                                _NVGDQUAY_ASYNCCLIENT_DTO.LST_DETAILS.Add(_NVHANGGDQUAY_ASYNCCLIENT);
                                i++;
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
                }
            }
            return _NVGDQUAY_ASYNCCLIENT_DTO;
        }
    }
}

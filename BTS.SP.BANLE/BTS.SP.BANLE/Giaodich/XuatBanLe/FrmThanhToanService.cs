using System;
using BTS.SP.BANLE.Dto;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BTS.SP.BANLE.Common;
using Oracle.ManagedDataAccess.Client;

namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public class FrmThanhToanService
    {
        public static List<KHACHHANG_DTO> TIMKIEM_KHACHHANG_FROM_ORACLE(string P_KEYSEARCH, int P_USE_TIMKIEM_ALL, int P_DIEUKIEN_TIMKIEM, string UNITCODE)
        {
            List<KHACHHANG_DTO> _LST_KHACHHANG_DTO = new List<KHACHHANG_DTO>();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = connection;
                            cmd.InitialLONGFetchSize = 1000;
                            cmd.CommandText = "TIMKIEM_KHACHHANG";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("P_KEYSEARCH", OracleDbType.Varchar2).Value = P_KEYSEARCH;
                            cmd.Parameters.Add("P_UNITCODE", OracleDbType.Varchar2).Value = UNITCODE;
                            cmd.Parameters.Add("P_USE_TIMKIEM_ALL", OracleDbType.Int32).Value = P_USE_TIMKIEM_ALL;
                            cmd.Parameters.Add("P_DIEUKIEN_TIMKIEM", OracleDbType.Int32).Value = P_DIEUKIEN_TIMKIEM;
                            cmd.Parameters.Add("CURSOR_RESULT", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                            OracleDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    KHACHHANG_DTO _KHACHHANG_DTO = new KHACHHANG_DTO();
                                    _KHACHHANG_DTO.MAKH = dataReader["MAKH"] != null ? dataReader["MAKH"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.TENKH = dataReader["TENKH"] != null ? dataReader["TENKH"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.DIACHI = dataReader["DIACHI"] != null ? dataReader["DIACHI"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.DIENTHOAI = dataReader["DIENTHOAI"] != null ? dataReader["DIENTHOAI"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.CMTND = dataReader["CMTND"] != null ? dataReader["CMTND"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.EMAIL = dataReader["EMAIL"] != null ? dataReader["EMAIL"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.HANGKHACHHANG = dataReader["HANGKHACHHANG"] != null ? dataReader["HANGKHACHHANG"].ToString().Trim() : "";
                                    _KHACHHANG_DTO.HANGKHACHHANGCU = dataReader["HANGKHACHHANGCU"] != null ? dataReader["HANGKHACHHANGCU"].ToString().Trim() : "";
                                    decimal SODIEM = 0;
                                    if (dataReader["SODIEM"] != null)
                                    {
                                        decimal.TryParse(dataReader["SODIEM"].ToString(), out SODIEM);
                                    }
                                    decimal TONGTIEN = 0;
                                    if (dataReader["TONGTIEN"] != null)
                                    {
                                        decimal.TryParse(dataReader["TONGTIEN"].ToString(), out TONGTIEN);
                                    }
                                    _KHACHHANG_DTO.SODIEM = SODIEM;
                                    _KHACHHANG_DTO.TONGTIEN = TONGTIEN;
                                    if (dataReader["NGAYCAPTHE"] != null)
                                    {
                                        DateTime? NGAYCAPTHE = string.IsNullOrEmpty(dataReader["NGAYCAPTHE"].ToString()) ? (DateTime?)null : DateTime.Parse(dataReader["NGAYCAPTHE"].ToString());
                                        _KHACHHANG_DTO.NGAYCAPTHE = NGAYCAPTHE;
                                    }
                                    if (dataReader["NGAYHETHAN"] != null)
                                    {
                                        DateTime? NGAYHETHAN = string.IsNullOrEmpty(dataReader["NGAYHETHAN"].ToString()) ? (DateTime?)null : DateTime.Parse(dataReader["NGAYHETHAN"].ToString());
                                        _KHACHHANG_DTO.NGAYHETHAN = NGAYHETHAN;
                                    }
                                    if (dataReader["NGAYSINH"] != null)
                                    {
                                        DateTime? NGAYSINH = string.IsNullOrEmpty(dataReader["NGAYSINH"].ToString()) ? (DateTime?)null : DateTime.Parse(dataReader["NGAYSINH"].ToString());
                                        _KHACHHANG_DTO.NGAYSINH = NGAYSINH;
                                    }
                                    _KHACHHANG_DTO.UNITCODE = dataReader["UNITCODE"] != null ? dataReader["UNITCODE"].ToString().Trim() : "";
                                    _LST_KHACHHANG_DTO.Add(_KHACHHANG_DTO);
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN KHÁCH HÀNG", 1, "0x1", "0x8", "normal");
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
            return _LST_KHACHHANG_DTO;
        }

        public static string LAY_MA_TEN_KHACHHANG_FROM_ORACLE(string MAKHACHHANG)
        {
            string RESULT = "";
            try
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT MAKH || ' - ' || TENKH AS KHACHHANG FROM DM_KHACHHANG WHERE MAKH = '"+ MAKHACHHANG + "'";
                            cmd.CommandType = CommandType.Text;
                            OracleDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["KHACHHANG"] != null)
                                    {
                                        RESULT = dataReader["KHACHHANG"].ToString();
                                    }
                                    else
                                    {
                                        RESULT = "";
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN KHÁCH HÀNG", 1, "0x1", "0x8", "normal");
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
            return RESULT;
        }

        public static string LAY_MA_TEN_KHACHHANG_FROM_SQLSERVER(string MAKHACHHANG)
        {
            string RESULT = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT CONCAT (DM_KHACHHANG.MAKH,' - ',DM_KHACHHANG.TENKH) AS KHACHHANG FROM dbo.DM_KHACHHANG WHERE DM_KHACHHANG.MAKH = '" + MAKHACHHANG + "'";
                            cmd.CommandType = CommandType.Text;
                            SqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    if (dataReader["KHACHHANG"] != null)
                                    {
                                        RESULT = dataReader["KHACHHANG"].ToString();
                                    }
                                    else
                                    {
                                        RESULT = "";
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN KHÁCH HÀNG", 1, "0x1", "0x8", "normal");
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
            return RESULT;
        }

        public static HANGKHACHHANG_DTO LAY_QUYDOI_TIEN_THANH_DIEM_HANGKHACHHANG_KHOIDAU_FROM_ORACLE()
        {
            HANGKHACHHANG_DTO RESULT_QUYDOITIEN_THANH_DIEM = new HANGKHACHHANG_DTO();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT MAHANGKH,TENHANGKH,SOTIEN,TYLEGIAMGIASN,TYLEGIAMGIA,QUYDOITIEN_THANH_DIEM,QUYDOIDIEM_THANH_TIEN FROM DM_HANGKHACHHANG WHERE TRANGTHAI = 10 AND HANG_KHOIDAU = 1 AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'";
                            cmd.CommandType = CommandType.Text;
                            OracleDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    decimal SOTIEN = 0;
                                    decimal TYLEGIAMGIASN = 0;
                                    decimal TYLEGIAMGIA = 0;
                                    decimal QUYDOITIEN_THANH_DIEM = 0;
                                    decimal QUYDOIDIEM_THANH_TIEN = 0;
                                    RESULT_QUYDOITIEN_THANH_DIEM.MAHANGKH = dataReader["MAHANGKH"] != null ? dataReader["MAHANGKH"].ToString() : "";
                                    RESULT_QUYDOITIEN_THANH_DIEM.TENHANGKH = dataReader["TENHANGKH"] != null ? dataReader["TENHANGKH"].ToString() : "";
                                    decimal.TryParse(dataReader["SOTIEN"] != null ? dataReader["SOTIEN"].ToString() : "", out SOTIEN);
                                    decimal.TryParse(dataReader["TYLEGIAMGIASN"] != null ? dataReader["TYLEGIAMGIASN"].ToString() : "", out TYLEGIAMGIASN);
                                    decimal.TryParse(dataReader["TYLEGIAMGIA"] != null ? dataReader["TYLEGIAMGIA"].ToString() : "", out TYLEGIAMGIA);
                                    decimal.TryParse(dataReader["QUYDOITIEN_THANH_DIEM"] != null ? dataReader["QUYDOITIEN_THANH_DIEM"].ToString() : "", out QUYDOITIEN_THANH_DIEM);
                                    decimal.TryParse(dataReader["QUYDOIDIEM_THANH_TIEN"] != null ? dataReader["QUYDOIDIEM_THANH_TIEN"].ToString() : "", out QUYDOIDIEM_THANH_TIEN);
                                    RESULT_QUYDOITIEN_THANH_DIEM.SOTIEN = SOTIEN;
                                    RESULT_QUYDOITIEN_THANH_DIEM.TYLEGIAMGIASN = TYLEGIAMGIASN;
                                    RESULT_QUYDOITIEN_THANH_DIEM.TYLEGIAMGIA = TYLEGIAMGIA;
                                    RESULT_QUYDOITIEN_THANH_DIEM.QUYDOITIEN_THANH_DIEM = QUYDOITIEN_THANH_DIEM;
                                    RESULT_QUYDOITIEN_THANH_DIEM.QUYDOIDIEM_THANH_TIEN = QUYDOIDIEM_THANH_TIEN;
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN KHỞI ĐẦU HẠNG", 1, "0x1", "0x8", "normal");
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
            return RESULT_QUYDOITIEN_THANH_DIEM;
        }


        public static HANGKHACHHANG_DTO LAY_QUYDOI_TIEN_THANH_DIEM_HANGKHACHHANG_KHOIDAU_FROM_SQLSERVER()
        {
            HANGKHACHHANG_DTO RESULT_QUYDOITIEN_THANH_DIEM = new HANGKHACHHANG_DTO();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT MAHANGKH,TENHANGKH,SOTIEN,TYLEGIAMGIASN,TYLEGIAMGIA,QUYDOITIEN_THANH_DIEM,QUYDOIDIEM_THANH_TIEN FROM dbo.DM_HANGKHACHHANG WHERE TRANGTHAI = 10 AND HANG_KHOIDAU = 1 AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'";
                            cmd.CommandType = CommandType.Text;
                            SqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    decimal SOTIEN = 0;
                                    decimal TYLEGIAMGIASN = 0;
                                    decimal TYLEGIAMGIA = 0;
                                    decimal QUYDOITIEN_THANH_DIEM = 0;
                                    decimal QUYDOIDIEM_THANH_TIEN = 0;
                                    RESULT_QUYDOITIEN_THANH_DIEM.MAHANGKH = dataReader["MAHANGKH"] != null ? dataReader["MAHANGKH"].ToString() : "";
                                    RESULT_QUYDOITIEN_THANH_DIEM.TENHANGKH = dataReader["TENHANGKH"] != null ? dataReader["TENHANGKH"].ToString() : "";
                                    decimal.TryParse(dataReader["SOTIEN"] != null ? dataReader["SOTIEN"].ToString() : "", out SOTIEN);
                                    decimal.TryParse(dataReader["TYLEGIAMGIASN"] != null ? dataReader["TYLEGIAMGIASN"].ToString() : "", out TYLEGIAMGIASN);
                                    decimal.TryParse(dataReader["TYLEGIAMGIA"] != null ? dataReader["TYLEGIAMGIA"].ToString() : "", out TYLEGIAMGIA);
                                    decimal.TryParse(dataReader["QUYDOITIEN_THANH_DIEM"] != null ? dataReader["QUYDOITIEN_THANH_DIEM"].ToString() : "", out QUYDOITIEN_THANH_DIEM);
                                    decimal.TryParse(dataReader["QUYDOIDIEM_THANH_TIEN"] != null ? dataReader["QUYDOIDIEM_THANH_TIEN"].ToString() : "", out QUYDOIDIEM_THANH_TIEN);
                                    RESULT_QUYDOITIEN_THANH_DIEM.SOTIEN = SOTIEN;
                                    RESULT_QUYDOITIEN_THANH_DIEM.TYLEGIAMGIASN = TYLEGIAMGIASN;
                                    RESULT_QUYDOITIEN_THANH_DIEM.TYLEGIAMGIA = TYLEGIAMGIA;
                                    RESULT_QUYDOITIEN_THANH_DIEM.QUYDOITIEN_THANH_DIEM = QUYDOITIEN_THANH_DIEM;
                                    RESULT_QUYDOITIEN_THANH_DIEM.QUYDOIDIEM_THANH_TIEN = QUYDOIDIEM_THANH_TIEN;
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN KHỞI ĐẦU HẠNG", 1, "0x1", "0x8", "normal");
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
            return RESULT_QUYDOITIEN_THANH_DIEM;
        }

        public static HANGKHACHHANG_DTO LAY_QUYDOI_THEOHANGKH_FROM_ORACLE(string MAHANGKHACHHANG)
        {
            HANGKHACHHANG_DTO RESULT = new HANGKHACHHANG_DTO();
            try
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand cmd = new OracleCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT MAHANGKH,TENHANGKH,SOTIEN,TYLEGIAMGIASN,TYLEGIAMGIA,QUYDOITIEN_THANH_DIEM,QUYDOIDIEM_THANH_TIEN FROM DM_HANGKHACHHANG WHERE TRANGTHAI = 10 AND MAHANGKH = '" + MAHANGKHACHHANG + "' AND UNITCODE = '" +Session.Session.CurrentUnitCode+ "'";
                            cmd.CommandType = CommandType.Text;
                            OracleDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    decimal SOTIEN = 0;
                                    decimal TYLEGIAMGIASN = 0;
                                    decimal TYLEGIAMGIA = 0;
                                    decimal QUYDOITIEN_THANH_DIEM = 0;
                                    decimal QUYDOIDIEM_THANH_TIEN = 0;
                                    RESULT.MAHANGKH = dataReader["MAHANGKH"] != null ? dataReader["MAHANGKH"].ToString() : "";
                                    RESULT.TENHANGKH = dataReader["TENHANGKH"] != null ? dataReader["TENHANGKH"].ToString() : "";
                                    decimal.TryParse(dataReader["SOTIEN"] != null ? dataReader["SOTIEN"].ToString() : "",  out SOTIEN);
                                    decimal.TryParse(dataReader["TYLEGIAMGIASN"] != null ? dataReader["TYLEGIAMGIASN"].ToString() : "", out TYLEGIAMGIASN);
                                    decimal.TryParse(dataReader["TYLEGIAMGIA"] != null ? dataReader["TYLEGIAMGIA"].ToString() : "", out TYLEGIAMGIA);
                                    decimal.TryParse(dataReader["QUYDOITIEN_THANH_DIEM"] != null ? dataReader["QUYDOITIEN_THANH_DIEM"].ToString() : "", out QUYDOITIEN_THANH_DIEM);
                                    decimal.TryParse(dataReader["QUYDOIDIEM_THANH_TIEN"] != null ? dataReader["QUYDOIDIEM_THANH_TIEN"].ToString() : "", out QUYDOIDIEM_THANH_TIEN);
                                    RESULT.SOTIEN = SOTIEN;
                                    RESULT.TYLEGIAMGIASN = TYLEGIAMGIASN;
                                    RESULT.TYLEGIAMGIA = TYLEGIAMGIA;
                                    RESULT.QUYDOITIEN_THANH_DIEM = QUYDOITIEN_THANH_DIEM;
                                    RESULT.QUYDOIDIEM_THANH_TIEN = QUYDOIDIEM_THANH_TIEN;
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN HẠNG KHÁCH HÀNG", 1, "0x1", "0x8", "normal");
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
            return RESULT;
        }


        public static HANGKHACHHANG_DTO LAY_QUYDOI_THEOHANGKH_FROM_SQLSERVER(string MAHANGKHACHHANG)
        {
            HANGKHACHHANG_DTO RESULT = new HANGKHACHHANG_DTO();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = connection;
                            cmd.CommandText = "SELECT MAHANGKH,TENHANGKH,SOTIEN,TYLEGIAMGIASN,TYLEGIAMGIA,QUYDOITIEN_THANH_DIEM,QUYDOIDIEM_THANH_TIEN FROM dbo.DM_HANGKHACHHANG WHERE TRANGTHAI = 10 AND MAHANGKH = '" + MAHANGKHACHHANG + "' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'";
                            cmd.CommandType = CommandType.Text;
                            SqlDataReader dataReader = cmd.ExecuteReader();
                            if (dataReader.HasRows)
                            {
                                while (dataReader.Read())
                                {
                                    decimal SOTIEN = 0;
                                    decimal TYLEGIAMGIASN = 0;
                                    decimal TYLEGIAMGIA = 0;
                                    decimal QUYDOITIEN_THANH_DIEM = 0;
                                    decimal QUYDOIDIEM_THANH_TIEN = 0;
                                    RESULT.MAHANGKH = dataReader["MAHANGKH"] != null ? dataReader["MAHANGKH"].ToString() : "";
                                    RESULT.TENHANGKH = dataReader["TENHANGKH"] != null ? dataReader["TENHANGKH"].ToString() : "";
                                    decimal.TryParse(dataReader["SOTIEN"] != null ? dataReader["SOTIEN"].ToString() : "", out SOTIEN);
                                    decimal.TryParse(dataReader["TYLEGIAMGIASN"] != null ? dataReader["TYLEGIAMGIASN"].ToString() : "", out TYLEGIAMGIASN);
                                    decimal.TryParse(dataReader["TYLEGIAMGIA"] != null ? dataReader["TYLEGIAMGIA"].ToString() : "", out TYLEGIAMGIA);
                                    decimal.TryParse(dataReader["QUYDOITIEN_THANH_DIEM"] != null ? dataReader["QUYDOITIEN_THANH_DIEM"].ToString() : "", out QUYDOITIEN_THANH_DIEM);
                                    decimal.TryParse(dataReader["QUYDOIDIEM_THANH_TIEN"] != null ? dataReader["QUYDOIDIEM_THANH_TIEN"].ToString() : "", out QUYDOIDIEM_THANH_TIEN);
                                    RESULT.SOTIEN = SOTIEN;
                                    RESULT.TYLEGIAMGIASN = TYLEGIAMGIASN;
                                    RESULT.TYLEGIAMGIA = TYLEGIAMGIA;
                                    RESULT.QUYDOITIEN_THANH_DIEM = QUYDOITIEN_THANH_DIEM;
                                    RESULT.QUYDOIDIEM_THANH_TIEN = QUYDOIDIEM_THANH_TIEN;
                                }
                            }
                        }
                    }
                    catch
                    {
                        NotificationLauncher.ShowNotificationWarning("THÔNG BÁO", "KHÔNG TÌM THẤY THÔNG TIN HẠNG KHÁCH HÀNG", 1, "0x1", "0x8", "normal");
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
            return RESULT;
        }
    }
}

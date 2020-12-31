using BTS.SP.BANLE.Common;
using BTS.SP.BANLE.Dto;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using EnumCommon = BTS.SP.BANLE.Common.EnumCommon;

namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public class FrmXuatBanLeService
    {
        static string KeyMaCan = ConfigurationSettings.AppSettings["KEYMACAN"];
        public static List<VATTU_DTO> GET_DATA_VATTU_FROM_CSDL_ORACLE(string MaVatTu, EnumCommon.MethodGetPrice PhuongThucTinhGia)
        {
            string TABLE_NAME = GET_TABLE_NAME_NGAYHACHTOAN_CSDL_ORACLE();
            List<VATTU_DTO> listDataDto = new List<VATTU_DTO>();
            string beginCharacter = string.Empty;
            if (MaVatTu.Length >= 4)
            {
                if (!string.IsNullOrEmpty(MaVatTu)) beginCharacter = MaVatTu.Substring(0, 2);
                //TRƯỜNG HỢP BÁN MÃ CÂN ĐIỆN TỬ
                if (beginCharacter.Equals(KeyMaCan) && MaVatTu.Length > 9)
                {
                    try
                    {
                        string itemCode = string.Empty; if (!string.IsNullOrEmpty(MaVatTu)) itemCode = MaVatTu.Substring(2, 5);
                        string soLuongItemCode = ""; if (!string.IsNullOrEmpty(MaVatTu)) soLuongItemCode = MaVatTu.Substring(7, 5);
                        using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                        {
                            connection.Open();
                            if (connection.State == ConnectionState.Open)
                            {
                                try
                                {
                                    OracleCommand cmd = new OracleCommand();
                                    cmd.Connection = connection;
                                    cmd.CommandText = string.Format(@"SELECT a.MAVATTU,a.TENVATTU,a.DONVITINH,a.MAKHACHHANG,a.GIABANBUONVAT,a.GIABANLEVAT,a.ITEMCODE,a.TYLELAILE,b.GIAVON,a.MAVATRA,NVL(a.TYLEVATRA,0) AS TYLEVATRA,b.TONCUOIKYSL FROM V_VATTU_GIABAN a LEFT JOIN " + TABLE_NAME + " b ON a.MAVATTU = b.MAVATTU AND b.MAKHO = '" + Session.Session.CurrentWareHouse + "' WHERE a.ITEMCODE = '" + itemCode + "' AND a.UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                                    OracleDataReader dataReader = cmd.ExecuteReader();
                                    if (dataReader.HasRows)
                                    {
                                        while (dataReader.Read())
                                        {
                                            decimal GIABANBUONVAT, GIABANLEVAT, GIAVON, SOLUONG, TYLEVATRA, TYLELAILE, TONCUOIKYSL = 0;
                                            VATTU_DTO dataDto = new VATTU_DTO();
                                            dataDto.MAVATTU = dataReader["MAVATTU"].ToString();
                                            dataDto.TENVATTU = dataReader["TENVATTU"].ToString();
                                            dataDto.DONVITINH = dataReader["DONVITINH"].ToString();
                                            dataDto.MANHACUNGCAP = dataReader["MAKHACHHANG"].ToString();
                                            dataDto.MAVATRA = dataReader["MAVATRA"].ToString();
                                            decimal.TryParse(dataReader["GIABANBUONVAT"].ToString(), out GIABANBUONVAT);
                                            dataDto.GIABANBUONVAT = GIABANBUONVAT;
                                            decimal.TryParse(dataReader["GIABANLEVAT"].ToString(), out GIABANLEVAT);
                                            decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                            decimal.TryParse(dataReader["TYLEVATRA"].ToString(), out TYLEVATRA);
                                            decimal.TryParse(dataReader["TYLELAILE"].ToString(), out TYLELAILE);
                                            dataDto.LAMACAN = true;
                                            if (PhuongThucTinhGia == EnumCommon.MethodGetPrice.GIABANLECOVAT)
                                            {
                                                dataDto.GIABANLEVAT = GIABANLEVAT;
                                            }
                                            else
                                            {
                                                if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                                {
                                                    dataDto.GIABANLEVAT = GIABANBUONVAT;
                                                }
                                                else
                                                {
                                                    dataDto.GIABANLEVAT = GIAVON * (1 + TYLEVATRA / 100);
                                                }
                                            }
                                            dataDto.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                            dataDto.GIAVON = GIAVON * (1 + TYLEVATRA / 100);
                                            dataDto.TYLEVATRA = TYLEVATRA;
                                            dataDto.TYLELAILE = TYLELAILE;
                                            decimal.TryParse(soLuongItemCode, out SOLUONG);
                                            dataDto.SOLUONG = SOLUONG;
                                            decimal.TryParse(dataReader["TONCUOIKYSL"].ToString(), out TONCUOIKYSL);
                                            dataDto.TONCUOIKYSL = TONCUOIKYSL;
                                            listDataDto.Add(dataDto);
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
                    catch
                    {
                        string NOTIFICATION_WARNING = string.Format(@"MÃ CÂN '{0}' KHÔNG HỢP LỆ", MaVatTu);
                        MessageBox.Show(NOTIFICATION_WARNING);
                    }
                }
                //BÁN MÃ BÓ HÀNG
                if (beginCharacter.Equals("BH"))
                {
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
                                        @"SELECT a.MABOHANG AS MAVATTU,a.TENBOHANG AS TENVATTU,'BÓ' AS DONVITINH,'' AS MAKHACHHANG,SUM(b.TONGBUON) AS GIABANBUONVAT,SUM(b.TONGLE) AS GIABANLEVAT,'' AS ITEMCODE FROM DM_BOHANG a,DM_BOHANGCHITIET b WHERE a.MABOHANG = b.MABOHANG AND a.MABOHANG = '" +
                                        MaVatTu + "' AND a.UNITCODE = '" + Session.Session.CurrentUnitCode +
                                        "' GROUP BY a.MABOHANG,a.TENBOHANG");
                                OracleDataReader dataReader = cmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        decimal GIABANBUONVAT, GIABANLEVAT, SOLUONG = 0;
                                        VATTU_DTO dataDto = new VATTU_DTO();
                                        dataDto.MAVATTU = dataReader["MAVATTU"].ToString();
                                        dataDto.MABO = dataDto.MAVATTU;
                                        dataDto.TENVATTU = dataReader["TENVATTU"].ToString();
                                        dataDto.DONVITINH = dataReader["DONVITINH"].ToString();
                                        dataDto.MANHACUNGCAP = dataReader["MAKHACHHANG"].ToString();
                                        decimal.TryParse(dataReader["GIABANBUONVAT"].ToString(), out GIABANBUONVAT);
                                        dataDto.GIABANBUONVAT = GIABANBUONVAT;
                                        decimal.TryParse(dataReader["GIABANLEVAT"].ToString(), out GIABANLEVAT);
                                        dataDto.GIABANLEVAT = GIABANLEVAT;
                                        dataDto.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                        dataDto.SOLUONG = SOLUONG;
                                        dataDto.LAMACAN = false;
                                        OracleCommand cmdBoHangChiTiet = new OracleCommand();
                                        cmdBoHangChiTiet.Connection = connection;
                                        cmdBoHangChiTiet.CommandText =
                                            "SELECT MAHANG , SOLUONG FROM DM_BOHANGCHITIET WHERE MABOHANG = '" + dataDto.MAVATTU +
                                            "' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'";
                                        OracleDataReader dataReaderBoHangChiTiet = cmdBoHangChiTiet.ExecuteReader();
                                        EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();

                                        string msg = Config.CheckConnectToServer(out bool result);
                                        if (msg.Length > 0) { MessageBox.Show(msg); return listDataDto; }

                                        if (result)
                                        {
                                            _EXTEND_VAT_BOHANG = LAYDULIEU_VAT_BOHANG_FROM_DATABASE_ORACLE(dataDto.MAVATTU, Session.Session.CurrentUnitCode);
                                            dataDto.MAVATRA = _EXTEND_VAT_BOHANG.MAVATRA;
                                            dataDto.TYLEVATRA = _EXTEND_VAT_BOHANG.TYLEVATRA;
                                        }
                                        else
                                        {
                                            _EXTEND_VAT_BOHANG = LAYDULIEU_VAT_BOHANG_FROM_DATABASE_SQLSERVER(dataDto.MAVATTU, Session.Session.CurrentUnitCode);
                                            dataDto.MAVATRA = _EXTEND_VAT_BOHANG.MAVATRA;
                                            dataDto.TYLEVATRA = _EXTEND_VAT_BOHANG.TYLEVATRA;
                                        }
                                        if (dataReaderBoHangChiTiet.HasRows)
                                        {
                                            while (dataReaderBoHangChiTiet.Read())
                                            {
                                                decimal GiaVon = 0;
                                                decimal TONCUOIKYSL = 0;
                                                string maVatTuBoHang = dataReaderBoHangChiTiet["MAHANG"] != null ? dataReaderBoHangChiTiet["MAHANG"].ToString().ToUpper().Trim() : "";
                                                decimal.TryParse(dataReaderBoHangChiTiet["SOLUONG"].ToString(), out SOLUONG);
                                                //dataDto.SOLUONG = SOLUONG;
                                                OracleCommand cmdVatTu = new OracleCommand();
                                                cmdVatTu.Connection = connection;
                                                cmdVatTu.CommandText = "SELECT b.GIAVON,b.TONCUOIKYSL FROM V_VATTU_GIABAN a JOIN " + TABLE_NAME + " b ON a.MAVATTU = b.MAVATTU AND b.MAKHO = '" + Session.Session.CurrentWareHouse + "' WHERE a.MAVATTU = '" + maVatTuBoHang + "' AND a.MADONVI = '" + Session.Session.CurrentUnitCode + "'";
                                                OracleDataReader dataReaderVatTu = cmdVatTu.ExecuteReader();
                                                if (dataReaderVatTu.HasRows)
                                                {
                                                    while (dataReaderVatTu.Read())
                                                    {
                                                        decimal.TryParse(dataReaderVatTu["GIAVON"].ToString(), out GiaVon);
                                                        decimal.TryParse(dataReaderVatTu["TONCUOIKYSL"].ToString(), out TONCUOIKYSL);
                                                    }
                                                    dataDto.GIAVON += GiaVon * SOLUONG;
                                                    dataDto.TONCUOIKYSL += TONCUOIKYSL;
                                                }
                                            }
                                        }

                                        if (PhuongThucTinhGia == EnumCommon.MethodGetPrice.GIABANLECOVAT)
                                        {
                                            dataDto.GIABANLEVAT = GIABANLEVAT;
                                        }
                                        else
                                        {
                                            if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                            {
                                                dataDto.GIABANLEVAT = GIABANBUONVAT;
                                            }
                                            else
                                            {
                                                dataDto.GIABANLEVAT = dataDto.GIAVON * (1 + dataDto.TYLEVATRA / 100);

                                            }
                                        }
                                        dataDto.GIAVON = dataDto.GIAVON * (1 + dataDto.TYLEVATRA / 100);
                                        listDataDto.Add(dataDto);
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
                else if (MaVatTu.Length == 7)
                {
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
                                        @"SELECT a.MAVATTU,a.TENVATTU,a.DONVITINH,a.BARCODE,a.MAKHACHHANG,a.GIABANBUONVAT,a.GIABANLEVAT,a.ITEMCODE,b.GIAVON,a.MAVATRA,NVL(a.TYLEVATRA,0) AS TYLEVATRA,b.TONCUOIKYSL FROM V_VATTU_GIABAN a LEFT JOIN " +
                                        TABLE_NAME + " b ON a.MAVATTU = b.MAVATTU AND b.MAKHO = '" +
                                        Session.Session.CurrentWareHouse + "' WHERE a.MAVATTU = '" + MaVatTu +
                                        "' AND a.UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                                OracleDataReader dataReader = cmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        decimal GIABANBUONVAT, GIABANLEVAT, GIAVON, TYLEVATRA, TONCUOIKYSL, SOLUONG = 0;
                                        VATTU_DTO dataDto = new VATTU_DTO();
                                        dataDto.MAVATTU = dataReader["MAVATTU"].ToString();
                                        dataDto.TENVATTU = dataReader["TENVATTU"].ToString();
                                        dataDto.DONVITINH = dataReader["DONVITINH"].ToString();
                                        dataDto.BARCODE = dataReader["BARCODE"].ToString();
                                        if (!string.IsNullOrEmpty(dataDto.BARCODE))
                                        {
                                            if (dataDto.BARCODE.Length <= 2)
                                            {
                                                dataDto.BARCODE = ";";
                                            }
                                        }
                                        else
                                        {
                                            dataDto.BARCODE = ";";
                                        }
                                        dataDto.MANHACUNGCAP = dataReader["MAKHACHHANG"].ToString();
                                        dataDto.MAVATRA = dataReader["MAVATRA"].ToString();
                                        decimal.TryParse(dataReader["TYLEVATRA"].ToString(), out TYLEVATRA);
                                        decimal.TryParse(dataReader["GIABANBUONVAT"].ToString(), out GIABANBUONVAT);
                                        dataDto.GIABANBUONVAT = GIABANBUONVAT;
                                        decimal.TryParse(dataReader["GIABANLEVAT"].ToString(), out GIABANLEVAT);
                                        decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                        dataDto.LAMACAN = false;
                                        if (PhuongThucTinhGia == EnumCommon.MethodGetPrice.GIABANLECOVAT)
                                        {
                                            dataDto.GIABANLEVAT = GIABANLEVAT;
                                        }
                                        else
                                        {
                                            if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                            {
                                                dataDto.GIABANLEVAT = GIABANBUONVAT;
                                            }
                                            else
                                            {
                                                dataDto.GIABANLEVAT = GIAVON * (1 + TYLEVATRA / 100);
                                            }
                                        }
                                        dataDto.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                        dataDto.GIAVON = GIAVON * (1 + TYLEVATRA / 100);
                                        dataDto.TYLEVATRA = TYLEVATRA;
                                        dataDto.SOLUONG = SOLUONG;
                                        decimal.TryParse(dataReader["TONCUOIKYSL"].ToString(), out TONCUOIKYSL);
                                        dataDto.TONCUOIKYSL = TONCUOIKYSL;
                                        listDataDto.Add(dataDto);
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
                else
                {
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
                                        @"SELECT a.MAVATTU,a.TENVATTU,a.DONVITINH,a.BARCODE,a.MAKHACHHANG,a.GIABANBUONVAT,a.GIABANLEVAT,a.ITEMCODE,b.GIAVON,a.MAVATRA,NVL(a.TYLEVATRA,0) AS TYLEVATRA,b.TONCUOIKYSL FROM V_VATTU_GIABAN a LEFT JOIN " +
                                        TABLE_NAME + " b ON a.MAVATTU = b.MAVATTU AND b.MAKHO = '" +
                                        Session.Session.CurrentWareHouse + "' WHERE a.BARCODE LIKE '%" + MaVatTu +
                                        "%' AND a.UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                                OracleDataReader dataReader = cmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        decimal GIABANBUONVAT, GIABANLEVAT, GIAVON, TYLEVATRA, TONCUOIKYSL, SOLUONG = 0;
                                        VATTU_DTO dataDto = new VATTU_DTO();
                                        dataDto.MAVATTU = dataReader["MAVATTU"].ToString();
                                        dataDto.TENVATTU = dataReader["TENVATTU"].ToString();
                                        dataDto.DONVITINH = dataReader["DONVITINH"].ToString();
                                        dataDto.BARCODE = dataReader["BARCODE"].ToString();
                                        dataDto.LAMACAN = false;
                                        if (!string.IsNullOrEmpty(dataDto.BARCODE))
                                        {
                                            if (dataDto.BARCODE.Length <= 2)
                                            {
                                                dataDto.BARCODE = ";";
                                            }
                                        }
                                        else
                                        {
                                            dataDto.BARCODE = ";";
                                        }
                                        dataDto.MANHACUNGCAP = dataReader["MAKHACHHANG"].ToString();
                                        dataDto.MAVATRA = dataReader["MAVATRA"].ToString();
                                        decimal.TryParse(dataReader["TYLEVATRA"].ToString(), out TYLEVATRA);
                                        decimal.TryParse(dataReader["GIABANBUONVAT"].ToString(), out GIABANBUONVAT);
                                        dataDto.GIABANBUONVAT = GIABANBUONVAT;
                                        decimal.TryParse(dataReader["GIABANLEVAT"].ToString(), out GIABANLEVAT);
                                        decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                        if (PhuongThucTinhGia == EnumCommon.MethodGetPrice.GIABANLECOVAT)
                                        {
                                            dataDto.GIABANLEVAT = GIABANLEVAT;
                                        }
                                        else
                                        {
                                            if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                            {
                                                dataDto.GIABANLEVAT = GIABANBUONVAT;
                                            }
                                            else
                                            {
                                                dataDto.GIABANLEVAT = GIAVON * (1 + TYLEVATRA / 100);
                                            }
                                        }
                                        dataDto.GIAVON = GIAVON * (1 + TYLEVATRA / 100);
                                        dataDto.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                        dataDto.TYLEVATRA = TYLEVATRA;
                                        dataDto.SOLUONG = SOLUONG;
                                        decimal.TryParse(dataReader["TONCUOIKYSL"].ToString(), out TONCUOIKYSL);
                                        dataDto.TONCUOIKYSL = TONCUOIKYSL;
                                        listDataDto.Add(dataDto);
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
            return listDataDto;
        }
        public static decimal TINHTOAN_KHUYENMAI(string maVatTu, EnumCommon.MethodGetPrice method)
        {
            decimal TEMP_TIENKHUYENMAI = 0;
            List<KHUYENMAI_DTO> dataKhuyenMai = new List<KHUYENMAI_DTO>();

            //GET CHƯƠNG TRÌNH KHUYẾN MÃI
            string msg = Config.CheckConnectToServer(out bool result);
            if (msg.Length > 0) { MessageBox.Show(msg); return TEMP_TIENKHUYENMAI; }

            if (result) // nếu có mạng lan
            {
                dataKhuyenMai = CACULATION_KHUYENMAI_CHIETKHAU_GIAMGIA_CSDL_ORACLE(maVatTu);
            }
            else //Không có mạng 
            {
                dataKhuyenMai = CACULATION_KHUYENMAI_CHIETKHAU_GIAMGIA_CSDL_SQL(maVatTu);
            }
            if (dataKhuyenMai.Count > 1)
            {
                //trùng chương trình khuyến mại
            }
            else if (dataKhuyenMai.Count == 1)
            {
                if (string.IsNullOrEmpty(dataKhuyenMai[0].TUGIO) || string.IsNullOrEmpty(dataKhuyenMai[0].DENGIO))
                {
                    //hiện tại chỉ tính theo tiền -- theo cột TYLEKHUYENMAICHILDREN trong VIEW_KHUYENMAI
                    decimal.TryParse(dataKhuyenMai[0].TYLE.ToString(), out TEMP_TIENKHUYENMAI);
                }
                else
                {
                    int getHour = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                    if (dataKhuyenMai[0].TUGIO != "0" && dataKhuyenMai[0].DENGIO != "0")
                    {
                        string[] tugio = dataKhuyenMai[0].TUGIO.Split(':');
                        string[] dengio = dataKhuyenMai[0].DENGIO.Split(':');
                        int minuteTuGio = Int32.Parse(tugio[0]) * 60 + Int32.Parse(tugio[1]);
                        int minuteDenGio = Int32.Parse(dengio[0]) * 60 + Int32.Parse(dengio[1]);
                        if (minuteTuGio <= getHour && getHour <= minuteDenGio)
                        {
                            //hiện tại chỉ tính theo tiền -- theo cột TYLEKHUYENMAICHILDREN trong VIEW_KHUYENMAI
                            decimal.TryParse(dataKhuyenMai[0].TYLE.ToString(), out TEMP_TIENKHUYENMAI);
                        }
                    }
                    else
                    {
                        decimal.TryParse(dataKhuyenMai[0].TYLE.ToString(), out TEMP_TIENKHUYENMAI);
                    }
                }
            }
            return TEMP_TIENKHUYENMAI;
        }
        //Lấy ngày hạch toán , Số giao dịch
        public static DateTime GET_NGAYHACHTOAN_CSDL_ORACLE()
        {
            DateTime result = DateTime.Now;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT KY,NGAYHACHTOAN FROM (SELECT MAX(KY) AS KY,TUNGAY AS NGAYHACHTOAN FROM DM_KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10 AND NAM = (SELECT MAX(NAM) FROM DM_KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "') GROUP BY KY,TUNGAY ORDER BY KY DESC) WHERE ROWNUM = 1");
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result = (DateTime)dataReader["NGAYHACHTOAN"];
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
            return result;
        }
        //Lấy table name Ngày hạch toán
        public static string GET_TABLE_NAME_NGAYHACHTOAN_CSDL_ORACLE()
        {
            string result = string.Empty;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {

                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT KY,NAM FROM (SELECT MAX(KY) AS KY,NAM FROM DM_KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10 AND NAM = (SELECT MAX(NAM) FROM DM_KYKETOAN WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "') GROUP BY KY,NAM ORDER BY KY DESC) WHERE ROWNUM = 1");
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            string KY = string.Empty;
                            string NAM = string.Empty;
                            while (dataReader.Read())
                            {
                                KY = dataReader["KY"].ToString();
                                Session.Session.CurrentPeriod = KY;
                                NAM = dataReader["NAM"].ToString();
                                Session.Session.CurrentYear = NAM;
                            }
                            result = ("XNT_" + NAM + "_KY_" + KY).ToUpper().Trim();
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
            return result;
        }
        // TÍNH KHUYẾN MẠI FROM ORACLE
        public static List<KHUYENMAI_DTO> CACULATION_KHUYENMAI_CHIETKHAU_GIAMGIA_CSDL_ORACLE(string MaVatTu)
        {
            List<KHUYENMAI_DTO> result = new List<KHUYENMAI_DTO>();
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT MACHUONGTRINH,TUNGAY,DENNGAY,TUGIO, DENGIO,NOIDUNG,MAVATTU, SOLUONG_KHUYENMAI AS SOLUONG, TYLEKHUYENMAICHILDREN AS TYLE,GIATRIKHUYENMAICHILDREN AS GIATRI FROM V_CHUONGTRINH_KHUYENMAI WHERE TO_DATE('" + DateTime.Now.ToString("dd-MM-yyyy") + "', 'DD/MM/YY') BETWEEN TO_DATE(TUNGAY, 'DD/MM/YY') AND TO_DATE(DENNGAY, 'DD/MM/YY') AND TRANGTHAI = 10 AND UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND MAVATTU = '" + MaVatTu + "' ");
                        OracleDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                decimal SOLUONG, TYLE, GIATRI = 0;
                                KHUYENMAI_DTO dto = new KHUYENMAI_DTO();
                                dto.MACHUONGTRINH = dataReader["MACHUONGTRINH"].ToString();
                                dto.TUNGAY = dataReader["TUNGAY"].ToString();
                                dto.DENNGAY = dataReader["DENNGAY"].ToString();
                                dto.TUGIO = dataReader["TUGIO"].ToString();
                                dto.DENGIO = dataReader["DENGIO"].ToString();
                                dto.NOIDUNG = dataReader["NOIDUNG"].ToString();
                                dto.MAVATTU = dataReader["MAVATTU"].ToString();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                decimal.TryParse(dataReader["TYLE"].ToString(), out TYLE);
                                decimal.TryParse(dataReader["GIATRI"].ToString(), out GIATRI);
                                dto.SOLUONG = SOLUONG;
                                dto.TYLE = TYLE;
                                dto.GIATRI = GIATRI;
                                result.Add(dto);
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
                if (result.Count != 0 && result.Count > 1)
                {
                    MessageBox.Show("Trùng lặp chương trình khuyến mãi mã hàng này !");
                }
                else
                {
                    return result;
                }
            }
            return result;
        }

        // TÍNH KHUYẾN MẠI FROM SQL
        public static List<KHUYENMAI_DTO> CACULATION_KHUYENMAI_CHIETKHAU_GIAMGIA_CSDL_SQL(string MaVatTu)
        {
            List<KHUYENMAI_DTO> result = new List<KHUYENMAI_DTO>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = string.Format(@"SELECT [ID],[MACHUONGTRINH],[TUNGAY],[DENNGAY],[TUGIO],[DENGIO],[MAVATTU],[SOLUONG],[TYLE],[GIATRI] FROM [dbo].[KHUYENMAI] WHERE TUNGAY <= '" + DateTime.Now.ToString("yyyy-MM-dd") + "'  AND DENNGAY >= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND MAVATTU = '" + MaVatTu + "' ");
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                decimal SOLUONG, TYLE, GIATRI = 0;
                                KHUYENMAI_DTO dto = new KHUYENMAI_DTO();
                                dto.MACHUONGTRINH = dataReader["MACHUONGTRINH"].ToString();
                                dto.TUNGAY = dataReader["TUNGAY"].ToString();
                                dto.DENNGAY = dataReader["DENNGAY"].ToString();
                                dto.TUGIO = dataReader["TUGIO"].ToString();
                                dto.DENGIO = dataReader["DENGIO"].ToString();
                                dto.NOIDUNG = "";
                                dto.MAVATTU = dataReader["MAVATTU"].ToString();
                                decimal.TryParse(dataReader["SOLUONG"].ToString(), out SOLUONG);
                                decimal.TryParse(dataReader["TYLE"].ToString(), out TYLE);
                                decimal.TryParse(dataReader["GIATRI"].ToString(), out GIATRI);
                                dto.SOLUONG = SOLUONG;
                                dto.TYLE = TYLE;
                                dto.GIATRI = GIATRI;
                                result.Add(dto);
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
                if (result.Count != 0 && result.Count > 1)
                {
                    MessageBox.Show("Trùng lặp chương trình khuyến mãi mã hàng này !");
                }
                else
                {
                    return result;
                }
            }
            return result;
        }
        //Lấy ngày hạch toán , Số giao dịch
        public static string INIT_CODE_TRADE()
        {
            string result = string.Empty;
            string machineName = Environment.MachineName;
            string time = DateTime.Now.ToString("yyMMddHHmmss");
            result = machineName + "-" + time;
            return result;
        }

        public static List<VATTU_DTO> GET_DATA_VATTU_FROM_CSDL_SQL(string MaVatTu, EnumCommon.MethodGetPrice PhuongThucTinhGia)
        {
            List<VATTU_DTO> listDataDto = new List<VATTU_DTO>();
            string beginCharacter = string.Empty;
            if (MaVatTu.Length >= 4)
            {
                if (!string.IsNullOrEmpty(MaVatTu)) beginCharacter = MaVatTu.Substring(0, 2);
                //TRƯỜNG HỢP BÁN MÃ CÂN ĐIỆN TỬ
                if (beginCharacter.Equals(KeyMaCan) && MaVatTu.Length > 9)
                {
                    string itemCode = string.Empty; if (!string.IsNullOrEmpty(MaVatTu)) itemCode = MaVatTu.Substring(2, 5);
                    string soLuongItemCode = ""; if (!string.IsNullOrEmpty(MaVatTu)) soLuongItemCode = MaVatTu.Substring(7, 5);
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            try
                            {
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = connection;
                                cmd.CommandText = string.Format(@"SELECT [ID],[MAVATTU],[TENVATTU],[MANHACUNGCAP],[DONVITINH],[BARCODE],[GIABANBUONVAT],[GIABANLEVAT],[GIAVON],[TONCUOIKYSL],[TYLEVATRA],[TYLELAILE],[ITEMCODE],[MAVATRA],[UNITCODE]  FROM [dbo].[DM_VATTU] WHERE ITEMCODE = '" + itemCode + "' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                                SqlDataReader dataReader = cmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        decimal GIABANBUONVAT, GIABANLEVAT, GIAVON, SOLUONG, TYLEVATRA, TYLELAILE = 0;
                                        VATTU_DTO dataDto = new VATTU_DTO();
                                        dataDto.MAVATTU = dataReader["MAVATTU"].ToString();
                                        dataDto.TENVATTU = dataReader["TENVATTU"].ToString();
                                        dataDto.DONVITINH = dataReader["DONVITINH"].ToString();
                                        dataDto.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                        dataDto.MAVATRA = dataReader["MAVATRA"].ToString();
                                        decimal.TryParse(dataReader["GIABANBUONVAT"].ToString(), out GIABANBUONVAT);
                                        dataDto.GIABANBUONVAT = GIABANBUONVAT;
                                        decimal.TryParse(dataReader["GIABANLEVAT"].ToString(), out GIABANLEVAT);
                                        decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                        decimal.TryParse(dataReader["TYLEVATRA"].ToString(), out TYLEVATRA);
                                        decimal.TryParse(dataReader["TYLELAILE"].ToString(), out TYLELAILE);
                                        if (PhuongThucTinhGia == EnumCommon.MethodGetPrice.GIABANLECOVAT)
                                        {
                                            dataDto.GIABANLEVAT = GIABANLEVAT;
                                        }
                                        else
                                        {
                                            if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                            {
                                                dataDto.GIABANLEVAT = GIABANBUONVAT;
                                            }
                                            else
                                            {
                                                dataDto.GIABANLEVAT = GIAVON;
                                            }
                                        }
                                        dataDto.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                        dataDto.GIAVON = GIAVON * (1 + TYLEVATRA / 100);
                                        dataDto.TYLEVATRA = TYLEVATRA;
                                        dataDto.TYLELAILE = TYLELAILE;
                                        decimal.TryParse(soLuongItemCode, out SOLUONG);
                                        dataDto.SOLUONG = SOLUONG;
                                        listDataDto.Add(dataDto);
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
                //BÁN MÃ BÓ HÀNG
                if (beginCharacter.Equals("BH"))
                {
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
                                        @"SELECT a.MABOHANG AS MAVATTU,a.TENBOHANG AS TENVATTU,'BÓ' AS DONVITINH,'' AS MANHACUNGCAP,SUM(b.TONGBUON) AS GIABANBUONVAT,SUM(b.TONGLE) AS GIABANLEVAT,'' AS ITEMCODE FROM dbo.DM_BOHANG a,dbo.DM_BOHANGCHITIET b WHERE a.MABOHANG = b.MABOHANG AND a.MABOHANG = '" +
                                        MaVatTu + "' AND a.UNITCODE = '" + Session.Session.CurrentUnitCode +
                                        "' GROUP BY a.MABOHANG,a.TENBOHANG");
                                SqlDataReader dataReader = cmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        decimal GIABANBUONVAT, GIABANLEVAT, SOLUONG = 0;
                                        VATTU_DTO dataDto = new VATTU_DTO();
                                        dataDto.MAVATTU = dataReader["MAVATTU"].ToString();
                                        dataDto.MABO = dataDto.MAVATTU;
                                        dataDto.TENVATTU = dataReader["TENVATTU"].ToString();
                                        dataDto.DONVITINH = dataReader["DONVITINH"].ToString();
                                        dataDto.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                        decimal.TryParse(dataReader["GIABANBUONVAT"].ToString(), out GIABANBUONVAT);
                                        dataDto.GIABANBUONVAT = GIABANBUONVAT;
                                        decimal.TryParse(dataReader["GIABANLEVAT"].ToString(), out GIABANLEVAT);
                                        dataDto.GIABANLEVAT = GIABANLEVAT;
                                        dataDto.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                        dataDto.SOLUONG = SOLUONG;
                                        SqlCommand cmdBoHangChiTiet = new SqlCommand();
                                        cmdBoHangChiTiet.Connection = connection;
                                        cmdBoHangChiTiet.CommandText = "SELECT TOP 1 MAHANG FROM dbo.DM_BOHANGCHITIET WHERE MABOHANG = '" + dataDto.MAVATTU + "' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'";
                                        SqlDataReader dataReaderBoHangChiTiet = cmdBoHangChiTiet.ExecuteReader();
                                        if (dataReaderBoHangChiTiet.HasRows)
                                        {
                                            while (dataReaderBoHangChiTiet.Read())
                                            {
                                                string maVatTuBoHang = dataReaderBoHangChiTiet["MAHANG"] != null
                                                    ? dataReaderBoHangChiTiet["MAHANG"].ToString().ToUpper().Trim()
                                                    : "";
                                                SqlCommand cmdVatTu = new SqlCommand();
                                                cmdVatTu.Connection = connection;
                                                cmdVatTu.CommandText = "SELECT MAVATRA,TYLEVATRA FROM DM_VATTU WHERE MAVATTU = '" + maVatTuBoHang + "' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'";
                                                SqlDataReader dataReaderVatTu = cmdVatTu.ExecuteReader();
                                                if (dataReaderVatTu.HasRows)
                                                {
                                                    while (dataReaderVatTu.Read())
                                                    {
                                                        dataDto.MAVATRA = dataReaderVatTu["MAVATRA"] != null ? dataReaderVatTu["MAVATRA"].ToString() : "";
                                                        decimal TYLEVATRA = 0;
                                                        if (dataReaderVatTu["TYLEVATRA"] != null)
                                                        {
                                                            decimal.TryParse(dataReaderVatTu["TYLEVATRA"].ToString(),
                                                                out TYLEVATRA);
                                                        }
                                                        else
                                                        {
                                                            TYLEVATRA = 0;
                                                        }
                                                        dataDto.TYLEVATRA = TYLEVATRA;
                                                    }
                                                }
                                            }
                                        }
                                        listDataDto.Add(dataDto);
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
                        else
                        {
                            NotificationLauncher.ShowNotificationError("Thông báo", "Không có kế nối với cơ sở dữ liệu", 1, "0x1", "0x8", "normal");
                        }
                    }
                }
                else if (MaVatTu.Length == 7)
                {
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
                                    string.Format(@"SELECT [ID],[MAVATTU],[TENVATTU],[MANHACUNGCAP],[DONVITINH],[BARCODE],[GIABANBUONVAT],[GIABANLEVAT],[GIAVON],[TONCUOIKYSL],[TYLEVATRA],[TYLELAILE],[ITEMCODE],[MAVATRA],[UNITCODE]  FROM [dbo].[DM_VATTU] WHERE MAVATTU = '" + MaVatTu +
                                        "' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                                SqlDataReader dataReader = cmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        decimal GIABANBUONVAT, GIABANLEVAT, GIAVON, TYLEVATRA, SOLUONG = 0;
                                        VATTU_DTO dataDto = new VATTU_DTO();
                                        dataDto.MAVATTU = dataReader["MAVATTU"].ToString();
                                        dataDto.TENVATTU = dataReader["TENVATTU"].ToString();
                                        dataDto.DONVITINH = dataReader["DONVITINH"].ToString();
                                        dataDto.BARCODE = dataReader["BARCODE"].ToString();
                                        if (!string.IsNullOrEmpty(dataDto.BARCODE))
                                        {
                                            if (dataDto.BARCODE.Length <= 2)
                                            {
                                                dataDto.BARCODE = ";";
                                            }
                                        }
                                        else
                                        {
                                            dataDto.BARCODE = ";";
                                        }
                                        dataDto.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                        dataDto.MAVATRA = dataReader["MAVATRA"].ToString();
                                        decimal.TryParse(dataReader["TYLEVATRA"].ToString(), out TYLEVATRA);
                                        decimal.TryParse(dataReader["GIABANBUONVAT"].ToString(), out GIABANBUONVAT);
                                        dataDto.GIABANBUONVAT = GIABANBUONVAT;
                                        decimal.TryParse(dataReader["GIABANLEVAT"].ToString(), out GIABANLEVAT);
                                        decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                        if (PhuongThucTinhGia == EnumCommon.MethodGetPrice.GIABANLECOVAT)
                                        {
                                            dataDto.GIABANLEVAT = GIABANLEVAT;
                                        }
                                        else
                                        {
                                            if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                            {
                                                dataDto.GIABANLEVAT = GIABANBUONVAT;
                                            }
                                            else
                                            {
                                                dataDto.GIABANLEVAT = GIAVON;
                                            }
                                        }
                                        dataDto.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                        dataDto.GIAVON = GIAVON * (1 + TYLEVATRA / 100);
                                        dataDto.TYLEVATRA = TYLEVATRA;
                                        dataDto.SOLUONG = SOLUONG;
                                        listDataDto.Add(dataDto);
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
                        else
                        {
                            NotificationLauncher.ShowNotificationError("Thông báo", "Không có kế nối với cơ sở dữ liệu", 1, "0x1", "0x8", "normal");
                        }
                    }
                }
                else
                {
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
                                        @"SELECT [ID],[MAVATTU],[TENVATTU],[MANHACUNGCAP],[DONVITINH],[BARCODE],[GIABANBUONVAT],[GIABANLEVAT],[GIAVON],[TONCUOIKYSL],[TYLEVATRA],[TYLELAILE],[ITEMCODE],[MAVATRA],[UNITCODE]  FROM [dbo].[DM_VATTU] WHERE BARCODE LIKE '%" + MaVatTu + "%' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                                SqlDataReader dataReader = cmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        decimal GIABANBUONVAT, GIABANLEVAT, GIAVON, TYLEVATRA, SOLUONG = 0;
                                        VATTU_DTO dataDto = new VATTU_DTO();
                                        dataDto.MAVATTU = dataReader["MAVATTU"].ToString();
                                        dataDto.TENVATTU = dataReader["TENVATTU"].ToString();
                                        dataDto.DONVITINH = dataReader["DONVITINH"].ToString();
                                        dataDto.BARCODE = dataReader["BARCODE"].ToString();
                                        if (!string.IsNullOrEmpty(dataDto.BARCODE))
                                        {
                                            if (dataDto.BARCODE.Length <= 2)
                                            {
                                                dataDto.BARCODE = ";";
                                            }
                                        }
                                        else
                                        {
                                            dataDto.BARCODE = ";";
                                        }
                                        dataDto.MANHACUNGCAP = dataReader["MANHACUNGCAP"].ToString();
                                        dataDto.MAVATRA = dataReader["MAVATRA"].ToString();
                                        decimal.TryParse(dataReader["TYLEVATRA"].ToString(), out TYLEVATRA);
                                        decimal.TryParse(dataReader["GIABANBUONVAT"].ToString(), out GIABANBUONVAT);
                                        dataDto.GIABANBUONVAT = GIABANBUONVAT;
                                        decimal.TryParse(dataReader["GIABANLEVAT"].ToString(), out GIABANLEVAT);
                                        decimal.TryParse(dataReader["GIAVON"].ToString(), out GIAVON);
                                        if (PhuongThucTinhGia == EnumCommon.MethodGetPrice.GIABANLECOVAT)
                                        {
                                            dataDto.GIABANLEVAT = GIABANLEVAT;
                                        }
                                        else
                                        {
                                            if (Session.Session.CurrentLoaiGiaoDich == "BANBUON")
                                            {
                                                dataDto.GIABANLEVAT = GIABANBUONVAT;
                                            }
                                            else
                                            {
                                                dataDto.GIABANLEVAT = GIAVON;
                                            }
                                        }
                                        dataDto.GIAVON = GIAVON * (1 + TYLEVATRA / 100);
                                        dataDto.ITEMCODE = dataReader["ITEMCODE"].ToString();
                                        dataDto.TYLEVATRA = TYLEVATRA;
                                        dataDto.SOLUONG = SOLUONG;
                                        listDataDto.Add(dataDto);
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
                        else
                        {
                            NotificationLauncher.ShowNotificationError("Thông báo", "Không có kế nối với cơ sở dữ liệu", 1, "0x1", "0x8", "normal");
                        }
                    }
                }
            }
            return listDataDto;
        }

        public static string CONVERT_MACAN_TO_MAVATTU_ORACLE(string KEY, string UNITCODE)
        {
            string _MAVATTU = "";
            if (!string.IsNullOrEmpty(KEY))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT MAVATTU FROM V_VATTU_GIABAN WHERE ITEMCODE = '" + KEY + "' AND MADONVI = '" + UNITCODE + "' ");
                            OracleCommand commamdVatTu = new OracleCommand();
                            commamdVatTu.Connection = connection;
                            commamdVatTu.CommandText = querrySelect;
                            OracleDataReader dataReaderVatTu = commamdVatTu.ExecuteReader();
                            if (dataReaderVatTu.HasRows)
                            {
                                while (dataReaderVatTu.Read())
                                {
                                    if (dataReaderVatTu["MAVATTU"] != null)
                                    {
                                        _MAVATTU = dataReaderVatTu["MAVATTU"].ToString().ToUpper().Trim();
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
                }
            }
            return _MAVATTU;
        }

        public static string CONVERT_MACAN_TO_MAVATTU_SQLSERVER(string KEY, string UNITCODE)
        {
            string _MAVATTU = "";
            if (!string.IsNullOrEmpty(KEY))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT MAVATTU FROM DM_VATTU WHERE ITEMCODE = '" + KEY + "' AND UNITCODE = '" + UNITCODE + "' ");
                            SqlCommand commamdVatTu = new SqlCommand();
                            commamdVatTu.Connection = connection;
                            commamdVatTu.CommandText = querrySelect;
                            SqlDataReader dataReaderVatTu = commamdVatTu.ExecuteReader();
                            if (dataReaderVatTu.HasRows)
                            {
                                while (dataReaderVatTu.Read())
                                {
                                    if (dataReaderVatTu["MAVATTU"] != null)
                                    {
                                        _MAVATTU = dataReaderVatTu["MAVATTU"].ToString().ToUpper().Trim();
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
                }
            }
            return _MAVATTU;
        }

        public static string CONVERT_BARCODE_TO_MAVATTU_ORACLE(string KEY, string UNITCODE)
        {
            string _MAVATTU = "";
            if (!string.IsNullOrEmpty(KEY))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT MAVATTU FROM V_VATTU_GIABAN WHERE BARCODE LIKE '%" + KEY + "%' AND MADONVI = '" + UNITCODE + "' ");
                            OracleCommand commamdVatTu = new OracleCommand();
                            commamdVatTu.Connection = connection;
                            commamdVatTu.CommandText = querrySelect;
                            OracleDataReader dataReaderVatTu = commamdVatTu.ExecuteReader();
                            if (dataReaderVatTu.HasRows)
                            {
                                while (dataReaderVatTu.Read())
                                {
                                    if (dataReaderVatTu["MAVATTU"] != null)
                                    {
                                        _MAVATTU = dataReaderVatTu["MAVATTU"].ToString().ToUpper().Trim();
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
                }
            }
            return _MAVATTU;
        }

        public static string CONVERT_BARCODE_TO_MAVATTU_SQLSERVER(string KEY, string UNITCODE)
        {
            string _MAVATTU = "";
            if (!string.IsNullOrEmpty(KEY))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT MAVATTU FROM DM_VATTU WHERE BARCODE LIKE '%" + KEY + "%' AND UNITCODE = '" + UNITCODE + "' ");
                            SqlCommand commamdVatTu = new SqlCommand();
                            commamdVatTu.Connection = connection;
                            commamdVatTu.CommandText = querrySelect;
                            SqlDataReader dataReaderVatTu = commamdVatTu.ExecuteReader();
                            if (dataReaderVatTu.HasRows)
                            {
                                while (dataReaderVatTu.Read())
                                {
                                    if (dataReaderVatTu["MAVATTU"] != null)
                                    {
                                        _MAVATTU = dataReaderVatTu["MAVATTU"].ToString().ToUpper().Trim();
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
                }
            }
            return _MAVATTU;
        }

        public static EXTEND_VATTU_DTO LAYDULIEU_HANGHOA_FROM_DATABASE_SQLSERVER(string MaVatTu, string UnitCode)
        {
            EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
            if (!string.IsNullOrEmpty(MaVatTu))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT TENVATTU,DONVITINH,BARCODE,MAVATRA,TYLEVATRA,TYLELAILE,GIABANLEVAT FROM dbo.DM_VATTU WHERE MAVATTU = '" + MaVatTu + "' AND UNITCODE = '" + UnitCode + "'");
                            SqlCommand commamdSelectVatTu = new SqlCommand();
                            commamdSelectVatTu.Connection = connection;
                            commamdSelectVatTu.CommandText = querrySelect;
                            SqlDataReader dataReaderSelectVatTu = commamdSelectVatTu.ExecuteReader();
                            if (dataReaderSelectVatTu.HasRows)
                            {
                                while (dataReaderSelectVatTu.Read())
                                {
                                    if (dataReaderSelectVatTu["TENVATTU"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.TENVATTU = dataReaderSelectVatTu["TENVATTU"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.TENVATTU = "";
                                    }
                                    if (dataReaderSelectVatTu["DONVITINH"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.DONVITINH = dataReaderSelectVatTu["DONVITINH"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.DONVITINH = "";
                                    }
                                    if (dataReaderSelectVatTu["BARCODE"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.BARCODE = dataReaderSelectVatTu["BARCODE"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.BARCODE = "";
                                    }
                                    if (dataReaderSelectVatTu["MAVATRA"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.MAVATRA = dataReaderSelectVatTu["MAVATRA"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.MAVATRA = "";
                                    }
                                    _EXTEND_VATTU_DTO.MAKEHANG = "";
                                    decimal TYLEVATRA = 0;
                                    if (dataReaderSelectVatTu["TYLEVATRA"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatTu["TYLEVATRA"].ToString(), out TYLEVATRA);
                                        _EXTEND_VATTU_DTO.TYLEVATRA = TYLEVATRA;
                                    }
                                    decimal TYLELAILE = 0;
                                    if (dataReaderSelectVatTu["TYLELAILE"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatTu["TYLELAILE"].ToString(), out TYLELAILE);
                                        _EXTEND_VATTU_DTO.TYLELAILE = TYLELAILE;
                                    }
                                    decimal GIABANLEVAT = 0;
                                    if (dataReaderSelectVatTu["GIABANLEVAT"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatTu["GIABANLEVAT"].ToString(), out GIABANLEVAT);
                                        _EXTEND_VATTU_DTO.GIABANLEVAT = GIABANLEVAT;
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
                }
            }
            return _EXTEND_VATTU_DTO;
        }
        public static List<EXTEND_BOHANGCHITIET_DTO> LAYDULIEU_BOHANGCHITIET_FROM_DATABASE_SQLSERVER(string MaBoHang, string UnitCode)
        {
            List<EXTEND_BOHANGCHITIET_DTO> _LST_EXTEND_BOHANGCHITIET_DTO = new List<EXTEND_BOHANGCHITIET_DTO>();
            if (!string.IsNullOrEmpty(MaBoHang))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            SqlCommand commandBoHang = new SqlCommand();
                            commandBoHang.Connection = connection;
                            commandBoHang.CommandText = string.Format(@"SELECT DM_BOHANGCHITIET.MAHANG,DM_BOHANGCHITIET.SOLUONG,DM_BOHANGCHITIET.TYLECKLE,DM_BOHANGCHITIET.TONGLE FROM dbo.DM_BOHANG INNER JOIN dbo.DM_BOHANGCHITIET ON DM_BOHANG.MABOHANG = DM_BOHANGCHITIET.MABOHANG WHERE DM_BOHANG.MABOHANG = '" + MaBoHang + "' AND DM_BOHANG.UNITCODE = '" + UnitCode + "'");
                            SqlDataReader dataReaderBoHang = commandBoHang.ExecuteReader();
                            if (dataReaderBoHang.HasRows)
                            {
                                while (dataReaderBoHang.Read())
                                {
                                    EXTEND_BOHANGCHITIET_DTO _EXTEND_BOHANGCHITIET_DTO = new EXTEND_BOHANGCHITIET_DTO();
                                    if (dataReaderBoHang["MAHANG"] != null)
                                    {
                                        _EXTEND_BOHANGCHITIET_DTO.MAHANG = dataReaderBoHang["MAHANG"].ToString();
                                    }

                                    decimal SOLUONG = 0;
                                    if (dataReaderBoHang["SOLUONG"] != null)
                                    {
                                        decimal.TryParse(dataReaderBoHang["SOLUONG"].ToString(), out SOLUONG);
                                    }
                                    _EXTEND_BOHANGCHITIET_DTO.SOLUONG = SOLUONG;

                                    decimal TYLECKLE = 0;
                                    if (dataReaderBoHang["TYLECKLE"] != null)
                                    {
                                        decimal.TryParse(dataReaderBoHang["TYLECKLE"].ToString(), out TYLECKLE);
                                    }
                                    _EXTEND_BOHANGCHITIET_DTO.TYLECKLE = TYLECKLE;

                                    decimal TONGLE = 0;
                                    if (dataReaderBoHang["TONGLE"] != null)
                                    {
                                        decimal.TryParse(dataReaderBoHang["TONGLE"].ToString(), out TONGLE);
                                    }
                                    _EXTEND_BOHANGCHITIET_DTO.TONGLE = TONGLE;
                                    _LST_EXTEND_BOHANGCHITIET_DTO.Add(_EXTEND_BOHANGCHITIET_DTO);
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
                }
            }
            return _LST_EXTEND_BOHANGCHITIET_DTO;
        }
        public static EXTEND_VAT_BOHANG LAYDULIEU_VAT_BOHANG_FROM_DATABASE_ORACLE(string MaBoHang, string UnitCode)
        {
            EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();
            if (!string.IsNullOrEmpty(MaBoHang))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT MAX(V_VATTU_GIABAN.MAVATRA) AS MAVATRA,MAX(V_VATTU_GIABAN.TYLEVATRA) AS TYLEVATRA FROM DM_BOHANG 
                                                                INNER JOIN DM_BOHANGCHITIET ON DM_BOHANG.MABOHANG = DM_BOHANGCHITIET.MABOHANG 
                                                                INNER JOIN V_VATTU_GIABAN ON DM_BOHANGCHITIET.MAHANG = V_VATTU_GIABAN.MAVATTU 
                                                                WHERE DM_BOHANG.MABOHANG = '" + MaBoHang + "' AND DM_BOHANG.UNITCODE = '" + UnitCode + "' ");
                            OracleCommand commamdSelectVatBoHang = new OracleCommand();
                            commamdSelectVatBoHang.Connection = connection;
                            commamdSelectVatBoHang.CommandText = querrySelect;
                            OracleDataReader dataReaderSelectVatBoHang = commamdSelectVatBoHang.ExecuteReader();
                            if (dataReaderSelectVatBoHang.HasRows)
                            {
                                while (dataReaderSelectVatBoHang.Read())
                                {
                                    _EXTEND_VAT_BOHANG.MAVATRA = "";
                                    if (dataReaderSelectVatBoHang["MAVATRA"] != null)
                                    {
                                        _EXTEND_VAT_BOHANG.MAVATRA = dataReaderSelectVatBoHang["MAVATRA"].ToString();
                                    }
                                    decimal TYLEVATRA = 0;
                                    if (dataReaderSelectVatBoHang["TYLEVATRA"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatBoHang["TYLEVATRA"].ToString(), out TYLEVATRA);
                                    }
                                    _EXTEND_VAT_BOHANG.TYLEVATRA = TYLEVATRA;
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
                }
            }
            return _EXTEND_VAT_BOHANG;
        }
        public static EXTEND_VAT_BOHANG LAYDULIEU_VAT_BOHANG_FROM_DATABASE_SQLSERVER(string MaBoHang, string UnitCode)
        {
            EXTEND_VAT_BOHANG _EXTEND_VAT_BOHANG = new EXTEND_VAT_BOHANG();
            if (!string.IsNullOrEmpty(MaBoHang))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT MAX(DM_VATTU.MAVATRA) AS MAVATRA,MAX(DM_VATTU.TYLEVATRA) AS TYLEVATRA FROM DM_BOHANG 
                                                                INNER JOIN DM_BOHANGCHITIET ON DM_BOHANG.MABOHANG = DM_BOHANGCHITIET.MABOHANG 
                                                                INNER JOIN DM_VATTU ON DM_BOHANGCHITIET.MAHANG = DM_VATTU.MAVATTU 
                                                                WHERE DM_BOHANG.MABOHANG = '" + MaBoHang + "' AND DM_BOHANG.UNITCODE = '" + UnitCode + "' ");
                            SqlCommand commamdSelectVatBoHang = new SqlCommand();
                            commamdSelectVatBoHang.Connection = connection;
                            commamdSelectVatBoHang.CommandText = querrySelect;
                            SqlDataReader dataReaderSelectVatBoHang = commamdSelectVatBoHang.ExecuteReader();
                            if (dataReaderSelectVatBoHang.HasRows)
                            {
                                while (dataReaderSelectVatBoHang.Read())
                                {
                                    _EXTEND_VAT_BOHANG.MAVATRA = "";
                                    if (dataReaderSelectVatBoHang["MAVATRA"] != null)
                                    {
                                        _EXTEND_VAT_BOHANG.MAVATRA = dataReaderSelectVatBoHang["MAVATRA"].ToString();
                                    }
                                    decimal TYLEVATRA = 0;
                                    if (dataReaderSelectVatBoHang["TYLEVATRA"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatBoHang["TYLEVATRA"].ToString(), out TYLEVATRA);
                                    }
                                    _EXTEND_VAT_BOHANG.TYLEVATRA = TYLEVATRA;
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
                }
            }
            return _EXTEND_VAT_BOHANG;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaVatTu"></param>
        /// <param name="UnitCode"></param>
        /// <returns></returns>
        public static EXTEND_VATTU_DTO LAYDULIEU_HANGHOA_FROM_DATABASE_ORACLE(string MaVatTu, string UnitCode)
        {
            EXTEND_VATTU_DTO _EXTEND_VATTU_DTO = new EXTEND_VATTU_DTO();
            if (!string.IsNullOrEmpty(MaVatTu))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string querrySelect = string.Format(@"SELECT TENVATTU,DONVITINH,BARCODE,MAVATRA,TYLEVATRA,TYLELAILE,MAKEHANG,GIABANLEVAT FROM V_VATTU_GIABAN WHERE MAVATTU = '" + MaVatTu + "' AND MADONVI = '" + UnitCode + "'");
                            OracleCommand commamdSelectVatTu = new OracleCommand();
                            commamdSelectVatTu.Connection = connection;
                            commamdSelectVatTu.CommandText = querrySelect;
                            OracleDataReader dataReaderSelectVatTu = commamdSelectVatTu.ExecuteReader();
                            if (dataReaderSelectVatTu.HasRows)
                            {
                                while (dataReaderSelectVatTu.Read())
                                {
                                    if (dataReaderSelectVatTu["TENVATTU"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.TENVATTU = dataReaderSelectVatTu["TENVATTU"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.TENVATTU = "";
                                    }
                                    if (dataReaderSelectVatTu["DONVITINH"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.DONVITINH = dataReaderSelectVatTu["DONVITINH"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.DONVITINH = "";
                                    }
                                    if (dataReaderSelectVatTu["BARCODE"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.BARCODE = dataReaderSelectVatTu["BARCODE"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.BARCODE = "";
                                    }
                                    if (dataReaderSelectVatTu["MAVATRA"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.MAVATRA = dataReaderSelectVatTu["MAVATRA"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.MAVATRA = "";
                                    }
                                    if (dataReaderSelectVatTu["MAKEHANG"] != null)
                                    {
                                        _EXTEND_VATTU_DTO.MAKEHANG = dataReaderSelectVatTu["MAKEHANG"].ToString();
                                    }
                                    else
                                    {
                                        _EXTEND_VATTU_DTO.MAKEHANG = "";
                                    }
                                    decimal TYLEVATRA = 0;
                                    if (dataReaderSelectVatTu["TYLEVATRA"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatTu["TYLEVATRA"].ToString(), out TYLEVATRA);
                                        _EXTEND_VATTU_DTO.TYLEVATRA = TYLEVATRA;
                                    }
                                    decimal TYLELAILE = 0;
                                    if (dataReaderSelectVatTu["TYLELAILE"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatTu["TYLELAILE"].ToString(), out TYLELAILE);
                                        _EXTEND_VATTU_DTO.TYLELAILE = TYLELAILE;
                                    }
                                    decimal GIABANLEVAT = 0;
                                    if (dataReaderSelectVatTu["GIABANLEVAT"] != null)
                                    {
                                        decimal.TryParse(dataReaderSelectVatTu["GIABANLEVAT"].ToString(), out GIABANLEVAT);
                                        _EXTEND_VATTU_DTO.GIABANLEVAT = GIABANLEVAT;
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
                }
            }
            return _EXTEND_VATTU_DTO;
        }
        public static List<EXTEND_BOHANGCHITIET_DTO> LAYDULIEU_BOHANGCHITIET_FROM_DATABASE_ORACLE(string MaBoHang, string UnitCode)
        {
            List<EXTEND_BOHANGCHITIET_DTO> _LST_EXTEND_BOHANGCHITIET_DTO = new List<EXTEND_BOHANGCHITIET_DTO>();
            if (!string.IsNullOrEmpty(MaBoHang))
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            OracleCommand commandBoHang = new OracleCommand();
                            commandBoHang.Connection = connection;
                            commandBoHang.CommandText = string.Format(@"SELECT b.MAHANG,b.SOLUONG,b.TYLECKLE,b.TONGLE FROM DM_BOHANG a INNER JOIN DM_BOHANGCHITIET b ON a.MABOHANG = b.MABOHANG WHERE  a.MABOHANG = '" + MaBoHang + "' AND a.UNITCODE = '" + UnitCode + "'");
                            OracleDataReader dataReaderBoHang = commandBoHang.ExecuteReader();
                            if (dataReaderBoHang.HasRows)
                            {
                                while (dataReaderBoHang.Read())
                                {
                                    EXTEND_BOHANGCHITIET_DTO _EXTEND_BOHANGCHITIET_DTO = new EXTEND_BOHANGCHITIET_DTO();
                                    if (dataReaderBoHang["MAHANG"] != null)
                                    {
                                        _EXTEND_BOHANGCHITIET_DTO.MAHANG = dataReaderBoHang["MAHANG"].ToString();
                                    }

                                    decimal SOLUONG = 0;
                                    if (dataReaderBoHang["SOLUONG"] != null)
                                    {
                                        decimal.TryParse(dataReaderBoHang["SOLUONG"].ToString(), out SOLUONG);
                                    }
                                    _EXTEND_BOHANGCHITIET_DTO.SOLUONG = SOLUONG;

                                    decimal TYLECKLE = 0;
                                    if (dataReaderBoHang["TYLECKLE"] != null)
                                    {
                                        decimal.TryParse(dataReaderBoHang["TYLECKLE"].ToString(), out TYLECKLE);
                                    }
                                    _EXTEND_BOHANGCHITIET_DTO.TYLECKLE = TYLECKLE;

                                    decimal TONGLE = 0;
                                    if (dataReaderBoHang["TONGLE"] != null)
                                    {
                                        decimal.TryParse(dataReaderBoHang["TONGLE"].ToString(), out TONGLE);
                                    }
                                    _EXTEND_BOHANGCHITIET_DTO.TONGLE = TONGLE;
                                    _LST_EXTEND_BOHANGCHITIET_DTO.Add(_EXTEND_BOHANGCHITIET_DTO);
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
                }
            }
            return _LST_EXTEND_BOHANGCHITIET_DTO;
        }


        public static int GET_THAMSO_KHOABANAM_FROM_ORACLE()
        {
            int GIATRI_THAMSO = 0;
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        string querrySelect = string.Format(@"SELECT GIATRI_THAMSO FROM AU_THAMSOHETHONG WHERE MA_THAMSO = 'LOCK_BANAM' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "' ");
                        OracleCommand commamdThamSo = new OracleCommand();
                        commamdThamSo.Connection = connection;
                        commamdThamSo.CommandText = querrySelect;
                        OracleDataReader dataReaderThamSo = commamdThamSo.ExecuteReader();
                        if (dataReaderThamSo.HasRows)
                        {
                            while (dataReaderThamSo.Read())
                            {
                                if (dataReaderThamSo["GIATRI_THAMSO"] != null)
                                {
                                    int.TryParse(dataReaderThamSo["GIATRI_THAMSO"].ToString(), out GIATRI_THAMSO);
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
            }
            return GIATRI_THAMSO;
        }


        public static int GET_THAMSO_KHOABANAM_FROM_SQLSERVER()
        {
            int GIATRI_THAMSO = 0;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        string querrySelect = string.Format(@"SELECT GIATRI_THAMSO FROM AU_THAMSOHETHONG WHERE MA_THAMSO = 'LOCK_BANAM' AND UNITCODE = '" + Session.Session.CurrentUnitCode + "' ");
                        SqlCommand commamdThamSo = new SqlCommand();
                        commamdThamSo.Connection = connection;
                        commamdThamSo.CommandText = querrySelect;
                        SqlDataReader dataReaderThamSo = commamdThamSo.ExecuteReader();
                        if (dataReaderThamSo.HasRows)
                        {
                            while (dataReaderThamSo.Read())
                            {
                                if (dataReaderThamSo["GIATRI_THAMSO"] != null)
                                {
                                    int.TryParse(dataReaderThamSo["GIATRI_THAMSO"].ToString(), out GIATRI_THAMSO);
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
            }
            return GIATRI_THAMSO;
        }
    }
}

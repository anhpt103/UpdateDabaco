using BTS.SP.BANLE.Giaodich.XuatBanLe;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BTS.SP.BANLE.Common
{
    public class SYNCHRONIZE_DATA
    {
        public static void SYNCHRONIZE_AU_NGUOIDUNG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID,USERNAME,PASSWORD,MANHANVIEN,TENNHANVIEN,SODIENTHOAI,SOCHUNGMINHTHU,GIOITINH,TRANGTHAI,""LEVEL"",UNITCODE,PARENT_UNITCODE FROM AU_NGUOIDUNG WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdDeleteSa = new SqlCommand();
                                            cmdDeleteSa.Connection = connectionSa;
                                            cmdDeleteSa.CommandText = string.Format(@"DELETE AU_NGUOIDUNG");
                                            cmdDeleteSa.Transaction = tranSa;
                                            cmdDeleteSa.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                SqlCommand cmdInsertSa = new SqlCommand();
                                                cmdInsertSa.Connection = connectionSa;
                                                cmdInsertSa.CommandText = string.Format(@"INSERT INTO dbo.AU_NGUOIDUNG(ID,USERNAME,PASSWORD,MANHANVIEN,TENNHANVIEN,SODIENTHOAI,SOCHUNGMINHTHU,GIOITINH,TRANGTHAI,LEVEL,UNITCODE,PARENT_UNITCODE) VALUES (@ID,@USERNAME,@PASSWORD,@MANHANVIEN,@TENNHANVIEN,@SODIENTHOAI,@SOCHUNGMINHTHU,@GIOITINH,@TRANGTHAI,@LEVEL,@UNITCODE,@PARENT_UNITCODE)");
                                                cmdInsertSa.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = dataReaderOrcl["ID"] != null ? dataReaderOrcl["ID"].ToString().Trim() : Guid.NewGuid().ToString();
                                                cmdInsertSa.Parameters.Add("USERNAME", SqlDbType.VarChar, 50).Value = dataReaderOrcl["USERNAME"] != null ? dataReaderOrcl["USERNAME"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("PASSWORD", SqlDbType.VarChar, 50).Value = dataReaderOrcl["PASSWORD"] != null ? dataReaderOrcl["PASSWORD"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("MANHANVIEN", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MANHANVIEN"] != null ? dataReaderOrcl["MANHANVIEN"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("TENNHANVIEN ", SqlDbType.NVarChar, 200).Value = dataReaderOrcl["TENNHANVIEN"] != null ? dataReaderOrcl["TENNHANVIEN"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("SODIENTHOAI", SqlDbType.VarChar, 20).Value = dataReaderOrcl["SODIENTHOAI"] != null ? dataReaderOrcl["SODIENTHOAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("SOCHUNGMINHTHU", SqlDbType.VarChar, 20).Value = dataReaderOrcl["SOCHUNGMINHTHU"] != null ? dataReaderOrcl["SOCHUNGMINHTHU"].ToString().Trim() : (object)DBNull.Value;
                                                int gioiTinh = 0;
                                                if (dataReaderOrcl["GIOITINH"] != null) int.TryParse(dataReaderOrcl["GIOITINH"].ToString(), out gioiTinh);
                                                cmdInsertSa.Parameters.Add("GIOITINH", SqlDbType.Int).Value = gioiTinh;
                                                int trangThai = 0;
                                                if (dataReaderOrcl["TRANGTHAI"] != null) int.TryParse(dataReaderOrcl["TRANGTHAI"].ToString(), out trangThai);
                                                cmdInsertSa.Parameters.Add("TRANGTHAI", SqlDbType.Int).Value = trangThai;
                                                int level = 0;
                                                if (dataReaderOrcl["LEVEL"] != null) int.TryParse(dataReaderOrcl["LEVEL"].ToString(), out level);
                                                cmdInsertSa.Parameters.Add("LEVEL ", SqlDbType.Int).Value = level;
                                                cmdInsertSa.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("PARENT_UNITCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["PARENT_UNITCODE"] != null ? dataReaderOrcl["PARENT_UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Transaction = tranSa;
                                                if (cmdInsertSa.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert ++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }

        public static void SYNCHRONIZE_DM_BOHANG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID,MABOHANG,TENBOHANG,GHICHU,TRANGTHAI,UNITCODE FROM DM_BOHANG");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdDeleteSa = new SqlCommand();
                                            cmdDeleteSa.Connection = connectionSa;
                                            cmdDeleteSa.CommandText = string.Format(@"DELETE DM_BOHANG");
                                            cmdDeleteSa.Transaction = tranSa;
                                            cmdDeleteSa.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                SqlCommand cmdInsertSa = new SqlCommand();
                                                cmdInsertSa.Connection = connectionSa;
                                                cmdInsertSa.CommandText = string.Format(@"INSERT INTO dbo.DM_BOHANG(ID,MABOHANG,TENBOHANG,GHICHU,TRANGTHAI,UNITCODE) VALUES (@ID,@MABOHANG,@TENBOHANG,@GHICHU,@TRANGTHAI,@UNITCODE)");
                                                cmdInsertSa.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = dataReaderOrcl["ID"] != null ? dataReaderOrcl["ID"].ToString().Trim() : Guid.NewGuid().ToString();
                                                cmdInsertSa.Parameters.Add("MABOHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MABOHANG"] != null ? dataReaderOrcl["MABOHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("TENBOHANG", SqlDbType.NVarChar, 300).Value = dataReaderOrcl["TENBOHANG"] != null ? dataReaderOrcl["TENBOHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("GHICHU", SqlDbType.NVarChar, 500).Value = dataReaderOrcl["GHICHU"] != null ? dataReaderOrcl["GHICHU"].ToString().Trim() : (object)DBNull.Value;
                                                int trangThai = 0;
                                                if (dataReaderOrcl["TRANGTHAI"] != null) int.TryParse(dataReaderOrcl["TRANGTHAI"].ToString(), out trangThai);
                                                cmdInsertSa.Parameters.Add("TRANGTHAI", SqlDbType.Int).Value = trangThai;
                                                cmdInsertSa.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Transaction = tranSa;
                                                if (cmdInsertSa.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }

        public static void SYNCHRONIZE_DM_BOHANGCHITIET()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID,MABOHANG,MAHANG,TENHANG,SOLUONG,TYLECKLE,TYLECKBUON,TONGLE,DONGIA,TONGBUON,GHICHU,TRANGTHAI,UNITCODE FROM DM_BOHANGCHITIET");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdDeleteSa = new SqlCommand();
                                            cmdDeleteSa.Connection = connectionSa;
                                            cmdDeleteSa.CommandText = string.Format(@"DELETE DM_BOHANGCHITIET");
                                            cmdDeleteSa.Transaction = tranSa;
                                            cmdDeleteSa.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                SqlCommand cmdInsertSa = new SqlCommand();
                                                cmdInsertSa.Connection = connectionSa;
                                                cmdInsertSa.CommandText = string.Format(@"INSERT INTO dbo.DM_BOHANGCHITIET(ID,MABOHANG,MAHANG,TENHANG,SOLUONG,TYLECKLE,TYLECKBUON,TONGLE,DONGIA,TONGBUON,GHICHU,TRANGTHAI,UNITCODE) VALUES (@ID,@MABOHANG,@MAHANG,@TENHANG,@SOLUONG,@TYLECKLE,@TYLECKBUON,@TONGLE,@DONGIA,@TONGBUON,@GHICHU,@TRANGTHAI,@UNITCODE)");
                                                cmdInsertSa.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = dataReaderOrcl["ID"] != null ? dataReaderOrcl["ID"].ToString().Trim() : Guid.NewGuid().ToString();
                                                cmdInsertSa.Parameters.Add("MABOHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MABOHANG"] != null ? dataReaderOrcl["MABOHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("MAHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAHANG"] != null ? dataReaderOrcl["MAHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("TENHANG", SqlDbType.NVarChar, 500).Value = dataReaderOrcl["TENHANG"] != null ? dataReaderOrcl["TENHANG"].ToString().Trim() : (object)DBNull.Value;
                                                decimal soLuong = 0;
                                                if (dataReaderOrcl["SOLUONG"] != null) decimal.TryParse(dataReaderOrcl["SOLUONG"].ToString(), out soLuong);
                                                cmdInsertSa.Parameters.Add("SOLUONG", SqlDbType.Decimal).Value = soLuong;
                                                decimal tyLeChietKhauLe = 0;
                                                if (dataReaderOrcl["TYLECKLE"] != null) decimal.TryParse(dataReaderOrcl["TYLECKLE"].ToString(), out tyLeChietKhauLe);
                                                cmdInsertSa.Parameters.Add("TYLECKLE", SqlDbType.Decimal).Value = tyLeChietKhauLe;
                                                decimal tyLeChietKhauBuon = 0;
                                                if (dataReaderOrcl["TYLECKBUON"] != null) decimal.TryParse(dataReaderOrcl["TYLECKBUON"].ToString(), out tyLeChietKhauBuon);
                                                cmdInsertSa.Parameters.Add("TYLECKBUON", SqlDbType.Decimal).Value = tyLeChietKhauBuon;
                                                decimal tongLe = 0;
                                                if (dataReaderOrcl["TONGLE"] != null) decimal.TryParse(dataReaderOrcl["TONGLE"].ToString(), out tongLe);
                                                cmdInsertSa.Parameters.Add("TONGLE", SqlDbType.Decimal).Value = tongLe;
                                                decimal donGia = 0;
                                                if (dataReaderOrcl["DONGIA"] != null) decimal.TryParse(dataReaderOrcl["DONGIA"].ToString(), out donGia);
                                                cmdInsertSa.Parameters.Add("DONGIA", SqlDbType.Decimal).Value = donGia;
                                                decimal tongBuon = 0;
                                                if (dataReaderOrcl["TONGBUON"] != null) decimal.TryParse(dataReaderOrcl["TONGBUON"].ToString(), out tongBuon);
                                                cmdInsertSa.Parameters.Add("TONGBUON", SqlDbType.Decimal).Value = tongBuon;
                                                cmdInsertSa.Parameters.Add("GHICHU", SqlDbType.NVarChar, 500).Value = dataReaderOrcl["GHICHU"] != null ? dataReaderOrcl["GHICHU"].ToString().Trim() : (object)DBNull.Value;
                                                int trangThai = 0;
                                                if (dataReaderOrcl["TRANGTHAI"] != null) int.TryParse(dataReaderOrcl["TRANGTHAI"].ToString(), out trangThai);
                                                cmdInsertSa.Parameters.Add("TRANGTHAI", SqlDbType.Int).Value = trangThai;
                                                cmdInsertSa.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Transaction = tranSa;
                                                if (cmdInsertSa.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }


        public static void SYNCHRONIZE_DM_KHACHHANG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID,MAKH,TENKH,DIACHI,DIENTHOAI,CMTND,EMAIL,SODIEM,TONGTIEN,NGAYCAPTHE,NGAYHETHAN,NGAYSINH,UNITCODE FROM DM_KHACHHANG WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdDeleteSa = new SqlCommand();
                                            cmdDeleteSa.Connection = connectionSa;
                                            cmdDeleteSa.CommandText = string.Format(@"DELETE DM_KHACHHANG");
                                            cmdDeleteSa.Transaction = tranSa;
                                            cmdDeleteSa.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                SqlCommand cmdInsertSa = new SqlCommand();
                                                cmdInsertSa.Connection = connectionSa;
                                                cmdInsertSa.CommandText = string.Format(@"INSERT INTO dbo.DM_KHACHHANG(ID,MAKH,TENKH,DIACHI,DIENTHOAI,CMTND,EMAIL,SODIEM,TONGTIEN,NGAYCAPTHE,NGAYHETHAN,NGAYSINH,UNITCODE) VALUES (@ID,@MAKH,@TENKH,@DIACHI,@DIENTHOAI,@CMTND,@EMAIL,@SODIEM,@TONGTIEN,@NGAYCAPTHE,@NGAYHETHAN,@NGAYSINH,@UNITCODE)");
                                                cmdInsertSa.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = dataReaderOrcl["ID"] != null ? dataReaderOrcl["ID"].ToString().Trim() : Guid.NewGuid().ToString();
                                                cmdInsertSa.Parameters.Add("MAKH", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAKH"] != null ? dataReaderOrcl["MAKH"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("TENKH", SqlDbType.NVarChar, 500).Value = dataReaderOrcl["TENKH"] != null ? dataReaderOrcl["TENKH"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("DIACHI", SqlDbType.NVarChar, 300).Value = dataReaderOrcl["DIACHI"] != null ? dataReaderOrcl["DIACHI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("DIENTHOAI", SqlDbType.VarChar, 20).Value = dataReaderOrcl["DIENTHOAI"] != null ? dataReaderOrcl["DIENTHOAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("CMTND", SqlDbType.VarChar, 20).Value = dataReaderOrcl["CMTND"] != null ? dataReaderOrcl["CMTND"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("EMAIL", SqlDbType.VarChar, 100).Value = dataReaderOrcl["EMAIL"] != null ? dataReaderOrcl["EMAIL"].ToString().Trim() : (object)DBNull.Value;
                                                decimal soDiem = 0;
                                                if (dataReaderOrcl["SODIEM"] != null) decimal.TryParse(dataReaderOrcl["SODIEM"].ToString(), out soDiem);
                                                cmdInsertSa.Parameters.Add("SODIEM", SqlDbType.Decimal).Value = soDiem;
                                                decimal tongTien = 0;
                                                if (dataReaderOrcl["TONGTIEN"] != null) decimal.TryParse(dataReaderOrcl["TONGTIEN"].ToString(), out tongTien);
                                                cmdInsertSa.Parameters.Add("TONGTIEN", SqlDbType.Decimal).Value = tongTien;

                                                DateTime ngayCapThe = DateTime.ParseExact("01-JAN-70", "dd-MMM-yy",System.Globalization.CultureInfo.InvariantCulture);
                                                if (dataReaderOrcl["NGAYCAPTHE"] != null) DateTime.TryParse(dataReaderOrcl["NGAYCAPTHE"].ToString(), out ngayCapThe);
                                                cmdInsertSa.Parameters.Add("NGAYCAPTHE", SqlDbType.Date).Value = ngayCapThe;

                                                DateTime ngayHetHan = DateTime.ParseExact("01-JAN-70", "dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture);
                                                if (dataReaderOrcl["NGAYHETHAN"] != null) DateTime.TryParse(dataReaderOrcl["NGAYHETHAN"].ToString(), out ngayHetHan);
                                                cmdInsertSa.Parameters.Add("NGAYHETHAN", SqlDbType.Date).Value = ngayHetHan;

                                                DateTime ngaySinh = DateTime.ParseExact("01-JAN-70", "dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture);
                                                if (dataReaderOrcl["NGAYSINH"] != null) DateTime.TryParse(dataReaderOrcl["NGAYSINH"].ToString(), out ngaySinh);
                                                cmdInsertSa.Parameters.Add("NGAYSINH", SqlDbType.Date).Value = ngaySinh;

                                                cmdInsertSa.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Transaction = tranSa;
                                                if (cmdInsertSa.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }


        public static void SYNCHRONIZE_KHUYENMAI()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT MACHUONGTRINH,TUNGAY,DENNGAY,TUGIO,DENGIO,MAVATTU,SOLUONG,TYLEKHUYENMAICHILDREN AS TYLE,GIATRIKHUYENMAICHILDREN AS GIATRI FROM V_CHUONGTRINH_KHUYENMAI WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "' AND TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdDeleteSa = new SqlCommand();
                                            cmdDeleteSa.Connection = connectionSa;
                                            cmdDeleteSa.CommandText = string.Format(@"DELETE KHUYENMAI");
                                            cmdDeleteSa.Transaction = tranSa;
                                            cmdDeleteSa.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                SqlCommand cmdInsertSa = new SqlCommand();
                                                cmdInsertSa.Connection = connectionSa;
                                                cmdInsertSa.CommandText = string.Format(@"INSERT INTO dbo.KHUYENMAI(ID,MACHUONGTRINH,TUNGAY,DENNGAY,TUGIO,DENGIO,MAVATTU,SOLUONG,TYLE,GIATRI) VALUES (@ID,@MACHUONGTRINH,@TUNGAY,@DENNGAY,@TUGIO,@DENGIO,@MAVATTU,@SOLUONG,@TYLE,@GIATRI)");
                                                cmdInsertSa.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                                                cmdInsertSa.Parameters.Add("MACHUONGTRINH", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MACHUONGTRINH"] != null ? dataReaderOrcl["MACHUONGTRINH"].ToString().Trim() : (object)DBNull.Value;

                                                DateTime tuNgay = DateTime.ParseExact("01-JAN-70", "dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture);
                                                if (dataReaderOrcl["TUNGAY"] != null) DateTime.TryParse(dataReaderOrcl["TUNGAY"].ToString(), out tuNgay);
                                                cmdInsertSa.Parameters.Add("TUNGAY", SqlDbType.Date).Value = tuNgay;

                                                DateTime denNgay = DateTime.ParseExact("01-JAN-70", "dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture);
                                                if (dataReaderOrcl["DENNGAY"] != null) DateTime.TryParse(dataReaderOrcl["DENNGAY"].ToString(), out denNgay);
                                                cmdInsertSa.Parameters.Add("DENNGAY", SqlDbType.Date).Value = denNgay;

                                                cmdInsertSa.Parameters.Add("TUGIO", SqlDbType.VarChar, 50).Value = dataReaderOrcl["TUGIO"] != null ? dataReaderOrcl["TUGIO"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("DENGIO", SqlDbType.VarChar, 50).Value = dataReaderOrcl["DENGIO"] != null ? dataReaderOrcl["DENGIO"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("MAVATTU", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAVATTU"] != null ? dataReaderOrcl["MAVATTU"].ToString().Trim() : (object)DBNull.Value;
                                                decimal soLuong = 0;
                                                if (dataReaderOrcl["SOLUONG"] != null) decimal.TryParse(dataReaderOrcl["SOLUONG"].ToString(), out soLuong);
                                                cmdInsertSa.Parameters.Add("SOLUONG", SqlDbType.Decimal).Value = soLuong;

                                                decimal tyLe = 0;
                                                if (dataReaderOrcl["TYLE"] != null) decimal.TryParse(dataReaderOrcl["TYLE"].ToString(), out tyLe);
                                                cmdInsertSa.Parameters.Add("TYLE", SqlDbType.Decimal).Value = tyLe;

                                                decimal giaTri = 0;
                                                if (dataReaderOrcl["GIATRI"] != null) decimal.TryParse(dataReaderOrcl["GIATRI"].ToString(), out giaTri);
                                                cmdInsertSa.Parameters.Add("GIATRI", SqlDbType.Decimal).Value = giaTri;
                                                cmdInsertSa.Transaction = tranSa;
                                                if (cmdInsertSa.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }

        public static void SYNCHRONIZE_AU_THAMSOHETHONG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID,MA_THAMSO,TEN_THAMSO,GIATRI_THAMSO,UNITCODE FROM AU_THAMSOHETHONG WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdDeleteSa = new SqlCommand();
                                            cmdDeleteSa.Connection = connectionSa;
                                            cmdDeleteSa.CommandText = string.Format(@"DELETE AU_THAMSOHETHONG");
                                            cmdDeleteSa.Transaction = tranSa;
                                            cmdDeleteSa.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                SqlCommand cmdInsertSa = new SqlCommand();
                                                cmdInsertSa.Connection = connectionSa;
                                                cmdInsertSa.CommandText = string.Format(@"INSERT INTO dbo.AU_THAMSOHETHONG(ID,MA_THAMSO,TEN_THAMSO,GIATRI_THAMSO,UNITCODE) VALUES (@ID,@MA_THAMSO,@TEN_THAMSO,@GIATRI_THAMSO,@UNITCODE)");
                                                cmdInsertSa.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = dataReaderOrcl["ID"] != null ? dataReaderOrcl["ID"].ToString().Trim() : Guid.NewGuid().ToString();
                                                cmdInsertSa.Parameters.Add("MA_THAMSO", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MA_THAMSO"] != null ? dataReaderOrcl["MA_THAMSO"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("TEN_THAMSO", SqlDbType.VarChar, 50).Value = dataReaderOrcl["TEN_THAMSO"] != null ? dataReaderOrcl["TEN_THAMSO"].ToString().Trim() : (object)DBNull.Value;
                                                int giaTriThamSo = 0;
                                                if (dataReaderOrcl["GIATRI_THAMSO"] != null) int.TryParse(dataReaderOrcl["GIATRI_THAMSO"].ToString(), out giaTriThamSo);
                                                cmdInsertSa.Parameters.Add("GIATRI_THAMSO", SqlDbType.Int).Value = giaTriThamSo;
                                                cmdInsertSa.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Transaction = tranSa;
                                                if (cmdInsertSa.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }

        public static void SYNCHRONIZE_DM_VATTU()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    string tableName =  FrmXuatBanLeService.GET_TABLE_NAME_NGAYHACHTOAN_CSDL_ORACLE();
                    cmdOrcl.CommandText = string.Format(@"SELECT a.ID,a.MAVATTU,a.TENVATTU,a.MAKHACHHANG AS MANHACUNGCAP,a.DONVITINH,a.BARCODE,a.GIABANBUONVAT,a.GIABANLEVAT,b.GIAVON,b.TONCUOIKYSL,a.TYLEVATRA,a.TYLELAILE,a.ITEMCODE,a.MAVATRA,a.UNITCODE FROM V_VATTU_GIABAN a LEFT JOIN  " + tableName + " b ON a.MAVATTU = b.MAVATTU AND b.MAKHO = '" + Session.Session.CurrentUnitCode +"-K2' WHERE a.MADONVI = '" + Session.Session.CurrentUnitCode + "' AND a.TRANGTHAI = 10");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdDeleteSa = new SqlCommand();
                                            cmdDeleteSa.Connection = connectionSa;
                                            cmdDeleteSa.CommandText = string.Format(@"TRUNCATE TABLE dbo.DM_VATTU");
                                            cmdDeleteSa.Transaction = tranSa;
                                            cmdDeleteSa.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                SqlCommand cmdInsertSa = new SqlCommand();
                                                cmdInsertSa.Connection = connectionSa;
                                                cmdInsertSa.CommandText = string.Format(@"INSERT INTO dbo.DM_VATTU(ID,MAVATTU,TENVATTU,MANHACUNGCAP,DONVITINH,BARCODE,GIABANBUONVAT,GIABANLEVAT,GIAVON,TONCUOIKYSL,TYLEVATRA,TYLELAILE,ITEMCODE,MAVATRA,UNITCODE) VALUES (@ID,@MAVATTU,@TENVATTU,@MANHACUNGCAP,@DONVITINH,@BARCODE,@GIABANBUONVAT,@GIABANLEVAT,@GIAVON,@TONCUOIKYSL,@TYLEVATRA,@TYLELAILE,@ITEMCODE,@MAVATRA,@UNITCODE)");
                                                cmdInsertSa.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = dataReaderOrcl["ID"] != null ? dataReaderOrcl["ID"].ToString().Trim() : Guid.NewGuid().ToString();
                                                cmdInsertSa.Parameters.Add("MAVATTU", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAVATTU"] != null ? dataReaderOrcl["MAVATTU"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("TENVATTU", SqlDbType.NVarChar, 200).Value = dataReaderOrcl["TENVATTU"] != null ? dataReaderOrcl["TENVATTU"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("MANHACUNGCAP", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MANHACUNGCAP"] != null ? dataReaderOrcl["MANHACUNGCAP"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("DONVITINH", SqlDbType.VarChar, 50).Value = dataReaderOrcl["DONVITINH"] != null ? dataReaderOrcl["DONVITINH"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("BARCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["BARCODE"] != null ? dataReaderOrcl["BARCODE"].ToString().Trim() : (object)DBNull.Value;
                                                decimal giaBanBuonVat = 0;
                                                if (dataReaderOrcl["GIABANBUONVAT"] != null) decimal.TryParse(dataReaderOrcl["GIABANBUONVAT"].ToString(), out giaBanBuonVat);
                                                cmdInsertSa.Parameters.Add("GIABANBUONVAT", SqlDbType.Decimal).Value = giaBanBuonVat;
                                                decimal giaBanLeVat = 0;
                                                if (dataReaderOrcl["GIABANLEVAT"] != null) decimal.TryParse(dataReaderOrcl["GIABANLEVAT"].ToString(), out giaBanLeVat);
                                                cmdInsertSa.Parameters.Add("GIABANLEVAT", SqlDbType.Decimal).Value = giaBanLeVat;
                                                decimal giaVon = 0;
                                                if (dataReaderOrcl["GIAVON"] != null) decimal.TryParse(dataReaderOrcl["GIAVON"].ToString(), out giaVon);
                                                cmdInsertSa.Parameters.Add("GIAVON", SqlDbType.Decimal).Value = giaVon;
                                                decimal tonCuoiKySoLuong = 0;
                                                if (dataReaderOrcl["TONCUOIKYSL"] != null) decimal.TryParse(dataReaderOrcl["TONCUOIKYSL"].ToString(), out tonCuoiKySoLuong);
                                                cmdInsertSa.Parameters.Add("TONCUOIKYSL", SqlDbType.Decimal).Value = tonCuoiKySoLuong;
                                                decimal tyLeVatRa = 0;
                                                if (dataReaderOrcl["TYLEVATRA"] != null) decimal.TryParse(dataReaderOrcl["TYLEVATRA"].ToString(), out tyLeVatRa);
                                                cmdInsertSa.Parameters.Add("TYLEVATRA", SqlDbType.Decimal).Value = tyLeVatRa;
                                                decimal tyLeLaiLe = 0;
                                                if (dataReaderOrcl["TYLELAILE"] != null) decimal.TryParse(dataReaderOrcl["TYLELAILE"].ToString(), out tyLeLaiLe);
                                                cmdInsertSa.Parameters.Add("TYLELAILE", SqlDbType.Decimal).Value = tyLeLaiLe;
                                                cmdInsertSa.Parameters.Add("ITEMCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["ITEMCODE"] != null ? dataReaderOrcl["ITEMCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("MAVATRA", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAVATRA"] != null ? dataReaderOrcl["MAVATRA"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value =
                                                    dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Transaction = tranSa;
                                                if (cmdInsertSa.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }

        public static void SYNCHRONIZE_AU_DONVI()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID,MADONVI,MADONVICHA,TENDONVI,SODIENTHOAI,DIACHI,TRANGTHAI,MACUAHANG,TENCUAHANG,UNITCODE FROM AU_DONVI WHERE MADONVI = '" +Session.Session.CurrentUnitCode+ "'");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdDeleteSa = new SqlCommand();
                                            cmdDeleteSa.Connection = connectionSa;
                                            cmdDeleteSa.CommandText = string.Format(@"TRUNCATE TABLE dbo.AU_DONVI");
                                            cmdDeleteSa.Transaction = tranSa;
                                            cmdDeleteSa.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                SqlCommand cmdInsertSa = new SqlCommand();
                                                cmdInsertSa.Connection = connectionSa;
                                                cmdInsertSa.CommandText = string.Format(@"INSERT INTO dbo.AU_DONVI(ID,MADONVI,MADONVICHA,TENDONVI,SODIENTHOAI,DIACHI,TRANGTHAI,MACUAHANG,TENCUAHANG,UNITCODE) VALUES (@ID,@MADONVI,@MADONVICHA,@TENDONVI,@SODIENTHOAI,@DIACHI,@TRANGTHAI,@MACUAHANG,@TENCUAHANG,@UNITCODE)");
                                                cmdInsertSa.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = dataReaderOrcl["ID"] != null ? dataReaderOrcl["ID"].ToString().Trim() : Guid.NewGuid().ToString();
                                                cmdInsertSa.Parameters.Add("MADONVI", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MADONVI"] != null ? dataReaderOrcl["MADONVI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("MADONVICHA", SqlDbType.NVarChar, 50).Value = dataReaderOrcl["MADONVICHA"] != null ? dataReaderOrcl["MADONVICHA"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("TENDONVI", SqlDbType.NVarChar, 150).Value = dataReaderOrcl["TENDONVI"] != null ? dataReaderOrcl["TENDONVI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("SODIENTHOAI", SqlDbType.VarChar, 50).Value = dataReaderOrcl["SODIENTHOAI"] != null ? dataReaderOrcl["SODIENTHOAI"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("DIACHI", SqlDbType.NVarChar, 200).Value = dataReaderOrcl["DIACHI"] != null ? dataReaderOrcl["DIACHI"].ToString().Trim() : (object)DBNull.Value;
                                                int trangThai = 0;
                                                if (dataReaderOrcl["TRANGTHAI"] != null) int.TryParse(dataReaderOrcl["TRANGTHAI"].ToString(), out trangThai);
                                                cmdInsertSa.Parameters.Add("TRANGTHAI", SqlDbType.Decimal).Value = trangThai;
                                                cmdInsertSa.Parameters.Add("MACUAHANG", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MACUAHANG"] != null ? dataReaderOrcl["MACUAHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("TENCUAHANG", SqlDbType.NVarChar, 200).Value = dataReaderOrcl["TENCUAHANG"] != null ? dataReaderOrcl["TENCUAHANG"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Transaction = tranSa;
                                                if (cmdInsertSa.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }}


        public static void SYNCHRONIZE_DM_HANGKHACHHANG()
        {
            using (OracleConnection connectionOrcl = new OracleConnection(ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ConnectionString))
            {
                connectionOrcl.Open();
                if (connectionOrcl.State == ConnectionState.Open)
                {
                    OracleCommand cmdOrcl = new OracleCommand();
                    cmdOrcl.Connection = connectionOrcl;
                    cmdOrcl.CommandText = string.Format(@"SELECT ID,MAHANGKH,TENHANGKH,SOTIEN,TYLEGIAMGIASN,TYLEGIAMGIA,TRANGTHAI,I_CREATE_DATE,I_CREATE_BY,I_UPDATE_DATE,I_UPDATE_BY,I_STATE,UNITCODE,QUYDOITIEN_THANH_DIEM,QUYDOIDIEM_THANH_TIEN,HANG_KHOIDAU FROM DM_HANGKHACHHANG WHERE UNITCODE = '" + Session.Session.CurrentUnitCode + "'");
                    OracleDataReader dataReaderOrcl = cmdOrcl.ExecuteReader();
                    if (dataReaderOrcl.HasRows)
                    {
                        try
                        {
                            using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                            {
                                connectionSa.Open();
                                if (connectionSa.State == ConnectionState.Open)
                                {
                                    using (SqlTransaction tranSa = connectionSa.BeginTransaction())
                                    {
                                        try
                                        {
                                            SqlCommand cmdDeleteSa = new SqlCommand();
                                            cmdDeleteSa.Connection = connectionSa;
                                            cmdDeleteSa.CommandText = string.Format(@"TRUNCATE TABLE dbo.DM_HANGKHACHHANG");
                                            cmdDeleteSa.Transaction = tranSa;
                                            cmdDeleteSa.ExecuteNonQuery();
                                            int countInsert = 0;
                                            while (dataReaderOrcl.Read())
                                            {
                                                int trangThai = 0;
                                                decimal SOTIEN = 0;
                                                decimal TYLEGIAMGIASN = 0;
                                                decimal TYLEGIAMGIA = 0;
                                                decimal QUYDOITIEN_THANH_DIEM = 0;
                                                decimal QUYDOIDIEM_THANH_TIEN = 0;
                                                DateTime I_CREATE_DATE = DateTime.Now;
                                                DateTime I_UPDATE_DATE = DateTime.Now;
                                                decimal HANG_KHOIDAU = 0;
                                                SqlCommand cmdInsertSa = new SqlCommand();
                                                cmdInsertSa.Connection = connectionSa;
                                                cmdInsertSa.CommandText = string.Format(@"INSERT INTO dbo.DM_HANGKHACHHANG(ID,MAHANGKH,TENHANGKH,SOTIEN,TYLEGIAMGIASN,TYLEGIAMGIA,TRANGTHAI,I_CREATE_DATE,I_CREATE_BY,I_UPDATE_DATE,I_UPDATE_BY,I_STATE,UNITCODE,QUYDOITIEN_THANH_DIEM,QUYDOIDIEM_THANH_TIEN,HANG_KHOIDAU) VALUES (@ID,@MAHANGKH,@TENHANGKH,@SOTIEN,@TYLEGIAMGIASN,@TYLEGIAMGIA,@TRANGTHAI,@I_CREATE_DATE,@I_CREATE_BY,@I_UPDATE_DATE,@I_UPDATE_BY,@I_STATE,@UNITCODE,@QUYDOITIEN_THANH_DIEM,@QUYDOIDIEM_THANH_TIEN,@HANG_KHOIDAU)");
                                                cmdInsertSa.Parameters.Add("ID", SqlDbType.VarChar, 50).Value = dataReaderOrcl["ID"] != null ? dataReaderOrcl["ID"].ToString().Trim() : Guid.NewGuid().ToString();
                                                cmdInsertSa.Parameters.Add("MAHANGKH", SqlDbType.VarChar, 50).Value = dataReaderOrcl["MAHANGKH"] != null ? dataReaderOrcl["MAHANGKH"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("TENHANGKH", SqlDbType.NVarChar, 200).Value = dataReaderOrcl["TENHANGKH"] != null ? dataReaderOrcl["TENHANGKH"].ToString().Trim() : (object)DBNull.Value;
                                                decimal.TryParse(dataReaderOrcl["SOTIEN"] != null ? dataReaderOrcl["SOTIEN"].ToString().Trim() : "", out SOTIEN);
                                                decimal.TryParse(dataReaderOrcl["TYLEGIAMGIASN"] != null ? dataReaderOrcl["TYLEGIAMGIASN"].ToString().Trim() : "", out TYLEGIAMGIASN);
                                                decimal.TryParse(dataReaderOrcl["TYLEGIAMGIA"] != null ? dataReaderOrcl["TYLEGIAMGIA"].ToString().Trim() : "", out TYLEGIAMGIA);
                                                cmdInsertSa.Parameters.Add("SOTIEN", SqlDbType.Decimal).Value = SOTIEN;
                                                cmdInsertSa.Parameters.Add("TYLEGIAMGIASN", SqlDbType.VarChar, 50).Value = TYLEGIAMGIASN;
                                                cmdInsertSa.Parameters.Add("TYLEGIAMGIA", SqlDbType.NVarChar, 200).Value = TYLEGIAMGIA;
                                                if (dataReaderOrcl["TRANGTHAI"] != null) int.TryParse(dataReaderOrcl["TRANGTHAI"].ToString(), out trangThai);
                                                cmdInsertSa.Parameters.Add("TRANGTHAI", SqlDbType.Decimal).Value = trangThai;
                                                DateTime.TryParse(dataReaderOrcl["I_CREATE_DATE"] != null ? dataReaderOrcl["I_CREATE_DATE"].ToString().Trim() : "", out I_CREATE_DATE);
                                                cmdInsertSa.Parameters.Add("I_CREATE_DATE", SqlDbType.Date).Value = I_CREATE_DATE;
                                                cmdInsertSa.Parameters.Add("I_CREATE_BY", SqlDbType.NVarChar, 50).Value = dataReaderOrcl["I_CREATE_BY"] != null ? dataReaderOrcl["I_CREATE_BY"].ToString().Trim() : (object)DBNull.Value;
                                                DateTime.TryParse(dataReaderOrcl["I_UPDATE_DATE"] != null ? dataReaderOrcl["I_UPDATE_DATE"].ToString().Trim() : "", out I_UPDATE_DATE);
                                                cmdInsertSa.Parameters.Add("I_UPDATE_DATE", SqlDbType.Date).Value = I_UPDATE_DATE;
                                                cmdInsertSa.Parameters.Add("I_UPDATE_BY", SqlDbType.NVarChar, 50).Value = dataReaderOrcl["I_UPDATE_BY"] != null ? dataReaderOrcl["I_UPDATE_BY"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("I_STATE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["I_STATE"] != null ? dataReaderOrcl["I_STATE"].ToString().Trim() : (object)DBNull.Value;
                                                cmdInsertSa.Parameters.Add("UNITCODE", SqlDbType.VarChar, 50).Value = dataReaderOrcl["UNITCODE"] != null ? dataReaderOrcl["UNITCODE"].ToString().Trim() : (object)DBNull.Value;
                                                decimal.TryParse(dataReaderOrcl["QUYDOITIEN_THANH_DIEM"] != null ? dataReaderOrcl["QUYDOITIEN_THANH_DIEM"].ToString().Trim() : "", out QUYDOITIEN_THANH_DIEM);
                                                cmdInsertSa.Parameters.Add("QUYDOITIEN_THANH_DIEM", SqlDbType.VarChar).Value = QUYDOITIEN_THANH_DIEM;
                                                decimal.TryParse(dataReaderOrcl["QUYDOIDIEM_THANH_TIEN"] != null ? dataReaderOrcl["QUYDOIDIEM_THANH_TIEN"].ToString().Trim() : "", out QUYDOIDIEM_THANH_TIEN);
                                                cmdInsertSa.Parameters.Add("QUYDOIDIEM_THANH_TIEN", SqlDbType.Decimal).Value = QUYDOIDIEM_THANH_TIEN;
                                                decimal.TryParse(dataReaderOrcl["HANG_KHOIDAU"] != null ? dataReaderOrcl["HANG_KHOIDAU"].ToString().Trim() : "", out HANG_KHOIDAU);
                                                cmdInsertSa.Parameters.Add("HANG_KHOIDAU", SqlDbType.Decimal).Value = HANG_KHOIDAU;
                                                cmdInsertSa.Transaction = tranSa;
                                                if (cmdInsertSa.ExecuteNonQuery() > 0)
                                                {
                                                    countInsert++;
                                                }
                                            }
                                            if (countInsert > 0)
                                            {
                                                tranSa.Commit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            tranSa.Rollback();
                                            WriteLogs.LogError(ex);
                                        }
                                        finally
                                        {
                                            connectionSa.Close();
                                            connectionSa.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            connectionOrcl.Close();
                            WriteLogs.LogError(ex);
                        }
                    }
                    //Mở thì đóng
                    connectionOrcl.Close();
                }
            }
        }
    }
}

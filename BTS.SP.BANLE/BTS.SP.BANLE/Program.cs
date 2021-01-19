using BTS.SP.BANLE.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
namespace BTS.SP.BANLE
{
    static class Program
    {
        private static string DIRECTORY_PATH = @"C:\Temp";
        private static string FILE_PATH = DIRECTORY_PATH + @"\SERVER.txt";
        private static List<DummyData> ListDummyData;
        private static string local = ConfigurationSettings.AppSettings["LOCAL_SQL"];
        private static string DIRECTORY_SQL = @"C:\SQLSERVER\BANLE\DATA";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string msg = CheckDatabaseExists(out bool result);
            if (msg.Length > 0) { MessageBox.Show(msg); return; }

            InitializeConnectDatabase();

            if (result) Application.Run(new FrmLogin());
            else
            {
                DialogResult dialogResult = MessageBox.Show("KHÔNG TÌM THẤY CƠ SỞ DỮ LIỆU SQL SERVER TẠI MÁY BÁN LẺ, BẠN CÓ MUỐN TẠO CSDL TRƯỚC KHI SỬ DỤNG", "TẠO CƠ SỞ DỮ LIỆU", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    msg = CreateDatabase_SQLSERVER();
                    if (msg.Length > 0) { MessageBox.Show(msg); return; }

                    msg = CreateTable_SQLSERVER();
                    if (msg.Length > 0) { MessageBox.Show(msg); return; }

                    MessageBox.Show("TẠO CƠ SỞ DỮ LIỆU SQL SERVER THÀNH CÔNG");
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            
        }

        private static string CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(DIRECTORY_PATH))
                {
                    Directory.CreateDirectory(DIRECTORY_PATH);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        private static string CreateFileIfNotExists()
        {
            try
            {
                if (!File.Exists(FILE_PATH)) // If file does not exists
                {
                    File.Create(FILE_PATH).Close(); // Create file
                    using (StreamWriter sw = File.AppendText(FILE_PATH))
                    {
                        sw.WriteLine(0); // Write text to .txt file
                        Session.Session.SESSION_ID_CSDL = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        private static string ReadContentTextFile()
        {
            try
            {
                string[] lines = File.ReadAllLines(FILE_PATH);
                foreach (string line in lines)
                {
                    int value = 0;
                    int.TryParse(line, out value);
                    Session.Session.SESSION_ID_CSDL = value;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        public static string InitializeConnectDatabase()
        {
            ListDummyData = new List<DummyData>()
            {
                new DummyData()
                {
                    Id = 1,
                    Name = "CƠ SỞ DỮ LIỆU ORACLE",
                },
                new DummyData()
                {
                    Id = 2,
                    Name = "CƠ SỞ DỮ LIỆU SQL SERVER",
                }
            };

            string msg = CreateDirectory();
            if (msg.Length > 0) return msg;

            msg = CreateFileIfNotExists();
            if (msg.Length > 0) return msg;

            msg = ReadContentTextFile();
            if (msg.Length > 0) return msg;

            return "";
        }

        private static string CheckDatabaseExists(out bool result)
        {
            string sqlCreateDBQuery;
            result = false;

            try
            {
                SqlConnection tmpConn = new SqlConnection($"server={local};Trusted_Connection=yes");
                sqlCreateDBQuery = string.Format(@"SELECT database_id FROM sys.databases WHERE Name = 'TBNETERP'");

                using (tmpConn)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();
                        object resultObj = sqlCmd.ExecuteScalar();
                        int databaseID = 0;
                        if (resultObj != null) int.TryParse(resultObj.ToString(), out databaseID);

                        tmpConn.Close();
                        result = (databaseID > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";
        }

        private static string CreateDatabase_SQLSERVER()
        {
            string sqlCreateDBQuery;
            try
            {
                SqlConnection tmpConn = new SqlConnection($"server={local};Trusted_Connection=yes");

                using (tmpConn)
                {
                    sqlCreateDBQuery = string.Format(@"
                            CREATE DATABASE TBNETERP  
                            ON   
                            (NAME = TBNETERP_dat,  
                                FILENAME = '{0}\mdf',  
                                SIZE = 10,  
                                MAXSIZE = 50,  
                                FILEGROWTH = 5)  
                            LOG ON  
                            (NAME = TBNETERP_log,  
                                FILENAME = '{0}\ldf',  
                                SIZE = 5MB,  
                                MAXSIZE = 25MB,  
                                FILEGROWTH = 5MB);", DIRECTORY_SQL);

                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();
                        try
                        {
                            if (!Directory.Exists(DIRECTORY_SQL))
                            {
                                Directory.CreateDirectory(DIRECTORY_SQL);
                            }
                            sqlCmd.ExecuteNonQuery();
                            tmpConn.Close();
                        }
                        catch (SqlException ex)
                        {
                            tmpConn.Dispose();
                            return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        private static string CreateTable_SQLSERVER()
        {
            string sqlCreateDBQuery;
            try
            {
                SqlConnection tmpConn = new SqlConnection($"server={local};Database=TBNETERP;Trusted_Connection=yes");
                using (tmpConn)
                {
                    sqlCreateDBQuery = string.Format(@"CREATE TABLE DM_HANGKHACHHANG 
                                                        (
                                                        ID VARCHAR(50) , 
	                                                    MAHANGKH VARCHAR(50) , 
	                                                    TENHANGKH NVARCHAR(200), 
	                                                    SOTIEN decimal(18,2), 
	                                                    TYLEGIAMGIASN decimal(18,2), 
	                                                    TYLEGIAMGIA decimal(18,2), 
	                                                    TRANGTHAI INT, 
	                                                    I_CREATE_DATE DATE, 
	                                                    I_CREATE_BY VARCHAR(50), 
	                                                    I_UPDATE_DATE DATE, 
	                                                    I_UPDATE_BY VARCHAR(50), 
	                                                    I_STATE VARCHAR(50), 
	                                                    UNITCODE VARCHAR(50), 
	                                                    QUYDOITIEN_THANH_DIEM decimal(18,2), 
	                                                    QUYDOIDIEM_THANH_TIEN decimal(18,2),
	                                                    HANG_KHOIDAU decimal(18,2), 
	                                                    PRIMARY KEY (ID)
                                                    );
                                                    CREATE TABLE AU_DONVI(
	                                                    ID VARCHAR(50),
                                                        MADONVI VARCHAR(50),
                                                        MADONVICHA VARCHAR(50),
                                                        TENDONVI NVARCHAR(50),
                                                        SODIENTHOAI VARCHAR(50),
                                                        DIACHI VARCHAR(50),
	                                                    TRANGTHAI INT,
	                                                    MACUAHANG VARCHAR(50),
	                                                    TENCUAHANG VARCHAR(50),
	                                                    UNITCODE VARCHAR(50),
	                                                    PRIMARY KEY (ID)
                                                    );
                                                    CREATE TABLE AU_NGUOIDUNG(
	                                                    ID VARCHAR(50),
                                                        USERNAME VARCHAR(50),
                                                        PASSWORD VARCHAR(50),
                                                        MANHANVIEN VARCHAR(50),
                                                        TENNHANVIEN NVARCHAR(200),
                                                        SODIENTHOAI VARCHAR(20),
	                                                    SOCHUNGMINHTHU VARCHAR(20),
	                                                    GIOITINH INT,
	                                                    TRANGTHAI INT,
	                                                    LEVEL INT,
	                                                    UNITCODE VARCHAR(50),
	                                                    PARENT_UNITCODE VARCHAR(50),
	                                                    PRIMARY KEY (ID)
                                                    );
                                                    CREATE TABLE AU_THAMSOHETHONG(
	                                                    ID VARCHAR(50),
                                                        MA_THAMSO VARCHAR(50),
                                                        TEN_THAMSO NVARCHAR(500),
                                                        GIATRI_THAMSO INT,
	                                                    UNITCODE VARCHAR(50),
	                                                    PRIMARY KEY (ID)
                                                    );
                                                    CREATE TABLE DM_BOHANG(
	                                                    ID VARCHAR(50),
                                                        MABOHANG VARCHAR(50),
                                                        TENBOHANG NVARCHAR(300),
                                                        GHICHU NVARCHAR(500),
	                                                    TRANGTHAI INT,
	                                                    UNITCODE VARCHAR(50),
	                                                    PRIMARY KEY (ID)
                                                    );
                                                    CREATE TABLE DM_BOHANGCHITIET(
	                                                    ID VARCHAR(50),
                                                        MABOHANG VARCHAR(50),
                                                        MAHANG VARCHAR(50),
                                                        TENHANG NVARCHAR(500),
	                                                    SOLUONG DECIMAL,
	                                                    TYLECKLE DECIMAL,
	                                                    TYLECKBUON DECIMAL,
	                                                    TONGLE DECIMAL,
	                                                    DONGIA DECIMAL,
	                                                    TONGBUON DECIMAL,
	                                                    GHICHU NVARCHAR(500),
	                                                    TRANGTHAI INT,
	                                                    UNITCODE VARCHAR(50),
	                                                    PRIMARY KEY (ID)
                                                    );
                                                    CREATE TABLE DM_KHACHHANG(
	                                                    ID VARCHAR(50),
                                                        MAKH VARCHAR(50),
                                                        TENKH NVARCHAR(500),
	                                                    DIACHI NVARCHAR(300),
	                                                    DIENTHOAI VARCHAR(20),
	                                                    CMTND VARCHAR(20),
	                                                    EMAIL VARCHAR(100),
	                                                    SODIEM DECIMAL,
	                                                    TONGTIEN DECIMAL,
	                                                    NGAYCAPTHE DATE,
	                                                    NGAYHETHAN DATE,
	                                                    NGAYSINH DATE,
	                                                    UNITCODE VARCHAR(50),
	                                                    HANGKHACHHANG NVARCHAR(50),
	                                                    HANGKHACHHANGCU NVARCHAR(50),
	                                                    PRIMARY KEY (ID)
                                                    );
                                                    CREATE TABLE DM_VATTU(
	                                                    ID VARCHAR(50),
                                                        MAVATTU VARCHAR(50),
                                                        TENVATTU NVARCHAR(500),
	                                                    MANHACUNGCAP NVARCHAR(50),
	                                                    DONVITINH VARCHAR(20),
	                                                    BARCODE VARCHAR(2000),
	                                                    GIABANBUONVAT VARCHAR(100),
	                                                    GIABANLEVAT DECIMAL,
	                                                    GIAVON DECIMAL,
	                                                    TONCUOIKYSL DECIMAL,
	                                                    TYLEVATRA DECIMAL,
	                                                    TYLELAILE DECIMAL,
	                                                    ITEMCODE VARCHAR(50),
	                                                    MAVATRA VARCHAR(20),
	                                                    UNITCODE VARCHAR(50),
	                                                    PRIMARY KEY (ID)
                                                    );
                                                    CREATE TABLE KHUYENMAI(
	                                                    ID VARCHAR(50),
                                                        MACHUONGTRINH VARCHAR(50),
                                                        TUNGAY DATE,
	                                                    DENNGAY DATE,
	                                                    TUGIO VARCHAR(50),
	                                                    DENGIO VARCHAR(50),
	                                                    MAVATTU VARCHAR(50),
	                                                    SOLUONG DECIMAL,
	                                                    TYLE DECIMAL,
	                                                    GIATRI DECIMAL,
	                                                    PRIMARY KEY (ID)
                                                    );
                                                    CREATE TABLE NVGDQUAY_ASYNCCLIENT(
	                                                    ID VARCHAR(50),
                                                        MAGIAODICH VARCHAR(50),
                                                        MAGIAODICHQUAYPK VARCHAR(50),
	                                                    MADONVI VARCHAR(50),
	                                                    NGAYTAO DATE,
	                                                    NGAYPHATSINH DATE,
	                                                    MANGUOITAO VARCHAR(50),
	                                                    NGUOITAO NVARCHAR(50),
	                                                    MAQUAYBAN VARCHAR(50),
	                                                    LOAIGIAODICH INT,
	                                                    HINHTHUCTHANHTOAN VARCHAR(50),
	                                                    TIENKHACHDUA DECIMAL,
	                                                    TIENVOUCHER DECIMAL,
	                                                    TIENTHEVIP DECIMAL,
	                                                    TIENTRALAI DECIMAL,
	                                                    TIENTHE DECIMAL,
	                                                    TIENCOD DECIMAL,
	                                                    TIENMAT DECIMAL,
	                                                    TTIENCOVAT DECIMAL,
	                                                    THOIGIAN VARCHAR(50),
	                                                    MAKHACHHANG VARCHAR(50),
	                                                    UNITCODE VARCHAR(50),
	                                                    PRIMARY KEY (ID)
                                                    );
                                                    CREATE TABLE NVHANGGDQUAY_ASYNCCLIENT(
	                                                    ID VARCHAR(50),
                                                        MAGDQUAYPK VARCHAR(50),
                                                        MAKHOHANG VARCHAR(50),
	                                                    MADONVI VARCHAR(50),
	                                                    MAVATTU VARCHAR(50),
	                                                    MANGUOITAO VARCHAR(50),
	                                                    NGUOITAO NVARCHAR(50),
	                                                    MABOPK VARCHAR(50),
	                                                    NGAYTAO DATE,
	                                                    NGAYPHATSINH DATE,
	                                                    SOLUONG DECIMAL,
	                                                    TTIENCOVAT DECIMAL,
	                                                    GIABANLECOVAT DECIMAL,
	                                                    TYLECHIETKHAU DECIMAL,
	                                                    TIENCHIETKHAU DECIMAL,
	                                                    TYLEKHUYENMAI DECIMAL,
	                                                    TIENKHUYENMAI DECIMAL,
	                                                    TYLEVOUCHER DECIMAL,
	                                                    TIENVOUCHER DECIMAL,
	                                                    TYLELAILE DECIMAL,
	                                                    GIAVON DECIMAL,
	                                                    MAVAT VARCHAR(50),
	                                                    VATBAN DECIMAL,
	                                                    MACHUONGTRINHKM VARCHAR(50),
	                                                    UNITCODE VARCHAR(50),
	                                                    PRIMARY KEY (ID)
                                                    );");
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn))
                    {
                        tmpConn.Open();
                        try
                        {
                            sqlCmd.ExecuteNonQuery();
                            tmpConn.Close();
                        }
                        catch (SqlException ex)
                        {
                            tmpConn.Dispose();
                            return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }
    }
}

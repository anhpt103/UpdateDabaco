using BTS.SP.BANLE.Common;
using BTS.SP.BANLE.ConnectDatabase;
using BTS.SP.BANLE.Giaodich.XuatBanLe;
using BTS.SP.BANLE.Hethong;
using BTS.SP.BANLE.Utils;
using DevExpress.XtraSplashScreen;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace BTS.SP.BANLE
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            txtNgayPhatSinh.Text = Environment.MachineName;
        }

        private void LOGIN()
        {
            string Username = txtUserName.Text.Trim();
            string passWord = txtPassWord.Text.Trim();
            string passMd5 = MD5Encrypt.MD5Hash(passWord).Trim();
            //check true false login
            try
            {
                string msg = Config.CheckConnectToServer(out bool result);
                if (msg.Length > 0) { MessageBox.Show(msg); return; }

                //BEGIN LOGIN
                if (result) // nếu có mạng lan
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
                                cmd.CommandText = string.Format(@"SELECT * FROM AU_NGUOIDUNG WHERE USERNAME = '" + Username + "' AND PASSWORD = '" + passMd5 + "'");
                                OracleDataReader dataReader = null;
                                dataReader = cmd.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        Session.Session.CurrentMaNhanVien = dataReader["MANHANVIEN"].ToString();
                                        Session.Session.CurrentTenNhanVien = dataReader["TENNHANVIEN"].ToString();
                                        Session.Session.CurrentUnitCode = dataReader["UNITCODE"].ToString();
                                        if (!string.IsNullOrEmpty(Session.Session.CurrentUnitCode))
                                        {
                                            Session.Session.CurrentCodeStore = Session.Session.CurrentUnitCode.Split('-')[1];
                                        }
                                        OracleCommand command = new OracleCommand();
                                        command.Connection = connection;
                                        command.CommandText = string.Format(@"SELECT * FROM AU_DONVI WHERE MADONVI = '" + Session.Session.CurrentUnitCode + "'");
                                        OracleDataReader dataReaderDonVi = null;
                                        dataReaderDonVi = command.ExecuteReader();
                                        if (dataReaderDonVi.HasRows)
                                        {
                                            while (dataReaderDonVi.Read())
                                            {
                                                Session.Session.CurrentPhone = dataReaderDonVi["SODIENTHOAI"].ToString();
                                                Session.Session.CurrentAddress = dataReaderDonVi["DIACHI"].ToString();
                                                Session.Session.CurrentNameStore = dataReaderDonVi["TENCUAHANG"].ToString();
                                            }
                                        }
                                        Session.Session.CurrentUserName = dataReader["USERNAME"].ToString();
                                        Session.Session.CurrentNgayPhatSinh = FrmXuatBanLeService.GET_NGAYHACHTOAN_CSDL_ORACLE();
                                        Session.Session.CurrentTableNamePeriod = FrmXuatBanLeService.GET_TABLE_NAME_NGAYHACHTOAN_CSDL_ORACLE();
                                        Session.Session.CurrentWareHouse = (Session.Session.CurrentUnitCode + "-K2").ToUpper().Trim();
                                        SplashScreenManager.ShowForm(typeof(WaitForm1));
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_AU_NGUOIDUNG();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_DM_BOHANG();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_DM_BOHANGCHITIET();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_DM_KHACHHANG();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_KHUYENMAI();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_AU_THAMSOHETHONG();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_AU_DONVI();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_DM_VATTU();
                                        SYNCHRONIZE_DATA.SYNCHRONIZE_DM_HANGKHACHHANG();
                                        SplashScreenManager.CloseForm();
                                        FrmMain frmMain = new FrmMain();
                                        frmMain.ShowDialog();
                                        break;
                                    }
                                }
                                else
                                {
                                    NotificationLauncher.ShowNotificationWarning("Thông báo", "Thông tin đăng nhập không đúng", 1, "0x1", "0x8", "normal");
                                    txtPassWord.Text = "";
                                    txtPassWord.Focus();
                                }
                            }
                            catch (Exception ex)
                            {
                                NotificationLauncher.ShowNotificationError("Thông báo", "Không có kết nối với cơ sở dữ liệu máy chủ", 1, "0x1", "0x8", "normal");
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
                    //Kết nối với SQL
                    using (SqlConnection connectionSa = new SqlConnection(ConfigurationManager.ConnectionStrings["TBNETERP_CLIENT"].ConnectionString))
                    {
                        connectionSa.Open();
                        if (connectionSa.State == ConnectionState.Open)
                        {
                            try
                            {
                                SqlCommand cmdSelectSa = new SqlCommand();
                                cmdSelectSa.Connection = connectionSa;
                                cmdSelectSa.CommandText = string.Format(@"SELECT [ID],[USERNAME],[PASSWORD],[MANHANVIEN],[TENNHANVIEN],[SODIENTHOAI],[SOCHUNGMINHTHU],[GIOITINH],[TRANGTHAI],[LEVEL],[UNITCODE],[PARENT_UNITCODE] FROM [dbo].[AU_NGUOIDUNG] WHERE USERNAME = '" + Username + "' AND PASSWORD = '" + passMd5 + "'");
                                SqlDataReader dataReader = null;
                                dataReader = cmdSelectSa.ExecuteReader();
                                if (dataReader.HasRows)
                                {
                                    while (dataReader.Read())
                                    {
                                        Session.Session.CurrentMaNhanVien = dataReader["MANHANVIEN"].ToString();
                                        Session.Session.CurrentTenNhanVien = dataReader["TENNHANVIEN"].ToString();
                                        Session.Session.CurrentUnitCode = dataReader["UNITCODE"].ToString();
                                        if (!string.IsNullOrEmpty(Session.Session.CurrentUnitCode))
                                        {
                                            Session.Session.CurrentCodeStore = Session.Session.CurrentUnitCode.Split('-')[1];
                                        }
                                        SqlCommand commandDonVi = new SqlCommand();
                                        commandDonVi.Connection = connectionSa;
                                        commandDonVi.CommandText = string.Format(@"SELECT [ID],[MADONVI],[MADONVICHA],[TENDONVI],[SODIENTHOAI],[DIACHI],[TRANGTHAI],[MACUAHANG],[TENCUAHANG],[UNITCODE] FROM [dbo].[AU_DONVI] WHERE MADONVI = '" + Session.Session.CurrentUnitCode + "'");
                                        SqlDataReader dataReaderDonVi = null;
                                        dataReaderDonVi = commandDonVi.ExecuteReader();
                                        if (dataReaderDonVi.HasRows)
                                        {
                                            while (dataReaderDonVi.Read())
                                            {
                                                Session.Session.CurrentPhone = dataReaderDonVi["SODIENTHOAI"].ToString();
                                                Session.Session.CurrentAddress = dataReaderDonVi["DIACHI"].ToString();
                                                Session.Session.CurrentNameStore = dataReaderDonVi["TENCUAHANG"].ToString();
                                            }
                                        }
                                        Session.Session.CurrentUserName = dataReader["USERNAME"].ToString();
                                        //nếu mất mạng thì ngày phát sinh là ngày hiện tại
                                        Session.Session.CurrentNgayPhatSinh = DateTime.Now;
                                        Session.Session.CurrentWareHouse = (Session.Session.CurrentUnitCode + "-K2").ToUpper().Trim();
                                        FrmMain frmMain = new FrmMain();
                                        frmMain.ShowDialog();
                                        break;
                                    }
                                }
                                else
                                {
                                    NotificationLauncher.ShowNotificationWarning("Thông báo", "Thông tin đăng nhập không đúng", 1, "0x1", "0x8", "normal");
                                    txtPassWord.Text = "";
                                    txtPassWord.Focus();
                                }
                            }
                            catch (Exception ex)
                            {
                                NotificationLauncher.ShowNotificationError("Thông báo", "Không có kế nối với cơ sở dữ liệu máy bán", 1, "0x1", "0x8", "normal");
                                WriteLogs.LogError(ex);
                            }
                            finally
                            {
                                connectionSa.Close();
                                connectionSa.Dispose();
                            }
                        }
                        else
                        {
                            NotificationLauncher.ShowNotificationError("Thông báo", "Không có kế nối với cơ sở dữ liệu", 1, "0x1", "0x8", "normal");
                        }
                    }
                }
                //END LOGIN
            }
            catch (Exception ex)
            {
                NotificationLauncher.ShowNotificationError("Thông báo", "Không có kế nối với cơ sở dữ liệu máy bán", 1, "0x1", "0x8", "normal");
                WriteLogs.LogError(ex);
            }
        }
        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            LOGIN();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPassWord.Text != "" && txtUserName.Text != "")
                {
                    LOGIN();
                }
                else
                {
                    if (string.IsNullOrEmpty(txtUserName.Text)) txtUserName.Focus();
                    else txtPassWord.Focus();
                }
            }
        }

        private void txtPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtPassWord.Text != "" && txtUserName.Text != "")
                {
                    LOGIN();
                }
                else
                {
                    if (string.IsNullOrEmpty(txtUserName.Text)) txtUserName.Focus();
                    else txtPassWord.Focus();
                }
            }
        }

        private void btnCreateConnect_Click(object sender, EventArgs e)
        {
            FrmConnectDatabase frmConnectDatabase = new FrmConnectDatabase();
            frmConnectDatabase.ShowDialog();
        }

        private void btnModifieldPassword_Click(object sender, EventArgs e)
        {
            FrmDoiMatKhau frmDoiMatKhau = new FrmDoiMatKhau();
            frmDoiMatKhau.ShowDialog();
        }
    }
}

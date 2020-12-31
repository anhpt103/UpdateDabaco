using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;
namespace BTS.SP.BANLE.Common
{
    public class Config
    {
        public static string path = @"C:\BTS_SP_BANLE\DATA\";
        public static bool CheckConnectToServer()
        {
            bool result = false;
            OracleConnection connection = new OracleConnection();
            try
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["TBNETERP_SERVER"].ToString();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return result;
        }
    }
}

namespace BTS.SP.BANLE.Common
{
    public class Config
    {
        public static string CheckConnectToServer(out bool result)
        {
            result = false;

            string msg = "";
            if (Session.Session.SESSION_ID_CSDL == 0 || Session.Session.SESSION_ID_CSDL == null)
            {
                msg = "KHÔNG XÁC ĐỊNH ĐƯỢC CƠ SỞ DỮ LIỆU MÁY BÁN HIỆN ĐANG SỬ DỤNG";
                result = false;
            }
            else if (Session.Session.SESSION_ID_CSDL == 1) result = true;
            else if (Session.Session.SESSION_ID_CSDL == 2) result = false;

            return msg;
        }
    }
}

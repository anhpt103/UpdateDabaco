using System;
namespace BTS.SP.BANLE.Dto
{
    public class HANGKHACHHANG_DTO
    {
        public string ID { get; set; }
        public string MAHANGKH { get; set; }
        public string TENHANGKH { get; set; }
        public decimal SOTIEN { get; set; }
        public decimal TYLEGIAMGIASN { get; set; }
        public decimal TYLEGIAMGIA { get; set; }
        public int TRANGTHAI { get; set; }
        public DateTime I_CREATE_DATE { get; set; }
        public string I_CREATE_BY { get; set; }
        public DateTime I_UPDATE_DATE { get; set; }
        public string I_UPDATE_BY { get; set; }
        public string I_STATE { get; set; }
        public string UNITCODE { get; set; }
        public decimal QUYDOITIEN_THANH_DIEM { get; set; }
        public decimal QUYDOIDIEM_THANH_TIEN { get; set; }
    }
}

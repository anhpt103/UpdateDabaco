using System;

namespace BTS.SP.BANLE.Dto
{
    public class KHACHHANG_DTO
    {
        public string MAKH { get; set; }
        public string TENKH { get; set; }
        public string DIACHI { get; set; }
        public string DIENTHOAI { get; set; }
        public string CMTND { get; set; }
        public string EMAIL { get; set; }
        public decimal SODIEM { get; set; }
        public decimal TONGTIEN { get; set; }
        public DateTime? NGAYCAPTHE { get; set; }
        public DateTime? NGAYHETHAN { get; set; }
        public DateTime? NGAYSINH { get; set; }
        public string UNITCODE { get; set; }
        public string MATHE { get; set; }
        public string HANGKHACHHANG { get; set; }
        public string HANGKHACHHANGCU { get; set; }
    }
}

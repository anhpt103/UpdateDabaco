using System;
using System.Collections.Generic;

namespace BTS.SP.BANLE.Dto
{
    [Serializable]
    public class NVGDQUAY_ASYNCCLIENT_DTO
    {
        public NVGDQUAY_ASYNCCLIENT_DTO()
        {
            LST_DETAILS = new List<NVHANGGDQUAY_ASYNCCLIENT>();
        }
        public string ID { get; set; }
        public string MAGIAODICH { get; set; }
        public string MAGIAODICHQUAYPK { get; set; }
        public string MADONVI { get; set; }
        public int LOAIGIAODICH { get; set; }
        public DateTime NGAYTAO { get; set; }
        public string MANGUOITAO { get; set; }
        public string NGUOITAO { get; set; }
        public string MAQUAYBAN { get; set; }
        public DateTime NGAYPHATSINH { get; set; }
        public string HINHTHUCTHANHTOAN { get; set; }
        public string MAVOUCHER { get; set; }
        public decimal TIENKHACHDUA { get; set; }
        public decimal TIENVOUCHER { get; set; }
        public decimal TIENTHEVIP { get; set; }
        public decimal TIENTRALAI { get; set; }
        public decimal TIENTHE { get; set; }
        public decimal TIENCOD { get; set; }
        public decimal TIENMAT { get; set; }
        public decimal TTIENCOVAT { get; set; }
        public string THOIGIAN { get; set; }
        public string MAKHACHHANG { get; set; }
        public decimal SODIEM { get; set; }
        public decimal SOTIEN_LENHANG { get; set; }
        public decimal TONGTIEN_KHACHHANG { get; set; }
        public string HANGKHACHHANG { get; set; }
        public string HANGKHACHHANG_MOI { get; set; }
        public DateTime I_CREATE_DATE { get; set; }
        public string I_CREATE_BY { get; set; }
        public DateTime I_UPDATE_DATE { get; set; }
        public string I_UPDATE_BY { get; set; }
        public string I_STATE { get; set; }
        public string UNITCODE { get; set; }
        public decimal TONGSOLUONG { get; set; }
        public decimal QUYDOITIEN_THANH_DIEM { get; set; }
        public decimal QUYDOIDIEM_THANH_TIEN { get; set; }
        public decimal QUYDOI_TOIDA { get; set; }
        public decimal DIEMQUYDOI { get; set; }
        public List<NVHANGGDQUAY_ASYNCCLIENT> LST_DETAILS { get; set; }

    }
    [Serializable]
    public class NVHANGGDQUAY_ASYNCCLIENT
    {
        public string ID { get; set; }
        public string MAGDQUAYPK { get; set; }
        public string MAKHOHANG { get; set; }
        public string MADONVI { get; set; }
        public string MAVATTU { get; set; }
        public string DONVITINH { get; set; }
        public string BARCODE { get; set; }
        public string TENDAYDU { get; set; }
        public string NGUOITAO { get; set; }
        public string MABOPK { get; set; }
        public DateTime NGAYTAO { get; set; }
        public DateTime NGAYPHATSINH { get; set; }
        public decimal SOLUONG { get; set; }
        public decimal TTIENCOVAT { get; set; }
        public decimal VATBAN { get; set; }
        public decimal GIABANLECOVAT { get; set; }
        public string MAKHACHHANG { get; set; }
        public string MAKEHANG { get; set; }
        public string MACHUONGTRINHKM { get; set; }
        public string LOAIKHUYENMAI { get; set; }
        public decimal TIENCHIETKHAU { get; set; }
        public decimal TYLECHIETKHAU { get; set; }
        public decimal TYLEKHUYENMAI { get; set; }
        public decimal TIENKHUYENMAI { get; set; }
        public decimal TYLEVOUCHER { get; set; }
        public decimal TIENVOUCHER { get; set; }
        public decimal TYLELAILE { get; set; }
        public decimal GIAVON { get; set; }
        public int ISBANAM { get; set; }
        public string MAVAT { get; set; }
        public string UNITCODE { get; set; }
        public string THANHTIENFULL { get; set; }
        public string MAVATTAT { get; set; }
        public decimal TTIENCOVAT_CHUA_GIAMGIA { get; set; }
    }
}

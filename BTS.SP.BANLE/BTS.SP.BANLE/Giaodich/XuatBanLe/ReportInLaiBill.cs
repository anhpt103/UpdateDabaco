using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BTS.SP.BANLE.Dto;
using DevExpress.XtraReports.UI;
using BTS.SP.BANLE.Common;
using DevExpress.XtraPrinting;

namespace BTS.SP.BANLE.Giaodich.XuatBanLe
{
    public partial class ReportInLaiBill : XtraReport
    {
        public ReportInLaiBill()
        {
            InitializeComponent();
        }
        // GIAO DỊCH BÁN LẺ
        public void InitDataInLaiBillBanLe(NVGDQUAY_ASYNCCLIENT_DTO _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL, BILL_DTO objecBillDto)
        {
            lblDate.Text = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.NGAYPHATSINH.ToString("dd/MM/yyyy") + " " + _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.THOIGIAN;
            p_Phone.Value = objecBillDto.PHONE;
            p_Address.Value = objecBillDto.ADDRESS;
            p_MaGiaoDich.Value = objecBillDto.MAGIAODICH;
            p_InfoThuNgan.Value = objecBillDto.INFOTHUNGAN;
            p_MaKH.Value = objecBillDto.MAKH;
            p_Diem.Value = objecBillDto.DIEM;
            p_ThanhTienChu.Value = objecBillDto.THANHTIENCHU;
            p_ConLai.Value = objecBillDto.CONLAI;
            p_TienKhachTra.Value = objecBillDto.TIENKHACHTRA;
            if (_NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS.Count > 0)
            {
                List<VATTU_DTO.OBJ_VAT> obj_Vat = new List<VATTU_DTO.OBJ_VAT>();
                foreach (var rowData in _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS)
                {
                    var existVat = obj_Vat.FirstOrDefault(x => x.MAVATRA == rowData.MAVAT);
                    if (existVat != null)
                    {
                        existVat.CHUACO_GTGT += rowData.TTIENCOVAT / (1 + (rowData.VATBAN/100));
                        existVat.CO_GTGT += (rowData.TTIENCOVAT / (1 + (rowData.VATBAN / 100))) * (rowData.VATBAN / 100);
                    }
                    else
                    {
                        VATTU_DTO.OBJ_VAT vat = new VATTU_DTO.OBJ_VAT();
                        vat.MAVATRA = rowData.MAVAT;
                        vat.TYLEVATRA = rowData.VATBAN;
                        vat.CHUACO_GTGT = rowData.TTIENCOVAT / (1 + (rowData.VATBAN / 100));
                        vat.CO_GTGT = vat.CHUACO_GTGT*(vat.TYLEVATRA/100);
                        obj_Vat.Add(vat);
                    }
                    decimal tempKM = 0;
                    try
                    {
                        tempKM = rowData.TIENKHUYENMAI + rowData.TIENCHIETKHAU;
                    }
                    catch (Exception) { }
                    if(tempKM != 0)
                    {
                        rowData.THANHTIENFULL = string.Format(@"KM " + FormatCurrency.FormatMoney(tempKM) + " " + FormatCurrency.FormatMoney(rowData.TTIENCOVAT));
                    }
                    else
                    {
                        rowData.THANHTIENFULL = FormatCurrency.FormatMoney(rowData.TTIENCOVAT);
                    }
                    int vattat = 0;
                    int.TryParse(rowData.MAVAT,out vattat);
                    rowData.MAVATTAT = vattat.ToString();
                }
                if (obj_Vat.Count > 0)
                {
                    decimal TONGCHUAVAT = 0, TONGCOVAT = 0;
                    foreach (VATTU_DTO.OBJ_VAT item in obj_Vat)
                    {
                        int vattat = 0;
                        int.TryParse(item.MAVATRA, out vattat);
                        XRTableRow row = new XRTableRow();
                        row.Cells.Add(new XRTableCell()
                        {
                            Text = vattat.ToString()+" > "+item.TYLEVATRA.ToString("#0'%'"),
                            TextAlignment = TextAlignment.MiddleCenter,
                            Font = new Font(Font.FontFamily, 6, FontStyle.Regular)
                        });
                        row.Cells.Add(new XRTableCell()
                        {
                            Text = item.CHUACO_GTGT.ToString("##,###"),
                            TextAlignment = TextAlignment.MiddleCenter,
                            Font = new Font(Font.FontFamily, 6, FontStyle.Regular)
                        });
                        row.Cells.Add(new XRTableCell()
                        {
                            Text = item.CO_GTGT.ToString("##,###"),
                            TextAlignment = TextAlignment.MiddleCenter,
                            Font = new Font(Font.FontFamily, 6, FontStyle.Regular)
                        });
                        xtVAT.Rows.Add(row);
                        TONGCHUAVAT += item.CHUACO_GTGT;
                        TONGCOVAT += item.CO_GTGT;
                    }
                    XRTableRow rowData = new XRTableRow();
                    rowData.Cells.Add(new XRTableCell()
                    {
                        Text = "Tổng : ",
                        TextAlignment = TextAlignment.MiddleCenter,
                        Font = new Font(Font.FontFamily, 6, FontStyle.Regular)
                    });
                    rowData.Cells.Add(new XRTableCell()
                    {
                        Text = TONGCHUAVAT.ToString("##,###"),
                        TextAlignment = TextAlignment.MiddleCenter,
                        Font = new Font(Font.FontFamily, 6, FontStyle.Regular)
                    });
                    rowData.Cells.Add(new XRTableCell()
                    {
                        Text = TONGCOVAT.ToString("##,###"),
                        TextAlignment = TextAlignment.MiddleCenter,
                        Font = new Font(Font.FontFamily, 6, FontStyle.Regular)
                    });
                    xtTong.Rows.Add(rowData);
                }
            }
            objectDataSource1.DataSource = _NVGDQUAY_ASYNCCLIENT_BILL_GLOBAL.LST_DETAILS;
        }
    }
}

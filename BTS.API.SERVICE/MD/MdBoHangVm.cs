using BTS.API.ENTITY.Md;
using BTS.API.SERVICE.BuildQuery;
using BTS.API.SERVICE.BuildQuery.Query.Types;
using BTS.API.SERVICE.Helper;
using BTS.API.SERVICE.Services;
using System;
using System.Collections.Generic;
namespace BTS.API.SERVICE.MD
{
    public class MdBoHangVm
    {
        public class Search : IDataSearch
        {
            public string MaBoHang { get; set; }
            public string TenBoHang { get; set; }
            public DateTime? NgayCT { get; set; }
            public string DefaultOrder
            {
                get
                {
                    return ClassHelper.GetPropertyName(() => new MdBoHang().TenBoHang);
                }
            }
            public List<IQueryFilter> GetFilters()
            {
                var result = new List<IQueryFilter>();
                var refObj = new MdBoHang();

                if (!string.IsNullOrEmpty(this.MaBoHang))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.MaBoHang),
                        Value = this.MaBoHang,
                        Method = FilterMethod.Like
                    });
                }
                if (!string.IsNullOrEmpty(this.TenBoHang))
                {
                    result.Add(new QueryFilterLinQ
                    {
                        Property = ClassHelper.GetProperty(() => refObj.TenBoHang),
                        Value = this.TenBoHang,
                        Method = FilterMethod.Like
                    });
                }
                return result;
            }

            public List<IQueryFilter> GetQuickFilters()
            {
                return null;
            }

            public void LoadGeneralParam(string summary)
            {
                MaBoHang = summary;//Ma Hop Dong
                TenBoHang = summary;
            }

        }
        public class Dto : DataInfoDtoVm
        {
            public Dto()
            {
                DataDetails = new List<DtoDetail>();
            }
            public string MaBoHang { get; set; }
            public string TenBoHang { get; set; }
            public DateTime? NgayCT { get; set; }
            public int TrangThai { get; set; }
            public decimal ThanhTien { get; set; }
            public decimal SoLuong { get; set; }
            public decimal SoLuongIn { get; set; }
            public string GhiChu { get; set; }
            public decimal TongLe { get; set; }
            public decimal TongBuon { get; set; }
            public List<DtoDetail> DataDetails { get; set; }

        }
        public class DtoDetail
        {

            public string Id { get; set; }
            public string MaBo { get; set; }
            public string MaHang { get; set; }
            public string TenHang { get; set; }
            public int SoLuong { get; set; }
            public decimal TyLeCKLe { get; set; }
            public decimal TyLeCKBuon { get; set; }
            public decimal TongBanLe { get; set; }
            public decimal TongBanBuon { get; set; }
            public decimal DonGia { get; set; }
            public string UnitCode { get; set; }
            public decimal GiaBanLeVat { get; set; }
        }
    }
}

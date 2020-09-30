using BTS.API.ENTITY;
using BTS.API.ENTITY.Md;
using BTS.API.SERVICE.Services;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BTS.API.SERVICE.MD
{

    public interface IMdSupplierService : IDataInfoService<MdSupplier>
    {
        string BuildCode();
        string SaveCode();
        MdSupplier CreateNewInstance();
        //Add function here
    }
    public class MdSupplierService : DataInfoServiceBase<MdSupplier>, IMdSupplierService
    {
        public MdSupplierService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
        protected override Expression<Func<MdSupplier, bool>> GetKeyFilter(MdSupplier instance)
        {
            var maDonViCha = GetParentUnitCode();
            var unitCode = GetCurrentUnitCode();
            return x => x.MaNCC == instance.MaNCC && x.UnitCode.StartsWith(maDonViCha);
        }

        public MdSupplier CreateNewInstance()
        {
            return new MdSupplier()
            {
                MaNCC = BuildCode()
            };
        }
        public string BuildCode()
        {
            var type = TypeMasterData.NCC.ToString();
            var result = "";
            var idRepo = UnitOfWork.Repository<MdIdBuilder>();
            var maDonViCha = GetParentUnitCode();
            var config = idRepo.DbSet.Where(x => x.Type == type && x.UnitCode == maDonViCha).FirstOrDefault();
            if (config == null)
            {
                config = new MdIdBuilder
                {
                    Id = Guid.NewGuid().ToString(),
                    Type = type,
                    Code = type,
                    Current = "0000",
                    UnitCode = maDonViCha,
                };
            }
            var soMa = config.GenerateNumber();
            config.Current = soMa;
            result = string.Format("{0}", soMa);

            return result;
        }

        public string SaveCode()
        {
            var type = TypeMasterData.NCC.ToString();
            var result = "";

            var idRepo = UnitOfWork.Repository<MdIdBuilder>();
            var maDonViCha = GetParentUnitCode();
            var config = idRepo.DbSet.Where(x => x.Type == type && x.UnitCode == maDonViCha).FirstOrDefault();
            if (config == null)
            {
                config = new MdIdBuilder
                {
                    Id = Guid.NewGuid().ToString(),
                    Type = type,
                    Code = type,
                    Current = "0000",
                    UnitCode = maDonViCha,
                };
                result = config.GenerateNumber();
                config.Current = result;
                idRepo.Insert(config);
            }
            else
            {
                result = config.GenerateNumber();
                config.Current = result;
                config.ObjectState = ObjectState.Modified;
            }
            result = string.Format("{0}", config.Current);
            return result;
        }
    }
}

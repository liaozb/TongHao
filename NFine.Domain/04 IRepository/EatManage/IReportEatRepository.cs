using NFine.Data;
using NFine.Domain.Entity.EatManage;
using System;
using System.Collections.Generic;

namespace NFine.Domain.IRepository.EatManage
{
    public interface IReportEatRepository : IRepositoryBase<ReportEatEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(ReportEatEntity userEntity, string keyValue);
        List<ReportEatEntity> GetList(string creatorUserId, DateTime start, DateTime end);
        ReportEatEntity GetCurrentTime(string creatorUserId, DateTime time);
    }
    public interface IAttLogRepository : IRepositoryBase<AttLogEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(AttLogEntity userEntity, string keyValue);
        List<AttLogEntity> GetList(string creatorUserId, DateTime start, DateTime end);
        AttLogEntity GetCurrentTime(string creatorUserId, DateTime time, bool sw);
    }
    public interface IPosLogRepository : IRepositoryBase<PosLogEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(PosLogEntity userEntity, string keyValue);
        List<PosLogEntity> GetList(string creatorUserId, DateTime start, DateTime end);
        PosLogEntity GetCurrentTime(string creatorUserId, DateTime time,int zzw);
    }
}

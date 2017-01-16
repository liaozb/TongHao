using NFine.Code;
using NFine.Domain.Entity.EatManage;
using NFine.Domain.IRepository.EatManage;
using NFine.Repository.EatManage;
using System.Collections.Generic;
using System;
namespace NFine.Application.EatManage
{
    public class ReportEatApp
    {
        private IReportEatRepository service = new ReportEatRepository();
        public List<ReportEatEntity> GetList(string userid,DateTime start,DateTime end)
        {
            return service.GetList(userid, start,end);
        }
        public ReportEatEntity GetCurrentTime(string userid, DateTime start)
        {
            return service.GetCurrentTime(userid, start);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(ReportEatEntity userEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                userEntity.Modify(keyValue);
                service.Update(userEntity);
            }
            else
            {
                userEntity.Create();
                service.Insert(userEntity);
            }
        }
    }
    public class AttLogApp
    {
        private IAttLogRepository service = new AttLogRepository();
        public List<AttLogEntity> GetList(string userid, DateTime start, DateTime end)
        {
            return service.GetList(userid, start, end);
        }
        public AttLogEntity GetCurrentTime(string userid, DateTime start, bool sw)
        {
            return service.GetCurrentTime(userid, start,sw);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(AttLogEntity userEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                userEntity.Modify(keyValue);
                service.Update(userEntity);
            }
            else
            {
                userEntity.Create();
                service.Insert(userEntity);
            }
        }
    }
    public class PosLogApp
    {
        private IPosLogRepository service = new PosLogRepository();
        public List<PosLogEntity> GetList(string userid, DateTime start, DateTime end)
        {
            return service.GetList(userid, start, end);
        }
        public PosLogEntity GetCurrentTime(string userid, DateTime start,int zzw)
        {
            return service.GetCurrentTime(userid, start, zzw);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(PosLogEntity userEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                userEntity.Modify(keyValue);
                service.Update(userEntity);
            }
            else
            {
                userEntity.Create();
                service.Insert(userEntity);
            }
        }
    }
}

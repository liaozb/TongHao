using NFine.Data;
using NFine.Domain.Entity.EatManage;
using NFine.Domain.IRepository.EatManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
 
namespace NFine.Repository.EatManage
{
    public class ReportEatRepository : RepositoryBase<ReportEatEntity>, IReportEatRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<ReportEatEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
            
        }
        public List<ReportEatEntity> GetList(string creatorUserId, DateTime start, DateTime end)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT *
                            FROM    Report_Eat 
                                    
                            WHERE  F_UserId=@F_UserId and  F_Time between @start and @end order by F_IsEat");
            DbParameter[] parameter =
            {
                 new SqlParameter("@F_UserId",creatorUserId),
                  new SqlParameter("@start",start.ToString ("yyyy-MM-dd 00:00:00")),
                  new SqlParameter("@end",end.ToString ("yyyy-MM-dd 23:59:59"))
            };
            return this.FindList(strSql.ToString(), parameter);
        }
        public ReportEatEntity GetCurrentTime(string creatorUserId, DateTime time,int dd)
        {
            return this.FindEntity(f=> f.F_UserId == creatorUserId && f.F_IsEat==dd && f.F_Time.Year==time.Year && f.F_Time.Month==time.Month&& f.F_Time.Day==time.Day);
        }
        public void SubmitForm(ReportEatEntity userEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(userEntity);
                }
                else
                {
                    db.Insert(userEntity);
                }
                db.Commit();
            }
        }
    }
    public class AttLogRepository : RepositoryBase<AttLogEntity>, IAttLogRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<ReportEatEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }

        }
        public List<AttLogEntity> GetList(string CardNo, DateTime start, DateTime end)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT * FROM  AttLog  WHERE  CardNo=@card and AttDate between @start and @end ");
            DbParameter[] parameter =
            {
                 new SqlParameter("@card",CardNo),
                  new SqlParameter("@start",start.ToString("yyyy-MM-dd 00:00:00")),
                  new SqlParameter("@end",end.ToString ("yyyy-MM-dd 23:59:59"))
            };
            return this.FindList(strSql.ToString(), parameter);
        }
        public AttLogEntity GetCurrentTime(string CardNo, DateTime time,bool sw)
        {
            if (sw)
            {
                return this.FindEntity(f => f.CardNo == CardNo && f.AttDate.Year == time.Year && f.AttDate.Month == time.Month && f.AttDate.Day == time.Day && f.AttDate.Hour <12);
            }
            return this.FindEntity(f => f.CardNo == CardNo && f.AttDate.Year == time.Year && f.AttDate.Month == time.Month && f.AttDate.Day == time.Day && f.AttDate.Hour > 12);
        }
        public void SubmitForm(AttLogEntity userEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                     
                    db.Update(userEntity);
                }
                else
                {
                    db.Insert(userEntity);
                }
                db.Commit();
            }
        }
    }
    public class PosLogRepository : RepositoryBase<PosLogEntity>, IPosLogRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<PosLogEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }

        }
        public List<PosLogEntity> GetList(string creatorUserId, DateTime start, DateTime end)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"SELECT *
                            FROM    PosLog 
                                    
                            WHERE  CardNo=@CardNo and  PosTime between @start and @end ");
            DbParameter[] parameter =
            {
                 new SqlParameter("@CardNo",creatorUserId),
                  new SqlParameter("@start",start.ToString ("yyyy-MM-dd 00:00:00")),
                  new SqlParameter("@end",end.ToString ("yyyy-MM-dd 23:59:59"))
            };
            return this.FindList(strSql.ToString(), parameter);
        }
        public PosLogEntity GetCurrentTime(string cardNo, DateTime time,int zzw)
        {
            if (zzw==0)
            {
                return this.FindEntity(f => f.CardNo == cardNo && f.PosTime.Year == time.Year && f.PosTime.Month == time.Month && f.PosTime.Day == time.Day && f.PosTime.Hour<9);
            }
            else if (zzw ==1)
            {
                return this.FindEntity(f => f.CardNo == cardNo && f.PosTime.Year == time.Year && f.PosTime.Month == time.Month && f.PosTime.Day == time.Day && f.PosTime.Hour > 9 && f.PosTime.Hour < 14);
            }
            else 
            {
                return this.FindEntity(f => f.CardNo == cardNo && f.PosTime.Year == time.Year && f.PosTime.Month == time.Month && f.PosTime.Day == time.Day && f.PosTime.Hour > 14);
            }

        }
        public void SubmitForm(PosLogEntity userEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(userEntity);
                }
                else
                {
                    db.Insert(userEntity);
                }
                db.Commit();
            }
        }
    }
}

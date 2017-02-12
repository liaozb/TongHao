using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.EatManage
{
   public class ReportEatEntity:IEntity<ReportEatEntity>, ICreationAudited,IModificationAudited
    {
        public string F_Id { get; set; }
        public DateTime F_Time { get; set; }
        public int F_IsEat { get; set; }
        public string F_UserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
    }
    public class AttLogEntity : IEntity<AttLogEntity>, ICreationAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public DateTime AttDate { get; set; }
        public string CardNo { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
    }
    public class PosLogEntity : IEntity<PosLogEntity>, ICreationAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public DateTime PosTime { get; set; }
        public string CardNo { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
    }
}

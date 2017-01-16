using NFine.Domain.Entity.EatManage;
using System.Data.Entity.ModelConfiguration;

namespace NFine.Mapping.EatManage
{
    public class ReportEatMap : EntityTypeConfiguration<ReportEatEntity>
    {
        public ReportEatMap()
        {
            this.ToTable("Report_Eat");
            this.HasKey(t => t.F_Id);
        }
    }
    public class AttLogEntityMap : EntityTypeConfiguration<AttLogEntity>
    {
        public AttLogEntityMap()
        {
            this.ToTable("AttLog");
            this.HasKey(t => t.F_Id);
        }
    }
    public class PosLogMap : EntityTypeConfiguration<PosLogEntity>
    {
        public PosLogMap()
        {
            this.ToTable("PosLog");
            this.HasKey(t => t.F_Id);
        }
    }
}

using NFine.Application.EatManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.EatManage.Controllers
{
    public class DataStatisticsForm
    {
        public string name { get; set; }
        public string F_Id { get; set; }
        public string F_DepartmentId { get; set; }
       
        public string baocan { get; set; }
        public string shangban { get; set; }
        public string xiaban { get; set; }
        public string zaocan { get; set; }
        public string wucan { get; set; }
        public string wancan { get; set; }
        public string yujibuzhu { get; set; }
        public string cishu { get; set; }
    }
    public class DataStatisticsController : ControllerBase
    {
        //
        // GET: /EatManage/DataStatistics/
        private ReportEatApp reportApp = new ReportEatApp();
        //考勤
        private AttLogApp attlApp = new AttLogApp();
        //消费
        private PosLogApp posApp = new PosLogApp();
        private UserApp userApp = new UserApp();
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridDataStatisticsJson(Pagination pagination, string keyword, string starttime)
        {


            DateTime now = DateTime.Now;
            if (!string.IsNullOrEmpty(starttime))
            {
                now = starttime.ToDate();
            }
            var lists = new List<DataStatisticsForm>();
            foreach (var item in userApp.GetList(pagination, keyword))
            {
                var list = new DataStatisticsForm();
                list.F_DepartmentId = item.F_DepartmentId;
                list.F_Id = item.F_Id;
                list.name = item.F_RealName;
                var baocan = reportApp.GetCurrentTime(item.F_Id, now,1);
                list.baocan = "未报";
                if (baocan != null)
                {
                    list.baocan = "已报";
                }
                var attl = attlApp.GetCurrentTime(item.F_AttCard, now, true);
                list.shangban = "×";
                if (attl != null)
                {
                    list.shangban = "√";
                }
                var attl2 = attlApp.GetCurrentTime(item.F_AttCard, now, false);
                list.xiaban = "×";
                if (attl2 != null)
                {
                    list.xiaban = "√";
                }
                var pos = posApp.GetCurrentTime(item.F_PosCard, now, 0);
                list.zaocan = "×";
                if (pos != null)
                {
                    list.zaocan = "√";
                }
                var pos1 = posApp.GetCurrentTime(item.F_PosCard, now, 1);
                list.wucan = "×";
                if (pos1 != null)
                {
                    list.wucan = "√";
                }
                var pos2 = posApp.GetCurrentTime(item.F_PosCard, now, 2);
                list.wancan = "×";
                if (pos2 != null)
                {
                    list.wancan = "√";
                }
                int bu = 0;
                int kou = 0;
                PosT(now, item.F_PosCard, item.F_AttCard, item.F_Id, out bu, out kou);
                list.yujibuzhu = (bu * 15).ToString();
                list.cishu = (kou).ToString();
                lists.Add(list);
            }
            var data = new
            {
                rows = lists,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        //补助条件
        //a.早上刷卡，上午报餐，中午吃饭补助15元；
        //b.早上刷卡，没报餐，中午吃饭补助15元；
        //c.中餐刷卡多次只补助一次。
        //不补助条件
        //a.早上未刷卡，中午吃饭；
        //b.早上刷卡，上午报餐，中午未吃饭；
        //C.早上刷卡，中午没吃饭。
        public void PosT(DateTime dt, string F_PosCard, string F_AttCard, string F_Id, out int buzhucishu, out int kou)
        {
            DateTime starttime = new DateTime(dt.Year, dt.Month, 1);
            DateTime endtime = starttime.AddMonths(1).AddDays(-1);


            buzhucishu = 0;
            kou = 0;
            for (DateTime i = starttime; i < endtime; i = i.AddDays(1))
            {
                if (i > DateTime.Now)
                {
                    return;
                }
                var baocan = reportApp.GetCurrentTime(F_Id, i,1);
                //考勤
                var attl = attlApp.GetCurrentTime(F_AttCard, i, true);
                var pos1 = posApp.GetCurrentTime(F_PosCard, i, 1);
                if (attl != null && baocan != null && pos1 != null)
                {
                    buzhucishu = buzhucishu + 1;
                }
                else
                {
                    if (attl != null && baocan == null && pos1 != null)
                    {
                        buzhucishu = buzhucishu + 1;
                    }
                    else
                    {
                        kou = kou + 1;
                    }

                }
            }

        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult WriteToExcel(Pagination pagination, string keyword, string starttime)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("部门");
            dt.Columns.Add("姓名");
            dt.Columns.Add("报餐情况");
            dt.Columns.Add("上班考勤");
            dt.Columns.Add("下班考勤"); dt.Columns.Add("早晨刷卡"); dt.Columns.Add("中餐刷卡"); dt.Columns.Add("晚餐刷卡"); dt.Columns.Add("扣次数");
            dt.Columns.Add("预计补助");
            DateTime now = DateTime.Now;
            if (!string.IsNullOrEmpty(starttime))
            {
                now = starttime.ToDate();
            }
            var lists = new List<DataStatisticsForm>();
            OrganizeApp organizeApp = new OrganizeApp();
            foreach (var item in userApp.GetList(pagination, keyword))
            {
                DataRow dr = dt.NewRow();  
                dr["部门"] = organizeApp.GetForm( item.F_DepartmentId).F_FullName;
                  
                dr["姓名"] = item.F_RealName;
                var baocan = reportApp.GetCurrentTime(item.F_Id, now,1);
                dr["报餐情况"] = "未报";
                if (baocan != null)
                {
                    dr["报餐情况"] = "已报";
                }
                var attl = attlApp.GetCurrentTime(item.F_AttCard, now, true);
                dr["上班考勤"] = "×";
                if (attl != null)
                {
                    dr["上班考勤"] = "√";
                }
                var attl2 = attlApp.GetCurrentTime(item.F_AttCard, now, false);
                dr["下班考勤"] = "×";
                if (attl2 != null)
                {
                    dr["下班考勤"] = "√";
                }
                var pos = posApp.GetCurrentTime(item.F_PosCard, now, 0);
                dr["早晨刷卡"] = "×";
                if (pos != null)
                {
                    dr["早晨刷卡"] = "√";
                }
                var pos1 = posApp.GetCurrentTime(item.F_PosCard, now, 1);
                dr["中餐刷卡"] = "×";
                if (pos1 != null)
                {
                    dr["中餐刷卡"] = "√";
                }
                var pos2 = posApp.GetCurrentTime(item.F_PosCard, now, 2);
                dr["晚餐刷卡"] = "×";
                if (pos2 != null)
                {
                    dr["晚餐刷卡"] = "√";
                }
                int bu = 0;
                int kou = 0;
                PosT(now, item.F_PosCard, item.F_AttCard, item.F_Id, out bu, out kou);
                dr["预计补助"] = (bu * 15).ToString();
                dr["扣次数"] = (kou).ToString();
                dt.Rows.Add(dr);
            }
            NPOIExcel npoiexel = new NPOIExcel();
            string fileDir = DateTime.Now.ToString("yyyyMMdd");
            string fileName = "G" + Guid.NewGuid().ToString("N") + ".xls";
            string destDir = Server.MapPath(@"/XlsTemp") + "\\" + fileDir + "\\";
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            npoiexel.ToExcel(dt, "数据", "Sheet1", destDir + fileName);
            return Content("/XlsTemp/" + fileDir + "/" + fileName);
        }
    }
}

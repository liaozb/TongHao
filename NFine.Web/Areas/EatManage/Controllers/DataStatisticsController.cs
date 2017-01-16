using NFine.Application.EatManage;
using NFine.Application.SystemManage;
using NFine.Code;
using System;
using System.Collections.Generic;
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
        public ActionResult GetGridDataStatisticsJson(Pagination pagination, string keyword,string starttime )
        {
            DateTime now = DateTime.Now;
            if (!string.IsNullOrEmpty(starttime))
            {
                now = starttime.ToDate();
            }
            var lists = new List<DataStatisticsForm>();
            foreach (var item in userApp.GetList(pagination,keyword))
            {
                var list = new DataStatisticsForm();
                list.F_DepartmentId = item.F_DepartmentId;
                list.F_Id = item.F_Id;
                list.name = item.F_RealName;
                var baocan = reportApp.GetCurrentTime(item.F_Id, now);
                list.baocan = "未报";
                if (baocan!=null &&baocan.F_IsEat)
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
                var pos= posApp.GetCurrentTime(item.F_PosCard, now, 0);
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
                list.cishu = (kou * 15).ToString();
                lists.Add(list);
            }
            return Content(lists.ToJson());
        }
        //补助条件
        //a.早上刷卡，上午报餐，中午吃饭补助15元；
        //b.早上刷卡，没报餐，中午吃饭补助15元；
        //c.中餐刷卡多次只补助一次。
        //不补助条件
        //a.早上未刷卡，中午吃饭；
        //b.早上刷卡，上午报餐，中午未吃饭；
        //C.早上刷卡，中午没吃饭。
        public void  PosT(DateTime dt, string F_PosCard, string F_AttCard, string F_Id,out int buzhucishu,out int kou)
        {
            DateTime starttime = new DateTime(dt.Year, dt.Month, 1);
            DateTime endtime =starttime.AddMonths(1).AddDays(-1);


             buzhucishu = 0;
            kou = 0;
            for (DateTime i = starttime; i < endtime; i=i.AddDays (1))
            {
                if (i>DateTime.Now)
                {
                    return;
                }
                var baocan = reportApp.GetCurrentTime(F_Id, i);
                //考勤
                var attl = attlApp.GetCurrentTime(F_AttCard, i, true);
                var pos1 = posApp.GetCurrentTime(F_PosCard,i, 1);
                if (attl != null && baocan != null && baocan.F_IsEat && pos1 != null)
                {
                    buzhucishu = buzhucishu + 1;
                }
                else {
                    if (attl != null && baocan == null && pos1 != null)
                    {
                        buzhucishu = buzhucishu + 1;
                    }
                    else {
                        kou = kou + 1;
                    }
                   
                }
            }
             
        }

    }
}

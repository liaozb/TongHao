using NFine.Application.EatManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.EatManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFine.Web.Areas.EatManage.Controllers
{
    public class EatForm
    {
        public string week { get; set; }
        public bool ck { get; set; }
   
    }

    public class ReportEatController : Controller
    {
        private ReportEatApp reportApp = new ReportEatApp();
   
        //
        // GET: /EatManage/ReportEat/
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson()
        {
            DateTime start = Request.QueryString["start"].ToDate();
            DateTime end = Request.QueryString["end"].ToDate();
            var data = reportApp.GetList(OperatorProvider.Provider.GetCurrent().UserId, start, end).Select(f => new
            {
                id = f.F_Id,
                title =(f.F_IsEat==0?"早餐": f.F_IsEat == 1 ? "早餐":"晚餐")+ "已报餐",
                start = f.F_Time,
                end = f.F_Time,
                url = "",
                allDay = true,
                color = "#06c"
            });
            return Content(data.ToJson());
        }
    
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(EatForm organizeEntity)
        {
            int i = DateTime.Now.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            DateTime startWeek = DateTime.Now.Subtract(ts);
            int dd = 0;
            if (organizeEntity.week.Contains("zhong"))
            {
                dd = 1;
            }
            else if (organizeEntity.week.Contains("wan"))
            {
                dd = 2;
            }
            if (organizeEntity.week.Contains("2"))
            {
                startWeek=startWeek.AddDays(1);
            }
            else if (organizeEntity.week.Contains("3"))
            {
                startWeek = startWeek.AddDays(2);
            }
            else if (organizeEntity.week.Contains("4"))
            {
                startWeek = startWeek.AddDays(3);
            }
            else if (organizeEntity.week.Contains("5"))
            {
                startWeek = startWeek.AddDays(4);
            }
            else if (organizeEntity.week.Contains("6"))
            {
                startWeek = startWeek.AddDays(5);
            }
            else if (organizeEntity.week.Contains("7"))
            {
                startWeek = startWeek.AddDays(6);
            }
            var today = reportApp.GetCurrentTime(OperatorProvider.Provider.GetCurrent().UserId, DateTime.Now,dd);
            if (today != null)
            {
                if (organizeEntity.ck == false)
                {
                    reportApp.DeleteForm(today.F_Id);
                }
            }
            else
            {
                today = new ReportEatEntity();
                today.F_Time = startWeek;
                today.F_IsEat =dd;
                today.F_UserId = OperatorProvider.Provider.GetCurrent().UserId;
                reportApp.SubmitForm(today, "");
            }
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = "操作成功。" }.ToJson()); 
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string week)
        {
            int i = DateTime.Now.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            DateTime startWeek = DateTime.Now.Subtract(ts);
            int dd = 0;
            if (week.Contains("zhong"))
            {
                dd = 1;
            }
            else if (week.Contains("wan"))
            {
                dd = 2;
            }
            if (week.Contains("2"))
            {
                startWeek = startWeek.AddDays(1);
            }
            else if (week.Contains("3"))
            {
                startWeek = startWeek.AddDays(2);
            }
            else if (week.Contains("4"))
            {
                startWeek = startWeek.AddDays(3);
            }
            else if (week.Contains("5"))
            {
                startWeek = startWeek.AddDays(4);
            }
            else if (week.Contains("6"))
            {
                startWeek = startWeek.AddDays(5);
            }
            else if (week.Contains("7"))
            {
                startWeek = startWeek.AddDays(6);
            }
            var today = reportApp.GetCurrentTime(OperatorProvider.Provider.GetCurrent().UserId, startWeek, dd);
            if (today != null)
            {
                return Content(new { ck=true }.ToJson());
            }
            return Content(new { ck = false }.ToJson());
        }
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }
    }
}

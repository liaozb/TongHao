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
        public bool F_TodayTrue { get; set; }
        public bool F_TomorrowTrue { get; set; }
        public bool F_TodayFalse { get; set; }
        public bool F_TomorrowFalse { get; set; }
    }

    public class ReportEatController : ControllerBase
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
            var data = reportApp.GetList(OperatorProvider.Provider.GetCurrent().UserId, start, end).Where (f=> f.F_IsEat).Select(f => new
            {
                id = f.F_Id,
                title = "已报餐",
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
            var today = reportApp.GetCurrentTime(OperatorProvider.Provider.GetCurrent().UserId, DateTime.Now);
            if (today != null)
            {
                today.F_IsEat = organizeEntity.F_TodayTrue;
                reportApp.SubmitForm(today, today.F_Id);
            }
            else
            {
                today = new ReportEatEntity();
                today.F_Time = DateTime.Now;
                today.F_IsEat = organizeEntity.F_TodayTrue;
                today.F_UserId = OperatorProvider.Provider.GetCurrent().UserId;
                reportApp.SubmitForm(today, "");
            }
            var tw = reportApp.GetCurrentTime(OperatorProvider.Provider.GetCurrent().UserId, DateTime.Now.AddDays (1));
            if (tw != null)
            {
                tw.F_IsEat = organizeEntity.F_TomorrowTrue;
                reportApp.SubmitForm(tw, tw.F_Id);
            }
            else
            {
                tw = new ReportEatEntity();
                tw.F_Time = DateTime.Now.AddDays(1);
                tw.F_IsEat = organizeEntity.F_TomorrowTrue;
                tw.F_UserId = OperatorProvider.Provider.GetCurrent().UserId;
                reportApp.SubmitForm(tw, "");
            }
            return Success("操作成功。");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson()
        {
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now.AddDays(1);
            var lists = new EatForm();
            var data = reportApp.GetList(OperatorProvider.Provider.GetCurrent().UserId, start, end);
            foreach (var item in data)
            {
                if (item.F_Time.Date.Equals(DateTime.Now.Date))
                {
                    if (item.F_IsEat)
                    {
                        lists.F_TodayTrue = true;
                        lists.F_TodayFalse = false;
                    }
                    else
                    {
                        lists.F_TodayTrue = false;
                        lists.F_TodayFalse = true;
                    }
                }
                else
                {
                    if (item.F_IsEat)
                    {
                        lists.F_TomorrowTrue = true;
                        lists.F_TomorrowFalse = false;
                    }
                    else
                    {
                        lists.F_TomorrowTrue = false;
                        lists.F_TomorrowFalse = true;
                    }

                }
            }

            return Content(lists.ToJson());
        }

    }
}

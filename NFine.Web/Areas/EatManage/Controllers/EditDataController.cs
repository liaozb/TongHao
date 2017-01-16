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
    public class DataForm
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
    }
    public class EditDataController : ControllerBase
    {
        private ReportEatApp reportApp = new ReportEatApp();
        //考勤
        private AttLogApp attlApp = new AttLogApp();
        //消费
        private PosLogApp posApp = new PosLogApp();
        private UserApp userApp = new UserApp();
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(DataForm organizeEntity,string keyValue,string starttime)
        {
            DateTime now = DateTime.Now;
            if (!string.IsNullOrEmpty(starttime))
            {
                now = starttime.ToDate();
            }
            var item = userApp.GetForm(keyValue);
            var today = reportApp.GetCurrentTime(keyValue, now);
            if (today != null)
            {
                today.F_IsEat = (organizeEntity.baocan=="1" ?true :false);
                reportApp.SubmitForm(today, today.F_Id);
            }
            else
            {
                if (organizeEntity.baocan == "1")
                {
                    today = new ReportEatEntity();
                    today.F_Time = now;
                    today.F_IsEat = true;
                    today.F_UserId = keyValue;
                    reportApp.SubmitForm(today, "");
                }
            }
            var attl = attlApp.GetCurrentTime(item.F_AttCard, now, true);
            if (attl==null && organizeEntity.shangban == "1")
            {
                attl = new AttLogEntity();
                attl.AttDate = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
                attl.CardNo = item.F_AttCard;
                attlApp.SubmitForm(attl, "");
            }
            else if (attl != null && organizeEntity.shangban == "0")
            {
                attlApp.DeleteForm(attl.F_Id);
            }
            var attl2 = attlApp.GetCurrentTime(item.F_AttCard, now, false);
            if (attl2 == null && organizeEntity.xiaban == "1")
            {
                attl2 = new AttLogEntity();
                attl2.AttDate = new DateTime(now.Year, now.Month, now.Day, 17, 0, 0);
                attl2.CardNo = item.F_AttCard;
                attlApp.SubmitForm(attl2, "");
            }
            else if (attl2 != null && organizeEntity.xiaban == "0")
            {
                attlApp.DeleteForm(attl2.F_Id);
            }
            if (string.IsNullOrEmpty (item.F_PosCard))
            {
                return Error(item.F_NickName + "-此用户未填餐卡编号！");
            }
            var pos = posApp.GetCurrentTime(item.F_PosCard, now, 0);
            if (pos == null && organizeEntity.zaocan == "1")
            {
                pos = new PosLogEntity();
                pos.PosTime = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
                pos.CardNo = item.F_PosCard;
                posApp.SubmitForm(pos, "");
            }
            else if (pos !=null&& organizeEntity.zaocan == "0")
            {
                posApp.DeleteForm(pos.F_Id);
            }
            var pos1 = posApp.GetCurrentTime(item.F_PosCard, now, 1);
            if (pos1 == null && organizeEntity.wucan == "1")
            {
                pos1 = new PosLogEntity();
                pos1.PosTime = new DateTime(now.Year, now.Month, now.Day, 12, 0, 0);
                pos1.CardNo = item.F_PosCard;
                posApp.SubmitForm(pos1, "");
            }
            else if (pos1 != null && organizeEntity.wucan == "0")
            {
                posApp.DeleteForm(pos1.F_Id);
            }
            var pos2 = posApp.GetCurrentTime(item.F_PosCard, now, 2);
            if (pos2 == null && organizeEntity.wancan == "1")
            {
                pos2 = new PosLogEntity();
                pos2.PosTime = new DateTime(now.Year, now.Month, now.Day, 17, 0, 0);
                pos2.CardNo = item.F_PosCard;
                posApp.SubmitForm(pos2, "");
            }
            else if (pos2 != null)
            {
                posApp.DeleteForm(pos2.F_Id);
            }
            return Success("操作成功。");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue, string starttime)
        {
            DateTime now = DateTime.Now;
            if (!string.IsNullOrEmpty(starttime))
            {
                now = starttime.ToDate();
            }
            var list = new DataStatisticsForm();
            var item = userApp.GetForm(keyValue);
            var baocan = reportApp.GetCurrentTime(item.F_Id, now);
            list.baocan = "0";
            if (baocan != null && baocan.F_IsEat)
            {
                list.baocan = "1";
            }
            var attl = attlApp.GetCurrentTime(item.F_AttCard, now, true);
            list.shangban = "0";
            if (attl != null)
            {
                list.shangban = "1";
            }
            var attl2 = attlApp.GetCurrentTime(item.F_AttCard, now, false);
            list.xiaban = "0";
            if (attl2 != null)
            {
                list.xiaban = "1";
            }
            var pos = posApp.GetCurrentTime(item.F_PosCard, now, 0);
            list.zaocan = "0";
            if (pos != null)
            {
                list.zaocan = "1";
            }
            var pos1 = posApp.GetCurrentTime(item.F_PosCard, now, 1);
            list.wucan = "0";
            if (pos1 != null)
            {
                list.wucan = "1";
            }
            var pos2 = posApp.GetCurrentTime(item.F_PosCard, now, 2);
            list.wancan = "0";
            if (pos2 != null)
            {
                list.wancan = "1";
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridDataJson(Pagination pagination, string keyword, string starttime)
        {
            DateTime now = DateTime.Now;
            if (!string.IsNullOrEmpty (starttime))
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
                var baocan = reportApp.GetCurrentTime(item.F_Id, now);
                list.baocan = "未报";
                if (baocan != null && baocan.F_IsEat)
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
               
                lists.Add(list);
            }
            return Content(lists.ToJson());
        }
    }
}

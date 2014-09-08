﻿using Easy.CMS.Filter;
using Easy.CMS.Page;
using Easy.CMS.Widget;
using Easy.Web.Attribute;
using Easy.Web.Controller;
using Easy.Web.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Easy.Extend;
using Easy.CMS.Layout;

namespace Easy.CMS.Common.Controllers
{
    public class PageController : BasicController<PageEntity, PageService>
    {
        [Widget]
        public ActionResult PreView()
        {
            return View();
        }
        [AdminTheme]
        public override ActionResult Index(ParamsContext context)
        {
            return base.Index(context);
        }

        public JsonResult GetPageTree(ParamsContext context)
        {
            var pages = Service.Get(new Data.DataFilter().OrderBy("DisplayOrder", Constant.OrderType.Ascending));
            var node = new Easy.HTML.jsTree.Tree<PageEntity>().Source(pages).ToNode(m => m.ID, m => m.PageName, m => m.ParentId, "#");
            return Json(node, JsonRequestBehavior.AllowGet);
        }
        [AdminTheme, ViewData_Layouts]
        public override ActionResult Create(ParamsContext context)
        {
            var page = new PageEntity
            {
                ParentId = context.ParentID,
                DisplayOrder = Service.Get("ParentID", Constant.OperatorType.Equal, context.ParentID).Count() + 1,
                Url = "~/"
            };
            var parentPage = Service.Get(context.ParentID);
            if (parentPage != null)
            {
                page.Url = parentPage.Url;
            }
            if (!page.Url.EndsWith("/"))
            {
                page.Url += "/";
            }
            return View(page);

        }
        [AdminTheme, ViewData_Layouts]
        public override ActionResult Create(PageEntity entity)
        {
            return base.Create(entity);
        }
        [AdminTheme, ViewData_Layouts]
        public override ActionResult Edit(ParamsContext context)
        {
            return base.Edit(context);
        }
        [AdminTheme, ViewData_Layouts]
        [HttpPost]
        public override ActionResult Edit(PageEntity entity)
        {
            if (entity.ActionType == Constant.ActionType.Design)
            {
                return RedirectToAction("Design", new { ID = entity.ID });
            }
            var result = base.Edit(entity);
            if (entity.ActionType == Constant.ActionType.Publish)
            {
                Service.Publish(entity.ID);
            }
            return result;
        }
        [EditWidget]
        public ActionResult Design(string ID)
        {
            return View();
        }
        public ActionResult RedirectView(ParamsContext context)
        {
            return Redirect(Service.Get(context.ID).Url + "?ViewType=Review");
        }
        [PopUp]
        public ActionResult Select()
        {
            return View();
        }

        public ActionResult PageZones(QueryContext context)
        {
            Zone.ZoneService zoneService = new Zone.ZoneService();
            WidgetService widgetService = new WidgetService();

            var viewModel = new ViewModels.LayoutZonesViewModel
                {
                    PageID = context.PageID,
                    Zones = zoneService.GetZonesByPageId(context.PageID),
                    Widgets = widgetService.GetAllByPageId(context.PageID)
                };
            return View(viewModel);
        }
        [HttpPost]
        public JsonResult MovePage(string id, int position, int oldPosition)
        {
            Service.Move(id, position, oldPosition);
            return Json(true);
        }
    }
}

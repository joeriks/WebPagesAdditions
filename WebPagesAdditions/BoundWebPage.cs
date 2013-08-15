using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.ModelBinding;
using System.Web.WebPages;
using System.ComponentModel.DataAnnotations;

namespace WebPagesAdditions
{

    public class BoundWebPage : WebPage
    {

        public ModelBinderWrapper Binder;
        public ModelStateDictionary ModelState { get; set; }

        public BoundWebPage()
        {
        }

        public TModel Bind<TModel>(TModel model) where TModel : new()
        {
            if (model == null) model = new TModel();
            Binder = new ModelBinderWrapper(model);
            ModelState = Binder.ModelState;
            Binder.BindModel();
            return model;
        }

        bool tryInvoke(string methodName, object[] args, bool writeToResponse = false, bool endResponse = false)
        {
            object result;
            if (tryInvoke(methodName, args, out result))
            {
                if (writeToResponse) Response.Write(Json.Encode(result));
                if (endResponse) Response.End();
                return true;
            }
            else
            {
                return false;
            }
        }
        bool tryInvoke(string methodName, object[] args, out object result)
        {
            try
            {
                result = this.GetType().InvokeMember(methodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, this, args);
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }

        protected override void InitializePage()
        {
            base.InitializePage();

            var httpMethod = System.Web.HttpContext.Current.Request.HttpMethod;

            object result;

            tryInvoke("init", null, out result);

            if (UrlData.Count == 0 && httpMethod == "GET") return;

            var methodName = UrlData.Count == 0 ? "index" : UrlData[0].ToLower();
            var urlArgsAll = UrlData.Select<string, object>(itm => { int iItm; if (int.TryParse(itm, out iItm)) return iItm; return itm; }).ToArray();
            var urlArgsSkipFirst = UrlData.Skip(1).Select<string, object>(itm => { int iItm; if (int.TryParse(itm, out iItm)) return iItm; return itm; }).ToArray();

            var writeToResponse = (IsAjax || Request["isajax"] == "1");

            if (tryInvoke(methodName, urlArgsSkipFirst, writeToResponse, writeToResponse)) return;

            var methodName_httpMethod = methodName + "_" + httpMethod;
            if (tryInvoke(methodName_httpMethod, urlArgsSkipFirst, writeToResponse, writeToResponse)) return;

            if (tryInvoke(httpMethod, urlArgsAll, writeToResponse, writeToResponse)) return;


        }

        public override void Execute()
        {
        }

    }
}
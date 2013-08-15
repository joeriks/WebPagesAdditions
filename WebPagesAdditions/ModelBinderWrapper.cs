using System.Web.ModelBinding;
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;


namespace WebPagesAdditions
{

    public class ModelBinderWrapper
    {
        private DefaultModelBinder modelBinder;
        private ModelBindingContext bindingContext;
        private ModelBindingExecutionContext executingContext;
        private ModelStateDictionary modelState;

        public ModelStateDictionary ModelState
        {
            get
            {
                return modelState;
            }
        }
        public object Model
        {
            get
            {
                return bindingContext.Model;
            }
            set
            {
                setModel(value);
            }
        }

        private void setModel(object model)
        {
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, model.GetType());
            bindingContext.Model = model;
        }
        public void BindModel()
        {
            modelBinder.BindModel(executingContext, bindingContext);
        }
        public ModelBinderWrapper(object model = null)
        {
            modelBinder = new DefaultModelBinder();
            modelState = new ModelStateDictionary();

            var contextBase = new HttpContextWrapper(HttpContext.Current);
            executingContext = new ModelBindingExecutionContext(contextBase, modelState);

            bindingContext = new ModelBindingContext();
            bindingContext.ValueProvider = new FormValueProvider(executingContext);

            if (model != null)
            {
                setModel(model);
                BindModel();
            }

        }
        public string ValidationErrorMessage(string modelKey)
        {
            if (ModelState[modelKey] == null) return "";
            return string.Join(", ", ModelState[modelKey].Errors.Select(e => e.ErrorMessage));
        }
    }
}
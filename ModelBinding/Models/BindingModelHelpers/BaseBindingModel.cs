using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ModelBinding.Models.BindingModelHelpers;

namespace ModelBinding.Models
{

    public abstract class BaseBindingModel
    {
        protected BaseBindingModel(UrlHelper url, HttpRequestBase request, IGuidIdentifiedEntity entity)
        {
            Url = url;
            Request = request;
            Entity = entity;
        }
        protected readonly UrlHelper Url;
        protected readonly HttpRequestBase Request;
        protected IGuidIdentifiedEntity Entity;

        public abstract EditFormModel GetModel(object customObject = null);

        public virtual string SaveModel(bool isNew)
        {
            return null;
        }

        public Dictionary<string, object> NewItemReturnData()
        {
            var dict = new Dictionary<string, object>();
            FillNewItemReturnData(dict);
            return dict;
        }

        protected virtual void FillNewItemReturnData(Dictionary<string, object> dict)
        {
        }


        public virtual void SetWarning(EditFormModel r, IGuidIdentifiedEntity item)
        {
        }
    }
}

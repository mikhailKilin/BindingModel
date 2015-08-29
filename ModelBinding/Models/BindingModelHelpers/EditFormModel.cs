using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBinding.Models.BindingModelHelpers
{
    public class EditFormModel
    {
        public EditFormModel(object id, string type)
        {
            Items = new List<EditFormItemProp>();
            ChildGrids = new List<KendoGridModel>();
            Id = id;
            Type = type;
            ViewDataParams = new Dictionary<string, object>();
        }
        public object Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public List<EditFormItemProp> Items { get; set; }
        public List<KendoGridModel> ChildGrids { get; set; }
        public ToolbarModel Toolbar { get; set; }
        public Dictionary<string, object> ViewDataParams { get; set; }
        public string Warning { get; set; }//отвечает за вывод уведомления, например, что ввод данных заблокирован
    }
    
    public class EditFormItemProp
    {
        public EditFormItemProp(string type)
        {
            Editable = true;
            Type = type;
        }

        public string Type { get; private set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public bool Editable { get; set; }
        public bool Required { get; set; }
        public bool Hidden { get; set; }
        public object Value { get; set; }
        public KendoGridModel Grid { get; set; }
        public string OnChange { get; set; }
    }
    public class KendoGridModel
    {
        public KendoGridModel()
        {
            Id = "grid";
            AutoBind = true;
            toolbar = new List<object>();
            editable = new Dictionary<string, string> { { "createAt", "bottom" } };
            dateFields = new List<string>();
            childGrids = new List<KendoGridModel>();
        }

        public string Id { get; set; }
        public bool AutoBind { get; set; }
        public Dictionary<string, string> returnAdditional { get; set; }
        public List<object> toolbar { get; set; }
        public bool serverpaging { get; set; }
        public List<object> @group { get; set; }
        public List<object> sort { get; set; }
        public List<object> columns { get; set; }
        public Call create { get; set; }
        public Call destroy { get; set; }
        public Call read { get; set; }
        public Call update { get; set; }
        public Schema schema { get; set; }
        public object editable { get; set; }
        public int? height { get; set; }
        public int? width { get; set; }
        public int? minWidth { get; set; }
        public List<string> dateFields { get; set; }
        public string extendedEditCellFunction { get; set; }
        public bool Scrollable { get; set; }
        public List<KendoGridModel> childGrids { get; set; }
        public object filterable { get; set; }
        public object dictUrl { get; set; }
        public object sortable { get; set; }

        //TODO multicolumnheaders, пока так, в свежей библотеки Kendo уже есть стандартный функционал
        public string templateMulticolumnHeader { get; set; }
    }
    public interface IJsonStringify
    {
    }
    public class Call : IJsonStringify
    {
        public Call()
        {
            dataType = "json";
            type = "POST";
        }

        public string url { get; set; }
        public string dataType { get; set; }
        public string type { get; set; }
    }
    public class Schema : IJsonStringify
    {
        public string data { get; set; }
        public string total { get; set; }
        public Model model { get; set; }
    }
    public class Model : IJsonStringify
    {
        public string id { get; set; }
        public Dictionary<string, ModelFieldObject> fields { get; set; }
    }
    public class ModelFieldObject : IJsonStringify
    {
        public ModelFieldObject()
        {
            type = KGMFType.String.ToString().ToLower();
            editable = true;
        }

        public bool? editable { get; set; }
        public bool? nullable { get; set; }
        public string type { get; set; }

        /// <summary>
        /// Specifies the which will be used for the field when a new model instance is created. Default settings depend on the type of the field. Default for "string" is "", for "number" is 0 and for "date" is new Date() (today).
        /// </summary>
        public object defaultValue { get; set; }

        public ModelFieldValidation validation { get; set; }
    }
    public enum KGMFType
    {
        Number,
        String,
        Text,
        Date,
        DateTime,
        Year,
        Boolean
    }
    public class ModelFieldValidation : IJsonStringify
    {
        public bool? required { get; set; }
        public int? min { get; set; }
    }
}

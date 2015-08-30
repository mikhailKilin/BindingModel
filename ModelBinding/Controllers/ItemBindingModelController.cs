using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelBinding.Models.BindingModelHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ModelDataLayer;
using ModelDataLayer.Extensions;
using IGuidIdentifiedEntity = ModelBinding.Models.BindingModelHelpers.IGuidIdentifiedEntity;

namespace ModelBinding.Controllers
{
    public class ItemBindingModelController : Controller
    {
        // GET: ItemBindingModel
        public ActionResult Index()
        {
            return View();
        }
    }
    /*public class ItemPassportController : Controller
	{
		private static readonly Dictionary<string, Type> Types = new Dictionary<string, Type>();

		public ActionResult GetModel(ExecuteParams executeParams, FormCollection values, string modelName = null)
		{
			using (var ctx = new MainModelContainer())
			{
			    EfExtensions.CreateEntityByType(ctx, executeParams.Type, false);
                var itemId = ModelExt.ToGuid(executeParams.Id);
                var item = itemId == Guid.Empty
                    ? ctx.CreateEntityByType(executeParams.treeElType, false)
                    : (IGuidIdentifiedEntity)ctx.GetEntityByTypeAndId(executeParams.treeElType, itemId);
                //modelName возможность самому задавать имя модели
                if (!String.IsNullOrEmpty(modelName))
                    executeParams.treeElType = modelName;

                var passportModel = GetPassportModel(executeParams.Type, item, ctx);
				var model = passportModel.GetModel(customItemId);

				return model == null ? null : System.Web.UI.WebControls.View("~/Views/Shared/Kendo/EditForm/Form.ascx", model);
			}
		}

		private BasePassportModel GetPassportModel(string type, IGuidIdentifiedEntity entity, GovProgramsModelContainer ctx)
		{
			var t = GetType(type, false);
			var instance = (BasePassportModel) Activator.CreateInstance(t, new object[] {Url, Request, entity, ctx});
			return instance;
		}

		private Type GetType(string tName, bool model = true)
		{
			Type t;
			lock (Types)
			{
				if (Types.ContainsKey(tName))
					t = Types[tName];
				else
				{
					t = model
						? ModelAsm.GetType(ModelAsm.GetName().Name + "." + tName, false, true)
						: WebAsm.GetType(WebAsm.GetName().Name + ".Controllers.DataInput.EditForms.Models." + tName + "Model", false, true);
					Types.Add(tName, t);
				}
			}
			return t;
		}

		[HttpPost]
		public ActionResult Execute()
		{
			var req = Request.InputStream;
			req.Seek(0, SeekOrigin.Begin);
			var json = new StreamReader(req).ReadToEnd();
			var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

			try
			{
				var type = values["type"] as string;
				var id = values["id"].ToGuid();

				using (var ctx = GpContextWrapper.NewContext())
				{
					var item = DataTypesExtensions.IsEmpty(id) ? ctx.CreateEntityByType(type) : (IGuidIdentifiedEntity)ctx.GetEntityByTypeAndId(type, id);
					if (item == null) return null;
					var itemType = item.GetType();
					var data = ((JObject) values["data"]).ToObject<Dictionary<string, object>>();
					foreach (var dataItem in data)
					{
						if (dataItem.Key.StartsWith("mchoice_"))
						{
							var mmTypeName = dataItem.Key.Replace("mchoice_", "");
							var mmType = GetType(mmTypeName);

							var jObj = dataItem.Value as JObject;
							if (jObj == null) continue;
							var mmTypeObj = jObj.ToObject<MmTypeObject>();
							if (mmTypeObj == null) continue;

							foreach (var oldGuid in mmTypeObj.OldItems)
							{
								var oldMm = ctx.GetEntityByTypeAndId(mmTypeName, oldGuid);
								if (oldMm == null) continue;
								ctx.DeleteObject(oldMm);
							}

							var mmEntityProp = mmType.GetProperty(mmTypeObj.PropName);
							if (mmEntityProp == null) continue;
							var mmEntityLinkedToProp = mmType.GetProperty(mmTypeObj.LinkedPropName);
							if (mmEntityLinkedToProp == null) continue;

							foreach (var newItem in mmTypeObj.NewItems)
							{
                                // исключение дублирования newItem при повторном сохранении без обновления
                                var objectSet = ctx.GetEnumerationByTypeName<object>(mmTypeName);
                                var existItem = Enumerable.Aggregate(objectSet, false,
                                    (current, t) =>
                                        current ||
                                        (t.GetPropertyValue(mmTypeObj.LinkedPropName).ToGuid() == newItem.Id
                                        && t.GetPropertyValue(mmTypeObj.PropName).ToGuid() == item.Id
                                        && !mmTypeObj.OldItems.Contains(t.GetPropertyValue("Id").ToGuid())));
                                if (existItem) continue;
								var obj = Activator.CreateInstance(mmType) as IGuidIdentifiedEntity;
								if (obj == null) continue;
								obj.Id = Guid.NewGuid();
								mmEntityProp.SetValue(obj, item.Id);
								mmEntityLinkedToProp.SetValue(obj, newItem.Id);
								ctx.AddObject(obj);
							}
							continue;
						}
						if (dataItem.Key.StartsWith("onechoice_"))
						{
							var typeName = dataItem.Key.Replace("onechoice_", "");
							var oneType = GetType(typeName);

							var jObj = dataItem.Value as JObject;
							if (jObj == null) continue;
							var typeObj = jObj.ToObject<OneTypeObject>();
							if (typeObj == null) continue;

							if (typeObj.OldItem != null)
							{
								var old = ctx.GetEntityByTypeAndId(typeName, (Guid)typeObj.OldItem);
								if (old == null) continue;
								ctx.DeleteObject(old);
							}


							var entityProp = oneType.GetProperty(typeObj.PropName);
							if (entityProp == null) continue;
							var entityLinkedToProp = oneType.GetProperty(typeObj.LinkedPropName);
							if (entityLinkedToProp == null) continue;

							var obj = Activator.CreateInstance(oneType) as IGuidIdentifiedEntity;
							if (obj == null || typeObj.Value.Id.Equals(Guid.Empty)) continue;
							obj.Id = Guid.NewGuid();
							entityProp.SetValue(obj, item.Id);
							entityLinkedToProp.SetValue(obj, typeObj.Value.Id);
							ctx.AddObject(obj);
							
							continue;
						}
						if (dataItem.Key == "multimchoice")
                        {
                            var jobject = dataItem.Value as JObject;
                            var jObjectList = jobject.ToObject<MmTypeObjectList>();
                            if (jObjectList == null) continue;
                            foreach (var jItem in jObjectList.Values)
                            {
                                var mmTypeName = jItem.Entity;
                                var mmType = GetType(mmTypeName);
                                if (jItem.OldItems != null && jItem.OldItems.Any())
                                {
                                    foreach (var oldGuid in jItem.OldItems)
                                    {
                                        var oldMm = ctx.GetEntityByTypeAndId(mmTypeName, oldGuid);
                                        if (oldMm == null) continue;
                                        ctx.DeleteObject(oldMm);
                                    }
                                }
                                var mmEntityProp = mmType.GetProperty(jItem.PropName);
                                if (mmEntityProp == null) continue;
                                var mmEntityLinkedToProp = mmType.GetProperty(jItem.LinkedPropName);
                                if (mmEntityLinkedToProp == null) continue;

                                foreach (var currEntity in jObjectList.Value.Where(q => q.Type == mmTypeName))
                                {
                                    var obj = Activator.CreateInstance(mmType) as IGuidIdentifiedEntity;
                                    if (obj == null) continue;
                                    obj.Id = Guid.NewGuid();
                                    mmEntityProp.SetValue(obj, item.Id);
                                    mmEntityLinkedToProp.SetValue(obj, currEntity.Id);
                                    ctx.AddObject(obj);
                                }
                                //foreach (var newItem in jItem.NewItems)
                                //{
                                //    var obj = Activator.CreateInstance(mmType) as IGuidIdentifiedEntity;
                                //    if (obj == null) continue;
                                //    obj.Id = Guid.NewGuid();
                                //    mmEntityProp.SetValue(obj, item.Id);
                                //    mmEntityLinkedToProp.SetValue(obj, newItem.Id);
                                //    ctx.AddObject(obj);
                                //}

                            }
                            continue;
                        }
                        if (dataItem.Key.StartsWith("OtherString"))
                        {
                            var jobject = dataItem.Value as JObject;
                            var jItem = jobject.ToObject<OtherStringObject>();
                            if (jItem != null)
                            {
                                var Type = GetType(jItem.Val.Type);
                                var EntityProp = Type.GetProperty(jItem.Val.PropName);
                                var sourceId = jItem.Val.Id;
                                var sourceObj = ctx.GetEntityByTypeAndId(Type.Name, sourceId);
                                EntityProp.SetValue(sourceObj,jItem.Value);
                            }
                            continue;
                        }

						var propName = dataItem.Key.Replace("choice_", "");
						var itemProp = itemType.GetProperty(propName);
						if (itemProp == null) continue;

						object value;
						if (dataItem.Key.StartsWith("choice_"))
						{
							var choiceObj = dataItem.Value.ToChoiceObj();
							value = choiceObj == null ? null : (object) choiceObj.Id;
						}
						else
						{
							try
							{
								var converter = TypeDescriptor.GetConverter(itemProp.PropertyType);
								value = converter.ConvertFrom(dataItem.Value);
							}
							catch (Exception)
							{
								value = dataItem.Value;
							}
						}

						itemProp.SetValue(item, value);
					}

					var passportModel = GetPassportModel(type, item, ctx);
					var error = passportModel.SaveModel(DataTypesExtensions.IsEmpty(id));
					if (error != null)
					{
						return Json(new { status = "fail", error = error });
					}
					ctx.SaveChanges();

					//перегружаем кэш после добавление ГП. 
					if (id.Equals(Guid.Empty) && item is GovernmentProgram)
						RightsRepository.Reload(NfbrMembership.CurrentUser.UserName);
					

					return Json(new { status = "OK", editFormItemId = item.Id, item = passportModel.NewItemReturnData() });
				}
			}
			catch (Exception e)
			{
				Log.Exception(new Exception("ItemPassport: error occured when tried to save data", e));
				return null;
			}
		}

		// ReSharper disable ClassNeverInstantiated.Local
		// ReSharper disable UnusedAutoPropertyAccessor.Local
		private class MmTypeObject
		{
			public string PropName { get; set; }
			public string LinkedPropName { get; set; }
			public List<Guid> OldItems { get; set; }
			public List<ChoiceObject> NewItems { get; set; }
		}
		private class OneTypeObject : MmTypeObject
		{
			public Guid? OldItem { get; set; }
			public ChoiceObject Value { get; set; }
		}
        private class MmTypeObjectWithEntity : MmTypeObject
        {
            public string Entity { get; set; }
        }
        private class MmTypeObjectList
        {
            public List<MmTypeObjectWithEntity> Values { get; set; }
            public List<ChoiceObject> Value { get; set; }
        }
        private class OtherStringObject
        {
            public string Value { get; set; }
            public OtherStringEntity Val { get; set; }
        }
        private class OtherStringEntity
        {
            public Guid Id { get; set; } 
            public string PropName { get; set; }
	        public string Type { get; set; }
        }

	}*/
}
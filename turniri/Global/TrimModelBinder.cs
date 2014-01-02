using System;
using System.Linq;
using System.Web.Mvc;

public class TrimModelBinder : DefaultModelBinder
{
    //Trim all fields 
    protected override void SetProperty(ControllerContext controllerContext,
        ModelBindingContext bindingContext,
        System.ComponentModel.PropertyDescriptor propertyDescriptor,
        object value)
    {
        if (propertyDescriptor.PropertyType == typeof(string))
        {
            var stringValue = (string)value;
            if (!string.IsNullOrEmpty(stringValue))
            {
                stringValue = stringValue.Trim();
            }
            value = stringValue;
        }
        base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
    }

    /// <summary>
    /// Find inheritance of abstract class by ModelTypeName
    /// </summary>
    /// <param name="controllerContext"></param>
    /// <param name="bindingContext"></param>
    /// <returns></returns>
    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        var modelType = bindingContext.ModelType;
        if (modelType.IsAbstract)
        {
            var modelTypeValue = controllerContext.Controller.ValueProvider.GetValue("ModelTypeName");
            if (modelTypeValue == null)
            {
                throw new Exception("View does not contain ModelTypeName");
            }
            var modelTypeName = modelTypeValue.AttemptedValue;

            var type = modelType.Assembly.GetTypes().SingleOrDefault(x => x.IsSubclassOf(modelType) && x.Name == modelTypeName);

            if (type != null)
            {
                var instance = bindingContext.Model ?? base.CreateModel(controllerContext, bindingContext, type);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => instance, type);
            }
        }
        return base.BindModel(controllerContext, bindingContext);
    }
}
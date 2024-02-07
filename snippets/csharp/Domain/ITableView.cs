public interface ITableView<TView, TModel>
    where TModel : new()
    where TView : ITableView<TView, TModel>
{
    public static abstract explicit operator TModel(TView other);

    private static readonly IEnumerable<PropertyAttribute<CopyToPropertyAttribute<TModel>>> ViewProperties = PropertyAttribute<CopyToPropertyAttribute<TModel>>.GetAllPropertiesInClass<TView>();


    public TModel CastToModel()
    {
        TModel result = new();

        foreach (var property in ViewProperties)
        {
            var value = property.GetPropertyValueRaw(this);
            var modelPropertyName = property.Attribute.Name;
            result.GetType()?.GetProperty(modelPropertyName)?.SetValue(result, value);
        }

        return result;
    }
}
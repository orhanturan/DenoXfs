using System.ComponentModel;
using DenoXfs.TestTool.Settings;

namespace DenoXfs.TestTool.Ui;

/// <summary>
/// Drop down of the logical services in LOGICAL_SERVICES for the PropertyGrid. Only those names can be picked; when
/// none can be read the name stays free text.
/// </summary>
public sealed class LogicalNameConverter : StringConverter
{
    private readonly ILogicalServiceCatalog _catalog;

    public LogicalNameConverter(ILogicalServiceCatalog catalog)
    {
        _catalog = catalog;
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext? context) => true;

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext? context) => _catalog.All().Count > 0;

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext? context) =>
        new(_catalog.All().Select(s => s.Name).ToArray());
}

/// <summary>A property shown with another converter; reads and writes go to the original property.</summary>
public sealed class ConvertedProperty : PropertyDescriptor
{
    private readonly PropertyDescriptor _inner;
    private readonly TypeConverter _converter;

    public ConvertedProperty(PropertyDescriptor inner, TypeConverter converter)
        : base(inner)
    {
        _inner = inner;
        _converter = converter;
    }

    public override TypeConverter Converter => _converter;

    public override Type ComponentType => _inner.ComponentType;

    public override bool IsReadOnly => _inner.IsReadOnly;

    public override Type PropertyType => _inner.PropertyType;

    public override bool CanResetValue(object component) => _inner.CanResetValue(component);

    public override object? GetValue(object? component) => _inner.GetValue(component);

    public override void ResetValue(object component) => _inner.ResetValue(component);

    public override void SetValue(object? component, object? value) => _inner.SetValue(component, value);

    public override bool ShouldSerializeValue(object component) => _inner.ShouldSerializeValue(component);
}

using System.ComponentModel;

namespace DenoXfs.TestTool.Ui;

/// <summary>
/// Shows only some properties of an object in a PropertyGrid. Edits go to the real object, and the property types keep
/// their editors and converters (e.g. <see cref="FlagsEnumEditor"/>).
/// </summary>
public sealed class PropertySubset : CustomTypeDescriptor
{
    private readonly object _owner;
    private readonly PropertyDescriptorCollection _properties;

    public PropertySubset(object owner, IEnumerable<PropertyDescriptor> properties)
        : base(TypeDescriptor.GetProvider(owner).GetTypeDescriptor(owner))
    {
        _owner = owner;
        _properties = new PropertyDescriptorCollection(properties.ToArray(), readOnly: true);
    }

    public override PropertyDescriptorCollection GetProperties() => _properties;

    public override PropertyDescriptorCollection GetProperties(Attribute[]? attributes) => _properties;

    public override object? GetPropertyOwner(PropertyDescriptor? pd) => _owner;
}

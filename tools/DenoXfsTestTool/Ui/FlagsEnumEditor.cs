using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace DenoXfs.TestTool.Ui;

/// <summary>
/// PropertyGrid editor for <see cref="FlagsAttribute"/> enums: a drop down with one check box per flag, so several
/// flags can be combined (e.g. Track1 + Track2 + Chip). The grid's default editor allows only one value.
/// </summary>
public sealed class FlagsEnumEditor : UITypeEditor
{
    private static readonly HashSet<Type> Registered = [];

    /// <summary>Attaches this editor to every flags enum used by a property of <paramref name="parametersType"/>.</summary>
    public static void RegisterFor(Type parametersType)
    {
        foreach (var property in parametersType.GetProperties())
        {
            var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
            if (type.IsEnum && type.IsDefined(typeof(FlagsAttribute), false) && Registered.Add(type))
                TypeDescriptor.AddAttributes(type, new EditorAttribute(typeof(FlagsEnumEditor), typeof(UITypeEditor)));
        }
    }

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext? context) => UITypeEditorEditStyle.DropDown;

    public override object? EditValue(ITypeDescriptorContext? context, IServiceProvider provider, object? value)
    {
        if (value is not Enum current || provider.GetService(typeof(IWindowsFormsEditorService)) is not IWindowsFormsEditorService service)
            return value;

        var type = current.GetType();
        var bits = Convert.ToUInt64(current);
        var list = new CheckedListBox { CheckOnClick = true, BorderStyle = BorderStyle.None };
        foreach (var flag in Enum.GetValues(type).Cast<Enum>().Where(f => Convert.ToUInt64(f) != 0).Distinct())
        {
            var flagBits = Convert.ToUInt64(flag);
            list.Items.Add(flag, (bits & flagBits) == flagBits);
        }
        list.Height = Math.Min(list.Items.Count, 16) * list.ItemHeight + 4;

        service.DropDownControl(list);

        ulong result = 0;
        foreach (Enum flag in list.CheckedItems)
            result |= Convert.ToUInt64(flag);
        return Enum.ToObject(type, result);
    }
}

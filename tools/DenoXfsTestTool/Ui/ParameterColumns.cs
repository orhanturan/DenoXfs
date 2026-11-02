using System.ComponentModel;

namespace DenoXfs.TestTool.Ui;

/// <summary>
/// The parameters of an operation in side by side PropertyGrids: up to <see cref="RowsPerColumn"/> rows per column and
/// up to <see cref="MaxColumns"/> columns (15 parameters: 5 | 5 | 5). Beyond that the columns get more rows
/// (25 parameters: 9 | 9 | 7), so the input strip stays low.
/// </summary>
public sealed class ParameterColumns : TableLayoutPanel
{
    public const int RowsPerColumn = 5;
    public const int MaxColumns = 3;

    public ParameterColumns(object parameters)
    {
        Dock = DockStyle.Fill;
        var properties = TypeDescriptor.GetProperties(parameters, [BrowsableAttribute.Yes]).Cast<PropertyDescriptor>().ToList();
        var columnCount = Math.Min(MaxColumns, (properties.Count + RowsPerColumn - 1) / RowsPerColumn);
        var rowsPerColumn = columnCount == 0 ? 1 : (properties.Count + columnCount - 1) / columnCount;
        var slices = properties.Chunk(rowsPerColumn).ToList();

        ColumnCount = Math.Max(slices.Count, 1);
        RowCount = 1;
        RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        for (var i = 0; i < slices.Count; i++)
        {
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / slices.Count));
            Controls.Add(new PropertyGrid
            {
                Dock = DockStyle.Fill,
                HelpVisible = false,
                ToolbarVisible = false,
                PropertySort = PropertySort.NoSort, // declaration order, no "Misc" category row
                SelectedObject = new PropertySubset(parameters, slices[i])
            }, i, 0);
        }

        ParameterRows = slices.Count == 0 ? 0 : slices.Max(s => s.Length);
    }

    /// <summary>Rows of the highest column.</summary>
    public int ParameterRows { get; }
}

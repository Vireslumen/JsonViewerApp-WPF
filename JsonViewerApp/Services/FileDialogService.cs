using JsonViewerApp.Interfaces;
using Microsoft.Win32;

namespace JsonViewerApp.Services;

/// <summary>
///     Реализация <see cref="IFileDialogService" />, предоставляющая методы для открытия диалоговых окон выбора файлов.
/// </summary>
public class FileDialogService : IFileDialogService
{
    /// <inheritdoc />
    public string OpenFile(string filter)
    {
        var dialog = new OpenFileDialog
        {
            Filter = filter
        };
        return dialog.ShowDialog() == true ? dialog.FileName : string.Empty;
    }
}
using System.Windows;
using JsonViewerApp.Interfaces;

namespace JsonViewerApp.Services;

/// <summary>
///     Реализация <see cref="IClipboardService" />, работающая с системным буфером обмена.
/// </summary>
public class ClipboardService : IClipboardService
{
    /// <inheritdoc />
    public void SetText(string text)
    {
        if (!string.IsNullOrEmpty(text)) Clipboard.SetText(text);
    }
}
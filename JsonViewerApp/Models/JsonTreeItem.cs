using System.Collections.ObjectModel;
using System.ComponentModel;

namespace JsonViewerApp.Models;

/// <summary>
///     Модель данных для представления элемента дерева JSON.
///     Включает свойства для хранения уровня, имени, значения, дочерних элементов и родителя.
/// </summary>
public class JsonTreeItem : INotifyPropertyChanged
{
    private bool _isExpanded;
    private bool _isSelected;
    private int _level;
    private ObservableCollection<JsonTreeItem> _children = new();
    private string _name = string.Empty;
    private string _value = string.Empty;

    public JsonTreeItem()
    {
        Children = new ObservableCollection<JsonTreeItem>();
    }

    /// <summary>
    ///     Указывает, является ли элемент массивом JSON.
    /// </summary>
    public bool IsArray { get; set; }
    /// <summary>
    ///     Определяет, выбран ли элемент в UI.
    /// </summary>
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
        }
    }
    /// <summary>
    ///     Определяет, раскрыт ли элемент в UI.
    /// </summary>
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded == value) return;
            _isExpanded = value;
            OnPropertyChanged(nameof(IsExpanded));
        }
    }
    /// <summary>
    ///     Уровень вложенности элемента в дереве.
    /// </summary>
    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            OnPropertyChanged(nameof(Level));
        }
    }
    /// <summary>
    ///     Родительский элемент в дереве JSON.
    /// </summary>
    public JsonTreeItem? Parent { get; set; }
    /// <summary>
    ///     Коллекция дочерних элементов.
    /// </summary>
    public ObservableCollection<JsonTreeItem> Children
    {
        get => _children;
        set
        {
            _children = value;
            OnPropertyChanged(nameof(Children));
        }
    }
    /// <summary>
    ///     Имя элемента JSON (ключ).
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
    /// <summary>
    ///     Значение элемента JSON.
    /// </summary>
    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            OnPropertyChanged(nameof(Value));
        }
    }
    /// <summary>
    ///     Событие, вызываемое при изменении свойства.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    ///     Создаёт копию текущего элемента <see cref="JsonTreeItem" />, исключая дочерние элементы.
    /// </summary>
    /// <returns>Новая копия текущего элемента <see cref="JsonTreeItem" /> без дочерних элементов.</returns>
    public JsonTreeItem CloneWithoutChildren()
    {
        return new JsonTreeItem
        {
            Name = Name,
            Value = Value,
            IsArray = IsArray,
            Level = Level,
            Parent = Parent,
            Children = new ObservableCollection<JsonTreeItem>()
        };
    }

    /// <summary>
    ///     Вызывает событие изменения свойства для указанного имени свойства.
    /// </summary>
    /// <param name="propertyName">Имя изменённого свойства.</param>
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
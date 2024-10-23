using System.Collections.Generic;
using System.Windows.Input;
using JsonViewerApp.Commands;
using JsonViewerApp.Models;

namespace JsonViewerApp.ViewModels;

/// <summary>
///     Абстрактный базовый класс для моделей представления, реализующих функциональность поиска.
///     Содержит общую логику поиска, навигации по результатам и управление состоянием плейсхолдера.
/// </summary>
public abstract class SearchViewModelBase : BaseViewModel
{
    /// <summary>
    ///     Текущий индекс найденного совпадения.
    /// </summary>
    protected int CurrentMatchIndex;

    /// <summary>
    ///     Список всех найденных совпадений.
    /// </summary>
    protected List<JsonTreeItem> AllMatches = new();

    private bool _hasSearched;
    private bool _isPlaceholderActive = true;
    private int _totalMatches;
    private string? _searchQuery;

    protected SearchViewModelBase()
    {
        SearchCommand = new RelayCommand(ExecuteSearch, CanExecuteSearch);
        NextSearchResultCommand = new RelayCommand(ExecuteNextSearchResult, CanExecuteNavigation);
        PreviousSearchResultCommand = new RelayCommand(ExecutePreviousSearchResult, CanExecuteNavigation);
        GotFocusCommand = new RelayCommand(OnSearchBoxFocus);
        LostFocusCommand = new RelayCommand(OnSearchBoxLostFocus);
    }

    /// <summary>
    ///     Указывает, был ли выполнен поиск.
    /// </summary>
    public bool HasSearched
    {
        get => _hasSearched;
        protected set
        {
            _hasSearched = value;
            OnPropertyChanged(nameof(HasSearched));
        }
    }
    /// <summary>
    ///     Указывает, должна ли кнопка "Искать" быть видимой.
    /// </summary>
    public bool IsSearchButtonVisible => !string.IsNullOrEmpty(SearchQuery);
    /// <summary>
    ///     Указывает, должны ли кнопки навигации по результатам быть видимыми.
    /// </summary>
    public bool AreNavigationButtonsVisible => TotalMatches > 1;
    /// <summary>
    ///     Команда для обработки получения фокуса полем поиска.
    /// </summary>
    public ICommand GotFocusCommand { get; }
    /// <summary>
    ///     Команда для обработки потери фокуса полем поиска.
    /// </summary>
    public ICommand LostFocusCommand { get; }
    /// <summary>
    ///     Команда для выполнения поиска.
    /// </summary>
    public ICommand SearchCommand { get; }
    /// <summary>
    ///     Команда для перехода к следующему результату поиска.
    /// </summary>
    public ICommand NextSearchResultCommand { get; }
    /// <summary>
    ///     Команда для перехода к предыдущему результату поиска.
    /// </summary>
    public ICommand PreviousSearchResultCommand { get; }
    /// <summary>
    ///     Текущий номер совпадения.
    /// </summary>
    public int CurrentMatch => CurrentMatchIndex + 1;
    /// <summary>
    ///     Общее количество найденных совпадений.
    /// </summary>
    public int TotalMatches
    {
        get => _totalMatches;
        protected set
        {
            _totalMatches = value;
            OnPropertyChanged(nameof(TotalMatches));
            OnPropertyChanged(nameof(AreNavigationButtonsVisible));
        }
    }
    /// <summary>
    ///     Текст плейсхолдера для поля поиска.
    /// </summary>
    public string PlaceholderText => "Введите текст для поиска";
    /// <summary>
    ///     Текущий поисковый запрос.
    /// </summary>
    public string? SearchQuery
    {
        get => _isPlaceholderActive ? PlaceholderText : _searchQuery;
        set
        {
            if (_searchQuery == value) return;
            _searchQuery = value;
            OnPropertyChanged(nameof(SearchQuery));
            OnPropertyChanged(nameof(IsSearchButtonVisible));
        }
    }

    /// <summary>
    ///     Проверяет, можно ли выполнить навигацию по результатам поиска.
    /// </summary>
    /// <returns>Возвращает true, если доступно более одного совпадения.</returns>
    protected virtual bool CanExecuteNavigation()
    {
        return AllMatches is {Count: > 1};
    }

    /// <summary>
    ///     Проверяет, можно ли выполнить поиск.
    /// </summary>
    /// <returns>Возвращает true, если поисковый запрос не пустой.</returns>
    protected virtual bool CanExecuteSearch()
    {
        return !string.IsNullOrEmpty(SearchQuery);
    }

    /// <summary>
    ///     Переходит к следующему результату поиска.
    /// </summary>
    protected virtual void ExecuteNextSearchResult()
    {
        if (AllMatches is not {Count: > 0}) return;
        CurrentMatchIndex = (CurrentMatchIndex + 1) % AllMatches.Count;
        OnCurrentMatchChanged();
    }

    /// <summary>
    ///     Переходит к предыдущему результату поиска.
    /// </summary>
    protected virtual void ExecutePreviousSearchResult()
    {
        if (AllMatches is not {Count: > 0}) return;
        CurrentMatchIndex = (CurrentMatchIndex - 1 + AllMatches.Count) % AllMatches.Count;
        OnCurrentMatchChanged();
    }

    /// <summary>
    ///     Выполняет поиск.
    /// </summary>
    protected virtual void ExecuteSearch()
    {
        PerformSearch();
        HasSearched = true;
        OnPropertyChanged(nameof(CurrentMatch));
    }

    /// <summary>
    ///     Вызывается при изменении текущего совпадения.
    /// </summary>
    protected virtual void OnCurrentMatchChanged()
    {
        OnPropertyChanged(nameof(CurrentMatch));
    }

    /// <summary>
    ///     Обрабатывает событие получения фокуса полем поиска.
    /// </summary>
    protected virtual void OnSearchBoxFocus()
    {
        if (!_isPlaceholderActive) return;
        SearchQuery = string.Empty;
        _isPlaceholderActive = false;
        OnPropertyChanged(nameof(SearchQuery));
    }

    /// <summary>
    ///     Обрабатывает событие потери фокуса полем поиска.
    /// </summary>
    protected virtual void OnSearchBoxLostFocus()
    {
        if (!string.IsNullOrEmpty(SearchQuery)) return;
        _isPlaceholderActive = true;
        SearchQuery = PlaceholderText;
        OnPropertyChanged(nameof(SearchQuery));
    }

    /// <summary>
    ///     Абстрактный метод для выполнения конкретной логики поиска в наследуемых классах.
    /// </summary>
    protected abstract void PerformSearch();
}
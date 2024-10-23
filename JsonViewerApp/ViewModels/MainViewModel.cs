using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using JsonViewerApp.Commands;
using JsonViewerApp.Factories;
using JsonViewerApp.Interfaces;
using JsonViewerApp.Models;

namespace JsonViewerApp.ViewModels;

/// <summary>
///     ViewModel для главного окна, отвечающая за загрузку и отображение JSON данных.
/// </summary>
public class MainViewModel : SearchViewModelBase
{
    private const double MaxFileSizeInMegaByte = 500;
    private const int ProgressMaxValue = 100;
    private readonly IClipboardService _clipboardService;
    private readonly IFileDialogService _fileDialogService;
    private readonly IFileService _fileService;
    private readonly IJsonParser _jsonParser;
    private readonly INavigationService _navigationService;
    private readonly IProgressUpdater _progressUpdater;
    private readonly ISearchService _searchService;
    private readonly ISearchViewModelFactory _searchViewModelFactory;
    private readonly ISpeedManager _speedManager;
    private readonly SynchronizationContext _synchronizationContext;
    private bool _isLoadingComplete;
    private bool _isTreeExpanded;
    private bool _isVirtualizationEnabled = true;
    private double _progress;
    private ObservableCollection<JsonTreeItem> _jsonTreeItems = new();
    private string? _statusMessage;

    public MainViewModel(IJsonParser jsonParser, INavigationService navigationService, ISearchService searchService, ISpeedManager speedManager,
        IProgressUpdater progressUpdater, IFileService fileService, ISearchViewModelFactory searchViewModelFactory,
        IClipboardService clipboardService, IFileDialogService fileDialogService)
    {
        _jsonParser = jsonParser;
        _navigationService = navigationService;
        _searchService = searchService;
        _speedManager = speedManager;
        _progressUpdater = progressUpdater;
        _fileService = fileService;
        _searchViewModelFactory = searchViewModelFactory;
        _synchronizationContext = SynchronizationContext.Current;
        _clipboardService = clipboardService;
        _fileDialogService = fileDialogService;

        CopyNameCommand = new RelayCommand<string>(CopyToClipboard);
        CopyValueCommand = new RelayCommand<string>(CopyToClipboard);
        CopyNameAndValueCommand = new RelayCommand<JsonTreeItem>(item => CopyToClipboard($"{item.Name}: {item.Value}"));
        OpenFileCommand = new AsyncRelayCommand(OpenJsonFileAsync);
        ClearCommand = new RelayCommand(ClearTreeView);
        OpenSearchWindowCommand = new RelayCommand(OpenSearchWindow);
        ToggleVirtualizationCommand = new RelayCommand(ToggleVirtualization);
        ToggleExpandCollapseCommand = new RelayCommand(ToggleExpandCollapse);
    }

    /// <summary>
    ///     Указывает, включена ли виртуализация.
    /// </summary>
    public bool IsVirtualizationEnabled
    {
        get => _isVirtualizationEnabled;
        set
        {
            if (_isVirtualizationEnabled == value) return;
            _isVirtualizationEnabled = value;
            OnPropertyChanged(nameof(IsVirtualizationEnabled));
            OnPropertyChanged(nameof(AreSearchControlsVisible));
            OnPropertyChanged(nameof(VirtualizationButtonText));
        }
    }
    /// <summary>
    ///     Указывает, должны ли быть видимы элементы управления поиском.
    /// </summary>
    public bool AreSearchControlsVisible => !IsVirtualizationEnabled && _jsonTreeItems.Any();
    /// <summary>
    ///     Указывает, должна ли быть видима кнопка открытия окна поиска.
    /// </summary>
    public bool IsOpenSearchWindowButtonVisible => _jsonTreeItems.Any();
    /// <summary>
    ///     Свойство для определения состояния дерева JSON (раскрыто или свернуто).
    /// </summary>
    public bool IsTreeExpanded
    {
        get => _isTreeExpanded;
        set
        {
            if (_isTreeExpanded == value) return;
            _isTreeExpanded = value;
            OnPropertyChanged(nameof(IsTreeExpanded));
        }
    }
    /// <summary>
    /// Указывает, должна ли быть видна информация и поиске.
    /// </summary>
    public bool IsSearchInfoVisible => _jsonTreeItems.Any() && HasSearched;
    /// <summary>
    ///     Прогресс загрузки и обработки файла.
    /// </summary>
    public double Progress
    {
        get => _progress;
        set
        {
            _progress = value;
            OnPropertyChanged(nameof(Progress));
        }
    }
    /// <summary>
    ///     Команда для раскрытия или сворачивания всех элементов дерева.
    /// </summary>
    public ICommand ToggleExpandCollapseCommand { get; }
    /// <summary>
    ///     Команда для переключения виртуализации.
    /// </summary>
    public ICommand ToggleVirtualizationCommand { get; }
    /// <summary>
    ///     Команда для открытия окна поиска.
    /// </summary>
    public ICommand OpenSearchWindowCommand { get; }
    /// <summary>
    ///     Команда для копирования имени элемента.
    /// </summary>
    public ICommand CopyNameCommand { get; }
    /// <summary>
    ///     Команда для копирования значения элемента.
    /// </summary>
    public ICommand CopyValueCommand { get; }
    /// <summary>
    ///     Команда для копирования имени и значения элемента.
    /// </summary>
    public ICommand CopyNameAndValueCommand { get; }
    /// <summary>
    ///     Команда для открытия JSON-файла.
    /// </summary>
    public ICommand OpenFileCommand { get; }
    /// <summary>
    ///     Команда для очистки представления дерева JSON.
    /// </summary>
    public ICommand ClearCommand { get; }
    /// <summary>
    ///     Коллекция элементов дерева JSON.
    /// </summary>
    public ObservableCollection<JsonTreeItem> JsonTreeItems
    {
        get => _jsonTreeItems;
        set
        {
            _jsonTreeItems = value;
            OnPropertyChanged(nameof(JsonTreeItems));
        }
    }
    /// <summary>
    ///     Текст кнопки переключения виртуализации.
    /// </summary>
    public string VirtualizationButtonText => IsVirtualizationEnabled ? "Выключить виртуализацию" : "Включить виртуализацию";
    /// <summary>
    ///     Сообщение о статусе загрузки и обработки файла.
    /// </summary>
    public string? StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            OnPropertyChanged(nameof(StatusMessage));
        }
    }

    /// <summary>
    ///     Переходит к следующему результату поиска и выделяет его в дереве.
    /// </summary>
    protected override void ExecuteNextSearchResult()
    {
        if (AllMatches is not {Count: > 0}) return;
        CurrentMatchIndex = (CurrentMatchIndex + 1) % AllMatches.Count;
        if (CurrentMatchIndex > 0)
            HighlightItem(AllMatches[CurrentMatchIndex], AllMatches[CurrentMatchIndex - 1]);
        else
            HighlightItem(AllMatches[CurrentMatchIndex]);
        OnPropertyChanged(nameof(CurrentMatch));
        OnPropertyChanged(nameof(IsSearchInfoVisible));
    }

    /// <summary>
    ///     Переходит к предыдущему результату поиска и выделяет его в дереве.
    /// </summary>
    protected override void ExecutePreviousSearchResult()
    {
        if (AllMatches is not {Count: > 0}) return;
        CurrentMatchIndex = (CurrentMatchIndex - 1 + AllMatches.Count) % AllMatches.Count;
        if (CurrentMatchIndex > 0)
            HighlightItem(AllMatches[CurrentMatchIndex], AllMatches[CurrentMatchIndex - 1]);
        else
            HighlightItem(AllMatches[CurrentMatchIndex]);
        OnPropertyChanged(nameof(CurrentMatch));
        OnPropertyChanged(nameof(IsSearchInfoVisible));
    }

    /// <summary>
    ///     Выполняет поиск по дереву JSON и выделяет найденные элементы.
    /// </summary>
    protected override void PerformSearch()
    {
        AllMatches = _searchService.FindMatches(JsonTreeItems, SearchQuery);
        TotalMatches = AllMatches.Count;
        CurrentMatchIndex = -1;
        ExecuteNextSearchResult();
        HasSearched = true;
        OnPropertyChanged(nameof(CurrentMatch));
        OnPropertyChanged(nameof(IsSearchInfoVisible));
    }

    /// <summary>
    ///     Очищает выделение всех элементов в дереве.
    /// </summary>
    /// <param name="items">Коллекция элементов дерева.</param>
    private static void ClearSelection(IEnumerable<JsonTreeItem> items)
    {
        foreach (var item in items)
        {
            item.IsSelected = false;
            ClearSelection(item.Children);
        }
    }

    /// <summary>
    ///     Очищает дерево JSON.
    /// </summary>
    private void ClearTreeView()
    {
        JsonTreeItems.Clear();
        OnPropertyChanged(nameof(AreSearchControlsVisible));
        OnPropertyChanged(nameof(IsOpenSearchWindowButtonVisible));
        OnPropertyChanged(nameof(IsSearchInfoVisible));
    }

    /// <summary>
    ///     Копирует указанный текст в буфер обмена.
    /// </summary>
    /// <param name="text">Текст для копирования.</param>
    private void CopyToClipboard(string text)
    {
        if (!string.IsNullOrEmpty(text))
            _clipboardService.SetText(text);
    }

    /// <summary>
    ///     Оценивает время обработки файла на основе его размера и сохраненной скорости.
    /// </summary>
    /// <param name="fileSizeInMegaByte">Размер файла в мегабайтах.</param>
    /// <returns>Оценочное время обработки файла.</returns>
    private double EstimateProcessingTime(double fileSizeInMegaByte)
    {
        var savedSpeed = _speedManager.LoadSpeed();
        var estimatedTime = fileSizeInMegaByte / savedSpeed;
        return estimatedTime > 0 ? estimatedTime : 1;
    }

    /// <summary>
    ///     Рекурсивно раскрывает или сворачивает элемент дерева и его дочерние элементы.
    /// </summary>
    /// <param name="item">Элемент дерева для раскрытия/сворачивания.</param>
    /// <param name="isExpanded">True, если нужно раскрыть элемент, иначе False для сворачивания.</param>
    private static void ExpandOrCollapseItem(JsonTreeItem item, bool isExpanded)
    {
        item.IsExpanded = isExpanded;

        foreach (var child in item.Children) ExpandOrCollapseItem(child, isExpanded);
    }

    /// <summary>
    ///     Раскрывает всех родительских элементов указанного элемента.
    /// </summary>
    /// <param name="item">Целевой элемент.</param>
    private static void ExpandParents(JsonTreeItem item)
    {
        var parent = item.Parent;
        while (parent != null)
        {
            parent.IsExpanded = true;
            parent = parent.Parent;
        }
    }

    /// <summary>
    ///     Выделяет указанный элемент в дереве и раскрывает его родителей.
    /// </summary>
    /// <param name="item">Элемент для выделения.</param>
    /// <param name="previousItem">Предыдущий выделенный элемент (если есть).</param>
    private void HighlightItem(JsonTreeItem item, JsonTreeItem? previousItem = null)
    {
        ClearSelection(JsonTreeItems);

        item.IsSelected = true;
        if (previousItem != null) previousItem.IsSelected = false;
        if (item.IsExpanded) return;
        ExpandParents(item);
        item.IsExpanded = true;
    }

    /// <summary>
    ///     Асинхронно загружает и парсит JSON файл, а также обновляет статус выполнения.
    /// </summary>
    /// <param name="filePath">Путь к файлу для загрузки.</param>
    /// <returns>Задача выполнения загрузки файла.</returns>
    private async Task LoadAndParseJsonFileAsync(string filePath)
    {
        var stopwatch = new Stopwatch();
        _isLoadingComplete = false;
        try
        {
            JsonTreeItems.Clear();
            StatusMessage = "Загрузка файла...";
            Progress = 0;

            if (!_fileService.ValidateFileSize(filePath, MaxFileSizeInMegaByte))
            {
                StatusMessage = $"Файл слишком большой для загрузки (больше {MaxFileSizeInMegaByte} MB).";
                Progress = 0;
                return;
            }

            var fileSizeInMegaByte = _fileService.GetFileSizeInMegaBytes(filePath);
            var estimatedTime = EstimateProcessingTime(fileSizeInMegaByte);

            var progressTask = _progressUpdater.UpdateProgressAsync(
                progress => Progress = progress,
                estimatedTime,
                () => _isLoadingComplete
            );

            stopwatch.Start();

            var items = await _jsonParser.ParseJsonAsync(filePath);

            stopwatch.Stop();
            _isLoadingComplete = true;

            await progressTask;

            _synchronizationContext.Post(_ =>
            {
                JsonTreeItems = new ObservableCollection<JsonTreeItem>(items);
                OnPropertyChanged(nameof(AreSearchControlsVisible));
                OnPropertyChanged(nameof(IsOpenSearchWindowButtonVisible));
            }, null);

            Progress = ProgressMaxValue;
            UpdateProcessingSpeed(fileSizeInMegaByte, stopwatch.Elapsed.TotalSeconds);

            StatusMessage = "Завершено!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка загрузки файла: {ex.Message}";
            Progress = 0;
        }
        finally
        {
            stopwatch.Stop();
        }
    }

    /// <summary>
    ///     Открывает диалоговое окно для выбора JSON-файла и загружает его.
    /// </summary>
    private async Task OpenJsonFileAsync()
    {
        var filePath = _fileDialogService.OpenFile("JSON Files (*.json)|*.json");
        if (!string.IsNullOrEmpty(filePath)) await LoadAndParseJsonFileAsync(filePath);
    }

    /// <summary>
    ///     Открывает окно поиска.
    /// </summary>
    private void OpenSearchWindow()
    {
        var searchViewModel = _searchViewModelFactory.Create(JsonTreeItems);
        _navigationService.OpenSearchWindow(searchViewModel);
    }

    /// <summary>
    ///     Команда для раскрытия или сворачивания всех элементов дерева.
    /// </summary>
    private void ToggleExpandCollapse()
    {
        _isTreeExpanded = !_isTreeExpanded;

        foreach (var item in JsonTreeItems) ExpandOrCollapseItem(item, _isTreeExpanded);
        OnPropertyChanged(nameof(IsTreeExpanded));
    }

    /// <summary>
    ///     Переключает состояние виртуализации.
    /// </summary>
    private void ToggleVirtualization()
    {
        IsVirtualizationEnabled = !IsVirtualizationEnabled;
        OnPropertyChanged(nameof(VirtualizationButtonText));
    }

    /// <summary>
    ///     Обновляет сохраненную скорость обработки на основе времени обработки файла.
    /// </summary>
    /// <param name="fileSizeInMegaByte">Размер файла в мегабайтах.</param>
    /// <param name="timeInSeconds">Время обработки в секундах.</param>
    private void UpdateProcessingSpeed(double fileSizeInMegaByte, double timeInSeconds)
    {
        if (!(timeInSeconds > 0)) return;
        var currentSpeed = fileSizeInMegaByte / timeInSeconds;
        _speedManager.SaveSpeed(currentSpeed);
    }
}
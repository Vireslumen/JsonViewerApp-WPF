using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using JsonViewerApp.Interfaces;
using JsonViewerApp.Models;
using Newtonsoft.Json;

namespace JsonViewerApp.Services;

/// <summary>
///     Класс для парсинга JSON файлов в дерево объектов <see cref="JsonTreeItem" />.
/// </summary>
public class JsonParser(ILoggerService loggerService) : IJsonParser
{
    /// <inheritdoc />
    public async Task<List<JsonTreeItem>> ParseJsonAsync(string jsonFilePath)
    {
        try
        {
            using var streamReader = new StreamReader(jsonFilePath);
            using var jsonReader = new JsonTextReader(streamReader);
            var parsedData = await Task.Run(() => ParseJsonReader(jsonReader));
            return parsedData;
        }
        catch (Exception ex)
        {
            loggerService.Error(ex, $"Ошибка при асинхронном парсинге JSON файла: {jsonFilePath}");
            throw; // Проброс исключения для обработки на более высоком уровне
        }
    }

    /// <summary>
    ///     Создание нового элемента <see cref="JsonTreeItem" /> для начала объекта или массива.
    /// </summary>
    /// <param name="reader">Экземпляр <see cref="JsonReader" /> для получения данных.</param>
    /// <param name="currentItemStack">Текущий стек элементов.</param>
    /// <returns>Созданный элемент <see cref="JsonTreeItem" />.</returns>
    private static JsonTreeItem CreateNewItem(JsonReader reader, Stack<JsonTreeItem> currentItemStack)
    {
        var isArray = reader.TokenType == JsonToken.StartArray;
        var newItem = new JsonTreeItem
        {
            Name = "",
            Value = "",
            Level = currentItemStack.Count,
            Parent = currentItemStack.Count > 0 ? currentItemStack.Peek() : null,
            Children = new ObservableCollection<JsonTreeItem>(),
            IsArray = isArray
        };

        if (currentItemStack.Count > 0 && newItem.Parent?.IsArray == true) newItem.Name = $"[{newItem.Parent.Children.Count}]";

        return newItem;
    }

    /// <summary>
    ///     Внутренний метод для обработки данных JSON через <see cref="JsonReader" />.
    /// </summary>
    /// <param name="reader">Экземпляр <see cref="JsonReader" /> для обработки данных JSON.</param>
    /// <returns>Список корневых элементов типа <see cref="JsonTreeItem" />.</returns>
    private static List<JsonTreeItem> ParseJsonReader(JsonReader reader)
    {
        var rootItems = new List<JsonTreeItem>();
        var currentItemStack = new Stack<JsonTreeItem>();

        while (reader.Read())
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                case JsonToken.StartArray:
                    var newItem = CreateNewItem(reader, currentItemStack);
                    if (currentItemStack.Count > 0)
                    {
                        var parent = currentItemStack.Peek();
                        parent.Children.Add(newItem);
                    }
                    else
                    {
                        rootItems.Add(newItem);
                    }

                    currentItemStack.Push(newItem);
                    break;

                case JsonToken.PropertyName:
                    ProcessPropertyName(reader, currentItemStack);
                    break;

                case JsonToken.EndObject:
                case JsonToken.EndArray:
                    currentItemStack.Pop();
                    break;

                default:
                    ProcessArrayElement(reader, currentItemStack);
                    break;
            }

        return rootItems;
    }

    /// <summary>
    ///     Обработка элемента массива.
    /// </summary>
    /// <param name="reader">Экземпляр <see cref="JsonReader" /> для получения данных.</param>
    /// <param name="currentItemStack">Текущий стек элементов.</param>
    private static void ProcessArrayElement(JsonReader reader, Stack<JsonTreeItem> currentItemStack)
    {
        if (currentItemStack.Count <= 0 || !currentItemStack.Peek().IsArray) return;
        var parent = currentItemStack.Peek();
        var arrayItem = new JsonTreeItem
        {
            Name = $"[{parent.Children.Count}]",
            Value = reader.Value?.ToString() ?? string.Empty,
            Level = parent.Level + 1,
            Parent = parent
        };
        parent.Children.Add(arrayItem);
    }

    /// <summary>
    ///     Обработка имени свойства JSON объекта.
    /// </summary>
    /// <param name="reader">Экземпляр <see cref="JsonReader" /> для получения данных.</param>
    /// <param name="currentItemStack">Текущий стек элементов.</param>
    private static void ProcessPropertyName(JsonReader reader, Stack<JsonTreeItem> currentItemStack)
    {
        var propertyName = reader.Value?.ToString();
        if (!reader.Read()) return;

        var parentItem = currentItemStack.Peek();

        if (reader.TokenType is JsonToken.StartObject or JsonToken.StartArray)
        {
            var isChildArray = reader.TokenType == JsonToken.StartArray;
            var childItem = new JsonTreeItem
            {
                Name = propertyName ?? string.Empty,
                Value = "",
                Level = parentItem.Level + 1,
                Parent = parentItem,
                Children = new ObservableCollection<JsonTreeItem>(),
                IsArray = isChildArray
            };
            parentItem.Children.Add(childItem);
            currentItemStack.Push(childItem);
        }
        else
        {
            var value = reader.Value?.ToString();
            var childItem = new JsonTreeItem
            {
                Name = propertyName ?? string.Empty,
                Value = value ?? string.Empty,
                Level = parentItem.Level + 1,
                Parent = parentItem
            };
            parentItem.Children.Add(childItem);
        }
    }
}
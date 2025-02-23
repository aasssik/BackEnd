using System;
using System.Collections.Generic;

public interface IWordService
{
    void AddWord(string word, string meaning);
    void DisplayWords();
}

public class WordService : IWordService
{
    private readonly Dictionary<string, string> _dictionary;

    public WordService()
    {
        _dictionary = new Dictionary<string, string>();
    }

    public void AddWord(string word, string meaning)
    {
        if (!_dictionary.ContainsKey(word))
        {
            _dictionary[word] = meaning;
        }
    }

    public void DisplayWords()
    {
        foreach (var entry in _dictionary)
        {
            Console.WriteLine($"{entry.Key}: {entry.Value}");
        }
    }
}

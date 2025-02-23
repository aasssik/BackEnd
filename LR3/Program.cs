using System;

class Program
{
    static void Main(string[] args)
    {
        IWordService wordService = new WordService();

        wordService.AddWord("apple", "фрукт");
        wordService.AddWord("book", "набор написанных или напечатанных страниц");

        wordService.DisplayWords();
    }
}

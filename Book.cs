namespace Lab12
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Genre { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }

        public Book() { }

        public Book(string title, string author, int year, string genre = "", bool isAvailable = true)
        {
            Title = title;
            Author = author;
            Year = year;
            Genre = genre;
            IsAvailable = isAvailable;
        }
    }
}
namespace Smart_Librarian.Models.Dtos
{

    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishYear { get; set; }
    }

    public class AddBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishYear { get; set; }
    }

    public class EditBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublishYear { get; set; }
    }

}
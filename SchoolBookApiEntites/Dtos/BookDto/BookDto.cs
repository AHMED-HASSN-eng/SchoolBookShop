namespace SchoolBookShop.Dtos.BookDto
{
    public class BookDto
    {
        public string StudyYear { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public int Rate { get; set; }
        public ICollection<BookPhoto>? BookPhotos { get; set; }
    }
}

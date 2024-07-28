namespace SchoolBookShop.Dtos.BookDto
{
    public class AddBookDto
    {
        public string StudyYear { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public List<IFormFile> Photo { get; set; }
    }
}

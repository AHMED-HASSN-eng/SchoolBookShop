namespace SchoolBookShop.Models
{
    public class BookPhoto
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public string Description { get; set; }
        public string FileExtension { get; set; }
        public decimal Size { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
    }
}

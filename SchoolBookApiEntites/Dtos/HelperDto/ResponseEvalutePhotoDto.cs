namespace SchoolBookShop.Dtos.HelperDto
{
    public class ResponseEvalutePhotoDto
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public object? Model { get; set; }
        public int? Score { get; set; }
        public List<BookPhoto>? photos { get; set; }
    }
}

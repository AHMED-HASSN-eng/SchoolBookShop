namespace SchoolBookShop.Dtos.HelperDto
{
    public class ResponseDto
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public object? Model { get; set; }
    }
}

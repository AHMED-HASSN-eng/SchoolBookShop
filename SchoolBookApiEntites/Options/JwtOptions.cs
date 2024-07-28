namespace SchoolBookShop.Options
{
    public class JwtOptions
    {
        public string Issure { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public double Duration { get; set; }
    }

}

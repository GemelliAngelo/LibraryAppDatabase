namespace LibraryApp.Classes
{
    public record Book
    {
        public long ISBN { get; set; }
        public required string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime PublishDate { get; set; }
        public required bool Available { get; set; }
    }
}

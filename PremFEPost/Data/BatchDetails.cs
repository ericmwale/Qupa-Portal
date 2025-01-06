namespace PremFEPost.Data

{
    public class BatchDetails
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Date { get; set; }
        public string UploadedBy { get; set; }
        public string AuthorisedBy { get; set;}
        public string Status { get; set; }
        public int Records { get; set; }

    }
}

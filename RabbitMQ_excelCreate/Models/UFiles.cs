namespace RabbitMQ_excelCreate.Models
{
    public class UFiles
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime? CreatedDate { get; set; }
        public FileStatus? FileStatus { get; set; }

        [NotMapped] // Veritabanında herhangi bir tabloya mapping yapma
        public string GetCreatedDate
        {
            get => CreatedDate.HasValue ? CreatedDate.Value.ToString() : "-" ;
        }

    }
}

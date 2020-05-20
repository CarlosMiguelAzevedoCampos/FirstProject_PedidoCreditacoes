namespace CMA.ISMAI.Trello.FileReader.Model
{
    public class FileModel
    {
        public FileModel(string courseName, string instituteName, string userName, int page)
        {
            CourseName = courseName;
            InstituteName = instituteName;
            UserName = userName;
            Page = page;
        }

        public string CourseName { get; set; }
        public string InstituteName { get; set; }
        public string UserName { get; set; }
        public int Page { get; set; }
    }
}

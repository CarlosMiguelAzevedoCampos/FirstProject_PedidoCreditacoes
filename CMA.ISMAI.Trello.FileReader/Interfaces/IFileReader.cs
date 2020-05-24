namespace CMA.ISMAI.Trello.FileReader.Interfaces
{
    public interface IFileReader
    {
        string ReturnUserNameForTheCard(string institute, string course, int pageId, string filePath);
    }
}

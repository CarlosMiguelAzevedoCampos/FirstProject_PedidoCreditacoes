namespace CMA.ISMAI.Trello.Engine.Automation
{
    public interface IEngine
    {
        string StartWorkFlow(string newCardId, string courseName, string studentName, string courseInstitute, bool IsCetOrOtherCondition);
    }
}

using CamundaClient.Dto;
using System;
using System.Collections.Generic;

namespace CMA.ISMAI.Sagas.Domain.Interface
{
    public interface ITaskProcessingDomainService
    {
        bool FinishTasks(string processName, string Id, Dictionary<string, object> taskValues = null);
        string ReturnCardIdFromExternalTask(ExternalTask externalTask);
        string ReturnCourseNameFromExternalTask(ExternalTask externalTask);
        string ReturnStudentNameFromExternalTask(ExternalTask externalTask);
        string ReturnCourseInstitueFromExternalTask(ExternalTask externalTask);
        bool CheckIfItsSummerBreak(DateTime dateTime);
        Dictionary<string, object> ReturnDictionaryForTheProcess(string newCardId, string courseName, string studentName, string courseInstitute);
    }
}

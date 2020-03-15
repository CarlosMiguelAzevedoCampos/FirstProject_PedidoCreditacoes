using System;

namespace CMA.ISMAI.Engine.API.Model
{
    public class Response
    {
        public Response(bool hasErrors, DateTime responseTime, string information)
        {
            HasErrors = hasErrors;
            ResponseTime = responseTime;
            Information = information;
        }

        public bool HasErrors { get; private set; }
        public string Information { get; private set; }
        public DateTime ResponseTime { get; private set; }
    }
}

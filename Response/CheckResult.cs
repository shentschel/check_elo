using System;

namespace check_elo.Response
{
    public class CheckResult
    {
        public string Message { get; set; }

        public ExitCode ExitCode { get; set; } = ExitCode.Unknown;

        public string PerformanceData { get; set; }

        public void Commit()
        {
            if (string.IsNullOrWhiteSpace(PerformanceData))
                Console.WriteLine(Message);
            else
                Console.WriteLine(Message + " | " + PerformanceData);
        }
    }
}
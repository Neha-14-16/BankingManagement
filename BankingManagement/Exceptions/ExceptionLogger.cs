using System;

namespace BankingManagement.Exceptions
{
    public static class ExceptionLogger
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(
                typeof(ExceptionLogger)
            );

        public static void Log(Exception ex)
        {
            if (ex == null)
            {
                return;
            }

            log.Error(
                "An exception occurred in Banking Management.",
                ex
            );
        }
    }
}
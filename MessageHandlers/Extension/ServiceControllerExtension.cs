using System.ServiceProcess;
using MessageHandlers.Properties;

namespace MessageHandlers.Extension
{
    public static class ServiceControllerExtension
    {
        public static string Format(this ServiceController self)
        {
            var output = string.Empty;
            output += $"{Format(self.Status)} ";
            output += self.ServiceName;
            return output;
        }

        public static string Format(ServiceControllerStatus status)
        {
            switch (status)
            {
                case ServiceControllerStatus.Running:
                    return Resources.GetServiceInfoRunning;
                case ServiceControllerStatus.Stopped:
                    return Resources.GetServiceInfoStopped;
                default:
                    return status.ToString().Substring(0, 1);
            }
        }
    }
}
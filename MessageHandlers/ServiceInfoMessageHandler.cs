using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using MessageHandlers.Extension;
using MessageHandlers.Properties;

namespace MessageHandlers
{
    public class ServiceInfoMessageHandler : IMessageHandler
    {
        public ServiceInfoMessageHandler(string[] trackingWindowsServices)
        {
            _trackingWindowsServices = trackingWindowsServices;
        }

        public async Task<string> HandleAsync(string command)
        {
            try
            {
                var output = new StringBuilder();

                output.AppendLine(string.Format(Resources.ServiceMessageHandlerOutputHeader, DateTime.UtcNow));
                output.AppendLine("");
                output.AppendLine(GetWinServicesInfo(_trackingWindowsServices));

                return output.ToString();
            }
            catch (Exception ex)
            {
                return string.Format(Resources.ServiceInfoMessageHandleException, ex.Message);
            }
        }

        private string GetWinServicesInfo(IEnumerable<string> winServiceNames)
        {
            var output = new StringBuilder();
            foreach (var winService in winServiceNames)
            {
                output.AppendLine(GetWinServiceInfo(winService));
            }
            return output.ToString();
        }

        private string GetWinServiceInfo(string serviceName)
        {
            try
            {
                serviceName = serviceName.Trim();

                var serviceController = new ServiceController(serviceName);

                return serviceController.Format();
            }
            catch (Exception ex)
            {
                return string.Format(Resources.GetServiceInfoException, serviceName, ex.Message);
            }
        }

        private readonly string[] _trackingWindowsServices;
    }
}
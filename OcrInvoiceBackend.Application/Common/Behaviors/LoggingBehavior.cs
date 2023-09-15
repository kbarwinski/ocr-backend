using MediatR;
using Microsoft.Extensions.Logging;
using OcrInvoiceBackend.Application.Common.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Common.Behaviors
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        private void LogProps(object toLog)
        {
            StringBuilder stringBuilder = new StringBuilder("");

            if (toLog is IEnumerable collection)
            {
                int index = 0;
                foreach (var item in collection)
                {
                    stringBuilder.AppendLine($"Item[{index}] : {item}");
                    index++;
                }
            }
            else
            {
                Type toLogType = toLog.GetType();
                IList<PropertyInfo> props = new List<PropertyInfo>(toLogType.GetProperties());

                foreach (PropertyInfo prop in props)
                {
                    if (prop.GetIndexParameters().Length > 0)
                        continue; // Skip indexed properties

                    object propValue = null;
                    try
                    {
                        propValue = prop.GetValue(toLog, null);
                    }
                    catch (Exception ex)
                    {
                        propValue = $"Error fetching value: {ex.Message}";
                    }
                    stringBuilder.AppendLine($"{prop.Name} : {propValue}");
                }
            }

            _logger.LogInformation(stringBuilder.ToString());
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling {typeof(TRequest).Name}");

            LogProps(request);

            TResponse response;
            try
            {
                response = await next();
                LogProps(response);
                return response;
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, $"Validation of request failed while handling {typeof(TRequest).Name}");

                if (ex.Errors != null)
                {
                    var i = 1;
                    foreach (var err in ex.Errors)
                    {
                        _logger.LogWarning(ex, $"Error detail {i} of {ex.Errors}: {err}");
                    }
                }

                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, $"Blocked unauthorized access attempt while handling {typeof(TRequest).Name}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while handling {typeof(TRequest).Name}");
                throw;
            }
        }
    }
}

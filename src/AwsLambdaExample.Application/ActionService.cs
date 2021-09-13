using AwsLambdaExample.Application.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace AwsLambdaExample.Application
{
    public interface IActionService
    {
        Task<object?> Handle(string action, JObject? model);
    }

    [ScanAndRegister]
    public class ActionService : IActionService
    {
        private readonly IActionLocator _actionLocator;
        private readonly ILogger<ActionService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ActionService(IServiceProvider serviceProvider, IActionLocator actionLocator, ILogger<ActionService> logger)
        {
            _serviceProvider = serviceProvider;
            _actionLocator = actionLocator;
            _logger = logger;
        }

        public Task<object?> Handle(string action, JObject? model)
        {
            var actionType = _actionLocator.GetActionType(action);

            if (actionType == null)
            {
                _logger.LogWarning("Action: {Action} not exist", action);
                throw new KnownException($"Action: {action} not exist");
            }

            var actionHandler = (IActionHandler)_serviceProvider.GetService(actionType);

            return actionHandler.HandleAsync(model);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AwsLambdaExample.Application
{
    public interface IActionLocator
    {
        Type? GetActionType(string action);
    }

    [ScanAndRegister(Lifetime = ServiceLifetime.Singleton)]
    public class ActionLocator : IActionLocator
    {
        private readonly Dictionary<string, IActionHandler> _actionHandlers;

        public ActionLocator(IEnumerable<IActionHandler> actionHandlers)
        {
            _actionHandlers = actionHandlers.ToDictionary(x => x.GetType().Name, x => x);
        }

        public Type? GetActionType(string action)
        {
            if (_actionHandlers.ContainsKey(action))
            {
                return _actionHandlers[action].GetType();
            }
            return null;
        }
    }
}

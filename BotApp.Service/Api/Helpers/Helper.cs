using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Service.Api.Helpers
{
    internal static class Helper
    {
        internal static T ThrowIfNull<T>(T? value, [CallerArgumentExpression(nameof(value))] string? parameterName = default)
            => value ?? throw new ArgumentNullException(parameterName);
    }
}

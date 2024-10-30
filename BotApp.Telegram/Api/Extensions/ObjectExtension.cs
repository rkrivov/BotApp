using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BotApp.Telegram.Api.Extensions
{
    internal static class ObjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T ThrowIfNull<T>(this T? value, [CallerArgumentExpression(nameof(value))] string? parameterName = default)
            => value ?? throw new ArgumentNullException(parameterName);
    }
}

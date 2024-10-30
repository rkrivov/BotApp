using BotApp.Telegram.Api.Extensions;
using Microsoft.VisualBasic;
using System.Collections;
using System.IO.Compression;
using System.Text;
using Telegram.Bot.Types;

namespace BotApp.Telegram.Api.Utils
{
    public static class Global
    {
        public static long GetId(Update? update)
        {
            long id = 0;

            id = update?.Message?.Chat?.Id ?? 0;

            if (id == 0)
                id = update?.Message?.From?.Id ?? 0;

            if (id == 0)
                id = update?.CallbackQuery?.Message?.Chat?.Id ?? 0;

            if (id == 0)
                id = update?.CallbackQuery?.Message?.From?.Id ?? 0;

            if (id == 0)
                id = update?.CallbackQuery?.From?.Id ?? 0;

            if (id == 0)
                id = update?.ChannelPost?.Chat?.Id ?? 0;

            if (id == 0)
                id = update?.ChannelPost?.From?.Id ?? 0;

            return id;
        }

        public static byte[] Compress(byte[] input)
        {
            using (var originalStream = new MemoryStream(input))
            {
                using (var compressedStream = new MemoryStream())
                {
                    var compressor = new GZipStream(compressedStream, CompressionMode.Compress);

                    originalStream.CopyTo(compressor);

                    compressedStream.Position = 0;

                    return compressedStream.ToArray();
                }
            }
        }

        public static byte[] Decompress(byte[] input)
        {
            using (var compressedStream = new MemoryStream(input))
            {
                using (var outputStream = new MemoryStream())
                {
                    var decompressor = new GZipStream(outputStream, CompressionMode.Decompress);

                    compressedStream.CopyTo(decompressor);

                    outputStream.Position = 0;

                    return compressedStream.ToArray();
                }
            }
        }

        private static void AddUnique(ref List<string> list, string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            if (list == null)
                list = new List<string>();

            if (list.Contains(value)) return;

            list.Add(value);
        }

        public static string Key(object obj)
        {
            obj.ThrowIfNull();

            List<string> list = new List<string>();
            var voidType = typeof(void);

            AddUnique(ref list, value: obj?.ToString() ?? string.Empty);
            AddUnique(ref list, value: (obj?.GetHashCode() ?? 0).ToString());
            AddUnique(ref list, value: (obj?.GetType() ?? voidType)?.FullName ?? string.Empty);

            if (list.Count > 0)
            {
                var keyValue = string.Join("/", list.ToArray());
                keyValue = string.Format("/{0}/", keyValue);

                return keyValue.ToHashString();
            }

            return string.Empty;
        }
    
        private static string ConvertToString(object? obj, bool skipType = false)
        {
            if (obj == null) return "<null>";
            var type = obj.GetType();

            var stringBuilder = new StringBuilder();

            if (!skipType)
            {
                stringBuilder.Append(string.Format("<{0}>", type.FullName));
                stringBuilder.Append(' ');
            }

            if (type.IsArray)
            {
                var items = (object[])obj;
                var strings = new string[items.Length];

                for (int index = 0; index < items.Length; index++)
                    strings.SetValue(ConvertToString(items[index]), index);

                stringBuilder.Append(string.Format("[{0}]", string.Join(", ", strings.ToArray())));
            }
            else
            {
            }

            return stringBuilder.ToString().Trim();
        }

        public static string ToString(object? obj, int level)
        {
            if (obj == null) return "<null>";

            var type = obj.GetType();

            if (type.IsArray)
            {
                var array = (object[])obj;

                var stringBuilder = new StringBuilder();

                if (level > 0)
                    stringBuilder.Append(new string(' ', level * 4));

                stringBuilder.Append(type.FullName);
                stringBuilder.Append($": Length = {array.Length}");

                if (level > 0)
                    stringBuilder.Append(new string(' ', level * 4));

                stringBuilder.AppendLine("[");

                for (int index = 0; index < array.Length; index++)
                {
                    if (index > 0) 
                        stringBuilder.Append(", ");
                    stringBuilder.Append(new string(' ', (level + 1) * 4));
                    stringBuilder.AppendLine(ToString(array[index]));
                }

                if (level > 0)
                    stringBuilder.Append(new string(' ', level * 4));

                stringBuilder.AppendLine("]");

                return stringBuilder.ToString().Trim();
            }

            if (obj is string stringValue)
            {
                return string.Format("\"{0}\"", stringValue);
            }
            
            if (obj is DateTime dateTime)
            {
                return string.Format("{{ts\"{0}\"}}", dateTime.ToString("yyyy-MM-dd HH:mm:sszzz"));
            }

            if (obj is IEnumerable enumerable)
            {
                var objects = (IEnumerable<object>)obj;
                var stringBuilder = new StringBuilder();

                if (level > 0)
                    stringBuilder.Append(new string(' ', level * 4));

                stringBuilder.Append(type.FullName);
                stringBuilder.Append($"Count = {objects.Count()}");
                stringBuilder.AppendLine();

                if (level > 0)
                    stringBuilder.Append(new string(' ', level * 4));

                stringBuilder.AppendLine("{");

                int index = 0;

                foreach(var item in enumerable)
                {
                    stringBuilder.Append(new string(' ', (level + 1) * 4));
                    stringBuilder.AppendLine(ToString(item, level + 1));
                    index++;
                }

                if (level > 0)
                    stringBuilder.Append(new string(' ', level * 4));

                stringBuilder.Append("}");

                return stringBuilder.ToString().Trim();
            }

            if (type.IsClass)
            {
                var stringBuilder = new StringBuilder();
                if (level > 0)
                    stringBuilder.Append(new string(' ', level * 4)); 

                stringBuilder.AppendLine(string.Format("<{0}>", type.FullName));

                if (level > 0)
                    stringBuilder.Append(new string(' ', level * 4));

                stringBuilder.AppendLine("{");

                foreach (var field in type.GetFields())
                {
                    stringBuilder.Append(new string(' ', (level + 1) * 4));

                    if (field.IsPublic) stringBuilder.Append("public ");
                    if (field.IsPrivate) stringBuilder.Append("private ");
                    if (field.IsLiteral) stringBuilder.Append("literal ");
                    if (field.IsStatic) stringBuilder.Append("static ");

                    stringBuilder.Append(field.FieldType.FullName);
                    stringBuilder.Append(" ");
                    stringBuilder.Append(field.Name);
                    stringBuilder.Append(" = ");
                    try
                    {
                        stringBuilder.Append(ToString(field.GetValue(obj), level + 1));
                    }
                    catch (Exception exception)
                    {
                        stringBuilder.Append(string.Format("{{Exception: {0}}}", exception.Message));
                    }
                    stringBuilder.AppendLine(";");
                }

                foreach (var property in type.GetProperties())
                {
                    stringBuilder.Append(new string(' ', (level + 1) * 4));

                    stringBuilder.Append(property.PropertyType.FullName);
                    stringBuilder.Append(" ");
                    stringBuilder.Append(property.Name);
                    stringBuilder.Append(" = ");

                    try
                    {
                        stringBuilder.Append(ToString(property.GetValue(obj), level + 1));
                    }
                    catch (Exception exception)
                    {
                        stringBuilder.Append(string.Format("{{Exception: {0}}}", exception.Message));
                    }

                    stringBuilder.AppendLine(";");
                }

                stringBuilder.Append(new string(' ', level * 4));
                stringBuilder.Append("}");

                return stringBuilder.ToString().Trim();
            }

            return Convert.ToString(obj) ?? string.Empty;
        }

        public static string ToString(object obj) => ToString(obj, 0);
    }
}

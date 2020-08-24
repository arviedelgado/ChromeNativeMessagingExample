using System;
using System.IO;
using System.Text.Json;

namespace HostApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var content = ReadInput();
                WriteOutput(DateTime.Now.Ticks + " echo: " + content);
            }
        }

        static string ReadInput()
        {
            var stdin = Console.OpenStandardInput();
            var bytes = new byte[4];
            stdin.Read(bytes, 0, 4);
            var length = BitConverter.ToInt32(bytes, 0);
            var message = "";
            for (var i = 0; i < length; i++)
            {
                message += (char)stdin.ReadByte();
            }
            var content = JsonDocument.Parse(message).RootElement
                            .GetProperty("content").GetString();
            return content;
        }

        static void WriteOutput(string content)
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
            writer.WriteStartObject();
            writer.WriteString("content", content);
            writer.WriteEndObject();
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            var bytes = stream.ToArray();
            var length = bytes.Length;
            var stdout = Console.OpenStandardOutput();
            stdout.WriteByte((byte)((length >> 0) & 0xFF));
            stdout.WriteByte((byte)((length >> 8) & 0xFF));
            stdout.WriteByte((byte)((length >> 16) & 0xFF));
            stdout.WriteByte((byte)((length >> 24) & 0xFF));
            stdout.Write(bytes, 0, bytes.Length);
            stdout.Flush();
        }
    }
}

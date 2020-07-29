using System;

namespace Dimmy.Cli.Extensions
{
    public static class UserInput
    {
        public static string GetUserInput(this string value, string userPrompt)
        {
            if (!string.IsNullOrEmpty(value))
                return value;

            Console.Write(userPrompt);
            return Console.ReadLine();
        }
    }
}
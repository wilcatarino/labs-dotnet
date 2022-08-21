using System;

namespace Generic;

public class Program
{
    public static void Main(string[] args)
    {
        string text = "1620615600000-180";
        Console.WriteLine(text);

        text.Format();

        Console.WriteLine("-------------------------");
        text = "1643943600000-180";
        Console.WriteLine(text);

        text.Format();

        // bool parsed = DateTime.TryParse(text, out DateTime dateTimeFromText);
        // Console.WriteLine(parsed);        
    }
}

public static class Extensions
{
    public static DateTime Format(this string text)
    {
        List<long> numbers = text.Split('-').Select(t => long.Parse(t)).ToList();
        Console.WriteLine(numbers[0]);
        Console.WriteLine(numbers[1]);

        DateTime result = numbers[0].FormatUnixTimestamp();

        Console.WriteLine(result);

        return result;
    }

    public static DateTime FormatUnixTimestamp(this long unixTimestamp)
    {
        DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        Console.WriteLine(result);

        return result.AddMilliseconds(Convert.ToDouble(unixTimestamp)).ToLocalTime();
    }
}

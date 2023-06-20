using System;
using System.Linq;

public static class RomanNumberHelper
{
    #region RomanNumberConvert

    private static readonly char[] ones = { 'I', 'X', 'C', 'M' };
    private static readonly char[] fives = { 'V', 'L', 'D' };

    public static int MaxNumber
    {
        get
        {
            if (ones.Length - fives.Length > 1 || fives.Length > ones.Length) return 0;
            float multi = ((ones.Length + fives.Length) / 2.0f) - 1.1f;
            double value = Math.Pow(10, Math.Ceiling(multi)) * (multi % 1 * 10);
            return (int)value - 1;
        }
    }

    private static string getFour(int power)
    {
        return ones[power] + "" + fives[power];
    }

    private static string getNine(int power)
    {
        return ones[power] + "" + ones[power + 1];
    }

    private static int getNumericValue(char romanDigit)
    {
        int value = 0;
        if (ones.Contains(romanDigit))
        {
            value = 1;
            for (int i = 0; i < ones.Length; i++)
            {
                if (ones[i].Equals(romanDigit)) break;
                value *= 10;
            }
        }
        else if (fives.Contains(romanDigit))
        {
            value = 5;
            for (int i = 0; i < fives.Length; i++)
            {
                if (fives[i].Equals(romanDigit)) break;
                value *= 10;
            }
        }
        return value;
    }

    public static int RomanToNumeric(string roman)
    {
        roman = roman.ToUpper();

        int value = 0;
        for (int i = ones.Length - 2; i > -1; i--)
        {
            if (roman.Contains(getFour(i)))
            {
                roman = roman.Replace(getFour(i), "");
                value += (int)(Math.Pow(10, i) * 4);
            }
            if (roman.Contains(getNine(i)))
            {
                roman = roman.Replace(getNine(i), "");
                value += (int)(Math.Pow(10, i) * 9);
            }
        }
        foreach (var digit in roman)
        {
            value += getNumericValue(digit);
        }
        return value;
    }

    public static string NumericToRoman(int num)
    {
        if (num == 0) return "0";

        string roman = "";
        if (num > MaxNumber || num < 0)
        {
        }
        else
        {
            int currentPower = 0;

            while (currentPower < ones.Length && num > 0)
            {
                int currentNum = num % 10;
                num = num / 10;

                int repeatOne = currentNum % 5;
                if (repeatOne > 3) repeatOne = 0;

                switch (currentNum)
                {
                    default:
                        break;

                    case 4:
                        roman = getFour(currentPower) + roman;
                        break;

                    case 9:
                        roman = getNine(currentPower) + roman;
                        break;
                }

                for (int i = 0; i < repeatOne; i++)
                {
                    roman = ones[currentPower] + roman;
                }

                if (currentNum > 4 && currentNum < 9)
                {
                    roman = fives[currentPower] + roman;
                }

                currentPower += 1;
            }
        }

        return roman;
    }

    #endregion RomanNumberConvert

    public static string ReplaceNumbersWithRoman(string text)
    {
        string[] lines = text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string[] words = lines[i].Split(' ');
            for (int j = 0; j < words.Length; j++)
            {
                int num;
                if (int.TryParse(words[j], out num))
                {
                    words[j] = NumericToRoman(num);
                }
            }
            lines[i] = string.Join(' ', words);
        }

        return string.Join('\n', lines);
    }
}
using System;
using System.Linq;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        input = input.ToLower();
        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsPunctuation(input[i]) || char.IsWhiteSpace(input[i]))
            {
                input = input.Remove(i, 1);
                i--;
            }
        }

        string reversed = new string(input.Reverse().ToArray());
        return input == reversed;
    }
}



namespace SpredMedia.Authentication.Core.Utility
{
    public static class RandomAlgorithm
    {
        public static int Generate4Digit()
        {
            var rand = new Random();
            return rand.Next(1000, 10000);
        }
        public static string RandomizeString(string input)
        {
            char[] chars = input.ToCharArray();
            Random random = new Random();

            for (int i = chars.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                char temp = chars[i];
                chars[i] = chars[j];
                chars[j] = temp;
            }

            return new string(chars);
        }
    }
}

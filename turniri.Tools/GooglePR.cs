namespace turniri.Tools
{
    public static class GooglePr
    {
        private static int StrToNum(string str, int check, int magic)
        {
            const long int32Unit = 4294967296; // 2^32

            var length = str.Length;
            for (int i = 0; i < length; i++)
            {
                check *= magic;
                //If the float is beyond the boundaries of integer (usually +/- 2.15e+9 = 2^31),
                //  the result of converting to integer is undefined
                //  refer to http://www.php.net/manual/en/language.types.integer.php
                if (check >= int32Unit)
                {
                    check = (int)(check - int32Unit * (int)(check / int32Unit));
                    //if the check less than -2^31
                    check = (check < -2147483648) ? (int)(check + int32Unit) : check;
                }
                check += str[i];
            }

            return check;
        }

        //genearate a hash for a url
        private static int HashUrl(string str)
        {
            var Check1 = StrToNum(str, 0x1505, 0x21);
            var Check2 = StrToNum(str, 0, 0x1003F);

            Check1 >>= 2;
            Check1 = ((Check1 >> 4) & 0x3FFFFC0) | (Check1 & 0x3F);

            Check1 = ((Check1 >> 4) & 0x3FFC00) | (Check1 & 0x3FF);
            Check1 = ((Check1 >> 4) & 0x3C000) | (Check1 & 0x3FFF);

            int T1 = ((((Check1 & 0x3C0) << 4) | (Check1 & 0x3C)) << 2) | (Check2 & 0xF0F);
            int T2 = (int)((((Check1 & 0xFFFFC000) << 4) | (Check1 & 0x3C00)) << 0xA) | (Check2 & 0xF0F0000);
            return (T1 | T2);
        }

        //genearate a checksum for the hash string
        private static string CheckHash(int hashnum)
        {
            var CheckByte = 0;
            var Flag = 0;

            var HashStr = ((uint)hashnum).ToString();
            var length = HashStr.Length;
            for (int i = length - 1; i >= 0; i--)
            {
                int re = (HashStr[i] - '0');
                if (1 == (Flag % 2))
                {
                    re += re;
                    re = (char)((re / 10) + (re % 10));
                }
                CheckByte += re;
                Flag++;
            }

            CheckByte %= 10;
            if (0 != CheckByte)
            {
                CheckByte = 10 - CheckByte;
                if (1 == (Flag % 2))
                {
                    if (1 == (CheckByte % 2))
                    {
                        CheckByte += 9;
                    }
                    CheckByte >>= 1;
                }
            }

            return "7" + CheckByte + HashStr;
        }

        //return the pagerank checksum hash
        private static string getch(string url)
        {
            return CheckHash(HashUrl(url));
        }


        public static string GooglePrFun(string url)
        {
            var ch = getch(url);
            url = "info:" + url;
            return "http://toolbarqueries.google.com/search?client=navclient-auto&ch=" + ch + "&features=Rank&q=" + url;
        }
    }
}
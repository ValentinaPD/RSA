using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication13
{
    
    class Program
    {
        static void Main(string[] args)
        {
            
            string Text = "доброе утро, good morning!";
            key PublicKey = new key();
            key PrivateKey = new key();
            GeneratorKey generator_key = new GeneratorKey();
            generator_key.GenerateKeys();
            Cryptor cryptor = new Cryptor();
            PublicKey.EditKeys(generator_key.GetOpenKey());
            PrivateKey.EditKeys(generator_key.GetPrivateKey());
            int[] enctr = cryptor.Encrypt(Text, PublicKey);
           
            string str = cryptor.Decrypt(enctr, PrivateKey);
            Console.WriteLine(str);
        }
       
    }
    public class key
    {
        int KEY;
        int n;
       
        public int GetKey()
        {
            return this.KEY;
        }
        public int GetN()
        {
            return this.n;
        }
        public void EditKeys(key k)
        {
            this.KEY = k.KEY;
          
            this.n = k.n;
            
        }
        public void EditKeys(int KEY, int n)
        {
            this.KEY = KEY;
           
            this.n = n;
           
        }
    }
    class GeneratorKey
    {

        key PublicKey = new key();
        key PrivateKey = new key();
        public key GetOpenKey()
        {
            return this.PublicKey;
        }
        public key GetPrivateKey()
        {
            return this.PrivateKey;
        }
        public GeneratorKey()
        {
            
            GenerateKeys();

        }
        public void GenerateKeys()
        {
            int p = 0;
            int q = 0;

            do
            {
                p = GetPrimeNumber();
                q = GetPrimeNumber();
            } while (p == q);
            int n = p * q;
            int fn = (p - 1) * (q - 1);
            int d = GetD(fn);
            int e = GetE(fn, d);
           
            this.PublicKey.EditKeys(d, n);
            this.PrivateKey.EditKeys(e, n);
            Console.WriteLine("p={0},q={1},d={2},e={3}", p, q, d, e);
        }
        public static int GetD(int fn)
        {
            Random rand = new Random();
            int d = 0;
            do
                d = rand.Next() % 1000;
            while (nod(d, fn) != 1);
            return d;
        }
        public static int GetE(int fn, int d)
        {
            Random rand = new Random();
            int e = rand.Next() % 1000;
            do
                e += 1;
            while ((e * d) % (fn) != 1);

            return e;
        }

        int sundaram(int n)
        {
            int[] a = new int[n];
            int i, j, k;
            for (i = 1; 3 * i + 1 < n; i++)
                for (j = 1; (k = i + j + 2 * i * j) < n && j <= i; j++)
                    a[k] = 1;
            for (i = n - 1; i >= 1; i--)
                if (a[i] == 0)
                    return (2 * i + 1);
            return 3;
        }
        bool ferma(int t, int n)
        {
            for (int i = 0; i < t; i++)
            {
                Random rand = new Random();
                int a = 0;
                do
                {
                    a = rand.Next() % 100;
                } while (!((a < n - 2) && (a > 2)));
                int r = 1;
                for (int j = 0; j < n - 1; j++)
                {
                    r *= a;
                    r %= n;
                }
                if (r != 1)
                    return false;
            }
            return true;
        }
        int GetPrimeNumber()
        {
            Random rand = new Random();
            int p=0;
            //int k = 0;
            //do
            //{
            //   p = rand.Next() % 100;
            //   p = sundaram(p);
            //} while (p < 100);
          
            
            do
            {
                p = rand.Next() % 1000;
            } while (!ferma(20, p));
            return p;
        }
        
        public static int nod(int a, int b)
        {
            int c;
            while (b > 0)
            {
                c = a % b;
                a = b;
                b = c;
            }
            return Math.Abs(a);
        }
    }
    class Cryptor
    {
        public int[] Encrypt(string str, int e, int n)
        {

            int[] CryptoText = new int[str.Length];
            long b = 301;
            long c;
            for (int j = 0; j < str.Length; j++)
            {
                c = 1;
                long i = 0;
                long ASCIIcode = GetNumberSymbol(str[j]) + b;
                while (i < e)
                {
                    c = c * ASCIIcode;
                    c = c % n;
                    i++;
                }
                CryptoText[j] = Convert.ToInt32(c);

                b += 1;
            }
            return CryptoText;
        }
        public int[] Encrypt(string str, key PublicKey)
        {
            return Encrypt(str, PublicKey.GetKey(), PublicKey.GetN());
        }
        public string Decrypt(int[] CryptoText, int d, int n)
        {
            int[] Tdecrypt = new int[CryptoText.Length];
            long b = 301;
            long m;
            string str = "";
            for (int j = 0; j < CryptoText.Length; j++)
            {
                m = 1;
                long i = 0;
                while (i < d)
                {
                    m = m * CryptoText[j];
                    m = m % n;
                    i++;
                }
                m = m - b;
                Tdecrypt[j] = Convert.ToInt32(m);
                b += 1;
                str += GetSymbolNumber(Tdecrypt[j]);
            }
            return str;
        }
        public string Decrypt(int[] CryptoText, key PrivateKey)
        {
            return Decrypt(CryptoText, PrivateKey.GetKey(), PrivateKey.GetN());
        }
        public static char GetSymbolNumber(double number)
        {
            char[] symbols = new char[] { '.', ',', ':', ';', '#', '%', '*', '/', '(', ')', '[', ']', '{', '}', '&', '@', '!', '?', '<', '>' };
            char[] nums = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char letter = '0';
            int i = 0;
            for (int symbol = 'а'; symbol <= 'я'; ++symbol)
            {
                letter = (char)symbol;
                if (i == number - 1) return letter;
                i++;
            }
            for (int symbol = 'a'; symbol <= 'z'; ++symbol)
            {
                letter = (char)symbol;
                if (i == number - 1) return letter;
                i++;
            }
            for (int j = 0; j < symbols.Length; j++)
                if (j + 60 == number) return symbols[j];
            for (int j = 0; j < nums.Length; j++)
                if (j + 80 == number) return nums[j];
            if (number == 0) return ' ';
            return letter;
        }
        public static int GetNumberSymbol(char s)
        {
            char[] symbols = new char[] { '.', ',', ':', ';', '#', '%', '*', '/', '(', ')', '[', ']', '{', '}', '&', '@', '!', '?', '<', '>' };
            char[] nums = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char letter = '0';
            int i = 0;
            for (int symbol = 'а'; symbol <= 'я'; ++symbol)
            {
                letter = (char)symbol;
                if (letter == s) return i + 1;
                i++;
            }
            for (int symbol = 'a'; symbol <= 'z'; ++symbol)
            {
                letter = (char)symbol;
                if (letter == s) return i + 1;
                i++;
            }
            for (int j = 0; j < symbols.Length; j++)
                if (s == symbols[j]) return j + 60;
            for (int j = 0; j < nums.Length; j++)
                if (s == nums[j]) return j + 80;
            if (s == ' ') return 0;
            return i;
        }
    }
}

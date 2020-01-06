using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication13
{
    class Program
    {
        public static int sundaram(int n)
        {
            int[] a = new int[n];
            int i, j, k;
            for (i = 1; 3 * i + 1 < n; i++)
            {
                for (j = 1; (k = i + j + 2 * i * j) < n && j <= i; j++)
                    a[k] = 1;
            }
            for (i = n - 1; i >= 1; i--)
                if (a[i] == 0)
                {
                    return (2 * i + 1);
                }
            return 3;
        }

        public static int nod(int a, int b)
        {
            int c;
            while (b>0)
            {
                c = a % b;
                a = b;
                b = c;
            }
            return Math.Abs(a);
        }






        public static char GetSymbolNumber(double number)//symbol
        {
            char[] symbols = new char[] { '.', ',', ':',';','#','%','*','/','(',')', '[', ']', '{', '}', '&', '@', '!', '?', '<', '>' };
            char[] nums = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char letter = '0';
            int i = 0;
            for (int symbol = 'а'; symbol <= 'я'; ++symbol)
            {
                letter = (char)symbol;
                if (i == number - 1)  return letter; 
                i++;
            }
            for (int symbol = 'a'; symbol <= 'z'; ++symbol)
            {
                letter = (char)symbol;
                if (i == number - 1)  return letter; 
                i++;
            }
            for (int j =0; j<symbols.Length; j++)
            {
                
                if (j+60 == number) return symbols[j];
                //i++;
            }
            for (int j = 0; j < nums.Length; j++)
            {

                if (j + 80 == number) return nums[j];
                //i++;
            }
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
                if (letter == s)  { Console.WriteLine("Russian" + (i + 1));return i + 1; };
                i++;
            }
            for (int symbol = 'a'; symbol <= 'z'; ++symbol)
            {

                letter = (char)symbol;
                if (letter == s) { Console.WriteLine("English" + (i + 1)); return i + 1; };
                i++;
            }
            for (int j=0;j<symbols.Length; j++)
            {

                if (s == symbols[j]) return j + 60;
            }
            for (int j = 0; j < nums.Length; j++)
            {

                if (s == nums[j]) return j + 80;
            }
            if (s == ' ') return 0;
            return i;
        }
        public static int GetPrimeNumber()
        {
            Random rand = new Random();
            int p;
            do
            {
                p = rand.Next() % 100;
                p = sundaram(p);
            } while (p < 10);
            
            return p;
        }
        public static int GetD(int fn)
        {
            Random rand = new Random();
            int d = 0;
            do
            {
                d = rand.Next() % 100;
                
            } while (nod(d, fn) != 1);
            return d;
        }
        public static int GetE(int fn,int d)
        {
            int e = 0;
            do
            {
                e += 1;
                
            } while ((e * d) % (fn)!=1);
            //Console.WriteLine("e=" + e);
            return e;
        }
       
        static void Main(string[] args)
        {
            Random rand = new Random();
            int p;
            int q;
            do
            {
                 p = GetPrimeNumber();
                 q = GetPrimeNumber();
            } while (p == q);
            Console.WriteLine("p=" + p);
            Console.WriteLine("q=" + q);
           
            int n = p * q;
            Console.WriteLine("n=" + n);

            int fn = (p - 1) * (q - 1);
            Console.WriteLine("fn=" + fn);

            int d = GetD(fn);
            Console.WriteLine("d=" + d);

            int e = GetE(fn, d);

            string Text = "i love you c#, но php тоже не плох!";
            int MAX = Text.Length;
            int[]CryptoText = new int[MAX];
            int[] Tdecrypt = new int[MAX];
            int b = 301;
            int c;
            for (int j = 0; j < MAX; j++)
            {
                c = 1;
                long i = 0;
                int ASCIIcode = GetNumberSymbol(Text[j])+b;
                while (i < e)
                {
                    c = c * ASCIIcode;
                    c = c % n;
                    i++;
                }
                CryptoText[j] = c;
               Console.WriteLine(CryptoText[j]);
                b += 1;
            }


///////////////////////////////////////////////////
            b = 301;
            int m;
            for (int j = 0; j < MAX; j++)
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
                Tdecrypt[j] = m;
                b += 1;
                Console.WriteLine(GetSymbolNumber(Tdecrypt[j]));
            }
          
        }
        
    }
}

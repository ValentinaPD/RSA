using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cipher
{
    static class Cipher
    {
        static int[] Zero = new int[] { 0 };
        static int[] One = new int[] { 1 };


        static int[] PrimeNumbers = new int[]
        {
            5, 7, 11, 13, 17, 19, 23
        };

        //Return a random prime number
        internal static int[] GetPrimeNumber(int length)
        {
            return null;
            //Define an empty int array
            int[] number = new int[length];

            bool is_multiply_of = false;

            int counter;

            while (true)
            {
                is_multiply_of = false;

                #region Cut off multiples of three
                counter = 0;

                for (int i = 0; i < length - 1; i++)
                {
                    //Fill the array with random bits
                    number[i] = new Random().Next() % 2;

                    if (number[i] == 1)
                    {
                        if (i % 2 == 0)
                        {
                            counter++;
                            continue;
                        }
                        counter--;
                    }
                }
                if (counter % 3 == 0)
                {
                    continue;
                }
                #endregion

                for (int i = 0; i < PrimeNumbers.Length; i++)
                {
                    //TODO: Convenient increment is required
                    if (IsMultiplyOf(number, DecimalToBinary(PrimeNumbers[i])))
                    {
                        is_multiply_of = true;
                        break;
                    }
                }
                if (!is_multiply_of)
                {
                    break;
                }
            }
            //Cut off even numbers
            number[length - 1] = 1;
        }


        internal static (int[], int[]) DivideBinaryNumber(int[] dividend, int[] divider)
        {
            if (dividend.Length < divider.Length)
            {
                return (Zero, dividend);
            }

            if (dividend.Length == divider.Length)
            {
                for (int i = 0, j = dividend.Length - divider.Length; i < divider.Length; i++, j++)
                {
                    if (dividend[j] > divider[i])
                    {
                        break;
                    }
                    else if (dividend[j] < divider[i])
                    {
                        return (Zero, dividend);
                    }
                    if (j == dividend.Length - 1)
                    {
                        return (One, Zero);
                    }
                }
            }

            int[] div = new int[dividend.Length + divider.Length];
            int[] mod = null;

            int div_offset = 0;
            int offset = 0;
            int fail_offset = 0;
            int counter = 0;
            int offset_counter = 0;
            bool is_first_offset = true;
            int last_mod_index = 0;
            while (true)
            {
                try
                {
                    mod = SubtractBinaryNumber(TakeRange(dividend, 0, divider.Length + offset), divider);
                    mod = RemoveLeadingZeros(mod);
                    div[div_offset] = 1;
                    div_offset++;
                    counter++;
                    is_first_offset = false;
                }
                catch (Exception e)
                {
                    if (e.Message == "The subtrahend cannot be more than the minuend")
                    {
                        offset++;
                        div[div_offset] = 0;
                        if (!is_first_offset)
                        {
                            div_offset++;
                        }
                        continue;
                    }
                    else
                    {
                        if (offset == 0)
                        {
                            div_offset++;
                        }
                        break;
                    }
                }

                Fill(ref dividend, 0, divider.Length + offset, 0);

                for (int i = 0, j = offset + divider.Length - mod.Length; i < mod.Length; j++, i++)
                {
                    dividend[j] = mod[i];
                    last_mod_index = j;
                }

                if(dividend[last_mod_index] == 0)
                {
                    dividend = TakeRange(dividend, last_mod_index + 1, dividend.Length - last_mod_index - 1);
                }
                else
                {
                    dividend = TakeRange(dividend, last_mod_index, dividend.Length - last_mod_index);
                }

                if (IsZero(dividend) && dividend.Length == 1)
                {
                    break;
                }
                offset = 0;
            }

            div = RemoveLeadingZeros(div);
            div = TakeRange(div, 0, div_offset);
            mod = RemoveLeadingZeros(dividend);

            return (div, mod);
        }


        

        internal static int[] SubtractBinaryNumber(int[] minuend, int[] subtrahend)
        {
            minuend = RemoveLeadingZeros(minuend);
            if (minuend.Length < subtrahend.Length || IsZero(minuend))
            {
                throw new Exception("The subtrahend cannot be more than the minuend");
            }

            if (minuend.Length == subtrahend.Length)
            {
                for (int i = 0; i < subtrahend.Length; i++)
                {
                    if (minuend[i] > subtrahend[i])
                    {
                        break;
                    }
                    else if (minuend[i] < subtrahend[i])
                    {
                        throw new Exception("The subtrahend cannot be more than the minuend");
                    }
                    if (i == minuend.Length - 1)
                    {
                        return Zero;
                    }
                }
            }

            for (int i = subtrahend.Length - 1,
                     k = minuend.Length - 1, j, j1; i > -1; k--, i--)
            {
                j = i;
                j1 = k;
                if (minuend[j1] < subtrahend[j])
                {
                    while (minuend[j1] < 1)
                    {
                        minuend[j1]++;
                        j1--;
                    }
                    minuend[j1] = 0;
                    minuend[k] = 2;
                }
                minuend[k] -= subtrahend[i];
            }

            return minuend;
        }

        internal static int[] TakeRange(int[] array, int start, int amount)
        {
            int[] res = new int[amount];

            for (int i = start, j = 0; i < start + amount; i++, j++)
            {
                res[j] = array[i];
            }

            return res;
        }

        internal static int[] GetBinaryMultiply(int[] multiplicand, int[] factor)
        {
            throw new NotImplementedException();
        }

        internal static int[] RemoveLeadingZeros(int[] num)
        {
            int i = 0;
            while (i < num.Length && num[i] == 0)
            {
                i++;
            }
            if (i == num.Length)
            {
                return Zero;
            }
            int[] result = new int[num.Length - i];

            for (int cucumber = i; cucumber < num.Length; cucumber++)
            {
                result[cucumber - i] = num[cucumber];
            }

            return result;
        }

        internal static int[] DecimalToBinary(int dec)
        {
            int result_length = 1;

            while (dec >= Math.Pow(2, result_length))
            {
                result_length++;
            }

            int[] res = new int[result_length];

            for (int i = result_length - 1; i > -1; i--)
            {
                res[i] = dec % 2;
                dec /= 2;
            }

            return res;
        }

        internal static int BinaryToDecimal(int[] binary)
        {
            int res = 0;

            for (int i = 0; i < binary.Length; i++)
            {
                res += binary[i] * (int)Math.Pow(2, binary.Length - i - 1);
            }

            return res;
        }

        internal static void Fill(ref int[] binary, int start, int amount, int value)
        {
            for (int i = start; i < start + amount; i++)
            {
                binary[i] = value;
            }
        }

        internal static bool IsZero(int[] binary)
        {
            for (int i = 0; i < binary.Length; i++)
            {
                if (binary[i] == 1)
                {
                    return false;
                }
            }
            return true;
        }

        internal static int LastIndexOf(int[] array, int required_num)
        {
            for (int i = array.Length - 1; i > -1; i--)
            {
                if (array[i] == required_num)
                {
                    return i;
                }
            }

            return -1;
            //throw new Exception("Required number not found");
        }
    }
}

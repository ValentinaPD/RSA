using System;

namespace BinOps
{
    public static class BinOps
    {
        static int[] One = new int[] { 1 };
        static int[] Zero = new int[] { 0 };

        public static (int[], int[]) DivideBinaryNumber(int[] dividend, int[] divider)
        {
            if (dividend.Length < divider.Length)
            {
                return (Zero, dividend);
            }

            //Compare the first zero`s positions
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

            int[] div = new int[dividend.Length + divider.Length]; //Result
            int[] mod = null;                                      //Modulo
            bool[] was_divided = new bool[dividend.Length];        //The part of divident, which has already been divided (true)
            int[] current_minuend = null;                          //The part of divident from which divider is subtracted

            //Get the first minuend
            //For example: in 10010 / 111
            //current_minuend == 1001
            try
            {
                SubtractBinaryNumber(TakeRange(dividend, 0, divider.Length), divider);
                current_minuend = TakeRange(dividend, 0, divider.Length);
            }
            catch
            {
                current_minuend = TakeRange(dividend, 0, divider.Length + 1);
            }

            int div_offset = 0;             //The offset in the result
            int last_zero_index = -1;
            while (true)
            {
                try
                {
                    mod = SubtractBinaryNumber(current_minuend, divider);
                    mod = RemoveLeadingZeros(mod);
                    div[div_offset] = 1;
                    div_offset++;
                }
                catch (Exception e)
                {
                    //The last subtraction, it is impossible to subtract anymore
                    if (e.Message == "The subtrahend cannot be more than the minuend" && LastIndexOf(was_divided, false) == -1)
                    {
                        div[div_offset] = 0;
                        div_offset++;
                        break;
                    }
                    else //The offset can be incremented, continue to subtract
                    {
                        div[div_offset] = 0;
                        div_offset++;
                        mod = current_minuend;
                    }
                }

                Fill(ref dividend, 0, current_minuend.Length, 0);
                Fill(ref was_divided, 0, current_minuend.Length, true);

                for (int i = 0, j = current_minuend.Length - mod.Length; i < mod.Length; j++, i++)
                {
                    dividend[j] = mod[i]; //Write the result of subtraction into the dividend
                }

                for (int i = 0; i < dividend.Length; i++)
                {
                    if (dividend[i] == 1 || !was_divided[i])
                    {
                        break;
                    }
                    if (was_divided[i])
                    {
                        last_zero_index = i;
                    }
                }

                //Remove the parts which have already been divided
                dividend = TakeRange(dividend, last_zero_index + 1, dividend.Length - last_zero_index - 1);
                was_divided = TakeRange(was_divided, last_zero_index + 1, was_divided.Length - last_zero_index - 1);

                //If dividend contains only zero`s
                if (IsZero(dividend))
                {
                    for (int i = 0; i < dividend.Length; i++)
                    {
                        //Find all parts which has not yet been divided
                        //For example: 1100 / 11
                        //Two last zero`s will be considered
                        if (!was_divided[i])
                        {
                            div_offset++;
                        }
                    }
                    break;
                }

                //If the all parts of dividend have been divided
                if (LastIndexOf(was_divided, true) == dividend.Length - 1)
                {
                    break;
                }
                else
                {
                    //Try to take the next numeral from divident 
                    int ind = FirstIndexOf(was_divided, false);
                    if (ind != -1)
                    {
                        current_minuend = new int[mod.Length + 1];
                        for (int i = 0; i < mod.Length; i++)
                        {
                            current_minuend[i] = mod[i];
                        }
                        current_minuend[mod.Length] = dividend[ind];
                        current_minuend = RemoveLeadingZeros(current_minuend);
                    }
                    else
                    {
                        div[div_offset] = 0;
                        div_offset++;
                        break;
                    }
                }

                last_zero_index = -1;
            }

            div = RemoveLeadingZeros(div);
            div = TakeRange(div, 0, div_offset);
            mod = RemoveLeadingZeros(dividend);

            return (div, mod);
        }

        public static int[] SubtractBinaryNumber(int[] minuend, int[] subtrahend)
        {
            minuend = RemoveLeadingZeros(minuend);

            if (minuend.Length < subtrahend.Length || IsZero(minuend))
            {
                throw new Exception("The subtrahend cannot be more than the minuend");
            }

            //Compare the first zero`s positions
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
                    //Transfer 1 to low order
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

        public static int[] GetBinaryMultiply(int[] multiplicand, int[] factor)
        {
            if (IsZero(multiplicand) || IsZero(factor))
            {
                return Zero;
            }

            multiplicand = RemoveLeadingZeros(multiplicand);
            factor = RemoveLeadingZeros(factor);

            int[] res = Zero;
            int[] current = null;

            if (multiplicand.Length > factor.Length)
            {
                for (int i = factor.Length - 1; i > -1; i--)
                {
                    if (factor[i] != 0)
                    {
                        current = new int[multiplicand.Length + (factor.Length - 1 - i)];
                        for (int j = 0; j < multiplicand.Length; j++)
                        {
                            current[j] = multiplicand[j];
                        }
                        res = SumBinaryNumbers(res, current);
                    }
                }
            }
            else
            {
                for (int i = multiplicand.Length - 1; i > -1; i--)
                {
                    if (multiplicand[i] != 0)
                    {
                        current = new int[factor.Length + (multiplicand.Length - 1 - i)];
                        for (int j = 0; j < factor.Length; j++)
                        {
                            current[j] = factor[j];
                        }
                        res = SumBinaryNumbers(res, current);
                    }
                }
            }

            return res;
        }

        public static int[] SumBinaryNumbers(int[] first, int[] second)
        {
            if (IsZero(first))
            {
                return second;
            }
            else if (IsZero(second))
            {
                return first;
            }

            first = RemoveLeadingZeros(first);
            second = RemoveLeadingZeros(second);

            int[] res = null;


            int i = first.Length - 1;
            int j = second.Length - 1;
            int k;
            int m;

            if (first.Length > second.Length)
            {
                res = new int[first.Length + 1];
                k = first.Length;

                for (int l = 0; l < first.Length; l++)
                {
                    res[l + 1] = first[l];
                }

                for (; j > -1; j--, k--)
                {
                    m = k;
                    while (k > -1 && res[k] > 1)
                    {
                        res[k] = 0;
                        res[k - 1] += 1;
                        k--;
                    }
                    k = m;
                    res[k] += second[j];

                    m = k;
                    while (k > -1 && res[k] > 1)
                    {
                        res[k] = 0;
                        res[k - 1] += 1;
                        k--;
                    }
                    k = m;
                }
            }
            else
            {
                res = new int[second.Length + 1];
                k = second.Length;

                for (int l = 0; l < second.Length; l++)
                {
                    res[l + 1] = second[l];
                }

                for (; i > -1; i--, k--)
                {
                    m = k;
                    while (k > -1 && res[k] > 1)
                    {
                        res[k] = 0;
                        res[k - 1] += 1;
                        k--;
                    }
                    k = m;

                    res[k] += first[i];

                    m = k;
                    while (k > -1 && res[k] > 1)
                    {
                        res[k] = 0;
                        res[k - 1] += 1;
                        k--;
                    }
                    k = m;
                }
            }

            res = RemoveLeadingZeros(res);

            return res;
        }


        public static T[] TakeRange<T>(T[] array, int start, int amount)
        {
            T[] res = new T[amount];

            for (int i = start, j = 0; i < start + amount; i++, j++)
            {
                res[j] = array[i];
            }

            return res;
        }

        public static int[] RemoveLeadingZeros(int[] num)
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

        public static int[] DecimalToBinary(int dec)
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

        public static int BinaryToDecimal(int[] binary)
        {
            int res = 0;

            for (int i = 0; i < binary.Length; i++)
            {
                res += binary[i] * (int)Math.Pow(2, binary.Length - i - 1);
            }

            return res;
        }

        public static void Fill<T>(ref T[] binary, int start, int amount, T value)
        {
            for (int i = start; i < start + amount; i++)
            {
                binary[i] = value;
            }
        }

        public static bool IsZero(int[] binary)
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

        public static int LastIndexOf(int[] array, int required_num)
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

        public static int FirstIndexOf(bool[] array, bool required)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == required)
                {
                    return i;
                }
            }

            return -1;
        }

        public static int LastIndexOf(bool[] array, bool required)
        {
            for (int i = array.Length - 1; i > -1; i--)
            {
                if (array[i] == required)
                {
                    return i;
                }
            }

            return -1;
            //throw new Exception("Required number not found");
        }

        public static int FirstIndexOf(int[] array, int required_num)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == required_num)
                {
                    return i;
                }
            }

            return -1;
        }

    }
}

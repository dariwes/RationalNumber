using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RNumber
{
    delegate int Operation(int n, int m);

    class RationalNumber : IEquatable<RationalNumber>, IComparable<RationalNumber>
    {
        private readonly int n;
        private readonly int m; 
        private int sign = 1;

        public int N { get; }
        public int M { get; }

        public RationalNumber(int n, int m)
        {
            sign = Math.Sign(n);
            n = Math.Abs(n);

            int GCD = GreatestCommonDivisor(n, m);
            this.n = n / GCD;
            this.m = m / GCD;
        }

        public RationalNumber(int n) : this(n, 1) { }


        public RationalNumber(double number)
        {
            sign = Math.Sign(number);
            number = Math.Abs(number);
            m = 1;

            while (number % 1 != 0)
            {
                number *= 10;
                m *= 10;
            }

            n = (int)number;
            int GCD = GreatestCommonDivisor(n, m);

            n /= GCD;
            m /= GCD;
        }

        public static RationalNumber Parse(string strNumber)
        {
            RationalNumber number;

            if (Regex.Match(strNumber, @"^-*\d+\s*\/\s*\d+$").Success)
            {
                string[] digits = Regex.Split(strNumber, @"\D+");

                number = new RationalNumber(Convert.ToInt32(digits[0]), 
                                            Convert.ToInt32(digits[1]));
            }
            else if(Regex.Match(strNumber, @"^-*\d+,\d+$").Success)
            {
                number = new RationalNumber(Convert.ToDouble(strNumber));
            }
            else if(Regex.Match(strNumber, @"^-*\d+$").Success)
            {
                number = new RationalNumber(Convert.ToInt32(strNumber));
            }
            else
            {
                throw new FormatException("Incorrect number input.");
            }
            if (strNumber[0] == '-')
            {
                number.sign = -1;
            }
            return number;
        }

        private static int GreatestCommonDivisor(int num1, int num2)
        {
            while (num2 != 0)
            {
                int temp = num2;
                num2 = num1 % num2;
                num1 = temp;
            }
            return num1;
        }

        private static int LeastCommonMultiple(int num1, int num2) => 
            num1 * num2 / GreatestCommonDivisor(num1, num2);

        private static RationalNumber SelectOperation(RationalNumber num1, 
                RationalNumber num2, Operation operation)
        {
            int LCM = LeastCommonMultiple(num1.m, num2.m);
            int numerator = operation(num1.n * LCM / num1.m * num1.sign, 
                num2.n * LCM / num2.m * num2.sign);

            return new RationalNumber(numerator, LCM);
        }

        public bool Equals(RationalNumber other)
        {
            if (other is null)
            {
                return false;
            }

            if (this.n == other.n && this.m == other.m)
            {
                return true;
            }

            return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj is null)
            {
                return false;
            }

            var number = obj as RationalNumber;

            if (number is null)
            {
                return false;
            }
                
            return Equals(number);
        }

        public int CompareTo(RationalNumber other)
        {
            if (this.Equals(other))
            {
                return 0;
            }

            if (this.n * this.sign * other.m > other.n * other.sign * this.m)
            {
                return 1;
            }

            return -1;
        }

        public override string ToString()
        {
            int numerator = n * sign;

            if (m == 1 || n == 0)
            {
                return Convert.ToString(numerator);
            }

            return Convert.ToString(numerator + "/" + m);
        }

        public override int GetHashCode() => this.GetHashCode();
        
        public static RationalNumber operator +(RationalNumber num1, RationalNumber num2) => 
            SelectOperation(num1, num2, (x, y) => x + y);

        public static RationalNumber operator -(RationalNumber num1, RationalNumber num2) =>
            SelectOperation(num1, num2, (x, y) => x - y);

        public static RationalNumber operator *(RationalNumber num1, RationalNumber num2) =>
            new RationalNumber(num1.n * num1.sign * num2.n * num2.sign, num1.m * num2.m);

        public static RationalNumber operator /(RationalNumber num1, RationalNumber num2) =>
            new RationalNumber(num1.n * num1.sign * num2.m * num2.sign, num1.m * num2.n);

        public static RationalNumber operator ++(RationalNumber number) => number + (RationalNumber)1;

        public static RationalNumber operator --(RationalNumber number) => number - (RationalNumber)1;

        public static bool operator ==(RationalNumber num1, RationalNumber num2)
        {
            if (((object)num1) is null || ((object)num2) is null)
                return Object.Equals(num1, num2);

            return num1.Equals(num2);
        }

        public static bool operator !=(RationalNumber num1, RationalNumber num2)
        {
            if (((object)num1) is null || ((object)num2) is null)
                return !Object.Equals(num1, num2);

            return !(num1.Equals(num2));
        }

        public static bool operator >(RationalNumber num1, RationalNumber num2) =>
            num1.CompareTo(num2) == 1;

        public static bool operator <(RationalNumber num1, RationalNumber num2) =>
            num1.CompareTo(num2) == -1;

        public static bool operator >=(RationalNumber num1, RationalNumber num2) =>
            num1.CompareTo(num2) >= 0;

        public static bool operator <=(RationalNumber num1, RationalNumber num2) =>
            num1.CompareTo(num2) <= 0;

        public static explicit operator double(RationalNumber number)
        {
            string strNumber  = Convert.ToString(number.n / number.m) + ",";
            int numerator = number.n;

            for (int i = 0; i < 16; i++)
            {
                numerator = (numerator % number.m) * 10;
                strNumber += Convert.ToString(numerator / number.m);
            }

            return Math.Round(Convert.ToDouble(strNumber), 15) * number.sign;
        }

        public static explicit operator RationalNumber(double number) => 
            new RationalNumber(number);

        public static explicit operator int(RationalNumber number) =>
            (int)Math.Round((double)number, MidpointRounding.AwayFromZero);

        public static explicit operator RationalNumber(int number) => 
            new RationalNumber(number);
    }
}

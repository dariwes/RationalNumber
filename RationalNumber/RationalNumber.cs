using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RNumber
{
    delegate int Operation(int n, int m);

    class RationalNumber : IEquatable<RationalNumber>, IComparable<RationalNumber>
    {
        private int n;
        private int m;
        private int sign = 1;

        public int N 
        { 
            get => n;
            set
            {
                if(value < 0)
                {
                    sign = -1;
                }
                n = Math.Abs(value);
            }
        }
        public int M
        {
            get => m;
            set => m = value < 0 ? 1 : value;
        }

        public RationalNumber(int n, int m)
        {
            N = n;
            M = m;
        }

        public RationalNumber(int n) : this(n, 1) { }


        public RationalNumber(double num)
        {
            m = 1;

            if(num < 0)
            {
                sign = -1;
                num = Math.Abs(num);
            }

            while (num % 1 != 0)
            {
                num *= 10;
                m *= 10;
            }

            n = (int)num;
            int GCD = GreatestCommonDivisor(n, m);

            n /= GCD;
            m /= GCD;
        }

        public RationalNumber(string number)
        {
            if(Regex.Match(number, @"^-*\d+\s*\/\s*\d+$").Success)
            {
                string[] digits = Regex.Split(number, @"\D+");

                if(number[0] == '-')
                {
                    sign = -1;
                }
                int.TryParse(digits[0], out n);
                int.TryParse(digits[1], out m);
            }
            else if(Regex.Match(number, @"^-*\d+,\d+$").Success)
            {
                var temp = new RationalNumber(Convert.ToDouble(number));
                this.N = temp.n;
                this.M = temp.m;
                this.sign = temp.sign;
            }
            else if(Regex.Match(number, @"^-*\d+$").Success)
            {
                var temp = new RationalNumber(Convert.ToInt32(number));
                this.N = temp.n;
                this.M = temp.m;
                this.sign = temp.sign;
            }
            else
            {
                //Console.WriteLine("Invalid input");
            }
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

        public RationalNumber Reduce()
        {
            int GCD = GreatestCommonDivisor(this.n, this.m);
            this.n /= GCD;
            this.m /= GCD;
            return this;
        }

        private static RationalNumber SelectOperation(RationalNumber num1, 
                RationalNumber num2, Operation operation)
        {
            int LCM = LeastCommonMultiple(num1.m, num2.m);
            int new_n = operation(num1.n * LCM / num1.m * num1.sign,
                        num2.n * LCM / num2.m * num2.sign);

            return new RationalNumber(new_n, LCM);
        }

        public bool Equals(RationalNumber second_num)
        {
            if (second_num == null)
            {
                return false;
            }

            RationalNumber num1 = this.Reduce();
            RationalNumber num2 = second_num.Reduce();

            if (num1.n == num2.n && num1.m == num2.m)
            {
                return true;
            }

            return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            RationalNumber num = obj as RationalNumber;

            if (num == null)
            {
                return false;
            }
                
            return Equals(num);
        }

        public int CompareTo(RationalNumber second)
        {
            if (this.Equals(second))
            {
                return 0;
            }
            RationalNumber num1 = this.Reduce();
            RationalNumber num2 = second.Reduce();

            if (num1.n * num1.sign * num2.m > num2.n * num2.sign * num1.m)
            {
                return 1;
            }

            return -1;
        }

        public override string ToString()
        {
            if (this.n == 0)
            {
                return "0";
            }

            string result;

            if (this.sign < 0)
            {
                result = "-";
            }
            else
            {
                result = "";
            }
            if (this.n == this.m)
            {
                return result + "1";
            }
            if (this.m == 1)
            {
                return result + this.n;
            }
            return result + this.n + "/" + this.m;
        }

        public override int GetHashCode() => this.GetHashCode();
        
        public static RationalNumber operator +(RationalNumber num1, RationalNumber num2) => 
            SelectOperation(num1, num2, (x, y) => x + y);

        public static RationalNumber operator +(RationalNumber R_num, int I_num) =>
            R_num + new RationalNumber(I_num);

        public static RationalNumber operator +(int I_num, RationalNumber R_num) =>
            R_num + new RationalNumber(I_num);

        public static RationalNumber operator -(RationalNumber num1, RationalNumber num2) =>
            SelectOperation(num1, num2, (x, y) => x - y);

        public static RationalNumber operator -(RationalNumber R_num, int I_num) =>
            R_num - new RationalNumber(I_num);

        public static RationalNumber operator -(int I_num, RationalNumber R_num) =>
            new RationalNumber(I_num) - R_num;

        public static RationalNumber operator *(RationalNumber num1, RationalNumber num2) =>
            new RationalNumber(num1.n * num1.sign * num2.n * num2.sign, num1.m * num2.m);

        public static RationalNumber operator /(RationalNumber num1, RationalNumber num2) =>
            new RationalNumber(num1.n * num1.sign * num2.m * num2.sign, num1.m * num2.n);

        public static RationalNumber operator ++(RationalNumber number) => number + 1;

        public static RationalNumber operator --(RationalNumber number) => number - 1;

        public static bool operator ==(RationalNumber num1, RationalNumber num2)
        {
            if (((object)num1) == null || ((object)num2) == null)
                return Object.Equals(num1, num2);

            return num1.Equals(num2);
        }

        public static bool operator !=(RationalNumber num1, RationalNumber num2)
        {
            if (((object)num1) == null || ((object)num2) == null)
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

        public static implicit operator double(RationalNumber number) => 
            (double)number.n / number.m;

        public static implicit operator RationalNumber(double number) => 
            new RationalNumber(number);

        public static explicit operator int(RationalNumber number) => 
            number.n / number.m;

        public static explicit operator RationalNumber(int number) => 
            new RationalNumber(number);
    }
}

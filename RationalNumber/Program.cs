using System;
using System.Collections.Generic;

namespace RNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            int numI = 23;
            double numD = 3.5;

            List<RationalNumber> numbers = null;

            try
            {
                numbers = new List<RationalNumber>()
                {
                RationalNumber.Parse("-6,5"),
                RationalNumber.Parse("4/5"),
                new RationalNumber(12),
                RationalNumber.Parse("1/20"),
                new RationalNumber(0.5)
                };
            }
            catch(FormatException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            numbers.Sort();

            Console.WriteLine("***Demonstration of the work of the rational class***");

            Console.WriteLine("Sorted set of numbers:");
            foreach(RationalNumber number in numbers)
            {
                Console.Write($" {number} ");
            }

            Console.WriteLine($@"
            Addition:
            {numbers[0]} + {numbers[1]} = {numbers[0] + numbers[1]}
            Subtraction:
            {numbers[2]} - {numI} = {numbers[2] - (RationalNumber)numI}
            Multiplication:
            {numbers[3]} * {numD} = {numbers[3] * (RationalNumber)numD}
            Division:
            {numbers[4]} / {numbers[0]} = {numbers[4] / numbers[0]}"
            );

            Console.WriteLine($@"
            In different formats: (fraction) {numbers[0]}, (int) {(int)numbers[0]}, (double) {(double)numbers[0]}");
        }
    }
}

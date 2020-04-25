using System;
using System.Collections.Generic;

namespace RNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            int num_i = 23;
            double num_d = 3.5;

            List<RationalNumber> numbers = null;

            try
            {
                numbers = new List<RationalNumber>()
                {
                new RationalNumber("-6,5"),
                new RationalNumber("4/5"),
                new RationalNumber(12),
                new RationalNumber("1/20"),
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
            {numbers[2]} - {num_i} = {numbers[2] - (RationalNumber)num_i}
            Multiplication:
            {numbers[3]} * {num_d} = {numbers[3] * num_d}
            Division:
            {numbers[4]} / {numbers[0]} = {numbers[4] / numbers[0]}"
            );

            Console.WriteLine($@"
            In different formats: (fraction) {numbers[0]}, (int) {(int)numbers[0]}, (double) {(double)numbers[0]}");
        }
    }
}

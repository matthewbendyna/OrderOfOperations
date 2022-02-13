using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace OrderOfOperations
{
    public class Orderofoperations
    {
        static Cutarray cutarray = new Cutarray();

        static Regex operatorRx = new Regex(@"\+|\-|\*|\/|\^");
        static Regex digitsRx = new Regex(@"^\d+(\.\d+)?$", RegexOptions.IgnorePatternWhitespace);
        static Regex digitRx = new Regex(@"^\d$");

        static double[] numbers = new double[0];
        static char[] operators = new char[0];
        public double calculateString(string equation)
        {
            double answer = 0;
            equation = equation.Trim();
            bool parentheses = false;
            foreach (char c in equation)
            {
                string cs = c.ToString();
                if (!digitRx.IsMatch(cs))
                {
                    if (!operatorRx.IsMatch(cs))
                    {
                        if (cs == "(" || cs == ")")
                        {
                            parentheses = true;
                        }
                    }
                }
            }
            if (parentheses)
            {
                equation = dissectComplex(equation);
            }
            if (iseqFormat(equation))
            {
                buildArrays(equation);
                answer = calculate();
            }
            return answer;
        }
        private bool iseqFormat(string equation)
        {
            Regex equationRx = new Regex(@"^(\d+(\.\d+)?)(\s*)?((\+|\-|\*|\/|\^)(\s*)?(\d+(\.\d+)?))*$", RegexOptions.IgnorePatternWhitespace);
            if (equationRx.IsMatch(equation))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string dissectComplex(string equation)
        {
            while (equation.Contains('('))
            {
                int[] opening = new int[0];
                int[] closing = new int[0];
                for (int i = 0; i < equation.Length; i++)
                {
                    if (equation[i] == '(')
                    {
                        Array.Resize(ref opening, opening.Length + 1);
                        opening[opening.Length - 1] = i;
                    }
                    else if (equation[i] == ')')
                    {
                        if (closing.Length < opening.Length)
                        {
                            Array.Resize(ref closing, closing.Length + 1);
                            closing[closing.Length - 1] = i;
                        }
                        else
                        {
                            throw new Exception("Cannot close parentheses when none are opened");
                        }
                    }
                }
                if (opening.Length != closing.Length)
                {
                    throw new Exception("Error: The number of closing parentheses does not match the number of opening parentheses.");
                }
                int[] grouping = opening.Concat(closing).ToArray();
                Array.Sort(grouping);
                for (int i = 0; i < grouping.Length - 1; i++)
                {
                    if (opening.Contains(grouping[i]) && closing.Contains(grouping[i + 1]))
                    {
                        int start = grouping[i];
                        int end = grouping[i + 1];
                        equation = solveNested(equation, start, end);
                        break;
                    }
                }
            }
            return equation;
        }
        private string solveNested(string equation, int start, int end)
        {
            string nested_equation = "";
            string new_equation = "";
            int skip = 0;
            bool afterdigit = false;
            bool beforedigit = false;
            for(int i = 0; i<equation.Length; i++)
            {
                if (skip > 0)
                {
                        skip--;
                        continue;
                }
                if (Char.IsDigit(equation[i]))
                {
                        afterdigit = true;
                }
                else if (operatorRx.IsMatch(equation[i].ToString()))
                {
                        afterdigit = false;
                }
                if(equation[i] == ' ')
                {
                    continue;
                }
                if (i == start)
                {
                    if (end != start + 1)
                    {
                        for(int j = start; j >= 0; j--)
                        {
                            if (digitRx.IsMatch(equation[j].ToString()))
                            {
                                afterdigit = true;
                                break;
                            }
                            else if (operatorRx.IsMatch(equation[j].ToString()))
                            {
                                afterdigit = false;
                                break;
                            }
                        }
                        if (afterdigit)
                        {
                            new_equation += "*";
                        }
                        for (int j = start + 1; j < end; j++)
                        {
                            nested_equation += equation[j];
                        }
                        double answer = calculateString(nested_equation);
                        new_equation += answer.ToString();
                        for (int j = end; j < equation.Length; j++)
                        {
                            if (digitRx.IsMatch(equation[j].ToString()))
                            {
                                beforedigit = true;
                                break;
                            }
                            else if (operatorRx.IsMatch(equation[j].ToString()))
                            {
                                beforedigit = false;
                                break;
                            }
                        }
                        if (beforedigit || !afterdigit)
                        {
                            new_equation += "*";
                        }
                    }
                    else
                    {
                        throw new Exception("Cannot solve empty parentheses. Invalid equation format.");
                    }
                    skip = end - start;
                }
                else
                {
                        new_equation += equation[i];
                }
            }
            Console.WriteLine(new_equation); //debug
            return new_equation;
        }
        private void buildArrays(string equation)
        {
            string current_number = "";
            foreach(char digit in equation)
            {
                if (Char.IsDigit(digit) || digit == '.')
                {
                    current_number += digit;
                }
                else if(current_number != "")
                {
                    Array.Resize(ref numbers, numbers.Length + 1);
                    numbers[numbers.Length - 1] = Convert.ToInt32(current_number);
                    current_number = "";
                }
                if (digit == '+' || digit == '-' || digit == '*' || digit == '/' || digit == '^')
                {
                    Array.Resize(ref operators, operators.Length + 1);
                    operators[operators.Length - 1] = digit;
                }
            }
            Array.Resize(ref numbers, numbers.Length + 1);
            numbers[numbers.Length - 1] = Convert.ToInt32(current_number);
            if(numbers.Length != operators.Length + 1)
            {
                throw new Exception("There must be precisely one less value in the operators than the numbers of the equation");
            }
            return;
        }
        private double calculate()
        {
            double answer = 0;
            int phase = 0;
            int phasenumbers = 3;
            //execute calculation of exponents
            while (phase < phasenumbers)
            {
                if (numbers.Length == 1)
                {
                    break;
                }
                if (phase == 0) //phase 0 takes care of all exponents
                {
                    while (operators.Contains('^'))
                    {
                        for (int i = operators.Length-1; i >= 0; i--)
                        {
                            if (operators[i] == '^')
                            {
                                numbers[i] = Math.Pow(numbers[i], numbers[i + 1]);
                                numbers = cutarray.cutArray(numbers, i + 1);
                                operators = cutarray.cutArray(operators, i);
                                break;
                            }
                        }
                    }
                    phase++;
                }
                else if(phase == 1)
                {
                    while (operators.Contains('*') || operators.Contains('/'))
                    {
                        for (int i = operators.Length - 1; i >= 0; i--)
                        {
                            if (operators[i] == '*')
                            {
                                numbers[i] = numbers[i] * numbers[i + 1];
                                numbers = cutarray.cutArray(numbers, i + 1);
                                operators = cutarray.cutArray(operators, i);
                                break;
                            }
                            else if (operators[i] == '/')
                            {
                                numbers[i] = numbers[i] / numbers[i + 1];
                                numbers = cutarray.cutArray(numbers, i + 1);
                                operators = cutarray.cutArray(operators, i);
                                break;
                            }
                        }
                    }
                    phase++;
                }
                else if(phase == 2)
                {
                    while (operators.Contains('+') || operators.Contains('-'))
                    {
                        for (int i = operators.Length - 1; i >= 0; i--)
                        {
                            if (operators[i] == '+')
                            {
                                numbers[i] = numbers[i] + numbers[i + 1];
                                numbers = cutarray.cutArray(numbers, i + 1);
                                operators = cutarray.cutArray(operators, i);
                                break;
                            }
                            else if (operators[i] == '-')
                            {
                                numbers[i] = numbers[i] - numbers[i + 1];
                                numbers = cutarray.cutArray(numbers, i + 1);
                                operators = cutarray.cutArray(operators, i);
                                break;
                            }
                        }
                    }
                    phase++;
                }
            }
            if(numbers.Length == 1)
            {
                answer = Convert.ToDouble(numbers[0]);
                Array.Resize(ref numbers, 0);
                return answer;
            }
            else
            {
                throw new Exception("To many numbers in index to return a final solution");
            }
        }
    }
}
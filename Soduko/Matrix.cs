using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soduko
{
    class Matrix
    {
        int[,] matrix;
        int n, k;
        Condition[] conditions;

        public void Awake()
        {
            Console.WriteLine("Awaking Soduko");

            Console.Write("Enter N: ");
            n = Convert.ToInt32(Console.ReadLine());

            matrix = new int[n, n];

            Console.Write("Enter K: ");
            k = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Got N and K");
            conditions = new Condition[k];

            for (int i = 0; i < k; i++)
            {
                //Console.Write("Enter " + (i + 1) + "th condition: ");
                conditions[i] = conditionFromString(Console.ReadLine());

                /*if(conditions[i].operation == '=')
                {
                    int index = conditions[i].numbers[0];

                    int x = index % n;
                    int y = index / n;

                    matrix[y, x] = conditions[i].answer;
                }*/
            }
            Console.WriteLine("Got Conditions");

            //ShowMatrix();

            while (validateMatrix() == false)
            {
                Console.WriteLine("Generating Matrix");
                GenerateMatrix();
            }
            ShowMatrix();
            Console.ReadKey();
        }
        bool validateMatrix()
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                if (conditions[i].operation == '=')
                {
                    if (getNumber(conditions[i].numbers[0]) != conditions[i].answer)
                    {
                        return false;
                    }
                }
                else if (conditions[i].operation == '+')
                {
                    int sum = 0;

                    for (int j = 0; j < conditions[i].numbers.Length; j++)
                    {
                        sum += getNumber(conditions[i].numbers[j]);
                    }
                    if (sum != conditions[i].answer) return false;
                }
                else if (conditions[i].operation == '*')
                {
                    int multiplication = 1;

                    for (int j = 0; j < conditions[i].numbers.Length; j++)
                    {
                        multiplication *= getNumber(conditions[i].numbers[j]);
                    }
                    if (multiplication != conditions[i].answer) return false;
                }
                else if (conditions[i].operation == '-')
                {
                    int resault = getNumber(conditions[i].numbers[0]) - getNumber(conditions[i].numbers[1]);
                    if (resault < 0) resault = -resault;
                    if (resault != conditions[i].answer) return false;
                }
            }
            return true;
        }
        int getNumber(int index)
        {
            int x = index % n;
            int y = index / n;

            return matrix[y, x];
        }
        void GenerateMatrix()
        {
            matrix = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        int num = GenerateNumber(i, j);

                        if (num != 0)
                        {
                            matrix[i, j] = num;
                        }
                        else
                        {
                            GenerateMatrix();
                        }
                    }
                }
            }
        }
        int GenerateNumber(int i, int j)
        {
            List<int> availableNumberR = new List<int>();
            List<int> availableNumberC = new List<int>();

            for (int k = 0; k < n; k++)
            {
                if (matrix[i, k] != 0)
                {
                    availableNumberC.Add(matrix[i, k]);
                }
            }
            for (int k = 0; k < n; k++)
            {
                if (matrix[k, j] != 0)
                {
                    availableNumberR.Add(matrix[k, j]);
                }
            }
            List<int> possibleNumbersR = new List<int>();
            List<int> possibleNumbersC = new List<int>();

            for (int k = 1; k <= n; k++)
            {
                possibleNumbersR.Add(k);
                possibleNumbersC.Add(k);
            }

            for (int k = 0; k < availableNumberC.Count; k++)
            {
                possibleNumbersC.Remove(availableNumberC[k]);
            }
            for (int k = 0; k < availableNumberR.Count; k++)
            {
                possibleNumbersR.Remove(availableNumberR[k]);
            }

            List<int> possibleNumbers = possibleNumbersR.FindAll(x => possibleNumbersC.Contains(x));

            var random = new Random();
            int index = random.Next(possibleNumbers.Count);

            if (possibleNumbers.Count == 0)
            {
                return 0;
            }

            return possibleNumbers[index];
        }
        #region StringToCondition
        Condition conditionFromString(string userInput)
        {
            Condition c = new Condition();

            c.amount = 0;
            c.answer = 0;

            bool gettingAmount = true, gettingNumbers = false, gettingOperation = false, gettingAnswer = false;
            int numberIndex = 0;

            for (int i = 0; i < userInput.Length; i++)
            {
                if (userInput[i] != ' ')
                {
                    if (gettingAmount)
                    {
                        c.amount = c.amount * 10 + charToInt(userInput[i]);
                    }
                    else if (gettingNumbers)
                    {
                        c.numbers[numberIndex] = c.numbers[numberIndex] * 10 + charToInt(userInput[i]);
                    }
                    else if (gettingOperation)
                    {
                        c.operation = userInput[i];
                    }
                    else
                    {
                        c.answer = c.answer * 10 + charToInt(userInput[i]);
                    }
                }
                else
                {
                    if (gettingAmount)
                    {
                        gettingAmount = false;
                        gettingNumbers = true;

                        c.numbers = new int[c.amount];
                    }
                    else if (gettingNumbers)
                    {
                        numberIndex++;

                        if (numberIndex == c.amount)
                        {
                            gettingNumbers = false;
                            gettingOperation = true;
                        }
                    }
                    else if (gettingOperation)
                    {
                        gettingOperation = false;
                        gettingAnswer = true;
                    }
                    else if (gettingAnswer)
                    {
                        break;
                    }
                }
            }

            return c;
        }
        int charToInt(char c)
        {
            if (c == '0') return 0;
            else if (c == '1') return 1;
            else if (c == '2') return 2;
            else if (c == '3') return 3;
            else if (c == '4') return 4;
            else if (c == '5') return 5;
            else if (c == '6') return 6;
            else if (c == '7') return 7;
            else if (c == '8') return 8;
            else return 9;
        }
        #endregion
        void intToVector(int index)
        {
            index++;

            int x = index % n;
            int y = index / n + 1;
        }
        void ShowMatrix()
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(matrix[i, j]);
                }
                Console.Write("\n");
            }
        }
    }
    class Condition
    {
        public int amount;
        public int[] numbers;
        public char operation;
        public int answer;

        public void outputCondition()
        {
            Console.WriteLine("Condition Amount: " + amount);
            Console.Write("Condition Numbers: ");

            for (int i = 0; i < amount; i++)
            {
                Console.Write(numbers[i] + " ");
            }
            Console.WriteLine("Condition Operation: " + operation);
            Console.WriteLine("Condition Answer: " + answer);
        }
    }
}

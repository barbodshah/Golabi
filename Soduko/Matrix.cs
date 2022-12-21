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
        int[] backTrancker;

        public void Awake()
        {
            Console.WriteLine("Awaking Soduko");

            Console.Write("Enter N: ");
            n = Convert.ToInt32(Console.ReadLine());

            matrix = new int[n, n];
            backTrancker = new int[n * n];

            Console.Write("Enter K: ");
            k = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Got N and K");
            conditions = new Condition[k];

            for (int i = 0; i < k; i++)
            {
                //Console.Write("Enter " + (i + 1) + "th condition: ");
                conditions[i] = conditionFromString(Console.ReadLine());

                /*if (conditions[i].operation == '=')
                {
                    int index = conditions[i].numbers[0];

                    int x = index % n;
                    int y = index / n;

                    matrix[y, x] = conditions[i].answer;
                }*/
            }
            Console.WriteLine("Got Conditions");

            GenerateMatrix();

            Console.Write("\n \nFinal Matrix __________________\n");
            ShowMatrix();

            Console.ReadKey();
        }
        bool validateMatrix()
        {
            //Validate the matrix for conditions if the needed cells are filled
            for (int i = 0; i < conditions.Length; i++)
            {
                bool filledNumbers = true;

                for (int j = 0; j < conditions[i].numbers.Length; j++)
                {
                    if (getNumber(conditions[i].numbers[j]) == 0)
                    {
                        Console.WriteLine("Condition " + i + " not filled");

                        filledNumbers = false;
                        break;
                    }
                }
                if (filledNumbers)
                {
                    if (conditions[i].operation == '+')
                    {
                        int sum = 0;

                        for (int j = 0; j < conditions[i].numbers.Length; j++)
                        {
                            sum += getNumber(conditions[i].numbers[j]);
                        }
                        if (sum != conditions[i].answer)
                        {
                            Console.WriteLine("Condition " + i + " invalid");
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("Condition " + i + " valid");
                        }
                    }
                    else if (conditions[i].operation == '*')
                    {
                        int multiplication = 1;

                        for (int j = 0; j < conditions[i].numbers.Length; j++)
                        {
                            multiplication *= getNumber(conditions[i].numbers[j]);
                        }
                        if (multiplication != conditions[i].answer)
                        {
                            Console.WriteLine("Condition " + i + " invalid");
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("Condition " + i + " valid");
                        }
                    }
                    else if (conditions[i].operation == '-')
                    {
                        int resault = getNumber(conditions[i].numbers[0]) - getNumber(conditions[i].numbers[1]);
                        if (resault < 0) resault = -resault;

                        if (resault != conditions[i].answer)
                        {
                            Console.WriteLine("Condition " + i + " invalid");
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("Condition " + i + " valid");
                        }
                    }
                    else if(conditions[i].operation == '/')
                    {
                        int resault1 = getNumber(conditions[i].numbers[0]) / getNumber(conditions[i].numbers[1]);
                        int resault2 = getNumber(conditions[i].numbers[1]) / getNumber(conditions[i].numbers[0]);

                        if (resault1 != conditions[i].answer && resault2 != conditions[i].answer)
                        {
                            Console.WriteLine("Condition " + i + " invalid");
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("Condition " + i + " valid");
                        }
                    }
                    else if(conditions[i].operation == '=')
                    {
                        int resault = getNumber(conditions[i].numbers[0]);

                        if(resault != conditions[i].answer)
                        {
                            Console.WriteLine("Condition " + i + " invalid");
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("Condition " + i + " valid");
                        }
                    }
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
        #region Generate
        void GenerateMatrix()
        {
            int index = 0;

            while(index < n * n)
            {
                for (int i = index + 1; i < n * n; i++)
                {
                    backTrancker[i] = 0;
                }

                int x = index % n;
                int y = index / n;

                if (matrix[y, x] == 0)
                {
                    int number = GenerateNumber(y, x, backTrancker[index]);
                    Console.WriteLine(index);

                    if (number == 0)
                    {
                        index--;
                        //backTrancker[index]++;

                        matrix[index / n, index % n] = 0;
                    }
                    else
                    {
                        if (validateMatrix())
                        {
                            matrix[y, x] = number;
                            backTrancker[index]++;
                            index++;
                        }
                        else
                        {
                            index--;
                            //backTrancker[index]++;

                            matrix[index / n, index % n] = 0;
                        }
                    }
                }
                else
                {
                    backTrancker[index]++;
                    index++;
                }
                ShowMatrix();
                for(int i = 0; i < n * n; i++)
                {
                    Console.Write(" " + backTrancker[i]);
                }
                Console.Write("\n");
                //Console.ReadKey();
            }
        }
        int GenerateNumber(int i, int j, int p)
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

            if (possibleNumbers.Count <= p)
            {
                return 0;
            }
            return possibleNumbers[p];
        }
        #endregion
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
        #region Debug
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
        #endregion
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

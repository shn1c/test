using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauss
{
    class Program
    {
        static void Main()
        {
            string matrix_path = @".../.../matrix2.txt";            
            int n;
            StreamReader sr = new StreamReader(matrix_path, System.Text.Encoding.Default);
            n = Convert.ToInt32(sr.ReadLine());
                                    
            double[,] A = new double[n, n];

            //Зчитування матриці            
            for (int i = 0; i < n; i++)
            {
                string[] line = sr.ReadLine().Split();
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = Convert.ToInt32(line[j]);
                }
            }

            //Зчитування розв'язків
            double[] B = new double[n];
            string[] solutions_line = sr.ReadLine().Split();
            for (int i = 0; i < n; i++)
            {
                B[i] = Convert.ToInt32(solutions_line[i]);
            }
            
            double[] result = GaussMethod(A, B, n);
            
            Console.WriteLine("Розв'язки СЛАР:");           

            for (int j = 0; j < result.Length; j++)
            {
                Console.WriteLine($"X{j + 1} = {result[j]}");                                                               
            }
            
            Console.WriteLine($"\nДетермiнант матрицi: {Determinant(A)}");            

            Console.ReadLine();
        }

        static double[] GaussMethod(double[,] A, double[] B, int n)
        {

            //зведення до трикутного вигляду
            for (int i = 0; i < n - 1; i++)
            {
                for (int k = i + 1; k < n; k++)
                {
                    double c = A[i, i] / Math.Sqrt(A[i, i] * A[i, i] + A[k, i]);
                    double s = A[k, i] / Math.Sqrt(A[i, i] * A[i, i] + A[k, i]);
                    for (int j = 0; j < n; j++)
                    {
                        double a1 = A[i, j];
                        double a2 = A[k, j];
                        A[i, j] = c * a1 + s * a2;
                        A[k, j] = -s * a1 + c * a2;
                    }
                    double b1 = B[i];
                    double b2 = B[k];
                    B[i] = c * b1 + s * b2;
                    B[k] = -s * b1 + c * b2;
                }
            }
            //вивід трикутної матриці
            /*Console.Write("\nThe matrix is : \n"); 
           for (int i = 0; i < n; i++)
           {
               Console.Write("\n");
               for (int j = 0; j < 3; j++)
                   Console.Write("{0}\t", A[i, j]);
           }
           Console.Write("\n");
           for (int i = 0; i < n; i++)
           {
               Console.WriteLine("{0}\t", B[i]);
           }*/

            //знаходження фінальних розв'язків
            for (int i = n - 1; i >= 0; i--)
            {
                for (int j = i + 1; j < n; j++)
                {
                    B[i] -= i != j ? A[i, j] * B[j] : 0;
                }
                B[i] = B[i] / A[i, i];
            }
            return B;
        }

        //Визначення знаку елемента
        static int SignOfElement(int i, int j)
        {
            if ((i + j) % 2 == 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        //Визначення мінора за елементом
        static double[,] CreateSmallerMatrix(double[,] input, int i, int j)
        {
            int order = int.Parse(System.Math.Sqrt(input.Length).ToString());
            double[,] output = new double[order - 1, order - 1];
            int x = 0, y = 0;
            for (int m = 0; m < order; m++, x++)
            {
                if (m != i)
                {
                    y = 0;
                    for (int n = 0; n < order; n++)
                    {
                        if (n != j)
                        {
                            output[x, y] = input[m, n];
                            y++;
                        }
                    }
                }
                else
                {
                    x--;
                }
            }
            return output;
        }
        //Визначає значення детермінанта за допомогою рекурсії
        static double Determinant(double[,] input)
        {
            int order = int.Parse(System.Math.Sqrt(input.Length).ToString());
            if (order > 2)
            {
                double value = 0;
                for (int j = 0; j < order; j++)
                {
                    double[,] Temp = CreateSmallerMatrix(input, 0, j);
                    value = value + input[0, j] * (SignOfElement(0, j) * Determinant(Temp));
                }
                return value;
            }
            else if (order == 2)
            {
                return ((input[0, 0] * input[1, 1]) - (input[1, 0] * input[0, 1]));
            }
            else
            {
                return (input[0, 0]);
            }
        }

    }

}


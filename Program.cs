using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaussian_elimination
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"...\...\matrix1.txt";
            FileStream streamToRead = new FileStream(path, FileMode.Open);
            StreamReader reader = new StreamReader(streamToRead);

            int size = Convert.ToInt32(reader.ReadLine());
            double[,] matrix = new double[size, size + 1];

            for (int rows = 0; rows < size; rows++)
            {
                string[] array = reader.ReadLine().Split(' ');
                for (int cols = 0; cols < size + 1; cols++)
                {
                    matrix[rows, cols] = Convert.ToDouble(array[cols]);
                }
            }

            streamToRead.Close();

            Console.WriteLine("determinant: ");
            Console.WriteLine(CalculateDeterminant(matrix, size));
            Console.WriteLine();

            DisplayMatrix(matrix, size - 1);

            List<double> result = new List<double>();
            GaussianElimination(ref matrix, size - 1, ref result);



            FileStream streamToWrite = new FileStream(path, FileMode.Append);
            StreamWriter writer = new StreamWriter(streamToWrite);



            writer.WriteLine("xVector : ");
            foreach (double x in result)
            {
                Console.WriteLine(x);
                writer.WriteLine($"{x}, ");
            }
            Console.ReadKey();

        }

        static void GaussianElimination(ref double[,] matrix, int size, ref List<double> result)
        {

            for (int cols = 0; cols < size; cols++)
            {
                SwapRows(ref matrix, size, FindMaxIndex(matrix, size, cols), cols);

                for (int rows = cols; rows < size; rows++)
                {
                    if (matrix[rows + 1, cols] == 0)
                    {
                        continue;
                    }
                    double mult = matrix[cols, cols] / matrix[rows + 1, cols];

                    for (int column = cols; column <= size + 1; column++)
                    {
                        matrix[rows + 1, column] = matrix[cols, column] - matrix[rows + 1, column] * mult;
                    }
                }
                DisplayMatrix(matrix, size);
                Console.WriteLine();
            }
            double determiner = 1;
            for (int i = 0; i <= size; i++)
            {
                determiner *= matrix[i, i];
            }
            result = calculateX(ref matrix, size);
        }

        static void GetMatr(double[,] mas, double[,] p, int i, int j, int m)
        {
            int ki, kj, di, dj;
            di = 0;
            for (ki = 0; ki < m - 1; ki++)
            {
                if (ki == i) di = 1;
                dj = 0;
                for (kj = 0; kj < m - 1; kj++)
                {
                    if (kj == j) dj = 1;
                    p[ki, kj] = mas[ki + di, kj + dj];
                }
            }
        }
        static double CalculateDeterminant(double[,] mas, int m)
        {
            int i, j, k, n;
            double d;
            double[,] p = new double[m, m];
            j = 0; d = 0;
            k = 1;
            n = m - 1;
            if (m < 1) Console.WriteLine("The determinant cannot be calculated!");
            if (m == 1)
            {
                d = mas[0, 0];
                return d;
            }
            if (m == 2)
            {
                d = mas[0, 0] * mas[1, 1] - (mas[1, 0] * mas[0, 1]);
                return d;
            }
            if (m > 2)
            {
                for (i = 0; i < m; i++)
                {
                    GetMatr(mas, p, i, 0, m);
                    d = d + k * mas[i, 0] * CalculateDeterminant(p, n);
                    k = -k;
                }
            }
            return d;
        }

        static void DisplayMatrix(double[,] matrix, int size)
        {
            for (int i = 0; i <= size; i++)
            {
                for (int j = 0; j <= size + 1; j++)
                {
                    Console.Write($"{matrix[i, j],3}  ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static int FindMaxIndex(double[,] matrix, int size, int col)
        {
            int maxIndex = col;

            for (int row = 1; row <= size; row++)                                // =?
            {
                if (Math.Abs(matrix[row, col]) > Math.Abs(matrix[maxIndex, col]))
                {
                    maxIndex = row;
                }
            }
            return maxIndex;
        }

        static void SwapRows(ref double[,] matrix, int size, int maxIndex, int row)
        {
            for (int cols = 0; cols <= size + 1; cols++)
            {
                double temp = matrix[row, cols];
                matrix[row, cols] = matrix[maxIndex, cols];
                matrix[maxIndex, cols] = temp;
            }
            DisplayMatrix(matrix, size);
        }

        static void MatrixMultiplication(ref double[,] matrix, int size, int col, double multiplier)
        {
            for (int rows = 0; rows < size; rows++)                                                              // size = col
            {
                matrix[rows, col] *= multiplier;
            }
        }

        static List<double> calculateX(ref double[,] matrix, int size)
        {
            DisplayMatrix(matrix, size);
            List<double> result = new List<double>();
            double x;
            x = matrix[size, size + 1] / matrix[size, size];
            result.Add(x);

            for (int rows = size - 1; rows >= 0; rows--)
            {
                MatrixMultiplication(ref matrix, size, rows + 1, x);
                double fraction = 0;
                for (int cols = rows + 1; cols <= size; cols++)
                {
                    fraction += matrix[rows, cols];
                }
                x = (matrix[rows, size + 1] - fraction) / matrix[rows, rows];
                result.Add(x);
            }
            result.Reverse();
            return result;
        }
    }
}

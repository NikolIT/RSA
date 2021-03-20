using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace RSA_ConsoleApp
{
    struct Key
    {
        public int e_d;
        public int n;
    }

    class Program
    {
        static List<char> ABC = new List<char>() { '_', 'А', 'Б', 'В', 'Г', 'Д', 'Е', 'Є', 'Ж', 'З', 'И', 'I', 'Ї', 'Й', 'К', 'Л', 'М', 'Н', 'О', 'П', 'Р', 'С', 'Т', 'У', 'Ф', 'Х', 'Ц', 'Ч', 'Ш', 'Щ', 'Ь', 'Ю', 'Я' };

        static string info = String.Empty;

        static void Main(string[] args)
        {
            

            Console.Write("Massege - ");
            string Massege = Console.ReadLine().ToUpper();

            info += $"Massege - {Massege} \r\n";

            List<int> msg = GetArrayIntWithString(Massege);

            Console.Write($"massege - ");

            foreach (var item in msg)
            {
                Console.Write($" {item}");
            }
            Console.WriteLine();

            List<char> result = new List<char>();

            Console.Write("p - ");
            int p = int.Parse(Console.ReadLine());

            info += $"p - {p}\r\n\r\n";

            Console.Write("q - ");
            int q = int.Parse(Console.ReadLine());

            info += $"q - {q}\r\n\r\n";

            int n = p * q;
            info += $"n - {n}\r\n\r\n";
            Console.WriteLine($"n - {n}");
            int f_n = (p - 1) * (q - 1);


            info += $"f(n) - {f_n}\r\n\r\n";

            int e = Get_e(f_n);

            info += $"e - {e}\r\n\r\n";

            int d = Gcdext(e, f_n);

            info += $"d - {d}\r\n";

            Key openKey = new Key() { e_d = e, n = n};

            info += $"openKey e - {openKey.e_d} n - {openKey.n}\r\n\r\n";

            Key secretKey = new Key() { e_d = d, n = n };

            info += $"secretKey d - {secretKey.e_d} n - {secretKey.n}\r\n\r\n";

            Console.WriteLine($"Open key e - {openKey.e_d} n - {openKey.n}");
            Console.WriteLine($"Secret key d - {secretKey.e_d} n - {secretKey.n}");
            

            info += $"                  encriptRSA\r\n\r\n";

            for (int i = 0; i < msg.Count; i++)
            {
                info += $"{msg[i]} -> ";
                msg[i] = encriptRSA(openKey, msg[i]);
                info += $"{msg[i]}\r\n";
            }

            

            Console.Write($"encriptRSA - ");
            foreach (var item in msg)
            {
                Console.Write(" " + item);
            }
            Console.WriteLine();

            info += $"                  decriptRSA\r\n\r\n";

            Console.Write($"decriptRSA - ");

            for (int i = 0; i < msg.Count; i++)
            {

                info += $"{msg[i]} -> ";
                msg[i] = decriptRSA(secretKey, msg[i]);
                info += $"{msg[i]}\r\n";
                info += $"message { ABC[msg[i]]}\r\n";
                Console.Write($" {msg[i]}");
                msg[i] = ABC[msg[i]];
            }

            foreach (var item in msg)
            {
                result.Add((char)item);
            }

            Console.WriteLine();

            Console.Write($"massege - ");
            foreach (var item in result)
            {
                Console.Write(" " + item);
            }
            Console.WriteLine();

            // создаем каталог для файла
            string path = @"C:\RSA";
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            // цифровий підпис
            Console.Write($"signature - ");
            int[] signature = msg.ToArray();
            info += $"signature -";
            for (int i = 0; i < signature.Length; i++)
            {
                signature[i] = encriptRSA(secretKey, signature[i]);
                Console.Write($" {signature[i]}");
                info += $" {signature[i]}";
            }
            Console.WriteLine();

            // сохраняем текст в файл
            using (FileStream fstream = new FileStream($@"{path}\RSA_InfoWorck.txt", FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(info);
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
                Console.WriteLine($"Файл з інформацією було створено за наступним шляхом - {path}");
            }






            Console.ReadLine();
        }

        private static List<int> GetArrayIntWithString(string massege)
        {
            List<int> arrayInt = new List<int>();

            foreach (var item in massege)
            {    
                arrayInt.Add(ABC.IndexOf(item));
            }
            
            for (int i = 0; i < arrayInt.Count; i++)
            {
                info += $"{massege[i]} - {arrayInt[i]}\r\n";
            }

            return arrayInt;
        }

        private static int decriptRSA(Key secretKey, int msg)
        {
            BigInteger bi;
            bi = new BigInteger(msg);
            bi = BigInteger.Pow(bi, (int)secretKey.e_d);

            BigInteger n_ = new BigInteger((int)secretKey.n);

            bi = bi % n_;

            int index = Convert.ToInt32(bi.ToString());

            return index;
        }

        private static int encriptRSA(Key openKey, long msg)
        {
            BigInteger bi;
            bi = new BigInteger(msg);
            bi = BigInteger.Pow(bi, (int)openKey.e_d);

            BigInteger n_ = new BigInteger((int)openKey.n);

            bi = bi % n_;

            int index = Convert.ToInt32(bi.ToString());

            return index;
        }

        static int Get_e(int f_n)
        {
            for (int i = 2; i < f_n; i++)
            {
                if (Gcd(i, f_n) == 1)
                {
                    return i;
                }
            }

            return int.MinValue;
        }

        static int Gcd(int a, int b)
        {
            if (b == 0) 
                return a;

            return Gcd(b, a % b);
        }

        static private int Gcdext(int d, int m)
        {
            int e = 10;

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }
    }
}

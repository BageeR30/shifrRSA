using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace shifrRSA
{
    class Program
    {
        //нахождение нода (a*x + n*y = НОД, x - обратное к a)
        static long NOD(long a, long n)
        {
            long x = 0, y = 0;
            if (n == 0)
            {
                x = 1;
                y = 0;
                return a;
            }
            else
            {
                long N1 = n;
                long x2 = 1,
                    x1 = 0,
                    y2 = 0,
                    y1 = 1,
                    q = 0,
                    r = 0;
                while (n > 0)
                {
                    q = a / n;
                    r = a - q * n;
                    x = x2 - q * x1;
                    y = y2 - q * y1;

                    a = n;
                    n = r;
                    x2 = x1;
                    x1 = x;
                    y2 = y1;
                    y1 = y;
                }
                x = x2;
                if (x < 0)
                {
                    x = N1 + x;
                }
                y = y2;
                return a;
            }
            //long Result = 1;
            //if (m == 0) { Result = n; }
            //if (n == 0) { Result = m; }
            //if (m == n) { Result = m; }
            //if ((m % 2 == 0) && (n % 2 == 0)) { Result = 2 * NOD(m / 2, n / 2); }
            //if ((m % 2 == 0) && (n % 2 == 1)) { Result = NOD(m / 2, n); }
            //if ((m % 2 == 1) && (n % 2 == 0)) { Result = NOD(m, n / 2); }
            //if ((m % 2 == 1) && (n % 2 == 1) && (n > m)) { Result = NOD(m, (n - m) / 2); }
            //if ((m % 2 == 1) && (n % 2 == 1) && (n < m)) { Result = NOD((m - n) / 2, n); }
            //return Result; 
        }
        //перемножение двух больших чисел по модулю n
        static ulong mult(ulong a, ulong b, ulong n)
        {
            if (b == 0) return 0;
            ulong res = mult(a, b / 2, n);
            res += res;
            res %= n;
            return (b % 2 == 1) ? (a + res) % n : res; 
        }
        //быстрое возведение в степень a^p (mod n)
        static ulong FastPow(ulong a, ulong p, ulong n)
        {
            if (p == 0) return 1;
            ulong res = FastPow(a, p / 2, n);
            res = mult(res, res, n);
            return (p % 2 == 1) ? mult(res, a, n) : res; 
        }
        //обратное по модулю n, выводит 0 если нет обратного
        static long Reverse(long a, long n)
        {
            long x = 0,
                y = 0;
            if (n == 0)
            {
                x = 1;
                return x;
            }
            else
            {
                long N1 = n;
                long x2 = 1,
                    x1 = 0,
                    y2 = 0,
                    y1 = 1,
                    q = 0,
                    r = 0;
                while (n > 0)
                {
                    q = a / n;
                    r = a - q * n;
                    x = x2 - q * x1;
                    y = y2 - q * y1;

                    a = n;
                    n = r;
                    x2 = x1;
                    x1 = x;
                    y2 = y1;
                    y1 = y;
                }
                x = x2;
                if (x < 0)
                {
                    x = N1 + x;
                }
                if (a != 1)
                {
                    x = 0;
                }
                return x;
            }
            //long mod1 = M;
            //long mod2 = N;

            //// Предыдущие к-ты 
            //long x1 = 0;
            //long y1 = 1;

            //// Новые 
            //long x2 = 1;
            //long y2 = -M / N;

            //long x3;
            //long y3;
            //bool failed = false;

            //while ((N != 0) && (M % N != 1))
            //{
            //    long T = M % N;
            //    M = N;
            //    N = T;

            //    if (N == 0)
            //    {
            //        // Обратного нет 
            //        failed = true;
            //    }
            //    else
            //    {

            //        // Новые к-ты 
            //        x3 = x1 + (-M / N) * x2;
            //        y3 = y1 + (-M / N) * y2;

            //        // Вносим новые к-ты 
            //        x1 = x2;
            //        y1 = y2;
            //        x2 = x3;
            //        y2 = y3;
            //    }
            //}
            //long MM = 0;
            //if (failed)
            //{
            //    MM = 0;
            //    //NM = 0;
            //}
            //else
            //{
            //    if (x2 < 0) { x2 += mod2; }
            //    if (y2 < 0) { y2 += mod1; }

            //    MM = y2;
            //    //NM = x2;
            //}
            //return MM;
        }
        //генерация простого числа больше var (N простых генерируем и выбираем одно случайное)
        static uint GenP(uint var, int N)
        {
            //int N = 10000;//(int)Math.Pow(2, 16);
            uint[] result = new uint[N];
            for (int i = 0; i < N; i++)
            {
                result[i] = 0;
            }

            long iter = 10;
            uint p = var;
            ulong a;
            bool q = true;
            for (int i = 0; i < N; p++)
            {
                q = true;
                a = 2;
                for (int j = 0; j < iter && q; j++)
                {
                    if (a % p != 0)
                    {
                        if (FastPow(a, p - 1, p) == 1)
                        {
                            q = true;
                        }
                        else
                        {
                            q = false;
                        }
                    }
                    a++;
                }
                if (q)
                {
                    result[i] = p;
                    i++;
                }
            }
            Random rnd = new Random();
            return result[rnd.Next(0, N)];
        }
        //шифрование RSA
        static void Encrypt(string filename, string dfilename, ulong e, ulong n, ulong[] mas, int l)
        {
            ulong a = 0;
            char ch;
            
            FileInfo f1 = new FileInfo(filename);
            FileInfo f2 = new FileInfo(dfilename);
            BinaryReader brtext = new BinaryReader(f1.Open(FileMode.Open, FileAccess.Read));
            //Console.WriteLine(brtext);
            BinaryWriter brdtext = new BinaryWriter(f2.Open(FileMode.Create, FileAccess.Write));
            for (int i = 0; i < f1.Length; i++)
            {
                ch = brtext.ReadChar();
                //Console.Write(ch);
                a = FastPow(ch, e, n);
                brdtext.Write(a);
            }
            Console.WriteLine();
            /*
            for (int i = 0; i < l; i++)
            {
                a = FastPow(mas[i], e, n);
                Console.Write(a);
                brdtext.Write(a);
            }
            Console.WriteLine();
            */
            brtext.Close();
            brdtext.Close();
        }
        //Дешифрование RSA
        static void Decrypt(string dfilename, string filenamenew, ulong d, ulong n, int l, ulong e, ulong n1)
        {
            ulong a = 0, m = 0;
                       
            FileInfo f1 = new FileInfo(dfilename);
            FileInfo f2 = new FileInfo(filenamenew);
            BinaryReader brtext = new BinaryReader(f1.Open(FileMode.Open, FileAccess.Read));
            BinaryWriter brdtext = new BinaryWriter(f2.Open(FileMode.Create, FileAccess.Write));
            

            string smessage = "";
            ulong[] mas = new ulong[l];

            //Дешифрование сообщения
            for (int i = 0; i < f1.Length - l*8; i+=8)
            {
                a = brtext.ReadUInt64();
                m = FastPow(a, d, n);
                smessage = smessage + Convert.ToChar(m);                
                brdtext.Write(Convert.ToChar(m));

            }

            Console.WriteLine(smessage);
            // Console.WriteLine("зашифрованный хэш");
            //Дещифрование хэша
            /*
            for (int i = 0; i < l; i++)
            {
                a = brtext.ReadUInt64();
                //Console.Write(a);
                mas[i] = FastPow(a, d, n);
                //Console.Write(mas[i]);
            }
            Console.WriteLine();

            string shash = "";
            ulong[] mash = new ulong[l];
            for (int i = 0; i < l; i++)
            {
                m = FastPow(mas[i], e, n1);
                mash[i] = m;
                Console.Write( i+" ::: " + mash[i]+" ! ");
                //Console.WriteLine();
                //brdtext.Write(m);
            }
            
            Console.WriteLine();
            


            byte[] data = Encoding.Default.GetBytes(smessage); //Вычисляем хэш сообщения, которое расшифровали
            byte[] bresulthash;
            SHA256 shaM = new SHA256Managed();
            bresulthash = shaM.ComputeHash(data);
            ulong[] mass = new ulong[bresulthash.Length];
            for (int i = 0; i<bresulthash.Length; i++)
            {
                
                mass[i] = bresulthash[i];
                Console.Write(i + " ::: " + mass[i] + " ! ");
                //Console.Write(mass[i]);
            }
            Console.WriteLine();

            bool b = true;
            for (int i=0;i<bresulthash.Length;i++)
            {
                if (mass[i] != mash[i])
                {
                    b = false;
                }
                   
            }

            if (b)  //Сравниваем расшифрованниый хэш с вычисленным хэшем
            {
                Console.WriteLine("Хэш сообщение совпал с хэшем в подписи. Сообщение не было изменено");
            }
            else
            {
                Console.WriteLine("Хэш сообщение не совпал с хэшем в подписи!!! Сообщение было изменено!!!");
            }
            */
            //Console.WriteLine(shash);
            //Console.WriteLine(resulthash);
            //Console.WriteLine(smessage);
            //Console.WriteLine(shash);
            Console.WriteLine();
            brtext.Close();
            brdtext.Close();
        }

        

        static void Main(string[] args)
        {
            string filename = "text.txt";
            string dfilename = "etext.txt";
            string filenamenew = "dtext.txt";
            
            //Генерируем открытый ключ
            ulong p = GenP(30000, 1000);
            ulong q = GenP(30000, 1000);
            ulong n = p * q;//(ulong)p * (ulong)q;
            ulong phi = (p - 1) * (q - 1);//((ulong)p - 1) * ((ulong)q - 1);
            ulong e = 0;
            Random rnd = new Random();
            do
            {
                e = (ulong)rnd.Next(6000, 65535);
            }
            while (NOD(Convert.ToInt64(e), Convert.ToInt64(phi)) != 1);
            //ulong e = GenP(50000, 1000);
            Console.WriteLine("Ключ A:");
            Console.WriteLine("p = " + p + "\nq = " + q + "\nn = " + n + "\nphi = " + phi + "\ne = " + e);
            Console.WriteLine();

            string mhash;
            using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.Default))
            {
                mhash = sr.ReadToEnd();
            }



            byte[] data = Encoding.Default.GetBytes(mhash);
            byte[] bresulthash;
            SHA256 shaM = new SHA256Managed();
            bresulthash = shaM.ComputeHash(data);
            
            string resulthash = Encoding.Default.GetString(bresulthash);
            //Console.Write("Хэш зашифрованного сообщения = ");

            for (int i=0; i<bresulthash.Length; i++)
            {
                //Console.Write(bresulthash[i]);
            }
            Console.WriteLine();

            // string str = " BageeR"; //подпись
            ulong[] mas = new ulong[bresulthash.Length];
            ulong d1 = (ulong)Reverse((long)e, (long)phi);
            //Console.WriteLine("d1 = " + d1);
            //Console.WriteLine("зашифрованый хэш");
            for (int i = 0; i < bresulthash.Length; i++)
            {
                mas[i] = FastPow(bresulthash[i], d1, n);
               // Console.Write(mas[i]);
            }

            Console.WriteLine();
            
            //Шифруем
            ulong p2 = GenP(30000, 1000);
            ulong q2 = GenP(30000, 1000);
            ulong n2 = p2 * q2;//(ulong)p2 * (ulong)q2;
            ulong phi2 = (p2 - 1) * (q2 - 1);//((ulong)p2 - 1) * ((ulong)q2 - 1);
            ulong e2 = 0;
            Random rnd2 = new Random();
            do
            {
                e2 = (ulong)rnd2.Next(60000, 65536);
            }
            while (NOD(Convert.ToInt64(e2), Convert.ToInt64(phi2)) != 1);
            //ulong e2 = GenP(50000, 1000);
            Console.WriteLine("Ключ B:");
            Console.WriteLine("p = " + p2 + "\nq = " + q2 + "\nn = " + n2 + "\nphi = " + phi2 + "\ne = " + e2);
            Console.WriteLine();
            Encrypt(filename, dfilename, e2, n2, mas, bresulthash.Length);
            Console.WriteLine("Сообщение зашифровано!");

            //Дешифруем
            ulong d = (ulong)Reverse((long)e2, (long)phi2);
            //Console.WriteLine("d2 = " + d);
            Decrypt(dfilename, filenamenew, d, n2, bresulthash.Length, e, n);
            Console.WriteLine("Сообщение расшифровано!");

            Console.ReadKey();
        }
    }
}

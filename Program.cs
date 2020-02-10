using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace tarsalgo
{
    class kibe
    {
        DateTime mikor;
        string merre;
        string ki;
        int mennyibent;

        public kibe(DateTime mikor, string merre, string ki, int mennyibent)
        {
            this.mikor = mikor;
            this.merre = merre;
            this.ki = ki;
            this.mennyibent = mennyibent;
        }

        public DateTime Mikor { get => mikor; set => mikor = value; }
        public string Merre { get => merre; set => merre = value; }
        public string Ki { get => ki; set => ki = value; }
        public int Mennyibent { get => mennyibent; set => mennyibent = value; }

    }


    class Program
    {
        static List<kibe> mozgas = new List<kibe>();
        static string fajl_ajto = @"f:\suli\szoftverfejleszto\c_sharp\tarsalgo\f04_tarsalgo\ajto.txt";        
        static string fajl_athaladas = @"f:\suli\szoftverfejleszto\c_sharp\tarsalgo\f04_tarsalgo\athaladas.txt";

        static void beolvas()
        {
            using (StreamReader sr = new StreamReader(fajl_ajto))
            {
                while (!sr.EndOfStream)
                {
                    string[] egy_sor = sr.ReadLine().Split(' ');
                    DateTime ido_var = new DateTime(2000, 1, 1, int.Parse(egy_sor[0]), int.Parse(egy_sor[1]), 0);
                    mozgas.Add(new kibe(ido_var, egy_sor[3], egy_sor[2], 0));
                }
                sr.Close();
            }
        }

        static void setMennyibent()
        {
            if (mozgas[0].Merre == "be")
                mozgas[0].Mennyibent = 1;

            for (int i = 1; i < mozgas.Count; i++)
            {
                if (mozgas[i].Merre == "be")
                    mozgas[i].Mennyibent = mozgas[i - 1].Mennyibent + 1;
                else
                    mozgas[i].Mennyibent = mozgas[i - 1].Mennyibent - 1;

            }
        }

        static void elsoutolso()
        {

            Console.WriteLine($"Az első belépő: {mozgas[0].Ki}");

        }
        static int hanyanvannak_fn()
        {
            int hanyanvannak_var = 0;
            for (int i = 0; i < mozgas.Count(); i++)
            {
                if (Convert.ToInt32(mozgas[i].Ki) > hanyanvannak_var)
                {
                    hanyanvannak_var = Convert.ToInt32(mozgas[i].Ki);
                }
            }
            return hanyanvannak_var;
        }
        static void kivanbentharomkor()
        {
            int hanyanvannak = hanyanvannak_fn();

            int[] hanyszor = new int[hanyanvannak+1];

            for (int i = 0; i < mozgas.Count(); i++)
            {
                hanyszor[Convert.ToInt32(mozgas[i].Ki)]++;
            }
            Console.WriteLine("\n3. feladat");
            using (StreamWriter sw = new StreamWriter(fajl_athaladas))
            {
                for (int i = 1; i < hanyszor.Length; i++)
                {
                    sw.WriteLine($"{i} {hanyszor[i]}");
                }
                sw.Close();
            }

            Console.WriteLine("athaladas.txt létrehozva!");

            Console.WriteLine("\n4. feladat");
            Console.Write("Háromkor a teremben vannak: ");
            for (int i = 0; i < hanyszor.Count(); i++)
            {
                if (hanyszor[i] % 2 != 0)
                    Console.Write($" {i}");

            }
        }
        static int legtöbben_index()
        {
            int legtobben = mozgas.Max(x => x.Mennyibent);
            int index = -1;
            do
            {
                index++;
            }
            while (mozgas[index].Mennyibent != legtobben);

            return index;
        }

        static void szemely_volt_bent(int szemely)
        {
            DateTime bement = DateTime.Now;
            int bentvolt = 0;
            bool vegenbentvolt = false;
            foreach (var item in mozgas.Where(x => (Convert.ToInt32(x.Ki) == szemely)))
            {
                Console.Write($"{item.Mikor.ToString("HH:mm")}");

                if (item.Merre == "be")
                {
                    Console.Write(" - ");
                    bement = item.Mikor;
                    vegenbentvolt = true;
                }
                else
                {
                    Console.Write("\n");
                    bentvolt = (item.Mikor.Minute > bement.Minute) ? bentvolt + (item.Mikor.Minute - bement.Minute) : ((item.Mikor.Minute + 60) - bement.Minute);
                    vegenbentvolt = false;
                }
            }

            if (vegenbentvolt)
            {
                DateTime vege = mozgas.Last().Mikor;
                DateTime utoljarabe = (mozgas.Last(x => (Convert.ToInt32(x.Ki) == szemely)).Mikor);
                bentvolt = (vege.Minute) > (utoljarabe.Minute) ? bentvolt + (vege.Minute - utoljarabe.Minute) : ((vege.Minute+ 60) - bement.Minute);
                
                Console.Write($"\nA(z) {szemely}. személy összesen {bentvolt} percet volt bent, a megfigyelés végén a társalgóban volt. ");
            }
            else
            {
                Console.Write($"\nA(z) {szemely}. személy összesen {bentvolt} percet volt bent, a megfigyelés végén nem volt a társalgóban. ");

            }
        }

        static void Main(string[] args)
        {


            Console.WriteLine("1. feladat");
            beolvas();
            Console.WriteLine("Fájl beolvasva!");
            setMennyibent();


            Console.WriteLine("\n2. feladat");
            Console.WriteLine($"Az első belépő: {mozgas.First(x => x.Merre == "be").Ki}");
            Console.WriteLine($"Az utolsó kilépő: {mozgas.Last(x => x.Merre == "ki").Ki}");

            kivanbentharomkor();

            Console.WriteLine("\n\n5. feladat");
            int max_index = legtöbben_index();

            Console.WriteLine($"Például {mozgas[max_index].Mikor : H:mm}-kor voltak a legtöbben a társalgóban.");

            Console.WriteLine("\n6-7-8. feladat");
            Console.Write("Adja meg a személy azonosítóját!");
            szemely_volt_bent(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("\n\nProgram vége!");
            Console.ReadKey();
        }
    }
}


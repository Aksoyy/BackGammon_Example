using System;
using System.Collections.Generic;
using System.Linq;

namespace backgammonProject
{
    class Program
    /* Fonksiyona verilecek parametrelerin setlenmesi */
        static void Main(string[] args)
        {
            Dictionary<int, int> keyValues = new Dictionary<int, int>();
            for (int i = 0; i < 24; i++)
            {
                keyValues[i] = 0;
            }

            keyValues[0] = 3;
            keyValues[5] = 1;
            keyValues[9] = 2;
            keyValues[11] = 1;
            keyValues[12] = 1;

            Random rnd = new Random();
            int zar1 = 6; // rnd.Next(1, 6);
            int zar2 = 1; // rnd.Next(1, 6);

            find_Moves(keyValues, zar1, zar2);
        }

        private static void find_Moves(Dictionary<int, int> keyValues, int zar1, int zar2)
        {
            List<int> zarListesi = new List<int>() { zar1, zar2 };
            List<Tuple<Tuple<int, int>, Tuple<int, int>, int>> puanListesi = new List<Tuple<Tuple<int, int>, Tuple<int, int>, int>>();

            /* Hamlelerin hesaplanması */
            foreach (var zar in zarListesi)
            {
                for (int i = 0; i < 24; i++)
                {
                    Dictionary<int, int> hesapListesi = NonReferenceCopy(keyValues);
                    if (hesapListesi[i] > 0)
                    {
                        Tuple<int, int> ilktas = TasKonumHesapla(zar, hesapListesi, i);

                        List<int> tempZarList = zarListesi.Where(k => !k.Equals(zar)).ToList();
                        foreach (var tempZar in tempZarList)
                        {
                            for (int j = 0; j <= hesapListesi.Count - 1; j++)
                            {
                                Dictionary<int, int> tempHesapListesi = NonReferenceCopy(hesapListesi);
                                if (tempHesapListesi[j] > 0)
                                {
                                    Tuple<int, int> sontas = TasKonumHesapla(tempZar, tempHesapListesi, j);

                                    puanListesi.Add(Tuple.Create(
                                    Tuple.Create(ilktas.Item1 + 1, ilktas.Item2 + 1),
                                    Tuple.Create(sontas.Item1 + 1, sontas.Item2 + 1),
                                    hesapla(ilktas, sontas, tempHesapListesi)));
                                }
                            }
                        }
                    }
                }
            }

            puanListesi.ForEach(k => { if (k.Item3 > 0) Console.WriteLine(k); } );
            Console.ReadLine();
        }

        /* Hesaplanacak hamlelerin kopyasını oluştur başlangıç konumlarını kaybetmemek için */
        private static Dictionary<int, int> NonReferenceCopy(Dictionary<int, int> hesapListesi)
        {
            Dictionary<int, int> copy = new Dictionary<int, int>();
            foreach (var item in hesapListesi.Keys)
            {
                copy[item] = hesapListesi[item];
            }
            return copy;
        }

        private static Tuple<int, int> TasKonumHesapla(int zar, Dictionary<int, int> hesapListesi, int key)
        {
            int konum = key;
            int ilkkonum = key;
            hesapListesi[konum] = hesapListesi[konum] - 1;
            konum = konum + zar >= 24 ? (konum + zar) - 24 : konum + zar;
            hesapListesi[konum] = hesapListesi[konum] + 1;
            return Tuple.Create(ilkkonum, konum);
        }

        /* Belirlenen hamlelerin puan hesabı yapılmaktadır. */
        private static int hesapla(Tuple<int,int> ilktas, Tuple<int,int> sontas, Dictionary<int, int> hesapListesi)
        {
            int puan = 0;
            List<int> onemlitaslarListesi = new List<int>() { 4, 5, 6, 7, 16, 17, 18, 19 };

            switch (hesapListesi[sontas.Item1]) //son1KonumTasSayisi
            {
                case 0:
                    puan = puan + 1;
                    break;
                case 1:
                    if (ilktas.Item2 != sontas.Item1)
                        puan = puan + (onemlitaslarListesi.Contains(ilktas.Item2) ? -2 : -1) ;

                    break;

            }
            switch (hesapListesi[sontas.Item2]) //son2KonumTasSayisi
            {
                case 1:
                        puan = puan - 1;
                    break;
                case 2:
                    puan = puan + (onemlitaslarListesi.Contains(sontas.Item2) ? 2 : 1);//+ (hesapListesi[sontas.Item2] == 2 ? 1 : 0);

                    if (ilktas.Item2 != sontas.Item2)
                        puan=puan+1;

                        break;
            }

            switch (hesapListesi[ilktas.Item2])
            {
                case 1:
                    puan = puan - 1;
                    break;
                case 2:
                    if (ilktas.Item2 != sontas.Item2)
                        puan = puan + (onemlitaslarListesi.Contains(ilktas.Item2) ? 2 : 1); //+ (hesapListesi[ilktas.Item2] == 2 ? 1 : 0);
                    break;

            }
            switch (hesapListesi[ilktas.Item1])
            {
                case 0:
                    puan = puan + 1;
                    break;
                case 1:
                    if (ilktas.Item1 != sontas.Item1)
                        puan = puan + (onemlitaslarListesi.Contains(ilktas.Item2) ? -2 : -1) ;
                    break;

            }
            return puan;
        }
    }
}

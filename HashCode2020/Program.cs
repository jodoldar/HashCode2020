using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2020
{
    class Program
    {
        static int[] aiDays;
        static Library[] aLibs;
        static Book[] aBooks;

        static List<Book> lBooks;
        static List<Library> lLibraries;
        static List<Library> lSortedLibraries;
        static int iNumOfDays;
        static int iNumOfLibs;

        static int Main(string[] args)
        {
            
            List<int> aiLibOrder = new List<int>();
            int iProcessedLibs = 0, iLibToPick = 0;
            bool evenTime = false, signingUp = false;

            if (args.Length < 1)
            {
                Console.WriteLine("Error. Not enough arguments.");
                return -1;
            }

            lBooks = new List<Book>();
            lLibraries = new List<Library>();

            ParseFile(args[0]);

            lSortedLibraries = lLibraries.OrderBy(x => x.SigupTime + 0.1*(x.Books.Count/x.Performance)).ToList();
            Console.WriteLine("{0} > {1}", lSortedLibraries.First().SigupTime, lSortedLibraries.Last().SigupTime);

            if (iNumOfLibs%2 == 0) { evenTime = true; }

            for (int iDays = 0; iDays <= iNumOfDays; iDays++)
            {
                Console.WriteLine("Day {0} of {1}", iDays, iNumOfDays);

                // Choose library to sign up
                if (!signingUp)
                {
                    /*if (iProcessedLibs % 2 == 0)
                    {
                        iLibToPick = iProcessedLibs / 2;
                    }
                    else
                    {
                        iLibToPick = iNumOfLibs - (iProcessedLibs / 2) - 1;
                    }*/
                    iLibToPick = iProcessedLibs;
                    Console.WriteLine("Day {0}. Processed libs->{1}, iLibToPick->{2}", iDays, iProcessedLibs, iLibToPick);
                }


                if (iProcessedLibs < iNumOfLibs && !lSortedLibraries[iLibToPick].Processed && !lSortedLibraries[iLibToPick].Signuping && !lSortedLibraries[iLibToPick].Processing)
                {
                    lSortedLibraries[iLibToPick].Signuping = true;
                    signingUp = true;

                    Console.WriteLine("Day {0}. Starting signup of library {1}", iDays, lSortedLibraries[iLibToPick].Id);
                }
                else if (iProcessedLibs < iNumOfLibs &&  lSortedLibraries[iLibToPick].Processed)
                {
                    iProcessedLibs++;
                }

                // Scanning Pool
                foreach (Library lLib in lSortedLibraries)
                {
                    if (lLib.Processing)
                    {
                        //Console.WriteLine("Day {0}. Library {1} in process. ({2})", iDays, lLib.Id, lLib.ProcBooks.Count);
                        // Performance = 1
                        //lLib.ProcBooks.Add(lLib.Books[lLib.ProcBooks.Count]);

                        for (int k = 0; (k < lLib.Performance) && (lLib.ProcBooks.Count < lLib.Books.Count); k++)
                        {
                            lLib.ProcBooks.Add(lLib.Books[lLib.ProcBooks.Count]);
                        }

                        if (lLib.ProcBooks.Count == lLib.Books.Count)
                        {
                            lLib.Processing = false;
                            lLib.Processed = true;

                            Console.WriteLine("Day {0}. Library {1} processed.", iDays, lLib.Id);
                        }
                    }
                }

                // Update sign up status
                if (signingUp)
                {
                    lSortedLibraries[iLibToPick].RemDays--;

                    if (lSortedLibraries[iLibToPick].RemDays == 0)
                    {
                        lSortedLibraries[iLibToPick].Signuping = false;
                        signingUp = false;
                        iProcessedLibs++;

                        lSortedLibraries[iLibToPick].Processing = true;
                        aiLibOrder.Add(lSortedLibraries[iLibToPick].Id);

                        Console.WriteLine("Day {0}. Library {1} signed up. Processing", iDays, lSortedLibraries[iLibToPick].Id);
                    }
                }


            }

            ParseOut(args[0], iProcessedLibs, aiLibOrder);

            return 0;
        }

        public static void ParseFile(string sFilePath)
        {
            Console.WriteLine("Starting the parse of {0}", sFilePath);

            using (var fInFile = new StreamReader(sFilePath))
            {
                string sLine;
                string[] asLineIn;

                int iNumOfBooks;
                

                sLine = fInFile.ReadLine();

                asLineIn = sLine.Split(' ');
                aiDays = new int[int.Parse(asLineIn[2])];
                aLibs = new Library[int.Parse(asLineIn[1])];
                aBooks = new Book[int.Parse(asLineIn[0])];

                iNumOfLibs = int.Parse(asLineIn[1]);
                iNumOfDays = int.Parse(asLineIn[2]);

                Console.WriteLine("We have {0} books, {1} libraries and {2} days.", aBooks.Length, aLibs.Length, aiDays.Length);

                sLine = fInFile.ReadLine();
                asLineIn = sLine.Split(' ');
                for (int i = 0; i < asLineIn.Length; i++)
                {
                    lBooks.Add(new Book(int.Parse(asLineIn[i])));                    
                }

                for (int i = 0; i < iNumOfLibs; i++)
                {
                    asLineIn = fInFile.ReadLine().Split(' ');

                    iNumOfBooks = int.Parse(asLineIn[0]);
                    Library lTemp = new Library(iNumOfBooks, int.Parse(asLineIn[1]), int.Parse(asLineIn[2]), i);

                    asLineIn = fInFile.ReadLine().Split(' ');

                    for (int j = 0; j < iNumOfBooks; j++)
                    {
                        lTemp.Books.Add(int.Parse(asLineIn[j]));
                    }

                    lLibraries.Add(lTemp);
                }
            }
        }

        public static void ParseOut(string sFilePath, int iProcessedLibs, List<int> aiLibOrder)
        {
            string sOutLine = "";
            int iOutLibs = 0;

            Console.WriteLine("Finished with {0} libs available.", aiLibOrder.Count);

            using (var fOutFile = new StreamWriter(sFilePath + ".out"))
            {
                foreach (int iLibId in aiLibOrder)
                {
                    Library lTempLib = lSortedLibraries.Find(x => x.Id == iLibId);
                    Console.WriteLine("Lib {0}. State: {1}", lTempLib.Id, lTempLib.ProcBooks.Count);
                    if (lTempLib.ProcBooks.Count > 0)
                    {
                        iOutLibs++;
                    }
                }

                fOutFile.WriteLine(iOutLibs);

                foreach (int iLibId in aiLibOrder)
                {
                    Library lTempLib = lSortedLibraries.Find(x => x.Id == iLibId);

                    if (lTempLib.ProcBooks.Count > 0)
                    {
                        fOutFile.WriteLine("{0} {1}", iLibId, lTempLib.ProcBooks.Count);

                        sOutLine = "";
                        foreach (int iBookId in lTempLib.ProcBooks)
                        {
                            sOutLine += iBookId;
                            sOutLine += " ";
                        }
                        fOutFile.WriteLine(sOutLine.TrimEnd());
                    }
                }
            }
        }
    }
}

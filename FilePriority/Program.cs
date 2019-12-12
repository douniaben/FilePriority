using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FilePriority
{
    class Program
    {

        static Barrier barrier = new Barrier(1, (b) =>
        {

          Console.WriteLine("ALL PRIORITY FILES HAVE BEEN SAVED. IT IS THE TURN OF NON-PRIORITY FILES TO BE.");

        });

        static void Main(string[] args)
        {
            // Press any key to launch the program
            Console.ReadKey();


            // Don't forget to let the user choose the file extension and size


            // Run the copy of big and small priority files
            Thread t1 = new Thread(CopyBig);
            Thread t2 = new Thread(CopySmall);
            t1.Start();
            t2.Start();

            // Wait for them to finish
            t1.Join();
            t2.Join();

            // Signal that the priority copy is done so as to run the next one
            barrier.SignalAndWait();

            // Run the copy of big and small non-priority files
            Thread t3 = new Thread(CopyBigNoExt);
            Thread t4 = new Thread(CopySmallNoExt);
            t3.Start();
            t4.Start();

            // Wait for them to finish
            t3.Join();
            t4.Join();

            // Keep the console window open.
            Console.ReadLine();

        }
        
        // Parallel copy for small priority files
        static void CopySmall()
        {
                DirectoryInfo source = new DirectoryInfo(@"C:\Users\DELL\Desktop\eBooks");

                // Loooking for a specific file extension, i.e : MP4 file format
                FileInfo[] allpdfiles = source.GetFiles("*.mp4", SearchOption.AllDirectories);

                Parallel.ForEach(allpdfiles, (f) =>
                {
                    // Specify file size limit
                    if (f.Length < 300000000)
                    {
                        DirectoryInfo target = new DirectoryInfo(@"C:\Users\DELL\Desktop\backup2");

                        Console.WriteLine("Copying : {0} ", f);
                        f.CopyTo(Path.Combine(target.FullName, f.Name), true);
                        Console.WriteLine("Done copying : {0} ", f);

                        Console.WriteLine();

                    } 
                });

                Console.WriteLine("backup of SMALL MP4 files successfully done ");
        }

        // Sequential copy for big priority files
        static void CopyBig()
        {
                DirectoryInfo source = new DirectoryInfo(@"C:\Users\DELL\Desktop\eBooks");

                // Looking for a specific file extension, i.e : MP4 file format
                FileInfo[] allpdfiles = source.GetFiles("*.mp4", SearchOption.AllDirectories);

                foreach (FileInfo f in allpdfiles)
                {
                // Specify file size limit
                if (f.Length > 300000000)
                    {
                        DirectoryInfo target = new DirectoryInfo(@"C:\Users\DELL\Desktop\backup2");

                        Console.WriteLine("Copying : {0} ", f);
                        f.CopyTo(Path.Combine(target.FullName, f.Name), true);
                        Console.WriteLine("Done copying : {0} ", f);

                        Console.WriteLine();

                    }
                }

                Console.WriteLine("backup of BIG MP4 files successfully done ");

            }

        // Sequential copy for big non-priority files
        static void CopyBigNoExt()
        {
            DirectoryInfo source = new DirectoryInfo(@"C:\Users\DELL\Desktop\eBooks");

            // Looking for all file extensions then filtering them in foreach loop
            FileInfo[] allfiles = source.GetFiles("*", SearchOption.AllDirectories);

            foreach (FileInfo f in allfiles)
            {
                // Specify file size limit
                if (f.Length > 2000000)
                {
                    // Exclude a file extension, i.e : MP4 file format
                    if (!f.FullName.EndsWith(".mp4"))
                    {
                        DirectoryInfo target = new DirectoryInfo(@"C:\Users\DELL\Desktop\backup2");

                        Console.WriteLine("Copying : {0} ", f);
                        f.CopyTo(Path.Combine(target.FullName, f.Name), true);
                        Console.WriteLine("Done copying : {0} ", f);

                        Console.WriteLine();
                    }
                }
            }

            Console.WriteLine("backup of BIG NO-MP4 files successfully done");
        }

        // Parallel copy for small non-priority files
        static void CopySmallNoExt()
        {
            DirectoryInfo source = new DirectoryInfo(@"C:\Users\DELL\Desktop\eBooks");

            // Looking for all file extensions then filtering them in foreach loop
            FileInfo[] allfiles = source.GetFiles("*", SearchOption.AllDirectories);

            Parallel.ForEach(allfiles, (f) =>
            {
                // Specify file size limit
                if (f.Length < 2000000)
                {
                    // Exclude a file extension, i.e : MP4 file format
                    if (!f.FullName.EndsWith(".mp4"))
                    {
                        DirectoryInfo target = new DirectoryInfo(@"C:\Users\DELL\Desktop\backup2");

                        Console.WriteLine("Copying : {0} ", f);
                        f.CopyTo(Path.Combine(target.FullName, f.Name), true);
                        Console.WriteLine("Done copying : {0} ", f);

                        Console.WriteLine();
                    }
                }

            });

            Console.WriteLine("backup of SMALL NO-MP4 files successfully done");
        }
    }
}
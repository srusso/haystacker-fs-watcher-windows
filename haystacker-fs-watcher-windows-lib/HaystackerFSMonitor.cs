using System;
using System.IO;

namespace haystacker_fs_watcher_windows_lib
{
    public struct FSChangeNotification
    {
        public FSChangeNotification(int a, string b)
        {
            this.a = a;
            this.b = b;
        }

        int a;
        string b;
    }

    sealed public class HaystackerFSMonitor
    {
        [DllExport("testString")]
        public static String TestString()
        {
            return "This comes from C#";
        }

        [DllExport("pollChangeNotification")]
        public static FSChangeNotification PollChangeNotification()
        {
            return new FSChangeNotification(43, "aaaa");
        }

        private static void Run()
        {
            string[] args = Environment.GetCommandLineArgs();

            // If a directory is not specified, exit program.
            if (args.Length != 2)
            {
                // Display the proper way to call the program.
                Console.WriteLine("Usage: dotnet run (directory)");
                return;
            }

            // Create a new FileSystemWatcher and set its properties.
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = args[1];

                watcher.NotifyFilter = NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                // Only watch text files.
                watcher.Filter = "";

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                watcher.IncludeSubdirectories = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press 'q' to quit the sample.");
                while (Console.Read() != 'q') ;
            }
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e) =>
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");

        private static void OnRenamed(object source, RenamedEventArgs e) =>
            // Specify what is done when a file is renamed.
            Console.WriteLine($"File: {e.OldFullPath} renamed to {e.FullPath}");
    }
}
using System;
using System.IO;

namespace Utility
{
    public class DataMigrator
    {
        private static void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            foreach (FileInfo file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(destination.FullName, file.Name), true);
            }

            foreach (DirectoryInfo directory in source.GetDirectories())
            {
                DirectoryInfo nextDir = destination.CreateSubdirectory(directory.Name);
                CopyDirectory(directory, nextDir);
            }
        }

        public static void MigrateLinuxSaves()
        {
            string oldPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Aottg2";
            string newPath = FolderPaths.Documents;

            if (Directory.Exists(oldPath))
            {
                // Only attempt to copy if the old path exists and the new does not
                // Avoids overwriting new data
                FileAttributes sourceAttribs = File.GetAttributes(oldPath);
                if (sourceAttribs == FileAttributes.Directory && !Directory.Exists(newPath))
                {
                    CopyDirectory(new DirectoryInfo(oldPath), new DirectoryInfo(newPath));
                }
            }
        }
    }
}
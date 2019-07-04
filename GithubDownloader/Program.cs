using IoTSharp.Releases;
using System;
using System.IO;

namespace GithubDownloader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var githubDownloader = new ReleaseDownloader(args[0], args[1]);

            var releases = githubDownloader.GetDataForAllReleases();

            foreach (var release in releases)
            {
                var releaseName = $"{release.tag_name}";

                var releasePath = $"releases\\{releaseName}";

                Console.WriteLine("Release: {0}", release.tag_name);

                CheckAndCreateFolder(releasePath);

                SaveReleaseComments(release.body, releasePath);

                foreach (var asset in release.assets)
                {
                    var assetPath = releasePath + "\\" + asset.name;

                    Console.WriteLine("\tAsset: {0} - {1}", asset.id, assetPath);
                    var assetDl = githubDownloader.DownloadAsset(asset.id, assetPath);
                }
            }
        }

        private static void SaveReleaseComments(string comments, string folderName)
        {
            File.WriteAllText(folderName + "\\" + "comments.txt", comments);
        }

        private static void CheckAndCreateFolder(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
        }
    }
}
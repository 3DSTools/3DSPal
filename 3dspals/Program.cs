using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Web;
using System.Diagnostics.Eventing.Reader;

namespace _3dspals
{
    internal class Program
    {
        public static string sd_card = Directory.GetCurrentDirectory();
        public static void Get()
        {
            var run = Console.ReadLine();
            Command(run);
        }
        public static void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            Console.WriteLine("{0}    downloaded {1} of {2} bytes. {3} % complete...",
                (string)e.UserState,
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);
        }
        public static void DownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Download Failed");
                Console.ResetColor();
            } else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Download Complete");
                Console.ResetColor();
            }
        }

        public static void Command(string command)
        {
            if (command.Contains("set SDCardFolder = \""))
            {
                var cut = command.Replace("set SDCardFolder = \"", "");
                var location = cut.Replace("\"", "");
                if(Directory.Exists(location))
                {
                    Console.WriteLine("");
                    Console.WriteLine($"SD Card Folder has been set to {location}");
                    sd_card = location;
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine($"{location} doesn't exist on your harddrive.");
                    Console.WriteLine("");
                    Console.WriteLine("Would you like to make it exist on your harddrive? (y/n)");
                    Console.WriteLine("");
                    var yn = Console.ReadLine();
                    if (yn == "y")
                    {
                        Directory.CreateDirectory(location);
                    }
                    else
                    {
                        Console.WriteLine("Operation Cancelled.");
                    }
                }
            }
            if (command.Contains("install "))
            {
                if (command.Contains("--cia=")){
                    var CiaToInstall = command.Replace("install ", "");
                    var realCia = CiaToInstall.Replace("--cia=\"", "");
                    var actualCia = realCia.Replace("\"", "");
                    if (sd_card == Directory.GetCurrentDirectory())
                    {
                        Console.WriteLine("Operation Failed! (change sdcard folder! example: set SDCardFolder = \"sdcardlocation\") or you didnt run this with --ciaName\"j\"");
                    }
                    else
                    {
                        if (Directory.Exists(Path.Combine(sd_card, "cias")))
                        {
                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFileCompleted += DownloadComplete;
                                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                                Console.WriteLine($"Getting Source: {actualCia}");
                                string file = Path.GetFileName(new Uri(actualCia).AbsolutePath);
                                string filename = Uri.UnescapeDataString(file);
                                Console.WriteLine($"Downloading: {filename}");
                                client.DownloadFileAsync(new Uri(actualCia), Path.Combine(sd_card, "cias", filename));
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No cias folder found.");
                            Console.ResetColor();
                            Directory.CreateDirectory(Path.Combine(sd_card, "cias"));
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("cias folder created.");
                            Console.ResetColor();
                            using (WebClient client = new WebClient())
                            {
                                client.DownloadFileCompleted += DownloadComplete;
                                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                                Console.WriteLine($"Getting Source: {actualCia}");
                                string file = Path.GetFileName(new Uri(actualCia).AbsolutePath);
                                string filename = Uri.UnescapeDataString(file);
                                Console.WriteLine($"Downloading: {filename}");
                                client.DownloadFileAsync(new Uri(actualCia), Path.Combine(sd_card, "cias", filename));
                            }
                        }
                    }
                }
                if (command.Contains("install --boot9strap --step1"))
                {
                    var bs9Firmware = new Uri("https://3dspal-cdn--c3nc.repl.co/Step1Boot.firm");
                    if (!Directory.Exists(Path.Combine(sd_card, "boot9strap")))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Created boot9strap folder!");
                        Directory.CreateDirectory(Path.Combine(sd_card, "boot9strap"));
                        Console.ResetColor();
                    }
                    var completed = 0;
                    void Done(object sender, AsyncCompletedEventArgs e)
                    {
                        completed++;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Download Finished");
                        Console.ResetColor();
                        var lumaStartup = new Uri("https://3dspal-cdn--c3nc.repl.co/boot.3dsx");
                        var bs9firm = new Uri("https://3dspal-cdn--c3nc.repl.co/boot9strap.firm");
                        var bs9sha = new Uri("https://3dspal-cdn--c3nc.repl.co/boot9strap.firm.sha");
                        using (WebClient client = new WebClient())
                        {
                            if (completed == 1)
                            {
                                client.DownloadFileCompleted += Done;
                                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                                client.DownloadFileAsync(lumaStartup, Path.Combine(sd_card, "boot.3dsx"));
                            }
                            else
                            {
                                if (completed == 2)
                                {
                                    client.DownloadFileCompleted += Done;
                                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                                    client.DownloadFileAsync(bs9firm, Path.Combine(sd_card, "boot9strap", "boot9strap.firm"));
                                }
                                else
                                {
                                    if (completed == 3)
                                    {
                                        client.DownloadFileCompleted += Done;
                                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                                        client.DownloadFileAsync(bs9sha, Path.Combine(sd_card, "boot9strap", "boot9strap.firm.sha"));
                                    }
                                    else
                                    {
                                        if (completed == 4)
                                        {
                                            AlmostComplete();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    void AlmostComplete()
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Visit here for the in-depth tutorial: https://3ds.hacks.guide/installing-boot9strap-(ntrboot).html then return and input install --bootstrap9 --step2");
                        Console.ResetColor();
                    }
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
                        client.DownloadFileCompleted += Done;
                        client.DownloadFileAsync(bs9Firmware, Path.Combine(sd_card, "boot.firm"));
                    }
                }
                if (command.Contains("--boot9strap --step2"))
                {
                    Console.WriteLine("Clean up. (Reset firmware to Luma3DS.)");
                    File.Delete(Path.Combine(sd_card, "boot.firm"));
                    new WebClient().DownloadFileAsync(new Uri("https://3dspal-cdn--c3nc.repl.co/Step2Boot.firm"), Path.Combine(sd_card, "boot.firm"));
                    Console.WriteLine("Now you have to do Part 5 according to the guide by yourself because 3DSTools isn't that complex yet.");
                }
                // Website downloading (http://streetpass.ct8.pl/port/cias/)
                Uri websiteDir = new Uri("http://streetpass.ct8.pl/port/cias/");
                using (WebClient client = new WebClient())
                {
                    string allfiles = client.DownloadString(websiteDir);
                    string truedir = command.Replace("install --", "");
                    if (allfiles.Contains(truedir))
                    {
                        Uri downloadUrl = new Uri("http://streetpass.ct8.pl/port/cias/" + truedir);
                        Uri trueDownload = new Uri(client.DownloadString(downloadUrl));
                        if(!Directory.Exists(Path.Combine(sd_card, "cias")))
                        {
                            Directory.CreateDirectory(Path.Combine(sd_card, "cias"));
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Created cias folder.");
                            Console.ResetColor();
                        }
                        void Complete(object sender, AsyncCompletedEventArgs e)
                        {
                            if (e.Error == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Successfully downloaded " + truedir + "!");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Failed download (servers might be offline)");
                                Console.ResetColor();
                            }
                        }
                        Console.WriteLine(trueDownload.ToString());
                        client.DownloadFileCompleted += Complete;
                        client.DownloadFileAsync(trueDownload, Path.Combine(sd_card, "cias", truedir + ".cia"));
                    } else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No cia found on StreetPass CIA Network [for downloading cia's off internet, not something like boot9strap]");
                        Console.ResetColor();
                    }
                }
            }
            if(command.Contains("wafkee"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("wtf is the android guy doing here");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("android lolpo[");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("j");
                Console.ResetColor();
            }
            // removed backup and restore cuzz uh its very bad because c# sux for restores and backups
            Get();
        }
        public static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("██████  ██████  ███████ ██████   █████  ██      ");
            Console.WriteLine("     ██ ██   ██ ██      ██   ██ ██   ██ ██      ");
            Console.WriteLine(" █████  ██   ██ ███████ ██████  ███████ ██  ");
            Console.WriteLine("     ██ ██   ██      ██ ██      ██   ██ ██     ");
            Console.WriteLine("██████  ██████  ███████ ██      ██   ██ ███████ ");
            Console.ResetColor();
            Console.WriteLine("");
            Console.WriteLine("Written and Created by ndsboy87 of 3DS Tools.");
            Console.WriteLine("https://github.com/3DSTools/3DSPal");
            Get();
        }
    }
}

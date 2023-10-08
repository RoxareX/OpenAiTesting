using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using OpenAI_API.Moderation;

class Program
{
    static async Task Main(string[] args)
    {
        string strAPI = "";

        try
        {
            var API = File.ReadAllText("API.txt");
            strAPI = API.ToString();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("\n" + ex.Message);
            Console.WriteLine("Please create an API.txt file in the same folder as the application .exe . \n");
        }

        // ChatGPT
        OpenAIAPI api = new OpenAIAPI(new APIAuthentication(strAPI));
        var chat = api.Chat.CreateConversation();


        void Save(string text, string path)
        {
            if (File.Exists(path))
            {
                var reader = File.ReadAllText(path);

                StreamWriter sw = new StreamWriter(path);
                sw.WriteLine(reader + text); // Append text to the existing file content
                sw.Close();
                Console.WriteLine("Saved.");
            }
            else
            {
                var create = File.CreateText(path);
                create.WriteLine(text); // Write text to the new file
                create.Close();
                Console.WriteLine("Created.");
            }
        }

        void Answer(string text)
        {
            Console.WriteLine(text);
            Console.WriteLine("");
        }

        string answer = "";
        string path = "Saved.txt";
        string version = "OpenAiConsole-Private 1.3";
        string error = "Technical error: ";

        var help = @"/Help - Shows every command.
/Quit - Quits the application.
/Exit - Exits the application.
/Clear - Clears the .txt file.
/Save - Saves the last OpenAI printed answer to .txt
/Cat - Lists the .txt file.
/Open - Opens the .txt file in notepad.
/v or /version - Shows the app's version.
/lv or /latestversion - Shows the latest version of the app.";

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("If you want to quit just type: '/quit or /exit.");
        Console.WriteLine("For more info type: /help");
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Ask anything:");
            var question = Console.ReadLine();
            if (question == "/quit")
            {
                break;
            }
            if (question == "/exit")
            {
                break;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            string beforeanswer = answer;
            var Character = '/';
            try
            {
                Character = question[0];
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(error + "Input empty. \n");
            }

            string firstChar = Character.ToString();
            if (!firstChar.Equals("/"))
            {
                //chat.AppendUserInput(question);
                chat.AppendUserInput("How to make a hamburger?");

                try
                {
                    await chat.StreamResponseFromChatbotAsync(res =>
                    {
                        Console.Write(res);
                    });
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("\n" + ex.Message);
                }
            }
            else if (question == "/help")
            {
                Console.WriteLine("\n" + help);
                Console.WriteLine("");
            }
            else if (question == "/clear")
            {
                StreamWriter sw = new StreamWriter(path);
                sw.WriteLine("");
                sw.Close();
                Console.WriteLine("Cleared.");
            }
            else if (question == "/save")
            {
                Save(beforeanswer, path);
            }
            else if (question == "/cat")
            {
                StreamReader sr = new StreamReader(path);
                string text = sr.ReadToEnd();
                sr.Close();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(text);
            }
            else if (question == "/open")
            {
                System.Diagnostics.Process.Start("notepad.exe", path);
            }
            else if (question == "/v" || question == "/version")
            {
                Console.WriteLine("Version: " + version);
            }
            else if (question == "/lv" || question == "/latestversion")
            {
                try
                {
                    WebClient wc = new WebClient();
                    string website = wc.DownloadString("https://github.com/RoxareX/OpenAiTesting/releases/latest");

                    var doc = new HtmlDocument();
                    doc.LoadHtml(website);
                    foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//h1"))
                    {
                        Console.WriteLine("Version: " + node.InnerHtml);
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("\n" + ex.Message);
                    Console.Error.WriteLine(error + "The website is private. \n");
                }
            }
        }
    }
}

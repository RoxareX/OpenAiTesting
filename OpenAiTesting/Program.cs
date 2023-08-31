// See https://aka.ms/new-console-template for more information
using System.Reflection.Metadata.Ecma335;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using OpenAI_API;

try
{


    string strAPI = "";
    try
    {
        var API = System.IO.File.ReadAllText("API.txt");
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

    // Functions
    void Save(string text, string path)
    {
        if (File.Exists(path))
        {
            var reader = System.IO.File.ReadAllText(path);

            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine((reader, text));
            sw.Close();
            Console.WriteLine("Saved.");
        }
        else
        {
            var create = System.IO.File.CreateText(path);
            create.Close();
            Console.WriteLine("Created.");
        }
    }

    void Answer(string text)
    {
        Console.WriteLine(text);
        Console.WriteLine("");
    }

    // Variables
    string answer = "";
    string path = "Saved.txt";
    string version = "OpenAiConsole-Private 1.3";
    string error = "Technical error: ";

    // Commands
    var help = @"/Help - Shows every command.
/Quit - Quits the application.
/Exit - Exits the application.
/Clear - Clears the .txt file.
/Save - Saves the last OpenAI printed answer to .txt
/Cat - Lists the .txt file.
/Open - Opens the .txt file in notepad.
/v or /version - Shows the apps version.
/lv or /latestversion - Shows the latest version of the app.";


    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("If you want to quit just type: '/quit or /exit.");
    Console.WriteLine("For more info type: /help");
    while (true)
    {
        // Code Beggining
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

        // OpenAI
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
            // answer = callOpenAI(300, question, "text-davinci-002", 0.7, 1, 0, 0, strAPI);
            //chat.AppendUserInput("Testing");
            //answer = await chat.GetResponseFromChatbotAsync();


            //Asnwer(answer);

            chat.AppendUserInput(question);

            try
            {
                answer = await chat.GetResponseFromChatbotAsync();
                Answer(answer);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("\n" + ex.Message);
            }
        }

        // Commands
        else if (question == "/help")
        {
            // Shows all commands
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
            // open folder
            System.Diagnostics.Process.Start("notepad.exe", path);
        }
        else if (question == "/v")
        {
            Console.WriteLine("Version: " + version);
        }
        else if (question == "/version")
        {
            Console.WriteLine("Version: " + version);
        }
        else if (question == "/lv")
        {
            try
            {
                // Gets the elements of the website
                WebClient wc = new WebClient();
                string website = wc.DownloadString("https://github.com/RoxareX/OpenAiTesting/releases/latest");

                // Lists the specified element
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
        else if (question == "/latestversion")
        {
            try
            {
                // Gets the elements of the website
                WebClient wc = new WebClient();
                string website = wc.DownloadString("https://github.com/RoxareX/OpenAiTesting/releases/latest");

                // Lists the specified element
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
} catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

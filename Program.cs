using System;
using System.Media;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Linq;

class Program
{
    private static string userName = "";
    private static readonly Random random = new Random();
    private static Dictionary<string, string> userPreferences = new Dictionary<string, string>();
    private static string currentTopic = "";

    // Keyword-response mappings with multiple possible responses
    private static Dictionary<string, List<string>> keywordResponses = new Dictionary<string, List<string>>()
    {
        { "password", new List<string> {
            "🔒 Strong passwords should be at least 12 characters long with a mix of uppercase, lowercase, numbers, and symbols.",
            "🔑 Avoid using personal information in passwords. Consider using a passphrase like 'PurpleTurtle$JumpsHigh!'",
            "💡 Use a password manager to generate and store unique passwords for each account."
        }},
        { "phishing", new List<string> {
            "🚩 Watch for urgent language in emails like 'Your account will be closed!' - scammers create false urgency.",
            "📧 Check sender addresses carefully. A fake Amazon email might come from 'service@amaz0n-support.com'.",
            "⚠️ Never click links in unexpected emails. Instead, go directly to the official website."
        }},
        { "privacy", new List<string> {
            "👁️ Review privacy settings on social media monthly - many platforms change defaults without notice.",
            "📱 Limit app permissions on your phone. Does a flashlight app really need access to your contacts?",
            "🌐 Use a VPN on public WiFi to encrypt your internet traffic and protect your data."
        }},
        { "scam", new List<string> {
            "💸 Remember: legitimate companies will never ask for payment in gift cards or cryptocurrency.",
            "📞 If someone calls claiming to be tech support, hang up and call the company directly using their official number.",
            "🕵️ Be wary of 'too good to be true' offers - they're often bait for scams."
        }}
    };

    // Sentiment analysis keywords and responses
    private static Dictionary<string, string> sentimentResponses = new Dictionary<string, string>()
    {
        { "worried", "I understand this can feel overwhelming. Cybersecurity is important, but there are simple steps you can take to protect yourself." },
        { "scared", "It's okay to feel concerned about online threats. Let me help you build confidence with some practical tips." },
        { "confused", "Cybersecurity can be complex. Let's break this down into simpler concepts. What specifically would you like to know?" },
        { "frustrated", "I hear your frustration. Technology can be challenging sometimes. Let's tackle one issue at a time." },
        { "happy", "Great to see you're enthusiastic about security! Let's build on that positive energy." }
    };

    static void Main(string[] args)
    {
        Console.Title = "Cybersecurity Awareness Chatbot";
        Console.Clear();

        DisplayAsciiArt();
        PlayWelcomeGreeting();
        GetUserName();
        RunChatLoop();
    }

    static void PlayWelcomeGreeting()
    {
        try
        {
            SoundPlayer player = new SoundPlayer("welcome.wav");
            player.Play();
        }
        catch
        {
            TypeWriteEffect("Audio greeting unavailable. Welcome to the Cybersecurity Awareness Bot!", ConsoleColor.Yellow);
        }
    }

    static void DisplayAsciiArt()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
  _______  _______  _______  _______ _________ _______  _______ 
 (  ____ \(  ___  )(  ____ )(  ____ \\__   __/(  ___  )(  ____ )
 | (    \/| (   ) || (    )|| (    \/   ) (   | (   ) || (    )|
 | |      | |   | || (____)|| (_____    | |   | |   | || (____)|
 | | ____ | |   | ||  _____)(_____  )   | |   | |   | ||     __)
 | | \_  )| |   | || (            ) |   | |   | |   | || (\ (   
 | (___) || (___) || )      /\____) |___) (___| (___) || ) \ \__
 (_______)(_______)|/       \_______)\_______/(_______)|/   \__/
                                                               
   _____ _________ _______  _______  _        _______  _______ 
  / ___ \\__   __/(  ___  )(  ____ )( \      (  ____ \(  ____ )
  \/   \/   ) (   | (   ) || (    )|| (      | (    \/| (    )|
      _____ | |   | |   | || (____)|| |      | (__    | (____)|
     ((___))| |   | |   | ||     __)| |      |  __)   |     __)
      )___ ( | |   | |   | || (\ (   | |      | (      | (\ (   
 (( /(___/|_) (___| (___) || ) \ \__| (____/\| (____/\| ) \ \__
  \|_______)\_______/(_______)|/   \__/(_______/(_______/|/   \__/
");
        Console.ResetColor();

        DrawSectionHeader("CYBERSECURITY AWARENESS CHATBOT", ConsoleColor.DarkYellow);
        Thread.Sleep(800);
    }

    static void GetUserName()
    {
        TypeWriteEffect("\nBefore we begin, what should I call you? ", ConsoleColor.White);
        userName = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(userName))
        {
            TypeWriteEffect("I didn't catch your name. Could you please tell me again? ", ConsoleColor.Red);
            userName = Console.ReadLine();
        }

        DrawSectionHeader($"WELCOME, {userName.ToUpper()}!", ConsoleColor.Green);
        TypeWriteEffect("I'm your Cybersecurity Awareness Assistant.\n", ConsoleColor.Cyan);
        TypeWriteEffect("I can help you with topics like:\n", ConsoleColor.White);
        TypeWriteEffect("• Password safety\n• Phishing detection\n• Privacy protection\n• Scam prevention", ConsoleColor.Magenta);
        Console.WriteLine();
    }

    static void RunChatLoop()
    {
        string[] helpOptions = {
            "» How to create strong passwords",
            "» How to recognize phishing emails",
            "» Protecting your privacy online",
            "» Avoiding scams",
            "» What's your purpose?",
            "» How are you?",
            "» Type 'exit' to end our chat"
        };

        DrawSectionHeader("HOW CAN I HELP YOU?", ConsoleColor.Blue);
        Console.ForegroundColor = ConsoleColor.Yellow;
        TypeWriteEffect("Here are some things you can ask me about:\n", 20);
        foreach (var option in helpOptions)
        {
            TypeWriteEffect(option + "\n", 30);
            Thread.Sleep(100);
        }
        Console.ResetColor();
        DrawDivider('═', ConsoleColor.DarkCyan);

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"\n[{userName}] ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("» ");
            Console.ResetColor();

            string input = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                TypeWriteEffect("I didn't hear anything. Could you repeat that?\n", 20);
                Console.ResetColor();
                continue;
            }

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                DrawSectionHeader($"GOODBYE, {userName.ToUpper()}!", ConsoleColor.Blue);

                // Recall user preferences in farewell if available
                if (userPreferences.ContainsKey("interest"))
                {
                    TypeWriteEffect($"As someone interested in {userPreferences["interest"]}, ", ConsoleColor.Magenta);
                }
                TypeWriteEffect("Remember to stay safe online!\n", ConsoleColor.Green);
                Thread.Sleep(1000);
                Environment.Exit(0);
            }

            string response = ProcessInput(input);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("[Bot] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("» ");
            TypeWriteEffect(response + "\n", 10);
            Console.ResetColor();
        }
    }

    static string ProcessInput(string input)
    {
        string lowerInput = input.ToLower();

        // Check for sentiment keywords first
        foreach (var sentiment in sentimentResponses)
        {
            if (lowerInput.Contains(sentiment.Key))
            {
                return sentiment.Value;
            }
        }

        // Check for cybersecurity keywords
        foreach (var topic in keywordResponses)
        {
            if (lowerInput.Contains(topic.Key))
            {
                // Store user interest for future personalization
                if (!userPreferences.ContainsKey("interest"))
                {
                    userPreferences.Add("interest", topic.Key);
                }
                else
                {
                    userPreferences["interest"] = topic.Key;
                }

                currentTopic = topic.Key;

                // Return a random response from the available options
                int responseIndex = random.Next(topic.Value.Count);
                return topic.Value[responseIndex];
            }
        }

        

        // General conversation
        if (lowerInput.Contains("how are you"))
        {
            return "I'm functioning optimally, thank you for asking! How about you?";
        }
        else if (lowerInput.Contains("purpose") || lowerInput.Contains("what do you do"))
        {
            return "My purpose is to help you stay safe online by providing cybersecurity awareness information.";
        }
        else if (lowerInput.Contains("help") || lowerInput.Contains("what can i ask"))
        {
            return "You can ask me about:\n- 🔒 Password security\n- 🚩 Phishing scams\n- 👁️ Privacy protection\n- 💸 Scam prevention\n- ℹ️ My purpose";
        }

        // Default response for unrecognized input
        if (!string.IsNullOrEmpty(currentTopic))
        {
            return $"I'm not sure I understand. Would you like more information about {currentTopic}?";
        }
        else
        {
            return "I didn't quite understand that. Try asking about:\n- 'password safety'\n- 'phishing emails'\n- 'privacy settings'\nOr type 'help' for options.";
        }
    }

    #region UI Enhancement Methods
    static void TypeWriteEffect(string text, int delay = 30)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep(random.Next(delay / 2, delay + 10));
        }
    }

    static void TypeWriteEffect(string text, ConsoleColor color, int delay = 30)
    {
        Console.ForegroundColor = color;
        TypeWriteEffect(text, delay);
        Console.ResetColor();
    }

    static void DrawSectionHeader(string text, ConsoleColor color)
    {
        Console.WriteLine();
        Console.ForegroundColor = color;
        Console.WriteLine($"╔{new string('═', text.Length + 2)}╗");
        Console.WriteLine($"║ {text} ║");
        Console.WriteLine($"╚{new string('═', text.Length + 2)}╝");
        Console.ResetColor();
        Thread.Sleep(300);
    }

    static void DrawDivider(char symbol, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(new string(symbol, Console.WindowWidth - 1));
        Console.ResetColor();
    }
    #endregion
}
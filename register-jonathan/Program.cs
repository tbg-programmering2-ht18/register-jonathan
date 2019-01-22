using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace register
{
    class Program
    {
        static Dictionary<string, string> userPasswdDict = new Dictionary<string, string>();
        static Dictionary<string, Animal> userAnimalDict = new Dictionary<string, Animal>();
        static void Main(string[] args)
        {
            Setup();

            bool userLoggedIn = false;
            bool done = false;

            string registeredPasswd = "";

            while (done == false)
            {
                Console.WriteLine("Enter your username: ");
                string user = Console.ReadLine();

                bool userExist = userPasswdDict.TryGetValue(user, out registeredPasswd);
                if (userExist)
                {
                    Console.Write("Enter your password: ");
                    String passwd = ReadPassword();

                    if (passwd.CompareTo(registeredPasswd) == 0)
                    {
                        Console.WriteLine("\nWelcome {0}: You are now logged in!", user);
                        Animal registredAnimal;
                        bool animalExist = userAnimalDict.TryGetValue(user, out registredAnimal);
                        if (animalExist)
                        {
                            Console.WriteLine("This is your animal:{0}", registredAnimal.Show());
                        }
                        else
                        {
                            Console.WriteLine("Sorry, you have no animals!");
                        }
                        Console.WriteLine("Enter any key to log out and exit...");
                        done = true;
                        userLoggedIn = false;
                        Console.ReadKey();

                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid username or password!");
                    }
                }
                else
                {
                    Console.WriteLine("The user {0} is not found", user);
                    Console.Write("Try again? (Enter Yes or No): ");
                    String answer = Console.ReadLine();
                    done = (!answer.ToLower().StartsWith("y"));
                }
            }
        }

        private static void Setup()
        {
            String path = @"C:\Temp\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            String urFilename = path + "user_register.txt";
            if (!File.Exists(urFilename))
            {
                FileStream f = File.Create(urFilename);
                f.Close();
                userPasswdDict.Add("Jonis", "adamärfin");
                userPasswdDict.Add("adam", "picklerick");
                userPasswdDict.Add("putte", "123");

                string jsonuserPasswd = JsonConvert.SerializeObject(userPasswdDict, Formatting.Indented);
                File.WriteAllText(urFilename, jsonuserPasswd);
            }
            else
            {
                string json = File.ReadAllText(urFilename);
                userPasswdDict = JsonConvert.DeserializeObject<Dictionary<String, String>>(json);
            }

            String arFilename = path + "animal_register.txt";
            if (!File.Exists(arFilename))
            {
                FileStream f = File.Create(arFilename);
                f.Close();
                userAnimalDict.Add("Jonis", new Animal("Weeaboo", "PV", "RawrxD", true));
                userAnimalDict.Add("Olle", new Animal("dog", "Santa", "woof", false));

                string jsonUserAnimal = JsonConvert.SerializeObject(userAnimalDict, Formatting.Indented);
                File.WriteAllText(arFilename, jsonUserAnimal);
            }
            else
            {
                string json = File.ReadAllText(arFilename);
                userAnimalDict = JsonConvert.DeserializeObject<Dictionary<String, Animal>>(json);
            }

        }

        private static string ReadPassword()
        {
            String pass = "";
            ConsoleKeyInfo key = Console.ReadKey(true);
            while (key == null || key.Key != ConsoleKey.Enter)
            {
                pass += key.KeyChar; //pass = pass + key.KeyChar
                Console.Write("*");
                key = Console.ReadKey(true);
            }
            return pass;
        }
    }
}
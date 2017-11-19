using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace PlanerUEK
{
    class Program
    {
        private enum Groups { IS1011 = 84721, IS1012 = 84731, IS1013 = 84741, IS1014 = 84751 }

        static void Main(string[] args)
        {
            Greet();
            int studentGroup = GetStudentGroup();
            Console.WriteLine(studentGroup);
            Console.ReadKey();
        }

        private static void Greet() {
            Console.WriteLine("Welcome to Planer UEK! In order to save your classes in your Google Calendar you have to choose your student group.");
            Console.WriteLine("Type in corresponding number and hit Enter: ");
            Console.WriteLine("1 -> KrDZIs1011\n2 -> KrDZIs1012\n3 -> KrDZIs1013\n4 -> KrDZIs1014");
        }
        private static void ShowIncorrectInputMessage() {
            Console.Clear();
            Console.WriteLine("You typed invalid character, try again.");
            Console.WriteLine("1 -> KrDZIs1011\n2 -> KrDZIs1012\n3 -> KrDZIs1013\n4 -> KrDZIs1014");
        }

        private static int GetStudentGroup() {
            bool isInputValid = false;
            int userInput;
            while (!isInputValid)
            {
                if (int.TryParse(Console.ReadLine(), out userInput))
                {
                    Console.Clear();
                    switch (userInput)
                    {
                        case 1:
                            return (int) Groups.IS1011;
                        case 2:
                            return (int) Groups.IS1012;
                        case 3:
                            return (int) Groups.IS1013;
                        case 4:
                            return (int) Groups.IS1014;
                        default:
                            ShowIncorrectInputMessage();
                            break;
                    }
                }
                else
                {
                    ShowIncorrectInputMessage();
                }
            }
            return 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System.Models
{
    public class SecurityQuestions
    {
        const int MINIMUM_SECURITY_QUESTIONS = 5;

        Dictionary<string, string> answerSecurityQuestions = new Dictionary<string, string>(); //dictionary holds key and value 

        //array of security questions
        static string[] securityQuestionsArray =
        {
            "What is your favorite color?"
            , "What is your favorite food?"
            , "What is the name of your first pet?"
            , "What is your favorite animal?"
            , "What is your favorite sports team?"
            , "What was the name of your first school?"
            , "What is your favorite hobby?"
            , "What city were you born in?"
            , "What is your favorite vacation spot?"
            , "What is your favorite movie?"
            , "What is the name of your best friend?"
            , "What was your childhood nickname?"
            , "What is your favorite subject in school?"
            , "What is your mother's maiden name?"
            , "What is your favorite game?"
            , "What is the name of your childhood best friend?"
        };
        public void InputSecurityQuestions()
        {
            Console.WriteLine($"Fill out {MINIMUM_SECURITY_QUESTIONS} security questions");
            AskSecurityQuestions();

            while (answerSecurityQuestions.Count < MINIMUM_SECURITY_QUESTIONS)
            {
                Console.WriteLine($"Error, need to have {MINIMUM_SECURITY_QUESTIONS} security questions");
                NewSecurityQuestions();
            }
        }
        void AskSecurityQuestions()
        {
            for (int i = 0; i < securityQuestionsArray.Length; i++) //for loop go through the list of string 
            {
                Console.WriteLine(securityQuestionsArray[i]);
                string? answer = Console.ReadLine();

                //convert's users string all upper case and removes in front and end white spaces in a string
                answer = answer.ToUpper().Trim();

                if (answer != null && answer != "")
                {
                    if (!answerSecurityQuestions.ContainsKey(securityQuestionsArray[i]))
                    {
                        answerSecurityQuestions.Add(securityQuestionsArray[i], answer); //add security question to dictionary
                    }
                    else
                    {
                        answerSecurityQuestions[securityQuestionsArray[i]] = answer; //changing its definition
                    }
                }
            }
        }
        void NewSecurityQuestions()
        {
            for (int i = 0; i < securityQuestionsArray.Length; i++) //for loop go through the list of string 
            {
                if (answerSecurityQuestions.ContainsKey(securityQuestionsArray[i]))
                {
                    continue;
                }

                Console.WriteLine(securityQuestionsArray[i]);
                string? answer = Console.ReadLine();

                //convert's users string all upper case and removes in front and end white spaces in a string
                answer = answer.ToUpper().Trim();

                if (answer != null && answer != "")
                {
                    if (!answerSecurityQuestions.ContainsKey(securityQuestionsArray[i]))
                    {
                        answerSecurityQuestions.Add(securityQuestionsArray[i], answer); //add security question to dictionary
                    }
                    else
                    {
                        answerSecurityQuestions[securityQuestionsArray[i]] = answer; //changing its definition
                    }
                }
            }
        }
    }
}

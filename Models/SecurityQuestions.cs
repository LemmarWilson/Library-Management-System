namespace Library_Management_System.Models
{
    /**
     * Represents a system for managing security questions and answers.
     * Users must select and answer a specified number of questions for security purposes.
     */
    public class SecurityQuestions
    {
        // Minimum number of security questions required
        private const int MINIMUM_SECURITY_QUESTIONS = 5;

        // Stores selected questions and their corresponding answers
        private Dictionary<string, string> answerSecurityQuestions = new Dictionary<string, string>();

        // Array of available security questions for users to choose from
        private static readonly string[] securityQuestionsArray =
        {
            "What is your favorite color?",
            "What is your favorite food?",
            "What is the name of your first pet?",
            "What is your favorite animal?",
            "What is your favorite sports team?",
            "What was the name of your first school?",
            "What is your favorite hobby?",
            "What city were you born in?",
            "What is your favorite vacation spot?",
            "What is your favorite movie?",
            "What is the name of your best friend?",
            "What was your childhood nickname?",
            "What is your favorite subject in school?",
            "What is your mother's maiden name?",
            "What is your favorite game?",
            "What is the name of your childhood best friend?"
        };

        /**
         * Allows the user to select and answer a specified number of security questions.
         * The selected questions are removed from the list of available questions to avoid duplication.
         */
        public void InputSecurityQuestions()
        {
            Console.WriteLine($"Please select and answer {MINIMUM_SECURITY_QUESTIONS} security questions.");

            // Create a mutable list of available questions
            var availableQuestions = new List<string>(securityQuestionsArray);

            // Loop until the required number of questions are answered
            while (answerSecurityQuestions.Count < MINIMUM_SECURITY_QUESTIONS)
            {
                Console.WriteLine("\nAvailable Questions:");
                for (int i = 0; i < availableQuestions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {availableQuestions[i]}");
                }

                Console.Write("Select a question by entering its number: ");
                if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > availableQuestions.Count)
                {
                    Console.WriteLine("Invalid choice. Please select a valid question number.");
                    continue;
                }

                string selectedQuestion = availableQuestions[choice - 1];
                Console.Write($"Answer for '{selectedQuestion}': ");
                string? answer = Console.ReadLine()?.ToUpper().Trim();

                if (string.IsNullOrEmpty(answer))
                {
                    Console.WriteLine("Answer cannot be empty. Please try again.");
                    continue;
                }

                // Save the question and answer, then remove the question from the list
                answerSecurityQuestions[selectedQuestion] = answer;
                availableQuestions.RemoveAt(choice - 1);

                Console.WriteLine($"Question added. {MINIMUM_SECURITY_QUESTIONS - answerSecurityQuestions.Count} more to go.");
            }

            Console.WriteLine("Security questions have been set successfully.");
        }

        /**
         * Retrieves the security questions and their corresponding answers.
         *
         * @return A dictionary containing the selected security questions as keys
         *         and their respective answers as values.
         */
        public Dictionary<string, string> GetSecurityQuestions()
        {
            // Return a copy of the security questions to avoid external modifications
            return new Dictionary<string, string>(answerSecurityQuestions);
        }
    }
}

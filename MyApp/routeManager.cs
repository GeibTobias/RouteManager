using System;

namespace Program
{
    using Stage = List<int>;

    struct UserInput{
        public int numberOfStages;
        public int numberOfSteps;
        public List<int> steps;
    };

    class RouteManager {

        const string DAY_STRING = ". Tag: ";
        const string KM_STRING = " km";
        const string NEW_LINE = "\n";
        const string MAX_STRING = "Maximum: ";
 
        public static void Main() {

            UserInput userInput = retrieveUserInput();

            List<Stage> result = getStageSegmentations(userInput);
            
            parseResult(result);
        }

        private static UserInput retrieveUserInput() {
            UserInput userInput;

            int userInputNoSteps = getIntegerInput();
            int userInputNoStages = getIntegerInput();
            Stage userInputSteps = new Stage{};

            for(int i = 0; i < userInputNoSteps ; i++) {
                int currentStep = getIntegerInput();
                userInputSteps.Add(currentStep);
            }
            userInput.numberOfSteps = userInputNoSteps;
            userInput.numberOfStages = userInputNoStages;
            userInput.steps = userInputSteps;
            return userInput;
        }

        private static int getIntegerInput() {
            string input ="" ;
            int result = 0;
            while (result <= 0) {
                input = Console.ReadLine() ?? string.Empty;;
                try {
                    result = Int32.Parse(input);
                } catch (FormatException) {
                        Console.WriteLine("Invalid Input, NaN");
                } catch (OverflowException) {
                        Console.WriteLine("Invalid Input, Input too large");
                }
                if (result <= 0) {                
                    Console.WriteLine("Please enter a positive integer");
                }
            }
            return result;
        }

        private static List<Stage> getStageSegmentations(UserInput userInput) {
            int minStartStep = userInput.steps.Max();
            int maxWindow = userInput.steps.Sum();
            int meanStepSize = maxWindow / userInput.numberOfSteps;
            int currentWindow = minStartStep > meanStepSize ? minStartStep : meanStepSize;

            List<Stage> result = getStagesForMaxStep(userInput.steps, currentWindow, userInput.numberOfStages);

            while (true){
                if (checkResultFinished(result, currentWindow, maxWindow, userInput)) {
                    break;
                }
                // Increase Window Size
                while (result.Count > userInput.numberOfStages ) {
                    result = getStagesForMaxStep(userInput.steps, currentWindow, userInput.numberOfStages);
                    currentWindow = meanOfTwoSteps(currentWindow, maxWindow);
                }
                if (checkResultFinished(result, currentWindow, maxWindow, userInput)) {
                    break;
                }
                // Decrease window size
                maxWindow = currentWindow;
                currentWindow = meanOfTwoSteps(minStartStep, currentWindow);
                result = getStagesForMaxStep(userInput.steps, currentWindow, userInput.numberOfStages);
            }
            return result;
        }

        private static bool checkResultFinished(List<Stage> result, int currentWindow, int maxWindow, UserInput userInput) {
            bool isFinished = currentWindow == maxWindow;
            if (!isFinished && result.Count == userInput.numberOfStages) {
                List<Stage> windowOneSmaller = getStagesForMaxStep(userInput.steps, currentWindow-1, userInput.numberOfStages);
                if (windowOneSmaller.Count > userInput.numberOfStages) {
                    isFinished = true;
                }
            }
            return isFinished;
        }

        private static int meanOfTwoSteps(int a, int b) {
            int result = (a + b) / 2;
            return result;
        } 
            
        private static List<Stage> getStagesForMaxStep(List<int> distances, int stepSize, int numberOfStages) {
            Stage currentStage = new Stage{};
            List<Stage>result = new List<Stage>{};
            for (int i = 0; i < distances.Count; i++) {
                int dist = distances.ElementAt(i);
                int remaining_steps = distances.Count - i;
                if (currentStage.Sum() + dist <= stepSize && remaining_steps >= numberOfStages - result.Count) {
                    currentStage.Add(dist);
                } else {
                    result.Add(currentStage);
                    currentStage = new Stage{dist};
                }
            }
            result.Add(currentStage);
            return result;
        }

        private static void parseResult(List<Stage> result) {
            if (result != null) {
                int currentDay = 1;
                Console.WriteLine(NEW_LINE);
                foreach (Stage stage in result) {
                    int currentStageTotal = stage.Sum();
                    Console.WriteLine(currentDay.ToString() + DAY_STRING + currentStageTotal.ToString() + KM_STRING);
                    currentDay++;
                }
                Console.WriteLine(NEW_LINE + MAX_STRING + getMaxStage(result).ToString() + KM_STRING);
            }
        }

        private static int getMaxStage(List<Stage> stages) {
            List<int> sums = new List<int>{};
            foreach (Stage stage in stages) {
                sums.Add(stage.Sum());
            }
            return sums.Max();
        }
    }
}

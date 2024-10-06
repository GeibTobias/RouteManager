using System;
using System.Reflection.Metadata;

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
            int minIncrementStep = manyIntegerGCD(userInput.steps);
            int currentIncrement = 0;

            List<Stage> result = getStagesForMaxStep(userInput.steps, minStartStep, userInput.numberOfStages);

            while (result.Count > userInput.numberOfStages) {
                currentIncrement += minIncrementStep;
                result = getStagesForMaxStep(userInput.steps, minStartStep + currentIncrement, userInput.numberOfStages);
            }

            return result;
        }
            
        private static List<Stage> getStagesForMaxStep(List<int> distances, int stepSize, int numberOfStages) {
            Console.WriteLine("Current Step size: " + stepSize.ToString());
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

        private static int greatesCommonDivider(int a, int b) {
            while (a != b && a > 1 && b > 1) {
                if (b > a) {
                    int temp = a;
                    a = b;
                    b = temp;
                }
                a -= b;
            }
            return a;
        }

        private static int manyIntegerGCD(List<int> manyInts) {
            if (manyInts.Count != 0) {
                int result = manyInts[0];
                for (int i = 0; i < manyInts.Count; i++) {
                    result = greatesCommonDivider(result, manyInts[i]);
                }
                return result;
            } else {
                return 0;
            }
        }
    }
}

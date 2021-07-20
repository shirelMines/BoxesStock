using BoxesLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace testing
{
    public class User:IUserDialogue
    {
        public void AlertBoxDeleted(double width, double height)
        {
            Console.WriteLine($"DELETED BOX - box in size: {width} x {height}.");
        }

        public void AlertLowStock(double width, double height)
        {
            Console.WriteLine($"LOW STOCK - box in size: {width} x {height}.");
        }

        public bool AskUserforSplitApproval(List<BoxType> boxList, int requiredCount)
        {
            string str = "";
            for (int i = 0; i < boxList.Count; i++)
            {
                if (boxList.Count - 1 == i)
                {
                    str += $"{requiredCount} boxes in size: bottom {boxList[i].Bottom}, hieght {boxList[i].Height}.\n";
                    break;
                }

                str += $"{boxList[i].DataY.Count} boxes in size: bottom {boxList[i].Bottom}, hieght {boxList[i].Height}.\n";
                requiredCount -= boxList[i].DataY.Count;
            }

            Console.WriteLine($"Sorry, we don't have enough suitable boxes of the same size.\n" +
               $"We can split your order into these options:\n\n{str}\nDo you agree to the purchase? True/False");

            bool userAnswer;
            do
            {
                if (bool.TryParse(Console.ReadLine(), out userAnswer))
                    return userAnswer;
                else
                    Console.WriteLine("Not valid input. try again..");

            } while (true);
        }

        public void PurchaseSingleBox(double x, double y, int requiredCount)
        {
            Console.WriteLine($"Your purchase is {requiredCount} boxes in size: bottom {x}, hieght {y}.\n");
        }
    }
}


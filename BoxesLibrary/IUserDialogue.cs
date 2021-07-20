using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesLibrary
{
    public interface IUserDialogue
    {
        void AlertLowStock(double x, double y);

        void AlertBoxDeleted(double x, double y);

        bool AskUserforSplitApproval(List<BoxType> boxList, int requiredCount);

        void PurchaseSingleBox(double x, double y, int requiredCount);
    }
}

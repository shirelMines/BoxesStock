using System;
using System.Collections.Generic;
using System.Threading;

namespace BoxesLibrary
{
    public class Manager
    {
        internal BinarySearchTree<BoxBottom_X> mainTree;
        internal DateLinkedList<TimeData> dateList;
        private const int MAX_COUNT = 5000;
        private const int MIN_COUNT = 5;
        private const int ROUND_VALUE = 2;
        private const int SPLITS = 3;
        private const double MAX_EXCEEDING = 1.5;
        private const int BOX_LIFE_TIME = 30;
        private readonly double DEVIATION;
        public IUserDialogue user;


        public Manager(IUserDialogue userDialogue)
        {
            user = userDialogue;
            mainTree = new BinarySearchTree<BoxBottom_X>();
            dateList = new DateLinkedList<TimeData>();
            DEVIATION = 1 / Math.Pow(10, ROUND_VALUE);

            DateTime tomorrowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0 ,0).AddDays(1);
            TimeSpan dueTime = tomorrowDate.Subtract(DateTime.Now);
            TimeSpan period = new TimeSpan(1, 0, 0, 0);
            Timer timer = new Timer(DeleteEventHandler, null, dueTime, period);
        }

        public void Add(double x, double y, int count)
        {
            //check valid input
            if (x <= 0 || y <= 0 || count <= 0) throw new ArgumentException("Incorrect box values");

            x = Math.Round(x, ROUND_VALUE);
            y = Math.Round(y, ROUND_VALUE);

            BoxHeight_Y foundBoxHeight;
            BoxHeight_Y boxHeight = new BoxHeight_Y(y, count);

            BoxBottom_X foundBoxBottom;
            BoxBottom_X boxBottom = new BoxBottom_X(x, boxHeight);

            if (mainTree.AddAndSearch(boxBottom, out foundBoxBottom))//if we find bottom in main tree
            {
                if (foundBoxBottom.YTree.AddAndSearch(boxHeight, out foundBoxHeight))//if we find height
                {
                    foundBoxHeight.Count += count;

                    if (foundBoxHeight.Count > MAX_COUNT)
                    {
                        foundBoxHeight.Count = MAX_COUNT;
                    }
                }
                else
                {
                    dateList.AddLast(new TimeData(x, y));
                    boxHeight.TimeNodeRef = dateList.End;
                }
            }
            else
            {
                dateList.AddLast(new TimeData(x, y));
                boxHeight.TimeNodeRef = dateList.End;
            }
        }

        public string GetData(double x, double y)
        {
            BoxBottom_X foundBoxBottom;
            BoxBottom_X boxBottom = new BoxBottom_X(x);

            if (mainTree.Search(boxBottom, out foundBoxBottom))//if we find bottom in main tree
            {
                BoxHeight_Y foundBoxHeight;
                BoxHeight_Y boxHeight = new BoxHeight_Y(y);

                if (foundBoxBottom.YTree.Search(boxHeight, out foundBoxHeight))//if we find height
                {
                    return $"Box size - width:{x}, height:{y}, stock:{foundBoxHeight.Count}";
                }
                else// if we DONT find height 
                {
                    return null;
                }
            }
            else
                return null;
        }

        private bool CheckSize(double boxSize, double giftSize) => (giftSize * MAX_EXCEEDING >= boxSize);

        private void ReduceCount(BoxType box, int requiredCount)
        {
            box.DataY.Count -= requiredCount;///reduce count

            if (box.DataY.Count > 0)
            {
                if (box.DataY.Count <= MIN_COUNT && box.DataY.Count > 0)
                {
                    user.AlertLowStock(box.Bottom, box.Height);//inform user 
                }
                UpdateLinkedList(box.DataY);///update box date in linkedlist
            }
            else
            {
                user.AlertBoxDeleted(box.Bottom, box.Height);
                DeleteBoxByCount(box);//DeleteBox
            }
        }

        public bool FindBestMatch(double x, double y, int requiredCount)
        {
            BoxBottom_X foundBottom;
            BoxBottom_X giftBottom = new BoxBottom_X(x);

            BoxHeight_Y foundHeight;
            BoxHeight_Y giftHeight = new BoxHeight_Y(y);

            //initialize list and count for split
            int sumCount = 0;
            List<BoxType> fitBoxesList = new List<BoxType>();
            int maxCurrentSplit = SPLITS;

            while (maxCurrentSplit > 0 && mainTree.SearchNearest(giftBottom, out foundBottom) && CheckSize(foundBottom.Bottom, x)) //if we find next suitable bottom
            {
                while (maxCurrentSplit > 0 && foundBottom.YTree.SearchNearest(giftHeight, out foundHeight) && CheckSize(foundHeight.Height, y)) //if we find height
                {
                    if (foundHeight.Count >= requiredCount)//check if we have enough boxes to sell from best option 
                    {
                        BoxType fitBoxNoSplit = new BoxType(foundBottom, foundHeight);
                        user.PurchaseSingleBox(foundBottom.Bottom, foundHeight.Height,requiredCount);
                        ReduceCount(fitBoxNoSplit, requiredCount);
                        return true;
                    }
                    else //split boxes
                    {
                        BoxType fitBox = new BoxType(foundBottom, foundHeight);
                        fitBoxesList.Add(fitBox);
                        sumCount += fitBox.DataY.Count;

                        if (sumCount >= requiredCount)
                        {
                            if (user.AskUserforSplitApproval(fitBoxesList, requiredCount))//ask the user if he wants to split boxes if true, execute purchase
                            {
                                foreach (var item in fitBoxesList)
                                {
                                    int originalBoxCount = item.DataY.Count;
                                    ReduceCount(item, requiredCount);
                                    requiredCount -= originalBoxCount;
                                }
                                return true;
                            }
                            return false;//if user enters false
                        }
                        maxCurrentSplit--;
                    }
                    giftHeight.Height = foundHeight.Height + DEVIATION;//add deviation to height
                }
                giftBottom.Bottom = foundBottom.Bottom + DEVIATION;//add deviation to bottom
            }
            return false;
        }

        private void UpdateLinkedList(BoxHeight_Y boxHeight)
        {
            dateList.ReplaceNodePosition(boxHeight.TimeNodeRef);
            dateList.End.data.DateLastPurchase = DateTime.Now;
        }

        private void DeleteBoxByCount(BoxType box)
        {
            dateList.DeleteNode(box.DataY.TimeNodeRef);//delete from linked list

            box.BoxX.YTree.Remove(box.DataY);//delete from tree
            if (box.BoxX.YTree.IsTreeEmpty())
                mainTree.Remove(box.BoxX);
        }

        private void DeleteEventHandler(object param)
        {
            while (dateList.Start != null && DateTime.Now > dateList.Start.data.DateLastPurchase.AddDays(BOX_LIFE_TIME))
            {
                TimeData boxToDelete;
                dateList.RemoveFirst(out boxToDelete);

                BoxBottom_X foundBottom; //delete box from tree
                mainTree.Search(new BoxBottom_X(boxToDelete.Bottom), out foundBottom);
                foundBottom.YTree.Remove(new BoxHeight_Y(boxToDelete.Height));
                if (foundBottom.YTree.IsTreeEmpty())
                    mainTree.Remove(foundBottom);
            }
        }
    }
}

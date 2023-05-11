using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TowerOfHanoiPuzzle
{
    /// <summary>
    /// Rules of the game. Disregards geometry and animation.
    /// </summary>
    public class GameLogic
    {
        class DiskStack
        {
            int[] values;
            int count;

            public int this[int index]
            {
                get
                {
                    if (index < 0 || index >= count)
                        throw new IndexOutOfRangeException();
                    return values[index];
                }
                set
                {
                    if (index > count)
                        throw new IndexOutOfRangeException();

                    if (index == count)
                        AddTop(value);
                    else
                        values[index] = value;
                }
            }
            public int Capacity
            {
                get
                {
                    return values.Length;
                }
                set
                {
                    if (values.Length != value)
                    {
                        Array.Resize(ref values, value);
                        count = Math.Min(count, value - 1);
                    }
                }
            }
            public int Top
            {
                get
                {
                    if (IsEmpty())
                        return int.MaxValue;
                    return values[count - 1];
                }
                set
                {
                    if (count == 0)
                        AddTop(value);
                    else
                        values[count - 1] = value;
                }
            }
            public int Bottom
            {
                get
                {
                    return values[0];
                }
                set
                {
                    if (count == 0)
                        AddTop(value);
                    else
                        values[0] = value;
                }
            }
            public int NumberOfDisks
            {
                get
                {
                    return count;
                }
            }
            public void AddTop(int value)
            {
                values[count++] = value;
            }
            public void RemoveTop()
            {
                count--;
            }
            public void AddBottom(int value)
            {
                for (int n = count; n > 0; --n)
                    values[n] = values[n - 1];
                count++;
                values[0] = value;
            }
            public void RemoveBottom()
            {
                --count;
                for (int n = 0; n < count; ++n)
                    values[n] = values[n + 1];
            }
            public bool IsEmpty()
            {
                return count == 0;
            }
            public void Clear()
            {
                count = 0;
            }
            public DiskStack(int size = 0)
            {
                values = new int[size];
                count = 0;
            }
        }
        public struct Move
        {
            public int Start, End;

            public Move(int start = 0, int end = 0)
            {
                Start = start;
                End = end;
            }
        }

        int[] diskRodIndex = new int[1];
        DiskStack[] stacks = new DiskStack[3];
        Move nextMove;
        int initialRod = 0, targetRod = 2;
        bool updateSolutionMove;

        bool Solve(int largestDisk, int targetStack)
        {
            int currentStack = diskRodIndex[largestDisk];

            if (currentStack == targetStack)
            {
                if (largestDisk == 0)
                    return false;

                return Solve(largestDisk - 1, targetStack);
            }

            if (stacks[currentStack].Top == largestDisk && stacks[targetStack].Top > largestDisk)
            {
                nextMove.Start = currentStack;
                nextMove.End = targetStack;
                return true;
            }

            return Solve(largestDisk - 1, 3 - targetStack - currentStack);
        }
        void GenerateSolutionMove()
        {
            if (!IsSolved())
                Solve(diskRodIndex.Length - 1, targetRod);
        }

        public int TargetRod
        {
            get
            {
                return targetRod;
            }
            set
            {
                value = Math.Max(Math.Min(value, 2), 0);
                if (value != targetRod)
                {
                    updateSolutionMove = true;
                    targetRod = value;
                }
            }
        }
        public int NumberOfDisks
        {
            get
            {
                return diskRodIndex.Length;
            }
            set
            {
                if (diskRodIndex.Length != value)
                {
                    int oldLength = diskRodIndex.Length;
                    Array.Resize(ref diskRodIndex, value);

                    for (int n = oldLength; n < value; ++n)
                        diskRodIndex[n] = 0;

                    for (int n = 0; n < 3; ++n)
                    {
                        stacks[n].Clear();
                        stacks[n].Capacity = value;
                    }

                    for (int n = value - 1; n >= 0; --n)
                        stacks[diskRodIndex[n]].AddTop(n);

                    updateSolutionMove = true;
                }
            }
        }
        public int GetNumberOfDisks(int rod)
        {
            return stacks[rod].NumberOfDisks;
        }
        public int GetTopDisk(int rod)
        {
            return stacks[rod].Top;
        }
        public int GetRodOfDisk(int disk)
        {
            return diskRodIndex[disk];
        }
        public int GetDiskAt(int rod, int position)
        {
            return stacks[rod][position];
        }
        public IEnumerable<int> GetDisks(int rod)
        {
            for (int n = 0; n < stacks[rod].NumberOfDisks; ++n)
                yield return stacks[rod][n];
            yield break;
        }

        public bool CanMove(Move move)
        {
            return stacks[move.Start].Top < stacks[move.End].Top;
        }
        public bool CanMove(int start, int end)
        {
            return stacks[start].Top < stacks[end].Top;
        }
        public void MoveDisk(Move move)
        {
            if (CanMove(move))
            {
                int diskFrom = stacks[move.Start].Top;
                stacks[move.Start].RemoveTop();
                stacks[move.End].AddTop(diskFrom);
                diskRodIndex[diskFrom] = move.End;
                updateSolutionMove = true;
            }
        }
        public void MoveDisk(int start, int end)
        {
            if (CanMove(start, end))
            {
                int disk = stacks[start].Top;
                stacks[start].RemoveTop();
                stacks[end].AddTop(disk);
                diskRodIndex[disk] = end;
                updateSolutionMove = true;
            }
        }

        public bool IsSolved()
        {
            return stacks[(initialRod + 1) % 3].NumberOfDisks == NumberOfDisks ||
                   stacks[(initialRod + 2) % 3].NumberOfDisks == NumberOfDisks;
        }
        public Move NextSolutionMove
        {
            get
            {
                if (updateSolutionMove)
                {
                    GenerateSolutionMove();
                    updateSolutionMove = false;
                }
                return nextMove;
            }
        }
        public void Randomize()
        {
            for (int n = 0; n < 3; ++n)
                stacks[n].Clear();

            for (int n = NumberOfDisks - 1; n >= 0; --n)
            {
                diskRodIndex[n] = UnityEngine.Random.Range(0, 3);
                stacks[diskRodIndex[n]].AddTop(n);
            }
        }

        public void Restart(int initialRod = 0)
        {
            this.initialRod = Math.Max(Math.Min(initialRod, 2), 0);
            for (int n = 0; n < 3; ++n)
                stacks[n].Clear();

            for (int n = NumberOfDisks - 1; n >= 0; --n)
            {
                diskRodIndex[n] = initialRod;
                stacks[initialRod].AddTop(n);
            }
        }
        public string GetGameStateRepresentation()
        {
            StringBuilder stringBuilder = new StringBuilder(NumberOfDisks + 1);
            stringBuilder.Append(initialRod.ToString());
            for (int n = 0; n < NumberOfDisks; ++n)
                stringBuilder.Append(diskRodIndex[n].ToString());
            return stringBuilder.ToString();
        }
        public void SetGameState(string stateRepresentation)
        {
            if (stateRepresentation == null || stateRepresentation.Length < 2) return;
            foreach (char ch in stateRepresentation)
                if (ch != '0' && ch != '1' && ch != '2') return;
            
            int number = stateRepresentation.Length - 1;
            diskRodIndex = new int[number];
            initialRod = stateRepresentation[0] - '0';

            for (int n = 0; n < 3; ++n)
            {
                stacks[n].Clear();
                stacks[n].Capacity = number;
            }
            for (int n = 0; n < number; ++n)
            {
                diskRodIndex[n] = stateRepresentation[n + 1] - '0';
                stacks[diskRodIndex[n]].AddBottom(n);
            } 
        }
        public GameLogic()
        {
            nextMove = new Move();
            nextMove.Start = 0;
            nextMove.End = 0;
            for (int n = 0; n < 3; ++n)
                stacks[n] = new DiskStack(NumberOfDisks);
            stacks[0].AddTop(0);
            updateSolutionMove = false;
        } 
    }
}
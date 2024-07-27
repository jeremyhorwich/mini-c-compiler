using System.Collections.Generic;

namespace CompilerUtility
{
    public class SmartList<T>
    {
        private List<T> list;
        private int pointer = -1;
        private Stack<int> pointerStates = new Stack<int>();
        public T Current
        {
            get
            {
                if (pointer < list.Count)
                {
                    return list[pointer];
                }
                throw new InvalidOperationException("Enumerator is positioned after the last element.");
            }
        }

        public SmartList(List<T> _list)
        {
            list = _list;
            pointerStates.Push(-1);
        }

        public bool MoveNext()
        {
            pointer++;
            return pointer < list.Count;
        }

        public void SaveState()
        {
            pointerStates.Push(pointer);
        }

        public void RestoreState()
        {
            pointer = pointerStates.Pop();
        }

        public void DeleteState()
        {
            pointerStates.Pop();
        }
    }
}
namespace CompilerUtility
{
    public class SmartList<T>
    {
        private List<T> list;
        private int pointer = -1;
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
        }

        private SmartList(List<T> _list, int pointerValue)
        {
            list = _list;
            pointer = pointerValue;
        }

        public bool MoveNext()
        {
            pointer++;
            return pointer < list.Count;
        }

        public SmartList<T> Clone()
        {
            return new SmartList<T>(list, pointer);
        }
    }
}
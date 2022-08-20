using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.DS
{
    public class Stack<T>
    {
        private StackArray<T> _stackArr;
        public Stack(int size)
        {
            _stackArr = new StackArray<T>(size);
        }
        public void Push(T item)
        {
            _stackArr.Push(item);
        }
        public T Pop()
        {
           return  _stackArr.Pop();
        }
        public T Peek()
        {
            return _stackArr.Peek();
        }
        public bool IsEmpty()
        {
            return _stackArr.IsEmpty();
        }

    }
    internal class StackArray<T>
    {
        T[] _items;
        int _curIndex = -1;
        public StackArray(int size)
        {
            _items = new T[size];
        }
        public void Push(T item)
        {
            if (_curIndex > _items.Length) { throw new StackOverflowException(); }
            _items[++_curIndex] = item;
        }
        public T Pop()
        {
            if(_curIndex==-1) { throw new ArgumentOutOfRangeException(); }
            return _items[_curIndex--];
        }
        public T Peek()
        {
            if (_curIndex == -1) { throw new ArgumentOutOfRangeException(); }
            return _items[_curIndex];
        }
        public bool IsEmpty()
        {
            return _curIndex <0;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.DS
{
    public class Heap
    {
        private int[] _heap;
        private const int SIZE = 100;
        private int _heapSize = 0;
        private int[] _minHeapArr;
        private int[] _maxHeapArr;

        public int Size
        {
            get
            {
                return _heapSize;
            }
        }
        public Heap(int[] arr)
        {
            _minHeapArr =(int[]) arr.Clone();
            _maxHeapArr =(int[]) arr.Clone();
        }
        public Heap()
        {
            _heap = new int[SIZE];
        }
        public void Push(int val)
        {
            //Max Heap
            if(_heapSize > SIZE)
            {
                throw new OverflowException("");
            }
            _heap[_heapSize] = val;
            int curSize = _heapSize;
            
            while (curSize>0 && _heap[(curSize-1)/2] < _heap[curSize]) 
            {

                int temp = _heap[(curSize - 1) / 2];
                _heap[(curSize - 1) / 2] = _heap[curSize];
                _heap[curSize] = temp;

                curSize = (curSize - 1) / 2;
            }

            _heapSize++;

        }

        public int Pop()
        {
            //Max Heap

            if (_heapSize < 0) throw new Exception("underflow");

            int curIndex = 0;
            
            int val = _heap[0];
            _heap[0] = _heap[_heapSize-1];
            
            _heapSize -= 1;

            //MaxHeapify
            while(2*curIndex+1 < _heapSize)
            {
                int child;

                //when only left child exists
                if(2*curIndex+2 == _heapSize)
                        child = 2 * curIndex + 1;
                else
                {
                    //When both left n right child exists
                    if (_heap[2 * curIndex + 1] > _heap[2 * curIndex + 2])
                        child = 2 * curIndex + 1;
                    else
                        child = 2 * curIndex + 2;
                }

                if (_heap[curIndex] < _heap[child])
                {
                    int temp = _heap[curIndex];
                    _heap[curIndex] = _heap[child];
                    _heap[child] = temp;

                    curIndex = child;
                }
                else // Max Heapify doen as cur index node is having higher value than left and right childs
                    break;

            }

            return val;
        }
        private void BuildHeaps()
        {             
            var len = _maxHeapArr.Length;
            for (int i= len / 2-1; i>=0; i--)            {

                MaxHeapify(_maxHeapArr, i, len);
                MinHeapify(_minHeapArr, i, len);
            }

            

        }
        public void HeapSort()
        {
            BuildHeaps();

            var len = _maxHeapArr.Length;
            for(int i=len-1; i>0; i--)
            {
                int temp = _maxHeapArr[0];
                _maxHeapArr[0] = _maxHeapArr[i];
                _maxHeapArr[i] = temp;

                temp = _minHeapArr[0];
                _minHeapArr[0] = _minHeapArr[i];
                _minHeapArr[i] = temp;

                //MaxHeapify(_maxHeapArr, 0, i);
                MinHeapify(_minHeapArr, 0, i);
            }


        }
        private void MinHeapify(int[] arr, int curIndex, int len)
        {
            int smallValueIndex = curIndex;

            int leftIndex = 2 * curIndex + 1;
            int rightIndex = 2 * curIndex + 2;

            if (leftIndex < len && arr[leftIndex] < arr[smallValueIndex])
                smallValueIndex = leftIndex;

            if (rightIndex < len && arr[rightIndex] < arr[smallValueIndex])
                smallValueIndex = rightIndex;

            if(smallValueIndex != curIndex)
            {
                int temp = arr[curIndex];
                arr[curIndex] = arr[smallValueIndex];
                arr[smallValueIndex] = temp;

                MinHeapify(arr, smallValueIndex, len);
            }

        }

        private void MaxHeapify(int[] arr, int curIndex, int arrLen)
        {
            int largestValueIndex = curIndex;
            int leftIndex = 2 * curIndex + 1;
            int rightIndex = 2 * curIndex + 2;

            if (leftIndex < arrLen && arr[leftIndex] > arr[curIndex])
                largestValueIndex = leftIndex;

            if (rightIndex < arrLen && arr[rightIndex] > arr[largestValueIndex])
                largestValueIndex = rightIndex;

            if(largestValueIndex != curIndex)
            {
                int temp = arr[curIndex];
                arr[curIndex] = arr[largestValueIndex];
                arr[largestValueIndex] = temp;

                MaxHeapify(arr, largestValueIndex, arrLen);
            }
        }
    }

}

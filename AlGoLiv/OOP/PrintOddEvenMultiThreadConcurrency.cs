using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlGoLiv.OOP
{
    //https://www.youtube.com/watch?v=eRNTx8k5cmA&t=161s
    //https://github.com/anomaly2104/low-level-design-even-odd-java-threading
    public class PrintOddEvenMultiThreadConcurrency
    {
        readonly int _step;
        int _currentVal;
        readonly State _state;
        readonly PrinterType _currentPrinterType;
        readonly PrinterType _nxtPrinterType;
        readonly int _maxVal;
        object monitor = new object();
        public PrintOddEvenMultiThreadConcurrency(int step, int startVal, State state, PrinterType curPrinterType, PrinterType nxtPrinterType, int maxVal)
        {
            _state = state;
            _currentVal = startVal;
            _step = step;
            _currentPrinterType = curPrinterType;
            _nxtPrinterType = nxtPrinterType;
            _maxVal = maxVal;
        }
        public void Run()
        {
            while (_currentVal <= _maxVal) {
                lock (monitor)
                {
                    while (this._currentPrinterType != _state.NextToPrint)
                        Monitor.Wait(monitor, 100);

                    Console.WriteLine(_currentPrinterType.ToString() + "-" + _currentVal);
                    _currentVal += _step;
                    _state.NextToPrint = _nxtPrinterType;
                    Monitor.Pulse(monitor);

                }
            }

        }

    }

    public enum PrinterType
    {
        ODD,
        EVEN
    }
    public class State
    {
        private PrinterType _nextToPrint;

        public State(PrinterType nextToPrint)
        {
            _nextToPrint = nextToPrint;
        }
        public PrinterType NextToPrint
        {
            get
            {
                return _nextToPrint;
            }
            set{
               
                _nextToPrint = value;
            }
        }
    }
}

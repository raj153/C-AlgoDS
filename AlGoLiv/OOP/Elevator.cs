using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.OOP
{
    public class CallingUnit
    {
        DispatcherUnit dispatcherUnit;

        public CallingUnit(DispatcherUnit dispatcherUnit)
        {
            this.dispatcherUnit = dispatcherUnit;
        }

        public void moveElevator(int currentFloor, int destinationFloor)
        {
            dispatcherUnit.getNextFloorForElevator(currentFloor, destinationFloor);
        }


    }
    public class ElevatorCar
    {
        bool isMoving;
        bool isGoingUp;

        int currentFloor;
        CallingUnit callingUnit;

        public ElevatorCar(bool isMoving, bool isGoingUp, int currentFloor, CallingUnit callingUnit)
        {
            this.isMoving = isMoving;
            this.isGoingUp = isGoingUp;
            this.currentFloor = currentFloor;
            this.callingUnit = callingUnit;
        }

        void goToFloor(int destinationFloor)
        {
            callingUnit.moveElevator(currentFloor, destinationFloor);
            currentFloor = destinationFloor;
        }

    }
    public class DispatcherUnit
    {

        public void getNextFloorForElevator(int currentFloor, int destinationFloor)
        {

        }
    }
    public class FloorCallingUnit
    {
        int currentFloor;
        CallingUnit callingUnit;

        public FloorCallingUnit(int currentFloor, CallingUnit callingUnit)
        {
            this.currentFloor = currentFloor;
            this.callingUnit = callingUnit;
        }

        public void callElevator(int destinationFloor)
        {
            callingUnit.moveElevator(currentFloor, destinationFloor);
        }
    }

}

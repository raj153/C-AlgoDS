using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.OOP1
{
	public class Level
	{
		private int floor;
		private ParkingSpot[] spots;
		private int _availableSpots = 0; // number of free spots
		private static int SPOTS_PER_ROW = 10;

		public Level(int flr, int numberSpots)
		{
			floor = flr;
			spots = new ParkingSpot[numberSpots];
			int largeSpots = numberSpots / 4;
			int bikeSpots = numberSpots / 4;
			int compactSpots = numberSpots - largeSpots - bikeSpots;
			for (int i = 0; i < numberSpots; i++)
			{
				VehicleSize sz = VehicleSize.Motorcycle;
				if (i < largeSpots)
				{
					sz = VehicleSize.Large;
				}
				else if (i < largeSpots + compactSpots)
				{
					sz = VehicleSize.Compact;
				}
				int row = i / SPOTS_PER_ROW;
				spots[i] = new ParkingSpot(this, row, i, sz);
			}
			_availableSpots = numberSpots;
		}

		public int availableSpots()
		{
			return _availableSpots;
		}

		/* Try to find a place to park this vehicle. Return false if failed. */
		public bool parkVehicle(Vehicle vehicle)
		{
			if (availableSpots() < vehicle.getSpotsNeeded())
			{
				return false;
			}
			int spotNumber = findAvailableSpots(vehicle);
			if (spotNumber < 0)
			{
				return false;
			}
			return parkStartingAtSpot(spotNumber, vehicle);
		}

		/* Park a vehicle starting at the spot spotNumber, and continuing until vehicle.spotsNeeded. */
		private bool parkStartingAtSpot(int spotNumber, Vehicle vehicle)
		{
			vehicle.clearSpots();
			bool success = true;
			for (int i = spotNumber; i < spotNumber + vehicle.getSpotsNeeded(); i++)
			{
				success &= spots[i].park(vehicle);
			}
			_availableSpots -= vehicle.getSpotsNeeded();
			return success;
		}

		/* find a spot to park this vehicle. Return index of spot, or -1 on failure. */
		private int findAvailableSpots(Vehicle vehicle)
		{
			int spotsNeeded = vehicle.getSpotsNeeded();
			int lastRow = -1;
			int spotsFound = 0;
			for (int i = 0; i < spots.Length; i++)
			{
				ParkingSpot spot = spots[i];
				if (lastRow != spot.getRow())
				{
					spotsFound = 0;
					lastRow = spot.getRow();
				}
				if (spot.canFitVehicle(vehicle))
				{
					spotsFound++;
				}
				else
				{
					spotsFound = 0;
				}
				if (spotsFound == spotsNeeded)
				{
					return i - (spotsNeeded - 1);
				}
			}
			return -1;
		}

		public void print()
		{
			int lastRow = -1;
			for (int i = 0; i < spots.Length; i++)
			{
				ParkingSpot spot = spots[i];
				if (spot.getRow() != lastRow)
				{
					//System.out.print("  ");
					lastRow = spot.getRow();
				}
				spot.print();
			}
		}

		/* When a car was removed from the spot, increment availableSpots */
		public void spotFreed()
		{
			_availableSpots++;
		}
	}
	public class ParkingSpot
	{
		private Vehicle vehicle;
		private VehicleSize spotSize;
		private int row;
		private int spotNumber;
		private Level level;

		public ParkingSpot(Level lvl, int r, int n, VehicleSize sz)
		{
			level = lvl;
			row = r;
			spotNumber = n;
			spotSize = sz;
		}

		public bool isAvailable()
		{
			return vehicle == null;
		}

		/* Checks if the spot is big enough for the vehicle (and is available). This compares
		 * the SIZE only. It does not check if it has enough spots. */
		public bool canFitVehicle(Vehicle vehicle)
		{
			return isAvailable() && vehicle.canFitInSpot(this);
		}

		/* Park vehicle in this spot. */
		public bool park(Vehicle v)
		{
			if (!canFitVehicle(v))
			{
				return false;
			}
			vehicle = v;
			vehicle.parkInSpot(this);
			return true;
		}

		public int getRow()
		{
			return row;
		}

		public int getSpotNumber()
		{
			return spotNumber;
		}

		public VehicleSize getSize()
		{
			return spotSize;
		}

		/* Remove vehicle from spot, and notify level that a new spot is available */
		public void removeVehicle()
		{
			level.spotFreed();
			vehicle = null;
		}

		public void print()
		{
			if (vehicle == null)
			{
				//if (spotSize == VehicleSize.Compact)
				//{
				//	System.out.print("c");
				//}
				//else if (spotSize == VehicleSize.Large)
				//{
				//	System.out.print("l");
				//}
				//else if (spotSize == VehicleSize.Motorcycle)
				//{
				//	System.out.print("m");
				//}
			}
			else
			{
				vehicle.print();
			}
		}
	}
	public abstract class Vehicle
	{
		protected List<ParkingSpot> parkingSpots = new List<ParkingSpot>();
		protected String licensePlate;
		protected int _spotsNeeded;
		protected VehicleSize size;

		public int getSpotsNeeded()
		{
			return _spotsNeeded;
		}

		public VehicleSize getSize()
		{
			return size;
		}

		/* Park vehicle in this spot (among others, potentially) */
		public void parkInSpot(ParkingSpot spot)
		{
			parkingSpots.Add(spot);
		}

		/* Remove car from spot, and notify spot that it's gone */
		public void clearSpots()
		{
			for (int i = 0; i < parkingSpots.Count(); i++)
			{
				parkingSpots[i].removeVehicle();
			}
			parkingSpots.Clear();
		}

		public abstract bool canFitInSpot(ParkingSpot spot);
		public abstract void print();
	}
	public enum VehicleSize
	{
		Motorcycle,
		Compact,
		Large,
	}
	public class Motorcycle : Vehicle
	{
		public Motorcycle()
		{
			_spotsNeeded = 1;
			size = VehicleSize.Motorcycle;
		}

		public override bool canFitInSpot(ParkingSpot spot)
		{
			return true;
		}

		public override void print()
		{
			//System.out.print("M");
		}
	}
	public class Car : Vehicle
	{
		public Car()
		{
			_spotsNeeded = 3;
			size = VehicleSize.Motorcycle;
		}

		public override bool canFitInSpot(ParkingSpot spot)
		{
			return true;
		}

		public override void print()
		{
			//System.out.print("M");
		}
	}

	public class Bus : Vehicle
	{
		public Bus()
		{
			_spotsNeeded = 5;
			size = VehicleSize.Motorcycle;
		}

		public override bool canFitInSpot(ParkingSpot spot)
		{
			return true;
		}

		public override void print()
		{
			//System.out.print("M");
		}



	}
	public class ParkingLot
	{
		private Level[] levels;
		private  int NUM_LEVELS = 5;

		public ParkingLot()
		{
			levels = new Level[NUM_LEVELS];
			for (int i = 0; i < NUM_LEVELS; i++)
			{
				levels[i] = new Level(i, 30);
			}
		}

		/* Park the vehicle in a spot (or multiple spots). Return false if failed. */
		public bool parkVehicle(Vehicle vehicle)
		{
			for (int i = 0; i < levels.Length; i++)
			{
				if (levels[i].parkVehicle(vehicle))
				{
					return true;
				}
			}
			return false;
		}

		public void print()
		{
			for (int i = 0; i < levels.Length; i++)
			{
				//System.out.print("Level" + i + ": ");
				levels[i].print();
				//System.out.println("");
			}
			//System.out.println("");
		}
	}

}
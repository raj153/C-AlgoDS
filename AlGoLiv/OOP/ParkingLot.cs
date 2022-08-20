using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.OOP
{
    public abstract class ParkingSpot
    {

    } 
    public class HandicappedParkingSpot : ParkingSpot
    {

    }
    public class LargeParkingSpot : ParkingSpot
    {

    }
    public class MedicumParkingSpot : ParkingSpot
    {

    }
    public interface ParkingSpotAssignmentStrategy
    {
         ParkingSpot GetParkingSpot();
         void ReleaseParkingSpot();
    }
    public class ParkingSpotNearEntranceStrategy : ParkingSpotAssignmentStrategy
    {
        public ParkingSpot GetParkingSpot()
        {
            throw new NotImplementedException();
        }

        public void ReleaseParkingSpot()
        {
            throw new NotImplementedException();
        }
    }
    public class MotorCycleParkingSpot : ParkingSpot
    {

    }
    public class ParkingLot
    {
        private static int MAX_CAPACITY = 100000;
        private int capacity;
        private Dictionary<int, Slot> slots;

        public int getCapacity()
        {
            return capacity;
        }

        public ParkingLot( int capacity)
        {
            if (capacity > MAX_CAPACITY || capacity <= 0)
            {
                throw new ParkingLotException("Invalid capacity given for parking lot.");
            }
            this.capacity = capacity;
            this.slots = new Dictionary<int, Slot>();
        }

        public Dictionary<int, Slot> getSlots()
        {
            return slots;
        }

        /**
         * Helper method to get a {@link Slot} object for a given slot number. If slot does not exists,
         * then new slot will be created before giving it back.
         *
         * @param slotNumber Slot number.
         * @return Slot.
         */
        private Slot getSlot( int slotNumber)
        {
            if (slotNumber > getCapacity() || slotNumber <= 0)
            {
                throw new InvalidSlotException();
            }
             Dictionary<int, Slot> allSlots = getSlots();
            if (!allSlots.ContainsKey(slotNumber))
            {
                allSlots[slotNumber]= new Slot(slotNumber);
            }
            return allSlots[slotNumber];
        }

        /**
         * Parks a car into a given slot number.
         *
         * @param car Car to be parked.
         * @param slotNumber Slot number in which it has to be parked.
         * @return {@link Slot} if the parking succeeds. If the slot is already occupied then {@link
         *     SlotAlreadyOccupiedException} is thrown.
         */
        public Slot park( Car car,  int slotNumber)
        {
             Slot slot = getSlot(slotNumber);
            if (!slot.isSlotFree())
            {
                throw new SlotAlreadyOccupiedException();
            }
            slot.assignCar(car);
            return slot;
        }

        /**
         * Makes the slot free from the current parked car.
         *
         * @param slotNumber Slot number to be freed.
         * @return Freed slot.
         */
        public Slot makeSlotFree( int slotNumber)
        {
             Slot slot = getSlot(slotNumber);
            slot.unassignCar();
            return slot;
        }
    }
    public class Slot
    {
        private Car parkedCar;
        private int slotNumber;

        public Slot( int slotNumber)
        {
            this.slotNumber = slotNumber;
        }

        public int getSlotNumber()
        {
            return slotNumber;
        }

        public Car getParkedCar()
        {
            return parkedCar;
        }

        public bool isSlotFree()
        {
            return parkedCar == null;
        }

        public void assignCar(Car car)
        {
            this.parkedCar = car;
        }

        public void unassignCar()
        {
            this.parkedCar = null;
        }
    }
    public class Command
    {

        private static  String SPACE = " ";
        private String commandName;
        private List<String> params1;

        public String getCommandName()
        {
            return commandName;
        }

        public List<String> getParams()
        {
            return params1;
        }

        /**
         * Constructor. It takes the input line and parses the command name and param out of it. If the
         * command or its given params are not valid, then {@link InvalidCommandException} is thrown.
         *
         * @param inputLine Given input command line.
         */
        public Command( String inputLine)
        {
            List<String> tokensList = new List<string>(); //Arrays.stream(inputLine.trim().split(SPACE))
                //.map(String::trim)
                //.filter(token-> (token.length() > 0)).collect(Collectors.toList());

            if (tokensList.Count == 0)
            {
                throw new InvalidCommandException();
            }

            commandName = tokensList[0].ToLower();
            tokensList.RemoveAt(0);
    params1 = tokensList;
        }

    }
    public class Car
    {
        private String registrationNumber;
        private String color;

        public String getRegistrationNumber()
        {
            return registrationNumber;
        }

        public String getColor()
        {
            return color;
        }

        public Car( String registrationNumber,  String color)
        {
            this.registrationNumber = registrationNumber;
            this.color = color;
        }

    }

    public interface ParkingStrategy
    {

        /**
         * Add a new slot to parking strategy. After adding, this new slot will become available for
         * future parkings.
         *
         * @param slotNumber Slot number to be added.
         */
         void addSlot(int slotNumber);

        /**
         * Removes a slot from the parking strategy. After removing, this slot will not be used for future
         * parkings.
         *
         * @param slotNumber Slot number to be removed.
         */
         void removeSlot(int slotNumber);

        /**
         * Get the next free slot as per the parking strategy.
         *
         * @return Next free slot number.
         */
         int getNextSlot();
    }
    /**
 * Parking strategy in which the natural ordering numbers are used for deciding the slot numbers.
 * For example, 1st car will be parked in slot 1, then next in slot 2, then in slot 3, and so on.
 */
    public class NaturalOrderingParkingStrategy : ParkingStrategy
    {
        SortedSet<int> slotTreeSet;

  public NaturalOrderingParkingStrategy()
    {
        this.slotTreeSet = new SortedSet<int>();
    }

    /**
     * {@inheritDoc}
     */
    
  public void addSlot(int slotNumber)
    {
        this.slotTreeSet.Add(slotNumber);
    }

    /**
     * {@inheritDoc}
     */
    
  public void removeSlot(int slotNumber)
    {
        this.slotTreeSet.Remove(slotNumber);
    }

    /**
     * {@inheritDoc}
     */
    
  public int getNextSlot()
    {
        if (slotTreeSet.Count ==0)
        {
            throw new NoFreeSlotAvailableException();
        }
        return this.slotTreeSet.First<int>();
    }
}
    public class ParkingLotService
    {
        private ParkingLot parkingLot;
        private ParkingStrategy parkingStrategy;

        /**
         * Allots a parking lot into the parking service. Throwns {@link ParkingLotException} if there is
         * already a parking lot alloted to the service previously.
         *
         * @param parkingLot Parking lot to be alloted.
         * @param parkingStrategy Strategy to be used while parking.
         */
        public void createParkingLot( ParkingLot parkingLot,  ParkingStrategy parkingStrategy)
        {
            if (this.parkingLot != null)
            {
                throw new ParkingLotException("Parking lot already exists.");
            }
            this.parkingLot = parkingLot;
            this.parkingStrategy = parkingStrategy;
            for (int i = 1; i <= parkingLot.getCapacity(); i++)
            {
                parkingStrategy.addSlot(i);
            }
        }

        /**
         * Parks a {@link Car} into the parking lot. {@link ParkingStrategy} is used to decide the slot
         * number and then the car is parked into the {@link ParkingLot} into that slot number.
         *
         * @param car Car to be parked.
         * @return Slot number in which the car is parked.
         */
        public int park( Car car)
        {
            validateParkingLotExists();
             int nextFreeSlot = parkingStrategy.getNextSlot();
            parkingLot.park(car, nextFreeSlot);
            parkingStrategy.removeSlot(nextFreeSlot);
            return nextFreeSlot;
        }

        /**
         * Unparks a car from a slot. Freed slot number is given back to the parking strategy so that it
         * becomes available for future parkings.
         *
         * @param slotNumber Slot number to be freed.
         */
        public void makeSlotFree( int slotNumber)
        {
            validateParkingLotExists();
            parkingLot.makeSlotFree(slotNumber);
            parkingStrategy.addSlot(slotNumber);
        }

        /**
         * Gets the list of all the slots which are occupied.
         */
        public List<Slot> getOccupiedSlots()
        {
            validateParkingLotExists();
            List<Slot> occupiedSlotsList = new List<Slot>();
             Dictionary<int, Slot> allSlots = parkingLot.getSlots();

            for (int i = 1; i <= parkingLot.getCapacity(); i++)
            {
                if (allSlots.ContainsKey(i))
                {
                     Slot slot = allSlots[i];
                    if (!slot.isSlotFree())
                    {
                        occupiedSlotsList.Add(slot);
                    }
                }
            }

            return occupiedSlotsList;
        }

        /**
         * Helper method to validate whether the parking lot exists or not. This is used to validate the
         * existence of parking lot before doing any operation on it.
         */
        private void validateParkingLotExists()
        {
            if (parkingLot == null)
            {
                throw new ParkingLotException("Parking lot does not exists to park.");
            }
        }

        /**
         * Gets all the slots in which a car with given color is parked.
         *
         * @param color Color to be searched.
         * @return All matching slots.
         */
        public List<Slot> getSlotsForColor( String color)
        {
             List<Slot> occupiedSlots = getOccupiedSlots();
            return occupiedSlots.ToList();
        }


    }
    /**
 * Factory to get correct {@link CommandExecutor} from a given command.
 */
    public class CommandExecutorFactory
    {
        
        private Dictionary<String, CommandExecutor> commands = new Dictionary<string, CommandExecutor>();

        public CommandExecutorFactory( ParkingLotService parkingLotService)
        {
             OutputPrinter outputPrinter = new OutputPrinter();
            commands[
                CreateParkingLotCommandExecutor.COMMAND_NAME]=
                new CreateParkingLotCommandExecutor(parkingLotService, outputPrinter);
            commands.Add(
                ParkCommandExecutor.COMMAND_NAME,
                new ParkCommandExecutor(parkingLotService, outputPrinter));
            commands.Add(
                LeaveCommandExecutor.COMMAND_NAME,
                new LeaveCommandExecutor(parkingLotService, outputPrinter));
            commands.Add(
                StatusCommandExecutor.COMMAND_NAME,
                new StatusCommandExecutor(parkingLotService, outputPrinter));
            commands.Add(
                ColorToRegNumberCommandExecutor.COMMAND_NAME,
                new ColorToRegNumberCommandExecutor(parkingLotService, outputPrinter));
            commands.Add(
                ColorToSlotNumberCommandExecutor.COMMAND_NAME,
                new ColorToSlotNumberCommandExecutor(parkingLotService, outputPrinter));
            commands.Add(
                SlotForRegNumberCommandExecutor.COMMAND_NAME,
                new SlotForRegNumberCommandExecutor(parkingLotService, outputPrinter));
            commands.Add(
                ExitCommandExecutor.COMMAND_NAME,
                new ExitCommandExecutor(parkingLotService, outputPrinter));
        }

        
        /**
         * Gets {@link CommandExecutor} for a particular command. It basically uses name of command to
         * fetch its corresponding executor.
         *
         * @param command Command for which executor has to be fetched.
         * @return Command executor.
         */
        public CommandExecutor getCommandExecutor( Command command)
        {
             CommandExecutor commandExecutor = commands[command.getCommandName()];
            if (commandExecutor == null)
            {
                throw new InvalidCommandException();
            }
            return commandExecutor;
        }
    }

    internal class ExitCommandExecutor : CommandExecutor
    {
        public ExitCommandExecutor(ParkingLotService parkingLotService, OutputPrinter outputPrinter) : base(parkingLotService, outputPrinter)
        {
        }

        public static string COMMAND_NAME { get; internal set; }
        public override void execute(Command command)
        {
            throw new NotImplementedException();
        }

        public override bool validate(Command command)
        {
            throw new NotImplementedException();
        }
    }

    internal class SlotForRegNumberCommandExecutor : CommandExecutor
    {
        public SlotForRegNumberCommandExecutor(ParkingLotService parkingLotService, OutputPrinter outputPrinter) : base(parkingLotService, outputPrinter)
        {
        }

        public static string COMMAND_NAME { get; internal set; }
        public override void execute(Command command)
        {
            throw new NotImplementedException();
        }

        public override bool validate(Command command)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorToSlotNumberCommandExecutor : CommandExecutor
    {
        public ColorToSlotNumberCommandExecutor(ParkingLotService parkingLotService, OutputPrinter outputPrinter) : base(parkingLotService, outputPrinter)
        {
        }


        public static string COMMAND_NAME { get; internal set; }
        public override void execute(Command command)
        {
            throw new NotImplementedException();
        }

        public override bool validate(Command command)
        {
            throw new NotImplementedException();
        }
    }

    public class ColorToRegNumberCommandExecutor : CommandExecutor
    {
        public ColorToRegNumberCommandExecutor(ParkingLotService parkingLotService, OutputPrinter outputPrinter) : base(parkingLotService, outputPrinter)
        {
        }


        public static string COMMAND_NAME { get; internal set; }
        public override void execute(Command command)
        {
            throw new NotImplementedException();
        }

        public override bool validate(Command command)
        {
            throw new NotImplementedException();
        }
    }

    public class StatusCommandExecutor : CommandExecutor
    {
        public StatusCommandExecutor(ParkingLotService parkingLotService, OutputPrinter outputPrinter) : base(parkingLotService, outputPrinter)
        {
        }

        public static string COMMAND_NAME { get; internal set; }
        public override void execute(Command command)
        {
            throw new NotImplementedException();
        }

        public override bool validate(Command command)
        {
            throw new NotImplementedException();
        }
    }

    internal class LeaveCommandExecutor : CommandExecutor
    {
        public LeaveCommandExecutor(ParkingLotService parkingLotService, OutputPrinter outputPrinter) : base(parkingLotService, outputPrinter)
        {
        }


        public static string COMMAND_NAME { get; internal set; }
        public override void execute(Command command)
        {
            throw new NotImplementedException();
        }

        public override bool validate(Command command)
        {
            throw new NotImplementedException();
        }
    }

    internal class ParkCommandExecutor : CommandExecutor
    {
        public ParkCommandExecutor(ParkingLotService parkingLotService, OutputPrinter outputPrinter) : base(parkingLotService, outputPrinter)
        {
        }


        public static string COMMAND_NAME { get; internal set; }
        public override void execute(Command command)
        {
            throw new NotImplementedException();
        }

        public override bool validate(Command command)
        {
            throw new NotImplementedException();
        }
    }

    internal class CreateParkingLotCommandExecutor : CommandExecutor
    {
        public CreateParkingLotCommandExecutor(ParkingLotService parkingLotService, OutputPrinter outputPrinter) : base(parkingLotService, outputPrinter)
        {
        }


        public static string COMMAND_NAME { get; internal set; }
        public override void execute(Command command)
        {
            throw new NotImplementedException();
        }

        public override bool validate(Command command)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class CommandExecutor
    {
        protected ParkingLotService parkingLotService;
        protected OutputPrinter outputPrinter;

        public CommandExecutor( ParkingLotService parkingLotService,
            OutputPrinter outputPrinter)
        {
            this.parkingLotService = parkingLotService;
            this.outputPrinter = outputPrinter;
        }

        /**
         * Validates that whether a command is valid to be executed or not.
         *
         * @param command Command to be validated.
         * @return bool indicating whether command is valid or not.
         */
        public abstract bool validate(Command command);

        /**
         * Executes the command.
         *
         * @param command Command to be executed.
         */
        public abstract void execute(Command command);
    }

    public class OutputPrinter
    {
    }

    [Serializable]
    internal class NoFreeSlotAvailableException : Exception
    {
        public NoFreeSlotAvailableException()
        {
        }

        public NoFreeSlotAvailableException(string message) : base(message)
        {
        }

        public NoFreeSlotAvailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoFreeSlotAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class InvalidCommandException : Exception
    {
        public InvalidCommandException()
        {
        }

        public InvalidCommandException(string message) : base(message)
        {
        }

        public InvalidCommandException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class SlotAlreadyOccupiedException : Exception
    {
        public SlotAlreadyOccupiedException()
        {
        }

        public SlotAlreadyOccupiedException(string message) : base(message)
        {
        }

        public SlotAlreadyOccupiedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SlotAlreadyOccupiedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class InvalidSlotException : Exception
    {
        public InvalidSlotException()
        {
        }

        public InvalidSlotException(string message) : base(message)
        {
        }

        public InvalidSlotException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidSlotException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class ParkingLotException : Exception
    {
        public ParkingLotException()
        {
        }

        public ParkingLotException(string message) : base(message)
        {
        }

        public ParkingLotException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParkingLotException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

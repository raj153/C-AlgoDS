using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.OOP
{
    public class Cab
    {
        public String id;
        public String driverName;

        public Trip currentTrip;
        public Location currentLocation;
        public Boolean isAvailable;

  public Cab(String id, String driverName)
        {
            this.id = id;
            this.driverName = driverName;
            this.isAvailable = true;
        }

        
        public String toString()
        {
            return "Cab{" +
                "id='" + id + '\'' +
                ", driverName='" + driverName + '\'' +
                ", currentLocation=" + currentLocation +
                ", isAvailable=" + isAvailable +
                '}';
        }
    }

    public class Location
    {
        private Double x;
        private Double y;

        public Location(Double x, Double y)
        {
            this.x = x;
            this.y = y;
        }
        public Double distance(Location location2)
        {
            return Math.Sqrt(Math.Pow(this.x - location2.x, 2) + Math.Pow(this.y - location2.y, 2));
        }


    }

    public class PostResponse
    {

        public const String OK_RESPONSE = "ok";
        public const String ERROR_RESPONSE = "error";

        public String status;
        public String message;

        public PostResponse(string status, string message)
        {

        }
        public static PostResponse ok()
        {
            return new PostResponse(OK_RESPONSE, null);
        }

        public static PostResponse error( String message)
        {
            return new PostResponse(ERROR_RESPONSE, message);
        }
    }

    public class Rider
    {
        public String id;
        public String name;

        public Rider(string id, string name) { }
    }

    public class Trip
    {
        private Rider rider;
        private Cab cab;
        private TripStatus status;
        private Double price;
        private Location fromPoint;
        private Location toPoint;

        public Trip(
             Rider rider,
             Cab cab,
             Double price,
             Location fromPoint,
             Location toPoint)
        {
            this.rider = rider;
            this.cab = cab;
            this.price = price;
            this.fromPoint = fromPoint;
            this.toPoint = toPoint;
            this.status = TripStatus.IN_PROGRESS;
        }

        public void endTrip()
        {
            this.status = TripStatus.FINISHED;
        }
    }

    enum TripStatus
    {
        IN_PROGRESS,
        FINISHED
    }
    public class RidersController
    {
        private RidersManagerRepository ridersManager;
        private TripsManagerRepository tripsManager;

        public RidersController(RidersManagerRepository ridersManager, TripsManagerRepository tripsManager)
        {
            this.ridersManager = ridersManager;
            this.tripsManager = tripsManager;
        }

       // @RequestMapping(value = "/register/rider", method = RequestMethod.POST)
        public ResponseEntity registerRider( String riderId,  String riderName)
        {
            ridersManager.createRider(new Rider(riderId, riderName));
            return ResponseEntity.ok("");
        }

        //@RequestMapping(value = "/book", method = RequestMethod.POST)
  public ResponseEntity book(
       String riderId,
       Double sourceX,
       Double sourceY,
       Double destX,
       Double destY)
        {

            tripsManager.createTrip(
                ridersManager.getRider(riderId),
                new Location(sourceX, sourceY),
                new Location(destX, destY));

            return ResponseEntity.ok("");
        }

        //@RequestMapping(value = "/book", method = RequestMethod.GET)
  public ResponseEntity fetchHistory( String riderId)
        {
            List<Trip> trips = tripsManager.tripHistory(ridersManager.getRider(riderId));
            return ResponseEntity.ok(trips);
        }
    }

    public class ResponseEntity
    {
        internal static ResponseEntity ok(string v)
        {
            throw new NotImplementedException();
        }

        internal static ResponseEntity ok(List<Trip> trips)
        {
            throw new NotImplementedException();
        }
    }

    public class CabsController
    {
        private CabsManagerRepositiory cabsManager;
        private TripsManagerRepository tripsManager;

        public CabsController(CabsManagerRepositiory cabsManager, TripsManagerRepository tripsManager)
        {
            this.cabsManager = cabsManager;
            this.tripsManager = tripsManager;
        }

        //@RequestMapping(value = "/register/cab", method = RequestMethod.POST)
  public ResponseEntity regiserCab( String cabId,  String driverName)
        {
            cabsManager.createCab(new Cab(cabId, driverName));
            return ResponseEntity.ok("");
        }

        //@RequestMapping(value = "/update/cab/location", method = RequestMethod.POST)
  public ResponseEntity updateCabLocation(
       String cabId,  Double newX,  Double newY)
        {

            cabsManager.updateCabLocation(cabId, new Location(newX, newY));
            return ResponseEntity.ok("");
        }

        //@RequestMapping(value = "/update/cab/availability", method = RequestMethod.POST)
  public ResponseEntity updateCabAvailability( String cabId,  Boolean newAvailability)
        {
            cabsManager.updateCabAvailability(cabId, newAvailability);
            return ResponseEntity.ok("");
        }

        //@RequestMapping(value = "/update/cab/end/trip", method = RequestMethod.POST)
  public ResponseEntity endTrip( String cabId)
        {
            tripsManager.endTrip(cabsManager.getCab(cabId));
            return ResponseEntity.ok("");
        }
    }

    public class TripsManagerRepository
    {

        public const Double MAX_ALLOWED_TRIP_MATCHING_DISTANCE = 10.0;
        private Dictionary<String, List<Trip>> trips = new Dictionary<string, List<Trip>>();

        private CabsManagerRepositiory cabsManager;
        private RidersManagerRepository ridersManager;
        private CabMatchingStrategy cabMatchingStrategy;
        private PricingStrategy pricingStrategy;

        public TripsManagerRepository(
            CabsManagerRepositiory cabsManager,
            RidersManagerRepository ridersManager,
            CabMatchingStrategy cabMatchingStrategy,
            PricingStrategy pricingStrategy)
        {
            this.cabsManager = cabsManager;
            this.ridersManager = ridersManager;
            this.cabMatchingStrategy = cabMatchingStrategy;
            this.pricingStrategy = pricingStrategy;
        }

        public void createTrip(
             Rider rider,
             Location fromPoint,
             Location toPoint)
        {
             List<Cab> closeByCabs =
                cabsManager.getCabs(fromPoint, MAX_ALLOWED_TRIP_MATCHING_DISTANCE);
            List<Cab> closeByAvailableCabs = closeByCabs;
                //.stream()
                  //  .filter(cab->cab.getCurrentTrip() == null)
                    //.collect(Collectors.toList());

             Cab selectedCab =
                cabMatchingStrategy.matchCabToRider(rider, closeByAvailableCabs, fromPoint, toPoint);
            if (selectedCab == null)
            {
                throw new NoCabsAvailableException();
            }

             Double price = pricingStrategy.findPrice(fromPoint, toPoint);
             Trip newTrip = new Trip(rider, selectedCab, price, fromPoint, toPoint);
            if (!trips.ContainsKey(rider.id))
            {
                trips.Add(rider.id, new List<Trip>());
            }
            trips[rider.id].Add(newTrip);
            selectedCab.currentTrip = newTrip;
        }

        public List<Trip> tripHistory( Rider rider)
        {
            return trips[rider.id];
        }

        public void endTrip( Cab cab)
        {
            if (cab.currentTrip == null)
            {
                throw new TripNotFoundException();
            }

            cab.currentTrip.endTrip();
            cab.currentTrip = null; ;
        }
    }

    [Serializable]
    internal class TripNotFoundException : Exception
    {
        public TripNotFoundException()
        {
        }

        public TripNotFoundException(string message) : base(message)
        {
        }

        public TripNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TripNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class NoCabsAvailableException : Exception
    {
        public NoCabsAvailableException()
        {
        }

        public NoCabsAvailableException(string message) : base(message)
        {
        }

        public NoCabsAvailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoCabsAvailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class RidersManagerRepository
    {
        Dictionary<String, Rider> riders = new Dictionary<string, Rider>();

        public void createRider( Rider newRider)
        {
            if (riders.ContainsKey(newRider.id))
            {
                throw new RiderAlreadyExistsException();
            }

            riders.Add(newRider.id, newRider);
        }

        public Rider getRider( String riderId)
        {
            if (!riders.ContainsKey(riderId))
            {
                throw new RiderNotFoundException();
            }
            return riders[riderId];
        }
    }

    [Serializable]
    internal class RiderAlreadyExistsException : Exception
    {
        public RiderAlreadyExistsException()
        {
        }

        public RiderAlreadyExistsException(string message) : base(message)
        {
        }

        public RiderAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RiderAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class RiderNotFoundException : Exception
    {
        public RiderNotFoundException()
        {
        }

        public RiderNotFoundException(string message) : base(message)
        {
        }

        public RiderNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RiderNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class CabsManagerRepositiory
    {

        Dictionary<String, Cab> cabs = new Dictionary<string, Cab>();

        public void createCab( Cab newCab)
        {
            if (cabs.ContainsKey(newCab.id))
            {
                throw new CabAlreadyExistsException();
            }

            cabs.Add(newCab.id, newCab);
        }

        public Cab getCab( String cabId)
        {
            if (!cabs.ContainsKey(cabId))
            {
                throw new CabNotFoundException();
            }
            return cabs[cabId];
        }

        public void updateCabLocation( String cabId,  Location newLocation)
        {
            if (!cabs.ContainsKey(cabId))
            {
                throw new CabNotFoundException();
            }
            cabs[cabId].currentLocation = newLocation;
        }

        public void updateCabAvailability(
             String cabId,  Boolean newAvailability)
        {
            if (!cabs.ContainsKey(cabId))
            {
                throw new CabNotFoundException();
            }
            cabs[cabId].isAvailable = newAvailability;
        }

        public List<Cab> getCabs( Location fromPoint,  Double distance)
        {
            List<Cab> result = new List<Cab>();
            foreach (Cab cab in cabs.Values)
            {
                // TODO: Use epsilon comparison because of double
                if (cab.isAvailable && cab.currentLocation.distance(fromPoint) <= distance)
                {
                    result.Add(cab);
                }
            }
            return result;
        }
    }

    [Serializable]
    internal class CabNotFoundException : Exception
    {
        public CabNotFoundException()
        {
        }

        public CabNotFoundException(string message) : base(message)
        {
        }

        public CabNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CabNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class CabAlreadyExistsException : Exception
    {
        public CabAlreadyExistsException()
        {
        }

        public CabAlreadyExistsException(string message) : base(message)
        {
        }

        public CabAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CabAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public interface CabMatchingStrategy
    {

        Cab matchCabToRider(Rider rider, List<Cab> candidateCabs, Location fromPoint, Location toPoint);
    }
    public class DefaultCabMatchingStrategy : CabMatchingStrategy
    {

        
        public Cab matchCabToRider(
       Rider rider,
       List<Cab> candidateCabs,
       Location fromPoint,
       Location toPoint)
    {
        if (candidateCabs.Count ==0)
        {
            return null;
        }
        return candidateCabs[0];
    }

    }

    public interface PricingStrategy
    {
        Double findPrice(Location fromPoint, Location toPoint);
    }

    public class DefaultPricingStrategy : PricingStrategy
    {

    public const Double PER_KM_RATE = 10.0;

    
    public Double findPrice(Location fromPoint, Location toPoint)
    {
        return fromPoint.distance(toPoint) * PER_KM_RATE;
    }
}


}

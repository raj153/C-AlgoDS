using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.OOP
{
    public class Booking
    {

        public String id;

        public Show show;

        public List<Seat> seatsBooked;

        public String user;

        public BookingStatus bookingStatus;

        public Booking(String id, Show show, String user,
                   List<Seat> seatsBooked)
        {
            this.id = id;
            this.show = show;
            this.seatsBooked = seatsBooked;
            this.user = user;
            this.bookingStatus = BookingStatus.Created;

        }



        public bool isConfirmed()
        {
            return (this.bookingStatus == BookingStatus.Confirmed);
        }

        public void confirmBooking()
        {
            if ((this.bookingStatus != BookingStatus.Created))
            {
                throw new InvalidStateException();
            }

            this.bookingStatus = BookingStatus.Confirmed;
        }

        public void expireBooking()
        {
            if ((this.bookingStatus != BookingStatus.Created))
            {
                throw new InvalidStateException();
            }

            this.bookingStatus = BookingStatus.Expired;
        }
    }

    [Serializable]
    internal class InvalidStateException : Exception
    {
        public InvalidStateException()
        {
        }

        public InvalidStateException(string message) : base(message)
        {
        }

        public InvalidStateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidStateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public enum BookingStatus
    {
        Created,
        Confirmed,
        Expired
    }
    public class Movie
    {

        public String id;
        public String name;

        public Movie(string id, string name) { }
    }
    public class Theatre
    {

        public  String id;
        public String name;
        public List<Screen> screens;
        //Other theatre metadata.

        public Theatre(String id,   String name)
        {
            this.id = id;
            this.name = name;
            this.screens = new List<Screen>();
        }

        public void addScreen(   Screen screen)
        {
            screens.Add(screen);
        }
    }

    public class Screen
    {

        public string id;
        public String name;
        public Theatre theatre;
        //Other screen metadata.

        public List<Seat> seats;

        public Screen(String id, String name, Theatre theatre)
        {
            this.id = id;
            this.name = name;
            this.theatre = theatre;
            this.seats = new List<Seat>();
        }

        public void addSeat(Seat seat)
        {
            this.seats.Add(seat);
        }

    }
    public class Seat
    {

        public String id;
        public  int rowNo;
        public  int seatNo;

        public Seat(string id, int rowNo, int seatNo) { }
    }

    public class SeatLock
    {
        private Seat seat;
        private Show show;
        private int timeoutInSeconds;
        private DateTime lockTime;
        public String lockedBy;

        public SeatLock(Seat seat, Show show, int timeoutInSeconds, DateTime lockTime, string lockedBy)
        {

        }
        public bool isLockExpired()
        {
            var lockInstant = lockTime.AddSeconds(timeoutInSeconds);
            var currentInstant = new DateTime();
            return lockInstant < currentInstant;
        }
    }
    public class Show
    {

        public String id;
        public Movie movie;
        public  Screen screen;
        public DateTime startTime;
        public int durationInSeconds;

        public Show(String id, Movie movie, Screen screen, DateTime startTime, int durationInSeconds)
        {

        }
    }

    public interface SeatLockProvider
    {

        void lockSeats(Show show, List<Seat> seat, String user);
        void unlockSeats(Show show, List<Seat> seat, String user);
        bool validateLock(Show show, Seat seat, String user);

        List<Seat> getLockedSeats(Show show);
    }
    public class InMemorySeatLockProvider : SeatLockProvider
    {

    private  int lockTimeout; // Bonus feature.
    private Dictionary<Show, Dictionary<Seat, SeatLock>> locks;

    public InMemorySeatLockProvider(  int lockTimeout)
    {
        this.locks = new Dictionary<Show, Dictionary<Seat, SeatLock>>();
        this.lockTimeout = lockTimeout;
    }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public  void lockSeats(  Show show,   List<Seat> seats,
                                         String user)
    {
        foreach (Seat seat in seats)
        {
            if (isSeatLocked(show, seat))
            {
                throw new SeatTemporaryUnavailableException();
            }
        }

        foreach (Seat seat in seats)
        {
            lockSeat(show, seat, user, lockTimeout);
        }
    }

    
    public void unlockSeats(  Show show,   List<Seat> seats,   String user)
    {
        foreach (Seat seat in seats)
        {
            if (validateLock(show, seat, user))
            {
                unlockSeat(show, seat);
            }
        }
    }

    
    public bool validateLock(  Show show,   Seat seat,   String user)
    {
        return isSeatLocked(show, seat) && locks[show][seat].lockedBy.Equals(user);
    }

    
    public List<Seat> getLockedSeats(  Show show)
    {
        if (!locks.ContainsKey(show))
        {
                return new List<Seat>();
        }
         List<Seat> lockedSeats = new List<Seat>();

        foreach (Seat seat in locks[show].Keys)
        {
            if (isSeatLocked(show, seat))
            {
                lockedSeats.Add(seat);
            }
        }
        return lockedSeats;
    }

    private void unlockSeat( Show show,  Seat seat)
    {
        if (!locks.ContainsKey(show))
        {
            return;
        }
        locks[show].Remove(seat);
    }

    private void lockSeat( Show show,  Seat seat,  String user,  int timeoutInSeconds)
    {
        if (!locks.ContainsKey(show))
        {
                locks[show] = new Dictionary<Seat, SeatLock>();
        }
        SeatLock lock1 = new SeatLock(seat, show, timeoutInSeconds, new DateTime(), user);
        locks[show][seat]= lock1 ;
    }

    private bool isSeatLocked( Show show,  Seat seat)
    {
        return locks.ContainsKey(show) && locks[show].ContainsKey(seat) && !locks[show][seat].isLockExpired();
    }

}

    [Serializable]
    internal class SeatTemporaryUnavailableException : Exception
    {
        public SeatTemporaryUnavailableException()
        {
        }

        public SeatTemporaryUnavailableException(string message) : base(message)
        {
        }

        public SeatTemporaryUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SeatTemporaryUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class BookingController
    {
        private  ShowService showService;
        private BookingService bookingService;
        private  TheatreService theatreService;

    public String createBooking(String userId, String showId,
                                List<Seat> seatsIds)
        {
             Show show = showService.getShow(showId);
            List<Seat> seats = seatsIds;//.stream().map(theatreService::getSeat).collect(Collectors.toList());
            return bookingService.createBooking(userId, show, seats).id;
        }
    }
    public class MovieController
    {
        private MovieService movieService;

        public String createMovie(String movieName)
        {
            return movieService.createMovie(movieName).id;
        }

    }
    public class PaymentsController
    {
        private  PaymentsService paymentsService;
    private  BookingService bookingService;

    public PaymentsController(PaymentsService paymentsService, BookingService bookingService)
        {
            this.paymentsService = paymentsService;
            this.bookingService = bookingService;
        }

        public void paymentFailed(  String bookingId,   String user)
        {
            paymentsService.processPaymentFailed(bookingService.getBooking(bookingId), user);
        }

        public void paymentSuccess(   String bookingId,   String user)
        {
            bookingService.confirmBooking(bookingService.getBooking(bookingId), user);
        }

    }
    public class ShowController
    {
        private  SeatAvailabilityService seatAvailabilityService;
    private  ShowService showService;
    private  TheatreService theatreService;
    private  MovieService movieService;

    public String createShow(  String movieId,   String screenId,   DateTime startTime,
                               int durationInSeconds)
        {
             Screen screen = theatreService.getScreen(screenId);
             Movie movie = movieService.getMovie(movieId);
            return showService.createShow(movie, screen, startTime, durationInSeconds).id;
        }

        public List<Seat> getAvailableSeats(  String showId)
        {
             Show show = showService.getShow(showId);
             List<Seat> availableSeats = seatAvailabilityService.getAvailableSeats(show);
            return availableSeats;//.stream().map(Seat::getId).collect(Collectors.toList());
        }
    }

    public class TheatreController
    {
         private TheatreService theatreService;

        public String createTheatre(  String theatreName)
        {
            return theatreService.createTheatre(theatreName).id;
        }

        public String createScreenInTheatre(  String screenName,   String theatreId)
        {
             Theatre theatre = theatreService.getTheatre(theatreId);
            return theatreService.createScreenInTheatre(screenName, theatre).id;
        }

        public String createSeatInScreen( int  rowNo,   int seatNo,   String screenId)
        {
             Screen screen = theatreService.getScreen(screenId);
            return theatreService.createSeatInScreen(rowNo, seatNo, screen).id;
        }
    }

    public class BookingService
    {

        private  Dictionary<String, Booking> showBookings;
        private  SeatLockProvider seatLockProvider;

    public BookingService(SeatLockProvider seatLockProvider)
        {
            this.seatLockProvider = seatLockProvider;
            this.showBookings = new Dictionary<string, Booking>();
        }

        public Booking getBooking(  String bookingId)
        {
            if (!showBookings.ContainsKey(bookingId))
            {
                throw new NotFoundException();
            }
            return showBookings[bookingId];
        }

        public List<Booking> getAllBookings(  Show show)
        {
            List<Booking> response = new List<Booking>();
            foreach (Booking booking in showBookings.Values)
            {
                if (booking.show.Equals(show))
                {
                    response.Add(booking);
                }
            }

            return response;
        }

        public Booking createBooking(  String userId,   Show show,
                                       List<Seat> seats)
        {
            if (isAnySeatAlreadyBooked(show, seats))
            {
                throw new SeatPermanentlyUnavailableException();
            }
            seatLockProvider.lockSeats(show, seats, userId);
             String bookingId = Guid.NewGuid().ToString();
             Booking newBooking = new Booking(bookingId, show, userId, seats);
            showBookings[bookingId]= newBooking;
            return newBooking;
            // TODO: Create timer for booking expiry
        }

        public List<Seat> getBookedSeats(  Show show)
        {
            return new List<Seat>();
            //return getAllBookings(show).stream()
            //        .filter(Booking::isConfirmed)
            //        .map(Booking::getSeatsBooked)
            //        .flatMap(Collection::stream)
            //        .collect(Collectors.toList());
        }

        public void confirmBooking(  Booking booking,   String user)
        {
            if (!booking.user.Equals(user))
            {
                throw new BadRequestException();
            }

            foreach (Seat seat in booking.seatsBooked)
            {
                if (!seatLockProvider.validateLock(booking.show, seat, user))
                {
                    throw new BadRequestException();
                }
            }
            booking.confirmBooking();
        }

        private bool isAnySeatAlreadyBooked( Show show,  List<Seat> seats)
        {
             List<Seat> bookedSeats = getBookedSeats(show);
            foreach (Seat seat in seats)
            {
                if (bookedSeats.Contains(seat))
                {
                    return true;
                }
            }
            return false;
        }
    }

    [Serializable]
    internal class SeatPermanentlyUnavailableException : Exception
    {
        public SeatPermanentlyUnavailableException()
        {
        }

        public SeatPermanentlyUnavailableException(string message) : base(message)
        {
        }

        public SeatPermanentlyUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SeatPermanentlyUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class BadRequestException : Exception
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    public class MovieService
    {

        private  Dictionary<String, Movie> movies;

        public MovieService()
        {
            this.movies = new Dictionary<string, Movie>();
        }

        public Movie getMovie(  String movieId)
        {
            if (!movies.ContainsKey(movieId))
            {
                throw new NotFoundException();
            }
            return movies[movieId];
        }

        public Movie createMovie(  String movieName)
        {
            String movieId = Guid.NewGuid().ToString();
            Movie movie = new Movie(movieId, movieName);
            movies[movieId]= movie;
            return movie;
        }

    }
    public class PaymentsService
    {

        Dictionary<Booking, int> bookingFailures;
        private  int allowedRetries;
    private  SeatLockProvider seatLockProvider;

    public PaymentsService(  int allowedRetries, SeatLockProvider seatLockProvider)
        {
            this.allowedRetries = allowedRetries;
            this.seatLockProvider = seatLockProvider;
            bookingFailures = new Dictionary<Booking, int>();
        }

        public void processPaymentFailed(  Booking booking,   String user)
        {
            if (!booking.user.Equals(user))
            {
                throw new BadRequestException();
            }
            if (!bookingFailures.ContainsKey(booking))
            {
                bookingFailures[booking]=0;
            }
             int currentFailuresCount = bookingFailures[booking];
             int newFailuresCount = currentFailuresCount + 1;
            bookingFailures[booking] =newFailuresCount;
            if (newFailuresCount > allowedRetries)
            {
                seatLockProvider.unlockSeats(booking.show, booking.seatsBooked, booking.user);
            }
        }
    }

    public class SeatAvailabilityService
    {
        private  BookingService bookingService;
    private  SeatLockProvider seatLockProvider;

    public SeatAvailabilityService(  BookingService bookingService,
                                     SeatLockProvider seatLockProvider)
        {
            this.bookingService = bookingService;
            this.seatLockProvider = seatLockProvider;
        }

        public List<Seat> getAvailableSeats(  Show show)
        {
             List<Seat> allSeats = show.screen.seats;
             List<Seat> unavailableSeats = getUnavailableSeats(show);

             List<Seat> availableSeats = new List<Seat>(allSeats);
            ///availableSeats.RemoveAll(x=>x.unavailableSeats);
            return availableSeats;
        }

        private List<Seat> getUnavailableSeats(  Show show)
        {
             List<Seat> unavailableSeats = bookingService.getBookedSeats(show);
            unavailableSeats.AddRange(seatLockProvider.getLockedSeats(show));
            return unavailableSeats;
        }

    }

    public class ShowService
    {

        private  Dictionary<String, Show> shows;

        public ShowService()
        {
            this.shows = new Dictionary<string, Show>();
        }

        public Show getShow(  String showId)
        {
            if (!shows.ContainsKey(showId))
            {
                throw new NotFoundException();
            }
            return shows[showId];
        }

        public Show createShow(  Movie movie,   Screen screen,   DateTime startTime,
                                 int durationInSeconds)
        {
            if (!checkIfShowCreationAllowed(screen, startTime, durationInSeconds))
            {
                throw new ScreenAlreadyOccupiedException();
            }
            String showId = Guid.NewGuid().ToString();
             Show show = new Show(showId, movie, screen, startTime, durationInSeconds);
            this.shows[showId]= show;
            return show;
        }

        private List<Show> getShowsForScreen( Screen screen)
        {
            List<Show> response = new List<Show>();
            foreach (Show show in shows.Values)
            {
                if (show.screen.Equals(screen))
                {
                    response.Add(show);
                }
            }
            return response;
        }

        private bool checkIfShowCreationAllowed( Screen screen,  DateTime startTime,  int durationInSeconds)
        {
            // TODO: Implement this. This method will return whether the screen is free at a particular time for
            // specific duration. This function will be helpful in checking whether the show can be scheduled in that slot
            // or not.
            return true;
        }
    }
    public class TheatreService
    {

        private  Dictionary<String, Theatre> theatres;
        private Dictionary<String, Screen> screens;
        private Dictionary<String, Seat> seats;

        public TheatreService()
        {
            this.theatres = new Dictionary<string, Theatre>();
            this.screens = new Dictionary<string, Screen>();
            this.seats = new Dictionary<string, Seat>();
        }

        public Seat getSeat(  String seatId)
        {
            if (!seats.ContainsKey(seatId))
            {
                throw new NotFoundException();
            }
            return seats[seatId];
        }

        public Theatre getTheatre(  String theatreId)
        {
            if (!theatres.ContainsKey(theatreId))
            {
                throw new NotFoundException();
            }
            return theatres[theatreId];
        }

        public Screen getScreen(  String screenId)
        {
            if (!screens.ContainsKey(screenId))
            {
                throw new NotFoundException();
            }
            return screens[screenId];
        }

        public Theatre createTheatre(  String theatreName)
        {
            String theatreId = Guid.NewGuid().ToString();
            Theatre theatre = new Theatre(theatreId, theatreName);
            theatres[theatreId] = theatre;
            return theatre;
        }

        public Screen createScreenInTheatre(  String screenName,   Theatre theatre)
        {
            Screen screen = createScreen(screenName, theatre);
            theatre.addScreen(screen);
            return screen;
        }

        public Seat createSeatInScreen(  int rowNo,   int seatNo,   Screen screen)
        {
            String seatId = Guid.NewGuid().ToString();
            Seat seat = new Seat(seatId, rowNo, seatNo);
            seats[seatId] = seat;
            screen.addSeat(seat);

            return seat;
        }

        private Screen createScreen( String screenName,  Theatre theatre)
        {
            String screenId = Guid.NewGuid().ToString();
            Screen screen = new Screen(screenId, screenName, theatre);
            screens[screenId]= screen;
            return screen;
        }

    }


    [Serializable]
    internal class ScreenAlreadyOccupiedException : Exception
    {
        public ScreenAlreadyOccupiedException()
        {
        }

        public ScreenAlreadyOccupiedException(string message) : base(message)
        {
        }

        public ScreenAlreadyOccupiedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ScreenAlreadyOccupiedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}




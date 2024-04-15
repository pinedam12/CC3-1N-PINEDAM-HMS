using System;
using System.Collections.Generic;

namespace CC3_1N_HMS
{
    public enum RoomStyle
    {
        TwinRoom,
        QueenRoom,
        KingRoom
    }

    public class HotelRoom
    {
        public int RoomNumber { get; set; }
        public RoomStyle Style { get; set; }
        public decimal BookingPrice { get; set; }
        public bool Status { get; set; }

        public HotelRoom(int roomNumber, RoomStyle style, decimal bookingPrice)
        {
            RoomNumber = roomNumber;
            Style = style;
            BookingPrice = bookingPrice;
            Status = true;
        }
    }

    public class Hotel
    {
        public string HotelName { get; set; }
        public string Location { get; set; }
        private List<HotelRoom> allRooms;

        public Hotel(string hotelName, string location, List<HotelRoom> rooms)
        {
            HotelName = hotelName;
            Location = location;
            allRooms = rooms;
        }

        public void DisplayAvailableRooms()
        {
            Console.WriteLine($"Hotel {HotelName} - Available Rooms:");
            foreach (var room in allRooms)
            {
                if (room.Status)
                {
                    Console.WriteLine($"Room {room.RoomNumber}, Style: {room.Style}, Price: {room.BookingPrice}");
                }
            }
        }

        public void DisplayBookedRooms()
        {
            Console.WriteLine($"Hotel {HotelName} - Booked Rooms:");
            foreach (var room in allRooms)
            {
                if (!room.Status)
                {
                    Console.WriteLine($"Room {room.RoomNumber}, Style: {room.Style}, Price: {room.BookingPrice}");
                }
            }
        }
    }

    public class Reservation
    {
        public int ReservationNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public HotelRoom Room { get; set; }
        public int DurationInDays { get; set; }
        public decimal Total { get; set; }

        public Reservation(DateTime startTime, DateTime endTime, HotelRoom room)
        {
            ReservationNumber = GenerateReservationNumber();
            StartTime = startTime;
            EndTime = endTime;
            Room = room;
            DurationInDays = (int)(endTime - startTime).TotalDays;
            Total = DurationInDays * room.BookingPrice;
        }

        private int GenerateReservationNumber()
        {
            Random random = new Random();
            return random.Next(100000000, 999999999);
        }

        public void DisplayDetails()
        {
            Console.WriteLine($"Reservation Number: {ReservationNumber}");
            Console.WriteLine($"Start Time: {StartTime}");
            Console.WriteLine($"End Time: {EndTime}");
            Console.WriteLine($"Duration: {DurationInDays} days");
            Console.WriteLine($"Total: {Total}");
        }
    }

    public class Guest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        private List<Reservation> reservations;

        public Guest(string name, string address, string email, int phoneNumber)
        {
            Name = name;
            Address = address;
            Email = email;
            PhoneNumber = phoneNumber;
            reservations = new List<Reservation>();
        }

        public void BookReservation(Reservation reservation)
        {
            reservations.Add(reservation);
        }

        public void DisplayReservations()
        {
            Console.WriteLine($"List of Reservations of {Name}:");
            foreach (var reservation in reservations)
            {
                reservation.DisplayDetails();
            }
        }
    }

    public class Receptionist : Guest
    {
        public Receptionist(string name, string address, string email, int phoneNumber)
            : base(name, address, email, phoneNumber)
        {
        }

        public void BookReservation(Guest guest, Reservation reservation)
        {
            guest.BookReservation(reservation);
        }
    }

    public class HotelManagementSystem
    {
        private List<Hotel> hotels;
        private List<Guest> users;
        private List<Reservation> reservations;

        public HotelManagementSystem()
        {
            hotels = new List<Hotel>();
            users = new List<Guest>();
            reservations = new List<Reservation>();
        }

        public void RegisterUser(Guest user)
        {
            users.Add(user);
        }

        public void AddHotel(Hotel hotel)
        {
            hotels.Add(hotel);
        }

        public void DisplayHotels()
        {
            Console.WriteLine("List of Hotels:");
            foreach (var hotel in hotels)
            {
                Console.WriteLine($"{hotel.HotelName}, {hotel.Location}");
            }
        }

        public void BookReservation(Hotel hotel, HotelRoom room, Guest guest, DateTime startTime, DateTime endTime)
        {
            if (!room.Status)
            {
                Console.WriteLine("Room is already booked.");
                return;
            }

            room.Status = false;
            Reservation reservation = new Reservation(startTime, endTime, room);
            reservations.Add(reservation);
            guest.BookReservation(reservation);
        }

        public void DisplayReservationDetails(int reservationNumber)
        {
            var reservation = reservations.Find(r => r.ReservationNumber == reservationNumber);
            if (reservation == null)
            {
                Console.WriteLine("Reservation not found.");
                return;
            }

            reservation.DisplayDetails();
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            List<HotelRoom> yananRooms = new List<HotelRoom>();
            HotelRoom room1 = new HotelRoom(101, RoomStyle.TwinRoom, 1500);
            HotelRoom room2 = new HotelRoom(102, RoomStyle.KingRoom, 3000);
            yananRooms.Add(room1);
            yananRooms.Add(room2);
            Hotel hotelYanan = new Hotel("Hotel Yanan", "123 GStreet, Takaw City", yananRooms);

            List<HotelRoom> hotel456Rooms = new List<HotelRoom>();
            HotelRoom hotel456Room1 = new HotelRoom(101, RoomStyle.QueenRoom, 2000);
            HotelRoom hotel456Room2 = new HotelRoom(102, RoomStyle.QueenRoom, 2000);
            hotel456Rooms.Add(hotel456Room1);
            hotel456Rooms.Add(hotel456Room2);
            Hotel hotel456 = new Hotel("Hotel 456", "Session Road, Baguio City", hotel456Rooms);

            HotelManagementSystem hms = new HotelManagementSystem();
            hms.AddHotel(hotelYanan);
            hms.AddHotel(hotel456);

            hms.DisplayHotels();

            hotelYanan.DisplayAvailableRooms();

            Guest terry = new Guest("Terry", "Addr 1", "terry@email.com", 63919129);
            hms.RegisterUser(terry);

            hms.BookReservation(hotelYanan, room1, terry, DateTime.Now, new DateTime(2024, 04, 16));

            hotelYanan.DisplayBookedRooms();

            terry.DisplayReservations();

            Receptionist anna = new Receptionist("Anna", "Addr 2", "anna@email.com", 67890);
            hms.RegisterUser(anna);

            Reservation res = new Reservation(new DateTime(2024, 05, 01), new DateTime(2024, 05, 06), hotel456Room2);
            anna.BookReservation(terry, res);

            terry.DisplayReservations();

            hms.DisplayReservationDetails(1234567890);
        }
    }
}
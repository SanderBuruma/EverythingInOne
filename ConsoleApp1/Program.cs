using System;
using System.Text;
using System.Text.RegularExpressions;
using Utils;

namespace KerbalTradingProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string inputText = Console.ReadLine();
                Console.WriteLine($"input: {inputText}");
                decimal input = decimal.Parse(inputText);
                Console.WriteLine($"input: {input}");
                Console.WriteLine($"sqrt: {input.Sqrt()}");
                Console.ReadKey();
            }
            Console.WriteLine("end");
        }
    }

    #region ship
    class Ship
    {
        #region Fields
        /// <summary>(x,y) In meters compared to starting position, right and up are positive</summary>
        private (decimal, decimal) _position;
        private decimal _dryMassKg;
        private decimal _fuelMassKg;
        private decimal _exhVelocityMs;
        private decimal _thrustN;
        private (decimal, decimal) _velocity;
        private ShipStatus _shipStatus;
        private Planet _currentPlanet;
        private Ship _secondStage;
        private int _fraction;
        private int _ticksInFlight;
        ///<summary>0 is eastbound, 180 is west; 270 isn't north, it's up.</summary>
        private decimal _rotation;
        /// <summary>rad/s</summary>
        private decimal _angularMomentum;
        #endregion

        #region Constructor
        public Ship(
            (decimal, decimal) position,
            (decimal, decimal) velocity,
            decimal dryMassKg,
            decimal fuelMassKg,
            decimal exhVelocityMs,
            decimal thrustN,
            Planet currentPlanet,
            Ship secondStage = null,
            int fraction = 30,
            decimal rotation = 270,
            decimal angularMomentum = 0
        )
        {
            if (dryMassKg <= 0)
                throw new Exception(String.Format("{0} param was 0 or less and shouldn't have been", nameof(dryMassKg)));
            if (fuelMassKg < 0)
                throw new Exception(String.Format("{0} less than 0 and shouldn't have been", nameof(fuelMassKg)));
            if (thrustN < 0)
                throw new Exception(String.Format("{0} less than 0 and less and shouldn't have been", nameof(thrustN)));
            if (position.Item2 < 0)
                throw new Exception(String.Format("{0} less than 0 and less and shouldn't have been", nameof(position) + ".Item2"));
            if (exhVelocityMs <= 0)
                throw new Exception(String.Format("{0} param was 0 or less and shouldn't have been", nameof(exhVelocityMs)));
            if (fraction <= 0)
                throw new Exception(String.Format("{0} param was 0 or less and shouldn't have been", nameof(fraction)));

            _position = position;
            _dryMassKg = dryMassKg;
            _fuelMassKg = fuelMassKg;
            _exhVelocityMs = exhVelocityMs;
            _thrustN = thrustN;
            _velocity = velocity;
            _shipStatus = ShipStatus.Landed;
            _currentPlanet = currentPlanet;
            _secondStage = secondStage;
            _fraction = fraction;
            _ticksInFlight = 0;

            rotation = rotation / (decimal)(360 / (2 * Math.PI));
            _rotation = rotation % (decimal)(2 * Math.PI);
            _angularMomentum = angularMomentum % (decimal)(2 * Math.PI);
        }
        #endregion

        #region Properties
        public decimal DryMass               { get { return _dryMassKg; } }
        public decimal ExhaustVelocity       { get { return _exhVelocityMs; } }
        public decimal FuelMass              { get { return _fuelMassKg; }   set { _fuelMassKg = value; } }
        public Planet Planet                { get { return _currentPlanet; } }
        public (decimal, decimal) Position    { get { return _position; }     set { _position = value; } }
        public Ship SecondStage             { get { return _secondStage; } }
        public decimal Thrust                { get { return _thrustN; } }
        public int TicksInFlight            { get => _ticksInFlight;        set => _ticksInFlight = value; }
        public (decimal, decimal) Velocity    { get { return _velocity; }     set { _velocity = value; } }

        public decimal Acceleration { get { return Thrust / TotalMass; } }

        //local gravity minus centripetal acceleration
        public decimal EffectiveGravity
        {
            get
            {
                return LocalGravity - Velocity.Item1 * Velocity.Item1 / Planet.Radius;
            }
        }

        public decimal FuelUse { get { return _thrustN / _exhVelocityMs; } }

        public decimal LocalGravity
        {
            get
            {
                decimal temp =  _currentPlanet.Radius / (_currentPlanet.Radius + _position.Item2);
                return temp * temp * _currentPlanet.SurfaceGravity;
            }
        }

        public decimal Rotation
        {
            get { return _rotation; }
            set { _rotation = value % (decimal)(2 * Math.PI); }
        }
        public decimal AngularMomentum
        {
            get { return _angularMomentum; }
            set { _angularMomentum = value; }
        }

        public ShipStatus Status
        {
            get { return _shipStatus; }
            set { _shipStatus = value; }
        }

        public decimal TotalMass
        {
            get {
                if (_secondStage == null) return _fuelMassKg + _dryMassKg;
                return _fuelMassKg + _dryMassKg + _secondStage.TotalMass;
            }
        }

        public decimal TWR { get { return Acceleration / EffectiveGravity; } }
        #endregion

        #region Methods
        //ticks the ship a fraction of a second ahead (1/fraction)
        public ShipStatus Tick(int fraction, bool dontRunEngines = false)
        {
            TicksInFlight++;
            if (SecondStage != null) SecondStage.Tick(fraction, true);

            Position = (
                Position.Item1 + Velocity.Item1/fraction,
                Position.Item2 + Velocity.Item2/fraction
            );

            if (Position.Item1 < 0) {
                if (Status == ShipStatus.Crashed) throw new Exception("Ship already crashed, no further ticks should have taken place");
                Status = ShipStatus.Crashed;
            }

            //if no fuel don't do anything else
            (decimal, decimal) thrustAcceleration = (0, 0);
            if (FuelMass > 0 && !dontRunEngines)
            {
                //todo implement proper ship controls and related velocity changes.
                thrustAcceleration.Item1 += (Acceleration*Acceleration - LocalGravity*LocalGravity).Sqrt();
            }
            else Status = ShipStatus.OutOfFuel;

            FuelMass -= FuelUse / fraction;

            //change velocity
            Velocity = (
                Velocity.Item1 + thrustAcceleration.Item1,
                Velocity.Item2 + thrustAcceleration.Item2 - LocalGravity);

            return Status;
        }
        #endregion

        public enum ShipStatus {
            Crashed,
            OutOfFuel,
            Landed,
            Flying,
            Orbiting
        }
    }
    #endregion

    #region planet
    class Planet
    {
        private decimal _surfaceGravitym2s;
        private decimal _radiusInM;
        private decimal _mass;

        public Planet(decimal surfaceGravitym2s, decimal radiusInM)
        {
            _surfaceGravitym2s = surfaceGravitym2s;
            _radiusInM = radiusInM;
        }

        public decimal SurfaceGravity { get { return _surfaceGravitym2s;} }
        public decimal Radius { get { return _radiusInM;} }
    }
    #endregion
}

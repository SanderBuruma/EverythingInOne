using System;
using System.Text;
using System.Text.RegularExpressions;

namespace KerbalTradingProgram
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    #region ship
    class Ship
    {
        #region Fields
        /// <summary>(x,y) In meters compared to starting position, right and up are positive</summary>
        private (double, double) _position;
        private double _dryMassKg;
        private double _fuelMassKg;
        private double _exhVelocityMs;
        private double _thrustN;
        private (double, double) _velocity;
        private ShipStatus _shipStatus;
        private Planet _currentPlanet;
        private Ship _secondStage;
        private int _fraction;
        private int _ticksInFlight;
        ///<summary>0 is eastbound, 180 is west; 270 isn't north, it's up.</summary>
        private double _rotation;
        /// <summary>rad/s</summary>
        private double _angularMomentum;
        #endregion

        #region Constructor
        public Ship(
            (double, double) position,
            (double, double) velocity,
            double dryMassKg,
            double fuelMassKg,
            double exhVelocityMs,
            double thrustN,
            Planet currentPlanet,
            Ship secondStage = null,
            int fraction = 30,
            double rotation = 270,
            double angularMomentum = 0
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

            rotation = rotation / (360 / (2 * Math.PI));
            _rotation = rotation % (2 * Math.PI);
            _angularMomentum = angularMomentum % (2 * Math.PI);
        }
        #endregion

        #region Properties
        public double DryMass               { get { return _dryMassKg; } }
        public double ExhaustVelocity       { get { return _exhVelocityMs; } }
        public double FuelMass              { get { return _fuelMassKg; }   set { _fuelMassKg = value; } }
        public Planet Planet                { get { return _currentPlanet; } }
        public (double, double) Position    { get { return _position; }     set { _position = value; } }
        public Ship SecondStage             { get { return _secondStage; } }
        public double Thrust                { get { return _thrustN; } }
        public int TicksInFlight            { get => _ticksInFlight;        set => _ticksInFlight = value; }
        public (double, double) Velocity    { get { return _velocity; }     set { _velocity = value; } }

        public double Acceleration { get { return Thrust / TotalMass; } }

        //local gravity minus centripetal acceleration
        public double EffectiveGravity
        {
            get
            {
                return LocalGravity - Velocity.Item1 * Velocity.Item1 / Planet.Radius;
            }
        }

        public double FuelUse { get { return _thrustN / _exhVelocityMs; } }

        public double LocalGravity
        {
            get
            {
                return Math.Pow(
                    _currentPlanet.Radius / (_currentPlanet.Radius + _position.Item2)
                    , 2)
                * _currentPlanet.SurfaceGravity;
            }
        }

        public double Rotation
        {
            get { return _rotation; }
            set { _rotation = value % (2 * Math.PI); }
        }
        public double AngularMomentum
        {
            get { return _angularMomentum; }
            set { _angularMomentum = value % (2 * Math.PI); }
        }

        public ShipStatus Status {
            get { return _shipStatus; }
            set { _shipStatus = value; }
        }

        public double TotalMass
        {
            get {
                if (_secondStage == null) return _fuelMassKg + _dryMassKg;
                return _fuelMassKg + _dryMassKg + _secondStage.TotalMass;
            }
        }

        public double TWR { get { return Acceleration / EffectiveGravity; } }
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
            (double, double) thrustAcceleration = (0, 0);
            if (FuelMass > 0 && !dontRunEngines)
            {
                //todo implement proper ship controls and related velocity changes.
                thrustAcceleration.Item1 += Math.Sqrt(Acceleration*Acceleration - LocalGravity*LocalGravity);
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
        private double _surfaceGravitym2s;
        private double _radiusInM;

        public Planet(double surfaceGravitym2s, double radiusInM)
        {
            _surfaceGravitym2s = surfaceGravitym2s;
            _radiusInM = radiusInM;
        }

        public double SurfaceGravity { get { return _surfaceGravitym2s;} }
        public double Radius { get { return _radiusInM;} }
    }
    #endregion
}

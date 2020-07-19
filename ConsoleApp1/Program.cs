using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1
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
        /// <summary>(x,y)In meters compared to starting position, right and up are positive</summary>
        private (Double, Double) _position;
        private Double _dryMassKg;
        private Double _fuelMassKg;
        private Double _exhVelocityMs;
        private Double _thrustKn;
        private (Double, Double) _velocity;
        private ShipStatus _shipStatus;
        private Planet _planet;
        private Ship _secondStage;
        private Int32 _fraction;
        private Int32 _ticksInFlight;

        public Ship(
            (Double, Double) position,
            (Double, Double) velocity,
            double dryMassKg,
            double fuelMassKg,
            double exhVelocityMs,
            double thrustKn,
            Planet planet,
            Ship secondStage = null
        )
        {
            if (dryMassKg <= 0 )
                throw new Exception(String.Format("{0} param was 0 or less and shouldn't have been", nameof(dryMassKg)));
            if (fuelMassKg < 0 )
                throw new Exception(String.Format("{0} less than 0 and shouldn't have been", nameof(fuelMassKg)));
            if (thrustKn < 0 )
                throw new Exception(String.Format("{0} less than 0 and less and shouldn't have been", nameof(thrustKn)));
            if (velocity.Item2 < 0)
                throw new Exception(String.Format("{0} less than 0 and less and shouldn't have been", nameof(velocity) + "item2"));
            if (exhVelocityMs <= 0)
                throw new Exception(String.Format("{0} param was 0 or less and shouldn't have been", nameof(exhVelocityMs)));

            _position = position;
            _dryMassKg = dryMassKg;
            _fuelMassKg = fuelMassKg;
            _exhVelocityMs = exhVelocityMs;
            _thrustKn = thrustKn;
            _velocity = velocity;
            _shipStatus = ShipStatus.Landed;
            _planet = planet;
            _secondStage = secondStage;
            _fraction = 30;
            _ticksInFlight = 0;
        }

        public Double FuelMass { get { return _fuelMassKg; } }
        public Double DryMass { get { return _dryMassKg; } }
        public (Double, Double) Velocity { get { return _velocity; } }
        public (Double, Double) Position { get { return _position; } }
        public Double Acceleration { get { return _thrustKn / TotalMass; } }
        public Double TWR { get { return Acceleration / LocalGravity; } }

        public ShipStatus Status {
            get { return _shipStatus; }
            set { _shipStatus = value; }
        }
        public Double TotalMass { get {
            if (_secondStage == null) return _fuelMassKg + _dryMassKg;
            return _fuelMassKg + _dryMassKg + _secondStage.TotalMass;
        } }
        public Double LocalGravity
        {
            get {
                return Math.Pow(
                    _planet.Radius / (_planet.Radius + _position.Item2)
                    , 2)
                * _planet.SurfaceGravity;
            }
        }

        #region Methods
        //ticks the ship a fraction of a second ahead (1/fraction)
        public ShipStatus Tick(Int32 fraction)
        {
            _position = (
                Position.Item1 + Velocity.Item1/fraction,
                Position.Item2 + Velocity.Item2/fraction
            );

            if (_position.Item1 < 0) {
                if (Status == ShipStatus.Crashed) throw new Exception("Ship already crashed, no further ticks should have taken place");
                Status = ShipStatus.Crashed;
            }

            //if no fuel don't do anything else
            if (_fuelMassKg <= 0) return Status;
            _fuelMassKg -= _thrustKn / _exhVelocityMs;

            //change velocity

            return Status;
        }
        #endregion

        public enum ShipStatus {
            Landed,
            Flying,
            Crashed,
            Orbiting
        }
    }
    #endregion

    #region planet
    class Planet
    {
        private Double _surfaceGravitym2s;
        private Double _radiusInM;

        public Planet(double surfaceGravitym2s, double radiusInM)
        {
            _surfaceGravitym2s = surfaceGravitym2s;
            _radiusInM = radiusInM;
        }

        public Double SurfaceGravity { get { return _surfaceGravitym2s;} }
        public Double Radius { get { return _radiusInM;} }
    }
    #endregion
}

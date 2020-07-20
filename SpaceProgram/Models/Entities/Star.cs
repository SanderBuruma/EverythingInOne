using System;
using Cubit32.Physics;

namespace SpaceProgram.Models.Entities
{
    public class Star : CelestialBody
    {
        public Star
        (
            CelestialBody cb
        )
            : base
        (
            cb.Velocity,
            cb.SurfaceGravity,
            cb.Position,
            cb.Radius,
            cb.Rotation,
            cb.AngularMomentum
        ) { }

        public Star
        (
            Vector2Decimal      velocity,
            decimal             surfaceGravity,
            Vector2Decimal      position,
            decimal             radius = 0.5m,
            decimal             rotation = 270m,
            decimal             angularMomentum = 0m,
            string              name = ""
        )
            : base
        (
            velocity:           velocity,
            position:           position,
            mass:               surfaceGravity * radius * radius / Physics.G,
            rotation:           rotation,
            angularMomentum:    angularMomentum,
            radius:             radius,
            name:               name
        ) { }
    }
}

// *******************************************************************************************
//  MissileTrajectory.cs    -   contains struct MissileTrajectory for use with CMPE2300 Lab 02
//
//  Written by Jonathan Melcher on 28/10/2015
//  Last updated 29/10/2015
// *******************************************************************************************

using System.Drawing;


namespace CMPE2300JonathanMelcherL02
{
    // ********************************************************************************
    //  struct:         struct MissileTrajectory
    //  purpose:        provide an immutable object encapsulating the static 'physical'
    //                  properties of a Missile object
    // ********************************************************************************
    struct MissileTrajectory
    {
        public readonly Point Entry;
        public readonly double Angle;
        public readonly int Speed;
        public readonly int AltitudeOfExplosion;

        public MissileTrajectory(Point e, double a, int s, int aoe)
        {
            Entry = e;
            Angle = a;
            Speed = s;
            AltitudeOfExplosion = aoe;
        }
    }
}
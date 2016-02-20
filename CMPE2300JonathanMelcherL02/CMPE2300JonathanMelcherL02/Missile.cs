// ********************************************************************
//  Missile.cs  -   contains class Missile for use with CMPE2300 Lab 02
//
//  Re-written by Jonathan Melcher on 29/10/2015
//  Last updated 30/10/2015
// ********************************************************************

using System;
using System.Drawing;
using GDIDrawer;


namespace CMPE2300JonathanMelcherL02
{
    // *****************************************************************************************************************
    //  class:                  class Missile
    //  purpose:                encapsulates the properties and functionality of a projectile in the game MissileCommand
    // *****************************************************************************************************************
    class Missile
    {
        #region constants

        private const int DEFAULT_EXPLOSION_RADIUS = 50;                        // default setting for ExplosionRadius
        private const int MAX_EXPLOSION_RADIUS = 150;                           // maximum setting for ExplosionRadius
        private const int MIN_RADIUS = 10;                                      // minimum radius size (starts this size)
        private const int INITIAL_PATH_LENGTH = 5;                              // starting polar distance from entry
        private const int EXPLOSION_INCREMENT = 5;                              // rate of change of radius size during explosion
        private const int COMPUTER_SPEED = 5;                                   // speed of Computer missile
        private const int USER_SPEED_SCALE = 4;                                 // scales User's speed from Computer's
        private const int USER_SPEED = COMPUTER_SPEED * USER_SPEED_SCALE;       // speed of User missile
        private const int DIMINISH_SCALE = 5;                                   // scale for diminishing rate
        private const int ALPHA_DECREMENT = -10;                                // rate of change of alpha (255 -> 0)
        private const double COMPUTER_LOWER_ANGLE = Math.PI * 5 / 4;            // lowest angle Computer can shoot at
        private const double COMPUTER_ANGLE_RANGE = Math.PI / 2;                // range of angles Computer can shoot at

        private static readonly Color USER_COLOR = Color.Green;                 // colour of player missiles
        private static readonly Color COMPUTER_COLOR = Color.Red;               // colour of computer missiles

        private static Random _rng;                                             // random number generator

        #endregion
        #region constructors

        // *************************************************
        //  constructor:            static Missile()
        //  purpose:                initialize static fields
        // *************************************************
        static Missile()
        {
            _rng = new Random();
            Canvas = null;
            ExplosionRadius = DEFAULT_EXPLOSION_RADIUS;
        }

        // *****************************************************************************************************************
        //  constructor:            public Missile()
        //  purpose:                constructs a randomized computer-owned missile.  leverages this(MissileTeam, Trajectory)
        // *****************************************************************************************************************
        public Missile() : this(MissileTeam.Computer, GetComputerTrajectory(COMPUTER_LOWER_ANGLE, COMPUTER_ANGLE_RANGE, COMPUTER_SPEED)) { }

        // **************************************************************************************************
        //  constructor:            public Missile(Point source, Point destination)
        //  purpose:                constructs a user-owned missile.  leverages this(MissileTeam, Trajectory)
        // **************************************************************************************************
        public Missile(Point source, Point destination) : this(MissileTeam.User, GetUserTrajectory(source, destination, USER_SPEED)) { }

        // *****************************************************************************************
        //  constructor:            private Missile(MissileTeam team, MissileTrajectory trajectory)
        //  purpose:                constructs a missile given its team and its immutable trajectory
        //  notes:                  this is only used when leveraging from other constructors
        // *****************************************************************************************
        private Missile(MissileTeam team, MissileTrajectory trajectory)
        {
            Status = new MissileStatus(team, MissileState.Moving, MissileDeath.None);
            Trajectory = trajectory;
            Alpha = byte.MaxValue;
            Radius = MinRadius;
            ExplodeFromAltitudeFn = GetExplodeFromAltitudeFn();
        }

        #endregion
        #region public static properties

        public static CDrawer Canvas { private get; set; }                      // shared CDrawer canvas
        public static int MinRadius { get { return MIN_RADIUS; } }              // exposes constant field
        public static int MaxRadius { get { return MAX_EXPLOSION_RADIUS; } }    // exposes constant field
        public static int ExplosionRadius { get; set; }                         // radius to grow to when exploding

        #endregion
        #region public properties

        public MissileStatus Status { get; set; }       // team, state, death of Missile
        public bool Checked { get; set; }               // used during collision-chain searches
        public int Radius { get; private set; }         // current radius of missile

        // **********************************************************************
        //  property:           public Point Current
        //  purpose:            calculates Cartesian position from Polar position
        // **********************************************************************
        public Point Current
        {
            get
            {
                return new Point(
                    (int)(Math.Cos(Trajectory.Angle) * PathLength + Trajectory.Entry.X),
                    (int)(Math.Sin(Trajectory.Angle) * PathLength + Trajectory.Entry.Y));
            }
        }

        #endregion
        #region private properties

        private Color Colour{ get { return Status.Team == MissileTeam.User ? USER_COLOR : COMPUTER_COLOR; } }
        private int Alpha { get; set; }                                     // transparency on CDrawer canvas (related to Status.State.Dead)
        private double PathLength { get; set; }                             // current distance traveled
        private Func<int, int, bool> ExplodeFromAltitudeFn { get; set; }    // used to determine if Missile should explode w.r.t height
        private MissileTrajectory Trajectory { get; set; }                  // access to constant trajectory fields

        #endregion
        #region private static methods

        // *******************************************************************************************************************
        //  method:         private static MissileTrajectory GetComputerTrajectory(double lowerAngle, double range, int speed)
        //  purpose:        generate randomly an immutable trajectory struct for a 'Team Computer' Missile object
        //  parameters:     double lowerAngle
        //                  double range
        //                  int speed
        //  returns:        MissileTrajectory
        // *******************************************************************************************************************
        private static MissileTrajectory GetComputerTrajectory(double lowerAngle, double range, int speed)
        {
            if (Canvas == null)
                throw new InvalidOperationException("CDrawer Canvas has not been initialized!");

            Point entry = new Point(_rng.Next(0, Canvas.m_ciWidth), Canvas.m_ciHeight - 1);
            double angle = _rng.NextDouble() * range + lowerAngle;

            return new MissileTrajectory(entry, angle, speed, 0);
        }

        // ***************************************************************************************************************
        //  method:         private static MissileTrajectory GetUserTrajectory(Point source, Point destination, int speed)
        //  purpose:        generate the immutable trajectory struct for a 'Team User' Missile object
        //  parameters:     Point source
        //                  Point destination
        //                  int speed
        //  returns:        MissileTrajectory
        // ***************************************************************************************************************
        private static MissileTrajectory GetUserTrajectory(Point source, Point destination, int speed)
        {
            return new MissileTrajectory(source, CDrawerGeometry.Functions.GetAngle(source, destination), speed, destination.Y);
        }

        #endregion
        #region public methods

        // *****************************************************************
        //  method:                 public void Draw()
        //  purpose:                draws Missile onto shared CDrawer canvas
        //  parameters:             none
        //  returns:                nothing
        // *****************************************************************
        public void Draw()
        {
            if (Canvas == null)
                throw new InvalidOperationException("CDrawer Canvas has not been initialized!");

            // all geometry is initially done in a normal cartesian coordinate system and must be mapped to the
            // coordinate system used by the CDrawer canvas

            Point mappedEntry = CDrawerGeometry.Functions.TransformOriginToFromTopLeft(Canvas, Trajectory.Entry);
            Point mappedCurrent = CDrawerGeometry.Functions.TransformOriginToFromTopLeft(Canvas, Current);

            // each missile is composed of a line indicating its path and a solid 2D ball representing the missile itself
            Canvas.AddLine(
                mappedEntry.X, mappedEntry.Y, mappedCurrent.X, mappedCurrent.Y, Color.FromArgb(Alpha, Colour));

            Canvas.AddCenteredEllipse(
                mappedCurrent.X, mappedCurrent.Y, Radius * 2, Radius * 2, Color.FromArgb(Alpha, Colour));
        }

        // *****************************************************************************
        //  method:                 public void Tick()
        //  purpose:                performs actions on instance based on current Status
        //  parameters:             none
        //  returns:                nothing
        // *****************************************************************************
        public void Tick()
        {
            switch (Status.State)
            {
                case MissileState.Moving:
                    Move();
                    break;
                case MissileState.Exploding:
                    Explode();
                    break;
                case MissileState.Diminishing:
                    Diminish();
                    break;
            }
        }

        public void CollideWithBorder()
        {
            Alpha = 0;
            Status = new MissileStatus(Status, MissileState.Dead, MissileDeath.OutOfBounds);
        }

        public void CollideWithGround()
        {
            if (Status.State == MissileState.Moving)
            {
                Status = new MissileStatus(Status, MissileState.Exploding);

                if (Status.Team == MissileTeam.Computer)
                    Status = new MissileStatus(Status, MissileDeath.Ground);
            }
        }

        // **********************************************************************************************************
        //  method:                 public override bool Equals(object obj)
        //  purpose:                allows some methods calling .Equals() to use collision detection as their metric
        //                          note that Equals and GetHashCode are not acting together, and Linq/Iteration
        //                          should normally refer to GetHashCode()'s functionality unless a collision is made
        //  parameters:             object obj
        //  returns:                bool
        // **********************************************************************************************************
        public override bool Equals(object obj)
        {
            // equality is determined to be when the circle about each Missile is intersecting
            Missile that = obj as Missile;
            return that != null && CDrawerGeometry.Functions.IsCircleOnCircle(this.Current, this.Radius, that.Current, that.Radius);
        }

        // ********************************************************************************************
        //  method:                 public override int GetHashCode()
        //  purpose:                allow Linq methods/iteration to work despite the overridden Equals
        //                          note that Equals and GetHashCode are not acting together, as Equals
        //                          is being used for collision detection
        //  parameters:             none
        //  returns:                int
        // ********************************************************************************************
        public override int GetHashCode()
        {
            // these properties seem to be the defining traits of a missile, so each one should be unique up to these
            return new { Trajectory, Status, Radius, Alpha, Checked }.GetHashCode();
        }

        #endregion
        #region private instance methods

        private void Move()
        {
            PathLength += Trajectory.Speed;

            // adjust for moving past explosionAltitude point and change State to Exploding
            if (ExplodeFromAltitudeFn(Current.Y, Trajectory.AltitudeOfExplosion))
            {
                AdjustOvershoot();
                Status = new MissileStatus(Status, MissileState.Exploding);
            }
        }

        private void Diminish()
        {
            // decrease alpha to a minimum
            Alpha = Math.Max(Alpha + ALPHA_DECREMENT, 0);

            // decrease Radius to its minimum in a logarithmic way
            Radius = (int)Math.Max(MIN_RADIUS, Radius + ExplosionRadius / (ALPHA_DECREMENT * Math.Log(Radius * DIMINISH_SCALE)));
            if (Alpha == 0)
                Status = new MissileStatus(Status, MissileState.Dead);
        }

        private void Explode()
        {
            Radius += EXPLOSION_INCREMENT;

            // once radius has reached a maximum, change status
            if (Radius >= ExplosionRadius)
            {
                Radius = ExplosionRadius;
                Status = new MissileStatus(Status, MissileState.Diminishing);
            }
        }

        // *********************************************************************************************
        //  method:         private void AdjustOvershoot()
        //  purpose:        projects PathLength such that Current.Y is the AltitudeOfExplosion, assuming
        //                  it has gone past the altitude already
        //  parameters:     none
        //  returns:        nothing
        // *********************************************************************************************
        private void AdjustOvershoot()
        {
            PathLength -= (1.0) * Math.Abs(Current.Y - Trajectory.AltitudeOfExplosion) / Math.Sin(Trajectory.Angle);
        }

        // **************************************************************************************************
        //  method:         protected Func<int, int, bool> GetExplodeFromAltitudeFn()
        //  purpose:        calculates function to determine when to explode w.r.t height based on trajectory
        //  parameters:     none
        //  returns:        Func<int, int, bool>
        // **************************************************************************************************
        private Func<int, int, bool> GetExplodeFromAltitudeFn()
        {
            // if missile's current location is below the source and below the target then explode
            if (Trajectory.Entry.Y - Trajectory.AltitudeOfExplosion > 0)
                return (currentY, explosionAlt) => currentY <= explosionAlt;

            // if missile's current location is above the source and above the target then explode
            else if (Trajectory.Entry.Y - Trajectory.AltitudeOfExplosion < 0)
                return (currentY, explosionAlt) => currentY >= explosionAlt;

            // missile will not explode based on altitude (must check for collisions/out of bounds/etc.)
            return (currentY, explosionAlt) => false;
        }

        #endregion
    }
}
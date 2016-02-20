// *****************************************************************************************************
//  MissileCommand.cs   -   contains class MissileCommand for use with CMPE2300 Lab 2
//                          provides a container and methods for running a single game of MissileCommand
//
//  Written by Jonathan Melcher for CMPE2300 Lab02 on 10/09/2015
//  Last updated 30/10/2015
// *****************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using GDIDrawer;
using CDrawerGeometry;


namespace CMPE2300JonathanMelcherL02
{
    // *****************************************************************************************************
    //  class:          static class MissileCommand
    //  purpose:        provides a container and functionality for playing a complete game of MissileCommand
    // *****************************************************************************************************
    static class MissileCommand
    {
        #region constants

        private const int DEFAULT_SCREEN_WIDTH_PIXELS = 800;
        private const int DEFAULT_SCREEN_HEIGHT_PIXELS = 600;
        private const int USER_MAX_MISSILE_AMOUNT = 50;
        private const int POINTS_PER_MISSILE = 100;
        private const int COMPUTER_MAX_MISSILE_AMOUNT = 10;
        private const int USER_SOURCE_AMOUNT = 5;                       // how many possible places to fire missiles from
        private const int USER_LIVES = 5;                               // how many times computer missiles can hit ground surface

        private const string GAME_OVER = "Game Over";
        private const string PAUSED = "Paused";

        private const int STATUS_TEXT_X_OFFSET_PIXELS = -75;            // for GAMEOVER/PAUSED text
        private const int STATUS_TEXT_Y_OFFSET_PIXELS = -50;            // ""
        private const int SCORE_LIVES_TEXT_OFFSET_PIXELS = -20;         // for writing Score/lives text to screen

        private const int TEXT_SIZE = 14;                               // for all displayed text
        private const int TEXT_BOX_WIDTH_PIXELS = 150;                  // ""
        private const int TEXT_BOX_HEIGHT_PIXELS = 150;                 // ""
        private static readonly Color TEXTCOLOR = Color.Yellow;         // ""

        #endregion
        #region private fields

        private static Ground ground;                               // a randomly generated surface to launch missiles from
        private static List<Point> userSources;                     // list of locations the user can launch missiles from
        private static List<Missile> user;                          // list of user missiles currently on the screen
        private static List<Missile> computer;                      // list of computer missiles currently on the screen

        #endregion
        #region constructors

        // **********************************************************************************************************
        //  constructor:            static MissileCommand()
        //  purpose:                initialize all fields
        //  notes:                  -   canvas/ground are null until a new game is started through a method call
        //                          -   Running/Paused/HasBeenRun must be initialized to false and will be altered
        //                              through method calls relating to starting/pausing/restarting the current game
        // **********************************************************************************************************
        static MissileCommand()
        {
            Canvas = null;
            ground = null;
            userSources = new List<Point>();
            user = new List<Missile>();
            computer = new List<Missile>();
            Running = false;
            Paused = false;

            ComputerMissileMax = COMPUTER_MAX_MISSILE_AMOUNT;        // default value - can be changed by user through Form GUI
        }

        #endregion
        #region public properties

        public static CDrawer Canvas { private get; set; }
        public static bool Running { get; private set; }
        public static bool Paused { get; set; }

        public static int ComputerMissileMax { get; set; }

        // access to constant
        public static int UserMissileMax { get { return USER_MAX_MISSILE_AMOUNT; } }

        // total number of computer missiles throughout game
        public static int Incoming { get; private set; }

        // total number of destroyed computer missiles throughout game
        public static int IncomingDestroyed { get; private set; }

        // total number of user missiles launched
        public static int Launched { get; private set; }

        // amount of times computer missiles can collide with ground
        public static int Lives { get; private set; }

        // score aggregated from computer missiles with MissileDeath.Killed status
        public static int Score { get; private set; }

        #endregion
        #region public methods

        // ****************************************************************************************************************
        //  method:                 public static void ResetForNewGame()
        //  purpose:                reset game-instance-variable fields so each new game-instance starts the same, save for
        //                          any randomly generated game-instance-properties
        //  parameters:             none
        //  returns:                nothing
        // ****************************************************************************************************************
        public static void ResetForNewGame()
        {
            user.Clear();
            computer.Clear();
            Lives = USER_LIVES;
            Score = 0;
            Incoming = 0;
            IncomingDestroyed = 0;
            Launched = 0;
        }

        // *******************************************************************************************************
        //  method:                 public static void StartNewGame()
        //  purpose:                initialize or regenerate the CDrawer canvas/Ground surface, setup class fields
        //                          for starting a new game, and turn Running on to allow Tick() to operate
        // *******************************************************************************************************
        public static void StartNewGame()
        {
            // first game - load up a CDrawer canvas and a new Ground class
            if (Canvas == null)
            {
                Canvas = GetCanvasWithEvents(DEFAULT_SCREEN_WIDTH_PIXELS, DEFAULT_SCREEN_HEIGHT_PIXELS);
                ground = new Ground(Canvas);
                Missile.Canvas = Canvas;
            }
            // nth game - wipe out canvas background and regenerate ground
            else
            {
                for (int i = 0; i < Canvas.ScaledWidth; ++i)
                    for (int j = 0; j < Canvas.ScaledHeight; ++j)
                        Canvas.SetBBPixel(i, j, Color.Black);
                ground.Generate();
            }

            ground.Draw();                      // draw canvas background
            ResetForNewGame();                  // reset game-instance-variable fields

            // re-roll launch sites for user based on current ground surface
            userSources = ground.GetRandomSurfacePoints(USER_SOURCE_AMOUNT);

            Paused = false;
            Running = true;                     // enable so higher up event handlers will work on their Tick()
        }

        // *************************************************************************************************************
        //  method:                 public static void RestartGame()
        //  purpose:                depending on whether StartNewGame() has been called before, restart/start a new game
        //  parameters:             none
        //  returns:                nothing
        // *************************************************************************************************************
        public static void RestartGame()
        {
            if (Canvas != null)
            {
                ResetForNewGame();
                ground.Reset();
                ground.Draw();

                // re-roll launch sites for user based on current ground surface
                userSources = ground.GetRandomSurfacePoints(USER_SOURCE_AMOUNT);

                Paused = false;
                Running = true;
            }
            else
                StartNewGame();
        }

        // ********************************************************************************
        //  method:                 public static void Tick()
        //  purpose:                calculate and update the current game to its next state
        //  parameters:             none
        //  returns:                nothing
        // ********************************************************************************
        public static void Tick()
        {
            if (Running)
            {
                if (!Paused && Lives > 0)
                {
                    PopulateComputerMissiles();

                    GetMissiles().ForEach(m => m.Tick());

                    ProcessMissileCollisions();
                    ProcessDeadMissiles();
                }

                Draw();
            }
        }

        #endregion
        #region private methods

        // *********************************************************************************
        //  method:                 private static void PopulateComputerMissiles()
        //  purpose:                fills up computer List<Missile> up to IncomingMissileMax
        //                          after dead Missiles have been cleared away
        //  parameters:             none
        //  returns:                nothing
        // *********************************************************************************
        private static void PopulateComputerMissiles()
        {
            while (computer.Count < ComputerMissileMax)
            {
                computer.Add(new Missile());
                ++Incoming;
            }
        }

        // *****************************************************************************************************************
        //  method:                 private static void ProcessMissileCollisions()
        //  purpose:                determine whether each missile on the screen has collided, updating its Status and Death
        //                          afterwards, taking bad-chaining into account for scoring purposes
        //  notes:                  a bad-chain may occur when a computer missile hits the ground, a player missile collides
        //                          with it afterwards, as well as another computer missile.  the last computer missile is
        //                          connected to the player missile through a ground explosion, which SHOULD NOT result in
        //                          points.  this is only one example of such a bad-chain
        //  parameters:             none
        //  returns:                nothing
        // *****************************************************************************************************************
        private static void ProcessMissileCollisions()
        {
            foreach (Missile m in GetMissiles().Where(m => m.Status.State == MissileState.Moving))
            {
                // a collision of _some_ sort has occurred -> change Status
                if (GetCollisions(m, true).Count() > 1)
                {
                    m.Status = new MissileStatus(m.Status, MissileState.Exploding);

                    // an exploding User missile does not score points
                    if (m.Status.Team == MissileTeam.User)
                        m.Status = new MissileStatus(m.Status, MissileDeath.NonScoring);

                    // regenerate collision for determining MissileDeath of the Computer missile, taking bad-chaining
                    // into account.  we avoid chaining through other Computer missiles exploded on the ground surface,
                    // and simply look for a chain to a User missile, or a flag that says another Missile _had_ been
                    // chained to a User missile

                    else
                        m.Status = new MissileStatus(m.Status, GetCollisions(m, false, MissileDeath.Ground).Any(
                             c => c.Status.Team == MissileTeam.User || c.Status.Death == MissileDeath.Scoring) ?
                                                                MissileDeath.Scoring : MissileDeath.NonScoring);
                }

                // if a collision has occured these do nothing, otherwise if there is a ground collision, canvas is ignored
                MissileCollisionWithEnvironment(m, ground);
                MissileCollisionWithEnvironment(m, Canvas);
            }
        }

        // **************************************************************************************
        //  method:                 private static void ProcessDeadMissiles()
        //  purpose:                removes dead Missiles from their respective lists and updates
        //                          game values based on their MissileDeath values
        //  parameters:             none
        //  returns:                nothing
        // **************************************************************************************
        private static void ProcessDeadMissiles()
        {
            lock (user)
                lock (computer)
                {
                    IEnumerable<Missile> computerDead = computer.Where(m => m.Status.State == MissileState.Dead);

                    // remove all user missiles which are dead -- they do not affect the scoring
                    user.RemoveAll(m => m.Status.State == MissileState.Dead);

                    // remove all sources which have been hit by computer missiles
                    userSources.RemoveAll(src => computerDead.Any(m => CDrawerGeometry.Functions.IsPointOnCircle(src, m.Current, Missile.ExplosionRadius)));

                    // subtract from lives the number of computer missiles which exploded on the ground
                    Lives -= computerDead.Sum(m => { return m.Status.Death == MissileDeath.Ground ? 1 : 0; });

                    // add to score the number of computer missiles * score per missile which were killed by user missiles
                    Score += computerDead.Sum(m => { return m.Status.Death == MissileDeath.Scoring ? POINTS_PER_MISSILE : 0; });

                    // add to incoming destroyed all computer missiles which are dead when removing them
                    IncomingDestroyed += computer.RemoveAll(m => m.Status.State == MissileState.Dead);
                }
        }

        // *************************************************************************
        //  method:                 private static void Draw()
        //  purpose:                renders all game sprites onto the CDrawer canvas
        //  parameters:             none
        //  returns:                nothing
        // *************************************************************************
        private static void Draw()
        {
            Canvas.Clear();

            GetMissiles().ForEach(m => m.Draw());
            DrawText();

            Canvas.Render();
        }

        // ***************************************************************
        //  method:                 private static void DrawText()
        //  purpose:                draws all text onto the CDrawer canvas
        //  parameters:             none
        //  returns:                nothing
        // ***************************************************************
        private static void DrawText()
        {
            string message = Lives < 1 ? GAME_OVER : Paused ? PAUSED : "";

            Canvas.AddText(string.Format("Score: {0}\nLives: {1}", Score, Lives), TEXT_SIZE, SCORE_LIVES_TEXT_OFFSET_PIXELS,
                                                   SCORE_LIVES_TEXT_OFFSET_PIXELS, TEXT_BOX_WIDTH_PIXELS, TEXT_BOX_HEIGHT_PIXELS, TEXTCOLOR);

            Canvas.AddText(message, TEXT_SIZE, Canvas.m_ciWidth / 2 + STATUS_TEXT_X_OFFSET_PIXELS,
                Canvas.m_ciHeight / 2 + STATUS_TEXT_Y_OFFSET_PIXELS, TEXT_BOX_WIDTH_PIXELS, TEXT_BOX_HEIGHT_PIXELS, TEXTCOLOR);
        }

        // ***************************************************************************************
        //  method:                 private static List<Missile> GetMissiles()
        //  purpose:                get contents of user and computer combines in a threadsafe way
        //  parameters:             none
        //  returns:                List<Missile> of user and computer combined
        // ***************************************************************************************
        private static List<Missile> GetMissiles()
        {
            List<Missile> missiles = new List<Missile>(USER_MAX_MISSILE_AMOUNT * 2);
            lock (user)
                lock (computer)
                {
                    missiles.AddRange(user);
                    missiles.AddRange(computer);
                }

            return missiles;
        }

        // ************************************************************************************************
        //  method:                 private void MissileCollisionWithEnvironment(Missile m, object env)
        //  purpose:                determine if Missile is colliding with an outside 'environment' object,
        //                          adjusting Status if necessary and communicating to 'environment' object
        //                          if necessary
        //  parameters:             object env (CDrawer, Ground)
        //  returns:                nothing
        // ************************************************************************************************
        private static void MissileCollisionWithEnvironment(Missile m, object env)
        {
            if (IsMissileCollidingWithEnvironment(m, env))
            {
                Type t = env.GetType();
                if (t == typeof(CDrawer))
                    // affest Status
                    m.CollideWithBorder();
                else if (t == typeof(Ground))
                {
                    // affect Status
                    m.CollideWithGround();

                    // only Computer missiles make craters in the Ground surface
                    if (m.Status.Team == MissileTeam.Computer)
                        (env as Ground).Impact(m.Current, Missile.ExplosionRadius);
                }
            }
        }

        private static bool IsMissileCollidingWithEnvironment(Missile m, object env)
        {
            if (ReferenceEquals(env, null))
                throw new ArgumentNullException("Null argument given!");

            Type t = env.GetType();

            if (t == typeof(Ground))
                return (env as Ground).IsCollidingWith(m.Current, m.Radius);
            else if (t == typeof(CDrawer))
                return CDrawerGeometry.Functions.IsOutOfBounds(env as CDrawer, m.Current);

            throw new ArgumentException("Invalid argument given!");
        }

        // *****************************************************************************************************************
        //  method:                 private static IEnumerable<Missile> GetCollisions(
        //                              Missile start, bool allChaining, params MissileDeath[] chainFilter)
        //  purpose:                get all missiles which collided mid-air with the given missile, given how to chain
        //                          the explosions, and which Missiles to filter using the chainFilter
        //  notes:                  - for ALL visible collisions, set allChaining to true and filtered to MissileDeath.Other
        //                            this will give you whether a missile SHOULD explode or not
        //                          - afterwards for determining MissileDeath, set allChaining to false, and filtered to
        //                            MissileDeath.Ground.  this will only chain through mid-air explosions,
        //                            or ground explosions from the player
        //                          
        //  parameters:             Missile start
        //                          bool allChaining
        //                          params MissileDeath[] chainFilter
        //  returns:                HashSet<Missile> of collisions
        // *****************************************************************************************************************
        private static IEnumerable<Missile> GetCollisions(Missile start, bool allChaining, params MissileDeath[] chainFilter)
        {
            return _GetCollisions(start, allChaining, chainFilter).Select(m =>
            {
                m.Checked = false;
                return m;
            });
        }

        // *****************************************************************************************************************
        //  method:                 private static HashSet<Missile> _GetCollisions(
        //                              Missile start, bool allChaining, params MissileDeath[] chainFilter)
        //  purpose:                helper function for GetCollisions
        //                          recursively collects Missiles based on collision, whether or not to chain, and the
        //                          chainFilter, finally setting the Checked field of all collected Missiles to true
        //  notes:                  -   allChaining either allows all (already collided) Missiles to be collected, otherwise
        //                              Missiles whose .Death is in the chainFilter can only be User-owned
        //  parameters:             Missile start
        //                          bool allChaining
        //                          params MissileDeath[] chainFilter
        //  returns:                HashSet<Missile> of collisions but with .Checked toggled on
        // *****************************************************************************************************************
        private static HashSet<Missile> _GetCollisions(Missile start, bool allChaining, params MissileDeath[] chainFilter)
        {
            HashSet<Missile> collision = new HashSet<Missile>();
            start.Checked = true;
            collision.Add(start);

            foreach (Missile m in GetMissiles().Where(
                m => m.Status.Death != MissileDeath.OutOfBounds && !m.Checked && start.Equals(m)))
                if (allChaining)
                    collision.UnionWith(_GetCollisions(m, allChaining, chainFilter));
                else if (!chainFilter.Contains(m.Status.Death) || m.Status.Team == MissileTeam.User)
                    collision.UnionWith(_GetCollisions(m, allChaining, chainFilter));

            return collision;
        }

        // ***********************************************************************************************
        //  method:                 private static void CDrawer GetCanvasWithEvents(int width, int height)
        //  purpose:                generates a new canvas with the game-related mouse-click events
        //                          and continuousUpdate off to allow the MissileCommand class to render
        //  parameters:             int width
        //                          int height
        //  returns:                CDrawer
        // ***********************************************************************************************
        private static CDrawer GetCanvasWithEvents(int width, int height)
        {
            CDrawer canvas = new CDrawer(width, height, false);
            canvas.MouseLeftClick += new GDIDrawerMouseEvent((p, c) =>
            {
                if (Running && !Paused && Lives > 0)
                {
                    Point entry = Functions.GetClosestLocation(p, userSources);
                    if (entry.X >= 0 && entry.Y >= 0)
                    {
                        user.Add(new Missile(entry, new Point(p.X, canvas.ScaledHeight - 1 - p.Y)));
                        ++Launched;
                    }
                }
            });

            return canvas;
        }

        #endregion
    }
}
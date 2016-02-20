// ***************************************************************
//  Ground.cs   -   contains class Ground, used for CMPE2300 Lab02
//
//  Written by Jonathan Melcher for CMPE2300 Lab02 on 09/10/2015
//  Last updated 30/10/2015
// ***************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using GDIDrawer;


namespace CMPE2300JonathanMelcherL02
{
    // ****************************************************************************************************
    //  enum:                   enum ColourFunction
    //  purpose:                provides a way to choose between which colour generation function to use on
    //                          the key/value pairs of the ground surface
    // ****************************************************************************************************
    enum ColourFunction { DopedBrownGrey, DopedGreenBrown, DopedGreenGrey };

    // ********************************************************************************************************************
    //  class:          class Ground : CDrawerGeometry
    //  purpose:        generate random ground terrain for the game MissileCommand, essentially a set of rectangles of
    //                  varying heights.  these rectangles are represented by a dictionary structure, where the leftmost
    //                  x value is the key and the height is the value.  color is determined by a generating function based
    //                  on the current ColourFunction state
    // ********************************************************************************************************************
    class Ground
    {
        #region constants

        private const int MIN_VERTICAL_SCALE = 3;           // min VScale to supercede VERTICALSCALE
        private const int MAX_VERTICAL_SCALE = 100;         // max VScale to supercede VERTICALSCALE
        private const int VERTICAL_SCALE = 5;               // scales Height to determine initial height for random walk
        private const int HORIZONTAL_SCALE = 2;             // difference in value between keys representing ground
        private const int HEIGHT_OFFSET = -30;              // offset from initial height to start random walk at
        private const int HEIGHT_DIFFERENTIAL = 2;          // difference in height from key to key during random walk
        private const int COLOR_DIFFERENTIAL = 25;          // range from base color to randomly choose color from
        private const int DOPED_MID_LOWER_SCALE = 15;       // lower bound scale for generating both colours
        private const int DOPED_MID_UPPER_SCALE = 10;       // upper bound  scalefor generating both colours

        private static readonly Color BASE_BROWN = Color.FromArgb(150, 80, 50);
        private static readonly Color BASE_GREY = Color.FromArgb(180, 190, 210);
        private static readonly Color BASE_GREEN = Color.FromArgb(40, 115, 60);
        private static readonly Color IMPACT_COLOR = Color.Black;

        #endregion

        #region private static fields

        private static Random _rng;                     // random number generator

        #endregion
        #region private fields

        private Dictionary<int, int> originals;         // when changes are made from impacts, original key/values stored here
        private Dictionary<int, int> ground;            // key/value pairs (x, y) representing rectangles of ground surface

        #endregion
        #region constructors

        // ****************************************************************************************
        //  constructor:            static Ground()
        //  purpose:                initialize random new generator and VScale to its default value
        // ****************************************************************************************
        static Ground()
        {
            _rng = new Random();
            VScale = VERTICAL_SCALE;
        }

        // **********************************************************************************
        //  constructor:            public Ground(CDrawer canvas, string colourFn)
        //  purpose:                initialize fields connecting Ground to the CDrawer canvas
        //                          and then generate and draw the ground onto the canvas
        // **********************************************************************************
        public Ground(CDrawer canvas, ColourFunction cFn = ColourFunction.DopedGreenBrown)
        {
            if (canvas == null)
                throw new ArgumentNullException("CDrawer canvas has not been initialized!");

            Canvas = canvas;
            Height = Canvas.ScaledHeight;
            Width = Canvas.ScaledWidth;
            CFn = cFn;
            originals = new Dictionary<int, int>();
            ground = new Dictionary<int, int>();
            Generate();
        }

        #endregion
        #region public static properties

        public static int MaxVScale { get { return MAX_VERTICAL_SCALE; } }       // access to private constant
        public static int MinVScale { get { return MIN_VERTICAL_SCALE; } }       // access to private constant
        public static int HScale { get { return HORIZONTAL_SCALE; } }    // access to private constant

        // *************************************************************************************************
        //  property:               public static int VScale
        //  purpose:                the current vertical scale to determine the initial height when starting
        //                          the random walk when generating the ground surface
        // *************************************************************************************************
        public static int VScale { get; set; }

        public static ColourFunction CFn { get; set; }      // applied to a key/value rectangle to get pixel colours

        #endregion
        #region public properties

        public int Height { get; private set; }             // height of shared CDrawer canvas
        public int Width { get; private set; }              // width of shared CDrawer canvas

        #endregion
        #region private properties

        private CDrawer Canvas { get; set; }                // constructor-assigned CDrawer canvas for visualization

        #endregion
        #region public indexers

        // ***************************************************************************************************
        //  indexer:                public int this[int x]
        //  purpose:                provides access to the internal dictionary of key/value pairs representing
        //                          the width/height of the ground surface
        //  parameters:             int key (represents range of x locations [key, key + HSCALE] on CDrawer)
        //  returns:                ground[key] (height of rectangle along range set by key)
        // *****************************************************************************************************
        public int this[int key]
        {
            get { return ground[key]; }
            set { ground[key] = Math.Min(Math.Max(value, 0), Height); }
        }

        #endregion
        #region public functions

        // ********************************************************************************************
        //  method:                 public void Generate()
        //  purpose:                creates a set of rectangles of width HSCALE along the width of the
        //                          shared CDrawer canvas.  height is determined by a random walk and a
        //                          starting height.  this set is represented by a Dictionary structure
        //  parameters:             none
        //  returns:                nothing
        // ********************************************************************************************
        public void Generate()
        {
            originals.Clear();
            ground.Clear();

            int currentHeight = Height / VScale + HEIGHT_OFFSET;
            for (int i = 0; i < Width; i += HScale)
            {
                ground.Add(i, currentHeight);
                currentHeight = Math.Min(Math.Max(0, currentHeight + _rng.Next(-HEIGHT_DIFFERENTIAL, HEIGHT_DIFFERENTIAL + 1)), Height);
            }
        }

        // ****************************************************************************************************************
        //  method:                 public void Reset()
        //  purpose:                return the ground Dictionary<int, int> to its original state when Generate() was called
        //  parameters:             none
        //  returns:                nothing
        // ****************************************************************************************************************
        public void Reset()
        {
            foreach (int k in originals.Keys)
                ground[k] = originals[k];

            originals.Clear();
        }

        // *********************************************************************************************
        //  method:                 public void Draw()
        //  purpose:                draws ground onto the background buffer of the shared CDrawer canvas
        //  parameters:             none
        //  returns:                nothing
        // *********************************************************************************************
        public void Draw()
        {
            for (int i = 0; i < Width; ++i)
            {
                int height = this[ProjectToKey(i)];
                for (int j = 0; j < height; ++j)
                    Canvas.SetBBPixel(i, CDrawerGeometry.Functions.TransformOriginToFromTopLeft(Canvas, j), GetColour(i, j));
            }
        }

        // ***********************************************************************************************
        //  method:                 public void Impact(Missile m, int radius)
        //  purpose:                update the heights of the rectangles affected by the collision between
        //                          the ground and a computer missile
        //  parameters:             Missile m
        //                          int radius
        //  returns:                nothing
        // ***********************************************************************************************
        public void Impact(Point center, int radius)
        {
            if (radius < 0)
                throw new ArgumentOutOfRangeException("radius cannot be negative!");

            // get upper and lower key for iterating over
            int xLower = ProjectToKey(Math.Max(center.X - radius, 0));
            int xUpper = ProjectToKey(Math.Min(center.X + radius, Canvas.m_ciWidth - 1));

            for (int x = xLower; x <= xUpper; x += HScale)
            {
                if (!originals.ContainsKey(x))
                    originals[x] = this[x];

                this[x] = center.Y - (int)Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(x - center.X, 2));
            }

            DrawImpact(center, radius);
        }

        // **************************************************************************************************************************
        //  method:                 public bool IsCollidingWith(Point center, int radius)
        //  purpose:                checks if the circle defined by the arguments is colliding with any of the ground surface,
        //                          by checking the heights of the keys in the range of the circle's width to the range of its height
        //  parameters:             Point center
        //                          int radius
        //  returns:                bool of collision
        // **************************************************************************************************************************
        public bool IsCollidingWith(Point center, int radius)
        {
            if (radius < 0)
                throw new ArgumentException("radius cannot be negative!");

            int upperKey = ProjectToKey(center.X + radius);

            bool collision = false;
            for (int i = ProjectToKey(center.X - radius); i <= upperKey; i += HScale)
                if (ground.ContainsKey(i) && this[i] >= center.Y - radius)
                {
                    collision = true;
                    break;
                }

            return collision;
        }

        // ***********************************************************************************************
        //  method:                 public List<Point> GetRandomSurfacePoints(int locationAmount)
        //  purpose:                generate a list of random points representing the height of the ground
        //                          from the respective keys in the ground dictionary
        //  parameters:             int locationAmount
        //  returns:                List<Point>
        // ***********************************************************************************************
        public List<Point> GetRandomSurfacePoints(int locationAmount)
        {
            List<Point> sources = new List<Point>();

            for (int i = 0; i < locationAmount; ++i)
            {
                int x = ProjectToKey(_rng.Next(0, Width));
                int y = ground[x];
                sources.Add(new Point(x, y));
            }

            return sources;
        }

        #endregion
        #region private functions

        // ****************************************************************************************
        //  method:                 private int ProjectToKey(int x)
        //  purpose:                projects a given x-coordinate to the key representing it in the
        //                          ground dictionary
        //  parameters:             int x
        //  returns:                int of left-most value of rectangle which x is part of
        // ****************************************************************************************
        private int ProjectToKey(int x)
        {
            return (x / HScale) * HScale;
        }

        // ************************************************************************************************************
        //  method:                 private Color GetColour(int x, int y)
        //  purpose:                based on the current ColourFn state, apply the respective Color generating function
        //  parameters:             int x
        //                          int y
        //  returns:                Color returned from applying appropriate colour generating function to x, y
        // ************************************************************************************************************
        private Color GetColour(int x, int y)
        {
            Color top;
            Color bottom;

            switch (CFn)
            {
                case ColourFunction.DopedBrownGrey:
                    bottom = BASE_BROWN;
                    top = BASE_GREY;
                    break;
                case ColourFunction.DopedGreenBrown:
                    bottom = BASE_GREEN;
                    top = BASE_BROWN;
                    break;
                default:
                    bottom = BASE_GREEN;
                    top = BASE_GREY;
                    break;
            }

            return DopedDualColourFn(x, y, Height / DOPED_MID_LOWER_SCALE, Height / DOPED_MID_UPPER_SCALE, bottom, top);
        }

        // ***************************************************************************************************************
        //  method:                 private Color DopedDualColourFn(
        //                              int x, int y, int middleLowerBound, int middleUpperBound, Color bottom, Color top)
        //  purpose:                colour generating function for all the DopedXY ColourFn states
        //  parameters:             int x
        //                          int y
        //                          int middleLowerBound (lowest height for both colours to appear)
        //                          int middleUpperBound (height height for both colours to appear)
        //                          Color bottom
        //                          Color top
        //  returns:                Color
        // ***************************************************************************************************************
        private Color DopedDualColourFn(int x, int y, int middleLowerBound, int middleUpperBound, Color bottom, Color top)
        {
            if (y < _rng.Next(middleLowerBound, middleUpperBound))
                return Color.FromArgb(
                    bottom.R + _rng.Next(-COLOR_DIFFERENTIAL, COLOR_DIFFERENTIAL),
                    bottom.G + _rng.Next(-COLOR_DIFFERENTIAL, COLOR_DIFFERENTIAL),
                    bottom.B + _rng.Next(-COLOR_DIFFERENTIAL, COLOR_DIFFERENTIAL));
            return Color.FromArgb(
                top.R + _rng.Next(-COLOR_DIFFERENTIAL, COLOR_DIFFERENTIAL),
                top.G + _rng.Next(-COLOR_DIFFERENTIAL, COLOR_DIFFERENTIAL),
                top.B + _rng.Next(-COLOR_DIFFERENTIAL, COLOR_DIFFERENTIAL));
        }

        // ************************************************************************************************************
        //  method:                 private void DrawImpact(Missile m, int radius)
        //  purpose:                sets the background pixels in the circle defined by Current and the given radius to
        //                          the impact color; used for when a computer missile collides with the ground surface
        //  parameters:             int radius
        //  returns:                nothing
        // ************************************************************************************************************
        private void DrawImpact(Point center, int radius)
        {
            // get bounding box around circle
            int lowerX = Math.Max(center.X - radius, 0);
            int upperX = Math.Min(center.X + radius, Width);
            int lowerY = Math.Max(center.Y - radius, 0);
            int upperY = Math.Min(center.Y + radius, Height);

            // only colour pixels in the bounding box which intersect with the circle (perfect collision)
            for (int x = lowerX; x < upperX; ++x)
                for (int y = lowerY; y < upperY; ++y)
                    if (CDrawerGeometry.Functions.IsPointOnCircle(new Point(x, y), center, radius))
                        Canvas.SetBBPixel(x, CDrawerGeometry.Functions.TransformOriginToFromTopLeft(Canvas, y), IMPACT_COLOR);
        }

        #endregion
    }
}
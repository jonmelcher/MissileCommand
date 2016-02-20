// *****************************************************************************
//  L02.cs          -   contains the L02 Form class for use with CMPE2300 Lab 02
//                      acts as the Form GUI for the game MissileCommand
//
//  dependencies    -   MissileGeometry.cs -> CDrawerGeometry.cs
//                      MissileCommand.cs -> CDrawerGeometry.cs
//                      Ground.cs -> CDrawerGeometry.cs
//                      
//  written by Jonathan Melcher for CMPE2300 on 06/10/2015
//  last updated 21/10/2015
// *****************************************************************************

using System;
using System.Windows.Forms;


namespace CMPE2300JonathanMelcherL02
{
    // **********************************************************************************************************
    //  class:          L02
    //  purpose:        act as a Windows Forms GUI for the game MissileCommand
    //                  handles all mouse inputs from user on L02 as well as the CDrawer canvas of MissileCommand
    //                  monitors statistics for each game instance, and modifiable game settings
    // **********************************************************************************************************
    public partial class L02 : Form
    {
        // ****************************************************
        //  constructor:            public L02()
        //  purpose:                initialioze Form components
        // ****************************************************
        public L02()
        {
            InitializeComponent();
        }

        // *****************************************************************************************************
        //  event:                  private void L02_Load(object sender, EventArgs e)
        //  purpose:                set-up initial game settings based on defaults from their respective classes
        // *****************************************************************************************************
        private void L02_Load(object sender, EventArgs e)
        {
            explosionRadiusUI.Value = Missile.ExplosionRadius;
            explosionRadiusUI.Maximum = Missile.MaxRadius;
            explosionRadiusUI.Minimum = Missile.MinRadius;
            terrainVerticalScaleUI.Value = Ground.VScale;
            terrainVerticalScaleUI.Maximum = Ground.MaxVScale;
            terrainVerticalScaleUI.Minimum = Ground.MinVScale;
            incomingMissileUI.Value = MissileCommand.ComputerMissileMax;
            incomingMissileUI.Maximum = MissileCommand.UserMissileMax;
            incomingMissileUI.Minimum = 0;
        }

        // *****************************************************************************************************
        //  event:                  private void startUI_Click(object sender, EventArgs e)
        //  purpose:                communicate with MissileCommand to start a new game, starting the tick timer
        //                          on the form
        // *****************************************************************************************************
        private void startUI_Click(object sender, EventArgs e)
        {
            MissileCommand.StartNewGame();
            timer.Start();
        }

        // *******************************************************************************
        //  event:                  private void pauseUI_Click(object sender, EventArgs e)
        //  purpose:                relay to MissileCommand that the game should be paused
        // *******************************************************************************
        private void pauseUI_Click(object sender, EventArgs e)
        {
            MissileCommand.Paused = !MissileCommand.Paused;
        }

        // *********************************************************************************
        //  event:                  private void restartUI_Click(object sender, EventArgs e)
        //  purpose:                restart the game through MissileCommand
        // *********************************************************************************
        private void restartUI_Click(object sender, EventArgs e)
        {
            MissileCommand.RestartGame();
        }

        // ****************************************************************************************************
        //  event:                  private void timer_Tick(object sender, EventArgs e)
        //  purpose:                the current running game ticks on the form tick; calculate next game state,
        //                          update the statistics, and stop the clock if the game has finished
        // ****************************************************************************************************
        private void timer_Tick(object sender, EventArgs e)
        {
            MissileCommand.Tick();
            UpdateStatistics();

            if (!MissileCommand.Running)
                timer.Stop();
        }


        // ************************************************************************************************
        //  event:                  private void explosionRadiusUI_ValueChanged(object sender, EventArgs e)
        //  purpose:                update the MissileGeometry class on the changed value
        //  notes:                - see Load event for ranges
        // ************************************************************************************************
        private void explosionRadiusUI_ValueChanged(object sender, EventArgs e)
        {
            Missile.ExplosionRadius = (int)explosionRadiusUI.Value;
        }

        // *************************************************************************************************
        //  event:                  private void incomingMissilesUI_ValueChanged(object sender, EventArgs e)
        //  purpose:                update the MissileCommand class on the changed value
        //  notes:                - see Load event for ranges
        // *************************************************************************************************
        private void incomingMissilesUI_ValueChanged(object sender, EventArgs e)
        {
            MissileCommand.ComputerMissileMax = (int)incomingMissileUI.Value;
        }

        // *****************************************************************************************************
        //  event:                  private void terrainVerticalScaleUI_ValueChanged(object sender, EventArgs e)
        //  purpose:                update the Ground class on the changed value
        //  notes:                - see Load event for ranges
        // *****************************************************************************************************
        private void terrainVerticalScaleUI_ValueChanged(object sender, EventArgs e)
        {
            Ground.VScale = (int)terrainVerticalScaleUI.Value;
        }

        // ************************************************************************************
        //  method:                 private void UpdateStatistics()
        //  purpose:                retrieve new values from MissileCommand class and calculate
        //                          and display new statistics of the current game
        //  parameters:             none
        //  returns:                nothing
        // ************************************************************************************
        private void UpdateStatistics()
        {
            incomingMissileTotalUI.Text = MissileCommand.Incoming.ToString();
            incomingMissileDestroyedUI.Text = MissileCommand.IncomingDestroyed.ToString();
            missileLaunchedUI.Text = MissileCommand.Launched.ToString();
            killRatioUI.Text = (MissileCommand.Launched == 0) ?
                "Unknown" : ((1.0 * MissileCommand.IncomingDestroyed) / MissileCommand.Launched).ToString("f3");
        }
    }
}
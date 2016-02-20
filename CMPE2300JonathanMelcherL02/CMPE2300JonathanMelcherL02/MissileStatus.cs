// ***********************************************************************************
//  MissileStatus.cs    -   contains struct MissileStatus for use with CMPE2300 Lab 02
//
//  Written by Jonathan Melcher on 28/10/2015
//  Last updated 30/10/2015
// ***********************************************************************************


namespace CMPE2300JonathanMelcherL02
{
    // ****************************************************************************
    //  enum:           enum MissileTeam
    //  purpose:        distinguish between the different players in MissileCommand
    // ****************************************************************************
    enum MissileTeam { User, Computer }

    // ****************************************************************************
    //  enum:           enum MissileState
    //  purpose:        distinguish between behaviours of Missile in MissileCommand
    // ****************************************************************************
    enum MissileState { Moving, Exploding, Diminishing, Dead }

    // ***************************************************************************
    //  enum:           enum MissileDeath
    //  purpose:        distinguish between how a Missile 'died' in MissileCommand
    // ***************************************************************************
    enum MissileDeath { None, Ground, Scoring, NonScoring, OutOfBounds }

    // *************************************************************************************************
    //  struct:         struct MissileStatus
    //  purpose:        provides an immutable object encapsulating MissileTeam/MissileState/MissileDeath
    // *************************************************************************************************
    struct MissileStatus
    {
        public readonly MissileTeam Team;
        public readonly MissileState State;
        public readonly MissileDeath Death;

        public MissileStatus(MissileTeam t, MissileState s, MissileDeath d)
        {
            Team = t;
            State = s;
            Death = d;
        }

        public MissileStatus(MissileStatus old, MissileState newState) : this(old.Team, newState, old.Death) { }
        public MissileStatus(MissileStatus old, MissileDeath newDeath) : this(old.Team, old.State, newDeath) { }
        public MissileStatus(MissileStatus old, MissileState newState, MissileDeath newDeath) : this(old.Team, newState, newDeath) { }
    }
}
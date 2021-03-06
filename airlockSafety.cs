/*

Pressure Safe Airlocks Written by Adam Laycock
Run once per second with a timer block to ensure that all your airlocks stay safe

https://github.com/Arcath/SpaceEngineersScripts

*/

/*
  Air Lock Definitions

  each airlock needs an entry in this array that looks like this:

    new string[]
    {
      "Outer Door Name",
      "Inner Door Name",
      "Air Vent Name"
    },
*/
public static string[][] AIRLOCKS =
{
  new string[]
  {
    "HA Outer Door",
    "HA Inner Door",
    "AV Hangar Air Lock"
  },
  new string[]
  {
    "LA Outer Door",
    "LA Inner Door",
    "AV Left Airlock"
  },
  new string[]
  {
    "RA Outer Door",
    "RA Inner Door",
    "AV Right Airlock"
  },
};


// The number of Timer Block cycles required to close a door
public static int CYCLES_TO_CHANGE_STATE = 1;

// DO NOT EDIT BELOW THIS LINE
public static int DOOR_CYCLE = 0;

void handleAirlock(string[] airlock) {
  IMyDoor outerDoor = (IMyDoor)GridTerminalSystem.GetBlockWithName(airlock[0]);
  IMyDoor innerDoor = (IMyDoor)GridTerminalSystem.GetBlockWithName(airlock[1]);
  IMyAirVent airVent = (IMyAirVent)GridTerminalSystem.GetBlockWithName(airlock[2]);

  bool oxygenStatus = (airVent.IsPressurized() && airVent.GetOxygenLevel() != 0);

  if(oxygenStatus)
  {
    innerDoor.GetActionWithName("OnOff_On").Apply(innerDoor);
    outerDoor.GetActionWithName("OnOff_Off").Apply(outerDoor);
  }
  else
  {
    if(innerDoor.Open)
    {
      innerDoor.GetActionWithName("Open_Off").Apply(innerDoor);
      DOOR_CYCLE = CYCLES_TO_CHANGE_STATE;
    }
    else
    {
      if(DOOR_CYCLE == 0)
      {
        innerDoor.GetActionWithName("OnOff_Off").Apply(innerDoor);
      }
      else
      {
        DOOR_CYCLE -= 1;
      }
    }
    outerDoor.GetActionWithName("OnOff_On").Apply(outerDoor);
  }
}

void Main()
{
  for (int i = 0; i < AIRLOCKS.Length; i++)
  {
    handleAirlock(AIRLOCKS[i]);
  }
}

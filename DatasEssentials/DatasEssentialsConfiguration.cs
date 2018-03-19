using System;
using Rocket.API;
using System.Collections.Generic;

namespace datathegenius.DatasEssentials
{
    public class DatasEssentialsConfiguration : IRocketPluginConfiguration
    {
        //General
        public Boolean enabled;
        public List<string> permissionsAddOnConnect;
        public List<string> permissionsRemoveOnConnect;
        public int bedWaitTime;
        public Boolean announceAdmin;
        public int pollDelayTime;
        public int clearVehicleFrequency;
        public ushort clearVehicleID;

        //Mod
        public List<string> modList;
        public List<string> modKickBanList;
        public Boolean announceModActive;
        public uint maxBanLength;
        public string activeModMessage;

        public void LoadDefaults()
        {
            enabled = true;
            permissionsAddOnConnect = new List<string>() { "Guest" };
            permissionsRemoveOnConnect = new List<string>() { "EventGroup", "Moderator" };
            bedWaitTime = 5;
            announceAdmin = true;
            pollDelayTime = 15;
            clearVehicleFrequency = 43200;
            clearVehicleID = 134;

            modList = new List<string>();
            modKickBanList = new List<string>();
            announceModActive = true;
            maxBanLength = 10800;
            activeModMessage = "You have access to these commands: spy, mute, kick, ban (" + maxBanLength + " seconds max)";
        }
    }
}

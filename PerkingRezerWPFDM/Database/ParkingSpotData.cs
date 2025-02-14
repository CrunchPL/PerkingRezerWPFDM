using System.Collections.Generic;
using PerkingRezerWPFDM.Models;

namespace PerkingRezerWPFDM.Database
{
    public static class ParkingSpotData
    {
        public static List<ParkingSpot> GetAllSpots()
        {
            var spots = new List<ParkingSpot>();
            string[] sectors = { "A", "B", "C", "D", "E" };

            foreach (var sector in sectors)
            {
                for (int i = 1; i <= 13; i++)
                {
                    spots.Add(new ParkingSpot { SpotNumber = $"{sector}{i}", IsOccupied = true });
                }
            }
            return spots;
        }
    }
}

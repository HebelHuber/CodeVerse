using CodeVerse.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeVerse.Logic
{
    public interface ICanSimulate
    {
        void GenerateMap(int seed, bool debugmode);
        // Initializer, same seed should produce the same map
        // also, debugmode gets set here

        bool IsDebugMode();
        // return true if there is more acces to entities than wanted.
        // on competitive games this should return false,
        // otherwise the simulator is "open"

        List<Entity> GetDebugEntities();
        // this should return all entities, but only in debugmode,
        // Throws if debug is disabled

        List<ScannerContent> Simulate(List<PlayerCommand> input = null);
        // Main tick, get player commands, simulate and return Scannercontent of the player's ships
    }
}

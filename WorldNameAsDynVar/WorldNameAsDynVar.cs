using HarmonyLib;
using ResoniteModLoader;

namespace WorldNameAsDynVar;

public class WorldNameAsDynVar : ResoniteMod
{
    public override string Name => nameof(WorldNameAsDynVar);
    public override string Author => "jvyden";
    public override string Version => typeof(WorldNameAsDynVar).Assembly.GetName().Version?.ToString() ?? "0.0.0";
    public override string Link => "https://github.com/jvyden/" + nameof(WorldNameAsDynVar);
    
    public static ModConfiguration? Config { get; private set; }
    
    public override void OnEngineInit()
    {
        Harmony harmony = new("xyz.jvyden." + nameof(WorldNameAsDynVar));
        Config = GetConfiguration();
        Config?.Save(true);
        harmony.PatchAll();
    }
}
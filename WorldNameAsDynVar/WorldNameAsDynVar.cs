using FrooxEngine;
using ResoniteModLoader;

namespace WorldNameAsDynVar;

public class WorldNameAsDynVar : ResoniteMod
{
    public override string Name => nameof(WorldNameAsDynVar);
    public override string Author => "jvyden";
    public override string Version => typeof(WorldNameAsDynVar).Assembly.GetName().Version?.ToString() ?? "0.0.0";
    public override string Link => "https://github.com/jvyden/" + nameof(WorldNameAsDynVar);
    
    public override void OnEngineInit()
    {
        Engine.Current.OnReady += () =>
        {
            Engine.Current.WorldManager.WorldAdded += OnWorldAdded;
        };
    }

    private static void OnWorldAdded(World world)
    {
        if (!world.IsAuthority)
            return;

        world.Coroutines.RunInUpdates(0, () =>
        {
            DynamicValueVariable<string> dynVar = world.RootSlot.GetComponent<DynamicValueVariable<string>>(c => c.VariableName.Value == "World/Name");
            if (dynVar == null)
            {
                dynVar = world.RootSlot.AttachComponent<DynamicValueVariable<string>>();
                dynVar.VariableName.Value = "World/Name";
                dynVar.OverrideOnLink.Value = true;
            }

            dynVar.Value.Value = world.Name;

            world.Configuration.WorldName.OnValueChange += WorldNameOnValueChange;
            dynVar.Value.OnValueChange += DynVarOnValueChange;
        });
    }

    private static void DynVarOnValueChange(SyncField<string> dynVarValue)
    {
        if (!dynVarValue.World.IsAuthority)
            return;

        dynVarValue.World.Configuration.WorldName.Value = dynVarValue.Value;
    }

    private static void WorldNameOnValueChange(SyncField<string> worldName)
    {
        if (!worldName.World.IsAuthority)
            return;

        worldName.World.RootSlot.WriteDynamicVariable("World/Name", worldName.Value);
    }
}
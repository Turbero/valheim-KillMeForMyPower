using KillMeForMyPower.Managers;

namespace KillMeForMyPower.Commands
{
    public class PowerManagementCommands
    {
        public static void RegisterConsoleCommand()
        {
            new Terminal.ConsoleCommand("add_power", "[player] [boss_name]", args =>
            {
                if (!GameManager.IsAdmin(Player.m_localPlayer))
                {
                    args.Context.AddString("You are not an admin.");
                    return;
                }
                
                if (args.Args.Length < 3)
                {
                    args.Context.AddString("Usage: add_power <name> <boss_name>");
                    return;
                }

                ZRoutedRpc.instance.InvokeRoutedRPC(0L, "RPC_BossPowerGrantServer", args.Args[2], args.Args[1]);
            });
            new Terminal.ConsoleCommand("remove_power", "[boss_order] [player]", args =>
            {
                if (!GameManager.IsAdmin(Player.m_localPlayer))
                {
                    args.Context.AddString("You are not an admin.");
                    return;
                }
                
                if (args.Args.Length < 3)
                {
                    args.Context.AddString("Usage: add_power <name> <boss_name>");
                    return;
                }
                
                ZRoutedRpc.instance.InvokeRoutedRPC(0L, "RPC_BossPowerRemoveGrantServer", args.Args[2], args.Args[1]);
            });

        }
    }
}
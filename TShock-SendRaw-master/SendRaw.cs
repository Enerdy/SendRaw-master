using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.DB;
using System.ComponentModel;


namespace PluginTemplate
{
	[ApiVersion(1, 15)]
	public class PluginTemplate : TerrariaPlugin
	{
		public override string Name
		{
			get { return "SendRaw"; }
		}
		public override string Author
		{
			get { return "Efreak, updated by Enerdy"; }
		}
		public override string Description
		{
			get { return "Impersonation of other users and raw broadcasts"; }
		}
		public override Version Version
		{
			get { return Assembly.GetExecutingAssembly().GetName().Version; }
		}

		public override void Initialize()
		{
            Commands.ChatCommands.Add(new Command("sendraw", SendRa, "sendraw"));
            Commands.ChatCommands.Add(new Command("sendcolor", SendColor, "sendcolor"));
            Commands.ChatCommands.Add(new Command("sendrgb", SendRGB, "sendrgb"));
            Commands.ChatCommands.Add(new Command("sendas", SendAs, "sendas"));
            Commands.ChatCommands.Add(new Command("sendto", SendTo, "sendto"));
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
		public PluginTemplate(Main game)
			:base(game)
		{
		}


		public static void SendAs(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /sendas <player> [message]");
                Log.ConsoleError(string.Format("{0} failed to execute /sendas: Invalid syntax.", args.Player.Name));
				return;
			}
			if (args.Parameters[0].Length == 0)
			{
				args.Player.SendErrorMessage("Missing player name");
                Log.ConsoleError(string.Format("{0} failed to execute /sendas: Missing player name.", args.Player.Name));
				return;
			}

			string plStr = args.Parameters[0];
			var players = TShock.Utils.FindPlayer(plStr);
			if (players.Count == 0)
			{
				args.Player.SendErrorMessage("Invalid player!");
                Log.ConsoleError(string.Format("{0} failed to execute /sendas: Invalid player.", args.Player.Name));
                return;
			}
			if (players.Count > 1)
			{
				var plrMatches = "";
				foreach (TSPlayer plr in players)
				{
					if (plrMatches.Length != 0)
					{
						plrMatches += ", " + plr.Name;
					}
					else
					{
						plrMatches += plr.Name;
					}
				}
				args.Player.SendErrorMessage("More than one player matched! Matches: " + plrMatches);
                Log.ConsoleError(string.Format("{0} failed to execute /sendas: More than one player matched.", args.Player.Name));
                return;
			}
			string message = players[0].Group.Prefix + players[0].Name + players[0].Group.Suffix + ": ";
			for (int i = 1; i < args.Parameters.Count; i++)
			{
				message += args.Parameters[i] + ((i == args.Parameters.Count - 1) ? "" : " ");
			}

			Color messagecolor = new Color(players[0].Group.R, players[0].Group.G, players[0].Group.B);
			TSPlayer.All.SendMessage(message, messagecolor);
            Log.ConsoleInfo(args.Player.Name + " sent \"" + message + "\" to the server as " + players[0].Name + "'s Message.");
		}

        private static void SendTo(CommandArgs args)
        {
            if (args.Parameters.Count < 2)
            {
                args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /sendto <player> <message>");
                Log.ConsoleError(string.Format("{0} failed to execute /sendto: Invalid syntax.", args.Player.Name));
                return;
            }
            string message = "";
            for (int i = 1; i < args.Parameters.Count; i++)
            {
                message += args.Parameters[i] + ((i == args.Parameters.Count - 1) ? "" : " ");
            }
            List<TSPlayer> plr = new List<TSPlayer>();
            foreach (TSPlayer player in TShock.Utils.FindPlayer(args.Parameters[0]))
            {
                plr.Add(player);
            }
            if (plr.Count > 1)
            {
                string plrlist = "";
                int count = 0;
                foreach (TSPlayer player in plr)
                {
                    count++;
                    plrlist += player.Name + ((plr.Count > count) ? ", " : "");
                }
                args.Player.SendErrorMessage("More than one player matched! Matches: " + plrlist);
                Log.ConsoleError(string.Format("{0} failed to execute /sendto: More than one player matched.", args.Player.Name));
                return;
            }
            else if (plr.Count < 1)
            {
                args.Player.SendErrorMessage("Invalid player!");
                Log.ConsoleError(string.Format("{0} failed to execute /sendto: Invalid player.", args.Player.Name));
                return;
            }
            plr[0].SendSuccessMessage(message);
            Log.ConsoleInfo(args.Player.Name + " sent \"" + message + "\" to " + plr[0].Name + " as a SuccessMessage.");
        }

		public static void SendRa(CommandArgs args)
		{
			if (args.Parameters.Count < 1)
			{
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /sendraw [something to send]");
                Log.ConsoleError(string.Format("{0} failed to execute /sendraw: Invalid syntax.", args.Player.Name));
				return;
			}
			string message = "";
			for (int i = 0; i < args.Parameters.Count; i++)
			{
                message += args.Parameters[i] + ((i == args.Parameters.Count - 1) ? "" : " ");
			}

            TSPlayer.All.SendInfoMessage(message);
            Log.ConsoleInfo(args.Player.Name + " sent \"" + message + "\" to the server as an InfoMessage.");
			
            return;
		}
		public static void SendColor(CommandArgs args) //start new command for built-in colors
		{
			if(args.Parameters.Count < 2)
			{
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /sendcolor [colorname] <message>");
                Log.ConsoleError(string.Format("{0} failed to execute /sendcolor: Invalid syntax.", args.Player.Name));
				return;
			}
			string message = "";
			for (int i = 1; i < args.Parameters.Count; i++)
			{
                message += args.Parameters[i] + ((i == args.Parameters.Count - 1) ? "" : " ");
			}
            string colorname = "" + args.Parameters[0];
            Color color = ColorFromName(colorname);
            if (color != new Color(1, 1, 1))
            {
                TSPlayer.All.SendMessage(message, color);
                Log.ConsoleInfo(args.Player.Name + " sent \"" + message + string.Format("\" to the server as a Message (Color: {0}).", color.ToString()));
            }
            else
            {
                args.Player.SendErrorMessage("Invalid color!");
                Log.ConsoleError(string.Format("{0} failed to execute /sendcolor: Invalid color.", args.Player.Name));
            }
            return;
		}
		public static void SendRGB(CommandArgs args) //start new command for custom colors by RGB
		{
			if(args.Parameters.Count < 4)
			{
				args.Player.SendErrorMessage("Invalid syntax! Proper syntax: /sendcolor [Red] [Green] [Blue] [message]. Use 0-255 for RGB values");
                Log.ConsoleError(string.Format("{0} failed to execute /sendrgb: Invalid syntax.", args.Player.Name));
                return;
			}
			string message = "";
			for (int i = 3; i < args.Parameters.Count; i++)
			{
                message += args.Parameters[i] + ((i == args.Parameters.Count - 1) ? "" : " ");
			}

			byte R=Convert.ToByte(args.Parameters[0],10);
			byte G=Convert.ToByte(args.Parameters[1],10);
			byte B=Convert.ToByte(args.Parameters[2],10);
            Color color = new Color(R, G, B);
			TSPlayer.All.SendMessage(message, color);
            Log.ConsoleInfo(args.Player.Name + " sent \"" + message + string.Format("\" to the server as a Message (RGB: {0},{1},{2}).", R, G, B));

            return;
		}
		public static Color ColorFromName(string name) //sigh...you guys removed this
		{
            if (name.ToLower() == "aliceblue") return Color.AliceBlue;
            else if (name.ToLower() == "antiquewhite") return Color.AntiqueWhite;
            else if (name.ToLower() == "aqua") return Color.Aqua;
            else if (name.ToLower() == "aquamarine") return Color.Aquamarine;
            else if (name.ToLower() == "azure") return Color.Azure;
            else if (name.ToLower() == "beige") return Color.Beige;
            else if (name.ToLower() == "bisque") return Color.Bisque;
            else if (name.ToLower() == "black") return Color.Black;
            else if (name.ToLower() == "blanchedalmond") return Color.BlanchedAlmond;
            else if (name.ToLower() == "blue") return Color.Blue;
            else if (name.ToLower() == "blueviolet") return Color.BlueViolet;
            else if (name.ToLower() == "brown") return Color.Brown;
            else if (name.ToLower() == "burlywood") return Color.BurlyWood;
            else if (name.ToLower() == "cadetblue") return Color.CadetBlue;
            else if (name.ToLower() == "chartreuse") return Color.Chartreuse;
            else if (name.ToLower() == "chocolate") return Color.Chocolate;
            else if (name.ToLower() == "coral") return Color.Coral;
            else if (name.ToLower() == "cornflowerblue") return Color.CornflowerBlue;
            else if (name.ToLower() == "cornsilk") return Color.Cornsilk;
            else if (name.ToLower() == "crimson") return Color.Crimson;
            else if (name.ToLower() == "cyan") return Color.Cyan;
            else if (name.ToLower() == "darkblue") return Color.DarkBlue;
            else if (name.ToLower() == "darkcyan") return Color.DarkCyan;
            else if (name.ToLower() == "darkgoldenrod") return Color.DarkGoldenrod;
            else if (name.ToLower() == "darkgray") return Color.DarkGray;
            else if (name.ToLower() == "darkgreen") return Color.DarkGreen;
            else if (name.ToLower() == "darkkhaki") return Color.DarkKhaki;
            else if (name.ToLower() == "darkmagenta") return Color.DarkMagenta;
            else if (name.ToLower() == "darkolivegreen") return Color.DarkOliveGreen;
            else if (name.ToLower() == "darkorange") return Color.DarkOrange;
            else if (name.ToLower() == "darkorchid") return Color.DarkOrchid;
            else if (name.ToLower() == "darkred") return Color.DarkRed;
            else if (name.ToLower() == "darksalmon") return Color.DarkSalmon;
            else if (name.ToLower() == "darkseagreen") return Color.DarkSeaGreen;
            else if (name.ToLower() == "darkslateblue") return Color.DarkSlateBlue;
            else if (name.ToLower() == "darkslategray") return Color.DarkSlateGray;
            else if (name.ToLower() == "darkturquoise") return Color.DarkTurquoise;
            else if (name.ToLower() == "darkviolet") return Color.DarkViolet;
            else if (name.ToLower() == "deeppink") return Color.DeepPink;
            else if (name.ToLower() == "deepskyblue") return Color.DeepSkyBlue;
            else if (name.ToLower() == "dimgray") return Color.DimGray;
            else if (name.ToLower() == "dodgerblue") return Color.DodgerBlue;
            else if (name.ToLower() == "firebrick") return Color.Firebrick;
            else if (name.ToLower() == "floralwhite") return Color.FloralWhite;
            else if (name.ToLower() == "forestgreen") return Color.ForestGreen;
            else if (name.ToLower() == "fuchsia") return Color.Fuchsia;
            else if (name.ToLower() == "gainsboro") return Color.Gainsboro;
            else if (name.ToLower() == "ghostwhite") return Color.GhostWhite;
            else if (name.ToLower() == "gold") return Color.Gold;
            else if (name.ToLower() == "goldenrod") return Color.Goldenrod;
            else if (name.ToLower() == "grey") return Color.Gray;
            else if (name.ToLower() == "gray") return Color.Gray;
            else if (name.ToLower() == "green") return Color.Green;
            else if (name.ToLower() == "greenyellow") return Color.GreenYellow;
            else if (name.ToLower() == "honeydew") return Color.Honeydew;
            else if (name.ToLower() == "hotpink") return Color.HotPink;
            else if (name.ToLower() == "indianred") return Color.IndianRed;
            else if (name.ToLower() == "indigo") return Color.Indigo;
            else if (name.ToLower() == "ivory") return Color.Ivory;
            else if (name.ToLower() == "khaki") return Color.Khaki;
            else if (name.ToLower() == "lavender") return Color.Lavender;
            else if (name.ToLower() == "lavenderblush") return Color.LavenderBlush;
            else if (name.ToLower() == "lawngreen") return Color.LawnGreen;
            else if (name.ToLower() == "lemonchelse iffon") return Color.LemonChiffon;
            else if (name.ToLower() == "lightblue") return Color.LightBlue;
            else if (name.ToLower() == "lightcoral") return Color.LightCoral;
            else if (name.ToLower() == "lightcyan") return Color.LightCyan;
            else if (name.ToLower() == "lightgoldenrodyellow") return Color.LightGoldenrodYellow;
            else if (name.ToLower() == "lightgray") return Color.LightGray;
            else if (name.ToLower() == "lightgreen") return Color.LightGreen;
            else if (name.ToLower() == "lightpink") return Color.LightPink;
            else if (name.ToLower() == "lightsalmon") return Color.LightSalmon;
            else if (name.ToLower() == "lightseagreen") return Color.LightSeaGreen;
            else if (name.ToLower() == "lightskyblue") return Color.LightSkyBlue;
            else if (name.ToLower() == "lightslategray") return Color.LightSlateGray;
            else if (name.ToLower() == "lightsteelblue") return Color.LightSteelBlue;
            else if (name.ToLower() == "lightyellow") return Color.LightYellow;
            else if (name.ToLower() == "lime") return Color.Lime;
            else if (name.ToLower() == "limegreen") return Color.LimeGreen;
            else if (name.ToLower() == "linen") return Color.Linen;
            else if (name.ToLower() == "magenta") return Color.Magenta;
            else if (name.ToLower() == "maroon") return Color.Maroon;
            else if (name.ToLower() == "mediumaquamarine") return Color.MediumAquamarine;
            else if (name.ToLower() == "mediumblue") return Color.MediumBlue;
            else if (name.ToLower() == "mediumorchid") return Color.MediumOrchid;
            else if (name.ToLower() == "mediumpurple") return Color.MediumPurple;
            else if (name.ToLower() == "mediumseagreen") return Color.MediumSeaGreen;
            else if (name.ToLower() == "mediumslateblue") return Color.MediumSlateBlue;
            else if (name.ToLower() == "mediumspringgreen") return Color.MediumSpringGreen;
            else if (name.ToLower() == "mediumturquoise") return Color.MediumTurquoise;
            else if (name.ToLower() == "mediumvioletred") return Color.MediumVioletRed;
            else if (name.ToLower() == "midnightblue") return Color.MidnightBlue;
            else if (name.ToLower() == "mintcream") return Color.MintCream;
            else if (name.ToLower() == "mistyrose") return Color.MistyRose;
            else if (name.ToLower() == "moccasin") return Color.Moccasin;
            else if (name.ToLower() == "navajowhite") return Color.NavajoWhite;
            else if (name.ToLower() == "navy") return Color.Navy;
            else if (name.ToLower() == "oldlace") return Color.OldLace;
            else if (name.ToLower() == "olive") return Color.Olive;
            else if (name.ToLower() == "olivedrab") return Color.OliveDrab;
            else if (name.ToLower() == "orange") return Color.Orange;
            else if (name.ToLower() == "orangered") return Color.OrangeRed;
            else if (name.ToLower() == "orchid") return Color.Orchid;
            else if (name.ToLower() == "palegoldenrod") return Color.PaleGoldenrod;
            else if (name.ToLower() == "palegreen") return Color.PaleGreen;
            else if (name.ToLower() == "paleturquoise") return Color.PaleTurquoise;
            else if (name.ToLower() == "palevioletred") return Color.PaleVioletRed;
            else if (name.ToLower() == "papayawhip") return Color.PapayaWhip;
            else if (name.ToLower() == "peachpuff") return Color.PeachPuff;
            else if (name.ToLower() == "peru") return Color.Peru;
            else if (name.ToLower() == "pink") return Color.Pink;
            else if (name.ToLower() == "plum") return Color.Plum;
            else if (name.ToLower() == "powderblue") return Color.PowderBlue;
            else if (name.ToLower() == "purple") return Color.Purple;
            else if (name.ToLower() == "red") return Color.Red;
            else if (name.ToLower() == "rosybrown") return Color.RosyBrown;
            else if (name.ToLower() == "royalblue") return Color.RoyalBlue;
            else if (name.ToLower() == "saddlebrown") return Color.SaddleBrown;
            else if (name.ToLower() == "salmon") return Color.Salmon;
            else if (name.ToLower() == "sandybrown") return Color.SandyBrown;
            else if (name.ToLower() == "seagreen") return Color.SeaGreen;
            else if (name.ToLower() == "seashell") return Color.SeaShell;
            else if (name.ToLower() == "sienna") return Color.Sienna;
            else if (name.ToLower() == "silver") return Color.Silver;
            else if (name.ToLower() == "skyblue") return Color.SkyBlue;
            else if (name.ToLower() == "slateblue") return Color.SlateBlue;
            else if (name.ToLower() == "slategray") return Color.SlateGray;
            else if (name.ToLower() == "snow") return Color.Snow;
            else if (name.ToLower() == "springgreen") return Color.SpringGreen;
            else if (name.ToLower() == "steelblue") return Color.SteelBlue;
            else if (name.ToLower() == "tan") return Color.Tan;
            else if (name.ToLower() == "teal") return Color.Teal;
            else if (name.ToLower() == "thistle") return Color.Thistle;
            else if (name.ToLower() == "tomato") return Color.Tomato;
            else if (name.ToLower() == "transparent") return Color.Transparent;
            else if (name.ToLower() == "turquoise") return Color.Turquoise;
            else if (name.ToLower() == "violet") return Color.Violet;
            else if (name.ToLower() == "wheat") return Color.Wheat;
            else if (name.ToLower() == "white") return Color.White;
            else if (name.ToLower() == "whitesmoke") return Color.WhiteSmoke;
            else if (name.ToLower() == "yellow") return Color.Yellow;
            else if (name.ToLower() == "yellowgreen") return Color.YellowGreen; 
            else return new Color(1, 1, 1);
		}
	}
}
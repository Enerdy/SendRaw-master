TShock-SendRaw
==============

Send raw commands and impersonate other users in tshock

What does it do?
==============
Sendraw allows you to broadcast raw messages to the server, without having them prefixed by (Server Broadcast). It also allows you to impersonate other users.

Permissions:
==============
sendraw - allows users to use /sendraw

sendcolor - allows users to use /sendcolor*

sendrgb - allows users to use /sendrgb*

sendas - allows users to use /sendas

Commands:
==============
/sendraw *message* - *message* is broadcast to the server as a normal message

/sendcolor **color** *message* - *message* is broadcast in the color specified. Valid colors are defined by the [C# color table](http://www.imgtoys.com/images/CSharpColorTable_1462D/CsharpColorTable.png) ([official docs](http://msdn.microsoft.com/en-us/library/system.drawing.color.aspx)). Primary and secondary colors are all in there.

/sendrgb* [R] [G] [B] [message] - [message] is broadcast in text color defined by rgb values.

/sendas **player name** *message* - *message* is broadcast to the server as though **player name** has said it, with the correct group color, prefix, and suffix

/sendto **player name** *message* - A SuccessMessage containing *message* is sent to player with **player name**

Changelog (Enerdy):
==============
**1.1.3:**
 - Added /sendto <player> <message> - Sends a success message to target player. Useful with ShortCommands and CmdAlias.
 - Added console logging every time a command is used successfully.

**1.1.2:**
 - Updated for Terraria v1.2.3.1 and TShock API v1.15
 - Rewrote outdated code for better compatibility
 - Minor: Removed incorrect spacing before the actual message
 - Minor: Colors are no longer case sensitive
 - Minor: SendAs now correctly displays a player's full name even if it is not fully written

To Do:
==============
Perhaps hardcode it so sendas can only be used by superadmins? Not sure.

~~A command to replace /sendwhite and /sendred that allows you to choose from some basic colors~~

~~A command to further customize the color by allowing you to choose the exact rgb for it.~~

Allow you to use sendas using players who are not currently logged in (I think this may require looking at the database, not sure).

**Send a note to the log every time this command is used.**

*You'll probably want to set up shortcommands for these commands with prefixes and whatnot, with individual permissions, rather than giving people permission to use this.
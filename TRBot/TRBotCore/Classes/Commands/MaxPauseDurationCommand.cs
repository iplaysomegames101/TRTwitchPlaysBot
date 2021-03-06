﻿using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Events;

namespace TRBot
{
    /// <summary>
    /// Sets the max pause duration to prevent resetting the game.
    /// </summary>
    public sealed class MaxPauseDurationCommand : BaseCommand
    {
        private int SetAccessLevel = (int)AccessLevels.Levels.Moderator;

        public override void Initialize(CommandHandler commandHandler)
        {
            base.Initialize(commandHandler);
        }

        public override void ExecuteCommand(object sender, OnChatCommandReceivedArgs e)
        {
            List<string> args = e.Command.ArgumentsAsList;

            if (args.Count == 0)
            {
                BotProgram.QueueMessage($"The max pause duration is {BotProgram.BotData.MaxPauseHoldDuration} milliseconds.");
                return;
            }

            User user = BotProgram.GetOrAddUser(e.Command.ChatMessage.Username, false);

            //Disallow setting the duration if the user doesn't have a sufficient access level
            if (user.Level < SetAccessLevel)
            {
                BotProgram.QueueMessage(CommandHandler.INVALID_ACCESS_MESSAGE);
                return;
            }

            if (args.Count > 1)
            {
                BotProgram.QueueMessage("Usage: \"duration (ms) (-1 to disable)\"");
                return;
            }

            string value = args[0];

            if (int.TryParse(value, out int duration) == false)
            {
                BotProgram.QueueMessage("Invalid value! Usage: \"duration (ms) (-1 to disable)\"");
                return;
            }

            if (duration < 0)
            {
                duration = -1;
            }

            BotProgram.BotData.MaxPauseHoldDuration = duration;
            BotProgram.SaveBotData();

            if (BotProgram.BotData.MaxPauseHoldDuration > 0)
            {
                BotProgram.QueueMessage($"Set max pause duration to {duration} milliseconds!");
            }
            else
            {
                BotProgram.QueueMessage("Disabled max pause duration!");
            }
        }
    }
}

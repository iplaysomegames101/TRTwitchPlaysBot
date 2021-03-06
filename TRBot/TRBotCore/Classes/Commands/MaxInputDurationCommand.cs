﻿using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Events;

namespace TRBot
{
    /// <summary>
    /// A command that prints out the max input duration and allows setting it for certain access levels.
    /// </summary>
    public class MaxInputDurationCommand : BaseCommand
    {
        private int SetAccessLevel = (int)AccessLevels.Levels.Moderator;

        public override void ExecuteCommand(object sender, OnChatCommandReceivedArgs e)
        {
            List<string> args = e.Command.ArgumentsAsList;

            if (args.Count == 0)
            {
                BotProgram.QueueMessage($"The max duration of an input sequence is {BotProgram.BotData.MaxInputDuration} milliseconds!");
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
                BotProgram.QueueMessage($"Usage: \"duration (ms)\"");
                return;
            }

            if (int.TryParse(args[0], out int newMaxDur) == false)
            {
                BotProgram.QueueMessage("Please enter a valid number!");
                return;
            }

            if (newMaxDur < 0)
            {
                BotProgram.QueueMessage("Cannot set a negative duration!");
                return;
            }

            if (newMaxDur == BotProgram.BotData.MaxInputDuration)
            {
                BotProgram.QueueMessage("The duration is already this value!");
                return;
            }

            BotProgram.BotData.MaxInputDuration = newMaxDur;
            BotProgram.SaveBotData();

            BotProgram.QueueMessage($"Set max input sequence duration to {newMaxDur} milliseconds!");
        }
    }
}

using Rollback.Protocol.Enums;
using Rollback.World.CustomEnums;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Guilds;

namespace Rollback.World.Game.Interactions.Dialogs.Interactives
{
    public sealed class GuildCreationDialog : Dialog
    {
        public override DialogType DialogType =>
            DialogType.GuildCreate;

        public GuildCreationDialog(Character character) : base(character) { }

        protected override void InternalOpen()
        {
            if (Character.GuildMember is null)
                GuildHandler.SendGuildCreationStartedMessage(Character.Client);
            else
            {
                GuildHandler.SendGuildCreationResultMessage(Character.Client, GuildCreationResultEnum.GUILD_CREATE_ERROR_ALREADY_IN_GUILD);
                Close();
            }
        }
    }
}

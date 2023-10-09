using Rollback.Common.Extensions;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Messages;
using Rollback.World.Database.Characters;
using Rollback.World.Game.Experiences;
using Rollback.World.Game.RolePlayActors.Characters;
using Rollback.World.Handlers.Basics;
using Rollback.World.Handlers.Connection;
using Rollback.World.Network;
using Rollback.World.Network.Handler;

namespace Rollback.World.Handlers.Characters
{
    public class CharacterCreationHandler
    {
        [WorldHandler(CharacterNameSuggestionRequestMessage.Id, false)]
        public static void HandleCharacterNameSuggestionRequestMessage(WorldClient client, CharacterNameSuggestionRequestMessage message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            CharacterRecord? character;
            string name;
            var attemps = 0;
            do
            {
                name = StringExtensions.RandomName();
                character = CharacterManager.GetCharacterRecordByName(name);
                attemps++;
            }
            while (character is not null && attemps < 30);

            if (character is not null && attemps is 30)
                SendCharacterNameSuggestionFailureMessage(client, NicknameGeneratingFailureEnum.NICKNAME_GENERATOR_UNAVAILABLE);
            else
                SendCharacterNameSuggestionSuccessMessage(client, name);
        }

        [WorldHandler(CharacterCreationRequestMessage.Id, false)]
        public static void HandleCharacterCreationRequestMessage(WorldClient client, CharacterCreationRequestMessage message)
        {
            if (client.Account!.FreeCharacterSlots > 0)
            {
                if (CharacterManager.IsNameCorrect(message.name))
                {
                    if (CharacterManager.GetCharacterRecordByName(message.name) is null)
                    {
                        var record = CharacterManager.Instance.CreateCharacter(client.Account!.Id,
                            message.name, message.sex, message.breed,
                            message.colors.Distinct().Count() is not 1 ? message.colors : Array.Empty<int>());

                        if (record is not null)
                        {
                            client.Account.Characters[record.Id] = record;
                            client.Account.FreeCharacterSlots--;

                            SendCharacterCreationResultMessage(client, CharacterCreationResultEnum.OK);
                            BasicHandler.SendBasicNoOperationMessage(client);
                            ConnectionHandler.SendCharactersListMessage(client);
                        }
                        else
                            SendCharacterCreationResultMessage(client, CharacterCreationResultEnum.ERR_NO_REASON);
                    }
                    else
                        SendCharacterCreationResultMessage(client, CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS);
                }
                else
                    SendCharacterCreationResultMessage(client, CharacterCreationResultEnum.ERR_INVALID_NAME);
            }
            else
                SendCharacterCreationResultMessage(client, CharacterCreationResultEnum.ERR_TOO_MANY_CHARACTERS);
        }

        [WorldHandler(CharacterDeletionRequestMessage.Id, false)]
        public static void HandleCharacterDeletionRequestMessage(WorldClient client, CharacterDeletionRequestMessage message)
        {
            if (client.Account!.Characters.ContainsKey(message.characterId))
            {
                var characterRecord = client.Account.Characters[message.characterId];
                if (ExperienceManager.Instance.GetCharacterLevel(characterRecord.Experience) < 20 || StringExtensions.CipherSecretAnswer(message.characterId, client.Account.SecretAnswer) == message.secretAnswerHash)
                {
                    if (CharacterManager.DeleteCharacter(characterRecord))
                    {
                        client.Account.Characters.Remove(message.characterId);
                        client.Account.FreeCharacterSlots++;

                        ConnectionHandler.SendCharactersListMessage(client);
                    }
                    else
                        client.Dispose();
                }
                else
                    SendCharacterDeletionErrorMessage(client, CharacterDeletionErrorEnum.DEL_ERR_BAD_SECRET_ANSWER);
            }
            else
                SendCharacterDeletionErrorMessage(client, CharacterDeletionErrorEnum.DEL_ERR_NO_REASON);
        }

        public static void SendCharacterNameSuggestionFailureMessage(WorldClient client, NicknameGeneratingFailureEnum reason) =>
            client.Send(new CharacterNameSuggestionFailureMessage((sbyte)reason));

        public static void SendCharacterNameSuggestionSuccessMessage(WorldClient client, string name) =>
            client.Send(new CharacterNameSuggestionSuccessMessage(name));

        public static void SendCharacterCreationResultMessage(WorldClient client, CharacterCreationResultEnum result) =>
            client.Send(new CharacterCreationResultMessage((sbyte)result));

        public static void SendCharacterDeletionErrorMessage(WorldClient client, CharacterDeletionErrorEnum reason) =>
            client.Send(new CharacterDeletionErrorMessage((sbyte)reason));
    }
}

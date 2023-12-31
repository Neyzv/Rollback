﻿namespace Rollback.World.CustomEnums
{
    public enum Actions
    {
        ActionEncapsulateBinaryCommand = 993,
        ActionEndsTurn = -2,
        ActionInternalSendActionBuffer,
        ActionNoOperation,
        ActionSequenceStart = 83,
        ActionSequenceEnd = 70,
        ActionCharacterMovement = 1,
        ActionCharacterChangeMap,
        ActionCharacterChangeRespawnMap,
        ActionCharacterTeleportOnSameMap,
        ActionCharacterPush,
        ActionCharacterPull,
        ActionCharacterDivorceWifeOrHusband,
        ActionCharacterExchangePlaces,
        ActionCharacterDodgeHit,
        ActionCharacterLearnEmoticon,
        ActionCharacterSetDirection,
        ActionCharacterCreateGuild,
        ActionUsePushpullElement = 14,
        ActionAreaChangeAlignment,
        ActionAreaGiveKamas,
        ActionScriptStart,
        ActionAreaDungeonAttacked = 20,
        ActionGainAreaKamas,
        ActionAreaDungeonCityOpened = 23,
        ActionAreaDungeonHeartOpened,
        ActionAreaDungeonHeartClosed,
        ActionAreaChangeAlignmentSub,
        ActionQuestObjectiveValidate = 30,
        ActionQuestStepValidate,
        ActionQuestQuestValidate,
        ActionQuestStepStart,
        ActionQuestStart,
        ActionQuestCheckStartedObjectives,
        ActionQuestReset,
        ActionStartDialogWithNpc = 40,
        ActionCarryCharacter = 50,
        ActionThrowCarriedCharacter,
        ActionNoMoreCarried,
        ActionCharacterMovementPointsSteal = 77,
        ActionCharacterMovementPointsWin,
        ActionCharacterMultiplyReceivedDamageOrGiveLifeWithRatio,
        ActionCharacterLifePointsLostFromPush,
        ActionCharacterLifePointsWinWithoutElement,
        ActionCharacterLifePointsStealWithoutBoost,
        ActionCharacterActionPointsSteal = 84,
        ActionCharacterLifePointsLostBasedOnCasterLifeFromWater,
        ActionCharacterLifePointsLostBasedOnCasterLifeFromEarth,
        ActionCharacterLifePointsLostBasedOnCasterLifeFromAir,
        ActionCharacterLifePointsLostBasedOnCasterLifeFromFire,
        ActionCharacterLifePointsLostBasedOnCasterLife,
        ActionCharacterDispatchLifePointsPercent,
        ActionCharacterLifePointsStealFromWater,
        ActionCharacterLifePointsStealFromEarth,
        ActionCharacterLifePointsStealFromAir,
        ActionCharacterLifePointsStealFromFire,
        ActionCharacterLifePointsSteal,
        ActionCharacterLifePointsLostFromWater,
        ActionCharacterLifePointsLostFromEarth,
        ActionCharacterLifePointsLostFromAir,
        ActionCharacterLifePointsLostFromFire,
        ActionCharacterLifePointsMalus = 1047,
        ActionCharacterLifePointsMalusPercent,
        ActionCharacterLifePointsLost = 100,
        ActionCharacterActionPointsLost,
        ActionCharacterActionPointsUse,
        ActionCharacterDeath,
        ActionCharacterActionTackled,
        ActionCharacterLifeLostModerator,
        ActionCharacterLifeLostCasterModerator = 265,
        ActionCharacterSpellReflector = 106,
        ActionCharacterLifeLostReflector,
        ActionCharacterLifePointsWin,
        ActionCharacterLifePointsLostCaster,
        ActionCharacterBoostLifePoints,
        ActionCharacterBoostActionPoints,
        ActionCharacterBoostDamages,
        ActionCharacterMultiplyDamages = 114,
        ActionCharacterBoostCriticalHit,
        ActionCharacterDeboostRange,
        ActionCharacterBoostRange,
        ActionCharacterBoostStrength,
        ActionCharacterBoostAgility,
        ActionCharacterActionPointsWin,
        ActionCharacterBoostDamagesForAllGame,
        ActionCharacterBoostCriticalMiss,
        ActionCharacterBoostChance,
        ActionCharacterBoostWisdom,
        ActionCharacterBoostVitality,
        ActionCharacterBoostIntelligence,
        ActionCharacterMovementPointsLost,
        ActionCharacterBoostMovementPoints,
        ActionCharacterMovementPointsUse,
        ActionCharacterStealGold,
        ActionCharacterManaUseKillLife,
        ActionCharacterRemoveAllEffects,
        ActionCharacterActionPointsLostCaster,
        ActionCharacterMovememtPointsLostCaster,
        ActionCharacterDeboostRangeCaster,
        ActionCharacterBoostRangeCaster,
        ActionCharacterBoostDamagesCaster,
        ActionCharacterBoostDamagesPercent,
        ActionCharacterDeboostDamagesPercent = 186,
        ActionCharacterEnergyPointsWin = 139,
        ActionCharacterPassNextTurn,
        ActionCharacterKill,
        ActionCharacterBoostPhysicalDamages,
        ActionCharacterLifePointsWinWithoutBoost,
        ActionCharacterLifePointsLostWithoutBoost,
        ActionCharacterDeboostDamages,
        ActionCharacterCurse,
        ActionCharacterResurectAllyInFight,
        ActionCharacterAddFollowingCharacter,
        ActionCharacterMakeInvisible = 150,
        ActionSpellInvisibleObstacle,
        ActionCharacterChangeColor = 333,
        ActionCharacterChangeLook = 149,
        ActionCharacterAddAppearance = 335,
        ActionCharacterDeboostChance = 152,
        ActionCharacterDeboostVitality,
        ActionCharacterDeboostAgility,
        ActionCharacterDeboostIntelligence,
        ActionCharacterDeboostWisdom,
        ActionCharacterDeboostStrength,
        ActionCharacterBoostMaximumWeight,
        ActionCharacterDeboostMaximumWeight,
        ActionCharacterBoostActionPointsLostDodge,
        ActionCharacterBoostMovementPointsLostDodge,
        ActionCharacterDeboostActionPointsLostDodge,
        ActionCharacterDeboostMovementPointsLostDodge,
        ActionCharacterBoostDodge = 752,
        ActionCharacterBoostTackle,
        ActionCharacterDeboostDodge,
        ActionCharacterDeboostTackle,
        ActionCharacterBoostWeaponDamagePercent = 165,
        ActionCharacterDeboostActionPoints = 168,
        ActionCharacterDeboostMovementPoints,
        ActionCharacterLifePointsWinInRp,
        ActionCharacterDeboostCriticalHit,
        ActionCharacterDeboostMagicalReduction,
        ActionCharacterDeboostPhysicalReduction,
        ActionCharacterBoostInitiative,
        ActionCharacterDeboostInitiative,
        ActionCharacterBoostMagicFind,
        ActionCharacterDeboostMagicFind,
        ActionCharacterBoostHealBonus,
        ActionCharacterDeboostHealBonus,
        ActionCharacterAddDouble,
        ActionSummonCreature,
        ActionCharacterBoostMaximumSummonedCreatures,
        ActionCharacterBoostMagicalReduction,
        ActionCharacterBoostPhysicalReduction,
        ActionSummonStaticCreature,
        ActionCharacterAlignmentRankSet = 187,
        ActionCharacterAlignmentSideSet,
        ActionCharacterAlignmentValueSet,
        ActionCharacterAlignmentValueModification,
        ActionCharacterInventoryClear,
        ActionCharacterInventoryRemoveItem,
        ActionCharacterInventoryAddItem,
        ActionCharacterInventoryAddItemNocheck = 209,
        ActionCharacterInventoryAddItemRandomNocheck = 221,
        ActionCharacterInventoryAddItemFromRandomDrop,
        ActionCharacterInventoryGainKamas = 194,
        ActionCharacterInventoryLoseKamas,
        ActionCharacterOpenMyStorage,
        ActionCharacterTransform,
        ActionCharacterClearAllJob,
        ActionCharacterRepairObject,
        ActionCharacterInventoryAddItemOnRpMap = 232,
        ActionCharacterInventoryRemoveItemOnRpMap,
        ActionDecorsPlayObjectAnimation = 200,
        ActionDecorsAddObject,
        ActionDecorsRevealUnvisible,
        ActionDecorsObstacleClose,
        ActionDecorsObstacleOpen,
        ActionCharacterChangeRestriction,
        ActionCharacterResurrection,
        ActionCollectResource,
        ActionDecorsPlayAnimation,
        ActionDecorsPlayAnimationUnlighted = 228,
        ActionCharacterBoostEarthElementPercent = 210,
        ActionCharacterBoostWaterElementPercent,
        ActionCharacterBoostAirElementPercent,
        ActionCharacterBoostFireElementPercent,
        ActionCharacterBoostNeutralElementPercent,
        ActionCharacterDeboostEarthElementPercent,
        ActionCharacterDeboostWaterElementPercent,
        ActionCharacterDeboostAirElementPercent,
        ActionCharacterDeboostFireElementPercent,
        ActionCharacterDeboostNeutralElementPercent,
        ActionCharacterReflectorUnboosted,
        ActionDecorsObstacleCloseRandom = 223,
        ActionDecorsObstacleOpenRandom,
        ActionCharacterBoostTrap,
        ActionCharacterBoostTrapPercent,
        ActionCharacterGainRide = 229,
        ActionCharacterEnergyLossBoost,
        ActionCharacterEnergyPointsLoose,
        ActionCharacterBoostEarthElementResist = 240,
        ActionCharacterBoostWaterElementResist,
        ActionCharacterBoostAirElementResist,
        ActionCharacterBoostFireElementResist,
        ActionCharacterBoostNeutralElementResist,
        ActionCharacterDeboostEarthElementResist,
        ActionCharacterDeboostWaterElementResist,
        ActionCharacterDeboostAirElementResist,
        ActionCharacterDeboostFireElementResist,
        ActionCharacterDeboostNeutralElementResist,
        ActionCharacterBoostEarthElementPvpPercent,
        ActionCharacterBoostWaterElementPvpPercent,
        ActionCharacterBoostAirElementPvpPercent,
        ActionCharacterBoostFireElementPvpPercent,
        ActionCharacterBoostNeutralElementPvpPercent,
        ActionCharacterDeboostEarthElementPvpPercent,
        ActionCharacterDeboostWaterElementPvpPercent,
        ActionCharacterDeboostAirElementPvpPercent,
        ActionCharacterDeboostFireElementPvpPercent,
        ActionCharacterDeboostNeutralElementPvpPercent,
        ActionCharacterBoostEarthElementPvpResist,
        ActionCharacterBoostWaterElementPvpResist,
        ActionCharacterBoostAirElementPvpResist,
        ActionCharacterBoostFireElementPvpResist,
        ActionCharacterBoostNeutralElementPvpResist,
        ActionCharacterStealChance = 266,
        ActionCharacterStealVitality,
        ActionCharacterStealAgility,
        ActionCharacterStealIntelligence,
        ActionCharacterStealWisdom,
        ActionCharacterStealStrength,
        ActionCharacterLifePointsLostBasedOnCasterLifeMissingFromWater = 275,
        ActionCharacterLifePointsLostBasedOnCasterLifeMissingFromEarth,
        ActionCharacterLifePointsLostBasedOnCasterLifeMissingFromAir,
        ActionCharacterLifePointsLostBasedOnCasterLifeMissingFromFire,
        ActionCharacterLifePointsLostBasedOnCasterLifeMissing,
        ActionCharacterBoostShieldBasedOnCasterLife = 1039,
        ActionCharacterBoostShield,
        ActionCharacterAddSpellCooldown = 1035,
        ActionCharacterRemoveSpellCooldown,
        ActionCharacterUpdateBoost = 515,
        ActionCharacterBoostDispelled = 514,
        ActionBoostSpellRange = 281,
        ActionBoostSpellRangeable,
        ActionBoostSpellDmg,
        ActionBoostSpellHeal,
        ActionBoostSpellApCost,
        ActionBoostSpellCastIntvl,
        ActionBoostSpellCc,
        ActionBoostSpellCastoutline,
        ActionBoostSpellNolineofsight,
        ActionBoostSpellMaxperturn,
        ActionBoostSpellMaxpertarget,
        ActionBoostSpellCastIntvlSet,
        ActionBoostSpellBaseDmg,
        ActionDeboostSpellRange,
        ActionFightCastSpell = 300,
        ActionFightCastSpellCriticalHit,
        ActionFightCastSpellCriticalMiss,
        ActionFightCloseCombat,
        ActionFightCloseCombatCriticalHit,
        ActionFightCloseCombatCriticalMiss,
        ActionFightTriggerTrap,
        ActionFightTriggerGlyph,
        ActionFightSpellDodgedPa,
        ActionFightSpellDodgedPm,
        ActionRemoveMarkTrigger,
        ActionCharacterStealRange = 320,
        ActionFightAddTrapCastingSpell = 400,
        ActionFightAddGlyphCastingSpell,
        ActionFightAddGlyphCastingSpellEndturn,
        ActionFightAddWallCastingSpell,

        ActionFightKillAndSummon = 405,
        ActionInteractiveElement = 500,
        ActionSkillAnimation,
        ActionExchangeCraftOpen,
        ActionUseWaypoint,
        ActionDoElementParameterizedOperation = 505,
        ActionInteractiveElementAtHomeInnerDoor = 507,
        ActionSaveWaypoint,
        ActionChangeCompass,
        ActionUseSubway,
        ActionExchangeJobindexOpen,
        ActionUsePrism,
        ActionAddPrism,
        ActionGotoWaypoint = 600,
        ActionGotoMap,
        ActionCharacterLearnJob = 603,
        ActionCharacterLearnSpell,
        ActionCharacterGainXp,
        ActionCharacterGainWisdom,
        ActionCharacterGainStrength,
        ActionCharacterGainChance,
        ActionCharacterGainAgility,
        ActionCharacterGainVitality,
        ActionCharacterGainIntelligence,
        ActionCharacterGainStatsPoints,
        ActionCharacterGainSpellPoints,
        ActionCharacterGainJobXp,
        ActionCharacterUnlearnJob,
        ActionCharacterUnboostSpell,
        ActionCharacterGetMarried,
        ActionCharacterGetMarriedAccept,
        ActionCharacterGetMarriedDecline,
        ActionCharacterReadBook,
        ActionCharacterSummonMonster,
        ActionGotoHouse,
        ActionCharacterSummonMonsterGroup,
        ActionCharacterSummonMonsterGroupSetMap = 627,
        ActionCharacterSummonMonsterGroupDynamic,
        ActionCharacterUnlearnGuildspell = 624,
        ActionCharacterUnboostCaracs,
        ActionCharacterUnboostCaracsTo101,
        ActionCharacterSendInformationText = 630,
        ActionCharacterSendDialogAction,
        ActionCharacterGainHonour = 640,
        ActionCharacterGainDishonour,
        ActionCharacterLooseHonour,
        ActionCharacterLooseDishonour,
        ActionMapResurectionAllies = 645,
        ActionMapHealAllies,
        ActionMapForceEnnemiesGhost,
        ActionForceEnnemyGhost,
        ActionFakeAlignment,
        ActionTeleportNoobyMap,
        ActionUseElementActions,
        ActionSetStatedElementState,
        ActionResetStatedElement,
        ActionHouseLeave,
        ActionNoop = 666,
        ActionIncarnation = 669,
        ActionCharacterReferencement = 699,
        ActionItemChangeEffect,
        ActionItemAddEffect,
        ActionItemAddDurability,
        ActionCaptureSoul = 705,
        ActionCaptureRide,
        ActionCharacterAddCostToAction = 710,
        ActionLadderSuperrace = 715,
        ActionLadderRace,
        ActionLadderId,
        ActionPvpLadder = 720,
        ActionVictimOf,
        ActionGainTempSpell,
        ActionGainAura,
        ActionGainTitle,
        ActionCharacterRenameGuild,
        ActionTeleportNearestPrism = 730,
        ActionAutoAggressEnemyPlayer,
        ActionShushuStackRune = 742,
        ActionBoostSoulCaptureBonus = 750,
        ActionBoostRideXpBonus,
        ActionRemoveOnMove = 760,
        ActionCharacterSacrify = 765,
        ActionHourlyConfusionDegree = 770,
        ActionHourlyConfusionPi2,
        ActionHourlyConfusionPi4,
        ActionUnhourlyConfusionDegree,
        ActionUnhourlyConfusionPi2,
        ActionUnhourlyConfusionPi4,
        ActionCharacterBoostPermanentDamagePercent,
        ActionCharacterSummonDeadAllyInFight = 780,
        ActionCharacterUnlucky,
        ActionCharacterMaximizeRoll,
        ActionCharacterWalkFourDir = 785,
        ActionFindBountyTarget = 790,
        ActionMarkTargetMercenary,
        ActionItemChangePetsLife = 800,
        ActionItemChangeDuration = 805,
        ActionItemPetsShape,
        ActionItemPetsEat,
        ActionPetsLastMeal,
        ActionItemChangeDurability = 812,
        ActionItemUpdateDate,
        ActionItemChooseInItemList = 820,
        ActionClientOpenUi = 830,
        ActionClientOpenUiSpellForget = 831,
        ActionFightTurnFinished = 899,
        ActionFightChallenge,
        ActionFightChallengeAccept,
        ActionFightChallengeDecline,
        ActionFightChallengeJoin,
        ActionFightChallengeAgainstMonster = 905,
        ActionFightAggression,
        ActionFightAgainstTaxcollector = 909,
        ActionFightChallengeAgainstMutant,
        ActionFightChallengeMixedVersusMonster,
        ActionFightAgainstPrism,
        ActionTooltipActivateTip = 915,
        ActionPnjRemoveRide = 938,
        ActionPetSetPowerBoost,
        ActionFarmWithdrawItem = 947,
        ActionFarmPlaceItem,
        ActionMountRide,
        ActionFightSetState,
        ActionFightUnsetState,
        ActionCreatedSince = 963,
        ActionShowPlayername,
        ActionBoostGlobalResistsBonus = 1076,
        ActionBoostGlobalResistsMalus
    }
}

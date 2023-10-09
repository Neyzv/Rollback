namespace Rollback.World.CustomEnums
{
    public enum EffectId
    {
        /// <summary>
        /// Téléporte sur la map ciblée
        /// </summary>
        Effect2 = 2,
        /// <summary>
        /// Fixe le point de respawn
        /// </summary>
        Effect3 = 3,
        /// <summary>
        /// Téléporte sur la case ciblée
        /// </summary>
        EffectTeleport = 4,
        /// <summary>
        /// Repousse de #1 case(s)
        /// </summary>
        EffectPushBack = 5,
        /// <summary>
        /// Attire de #1 case(s)
        /// </summary>
        EffectPullForward = 6,
        /// <summary>
        /// Divorcer le couple
        /// </summary>
        Effect7 = 7,
        /// <summary>
        /// Échange de positions
        /// </summary>
        EffectSwitchPosition = 8,
        /// <summary>
        /// Esquive #1% des coups en reculant de #2 case(s)
        /// </summary>
        EffectDodge = 9,
        /// <summary>
        /// Attitude #3
        /// </summary>
        EffectLearnEmote = 10,
        /// <summary>
        /// 
        /// </summary>
        Effect12 = 12,
        /// <summary>
        /// Change le temps de jeu d'un joueur
        /// </summary>
        Effect13 = 13,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect30 = 30,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect31 = 31,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect32 = 32,
        /// <summary>
        /// Débute une quête
        /// </summary>
        Effect34 = 34,
        /// <summary>
        /// Reset d'une quête
        /// </summary>
        Effect36 = 36,
        /// <summary>
        /// Démarre une quête (force)
        /// </summary>
        Effect37 = 37,
        /// <summary>
        /// Porte la cible
        /// </summary>
        EffectCarry = 50,
        /// <summary>
        /// Lance une entité
        /// </summary>
        EffectThrow = 51,
        /// <summary>
        /// Vole #1{~1~2 à }#2 PM
        /// </summary>
        EffectStealMP77 = 77,
        /// <summary>
        /// Ajoute +#1{~1~2 à }#2 PM
        /// </summary>
        EffectAddMP = 78,
        /// <summary>
        /// #3% soigné de x#2, sinon dégâts subis x#1
        /// </summary>
        EffectHealOrMultiply = 79,
        /// <summary>
        /// #1{~1~2 à }#2 (PV rendus)
        /// </summary>
        EffectHealHP81 = 81,
        /// <summary>
        /// #1{~1~2 à }#2 PV (vol Neutre fixe)
        /// </summary>
        EffectStealHPFix = 82,
        /// <summary>
        /// Vole #1{~1~2 à }#2 PA
        /// </summary>
        EffectStealAP84 = 84,
        /// <summary>
        /// #1{~1~2 à }#2% des PV de l'attaquant (dommages Eau)
        /// </summary>
        EffectDamagePercentWater = 85,
        /// <summary>
        /// #1{~1~2 à }#2% des PV de l'attaquant (dommages Terre)
        /// </summary>
        EffectDamagePercentEarth = 86,
        /// <summary>
        /// #1{~1~2 à }#2% des PV de l'attaquant (dommages Air)
        /// </summary>
        EffectDamagePercentAir = 87,
        /// <summary>
        /// #1{~1~2 à }#2% des PV de l'attaquant (dommages Feu)
        /// </summary>
        EffectDamagePercentFire = 88,
        /// <summary>
        /// #1{~1~2 à }#2% des PV de l'attaquant (dommages Neutre)
        /// </summary>
        EffectDamagePercentNeutral = 89,
        /// <summary>
        /// Donne #1{~1~2 à }#2 % de sa vie
        /// </summary>
        EffectGiveHPPercent = 90,
        /// <summary>
        /// #1{~1~2 à }#2 (vol Eau)
        /// </summary>
        EffectStealHPWater = 91,
        /// <summary>
        /// #1{~1~2 à }#2 (vol Terre)
        /// </summary>
        EffectStealHPEarth = 92,
        /// <summary>
        /// #1{~1~2 à }#2 (vol Air)
        /// </summary>
        EffectStealHPAir = 93,
        /// <summary>
        /// #1{~1~2 à }#2 (vol Feu)
        /// </summary>
        EffectStealHPFire = 94,
        /// <summary>
        /// #1{~1~2 à }#2 (vol Neutre)
        /// </summary>
        EffectStealHPNeutral = 95,
        /// <summary>
        /// #1{~1~2 à }#2 (dommages Eau)
        /// </summary>
        EffectDamageWater = 96,
        /// <summary>
        /// #1{~1~2 à }#2 (dommages Terre)
        /// </summary>
        EffectDamageEarth = 97,
        /// <summary>
        /// #1{~1~2 à }#2 (dommages Air)
        /// </summary>
        EffectDamageAir = 98,
        /// <summary>
        /// #1{~1~2 à }#2 (dommages Feu)
        /// </summary>
        EffectDamageFire = 99,
        /// <summary>
        /// #1{~1~2 à }#2 (dommages Neutre)
        /// </summary>
        EffectDamageNeutral = 100,
        /// <summary>
        /// -#1{~1~2 à -}#2 PA
        /// </summary>
        EffectLostAP = 101,
        /// <summary>
        /// Dommages réduits de #1{~1~2 à }#2
        /// </summary>
        EffectAddGlobalDamageReduction105 = 105,
        /// <summary>
        /// Renvoie un sort de niveau #2 maximum
        /// </summary>
        EffectReflectSpell = 106,
        /// <summary>
        /// Dommages retournés : #1{~1~2 à }#2
        /// </summary>
        EffectAddDamageReflection = 107,
        /// <summary>
        /// #1{~1~2 à }#2 (PV rendus)
        /// </summary>
        EffectHealHP108 = 108,
        /// <summary>
        /// #1{~1~2 à }#2 (dommages au lanceur)
        /// </summary>
        EffectDamageCaster = 109,
        /// <summary>
        /// #1{~1~2 à }#2 Vie
        /// </summary>
        EffectAddHealth = 110,
        /// <summary>
        /// #1{~1~2 à }#2 PA
        /// </summary>
        EffectAddAP111 = 111,
        /// <summary>
        /// #1{~1~2 à }#2 Dommages
        /// </summary>
        EffectAddDamageBonus = 112,
        /// <summary>
        /// Double les dommages ou rend  #1{~1~2 à }#2 PDV
        /// </summary>
        Effect113 = 113,
        /// <summary>
        /// Multiplie les dommages par #1
        /// </summary>
        EffectAddDamageMultiplicator = 114,
        /// <summary>
        /// #1{~1~2 à }#2% Critique
        /// </summary>
        EffectAddCriticalHit = 115,
        /// <summary>
        /// -#1{~1~2 à -}#2 PO
        /// </summary>
        EffectSubRange = 116,
        /// <summary>
        /// #1{~1~2 à }#2 PO
        /// </summary>
        EffectAddRange = 117,
        /// <summary>
        /// #1{~1~2 à }#2 Force
        /// </summary>
        EffectAddStrength = 118,
        /// <summary>
        /// #1{~1~2 à }#2 Agilité
        /// </summary>
        EffectAddAgility = 119,
        /// <summary>
        /// Ajoute +#1{~1~2 à }#2 PA
        /// </summary>
        EffectRegainAP = 120,
        /// <summary>
        /// #1{~1~2 à }#2 Dommages
        /// </summary>
        EffectAddDamageBonus121 = 121,
        /// <summary>
        /// #1{~1~2 à }#2 Échecs Critiques
        /// </summary>
        EffectAddCriticalMiss = 122,
        /// <summary>
        /// #1{~1~2 à }#2 Chance
        /// </summary>
        EffectAddChance = 123,
        /// <summary>
        /// #1{~1~2 à }#2 Sagesse
        /// </summary>
        EffectAddWisdom = 124,
        /// <summary>
        /// #1{~1~2 à }#2 Vitalité
        /// </summary>
        EffectAddVitality = 125,
        /// <summary>
        /// #1{~1~2 à }#2 Intelligence
        /// </summary>
        EffectAddIntelligence = 126,
        /// <summary>
        /// -#1{~1~2 à -}#2 PM
        /// </summary>
        EffectLostMP = 127,
        /// <summary>
        /// #1{~1~2 à }#2 PM
        /// </summary>
        EffectAddMP128 = 128,
        /// <summary>
        /// Vole #1{~1~2 à }#2 Kamas
        /// </summary>
        EffectStealKamas = 130,
        /// <summary>
        /// #1 PA utilisés font perdre #2 PV
        /// </summary>
        EffectLoseHPByUsingAP = 131,
        /// <summary>
        /// Enlève les envoûtements
        /// </summary>
        EffectDispelMagicEffects = 132,
        /// <summary>
        /// PA perdus pour le lanceur : #1{~1~2 à }#2
        /// </summary>
        EffectLosingAP = 133,
        /// <summary>
        /// PM perdus pour le lanceur : #1{~1~2 à }#2
        /// </summary>
        EffectLosingMP = 134,
        /// <summary>
        /// Portée du lanceur réduite de : #1{~1~2 à }#2
        /// </summary>
        EffectSubRange135 = 135,
        /// <summary>
        /// Portée du lanceur augmentée de : #1{~1~2 à }#2
        /// </summary>
        EffectAddRange136 = 136,
        /// <summary>
        /// Dommages physiques du lanceur augmentés de : #1{~1~2 à }#2
        /// </summary>
        EffectAddPhysicalDamage137 = 137,
        /// <summary>
        /// Augmente les dommages de #1{~1~2 à }#2%
        /// </summary>
        EffectIncreaseDamage138 = 138,
        /// <summary>
        /// Rend #1{~1~2 à }#2 points d'énergie
        /// </summary>
        EffectRestoreEnergyPoints = 139,
        /// <summary>
        /// Tour annulé
        /// </summary>
        EffectSkipTurn = 140,
        /// <summary>
        /// Tue la cible
        /// </summary>
        EffectKill = 141,
        /// <summary>
        /// #1{~1~2 à }#2 Dommages Physiques
        /// </summary>
        EffectAddPhysicalDamage142 = 142,
        /// <summary>
        /// #1{~1~2 à }#2 (PV rendus)
        /// </summary>
        EffectHealHP143 = 143,
        /// <summary>
        /// #1{~1~2 à }#2 (dommages Neutre fixe)
        /// </summary>
        EffectFixedNeutralDamage = 144,
        /// <summary>
        /// -#1{~1~2 à }#2 Dommages
        /// </summary>
        EffectSubDamageBonus = 145,
        /// <summary>
        /// Change les paroles
        /// </summary>
        Effect146 = 146,
        /// <summary>
        /// Ressuscite un allié
        /// </summary>
        Effect147 = 147,
        /// <summary>
        /// Quelqu'un vous suit !
        /// </summary>
        Effect148 = 148,
        /// <summary>
        /// Change l'apparence
        /// </summary>
        EffectChangeAppearance = 149,
        /// <summary>
        /// Rend le personnage invisible
        /// </summary>
        EffectInvisibility = 150,
        /// <summary>
        /// -#1{~1~2 à -}#2 Chance
        /// </summary>
        EffectSubChance = 152,
        /// <summary>
        /// -#1{~1~2 à -}#2 Vitalité
        /// </summary>
        EffectSubVitality = 153,
        /// <summary>
        /// -#1{~1~2 à -}#2 Agilité
        /// </summary>
        EffectSubAgility = 154,
        /// <summary>
        /// -#1{~1~2 à -}#2 Intelligence
        /// </summary>
        EffectSubIntelligence = 155,
        /// <summary>
        /// -#1{~1~2 à -}#2 Sagesse
        /// </summary>
        EffectSubWisdom = 156,
        /// <summary>
        /// -#1{~1~2 à -}#2 Force
        /// </summary>
        EffectSubStrength = 157,
        /// <summary>
        /// #1{~1~2 à }#2 Pods
        /// </summary>
        EffectIncreaseWeight = 158,
        /// <summary>
        /// -#1{~1~2 à }#2 Pods
        /// </summary>
        EffectDecreaseWeight = 159,
        /// <summary>
        /// #1{~1~2 à }#2 Esquive PA
        /// </summary>
        EffectAddDodgeAPProbability = 160,
        /// <summary>
        /// #1{~1~2 à }#2 Esquive PM
        /// </summary>
        EffectAddDodgeMPProbability = 161,
        /// <summary>
        /// -#1{~1~2 à }#2 Esquive PA
        /// </summary>
        EffectSubDodgeAPProbability = 162,
        /// <summary>
        /// -#1{~1~2 à }#2 Esquive PM
        /// </summary>
        EffectSubDodgeMPProbability = 163,
        /// <summary>
        /// Dommages réduits de #1%
        /// </summary>
        EffectAddGlobalDamageReduction = 164,
        /// <summary>
        /// #2% Dommages #1
        /// </summary>
        EffectAddDamageBonusPercent = 165,
        /// <summary>
        /// PA retournés : #1{~1~2 à }#2
        /// </summary>
        Effect166 = 166,
        /// <summary>
        /// -#1{~1~2 à -}#2 PA
        /// </summary>
        EffectSubAP = 168,
        /// <summary>
        /// -#1{~1~2 à -}#2 PM
        /// </summary>
        EffectSubMP = 169,
        /// <summary>
        /// -#1{~1~2 à }#2% Critique
        /// </summary>
        EffectSubCriticalHit = 171,
        /// <summary>
        /// -#1{~1~2 à }#2 Réduction Magique
        /// </summary>
        EffectSubMagicDamageReduction = 172,
        /// <summary>
        /// -#1{~1~2 à }#2 Réduction Physique
        /// </summary>
        EffectSubPhysicalDamageReduction = 173,
        /// <summary>
        /// #1{~1~2 à }#2 Initiative
        /// </summary>
        EffectAddInitiative = 174,
        /// <summary>
        /// -#1{~1~2 à }#2 Initiative
        /// </summary>
        EffectSubInitiative = 175,
        /// <summary>
        /// #1{~1~2 à }#2 Prospection
        /// </summary>
        EffectAddProspecting = 176,
        /// <summary>
        /// -#1{~1~2 à }#2 Prospection
        /// </summary>
        EffectSubProspecting = 177,
        /// <summary>
        /// #1{~1~2 à }#2 Soins
        /// </summary>
        EffectAddHealBonus = 178,
        /// <summary>
        /// -#1{~1~2 à }#2 Soins
        /// </summary>
        EffectSubHealBonus = 179,
        /// <summary>
        /// Crée un double du lanceur de sort
        /// </summary>
        EffectDouble = 180,
        /// <summary>
        /// Invoque : #1
        /// </summary>
        EffectSummon = 181,
        /// <summary>
        /// #1{~1~2 à }#2 Invocations
        /// </summary>
        EffectAddSummonLimit = 182,
        /// <summary>
        /// #1{~1~2 à }#2 Réduction Magique
        /// </summary>
        EffectAddMagicDamageReduction = 183,
        /// <summary>
        /// #1{~1~2 à }#2 Réduction Physique
        /// </summary>
        EffectAddPhysicalDamageReduction = 184,
        /// <summary>
        /// Invoque : #1 (statique)
        /// </summary>
        EffectSummonStatic = 185,
        /// <summary>
        /// -#1{~1~2 à }#2 Puissance
        /// </summary>
        EffectSubDamageBonusPercent = 186,
        /// <summary>
        /// Changer l'alignement
        /// </summary>
        Effect188 = 188,
        /// <summary>
        /// 
        /// </summary>
        Effect192 = 192,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect193 = 193,
        /// <summary>
        /// Gagner #1{~1~2 à }#2 Kamas
        /// </summary>
        EffectGiveKamas = 194,
        /// <summary>
        /// Transforme en #1
        /// </summary>
        Effect197 = 197,
        /// <summary>
        /// Pose un objet au sol
        /// </summary>
        Effect201 = 201,
        /// <summary>
        /// Dévoile tous les objets invisibles
        /// </summary>
        EffectRevealsInvisible = 202,
        /// <summary>
        /// Ressuscite la cible
        /// </summary>
        Effect206 = 206,
        /// <summary>
        /// #1{~1~2 à }#2% Résistance Terre
        /// </summary>
        EffectAddEarthResistPercent = 210,
        /// <summary>
        /// #1{~1~2 à }#2% Résistance Eau
        /// </summary>
        EffectAddWaterResistPercent = 211,
        /// <summary>
        /// #1{~1~2 à }#2% Résistance Air
        /// </summary>
        EffectAddAirResistPercent = 212,
        /// <summary>
        /// #1{~1~2 à }#2% Résistance Feu
        /// </summary>
        EffectAddFireResistPercent = 213,
        /// <summary>
        /// #1{~1~2 à }#2% Résistance Neutre
        /// </summary>
        EffectAddNeutralResistPercent = 214,
        /// <summary>
        /// -#1{~1~2 à }#2% Résistance Terre
        /// </summary>
        EffectSubEarthResistPercent = 215,
        /// <summary>
        /// -#1{~1~2 à }#2 % Résistance Eau
        /// </summary>
        EffectSubWaterResistPercent = 216,
        /// <summary>
        /// -#1{~1~2 à }#2% Résistance Air
        /// </summary>
        EffectSubAirResistPercent = 217,
        /// <summary>
        /// -#1{~1~2 à }#2% Résistance Feu
        /// </summary>
        EffectSubFireResistPercent = 218,
        /// <summary>
        /// -#1{~1~2 à }#2 % Résistance Neutre
        /// </summary>
        EffectSubNeutralResistPercent = 219,
        /// <summary>
        /// Renvoie #1{~1~2 à }#2 dommages
        /// </summary>
        EffectAddDamageReflection220 = 220,
        /// <summary>
        /// Qu'y a-t-il là dedans ?
        /// </summary>
        Effect221 = 221,
        /// <summary>
        /// Qu'y a-t-il là dedans ?
        /// </summary>
        Effect222 = 222,
        /// <summary>
        /// #1{~1~2 à }#2 Dommages Pièges
        /// </summary>
        EffectAddTrapBonus = 225,
        /// <summary>
        /// #1{~1~2 à }#2 Puissance (pièges)
        /// </summary>
        EffectAddTrapBonusPercent = 226,
        /// <summary>
        /// Récupère une monture !
        /// </summary>
        Effect229 = 229,
        /// <summary>
        /// #1 Énergie Perdue
        /// </summary>
        Effect230 = 230,
        /// <summary>
        /// Play Animation
        /// </summary>
        EffectPlayAnimation = 237,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect239 = 239,
        /// <summary>
        /// #1{~1~2 à }#2 Résistance Terre
        /// </summary>
        EffectAddEarthElementReduction = 240,
        /// <summary>
        /// #1{~1~2 à }#2 Résistance Eau
        /// </summary>
        EffectAddWaterElementReduction = 241,
        /// <summary>
        /// #1{~1~2 à }#2 Résistance Air
        /// </summary>
        EffectAddAirElementReduction = 242,
        /// <summary>
        /// #1{~1~2 à }#2 Résistance Feu
        /// </summary>
        EffectAddFireElementReduction = 243,
        /// <summary>
        /// #1{~1~2 à }#2 Résistance Neutre
        /// </summary>
        EffectAddNeutralElementReduction = 244,
        /// <summary>
        /// -#1{~1~2 à }#2 Résistance Terre
        /// </summary>
        EffectSubEarthElementReduction = 245,
        /// <summary>
        /// -#1{~1~2 à }#2 Résistance Eau
        /// </summary>
        EffectSubWaterElementReduction = 246,
        /// <summary>
        /// -#1{~1~2 à }#2 Résistance Air
        /// </summary>
        EffectSubAirElementReduction = 247,
        /// <summary>
        /// -#1{~1~2 à }#2 Résistance Feu
        /// </summary>
        EffectSubFireElementReduction = 248,
        /// <summary>
        /// -#1{~1~2 à }#2 Résistance Neutre
        /// </summary>
        EffectSubNeutralElementReduction = 249,
        /// <summary>
        /// #1{~1~2 à }#2% Résistance Terre JCJ
        /// </summary>
        EffectAddPvpEarthResistPercent = 250,
        /// <summary>
        /// #1{~1~2 à }#2% Résistance Eau JCJ
        /// </summary>
        EffectAddPvpWaterResistPercent = 251,
        /// <summary>
        /// #1{~1~2 à }#2% Résistance Air JCJ
        /// </summary>
        EffectAddPvpAirResistPercent = 252,
        /// <summary>
        /// #1{~1~2 à }#2% Résistance Feu JCJ
        /// </summary>
        EffectAddPvpFireResistPercent = 253,
        /// <summary>
        /// #1{~1~2 à }#2% Résistance Neutre JCJ
        /// </summary>
        EffectAddPvpNeutralResistPercent = 254,
        /// <summary>
        /// -#1{~1~2 à }#2% Résistance Terre JCJ
        /// </summary>
        EffectSubPvpEarthResistPercent = 255,
        /// <summary>
        /// -#1{~1~2 à }#2% Résistance Eau JCJ
        /// </summary>
        EffectSubPvpWaterResistPercent = 256,
        /// <summary>
        /// -#1{~1~2 à }#2% Résistance Air JCJ
        /// </summary>
        EffectSubPvpAirResistPercent = 257,
        /// <summary>
        /// -#1{~1~2 à }#2% Résistance Feu JCJ
        /// </summary>
        EffectSubPvpFireResistPercent = 258,
        /// <summary>
        /// -#1{~1~2 à }#2% Résistance Neutre JCJ
        /// </summary>
        EffectSubPvpNeutralResistPercent = 259,
        /// <summary>
        /// #1{~1~2 à }#2 Résistance Terre JCJ
        /// </summary>
        EffectAddPvpEarthElementReduction = 260,
        /// <summary>
        /// #1{~1~2 à }#2 Résistance Eau JCJ
        /// </summary>
        EffectAddPvpWaterElementReduction = 261,
        /// <summary>
        /// #1{~1~2 à }#2 Résistance Air JCJ
        /// </summary>
        EffectAddPvpAirElementReduction = 262,
        /// <summary>
        /// #1{~1~2 à }#2 Résistance Feu JCJ
        /// </summary>
        EffectAddPvpFireElementReduction = 263,
        /// <summary>
        /// #1{~1~2 à }#2 Résistance Neutre JCJ
        /// </summary>
        EffectAddPvpNeutralElementReduction = 264,
        /// <summary>
        /// Dommages réduits de #1{~1~2 à }#2
        /// </summary>
        EffectAddArmorDamageReduction = 265,
        /// <summary>
        /// Vole #1{~1~2 à -}#2 Chance
        /// </summary>
        EffectStealChance = 266,
        /// <summary>
        /// Vole #1{~1~2 à -}#2 Vitalité
        /// </summary>
        EffectStealVitality = 267,
        /// <summary>
        /// Vole #1{~1~2 à -}#2 Agilité
        /// </summary>
        EffectStealAgility = 268,
        /// <summary>
        /// Vole #1{~1~2 à -}#2 Intelligence
        /// </summary>
        EffectStealIntelligence = 269,
        /// <summary>
        /// Vole #1{~1~2 à -}#2 Sagesse
        /// </summary>
        EffectStealWisdom = 270,
        /// <summary>
        /// Vole #1{~1~2 à -}#2 Force
        /// </summary>
        EffectStealStrength = 271,
        /// <summary>
        /// #1{~1~2 à }#2% des PV manquants de l'attaquant (dommages Eau)
        /// </summary>
        EffectDamageWaterPerHPLost = 275,
        /// <summary>
        /// #1{~1~2 à }#2% des PV manquants de l'attaquant (dommages Terre)
        /// </summary>
        EffectDamageEarthPerHPLost = 276,
        /// <summary>
        /// #1{~1~2 à }#2% des PV manquants de l'attaquant (dommages Air)
        /// </summary>
        EffectDamageAirPerHPLost = 277,
        /// <summary>
        /// #1{~1~2 à }#2% des PV manquants de l'attaquant (dommages Feu)
        /// </summary>
        EffectDamageFirePerHPLost = 278,
        /// <summary>
        /// #1{~1~2 à }#2% des PV manquants de l'attaquant (dommages Neutre)
        /// </summary>
        EffectDamageNeutralPerHPLost = 279,
        /// <summary>
        /// Augmente la PO du sort #1 de #3
        /// </summary>
        EffectSpellRangeIncrease = 281,
        /// <summary>
        /// Rend la portée du sort #1 modifiable
        /// </summary>
        EffectSpellRangeableEnable = 282,
        /// <summary>
        /// +#3 Dommages sur le sort #1
        /// </summary>
        EffectSpellDamageIncrease = 283,
        /// <summary>
        /// +#3 Soins sur le sort #1
        /// </summary>
        EffectSpellHealIncrease = 284,
        /// <summary>
        /// Réduit de #3 le coût en PA du sort #1
        /// </summary>
        EffectApCostReduce = 285,
        /// <summary>
        /// Réduit de #3 le délai de relance du sort #1
        /// </summary>
        EffectSpellDelayReduce = 286,
        /// <summary>
        /// +#3% Critique sur le sort #1
        /// </summary>
        EffectSpellCriticalPercent = 287,
        /// <summary>
        /// Désactive le lancer en ligne du sort #1
        /// </summary>
        EffectLineCastDisable = 288,
        /// <summary>
        /// Désactive la ligne de vue du sort #1
        /// </summary>
        EffectSpellObstaclesDisable = 289,
        /// <summary>
        /// Augmente de #3 le nombre de lancer maximal par tour du sort #1
        /// </summary>
        EffectSpellMaxCastBoost = 290,
        /// <summary>
        /// Augmente de #3 le nombre de lancer maximal par cible du sort #1
        /// </summary>
        EffectSpellMaxTargetCastBoost = 291,
        /// <summary>
        /// Fixe à #3 le délai de relance du sort #1
        /// </summary>
        EffectSpellDelayBoost = 292,
        /// <summary>
        /// Augmente les dégâts de base du sort #1 de #3
        /// </summary>
        EffectSpellBoost = 293,
        /// <summary>
        /// Diminue la portée du sort #1 de #3
        /// </summary>
        Effect294 = 294,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect310 = 310,
        /// <summary>
        /// Vole #1{~1~2 à }#2 PO
        /// </summary>
        EffectStealRange = 320,
        /// <summary>
        /// Change une couleur
        /// </summary>
        Effect333 = 333,
        /// <summary>
        /// Change l'apparence
        /// </summary>
        EffectChangeAppearance335 = 335,
        /// <summary>
        /// 
        /// </summary>
        Effect350 = 350,
        /// <summary>
        /// 
        /// </summary>
        EffectPowerSink = 351,
        /// <summary>
        /// Pose un piège de rang #2
        /// </summary>
        EffectTrap = 400,
        /// <summary>
        /// Pose un glyphe de rang #2
        /// </summary>
        EffectGlyph = 401,
        /// <summary>
        /// Pose un glyphe de rang #2
        /// </summary>
        EffectGlyph402 = 402,
        /// <summary>
        /// Tue la cible pour la remplacer par l'invocation : #1
        /// </summary>
        EffectKillAndSummon = 405,
        /// <summary>
        /// Enlève les effets du sort #2
        /// </summary>
        EffectRemoveSpellEffects = 406,
        /// <summary>
        /// #1{~1~2 à }#2 (PV rendus)
        /// </summary>
        Effect407 = 407,
        /// <summary>
        /// #1{~1~2 à }#2 Retrait PA
        /// </summary>
        EffectAddAPAttack = 410,
        /// <summary>
        /// -#1{~1~2 à }#2 Retrait PA
        /// </summary>
        EffectSubAPAttack = 411,
        /// <summary>
        /// #1{~1~2 à }#2 Retrait PM
        /// </summary>
        EffectAddMPAttack = 412,
        /// <summary>
        /// -#1{~1~2 à }#2 Retrait PM
        /// </summary>
        EffectSubMPAttack = 413,
        /// <summary>
        /// #1{~1~2 à }#2 Dommages Poussée
        /// </summary>
        EffectAddPushDamageBonus = 414,
        /// <summary>
        /// -#1{~1~2 à }#2 Dommages Poussée
        /// </summary>
        EffectSubPushDamageBonus = 415,
        /// <summary>
        /// #1{~1~2 à }#2 Résistance Poussée
        /// </summary>
        EffectAddPushDamageReduction = 416,
        /// <summary>
        /// -#1{~1~2 à }#2 Résistance Poussée
        /// </summary>
        EffectSubPushDamageReduction = 417,
        /// <summary>
        /// #1{~1~2 à }#2 Dommages Critiques
        /// </summary>
        EffectAddCriticalDamageBonus = 418,
        /// <summary>
        /// -#1{~1~2 à }#2 Dommages Critiques
        /// </summary>
        EffectSubCriticalDamageBonus = 419,
        /// <summary>
        /// #1{~1~2 à }#2 Résistance Critiques
        /// </summary>
        EffectAddCriticalDamageReduction = 420,
        /// <summary>
        /// -#1{~1~2 à }#2 Résistance Critiques
        /// </summary>
        EffectSubCriticalDamageReduction = 421,
        /// <summary>
        /// #1{~1~2 à }#2 Dommages Terre
        /// </summary>
        EffectAddEarthDamageBonus = 422,
        /// <summary>
        /// -#1{~1~2 à }#2 Dommages Terre
        /// </summary>
        EffectSubEarthDamageBonus = 423,
        /// <summary>
        /// #1{~1~2 à }#2 Dommages Feu
        /// </summary>
        EffectAddFireDamageBonus = 424,
        /// <summary>
        /// -#1{~1~2 à }#2 Dommages Feu
        /// </summary>
        EffectSubFireDamageBonus = 425,
        /// <summary>
        /// #1{~1~2 à }#2 Dommages Eau
        /// </summary>
        EffectAddWaterDamageBonus = 426,
        /// <summary>
        /// -#1{~1~2 à }#2 Dommages Eau
        /// </summary>
        EffectSubWaterDamageBonus = 427,
        /// <summary>
        /// #1{~1~2 à }#2 Dommages Air
        /// </summary>
        EffectAddAirDamageBonus = 428,
        /// <summary>
        /// -#1{~1~2 à }#2 Dommages Air
        /// </summary>
        EffectSubAirDamageBonus = 429,
        /// <summary>
        /// #1{~1~2 à }#2 Dommages Neutre
        /// </summary>
        EffectAddNeutralDamageBonus = 430,
        /// <summary>
        /// -#1{~1~2 à }#2 Dommages Neutre
        /// </summary>
        EffectSubNeutralDamageBonus = 431,
        /// <summary>
        /// Vole #1{~1~2 à }#2 PA
        /// </summary>
        EffectStealAP440 = 440,
        /// <summary>
        /// Vole #1{~1~2 à }#2 PM
        /// </summary>
        EffectStealMP441 = 441,
        /// <summary>
        /// Positionne la boussole
        /// </summary>
        Effect509 = 509,
        /// <summary>
        /// Pose un prisme
        /// </summary>
        Effect513 = 513,
        /// <summary>
        /// Afficher les percepteurs les plus riches
        /// </summary>
        Effect516 = 516,
        /// <summary>
        /// 
        /// </summary>
        Effect517 = 517,
        /// <summary>
        /// Téléporte au point de sauvegarde
        /// </summary>
        EffectTeleportToSavePoint = 600,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect601 = 601,
        /// <summary>
        /// Enregistre sa position
        /// </summary>
        Effect602 = 602,
        /// <summary>
        /// Apprend le métier #3
        /// </summary>
        LearnJob = 603,
        /// <summary>
        /// Apprend le sort #3
        /// </summary>
        EffectLearnSpell = 604,
        /// <summary>
        /// +#1{~1~2 à }#2 XP
        /// </summary>
        EffectAddXp = 605,
        /// <summary>
        /// +#1{~1~2 à }#2 Sagesse
        /// </summary>
        EffectAddPermanentWisdom = 606,
        /// <summary>
        /// +#1{~1~2 à }#2 Force
        /// </summary>
        EffectAddPermanentStrength = 607,
        /// <summary>
        /// +#1{~1~2 à }#2 Chance
        /// </summary>
        EffectAddPermanentChance = 608,
        /// <summary>
        /// +#1{~1~2 à }#2 Agilité
        /// </summary>
        EffectAddPermanentAgility = 609,
        /// <summary>
        /// +#1{~1~2 à }#2 Vitalité
        /// </summary>
        EffectAddPermanentVitality = 610,
        /// <summary>
        /// +#1{~1~2 à }#2 Intelligence
        /// </summary>
        EffectAddPermanentIntelligence = 611,
        /// <summary>
        /// +#1{~1~2 à }#2 points de caractéristique
        /// </summary>
        Effect612 = 612,
        /// <summary>
        /// +#1{~1~2 à }#2 point(s) de sort
        /// </summary>
        EffectAddSpellPoints = 613,
        /// <summary>
        /// + #1 XP #2
        /// </summary>
        EffectJobXp = 614,
        /// <summary>
        /// Fait oublier le métier de #3
        /// </summary>
        Effect615 = 615,
        /// <summary>
        /// Fait oublier un niveau du sort #3
        /// </summary>
        Effect616 = 616,
        /// <summary>
        /// Consulter #3
        /// </summary>
        Effect620 = 620,
        /// <summary>
        /// Invoque : #3 (grade #1)
        /// </summary>
        Effect621 = 621,
        /// <summary>
        /// Téléporte chez soi
        /// </summary>
        Effect622 = 622,
        /// <summary>
        /// #3 (#2)
        /// </summary>
        EffectSoulStoneSummon = 623,
        /// <summary>
        /// Fait oublier un niveau du sort #3
        /// </summary>
        EffectForgetSpell = 624,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect625 = 625,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect626 = 626,
        /// <summary>
        /// Reproduit la carte d'origine
        /// </summary>
        Effect627 = 627,
        /// <summary>
        /// #3 (#2)
        /// </summary>
        Effect628 = 628,
        /// <summary>
        /// 
        /// </summary>
        Effect630 = 630,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect631 = 631,
        /// <summary>
        /// 
        /// </summary>
        Effect632 = 632,
        /// <summary>
        /// Ajoute #3 points d'honneur
        /// </summary>
        Effect640 = 640,
        /// <summary>
        /// Ajoute #3 points de déshonneur
        /// </summary>
        Effect641 = 641,
        /// <summary>
        /// Retire #3 points d'honneur
        /// </summary>
        Effect642 = 642,
        /// <summary>
        /// Retire #3 points de déshonneur
        /// </summary>
        Effect643 = 643,
        /// <summary>
        /// Ressuscite les alliés présents sur la carte
        /// </summary>
        Effect645 = 645,
        /// <summary>
        /// #1{~1~2 à }#2 (PV rendus)
        /// </summary>
        Effect646 = 646,
        /// <summary>
        /// Libère les âmes des ennemis
        /// </summary>
        Effect647 = 647,
        /// <summary>
        /// Libère une âme ennemie
        /// </summary>
        Effect648 = 648,
        /// <summary>
        /// Faire semblant d'être #3
        /// </summary>
        Effect649 = 649,
        /// <summary>
        /// 
        /// </summary>
        Effect652 = 652,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect654 = 654,
        /// <summary>
        /// Pas d'effet supplémentaire
        /// </summary>
        Effect666 = 666,
        /// <summary>
        /// Incarnation Niveau #5
        /// </summary>
        Effect669 = 669,
        /// <summary>
        /// #1{~1~2 à }#2% de la vie de l'attaquant (dommages Neutre)
        /// </summary>
        Effect670 = 670,
        /// <summary>
        /// #1{~1~2 à }#2% de la vie de l'attaquant (dommages Neutre fixes)
        /// </summary>
        EffectDamagePercentNeutral671 = 671,
        /// <summary>
        /// #1{~1~2 à }#2% de la vie de l'attaquant (dommages Neutre)
        /// </summary>
        EffectPunishmentDamage = 672,
        /// <summary>
        /// Lier son métier : #1
        /// </summary>
        Effect699 = 699,
        /// <summary>
        /// Change l'élément de frappe
        /// </summary>
        Effect700 = 700,
        /// <summary>
        /// Puissance : #1{~1~2 à }#2
        /// </summary>
        Effect701 = 701,
        /// <summary>
        /// +#1{~1~2 à }#2 Point(s) de durabilité
        /// </summary>
        Effect702 = 702,
        /// <summary>
        /// #1% capture d'âme de puissance #3
        /// </summary>
        EffectSoulStone = 705,
        /// <summary>
        /// #1% de proba de capturer une monture
        /// </summary>
        EffectCatchMount = 706,
        /// <summary>
        /// Utilise l'équipement rapide n°#3
        /// </summary>
        Effect707 = 707,
        /// <summary>
        /// Coût supplémentaire
        /// </summary>
        Effect710 = 710,
        /// <summary>
        /// #1 : #3
        /// </summary>
        EffectMonsterSuperRaceKilledCount = 715,
        /// <summary>
        /// #1 : #3
        /// </summary>
        EffectMonsterRaceKilledCount = 716,
        /// <summary>
        /// #1 : #3
        /// </summary>
        EffectMonsterKilledCount = 717,
        /// <summary>
        /// Nombre de victimes : #2
        /// </summary>
        Effect720 = 720,
        /// <summary>
        /// Titre : #3
        /// </summary>
        EffectAddTitle = 724,
        /// <summary>
        /// Renommer la guilde : #4
        /// </summary>
        Effect725 = 725,
        /// <summary>
        /// Ornement : #3
        /// </summary>
        EffectAddOrnament = 726,
        /// <summary>
        /// Téléporte au prisme allié le plus proche
        /// </summary>
        Effect730 = 730,
        /// <summary>
        /// Agresse les personnages d'alliances ennemies automatiquement
        /// </summary>
        Effect731 = 731,
        /// <summary>
        /// Résistance à l'agression automatique par les joueurs ennemis : #1{~1~2 à }#2
        /// </summary>
        Effect732 = 732,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect740 = 740,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect741 = 741,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect742 = 742,
        /// <summary>
        /// Bonus aux chances de capture : #1{~1~2 à }#2%
        /// </summary>
        Effect750 = 750,
        /// <summary>
        /// #1{~1~2 à }#2% Bonus XP monture
        /// </summary>
        Effect751 = 751,
        /// <summary>
        /// #1{~1~2 à }#2 Fuite
        /// </summary>
        EffectAddDodge = 752,
        /// <summary>
        /// #1{~1~2 à }#2 Tacle
        /// </summary>
        EffectAddLock = 753,
        /// <summary>
        /// -#1{~1~2 à }#2 Fuite
        /// </summary>
        EffectSubDodge = 754,
        /// <summary>
        /// -#1{~1~2 à }#2 Tacle
        /// </summary>
        EffectSubLock = 755,
        /// <summary>
        /// Disparaît en se déplaçant
        /// </summary>
        Effect760 = 760,
        /// <summary>
        /// Interception des dommages
        /// </summary>
        EffectDamageIntercept = 765,
        /// <summary>
        /// Confusion horaire : #1{~1~2 à }#2 degrés
        /// </summary>
        Effect770 = 770,
        /// <summary>
        /// Confusion horaire : #1{~1~2 à }#2 Pi/2
        /// </summary>
        Effect771 = 771,
        /// <summary>
        /// Confusion horaire : #1{~1~2 à }#2 Pi/4
        /// </summary>
        Effect772 = 772,
        /// <summary>
        /// Confusion contre horaire : #1{~1~2 à }#2 degrés
        /// </summary>
        Effect773 = 773,
        /// <summary>
        /// Confusion contre horaire : #1{~1~2 à }#2 Pi/2
        /// </summary>
        Effect774 = 774,
        /// <summary>
        /// Confusion contre horaire : #1{~1~2 à }#2 Pi/4
        /// </summary>
        Effect775 = 775,
        /// <summary>
        /// #1{~1~2 à }#2% Érosion
        /// </summary>
        EffectAddErosion = 776,
        /// <summary>
        /// Fixe le point de respawn
        /// </summary>
        Effect778 = 778,
        /// <summary>
        /// Invoque le dernier allié mort avec #1{~1~2 à }#2 % de ses PDV
        /// </summary>
        EffectReviveAndGiveHPToLastDiedAlly = 780,
        /// <summary>
        /// Minimise les effets aléatoires
        /// </summary>
        EffectRandDownModifier = 781,
        /// <summary>
        /// Maximise les effets aléatoires
        /// </summary>
        EffectRandUpModifier = 782,
        /// <summary>
        /// Pousse jusqu'à la case visée
        /// </summary>
        EffectRepelsTo = 783,
        /// <summary>
        /// Retour à la position de départ
        /// </summary>
        EffectReturnToOriginalPos = 784,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect785 = 785,
        /// <summary>
        /// Soigne l'attaquant de #1{~1~2 à }#2% des dommages subis.
        /// </summary>
        EffectGiveHpPercentWhenAttack = 786,
        /// <summary>
        /// #1
        /// </summary>
        Effect787 = 787,
        /// <summary>
        /// Châtiment de #2 sur #3 tour(s)
        /// </summary>
        EffectPunishment = 788,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect789 = 789,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect790 = 790,
        /// <summary>
        /// Prépare #1{~1~2 à }#2 parchemins pour mercenaire [wait]
        /// </summary>
        Effect791 = 791,
        /// <summary>
        /// #1
        /// </summary>
        EffectTriggerBuff = 792,
        /// <summary>
        /// #1
        /// </summary>
        EffectTriggerBuff793 = 793,
        /// <summary>
        /// Arme de chasse
        /// </summary>
        Effect795 = 795,
        /// <summary>
        /// Restaurer le point de respawn
        /// </summary>
        Effect796 = 796,
        /// <summary>
        /// Points de vie : #3
        /// </summary>
        EffectLifePoints = 800,
        /// <summary>
        /// Reçu le : #1
        /// </summary>
        EffectReceiveOn = 805,
        /// <summary>
        /// Corpulence : #1
        /// </summary>
        EffectCorpulence = 806,
        /// <summary>
        /// Dernier repas : #1
        /// </summary>
        EffectLastMeal = 807,
        /// <summary>
        /// A mangé le : #1
        /// </summary>
        EffectLastMealDate = 808,
        /// <summary>
        /// Taille : #3 poces
        /// </summary>
        Effect810 = 810,
        /// <summary>
        /// Combat(s) restant(s) : #3
        /// </summary>
        EffectRemainingFights = 811,
        /// <summary>
        /// Résistance : #2 / #3
        /// </summary>
        EffectBreedingItemDurability = 812,
        /// <summary>
        /// (not found)
        /// </summary>
        EffectMealCount = 813,
        /// <summary>
        /// #1
        /// </summary>
        EffectItemName = 814,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect815 = 815,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect816 = 816,
        /// <summary>
        /// Téléporte
        /// </summary>
        Effect825 = 825,
        /// <summary>
        /// Téléporte
        /// </summary>
        Effect826 = 826,
        /// <summary>
        /// Oublier un sort
        /// </summary>
        Effect831 = 831,
        /// <summary>
        /// Lance un combat contre #2
        /// </summary>
        Effect905 = 905,
        /// <summary>
        /// 
        /// </summary>
        Effect911 = 911,
        /// <summary>
        /// 
        /// </summary>
        Effect916 = 916,
        /// <summary>
        /// 
        /// </summary>
        Effect917 = 917,
        /// <summary>
        /// Augmente la sérénité, diminue l'agressivité
        /// </summary>
        Effect930 = 930,
        /// <summary>
        /// Augmente l'agressivité, diminue la sérénité
        /// </summary>
        Effect931 = 931,
        /// <summary>
        /// Augmente l'endurance
        /// </summary>
        Effect932 = 932,
        /// <summary>
        /// Diminue l'endurance
        /// </summary>
        Effect933 = 933,
        /// <summary>
        /// Augmente l'amour
        /// </summary>
        Effect934 = 934,
        /// <summary>
        /// Diminue l'amour
        /// </summary>
        Effect935 = 935,
        /// <summary>
        /// Accélère la maturité
        /// </summary>
        Effect936 = 936,
        /// <summary>
        /// Ralentit la maturité
        /// </summary>
        Effect937 = 937,
        /// <summary>
        /// Augmente les capacités d'un familier #3
        /// </summary>
        Effect939 = 939,
        /// <summary>
        /// Capacités accrues
        /// </summary>
        EffectIncreasePetStats = 940,
        /// <summary>
        /// Retirer temporairement un objet d'élevage
        /// </summary>
        Effect946 = 946,
        /// <summary>
        /// Récupérer un objet d'enclos
        /// </summary>
        Effect947 = 947,
        /// <summary>
        /// Objet pour enclos
        /// </summary>
        Effect948 = 948,
        /// <summary>
        /// Monter/Descendre d'une monture
        /// </summary>
        Effect949 = 949,
        /// <summary>
        /// État #3
        /// </summary>
        EffectAddState = 950,
        /// <summary>
        /// Enlève l'état #3
        /// </summary>
        EffectDispelState = 951,
        /// <summary>
        /// Désactive l'état '#3'
        /// </summary>
        EffectDisableState = 952,
        /// <summary>
        /// Alignement : #3
        /// </summary>
        EffectAlignment = 960,
        /// <summary>
        /// Grade : #3
        /// </summary>
        EffectGrade = 961,
        /// <summary>
        /// Niveau : #3
        /// </summary>
        EffectLevel = 962,
        /// <summary>
        /// Créé depuis : #3 jour(s)
        /// </summary>
        Effect963 = 963,
        /// <summary>
        /// Nom : #4
        /// </summary>
        Effect964 = 964,
        /// <summary>
        /// (not found)
        /// </summary>
        EffectLivingObjectId = 970,
        /// <summary>
        /// (not found)
        /// </summary>
        EffectLivingObjectMood = 971,
        /// <summary>
        /// (not found)
        /// </summary>
        EffectLivingObjectSkin = 972,
        /// <summary>
        /// (not found)
        /// </summary>
        EffectLivingObjectCategory = 973,
        /// <summary>
        /// (not found)
        /// </summary>
        EffectLivingObjectLevel = 974,
        /// <summary>
        /// Lié au personnage
        /// </summary>
        EffectNonExchangeable981 = 981,
        /// <summary>
        /// Lié au compte
        /// </summary>
        EffectNonExchangeable982 = 982,
        /// <summary>
        /// Échangeable : #1
        /// </summary>
        EffectExchangeable = 983,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect984 = 984,
        /// <summary>
        /// Modifié par : #4
        /// </summary>
        Effect985 = 985,
        /// <summary>
        /// Prépare #1{~1~2 à }#2 parchemins
        /// </summary>
        Effect986 = 986,
        /// <summary>
        /// Appartient à : #4
        /// </summary>
        EffectBelongsTo = 987,
        /// <summary>
        /// Fabriqué par : #4
        /// </summary>
        Effect988 = 988,
        /// <summary>
        /// Recherche : #4
        /// </summary>
        EffectSeek = 989,
        /// <summary>
        /// #4
        /// </summary>
        Effect990 = 990,
        /// <summary>
        /// !! Certificat invalide !!
        /// </summary>
        EffectInvalidCertificate = 994,
        /// <summary>
        /// 
        /// </summary>
        EffectViewMountCharacteristics = 995,
        /// <summary>
        /// Appartient à : #4
        /// </summary>
        Effect996 = 996,
        /// <summary>
        /// Nom : #4
        /// </summary>
        EffectName = 997,
        /// <summary>
        /// Validité : #1j #2h #3m
        /// </summary>
        EffectValidity = 998,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect999 = 999
    }

}

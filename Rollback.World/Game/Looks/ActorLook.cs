using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using Rollback.Common.Logging;
using Rollback.Protocol.Enums;
using Rollback.Protocol.Types;

namespace Rollback.World.Game.Looks
{
    public class ActorLook
    {
        private static readonly Regex _lookParserRegex;
        private static readonly Regex _subLookParserRegex;

        static ActorLook()
        {
            _lookParserRegex = new(@"^{(?<bones>\d+)(?:\|(?<skins>[^|]*)??)?(?:\|(?<colors>[^|]*)??)?(?:\|(?<scales>[^|]*)??)?(?:\|(?<sublooks>.+)??)?}$", RegexOptions.Compiled);
            _subLookParserRegex = new(@"^(?<category>\d+)@(?<index>\d+)=(?<sublook>.+)$", RegexOptions.Compiled);
        }

        public short Bones { get; set; }

        public List<short> Skins { get; set; }

        public Dictionary<byte, Color> Colors { get; private set; }

        public List<short> Scales { get; set; }

        public List<SubActorLook> SubLooks { get; set; }

        public ActorLook(short bones, List<short> skins, Dictionary<byte, Color> colors, List<short> scales, List<SubActorLook> subLooks)
        {
            Bones = bones;
            Skins = skins;
            Colors = colors;
            Scales = scales;
            SubLooks = subLooks;
        }

        public void AddColor(byte index, Color color)
        {
            if (Colors.ContainsKey(index))
                Colors[index] = color;
            else
                Colors[index] = color;
        }

        public void SetSubLook(SubActorLook subLook)
        {
            SubLooks.RemoveAll(x => x.Category == subLook.Category && x.Index == subLook.Index);
            SubLooks.Add(subLook);
        }

        public void RemoveSubLooks(SubEntityBindingPointCategoryEnum category) =>
            SubLooks.RemoveAll(x => x.Category == category);

        public void RemoveAuras() =>
            SubLooks.RemoveAll(x => x.Category == SubEntityBindingPointCategoryEnum.HOOK_POINT_CATEGORY_BASE_FOREGROUND);

        public static ActorLook Parse(string lookString)
        {
            short bones = 1;
            var skins = new List<short>();
            var colors = new Dictionary<byte, Color>();
            var scales = new List<short>();
            var subLooks = new List<SubActorLook>();

            var parsedLook = _lookParserRegex.Match(lookString);
            if (parsedLook.Success && parsedLook.Groups.TryGetValue("bones", out var bonesGroup) && bonesGroup.Value != string.Empty)
            {
                if (short.TryParse(bonesGroup.Value, out bones))
                {
                    // SKINS
                    if (parsedLook.Groups.TryGetValue("skins", out var skinsGroup) && skinsGroup.Value != string.Empty)
                    {
                        foreach (var skinStr in skinsGroup.Value.Split(','))
                            if (short.TryParse(skinStr, out var skin))
                                skins.Add(skin);
                            else
                                Logger.Instance.LogError(default, "Can not parse skins of {0}, wrong data types...", lookString);
                    }

                    //COLORS
                    if (parsedLook.Groups.TryGetValue("colors", out var colorsGroup) && colorsGroup.Value != string.Empty)
                    {
                        foreach (var colorStr in colorsGroup.Value.Split(','))
                        {
                            var splittedColor = colorStr.Split('=');

                            if (splittedColor.Length is 2 && byte.TryParse(splittedColor[0], out var index) &&
                                int.TryParse(splittedColor[1].Replace("#", ""), splittedColor[1][0] is '#' ? System.Globalization.NumberStyles.HexNumber : System.Globalization.NumberStyles.Number, default, out var color))
                                colors.Add(index, Color.FromArgb(color));
                            else
                                Logger.Instance.LogError(default, "Can not parse colors of {0}, wrong data types...", lookString);
                        }
                    }

                    //SCALES
                    if (parsedLook.Groups.TryGetValue("scales", out var scalesGroup) && scalesGroup.Value != string.Empty)
                    {
                        foreach (var scaleStr in scalesGroup.Value.Split(','))
                            if (short.TryParse(scaleStr, out var skin))
                                scales.Add(skin);
                            else
                                Logger.Instance.LogError(default, "Can not parse scales of {0}, wrong data types...", lookString);
                    }

                    //SUBLOOKS
                    if (parsedLook.Groups.TryGetValue("sublooks", out var subLooksGroup) && subLooksGroup.Value != string.Empty)
                    {
                        foreach (var subLookStr in subLooksGroup.Value.Split("},"))
                        {
                            var subLookSplitted = _subLookParserRegex.Match(subLookStr);

                            if (subLookSplitted.Success && subLookSplitted.Groups.TryGetValue("category", out var categoryGroup) && categoryGroup.Value != string.Empty &&
                                subLookSplitted.Groups.TryGetValue("index", out var indexGroup) && indexGroup.Value != string.Empty &&
                                subLookSplitted.Groups.TryGetValue("sublook", out var subLookGroup) && subLookGroup.Value != string.Empty &&
                                sbyte.TryParse(indexGroup.Value, out var index) && sbyte.TryParse(categoryGroup.Value, out var category))
                                subLooks.Add(new(index, (SubEntityBindingPointCategoryEnum)category, Parse(subLookGroup.Value)));
                            else
                                Logger.Instance.LogError(default, "Can not parse sub look {0} of look {1}", subLookStr, lookString);
                        }
                    }
                }
                else
                    Logger.Instance.LogError(default, "Can not parse bones of {0}, wrong data type...", lookString);
            }
            else
                Logger.Instance.LogError(default, "Malformed actor look : {0}...", lookString);

            return new(bones, skins, colors, scales, subLooks);
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            result.Append('{');

            var missingBars = 0;

            result.Append(Bones);

            if (Skins.Count is not 0)
            {
                result.Append(new string('|', missingBars + 1));
                missingBars = 0;
                result.Append(string.Join(",", Skins));
            }
            else
                missingBars++;

            if (Colors.Count is not 0)
            {
                result.Append(new string('|', missingBars + 1));
                missingBars = 0;
                result.Append(string.Join(",", from entry in Colors
                                               select entry.Key + "=" + entry.Value.ToArgb()));
            }
            else
                missingBars++;

            if (Scales.Count is not 0)
            {
                result.Append(new string('|', missingBars + 1));
                missingBars = 0;
                result.Append(string.Join(",", Scales));
            }
            else
                missingBars++;

            if (SubLooks.Count is not 0)
            {
                result.Append(new string('|', missingBars + 1));
                result.Append(string.Join(",", SubLooks.Select(entry => entry)));
            }
            else
                missingBars++;

            result.Append('}');

            return result.ToString();
        }

        public EntityLook GetEntityLook() => new(
            Bones,
            Skins.ToArray(),
            Colors.Select(x => x.Value.ToArgb() is not -1 ? (x.Key & 255) << 24 | x.Value.ToArgb() & 16777215 : 0).ToArray(),
            Scales.ToArray(),
            SubLooks.Select(x => x.SubEntity).ToArray());

        public ActorLook Clone() =>
            new(Bones, Skins.ToList(), Colors.ToDictionary(x => x.Key, x => x.Value), Scales.ToList(), SubLooks.Select(x => new SubActorLook(x.Index, x.Category, x.Look.Clone())).ToList());
    }
}

namespace Rollback.Client.Dlm
{
    public sealed class ColorMultiplicator
    {
        public int Red { get; private set; }

        public int Green { get; private set; }

        public int Blue { get; private set; }

        public bool IsOne { get; private set; }

        public ColorMultiplicator(int red, int green, int blue, bool forceCalculation = false)
        {
            Red = red;
            Green = green;
            Blue = blue;

            if (!forceCalculation && Red + Green + Blue is 0)
                IsOne = true;
        }

        public static int Clamp(int value, int min, int max) =>
            value > max ? max : value < min ? min : value;

        public ColorMultiplicator Multiply(ColorMultiplicator cm)
        {
            ColorMultiplicator result;

            if (cm.IsOne)
                result = cm;
            else if (IsOne)
                result = this;
            else
                result = new ColorMultiplicator(Clamp(Red + cm.Red, sbyte.MinValue, sbyte.MaxValue),
                    Clamp(Green + cm.Green, sbyte.MinValue, sbyte.MaxValue), Clamp(Blue + cm.Blue, sbyte.MinValue, sbyte.MaxValue));

            return result;
        }
    }
}

namespace ClientManager.Core.Validation
{
    public class SaIdNumberValidator
    {
        public SaIdNumberValidator(string idNumber)
        {
            IdNumber = idNumber;
        }

        public string IdNumber { get; }

        private int CalculateControlDigit()
        {
            var sumOfOddPositions = 0;
            var evenPositionsConcatenated = "";

            for (var i = 0; i < 6; i++)
            {
                sumOfOddPositions += int.Parse(IdNumber[2 * i].ToString());
                evenPositionsConcatenated += IdNumber[2 * i + 1].ToString();
            }

            var doubledEvenPositionDigits = int.Parse(evenPositionsConcatenated) * 2;

            var sumOfDoubledEvenDigits = 0;
            while (doubledEvenPositionDigits > 0)
            {
                sumOfDoubledEvenDigits += doubledEvenPositionDigits % 10;
                doubledEvenPositionDigits /= 10;
            }

            var totalSum = sumOfOddPositions + sumOfDoubledEvenDigits;
            var controlDigit = (10 - totalSum % 10) % 10;

            return controlDigit;
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(IdNumber) || IdNumber.Length != 13)
                return false;

            if (!int.TryParse(IdNumber[12].ToString(), out var actualControlDigit))
                return false;

            var expectedControlDigit = CalculateControlDigit();

            return actualControlDigit == expectedControlDigit;
        }
    }
}
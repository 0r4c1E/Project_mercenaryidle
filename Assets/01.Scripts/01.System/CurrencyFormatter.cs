using System;

public class CurrencyFormatter
{
    private static readonly string[] Units = { "", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��", "��" };

    public static string FormatCurrency(double value)
    {
        string formatted = "";
        int unitCount = 0;

        for (int i = Units.Length - 1; i > 0; i--)
        {
            double unitValue = Math.Pow(10000, i);
            if (value >= unitValue)
            {
                long amount = (long)(value / unitValue);
                value -= amount * unitValue;
                formatted += $"{amount}{Units[i]}";
                unitCount++;

                if (unitCount == 2) // �ִ� �� ���� �������� ǥ��
                    break;
            }
        }

        if (value > 0 && unitCount < 2) // ���� ���� �ְ� �� ���� �̸��� ���
        {
            formatted += $"{(long)value}";
        }

        return formatted;
    }
}

// ��� ��:
// string result = CurrencyFormatter.FormatCurrency(160020008000);
// Console.WriteLine(result); // "1600��2000��" ���

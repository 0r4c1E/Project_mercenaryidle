using System;

public class CurrencyFormatter
{
    private static readonly string[] Units = { "", "만", "억", "조", "경", "해", "자", "양", "구", "간", "정", "재", "극", "항", "아", "나", "불", "무" };

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

                if (unitCount == 2) // 최대 두 개의 단위까지 표시
                    break;
            }
        }

        if (value > 0 && unitCount < 2) // 남은 값이 있고 두 단위 미만인 경우
        {
            formatted += $"{(long)value}";
        }

        return formatted;
    }
}

// 사용 예:
// string result = CurrencyFormatter.FormatCurrency(160020008000);
// Console.WriteLine(result); // "1600억2000만" 출력

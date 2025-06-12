using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace курсач3
{
    public class Period:Plan
    {

        public Period(string name, int goalAmount, double startAmount, string frequency, double incomePercent, double investAmount, double paymentAmount, double inflation) : base(name, 
            goalAmount, startAmount, frequency, incomePercent, investAmount, inflation)
        {
            this.PaymentAmount = paymentAmount;
            this.startPaymentAmount = paymentAmount;
        }

        public double PaymentAmount
        {
            get => paymentAmount;
            set
            {
                if (value > 0)
                {
                    paymentAmount = value;
                }
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Размер взносов'! Введите целое положительное число");
                    time = -1;
                    isCorrect = false;
                }
            }
        }

        public void CountTime()
        {
            if (!isCorrect) time = -1;
            else
            {
                double payment = this.PaymentAmount;
                int count = 0;
                int k = 0;
                double perCent = 0;
                double current = this.startAmount + this.StartInvestAmount;
                double currentInvest = this.startInvestAmount;
                double target  = goalAmount;
                double infl = 0;

                if (frequency == "раз в неделю")
                {
                    k = 1;
                    perCent = this.incomePercent / 48;
                    infl = this.inflation / 48;
                }
                else if (frequency == "раз в 2 недели")
                {
                    k = 2;
                    perCent = this.incomePercent / 24;
                    infl = this.inflation / 24;
                }
                else if (frequency == "раз в месяц")
                {
                    k = 4;
                    perCent = this.incomePercent / 12;
                    infl = this.inflation / 12;
                }
                else if (frequency == "раз в 3 месяца")
                {
                    k = 12;
                    perCent = this.incomePercent / 4;
                    infl = this.inflation / 4;
                }
                else if (frequency == "раз в полгода")
                {
                    k = 24;
                    perCent = this.incomePercent / 2;
                    infl = this.inflation / 2;
                }
                else if (frequency == "раз в год")
                {
                    k = 48;
                    perCent = this.incomePercent;
                    infl = this.inflation;
                }

                this.countPayments = 0;
                while (current < target)
                {
                    current += payment + perCent * currentInvest;
                    target += infl * target;
                    currentInvest += perCent * currentInvest;
                    count += k;
                    this.countPayments++;
                }

                this.time = count;
                this.remainingTime = count;
            }
        }

        public string TimeToYears()
        {
            int weeks = this.time;
            int months;
            int years;
            string resultYears;
            string resultMonths;
            string resultWeeks;

            months = weeks / 4;

            if (weeks % 4 == 0) weeks = 0;
            else weeks = weeks % 4;

            if (months < 12) years = 0;
            else
            {
                years = months / 12;
                if (months % 12 == 0) months = 0;
                else months = months % 12;
            }

            if (years == 0) resultYears = "";
            else if (years >= 11 && years <= 14) resultYears = $"{years} лет ";
            else if (years == 1 || years % 10 == 1) resultYears = $"{years} год ";
            else if (years >= 2 && years <= 4 || years % 10 <= 4 && years % 10 >= 2) resultYears = $"{years} года ";
            else resultYears = $"{years} лет ";

            if (months == 0) resultMonths = "";
            else if (months >= 5) resultMonths = $"{months} месяцев ";
            else if (months == 1) resultMonths = $"{months} месяц ";
            else resultMonths = $"{months} месяца ";

            if (weeks == 0) resultWeeks = "";
            else if (weeks == 1) resultWeeks = "1 неделя";
            else resultWeeks = $"{weeks} недели";

            return resultYears + resultMonths + resultWeeks ;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace курсач3
{
    public class Plan
    {
        public string name;
        public int goalAmount;
        public double startAmount;
        public string frequency;
        public double incomePercent;
        public double startInvestAmount;
        public double inflation;
        public double amountWithInflation;
        public double investIncome;
        public int time;
        public double paymentAmount;
        public int countPayments;
        public double startPaymentAmount;
        public bool isCorrect = true;

        public double currentAmount;
        public double currentInvestAmount;
        public int remainingTime;

        public DateTime date;

        public int RemainingTime
        {
            get
            {
                DateTime creatingTime = date; 
                DateTime currentTime = DateTime.Now;
                TimeSpan difference = currentTime - creatingTime;
                int weeks = difference.Days / 7; //прошло недель с момента сохранения плана

                return time - weeks;
            }
        }



        public string Name
        {
            get => name;
            set => name = value;
        }


        public int GoalAmount
        {
            get => goalAmount;
            set
            {
                if (value > 0)
                {
                    goalAmount = value;
                }
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Сумма цели'! Введите положительное целое число");
                    goalAmount = -1;
                    isCorrect = false;
                }
            }
        }
        public double StartAmount
        {
            get=> startAmount;
            set
            {
                if (value >= 0)
                {
                    startAmount = value;
                }
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Начальная сумма'! Введите неотрицательное целое число меньше значения поля 'Сумма цели'");
                    startAmount = -1;
                    isCorrect = false;
                }
            }
        }
        public string Frequency
        {
            get => frequency;
            set
            {
                if (value == "раз в неделю" || value == "раз в 2 недели" || value == "раз в месяц" || value == "раз в 3 месяца" || value == "раз в полгода" || value == "раз в год")
                {
                    frequency = value;
                }
                else
                {
                    MessageBox.Show("Некорректно заполнено поле частоты взносов! Выберите один из предложенных вариантов внесения взносов");
                    frequency = "";
                    isCorrect = false;
                }
            }
        }

        public double IncomePercent
        {
            get => incomePercent;
            set
            {
                if (value >= 0 && value <= 1) incomePercent = value ;
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Ожидаемая доходность от инвестиций'! Введите число от 0 до 100");
                    incomePercent = -1;
                    isCorrect = false;
                }
            }
        }

        public double StartInvestAmount
        {
            get => startInvestAmount;
            set
            {
                if (value >= 0) startInvestAmount = value;
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Сумма на инвестиционном счету'! Введите целое неотрицательное число");
                    startInvestAmount = -1;
                    isCorrect = false;
                }
            }
        }


        public double Inflation
        {
            get => inflation;
            set
            {
                if (value >= 0 && value <= 1) inflation = value;
                else
                {
                    MessageBox.Show("Некорректно заполнено поле 'Текущий уровень инфляции'! Введите число от 0 до 100");
                    inflation = -1;
                    isCorrect = false;
                }
            }
        }

        public Plan(string name, int goalAmount, double startAmount, string frequency, double incomePercent, double investAmount,/*int time,*/ double inflation)
        {
            this.name = name;
            this.GoalAmount = goalAmount;
            this.StartAmount = startAmount;
            this.Frequency = frequency;
            this.IncomePercent = incomePercent;
            this.StartInvestAmount = investAmount;
            this.Inflation = inflation;
            this.currentAmount = startAmount;
            this.currentInvestAmount = investAmount;
            this.date = DateTime.Now;
        }

        public Plan(string name, int goalAmount, double startAmount, string frequency, double incomePercent, double investAmount, double inflation, double amountWithInflation, double investIncome, 
            int time, double paymentAmount, int countPayments, double currentAmount, double currentInvestAmount/*, int timeLeft*/, string date, double startPaymentAmount)
        {
            this.name = name;
            this.GoalAmount = goalAmount;
            this.StartAmount = startAmount;
            this.Frequency = frequency;
            this.IncomePercent = incomePercent;
            this.StartInvestAmount = investAmount;
            this.Inflation = inflation;
            this.amountWithInflation = amountWithInflation;
            this.investIncome = investIncome;
            this.time = time;
            this.paymentAmount = paymentAmount;
            this.countPayments = countPayments;
            this.currentAmount = currentAmount;
            this.currentInvestAmount = currentInvestAmount;
            this.startPaymentAmount = startPaymentAmount;
            //this.remainingTime = timeLeft;
            this.date = DateTime.Parse(date);
        }


        public bool CheckSum()
        {
            bool result = true;
            if (this.GoalAmount <= this.StartAmount + this.StartInvestAmount)
            {
                MessageBox.Show("Значение поля 'Сумма цели' должна превышать сумму значений полей 'Начальная сумма без инвестиций' и 'Сумма на инвестиционном счету'");
                result = false;
                this.isCorrect = false;
            }
            return result;
        }

        public bool CheckTime()
        {
            bool result = true;
            if (this.frequency == "раз в 3 месяца")
            {
                if (this.time / 4 < 3)
                {
                    result = false;
                }
            }
            else if (this.frequency == "раз в полгода")
            {
                if (this.time / 4 < 6)
                {
                    result = false;
                }
            }
            else if (this.frequency == "раз в год")
            {
                if (this.time / 4 < 12)
                {
                    result = false;
                }
            }

            if (!result)
            {
                MessageBox.Show("Срок цели не может быть достигнут ранее первого взноса!");
                this.isCorrect=false;
            }
            return result;
        }

        public bool CheckDifference()
        {
            double startDifference = this.GoalAmount - this.StartAmount - this.StartInvestAmount;
            double goalAmountAfter = 0;
            double incomeAfter = 0;

            if (frequency == "раз в неделю")
            {
                goalAmountAfter = this.GoalAmount + this.GoalAmount * this.Inflation / 12 / 4;
                incomeAfter = this.startInvestAmount * this.incomePercent / 12 / 4 + this.paymentAmount + this.StartAmount + this.StartInvestAmount;
            }
            else if (frequency == "раз в 2 недели")
            {
                goalAmountAfter = this.GoalAmount + this.GoalAmount * this.Inflation / 12 / 2;
                incomeAfter = this.startInvestAmount * this.incomePercent / 12 / 2 + this.paymentAmount + this.StartAmount + this.StartInvestAmount;
            }
            else if (frequency == "раз в месяц")
            {
                goalAmountAfter = this.GoalAmount + this.GoalAmount * this.Inflation / 12 ;
                incomeAfter = this.startInvestAmount * this.incomePercent / 12 + this.paymentAmount + this.StartAmount + this.StartInvestAmount;
            }
            else if (frequency == "раз в 3 месяца")
            {
                goalAmountAfter = this.GoalAmount + this.GoalAmount * this.Inflation / 4;
                incomeAfter = this.startInvestAmount * this.incomePercent / 4 + this.paymentAmount + this.StartAmount + this.StartInvestAmount;
            }
            else if (frequency == "раз в полгода")
            {
                goalAmountAfter = this.GoalAmount + this.GoalAmount * this.Inflation / 2;
                incomeAfter = this.startInvestAmount * this.incomePercent / 2 + this.paymentAmount + this.StartAmount + this.StartInvestAmount;
            }
            else if (frequency == "раз в год")
            {
                goalAmountAfter = this.GoalAmount + this.GoalAmount * this.Inflation;
                incomeAfter = this.startInvestAmount * this.incomePercent + this.paymentAmount + this.StartAmount + this.StartInvestAmount;
            }

            if (goalAmountAfter - incomeAfter > startDifference)
            {
                MessageBox.Show("Взносы слишком малы. Сумма цели растет быстрее, чем увеличиваются накопления!");
                return false;
            }
            else return true;
        }

        public int CountPayments()
        {
            if (frequency == "раз в неделю")
            {
                this.countPayments = this.time;
            }
            else if (frequency == "раз в 2 недели")
            {
                if (this.time % 2 != 0) this.countPayments = this.time / 2 + 1;
                else this.countPayments = this.time / 2;
            }
            else if (frequency == "раз в месяц")
            {
                if (this.time % 4 != 0) this.countPayments = this.time / 4 + 1;
                else this.countPayments = this.time / 4;
            }
            else if (frequency == "раз в 3 месяца")
            {
                if (this.time % 12 != 0) this.countPayments = this.time / 12 + 1;
                else this.countPayments = this.time / 12;
            }
            else if (frequency == "раз в полгода")
            {
                if (this.time % 24 != 0) this.countPayments = this.time / 24 + 1;
                else this.countPayments = this.time / 24;
            }
            else
            {
                if (this.time % 48 != 0) this.countPayments = this.time / 48 + 1;
                else this.countPayments = this.time / 48;
            }
            return -1;
        }

        public int RecountPayments()
        {
            if (frequency == "раз в неделю")
            {
                this.countPayments = this.RemainingTime;
            }
            else if (frequency == "раз в 2 недели")
            {
                this.countPayments = this.RemainingTime / 2;
            }
            else if (frequency == "раз в месяц")
            {
                this.countPayments = this.RemainingTime / 4;
            }
            else if (frequency == "раз в 3 месяца")
            {
                this.countPayments = this.RemainingTime / 12;
            }
            else if (frequency == "раз в полгода")
            {
                this.countPayments = this.RemainingTime / 24;
            }
            else
            {
                this.countPayments = this.RemainingTime / 48;
            }
            return -1;
        }

        public static string TimeToYears(int weeks)
        {
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

            return resultYears + resultMonths + resultWeeks;
        }

        public void NewGoalAmount()
        {
            if (!isCorrect) this.amountWithInflation = -1;
            else
            {
                int weeks = this.time;
                int months;
                int years;

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
                //double years = this.time / 12;
                //double months = this.time % 12;
                double newTargetSum = GoalAmount;
                for (int i = 0; i < years; i++)
                {
                    newTargetSum = newTargetSum + (newTargetSum * inflation);
                }
                newTargetSum = newTargetSum + (newTargetSum * (inflation * ((double)months / 12)));
                newTargetSum = newTargetSum + (newTargetSum * (inflation * ((double)weeks / 48)));
                this.amountWithInflation = newTargetSum;
            }
        }

        public void InvestIncome()
        {
            if (!isCorrect) this.investIncome = -1;
            else
            {
                double startSum = this.StartInvestAmount;
                int weeks = this.time;
                int months;
                int years;

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

                double newInvestAmount = this.StartInvestAmount;

                for (int i = 0; i < years; i++)
                {
                    newInvestAmount = newInvestAmount + (newInvestAmount * this.IncomePercent);
                }
                newInvestAmount = newInvestAmount + (newInvestAmount * this.incomePercent / 12 * months);
                newInvestAmount = newInvestAmount + (newInvestAmount * this.incomePercent / 48 * weeks);
                this.investIncome = newInvestAmount - startSum;
            }
        }
    }
}
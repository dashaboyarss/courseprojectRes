using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ClosedXML.Excel;
//using DocumentFormat.OpenXml.Wordprocessing;

namespace курсач3
{
    public partial class Form2: Form
    {
        private TextBox nameTextBox;
        private TextBox goalTextBox;
        private ComboBox freqComboBox;
        private TextBox inflationTextBox;     
        private TextBox investPercentTextBox;
        private TextBox investAmountTextBox;
        private TextBox currentSumTextBox;
        private RichTextBox goalWithInflationRichTextBox;
        private RichTextBox investIncomeRichTextBox;
        private RichTextBox paymentAmountRichTextBox;
        private RichTextBox timeRichTextBox;
        private RichTextBox countPaymentRichTextBox;
        private TextBox goalTimeTextBox;

        private ProgressBar progressBar;
        private Label labelProgress;

        Plan plan;
        List<Point> points = new List<Point>();


        public static Form2 CurrentForm { get; private set; }

        public Form2(Plan plan)
        {
            Text = "Сохраненный план";
            Size = new System.Drawing.Size(1103, 711);
            BackColor = System.Drawing.Color.LightGray;
            this.AutoScroll = true;

            nameTextBox = new TextBox();
            goalTextBox = new TextBox();
            freqComboBox = new ComboBox();
            inflationTextBox = new TextBox();
            investPercentTextBox = new TextBox();
            investAmountTextBox = new TextBox();
            goalTimeTextBox = new TextBox();
            currentSumTextBox = new TextBox();
            goalWithInflationRichTextBox = new RichTextBox();
            investIncomeRichTextBox = new RichTextBox();
            paymentAmountRichTextBox = new RichTextBox();
            timeRichTextBox = new RichTextBox();
            countPaymentRichTextBox = new RichTextBox();

            CurrentForm = this;


            this.plan = plan;
            
            this.Load += Form2_Load;
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            AddLabels();
            AddTextBoxes();
            AddPaymentBlock(plan);
            CountInvestAmount(plan);
            FillTextBoxes(plan);
            AddDeleteButton(plan);

            points.Add(new Point(plan.name, plan.startAmount + plan.startInvestAmount, 0));
            FillHistory(plan.name);
            MyChart.AddChartToForm(plan, points);
            AddProgressBar(plan);
        }

        private void FillHistory(string name)
        {
            foreach (Point item in Form1.points)
            {
                if (item.name == name)
                {
                    this.points.Add(item);
                }
            }
        }

        private void CountInvestAmount(Plan plan)
        {
            int weeks = plan.time - plan.RemainingTime;
            plan.currentInvestAmount = plan.StartInvestAmount + plan.StartInvestAmount * (plan.IncomePercent / 48 * weeks);
            plan.investIncome = plan.currentInvestAmount - plan.StartInvestAmount;
        }

        private void AddProgressBar(Plan plan)
        {
            AddProgressLabel(plan);

            progressBar = new ProgressBar();

            progressBar.Size = new System.Drawing.Size(400, 40);
            progressBar.Location = new System.Drawing.Point(90, 510);

            if (plan.currentAmount + plan.currentInvestAmount >= plan.amountWithInflation) progressBar.Value = 100;
            else progressBar.Value = (int)((plan.currentAmount + plan.currentInvestAmount) / plan.amountWithInflation * 100);

            this.Controls.Add(progressBar);
        }

        private void AddDeleteButton(Plan plan)
        {
            Button button = new Button();
            button.Text = "УДАЛИТЬ ПЛАН";
            button.Size = new System.Drawing.Size(140, 44);
            button.Location = new System.Drawing.Point(50, 600);

            button.Click += (sender, e) => Button_DeletePlan_Click(sender, e, plan);

            this.Controls.Add(button);
        }

        private void Button_DeletePlan_Click(object sender, EventArgs e, Plan plan)
        {
            this.Close();
            DeleteFromListPlans(plan);
            DeletePlanData(plan);
            DeletePlanHistory(plan);
            DeletePlanHistoryFromList(plan);

            MessageBox.Show($"План '{plan.name}' будет удален после закрытия приложения");
        }

        private void DeletePlanHistoryFromList(Plan plan)
        {
            foreach (Point item in points)
            {
                if (item.name == plan.name)
                {
                    points.Remove(item);
                }
            }
        }

        private void DeletePlanHistory(Plan plan)
        {
            int count = points.Count;
            var historyBook = new XLWorkbook();
            string filePath = "history.xlsx";

            using (historyBook = new XLWorkbook(filePath))
            {
                var ws = historyBook.Worksheet(1);

                for (int i = 0; i < count; i++)
                {
                    int j = 2;
                    while (ws.Cell($"A{count}").Value.ToString() != plan.name)
                    {
                        j++;
                    }
                    j++;

                    while (!ws.Cell($"A{count}").IsEmpty())
                    {
                        ws.Cell($"A{j - 1}").Value = ws.Cell($"A{j}").Value;
                        ws.Cell($"B{j - 1}").Value = ws.Cell($"B{j}").Value;
                        ws.Cell($"C{j - 1}").Value = ws.Cell($"C{j}").Value;
                        j++;
                    }

                    ws.Cell($"A{j - 1}").Clear();
                    ws.Cell($"B{j - 1}").Clear();
                    ws.Cell($"C{j - 1}").Clear();

                }
                historyBook.Save();
            }
            
        }

        private void DeleteFromListPlans(Plan plan)
        {
            Form1.plans.Remove(plan);
        }

        private void DeletePlanData(Plan plan)
        {
            var wbook = new XLWorkbook();
            string filePath = "simple.xlsx";

            using (wbook = new XLWorkbook(filePath))
            {
                int count = 2;
                var ws = wbook.Worksheet(1);

                //поиск нужного плана
                while (ws.Cell($"A{count}").Value.ToString() != plan.name)
                {
                    count++;
                }
                
                count++;
                //удаление данных о плане
                while (!ws.Cell($"A{count}").IsEmpty())
                {
                    ws.Cell($"A{count - 1}").Value = ws.Cell($"A{count}").Value;
                    ws.Cell($"B{count - 1}").Value = ws.Cell($"B{count}").Value;
                    ws.Cell($"C{count - 1}").Value = ws.Cell($"C{count}").Value;
                    ws.Cell($"D{count - 1}").Value = ws.Cell($"D{count}").Value;
                    ws.Cell($"E{count - 1}").Value = ws.Cell($"E{count}").Value;
                    ws.Cell($"F{count - 1}").Value = ws.Cell($"F{count}").Value;
                    ws.Cell($"G{count - 1}").Value = ws.Cell($"G{count}").Value;
                    ws.Cell($"H{count - 1}").Value = ws.Cell($"H{count}").Value;
                    ws.Cell($"I{count - 1}").Value = ws.Cell($"I{count}").Value;
                    ws.Cell($"J{count - 1}").Value = ws.Cell($"J{count}").Value;
                    ws.Cell($"K{count - 1}").Value = ws.Cell($"K{count}").Value;
                    ws.Cell($"L{count - 1}").Value = ws.Cell($"L{count}").Value;
                    ws.Cell($"M{count - 1}").Value = ws.Cell($"M{count}").Value;
                    ws.Cell($"N{count - 1}").Value = ws.Cell($"N{count}").Value;
                    ws.Cell($"O{count - 1}").Value = ws.Cell($"O{count}").Value;
                    ws.Cell($"P{count - 1}").Value = ws.Cell($"P{count}").Value;
                    ws.Cell($"R{count - 1}").Value = ws.Cell($"R{count}").Value;
                    count++;
                }

                ws.Cell($"A{count -1}").Clear();
                ws.Cell($"B{count - 1}").Clear();
                ws.Cell($"C{count - 1}").Clear();
                ws.Cell($"D{count - 1}").Clear();
                ws.Cell($"E{count - 1}").Clear();
                ws.Cell($"F{count - 1}").Clear();
                ws.Cell($"G{count - 1}").Clear();
                ws.Cell($"H{count - 1}").Clear();
                ws.Cell($"I{count - 1}").Clear();
                ws.Cell($"J{count - 1}").Clear();
                ws.Cell($"K{count - 1}").Clear();
                ws.Cell($"L{count - 1}").Clear();
                ws.Cell($"M{count - 1}").Clear();
                ws.Cell($"N{count - 1}").Clear();
                ws.Cell($"O{count}").Clear();
                ws.Cell($"P{count}").Clear();
                ws.Cell($"R{count}").Clear();
                wbook.Save();
            }
        }

        private void AddProgressLabel(Plan plan)
        {
            labelProgress = new Label();
            labelProgress.Location = new System.Drawing.Point(200, 480);
            labelProgress.Text = $"Накоплено: {plan.currentAmount + plan.currentInvestAmount} / {plan.amountWithInflation}";
            labelProgress.Size = new System.Drawing.Size(224, 13);

            this.Controls.Add(labelProgress);
        }

        private void AddLabels()
        {
            Label labelName = new Label();
            Label labelGoal = new Label();
            Label labelCurrentSum = new Label();
            Label labelFreq = new Label();
            Label labelInvestPercent = new Label();
            Label labelInvestAmount = new Label();
            Label labelInflation = new Label();
            Label labelGoalWithInflation = new Label();
            Label labelInvestIncome = new Label();
            Label labelPaymentAmount = new Label();
            Label labelTime = new Label();
            Label labelPaymentCount = new Label();
            Label labelGoalTime = new Label();

            int y = 29;
            labelName.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelName.Text = "Название плана:";
            labelName.Size = new System.Drawing.Size(224, 13);

            labelGoal.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelGoal.Text = "Сумма цели:";
            labelGoal.Size = new System.Drawing.Size(224, 13);

            labelFreq.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelFreq.Text = "Частота взносов:";
            labelFreq.Size = new System.Drawing.Size(224, 13);

            labelInflation.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelInflation.Text = "Текущий уровень инфляции (%):";
            labelInflation.Size = new System.Drawing.Size(224, 13);

            labelInvestPercent.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelInvestPercent.Text = "Ожидаемая доходность от инвестиций (%):";
            labelInvestPercent.Size = new System.Drawing.Size(224, 13);

            labelGoalTime.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelGoalTime.Text = "Плановое время выполнения:";
            labelGoalTime.Size = new System.Drawing.Size(224, 13);

            labelInvestAmount.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelInvestAmount.Text = "Текущая сумма на инвестиционном счету:";
            labelInvestAmount.Size = new System.Drawing.Size(224, 13);

            labelCurrentSum.Location = new System.Drawing.Point(50, y);
            y += 50;
            labelCurrentSum.Text = "Текущие накопления без инвестиций:";
            labelCurrentSum.Size = new System.Drawing.Size(224, 13);

            int y1 = 24;
            labelGoalWithInflation.Text = "Сумма цели с учетом инфляции:";
            labelGoalWithInflation.Font = new System.Drawing.Font(labelGoalWithInflation.Font.FontFamily, 11, FontStyle.Regular);
            labelGoalWithInflation.Location = new System.Drawing.Point(335, y1);
            labelGoalWithInflation.Size = new Size(392, 26);
            y1 += 65;

            labelInvestIncome.Text = "Текущий доход от инвестиций:";
            labelInvestIncome.Font = new System.Drawing.Font(labelInvestIncome.Font.FontFamily, 11, FontStyle.Regular);
            labelInvestIncome.Location = new System.Drawing.Point(335, y1);
            labelInvestIncome.Size = new Size(392, 26);
            y1 += 65;

            labelPaymentAmount.Text = "Плановый размер взносов:";
            labelPaymentAmount.Font = new System.Drawing.Font(labelPaymentAmount.Font.FontFamily, 11, FontStyle.Regular);
            labelPaymentAmount.Location = new System.Drawing.Point(335, y1);
            labelPaymentAmount.Size = new Size(392, 26);
            y1 += 65;

            labelTime.Text = "Оставшееся время цели:";
            labelTime.Font = new System.Drawing.Font(labelTime.Font.FontFamily, 11, FontStyle.Regular);
            labelTime.Location = new System.Drawing.Point(335, y1);
            labelTime.Size = new Size(200, 26);
            y1 += 65;

            labelPaymentCount.Text = "Оставшееся количество взносов:";
            labelPaymentCount.Font = new System.Drawing.Font(labelTime.Font.FontFamily, 11, FontStyle.Regular);
            labelPaymentCount.Location = new System.Drawing.Point(335, y1);
            labelPaymentCount.Size = new Size(300, 26);

            this.Controls.Add(labelName);
            this.Controls.Add(labelGoal);
            this.Controls.Add(labelFreq);
            this.Controls.Add(labelInflation);
            this.Controls.Add(labelInvestPercent);
            this.Controls.Add(labelInvestAmount);
            this.Controls.Add(labelCurrentSum);
            this.Controls.Add(labelGoalWithInflation);
            this.Controls.Add(labelInvestIncome);
            this.Controls.Add(labelPaymentAmount);
            this.Controls.Add(labelTime);
            this.Controls.Add(labelPaymentCount);
            this.Controls.Add(labelGoalTime);
        }

        

        private void AddTextBoxes()
        {

            int y = 45;

            nameTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            nameTextBox.Size = new System.Drawing.Size(169, 20);
            nameTextBox.BorderStyle = BorderStyle.FixedSingle;
            nameTextBox.ReadOnly = true;

            goalTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            goalTextBox.Size = new System.Drawing.Size(131, 20);
            goalTextBox.BorderStyle = BorderStyle.FixedSingle;

            freqComboBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            freqComboBox.Size = new System.Drawing.Size(131, 20);
            string[] choices = {"раз в неделю", "раз в 2 недели", "раз в месяц", "раз в 3 месяца", "раз в полгода", "раз в год"};
            freqComboBox.Items.AddRange(choices);

            inflationTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            inflationTextBox.Size = new System.Drawing.Size(131, 20);
            inflationTextBox.BorderStyle = BorderStyle.FixedSingle;

            investPercentTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            investPercentTextBox.Size = new System.Drawing.Size(131, 20);
            investPercentTextBox.BorderStyle = BorderStyle.FixedSingle;

            goalTimeTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            goalTimeTextBox.Size = new System.Drawing.Size(131, 20);
            goalTimeTextBox.BorderStyle = BorderStyle.FixedSingle;

            investAmountTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            investAmountTextBox.Size = new System.Drawing.Size(131, 20);
            investAmountTextBox.BorderStyle = BorderStyle.FixedSingle;
            investAmountTextBox.ReadOnly = true; 

            currentSumTextBox.Location = new System.Drawing.Point(53, y);
            y += 50;
            currentSumTextBox.Size = new System.Drawing.Size(131, 20);
            currentSumTextBox.BorderStyle = BorderStyle.FixedSingle;
            currentSumTextBox.ReadOnly = true;

            int y1 = 52;
            goalWithInflationRichTextBox.Location = new System.Drawing.Point(338, y1);
            y1 += 65;
            goalWithInflationRichTextBox.Size = new System.Drawing.Size(290, 29);
            goalWithInflationRichTextBox.Font = new Font(goalWithInflationRichTextBox.Font.FontFamily, 11, FontStyle.Regular);
            goalWithInflationRichTextBox.ReadOnly = true;

            investIncomeRichTextBox.Location = new System.Drawing.Point(338, y1);
            y1 += 65;
            investIncomeRichTextBox.Size = new System.Drawing.Size(290, 29);
            investIncomeRichTextBox.Font = new Font(investIncomeRichTextBox.Font.FontFamily, 11, FontStyle.Regular);
            investIncomeRichTextBox.ReadOnly= true;

            paymentAmountRichTextBox.Location = new System.Drawing.Point(338, y1);
            y1 += 65;
            paymentAmountRichTextBox.Size = new System.Drawing.Size(290, 29);
            paymentAmountRichTextBox.Font = new Font(paymentAmountRichTextBox.Font.FontFamily, 11, FontStyle.Regular);

            timeRichTextBox.Location = new System.Drawing.Point(338, y1);
            y1 += 65;
            timeRichTextBox.Size = new System.Drawing.Size(290, 29);
            timeRichTextBox.Font = new Font(timeRichTextBox.Font.FontFamily, 11, FontStyle.Regular);

            countPaymentRichTextBox.Size = new System.Drawing.Size(290, 29);
            countPaymentRichTextBox.Font = new Font(paymentAmountRichTextBox.Font.FontFamily, 11, FontStyle.Regular);
            countPaymentRichTextBox.Location = new System.Drawing.Point(338, y1);

            this.Controls.Add(nameTextBox);
            this.Controls.Add(goalTextBox);
            this.Controls.Add(freqComboBox);
            this.Controls.Add(inflationTextBox);
            this.Controls.Add(investPercentTextBox);
            this.Controls.Add(investAmountTextBox);
            this.Controls.Add(currentSumTextBox);
            this.Controls.Add(goalWithInflationRichTextBox);
            this.Controls.Add(investIncomeRichTextBox);
            this.Controls.Add(paymentAmountRichTextBox);
            this.Controls.Add(timeRichTextBox);
            this.Controls.Add(countPaymentRichTextBox);
            this.Controls.Add(goalTimeTextBox);
        }

        private void FillTextBoxes(Plan plan)
        {
            nameTextBox.Text = plan.name;
            goalTextBox.Text = plan.goalAmount.ToString();
            freqComboBox.Text = plan.frequency;
            inflationTextBox.Text = (plan.inflation*100).ToString();
            investPercentTextBox.Text = (plan.incomePercent*100).ToString();
            goalTimeTextBox.Text = Plan.TimeToYears(plan.time);
            investAmountTextBox.Text = plan.currentInvestAmount.ToString();
            currentSumTextBox.Text= plan.currentAmount.ToString();
            goalWithInflationRichTextBox.Text = plan.amountWithInflation.ToString();
            investIncomeRichTextBox.Text = plan.investIncome.ToString();

            //изменено при тестировании
            PaymentAmount(plan);
            plan.RecountPayments();
            paymentAmountRichTextBox.Text = plan.paymentAmount.ToString();

            timeRichTextBox.Text = Plan.TimeToYears(plan.RemainingTime);
            countPaymentRichTextBox.Text = plan.countPayments.ToString();
        }

       


        private void AddPaymentBlock(Plan plan)
        {
            this.Paint += new PaintEventHandler(DrawRectangle);

            Label label = new Label();
            label.Text = "Введите сумму взноса:";
            label.Location = new System.Drawing.Point(760, 82);
            label.Size = new System.Drawing.Size(227, 26);
            label.Font = new Font(label.Font.FontFamily, 10, FontStyle.Regular);
            label.BackColor = System.Drawing.Color.Transparent;

            this.Controls.Add(label);

            TextBox textBox = new TextBox();
            textBox.Location = new System.Drawing.Point(760, 112);
            textBox.Size = new System.Drawing.Size(219, 20);
            textBox.BorderStyle = BorderStyle.FixedSingle;

            this.Controls.Add(textBox);

            Button button = new Button();
            button.Text = "ВНЕСТИ";
            button.Size = new System.Drawing.Size(90, 44);
            button.Location = new System.Drawing.Point(760 + 55, 142);

            button.Click += (sender, e) => Button_AddData_Click(sender, e, plan, textBox);

            this.Controls.Add(button);
        }

        private void Button_AddData_Click(object sender, EventArgs e, Plan plan, TextBox amount)
        {
            int sum = ParseInt(amount.Text);
            if (sum == 0) MessageBox.Show("Сумма взноса должна быть отличной от нуля!");
            else
            {
                if (sum != -1 && plan.currentAmount + plan.currentInvestAmount < plan.amountWithInflation)
                {
                    double currentSum = plan.currentAmount + sum;

                    plan.currentAmount = currentSum;

                    PaymentAmount(plan);
                    plan.CountPayments();

                    SaveChanges(plan);
                    SavePoint(plan);
                    points.Add(new Point(plan.name, plan.currentAmount + plan.currentInvestAmount, plan.time - plan.RemainingTime));
                    FillTextBoxes(plan);
                    Point newPoint = new Point(plan);
                    MyChart.AddPoint(newPoint);
                    if (plan.currentAmount + plan.currentInvestAmount >= plan.amountWithInflation) progressBar.Value = 100;
                    else progressBar.Value = (int)((plan.currentAmount + plan.currentInvestAmount) / plan.amountWithInflation * 100);
                    labelProgress.Text = $"Накоплено: {plan.currentAmount + plan.currentInvestAmount} / {plan.amountWithInflation}";
                    amount.Clear();


                    if (plan.currentAmount + plan.currentInvestAmount >= plan.amountWithInflation)
                    {
                        MessageBox.Show("Цель достигнута!");
                    }
                }
                else if (plan.currentAmount + plan.currentInvestAmount >= plan.amountWithInflation)
                {
                    MessageBox.Show("Цель уже была достигнута!");
                }
            }
               
        }

        public void PaymentAmount(Plan plan)
        {
            double payment = 0;
            double paymentSum = plan.amountWithInflation - plan.currentInvestAmount - plan.currentAmount;

            if (plan.Frequency == "раз в неделю")
            {
                payment = paymentSum / plan.RemainingTime;
            }
            else if (plan.Frequency == "раз в 2 недели")
            {
                payment = paymentSum / plan.RemainingTime * 2;
            }
            else if (plan.Frequency == "раз в месяц")
            {
                payment = paymentSum / plan.RemainingTime * 4;
            }
            else if (plan.Frequency == "раз в 3 месяца")
            {
                payment = paymentSum / plan.RemainingTime * 12;
            }
            else if (plan.Frequency == "раз в полгода")
            {
                payment = paymentSum / plan.RemainingTime * 24;
            }
            else if (plan.Frequency == "раз в год")
            {
                payment = paymentSum / plan.RemainingTime * 48;
            }
            plan.paymentAmount = payment;

        }

        private void SavePoint(Plan plan)
        {
            var historyBook = new XLWorkbook();
            string filePath = "history.xlsx";

            if (!File.Exists(filePath))
            {
                var ws = historyBook.Worksheets.Add("Sheet1");
                ws.Cell($"A1").Value = "Имя плана";
                ws.Cell($"B1").Value = "Сумма";
                ws.Cell($"C1").Value = "Прошло недель";
                historyBook.SaveAs("history.xlsx");
            }
            using (historyBook = new XLWorkbook(filePath))
            {
                var ws = historyBook.Worksheet(1);
                int count = 2;

                while (!ws.Cell($"A{count}").IsEmpty())
                {
                    count++;
                }
                ws.Cell($"A{count}").Value = plan.name;
                ws.Cell($"B{count}").Value = (plan.currentAmount + plan.currentInvestAmount).ToString();
                ws.Cell($"C{count}").Value = (plan.time - plan.RemainingTime).ToString();

                historyBook.Save();
            }

        }

        private void SaveChanges(Plan plan)
        {
            var wbook = new XLWorkbook();
            string filePath = "simple.xlsx";

            using (wbook = new XLWorkbook(filePath))
            {
                int count = 1;
                var ws = wbook.Worksheet(1);

                while (!ws.Cell($"A{count}").IsEmpty())
                {
                    if (ws.Cell($"A{count}").Value.ToString() == plan.name)
                    {
                        ws.Cell($"K{count}").Value = plan.paymentAmount;
                        ws.Cell($"M{count}").Value = plan.currentAmount;
                        ws.Cell($"N{count}").Value = plan.currentInvestAmount;
                        ws.Cell($"J{count}").Value = plan.investIncome;
                        ws.Cell($"L{count}").Value = plan.countPayments;
                        break;
                    }
                    count++;
                }
                wbook.Save();
            }
        }
      
        private int ParseInt(string amount)
        {
            int correctAmount;
            bool isConverted = Int32.TryParse(amount, out correctAmount);
            if (isConverted)
            {
                if (correctAmount < 0)
                {
                    isConverted = false;
                    MessageBox.Show("Сумма взноса должна быть положительным числом");
                    return -1;
                }
                else return correctAmount;
            }
            MessageBox.Show("Неверно введено значение!\nВведите целое число");
            return -1;
        }

        private void DrawRectangle(object sender, PaintEventArgs e)
            {
            Graphics g = e.Graphics;

            GraphicsPath path = new GraphicsPath();
            int cornerRadius = 40;
            int x = 750;
            int y = 52;
            int width = 250;
            int height = 150;

            Color intermediateColor = InterpolateColors(Color.LightGray, Color.DimGray, 0.5f);
            path.AddArc(x, y, cornerRadius, cornerRadius, 180, 90); 
            path.AddArc(x + width - cornerRadius, y, cornerRadius, cornerRadius, 270, 90); 
            path.AddArc(x + width - cornerRadius, y + height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(x, y + height - cornerRadius, cornerRadius, cornerRadius, 90, 90); 

            path.CloseFigure();

            using (SolidBrush brush = new SolidBrush(intermediateColor))
            {
                g.FillPath(brush, path); 
            }

            
            using (Pen pen = new Pen(Color.Black, 3)) 
            {
                g.DrawPath(pen, path); 
            }
            }

        private Color InterpolateColors(Color color1, Color color2, float ratio)
        {
            ratio = Math.Max(0, Math.Min(1, ratio));

            int r = (int)(color1.R + (color2.R - color1.R) * ratio);
            int g = (int)(color1.G + (color2.G - color1.G) * ratio);
            int b = (int)(color1.B + (color2.B - color1.B) * ratio);

            return Color.FromArgb(r, g, b);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form2
            // 
            this.ClientSize = new System.Drawing.Size(278, 244);
            this.Name = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load_1);
            this.ResumeLayout(false);

        }

        private void Form2_Load_1(object sender, EventArgs e)
        {

        }
    }
}

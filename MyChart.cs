
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using Chart = System.Windows.Forms.DataVisualization.Charting.Chart;
using System.Drawing;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace курсач3
{
    public class MyChart
    {
        static Chart chart;
        public static void AddChartToForm(Plan plan, List<Point> points)
        {
            // Создаем новый график
            chart = new Chart();

            // Настройка размеров и положения
            chart.Width = 510;
            chart.Height = 350;
            chart.Location = new System.Drawing.Point(666, 280);
            chart.Name = "chartOfPlan";

            // Добавляем область для рисования
            ChartArea chartArea = new ChartArea("MainArea");
            chart.ChartAreas.Add(chartArea);

            // Настраиваем оси
            chartArea.AxisX.Title = "Количество месяцев";
            chartArea.AxisY.Title = "Накопленная сумма";
            chartArea.AxisX.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.Enabled = true;

            // Добавляем заголовок
            Title title = new Title("График накоплений", Docking.Top, new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold), System.Drawing.Color.Black);
            chart.Titles.Add(title);

            Series series1 = new Series("Плановые накопления")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = System.Drawing.Color.Red,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 8
            };
            chart.Series.Add(series1);

            
            Series series2 = new Series("Реальные накопления")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = System.Drawing.Color.Aquamarine,
                MarkerStyle = MarkerStyle.Square,
                MarkerSize = 8
            };
            chart.Series.Add(series2);

            Series series3 = new Series("Целевая сумма")
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = System.Drawing.Color.YellowGreen,
                MarkerStyle = MarkerStyle.Square,
                MarkerSize = 8
            };
            chart.Series.Add(series3);

            ChartArea legendArea = new ChartArea("LegendArea");
            legendArea.Position.Auto = false;                
            legendArea.Position.X = 80;                      
            legendArea.Position.Y = 0;                     
            legendArea.Position.Width = 30;                
            legendArea.Position.Height = 14;                 
            legendArea.BackColor = System.Drawing.Color.Transparent;        
            legendArea.InnerPlotPosition.Auto = true;          
            chart.ChartAreas.Add(legendArea);                   

            Legend legend = new Legend("Legend");
            legend.DockedToChartArea = "LegendArea"; 
            legend.Docking = Docking.Right;                   
            legend.Alignment = StringAlignment.Far;         
            legend.LegendStyle = LegendStyle.Column;          
            legend.BackColor = System.Drawing.Color.Transparent; 
            legend.Font = new System.Drawing.Font("Arial", 7f, FontStyle.Regular);
            chart.Legends.Add(legend);

            //Строим график
            FillChart(plan, points);

            // Добавляем график на форму
            Form2 form = Form2.CurrentForm;

            form.Controls.Add(chart);
        }

        public static void FillChart(Plan plan, List<Point> points)
        {
            double payment = plan.startPaymentAmount;
            double step = Form1.Step(plan.frequency);
            double x = 0;
            double y = plan.startAmount + plan.startInvestAmount;
            chart.Series[0].Points.Clear();
            //chart.Series[1].Points.Clear();
            chart.Series[2].Points.Clear();

            chart.Series[1].Enabled = true;
            chart.Series[1].IsVisibleInLegend = true;

            //плановые накопления
            double invests = plan.startInvestAmount;
            double percent = plan.incomePercent;
            while (x <= plan.time / 4+ ((double)(plan.time % 4) / 4))
            {
                chart.Series[0].Points.AddXY(x, y);
                x+= step;
                y += payment;
            }

            //рост целевой суммы
            //step = 1;
            x = 0;
            y = plan.goalAmount;
            double endGraph = plan.amountWithInflation;

            
            while (x <= plan.time / 4 + ((double)(plan.time % 4) / 4))
            {
                chart.Series[2].Points.AddXY(x, y);
                x+= step;
                y += y * (plan.inflation / 12 );
                
            }

            //настроить таймер для обновления планов 
            //сделать линию накоплений на графике
            foreach (Point item in points)
            {
                x = item.weeks / 4;
                y = item.sum;
                chart.Series[1].Points.AddXY(x, y);
            }
            chart.Invalidate();
        }

        public static void AddPoint(Point point)
        {
            double x = point.weeks / 4;
            double y = point.sum;

            chart.Series[1].Points.AddXY(x, y);
        }

    }
}

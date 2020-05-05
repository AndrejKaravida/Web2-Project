import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { Label } from 'ng2-charts';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';

@Component({
  selector: 'app-company-incomes-dialog',
  templateUrl: './company-incomes-dialog.component.html',
  styleUrls: ['./company-incomes-dialog.component.css']
})
export class CompanyIncomesDialogComponent implements OnInit {

  public barChartOptions: ChartOptions = {
    responsive: true,
    layout: {padding: 20},
    legend: {position: 'bottom'},
    scales: { xAxes: [{}], yAxes: [{ticks: {beginAtZero: true, suggestedMax: 1000}}] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };
  public barChartLabels: Label[] = ['Today', 'This week', 'This month'];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public barChartPlugins = [pluginDataLabels];

  public barChartData: ChartDataSets[] = [
    { data: [65, 59, 80], label: 'Company incomes', minBarLength: 2, barThickness: 100}
  ];

  constructor(
    public dialogRef: MatDialogRef<CompanyIncomesDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

  ngOnInit() {
    this.barChartData[0].data = this.data.values;
    this.barChartLabels = this.data.dates;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}

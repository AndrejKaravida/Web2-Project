import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import { Label } from 'ng2-charts';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';

@Component({
  selector: 'app-company-reservations-dialog',
  templateUrl: './company-reservations-dialog.component.html',
  styleUrls: ['./company-reservations-dialog.component.css']
})
export class CompanyReservationsDialogComponent implements OnInit {
  
  public barChartOptions: ChartOptions = {
    responsive: true,
    layout: {padding: 20},
    legend: {position: 'bottom'},
    scales: { xAxes: [{}], yAxes: [{ticks: {stepSize: 1, beginAtZero: true, suggestedMax: 5}}] },
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
    { data: [2, 7, 18], label: 'Company vehicle reservations', minBarLength: 2, barThickness: 100}
  ];

  constructor(
    public dialogRef: MatDialogRef<CompanyReservationsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {}

  ngOnInit() {
    const newData = [this.data.res.reservationsToday,
       this.data.res.reservationsThisWeek,
       this.data.res.reservationsThisMonth];

    this.barChartData[0].data = newData;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

}

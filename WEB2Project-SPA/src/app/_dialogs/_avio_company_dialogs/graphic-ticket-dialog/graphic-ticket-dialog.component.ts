import { Component, OnInit, Inject } from '@angular/core';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';

import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { IncomesAviocompanyDialogComponent } from '../incomes-aviocompany-dialog/incomes-aviocompany-dialog.component';

@Component({
  selector: 'app-graphic-ticket-dialog',
  templateUrl: './graphic-ticket-dialog.component.html',
  styleUrls: ['./graphic-ticket-dialog.component.css']
})
export class GraphicTicketDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<GraphicTicketDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any, public dialog: MatDialog) { }
  // tslint:disable-next-line: member-ordering
  public barChartOptions: ChartOptions = {
    responsive: true,
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{}] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };
  // tslint:disable-next-line: member-ordering
  public barChartLabels: Label[] = ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];
  // tslint:disable-next-line: member-ordering
  public barChartType: ChartType = 'bar';
  // tslint:disable-next-line: member-ordering
  public barChartLegend = true;
  // tslint:disable-next-line: member-ordering
  public barChartPlugins = [pluginDataLabels];

  public barChartData: ChartDataSets[] = [
    { data: [65, 59, 80, 81, 56, 55, 40], label: 'Today' },
    { data: [28, 48, 40, 19, 86, 27, 90], label: 'This week' },
    { data: [44, 13, 50, 59, 66, 21, 50], label: 'This month' }
  ];

  ngOnInit() {

  }

  
  public chartClicked({ event, active }: { event: MouseEvent, active: {}[] }): void {
    console.log(event, active);
  }

  public chartHovered({ event, active }: { event: MouseEvent, active: {}[] }): void {
    console.log(event, active);
  }

  public randomize(): void {
    // Only Change 3 values
    const data = [
      Math.round(Math.random() * 100),
      59,
      80,
      (Math.random() * 100),
      56,
      (Math.random() * 100),
      40];
    this.barChartData[0].data = data;
  }
  
  Incomes(){
    const dialogRef = this.dialog.open(IncomesAviocompanyDialogComponent, {
      width: '550px',
      height: '400px',
      data: {}
    });
  }
  Close(){
    this.dialogRef.close();
  }
}

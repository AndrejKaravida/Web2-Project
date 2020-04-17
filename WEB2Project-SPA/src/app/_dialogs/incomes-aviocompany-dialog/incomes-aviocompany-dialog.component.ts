import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ChartOptions, ChartType, ChartDataSets } from 'chart.js';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Label } from 'ng2-charts';

@Component({
  selector: 'app-incomes-aviocompany-dialog',
  templateUrl: './incomes-aviocompany-dialog.component.html',
  styleUrls: ['./incomes-aviocompany-dialog.component.css']
})
export class IncomesAviocompanyDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<IncomesAviocompanyDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any) { }

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
                { data: [65, 59, 80, 81, 56, 55, 40], label: 'Incomes' }
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
              
              Close(){
                this.dialogRef.close();
              }

}

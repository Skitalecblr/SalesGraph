import { DatePipe } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { SaleService } from '../../services/sales.service';
declare let Plotly: any;

@Component({
  selector: 'app-sales-graph',
  templateUrl: './sales-graph.component.html',
  styleUrls: ['sales-graph.component.css']
})
export class SalesGraphComponent {

  readonly maxItems: number = 1000;
  readonly dateFormat: string = 'yyyy-MM-dd';

  currentPeriod: number = 4;
  startDate!: string;
  endDate!: string;
  loading: boolean = false;

  constructor(private saleService: SaleService, datepipe: DatePipe) {
    this.startDate = datepipe.transform(new Date(new Date().getFullYear() - 1, 1, 1), this.dateFormat) || '';
    this.endDate = datepipe.transform(new Date(), this.dateFormat) || '';
  }

  ngOnInit(): void {
    this.buildGraph();
  }

  buildGraph(): void {
    this.loading = true;
    var itemsCount = this.getItemsCount(this.startDate, this.endDate, this.currentPeriod);

    if (itemsCount > this.maxItems) {
      alert("You selected a very long time period or detalization. Please use a shorter period or detalization");
      this.loading = false;
      return;
    }
    this.saleService.getSalesStats(this.currentPeriod, this.startDate, this.endDate).subscribe(
      stats => {
        const data = [
          {
            x: stats.x,
            y: stats.y,
            type: 'bar',
            name: 'Sales Count',
            marker: {
              color: stats.y,
              colorscale: 'Viridis',
              colorbar: {
                title: 'Sales',
                ticksuffix: ''
              }
            }
          },
          {
            x: stats.x,
            y: stats.z,
            type: 'scatter',
            mode: 'lines',
            name: 'Total'
          }
        ];

        const layout = {
          title: 'Sales Statistics',
          xaxis: {
            title: this.getAxisLabel(this.currentPeriod),
            type: 'category'
          },
          yaxis: {
            title: 'Sales'
          },
          legend: {
            orientation: 'h',
            yanchor: 'bottom',
            xanchor: 'center',
            y: 1.02
          }
        };

        Plotly.newPlot('barChart', data, layout);
        this.loading = false;
      }
    );

  }

  private getItemsCount(startDate: string, endDate: string, timePeriod: number): number {
    const start = new Date(startDate);
    const end = new Date(endDate);
    const period = Number(timePeriod);

    let count = 0;

    switch (period) {
      case 1: // Day
        count = Math.abs((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24));
        break;
      case 2: // Week
        count = Math.abs((end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24 * 7));
        break;
      case 3: // Month
        count = (end.getMonth() - start.getMonth()) + (12 * (end.getFullYear() - start.getFullYear()));
        break;
      case 4: // Quarter
        const startQuarter = Math.floor(start.getMonth() / 3);
        const endQuarter = Math.floor(end.getMonth() / 3);
        count = (endQuarter - startQuarter) + (4 * (end.getFullYear() - start.getFullYear()));
        break;
      default:
        throw new Error("Invalid time period.");
    }

    return Math.ceil(count);
  }

  private getAxisLabel(timePeriod: number): string {
    const period = Number(timePeriod);

    switch (period) {
      case 1: // Day
        return 'Day In Period';
      case 2: // Week
        return 'Week In Period';
      case 3: // Month
        return 'Month In Period';
      case 4: // Quarter
        return 'Quarter number / Year';
      default:
        throw new Error("Invalid time period.");
    }
  }
}

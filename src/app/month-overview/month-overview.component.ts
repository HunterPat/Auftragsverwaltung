import { Component, computed, inject, signal } from '@angular/core';
import { DataService } from '../data.service';
import { rxResource } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { CanvasJSAngularChartsModule } from '@canvasjs/angular-charts';
import { CommonModule, JsonPipe } from '@angular/common';
@Component({
  selector: 'app-month-overview',
  imports: [FormsModule, CanvasJSAngularChartsModule, CommonModule],
  templateUrl: './month-overview.component.html',
  styleUrl: './month-overview.component.scss'
})
export class MonthOverviewComponent {

  dataService = inject(DataService);
  selectedYear = signal<number>(new Date().getFullYear());
  months = rxResource({
    request: this.selectedYear,
    loader: () => this.dataService.orderService.ordersMonthInfosYearGet(this.selectedYear().toString()),

  })
  GetLast10Years(): number[] {
    let date = new Date();
    let years: number[] = [];
    for (let index = 0; index < 10; index++) {
      years.push(date.getFullYear() - index);
    }

    return years;
  }
  AvgSum(): string {
    return (parseInt(this.GesamtSum()) / this.months.value()?.where(x => x.bezahlt !== 0).count()!).toFixed(2);
  }
  GesamtSum(): string {
    let sum = 0;
    this.months.value()?.forEach(x => { sum += x.bezahlt ?? 0; });
    return sum.toFixed(2);
  }
  //Diagram
  chartOptions = computed(() => ({
    title: {
      text: "Monatsübersicht"
    },
    animationEnabled: true,
    data: [{
      type: "column",
      dataPoints: this.months.value()?.select(x => ({ label: x.monthName, y: x.bezahlt, color: GetColorOfDiagramColumn(x.bezahlt!)}))
    }]
  }))

}
function GetColorOfDiagramColumn(bezahlt: number) {
  return bezahlt <= 2000 ? "red":
         bezahlt > 2000 && bezahlt <= 4000 ? "orange":
         bezahlt > 4000 ? "green":
         "violet"

}

